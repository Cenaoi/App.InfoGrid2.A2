/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

Mini2.define('Mini2.flow.Line', {



    isLine : true,

    width: 100,

    height: 40,

    x: 0,

    y: 0,

    //线段的样式
    style_name :'',

    size: 1,

    points: false,

    lineObj: null,

    startAnchor: null,

    endAnchor: "04",
    
    //开始的节点id
    fromNodeId: '',
    //结束的节点id 
    toNodeId: '',


    start_point: null,
    
    end_point: null,

    isHover : false,
    _isMouseOver: false,
    
    _isMouseDown: false,
    



    /**
    * 样式列表
    */
    styleList: null, //[]

    //样式
    className: '',



    //处理样式文件
    proClassName: function () {

        var me = this,
            styleList,
            classStr,
            classStrList,
            i,
            css,
            className = me.className;

        me.styleList = styleList = [];

        classStrList = className.split(' ');

        for (i = 0; i < classStrList.length; i++) {

            classStr = classStrList[i];

            css = Mini2.flow.ac3.css.ItemCssSet[classStr];

            if (css) {
                styleList.push(css);
            }

        }

    },


    css: function (className, pseudoName) {
        "use strict";
        var me = this,
            i,
            style,
            styleList = me.styleList,
            len = styleList.length,
            i = len,
            pseudo,
            cssValue;

        if (pseudoName) {
            while (--i >= 0) {
                style = styleList[i];

                if (style.pseudo) {
                    pseudo = style.pseudo[pseudoName];

                    if (pseudo) {
                        cssValue = pseudo[className];

                        if (cssValue) {
                            break;
                        }
                    }
                }
            }
        }


        if (undefined == cssValue) {
            i = len;
            while (--i >= 0) {

                style = styleList[i];

                if (style.common) {
                    cssValue = style.common[className];

                    if (cssValue) {
                        break;
                    }
                }
            }
        }

        return cssValue;
    },






    setStartPoint:function(sp){
        var me = this,
            old_sp = me.start_point;
        
        me.start_point = sp;

        me.resetDraw();

        return me;
    },

    setEndPoint:function(ep){
        var me = this;

        me.end_point = ep;

        me.resetDraw();

        return me;
    },


    //判断线段两头有连接
    hasLinked: function(){

        var me = this;

        return (me.start_point != null && me.end_point != null);
    },


    getX:function(){
        var me = this;

        return me.x;
    },

    getY:function(){
        var me = this;

        return me.y;
    },
    
    //eventSet :[],
    getPoints : function(){
        var me = this;

        return me.points;
    },

    bind: function (eventName, fun, data, owner) {
        "use strict";
        var me = this,
            evtSet = me.eventSet = me.eventSet || {},
            evts = evtSet[eventName] = evtSet[eventName] || []

        evts.push({
            fun: fun,
            owner: owner,
            data: data
        });

    },

    on: function (eventName, data) {
        "use strict";
        if (!this.eventSet) {
            return;
        }


        var me = this,
            i,
            evt,
            fun,
            evtData,
            evtSet = me.eventSet,
            evts = evtSet[eventName] || []

        for (i = 0; i < evts.length; i++) {

            evt = evts[i];

            fun = evt.fun;

            if (evt.data) {
                evtData = Mini2.clone(evt.data);
            }
            else {
                evtData = {};
            }


            evtData = Mini2.applyIf(evtData, data)

            fun.call(evt.owner || me, me,evtData);

        }

    },

    
    setPage:function(page){
        var me = this;

        me.page = page;
        return me;
    },


    onMouseUp: function () {
        var me = this;

        //me.on('mouseup');
    },


    onPreEvent: function (eventName, data) {
        var me = this;

        //console.log(this.text);

        if ('mouseup' == eventName) {
            me.onMouseUp();
        }
        else {
            me.on(eventName, data);
        }
    },

    // 重新绘制
    resetDraw: function () {
        var me = this,
            g,
            lineObj = me.lineObj;

        g = lineObj.graphics;


        if (me._isMouseOver) {

            g.clear();

            g.setStrokeStyle(8, "round");
            g.beginStroke(createjs.Graphics.getRGB(56, 181, 73, 0.5));

            me.drawLine(g);


            g.setStrokeStyle(2, "round");

            g.beginStroke("#448CCB");

            me.drawLine(g);
        }
        else {
            g.clear();

            g.setStrokeStyle(8, "round");
            g.beginStroke(createjs.Graphics.getRGB(255, 255, 255, 0.5));

            me.drawLine(g);


            g.setStrokeStyle(2, "round");
            g.beginStroke("#448CCB");


            if (!me.hasLinked()) {
                g.setStrokeDash([4, 4]);
            }

            me.drawLine(g);
        }
        

        return me;
    },

    drawLine:function(g){
        var me = this,
            i,
            p0,p1,

            ps = me.points,
            len = ps.length;

        p0 = ps[0];

        g.moveTo(p0.x, p0.y);

        for (i = 1; i < len - 1; i++) {
            p1 = ps[i];
            g.lineTo(p1.x, p1.y);

            p0 = p1;
        }

        //最后一个线段进行特殊处理
        p1 = ps[len - 1];
        var curLen = me.getLenForLine(p0, p1);

        var curAngle = me.getAnagle(p0, p1) ;

        var lastP = me.getCenterRadiusPoint(p0, 360 - curAngle, curLen - 10);

        g.lineTo(lastP.x, lastP.y);

    },


    draw: function (graphics) {
        "use strict";
        var me = this,
            el = me.el,
            lineObj = me.lineObj,
            g = graphics || lineObj.graphics;

        
        g.setStrokeStyle(me.size, 'round');
        g.beginStroke('#5b9bd5');

        me.drawLine(g);

        return me;
    },




    initPoints: function () {
        "use strict";
        var me = this,
            ps = me.points || [],
            len = ps.length,
            p,i;

        for (i = 0; i < len; i++) {
            
            p = ps[i];

            if (Mini2.isArray(p)) {
                ps[i] = {
                    x: p[0],
                    y: p[1]
                };
            }
        }
    },

    //设置末端
    preEndAnchor: function () {
        "use strict";
        var me = this,
            ps = me.points || [],
            len ,
            angle, anchor,
            p1,p2;

        len = ps.length;

        if (len == 0) {
            return null;
        }

        p1 = ps[len - 2];
        p2 = ps[len - 1];

        angle = me.getAnagle(p1,p2);


        anchor = Mini2.create('Mini2.flow.LineAnchor', {
            x: p2.x,
            y: p2.y,
            rotation: angle + 90
        });

        anchor.render();

        return anchor;
    },


    initComponent:function(){
        "use strict";
        var me = this;

        me.initComponentBase();


    },

    initComponentBase: function () {
        "use strict";
        var me = this,
            el,
            points = me.points,
            mainObj,
            endAnchor,
            startAnchor,
            lineObj;

        me.eventSet = {};

        me.points = me.points || [];

        me.initPoints();



        me.el = me.mainObj = mainObj = new createjs.Container();

        mainObj.x = me.x;
        mainObj.y = me.y;

        me.endAnchor = endAnchor = me.preEndAnchor();

        me.lineObj = lineObj = new createjs.Shape();



        mainObj.addChild(lineObj);

        if (endAnchor != null) {
            mainObj.addChild(endAnchor.el);
        }


        mainObj.addEventListener("rollover", function (evt) {

            me._isMouseOver = me.isHover = true;

            me.resetDraw()

            //console.log("Action 进入");


        });



        mainObj.addEventListener("rollout", function (evt) {

            me._isMouseOver = me.isHover = false;

            me.resetDraw();

            //console.log("Action 出来");
        });

        mainObj.addEventListener("mousedown", function (evt) {
            var page = me.page;

            //console.log("Action MouseDown  " + evt.stageX);

            me.downX = evt.stageX - mainObj.x;
            me.downY = evt.stageY - mainObj.y;

            if (page) {
                page.setMousedownTarget.call(page, me);
            }

            me.on('mousedown');


            me.stopPropagation(evt);
        });




        mainObj.on("pressmove", function (evt) {
            var page = me.page;

            if (!page || !page.designMode) {
                return;
            }

            me.x = mainObj.x = evt.stageX - me.downX;
            me.y = mainObj.y = evt.stageY - me.downY;

            //console.log("Action pressmove  " + evt.stageX);
            me.on('pressmove');
        });




    },




    stopPropagation: function (e) {
        e = e || window.event;

        if (e.stopPropagation) {
            e.stopPropagation();
        }
        else {
            e.cancelBubble = false;
        }
    },


    //插入点
    insert: function (index, x, y) {
        "use strict";
        var me = this,
            lineObj = me.lineObj,
            g = lineObj.graphics,
            ps = me.points;


        ps.splice(index, 0, {
            x: x,
            y: y
        });

        me.resetEndAnchor();

        me.resetDraw();

        return me;
    },


    //删除点
    removePoint: function (index) {
        "use strict";
        var me = this,
            lineObj = me.lineObj,
            ps = me.points;
        
        //console.log('line.remove = ' + index);

        ps.splice(index, 1);


        me.resetEndAnchor();


        me.resetDraw();

        return me;
    },


    //设置前端点的坐标
    setFirst: function (x, y) {
        "use strict";
        var me = this;

        me.setPointByIndex(0, x, y);

        return me;
    },

    //设置末端点的坐标
    setLast: function (x, y) {
        "use strict";
        var me = this,
            ps = me.points;

        me.setPointByIndex(ps.length - 1, x, y);

        return me;
    },


    //设置点的坐标
    setPointByIndex: function (index, x, y) {
        "use strict";
        var me = this,
            el = me.el,
            lineObj = me.lineObj,
            g = lineObj.graphics,
            p,
            ps = me.points;

        ps[index] = { x: x, y: y };

        me.resetEndAnchor();

        me.resetDraw();

        return me;
    },

    /**
    * 重置结束箭头
    */
    resetEndAnchor: function () {
        "use strict";
        var me = this,
            endAnchor = me.endAnchor,
            ps = me.points,
            len = ps.length,
            p1 = ps[len - 2],
            p2 = ps[len - 1],
            angle;

        angle = me.getAnagle(p1, p2);

        endAnchor.setXY(p2.x, p2.y);
        endAnchor.setRotation(360 - angle + 90);

        return me;
    },


    /**
    * 两点计算角度
    * @param {point} p0 开始点
    * @param {point} p1 结束点
    * @return {int} angle 角度 
    */
    getAnagle: function (p0, p1) {
        "use strict";
        var me = this;
        
        return me.calulateXYAnagle(p0.x, p0.y, p1.x, p1.y);
    },

    /**
    * 两点计算角度
    * @param {int} startx 开始 X 轴
    * @param {int} starty 开始 Y 轴
    * @param {int} endx 结束 X 轴
    * @param {int} endy 结束 Y 轴
    * @return {int} angle 角度 
    */
    calulateXYAnagle: function (startx, starty, endx, endy) {
        "use strict";
        if (starty == endy) { endy += 0.1;} //特殊
        if (startx == endx) { endx += 0.1;}

        //除数不能为0
        var tan = Math.atan(Math.abs((endy - starty) / (endx - startx))) * 180 / Math.PI;
        
        if (endx > startx && endy > starty)//第一象限
        {
            return -tan;
        }
        else if (endx > startx && endy < starty)//第二象限
        {
            return tan;
        }
        else if (endx < startx && endy > starty)//第三象限
        {
            return tan - 180;
        }
        else {
            return 180 - tan;
        }

    },


    remove: function () {
        var me = this;

        return me.baseRemove();
    },


    baseRemove: function () {
        "use strict";
        var me = this,
            el = me.el,
            page = me.page,
            hotspotPanel = me.hotspotPanel;

        el.enabled = false;

        createjs.Tween.get(el).to({ alpha: 0 }, 200).call(function () {
            page.removeChild(me);

            me.dispose();
        });


        return true;
    },



    //删除自己
    //删除成功,返回true, 否则返回 false
    dispose: function () {
        var me = this;

        return me.baseDispose();
    },


    baseDispose: function () {
        var me = this,
            el = me.el,
            page = me.page;

        el.removeAllEventListeners();

        delete me.points;

        delete me.page;

        delete me.el;

        
        return true;
    },

    //重新计算
    recountRect: function () {
        var me = this,
            i,
            p0, p1,
            ps = me.points,
            len = ps.length,
            minX, minY,
            maxX, maxY,
            offsetX = 0,offsetY = 0,
            rect = {};


        p0 = ps[0];

        minX = maxX = p0.x;
        minY = maxY = p0.y;

        for (i = 1; i < len; i++) {

            p0 = ps[i];

            minX = Math.min(minX, p0.x);
            minY = Math.min(minY, p0.y);
            
            maxX = Math.max(maxX, p0.x);
            maxY = Math.max(maxY, p0.y);
        }


        if (minX != 0 || minY != 0) {

            //if (minX < 0) { offsetX = minX; }

            //if (minY < 0) { offsetY = minY; }


            for (i = 0; i < len; i++) {

                p0 = ps[i];

                p0.x -= offsetX;
                p0.y -= offsetY;

            }

            maxX += offsetX;
            maxY += offsetY;



        }

        return rect;

    },

    //获取绝对坐标
    getALocal:function(){
        var me = this,
            el = me.el,
            parent = el.parent,
            local = {
                x: me.x,
                y: me.y
            };

        while (parent) {
            local.x += parent.x;
            local.y += parent.y;

            parent = parent.parent;
        }

        return local;
    },


    setXY:function(x,y){
        var me = this,
            el = me.el;

        el.x = me.x = x;
        el.y = me.y = y;

        return me;
    },

    getRect: function () {
        "use strict";
        var me = this,
            i,
            p0, p1,
            ps = me.points,
            len = ps.length,
            minX, minY,
            maxX, maxY,
            offsetX = 0, offsetY = 0,
            rect = {};


        p0 = ps[0];

        minX = maxX = p0.x;
        minY = maxY = p0.y;

        for (i = 1; i < len; i++) {

            p0 = ps[i];

            minX = Math.min(minX, p0.x);
            minY = Math.min(minY, p0.y);

            maxX = Math.max(maxX, p0.x);
            maxY = Math.max(maxY, p0.y);
        }


        rect = {
            x0: me.x + minX,
            y0: me.y + minY,
            x1: me.x + (maxX + minX),
            y1: me.y + (maxY + minY)
        }


        return rect;
    },
    

    //数组转换为字符串
    pointsToStr: function (points) {
        "use strict";
        var me = this,
            psStr,
            i,p;

        psStr = '[';

        for (i = 0; i < points.length; i++) {

            var p = points[i];

            if (i > 0) {
                psStr += ', ';
            }

            psStr += '[' + p.x + ',' + p.y + ']';
        }

        psStr += ']';

        return psStr;
    },

    createTextObj: function () {
        "use strict";
        var me = this,
            textObj,
            g;

        textObj = new createjs.Text(me.text, '10pt 微软雅黑', '#000000');

        //textObj.x = 80 / 2;
        //textObj.y = 80 / 2;
        //textObj.width = me.width;
        //textObj.height = me.height;
        textObj.textAlign = 'center';
        textObj.textBaseline = 'middle';

        textObj.shadow = new createjs.Shadow("#FFFFFF", 0, 0, 10);

        //textObj.setBounds(0, 0, me.width, me.height);

        return textObj;
    },
    

    /**
    * 获取线的总长度
    * @return {Number} len 长度
    */
    getLen: function(){
        "use strict";
        var me = this,
            i,
            p0,
            p1,
            ps = me.points,
            lenAll = 0,
            len = ps.length,
            lineLen;

        p0 = ps[0];

        for (i = 1; i < len; i++) {
            p1 = ps[i];

            lineLen = me.getLenForLine(p0, p1);

            lenAll += lineLen;

            p0 = p1;
        }

        return lenAll;
    },



    /**
    * 获取线段的中间点
    * @return {x,y} 
    */
    getCentrePoint: function () {
        "use strict";
        var me = this,
            i,
            p0,
            p1,
            ps = me.points,
            lenAll = 0,
            len = ps.length;


        if (me.textObj == undefined) {
            me.textObj = me.createTextObj();

            me.el.addChild(me.textObj);
        }


        lenAll = me.getLen();

        var lenM2 = lenAll / 2;

        var pIndex = 0;
        var pLen = 0;
        var pAngle = 0;

        var aa = 0;

        p0 = ps[0];

        for (i = 1; i < len; i++) {
            p1 = ps[i];

            var lineLen = me.getLenForLine(p0, p1);
            
            //aa += lineLen;

            if ( lenM2 < lineLen) {

                pIndex = i;
                pLen = lenM2;

                pAngle = me.getAnagle(p0, p1);

                break;
            }

            lenM2 -= lineLen;

            p0 = p1;
        }

        //pAngle -= 90;    //角度

        //console.log("===================== ");
        //console.log("长度: ", lenAll);

        //console.log("pIndex: ", pIndex);
        //console.log("pLen: ", pLen);
        //console.log("pAngle: ", pAngle);


        var cenPoint = me.getCenterRadiusPoint(p0, 360 - pAngle, pLen);

        //console.log("xy", xy);

        me.textObj.x = cenPoint.x;
        me.textObj.y = cenPoint.y;

        return cenPoint;

    },
    
    
    
    /**
    * 获取线段的中间点
    * @param {point} center 中心点
    * @param {number} angle 角度 0-360
    * @param {double} radius 距离
    * @return {point} 坐标
    */
    getCenterRadiusPoint:function (center, angle,  radius)  {
        var p = {
            x: 0,
            y: 0
        },
        angleHude = angle * Math.PI / 180;/*角度变成弧度*/  

        p.x = (radius * Math.cos(angleHude)) + center.x;  
        p.y = (radius * Math.sin(angleHude)) + center.y;

        return p;  
    },


    /**
    * 获取两点间的距离
    * @param {int} p0 点0
    * @param {int} p1 点1
    * @return {int} 长度
    */
    getLenForLine:function(p0,p1){
        "use strict";
        var len,
            xx = p1.x - p0.x,
            yy = p1.y - p0.y;

        len = Math.pow((xx * xx + yy * yy), 0.5);

        return len;
    },


    //获取配置信息, json 格式
    getConfig: function () {
        "use strict";
        var me = this;

        var psStr = me.pointsToStr(me.points);


        //console.debug('startAnchor : ', me.startAnchor);


        var t = {
            item_id: me.item_id,
            item_code: me.item_code,

            item_type: 'line',
            item_fullname: 'Mini2.flow.Line',

            style_name: me.style_name,

            //start_node_code: '',

            //end_node_code: '',

            //start_node_id: '',

            //end_node_id : '',

            desc: me.desc,
            x: me.x,
            y: me.y,

            size: 2,

            start_point : me.start_point,
            end_point: me.end_point,
            

            points_str: psStr
        };


        return t;

    }


}, function () {
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});