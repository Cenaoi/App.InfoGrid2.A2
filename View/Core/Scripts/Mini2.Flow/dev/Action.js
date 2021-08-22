/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

Mini2.define('Mini2.flow.Action', {

    extend: 'Mini2.flow.FlowNode',


    initComponent: function () {
        var me = this;


        me.initComponentBase();

        try {
            me.setSize(me.width, me.height);
        }
        catch (ex) {
            console.error('初始化错误', ex);
        }

    },

    width: 100,

    height: 40,

    x: 0,

    y: 0,

    mainObj:null,

    txtObj: null,

    boxObj: null,


    //状态
    state: 0,


    defaultRect: {
        x: 0,
        y: 0,
        w: 100,
        h: 50
    },

    //事件集
    //eventSet:{
    //    //'mousedown':[],
    //    //'
    //},


    //当前样式
    curStyle: null,

    tooltipText: false,

    


    style:{
        

        //灰透-未生效
        '0': {

            back_color: '#f2f2f2',

            back_color_over: '#f2f2f2',

            font_color: '#BFBFBF',

            //边框大小
            border_width: 1,

            //边框类型  solid-实线, dashed-虚线
            border_style: 'dashed' 

        },

        //状态1 ,橘色-未处理
        '1': {
            //背景颜色
            back_color: '#ed7d31',

            back_color_over: '#ed7d31',

            //文字颜色
            font_color: '#FFFFFF',

            //边框大小
            border_width: 1,

            //边框类型
            border_style: 'solid' //solid-实线, dashed-虚线
        },
        
        //绿色-已经处理过.
        '2': {

            //背景颜色
            back_color: '#70ad47',

            //鼠标移动到上面
            back_color_over: '#70ad47',

            //文字颜色
            font_color: '#FFFFFF',

            //边框大小
            border_width: 1,

            //边框类型
            border_style: 'solid' //solid-实线, dashed-虚线



        } ,

        //绿色-已经处理过.
        '4': {

            //背景颜色
            back_color: '#FFFFFF',

            //鼠标移动到上面
            back_color_over: '#FFFFFF',

            //文字颜色
            font_color: '#000000',

            //边框大小
            border_width: 1,

            //边框类型
            border_style: 'solid' //solid-实线, dashed-虚线



        }
        
    },


    setText:function(txt){
        var me = this,
            txtObj = me.txtObj;

        me.text = txt;

        if (txtObj) {
            txtObj.text = txt;
        }
    },

    onMouseUp: function () {
        var me = this;

        //console.log(me.text + ': MouseUp()');
    },
    

    createTextObj: function () {
        "use strict";
        var me = this,
            style = me.curStyle || me.style['0'],
            textObj,
            g;

        textObj = new createjs.Text(me.text, '10pt 微软雅黑', style.font_color);

        textObj.x = me.width / 2;
        textObj.y = me.height / 2;
        //textObj.width = me.width;
        //textObj.height = me.height;
        textObj.textAlign = 'center';
        textObj.textBaseline = 'middle';


        
        //textObj.setBounds(0, 0, me.width, me.height);

        return textObj;
    },

    //获取状态
    setState: function (value) {
        "use strict";
        var me = this,
            txtObj = me.txtObj,
            state = me.state = value,
            style;

            style = me.style[state];
        

        me.curStyle = style;

        me.drawReset();


        //txtObj.x = me.width / 2;
        //txtObj.y = me.height / 2;
        if (txtObj) {
            txtObj.color = style.font_color;
        }

        return me;
    },


    setSize: function (width, height) {
        "use strict";
        var me = this,
            txtObj = me.txtObj;

        me.baseSetSize(width,height);


        if (txtObj) {
            txtObj.x = me.width / 2;
            txtObj.y = me.height / 2;
        }
        
        me.resetHotspot();

        me.drawReset();

        return me;
    },

    
    setRect: function (x, y, width, height) {
        "use strict";
        var me = this,
            el = me.el,
            txtObj = me.txtObj;

        if (undefined != x) {
            me.x = el.x = x;
        }

        if (undefined != y) {
            me.y = el.y = y;
        }

        if (undefined != width) {
            me.width = el.width = width;

            if (txtObj) {
                txtObj.x = width / 2;
            }
        }

        if (undefined != height) {
            me.height = el.height = height;

            if (txtObj) {
                txtObj.y = height / 2;
            }
        }

        
        me.resetHotspot();  //重置热点位置

        me.drawReset(); //重新绘制

        return me;

    },


    //获取状态
    getState : function(){
        var me = this,
            state = me.state;

        return state;
    },

    createBoxObj:function(){
        var me = this,
            state = me.state,
            boxObj,
            g;

        boxObj = new createjs.Shape();
        
        return boxObj;
    },


    drawReset: function (graphics, exWidth, exHeight) {
        "use strict";
        var me = this,
            style = me.curStyle,
            boxObj = me.boxObj,
            width = exWidth || me.width,
            height = exHeight || me.height,
            g ;


        g = graphics || boxObj.graphics;

        g.clear();

        g.setStrokeStyle(style.border_width);

        if ('dashed' == style.border_style) {
            g.setStrokeDash([4, 4]);
        }

        g.beginStroke(createjs.Graphics.getRGB(202, 201, 199, 1));
        g.beginFill(style.back_color)
            .drawRect(0, 0, width, height);

        
        return me;
    },



    //重置连线的端点
    resetLinePoint:function(){
        var me = this,
            i,
            hot,
            hots = me.hotspots,
            len = hots.length;

        for (i = 0; i < len; i++) {

            hot = hots[i];

            var spList = hot.lineStartPoints;

            var epList = hot.lineEndPoints;

            if (spList.length) {

                var aX = me.x + hot.x;
                var aY = me.y + hot.y;

                //console.log('aX=' + aX + ', aY=' + aY);

                for (var j = 0; j < spList.length; j++) {

                    var sp = spList[j];

                    sp.setFirst(aX - sp.x,aY - sp.y);

                }

            }

            if (epList.length) {

                var aX = me.x + hot.x;
                var aY = me.y + hot.y;


                for (var j = 0; j < epList.length; j++) {

                    var ep = epList[j];

                    ep.setLast(aX - ep.x, aY - ep.y);


                }

            }
        }


    },



    //根据热点id 获取热点对象
    getHotById:function(hotId){
        var me = this,
            i,
            hot,
            hots = me.hotspots,
            len = hots.length;

        for (i = 0; i < len; i++) {

            hot = hots[i];

            if(hotId == hot.id){
                return hot;
            }
        }

        return null;

    },

    //创建热点集合
    createHotspotList:function(){
        var me = this,
            el = me.el,
            i,
            srcRect = me.srcRect,
            ach = Mini2.flow.Anchor,
            hot,
            hotName = 'Mini2.flow.Hotspot',
            hots = [];


        hot = Mini2.create(hotName, {
            id: 'L',
            srcRect: {
                y: srcRect.h / 2
            },
            ownerAction : me,
            anchor: ach.LEFT
        });
        hots.push(hot);
        

        hot = Mini2.create(hotName, {
            id: 'T',
            srcRect: {
                x: srcRect.w / 2
            },
            ownerAction: me,
            anchor: ach.TOP
        });
        hots.push(hot);


        hot = Mini2.create(hotName, {
            id: 'R',
            srcRect: {
                x: srcRect.w ,
                y: srcRect.h / 2
            },
            ownerAction: me,
            anchor: ach.RIGHT
        });
        hots.push(hot);


        hot = Mini2.create(hotName, {
            id: 'B',
            srcRect: {
                x: srcRect.w / 2,
                y: srcRect.h 
            },
            ownerAction: me,
            anchor: ach.BOTTOM
        });
        hots.push(hot);


        //中间
        hot = Mini2.create(hotName, {
            id: 'CC',
            srcRect: {
                x: srcRect.w / 2,
                y: srcRect.h / 2
            },
            ownerAction: me,
            anchor: null
        });
        hots.push(hot);

        return hots;
    },

    hot_rollover: function (e,exData) {
        var me = this;

        //console.log("ddddddddd");
        
        me.on('hotRollover', Mini2.apply(exData, {
            node:me
        }));
    },

    hot_rollout:function(e,exData){
        var me = this;

        me.on('hotRollout', Mini2.apply(exData, {
            node: me
        }));
    },


    //重置热点位置
    resetHotspot:function(){
        var me = this,
            ach = Mini2.flow.Anchor,
            srcRect = me.srcRect,
            hots = me.hotspots || [],
            hotPanel = me.hotPanel,
            newX,
            newY,
            isL,isR,isT,isB, anchor;

        for (var i = 0; i < hots.length; i++) {

            hot = hots[i];

            anchor = hot.anchor;

            isL = (anchor == ach.LEFT);
            isR = (anchor == ach.RIGHT);
            isT = (anchor == ach.TOP);
            isB = (anchor == ach.BOTTOM);


            if (isL && isR) {

            }
            else if (isL) {
                newX = hot.srcRect.x;
            }
            else if (isR) {
                newX = me.width - (srcRect.w - hot.srcRect.x);
            }
            else {
                var mm = me.width / srcRect.w;

                newX = mm * hot.srcRect.x;
            }


            if (isT && isB) {

            }
            else if (isT) {
                newY = hot.y;
            }
            else if (isB) {
                newY = me.height - (srcRect.h - hot.srcRect.y);
            }
            else {
                var mm = me.height / srcRect.h;

                newY = mm * hot.srcRect.y;
            }

            //console.log("---------------- index = " + i);

            //console.log($.format('L={0}, R={1}, T={2}, B={3}', isL, isR, isT, isB));

            //console.log(newX + "," + newY);

            hot.setXY(newX, newY);

        }


        if (hotPanel) {
            hotPanel.x = me.x;
            hotPanel.y = me.y;
        }

        return me;
    },


    //显示热点
    showHotspot:function(){
        var me = this,
            el = me.el,
            i,
            hots = me.hotspots,
            hotPanel = me.hotPanel;

        if (hotPanel && hotPanel.visible) {
            me.resetHotspot();

            return;
        }

        if (!me.hotPanel) {

            //me.hotspots = hots = me.createHotspotList();



            me.hotPanel = hotPanel = new createjs.Container();    //热点容器
            

            for (i = 0; i < hots.length; i++) {

                hot = hots[i];

                hot.render();

                hot.bind('rollover', me.hot_rollover, null, me);
                hot.bind('rollout', me.hot_rollout, null, me);

                hotPanel.addChild(hot.el);
            }
        }

        //el.addChild(me.hotPanel);

        me.page.hotspotPanel.addChild(hotPanel);

        hotPanel.visible = true;
        me.resetHotspot();

        return me;
    },



    //隐藏热点
    hideHotspot:function(){
        var me = this,
            hotPanel = me.hotPanel;

        if (hotPanel) {
            hotPanel.visible = false;
        }

        return me;
    },


    //提示框
    tooltipBox: false,


    setTooltip:function(text){
        var me = this,
            tipBox = me.tooltipBox;

        me.tooltipText = text;

        if (tipBox) {
            tipBox.setText(text);
        }

        return me;
    },

    //显示提示信息
    showTooltip:function(){
        var me = this,
            page = me.page,
            tipOffset = me.tipOffset || {x:0,y:0},
            tipBox = me.tooltipBox;

        if (!tipBox) {
            tipBox = Mini2.create('Mini2.flow.Tooltip', {
                owner: me,
                page : me.page,
                text: me.tooltipText
            });

            tipBox.render();

            me.tooltipBox = tipBox;
        }

        tipBox.setText(me.tooltipText);
        tipBox.setXY(me.x + tipOffset.x, me.y + tipOffset.y);

        tipBox.show();

        return me;
    },

    //隐藏提示信息
    hideTooltip:function(){
        var me = this,
            page = me.page,
            tipBox = me.tooltipBox;

        if (tipBox) {
            tipBox.hide();
        }

        return me;
    },


    rollover:function(){
        var me = this;

        me.baseRollover();

        return me;
    },
    

    rollout:function(){
        var me = this;

        me.baseRollout();

        return me;
    },


    baseRollover:function(){
        var me = this,
            boxObj = me.boxObj,
            g = boxObj.graphics,
            style = me.curStyle;

        g.beginFill(style.back_color_over).drawRoundRect(0, 0, me.width, me.height, 4);

        return me;
    },
    

    baseRollout:function(){
        var me = this,
            boxObj = me.boxObj,
            g = boxObj.graphics,
            style = me.curStyle;

        g.beginFill(style.back_color).drawRoundRect(0, 0, me.width, me.height, 4);

        return me;
    },


    initComponentBase:function(){

        var me = this,
            el,
            mainObj,
            boxObj,
            txtObj,
            srcRect = me.srcRect || me.defaultRect;

        
        me.srcRect = srcRect = Mini2.apply(srcRect, me.srcRect);

        //console.log('srcRect', srcRect);

        me.el = me.mainObj = mainObj = new createjs.Container();

        mainObj.Mini2_Object = me;

        mainObj.x = me.x + 0.5;
        mainObj.y = me.y + 0.5;

        //me.baseResetSize(me.width, me.height);

        me.boxObj = boxObj = me.createBoxObj();


        txtObj = me.createTextObj();

        if (txtObj) {
            me.txtObj = txtObj;
            txtObj.x = me.width / 2;
            txtObj.y = me.height / 2;

            mainObj.addChild(boxObj, txtObj);
        }

        me.setState(me.state);

        //boxObj.shadow = new createjs.Shadow(createjs.Graphics.getRGB(147, 147, 147, 0.1), 0, 5, 4);

        mainObj.addEventListener("rollover", function (evt) {

            me.rollover.call(me);

            me.showTooltip();


            me.stopPropagation(evt);
        });



        mainObj.addEventListener("rollout", function (evt) {

            me.rollout.call(me);

            me.hideTooltip();


            me.stopPropagation(evt);
        });

        mainObj.addEventListener("mousedown", function (evt) {

            var page = me.page;

            me.downX = evt.stageX - mainObj.x;
            me.downY = evt.stageY - mainObj.y;

            if (page) {
                page.setMousedownTarget.call(page, me);
            }

            //console.log('mousedown: ' + me.text);

            //console.log(evt);

            me.on('mousedown');
            
            me.stopPropagation(evt);
        });



        me.hotspots = me.createHotspotList();
        me.resetHotspot();
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

    setXY: function (x, y) {
        var me = this,
            el = me.el;

        if (undefined != x) {
            me.x = el.x = x;
        }

        if (undefined != y) {
            me.y = el.y = y;
        }

        return me;
    },



    //删除自己
    remove: function () {
        var me = this;

        return me.baseRemove();
    },

   
    //删除自己的基本函数
    baseRemove:function(){
        var me = this,
            el = me.el,
            page = me.page,
            hotspotPanel = me.hotspotPanel;

        el.enabled = false;

        page.unlinkForHotAll(me);

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
            page = me.page,
            hotspotPanel = me.hotspotPanel;


        if (hotspotPanel && me.hotPanel) {

            for (var i = 0; i < me.hotspots.length; i++) {

                me.hotspots[i].remove();

            }

            hotspotPanel.removeChild(me.hotPanel);

            delete me.hotspots;
        }

        el.removeAllEventListeners();

        if (me.eventSet) {
            
            delete me.eventSet;

        }

        return true;
    },


    //获取矩形区域
    getRect: function () {
        var me = this,
            aLocal = me.getALocal(),
            w = me.width,
            h = me.height,
            x = aLocal.x,
            y = aLocal.y,
            rect = {
                x : x,
                y : y
            };

        if (w < 0) {
            rect.x0 = x + w;
            rect.x1 = x;
        }
        else {
            rect.x0 = x;
            rect.x1 = x + w;
        }


        if (h < 0) {
            rect.y0 = y + h;
            rect.y1 = y;
        }
        else {
            rect.y0 = y;
            rect.y1 = y + h;
        }

        rect.width = w;
        rect.height = h;

        return rect;
    },
    //获取配置信息, json 格式
    getConfig: function () {
        var me = this;

        //{
        //    item_type: 'action',
        //    item_fullname: 'Mini2.flow.Action',

        //    id: 'A0001',
        //    text: '下订单',
        //    x: 10.5,
        //    y: 20.5,
        //    state: 1,       //状态 0-未执行, 1-已经执行 , 2 -
        //    width: 100,    //宽度
        //    height: 50      //高度

        //}

        var t = {
            item_type: 'node',
            item_fullname: 'Mini2.flow.Action',
            item_id: me.item_id,
            item_code: me.item_code,

            style_name: me.style_name,

            className: me.className,

            id: me.id,

            text: me.text,

            x: me.x,
            y: me.y,
            width: me.width,
            height : me.height
        };

        return t;
    }




}, function () {
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});