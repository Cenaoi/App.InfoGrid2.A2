/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

Mini2.define('Mini2.flow.Anchor', {

    singleton: true,

    LEFT: 1,

    TOP: 3,

    RIGHT: 5,

    BOTTOM:7

});


//组件库
Mini2.define('Mini2.flow.ComponentLabrary', {

    //线类型的组件
    lines : null,

    //图形对象的组件
    shapes: null,


    //初始化组件
    initComponent: function () {
        var me = this;

        me.lines = {};

        me.shapes = {};

        me.lines['Mini2.flow.Line'] = {};
    },

    //添加组件
    addComp:function( fullname){
        var me = this;


    },

    //获取组件库
    getComp: function (fullname) {
        var me = this;


    }


}, function () {
    var me = this;



    Mini2.apply(me, arguments[0]);

    me.initComponent();

});


/**
* 流程图的画板
*
*/
Mini2.define('Mini2.flow.FlowPage', {

    //backColor: '#FFFFFF',

    //画板
    el: null,

    //区域范围
    scope: null,

    visible: true,

    //画板 
    
    stage: null,

    /**
    * 每秒绘制的帧数
    */
    fps: 30,

    autoTick : true,

    width: 800,

    height: 600,

    runding: false,

    // @provate 网格层
    //gridPanel : null,

    // @provate
    //mainPanel: null,

    // @private
    //nodePanel: null,

    // @private
    //linePanel:null,

    
    //窗体管理器
    winMgr: false,

    //线段集合
    lines: false,

    //图像集合
    shapes: false,

    //鼠标点击的目标对象
    mousedownTarget: null,

    //组件库
    compLab: null,


    //是否在加载数据
    _isLoadData : false,

    //开始加载数据
    beginLoadData: function () {
        "use strict";
        var me = this;
        me._isLoadData = true;
        return me;
    },
    
    //结束加载数据
    endLoadData: function () {
        "use strict";
        var me = this;
        me._isLoadData = false;
        return me;
    },


    //初始化组件
    initComponent: function () {
        "use strict";
        var me = this,
            el, stage;


        me.el = el = document.getElementById(me.renderTo);

        me.stage = stage = new createjs.Stage(el);
        //stage.setScaleX(0.5);
        //stage.scaleY = 0.5;

        me.compLab = Mini2.create('Mini2.flow.ComponentLabrary', {});
        
        me.initPanels();
    },



    bind: function (eventName, fun, data, owner) {
        "use strict";

        var me = this,
            evtSet = me.eventSet,
            evts;

        if (!fun) {
            throw new Error('[' + eventName + '] 必须绑定函数.');
        }

        if (!evtSet) {
            me.eventSet = evtSet = {};
        }

        evts = evtSet[eventName];

        if (!evts) {
            evts = evtSet[eventName] = [];
        }


        evts.push({
            fun: fun,
            owner: owner,
            data: data
        });

        return me;
    },



    onPre: function (eventName, data) {
        var me = this;

        me.on(eventName, data);

        return me;
    },

    on: function (eventName, data) {
        "use strict";

        console.debug('流程事件 ' + eventName,  data);

        if (this.eventSet) {

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

                if (!fun || !fun.call) {
                    console.log('函数不存在...');
                    console.log(fun);
                    continue;
                }

                if (evt.data) {
                    evtData = Mini2.clone(evt.data);
                }
                else {
                    evtData = {};
                }


                evtData = Mini2.applyIf(evtData, data)

                
                fun.call(evt.owner || me, evtData);

            }

        }


        return this;
    },

    
    //根据 text 获取 Action 对象
    getActionByText: function (text) {
        "use strict";
        var me = this,
            i,
            shape,
            shapes = me.shapes;


        if (shapes) {

            for (i = 0; i < shapes.length; i++) {
                shape = shapes[i];

                if (shape.text == text) {
                    return shape;
                }
            }

        }

        return null;
    },

    getNodeById: function (nodeId) {
        "use strict";
        var me = this,
            i,
            shape,
            shapes = me.shapes,
            len = shapes.length;


        for (i = 0; i < len; i++) {

            shape = shapes[i];

            if (nodeId == shape.id) {
                return shape;
            }

        }

        return null;

    },


    getNodeByAttr: function (attrName, attrValue) {
        "use strict";
        var me = this,
            i,
            shape,
            shapes = me.shapes,
            len = shapes.length;


        for (i = 0; i < len; i++) {

            shape = shapes[i];

            if (attrValue == shape[attrName]) {
                return shape;
            }

        }

        return null;

    },


    // 重置线的连接-的点
    resetLineLinkPoint:function(point, pointName, line){
        "use strict";

        var me = this,
            node,
            hot;

        if (point) {

            //console.log("ep=" + ep);

            node = me.getNodeById(point.node_id);

            //if (undefined == node) {
            //    node = me.getNodeByAttr('item_code', point.item_code);
            //}

            if (node) {

                hot = node.getHotById(point.hot_id);

                if (hot) {

                    hot.link(pointName, line);

                    //console.log("结束端,接入...");
                }
            }
        }

        return me;
    },

    //重置线的连接
    resetLineLink: function () {
        "use strict";
        var me = this,
            i,
            line,
            lines = me.lines,
            len = lines.length,
            ep,sp;


        //console.log(len);

        for (i = 0; i < len; i++) {
            line = lines[i];

            ep = line.end_point;
            sp = line.start_point;

            me.resetLineLinkPoint(ep, 'end', line);
            me.resetLineLinkPoint(sp, 'start', line);

            line.resetDraw();
            line.resetEndAnchor();
        }



        return me;
    },




    //删除全部
    removeAll: function () {

        var me = this,
            i,
            lines = me.lines,
            shapes = me.shapes,
            line,
            shape;

        for (i = 0; i < lines.length; i++) {
            line = lines[i];
            line.remove();
        }

        for (i = 0; i < shapes.length; i++) {
            shape = shapes[i];

            shape.remove();
        }

        return me;
    },



    removeChild:function(item){
        var me = this;

        me.baseRemoveChild(item);
    },


    //删除
    baseRemoveChild:function(item){
        var me = this,
            subEl = item.el;

        if (item.isLine) {

            Mini2.Array.remove(me.lines, item);
            
            me.linePanel.removeChild(subEl);
        }
        else {

            Mini2.Array.remove(me.shapes, item);
            
            me.nodePanel.removeChild(subEl);

        }

    },


    //添加
    baseAddChild: function (item) {
        "use strict";
        var me = this,
            stage = me.stage,
            subEl;

        //console.log('添加:' + item.el);

        if (item == undefined ) {
            throw new Error('对象不能为 null');
        }

        if (item.el == undefined) {
            throw new Error('对象的 el 元素不能为空');
        }

        item.setPage(me);

        subEl = item.el;

        if (subEl.Mini2_Object !== item) {
            subEl.Mini2_Object = item;
        }



        item.parent = me;


        if (item.isLine) {

            me.lines.push(item);

            me.linePanel.addChild(subEl);

            if (item.bind) {
                item.bind('mousedown', me.item_MouseDown, null, me);
            }

            item.resetDraw();
        }
        else {
            me.shapes.push(item);

            me.nodePanel.addChild(subEl);

            if (item.bind) {
                item.bind('pressmove', me.item_PressMove, null, me)
                    .bind('mousedown', me.item_MouseDown, null, me);
            }
        }

    },

    // 添加子节点
    addChild: function (item) {
        "use strict";
        var me = this;

        me.baseAddChild(item);

        return me;
    },

    //根据 config 配置列表, 创建子元素
    addChildByConfigList: function (cfgList) {
        "use strict";
        var me = this,
            cfg,i = 0;
        
        for (; i < cfgList.length; i++) {
            cfg = cfgList[i];
            me.addChildByConfig(cfg);
        }

        return me;
    },

    //根据 config 配置, 创建子元素
    addChildByConfig: function (cfg) {
        "use strict";
        var me = this,
            fullname = cfg.item_fullname,
            type = cfg.item_type,
            obj;


        if (fullname) {
            obj = Mini2.create(fullname, cfg);
        }
        else {
            
            if ('line' == type) {
                obj = Mini2.create('Mini2.flow.Line', cfg);
            }
            else if ('action' == type) {
                obj = Mini2.create('Mini2.flow.Action', cfg);
            }
        }

        me.addChild(obj);

    },


    //添加窗体层
    addWindow:function(win){
        var me = this,
            stage = me.stage,
            winMgr = me.winMgr;

        if (!winMgr) {
            me.winMgr = winMgr = Mini2.create('Mini2.flow.WindowManager', {

            });

            winMgr.render();

            stage.addChild(winMgr.el);
        }

        winMgr.addChild(win);
    },

    item_MouseDown:function(sender,e){
        var me = this,
            item = sender;

        if (me.curFocusObj === item) {
            return;
        }


        me.curFocusObj = item;

        if (item.isLine) {


        }
        else {

        }

    },

    item_PressMove:function(sender,e){
        var me = this,
            item = sender;
        

    },


    initPanels:function(){
        var me = this,
            gridPanel,  //网格层
            mainPanel,   //画板
            nodePanel,  //节点图层
            linePanel;  //线图层

        //主绘画成
        me.mainPanel = mainPanel = new createjs.Container();

        //节点层
        me.nodePanel = nodePanel = new createjs.Container();

        //线段层
        me.linePanel = linePanel = new createjs.Container();

        //网格背景
        me.gridPanel = gridPanel = Mini2.create('Mini2.flow.FlowPageGridBack', {

        });

        mainPanel.addChild(gridPanel.el, linePanel, nodePanel);

    },


    start:function(){
        var me = this;

        me.runding = true;

        return me;
    },

    stop:function(){
        var me = this;

        me.runding = false;

        return me;
    },

    tick:function(evt){
        var me = this;

        if (me.runding) {
            me.stage.update();
        }
        
    },

    test:function(){

        //var bitmap = new createjs.Bitmap("http://www.baidu.com/img/baidu_jgylogo3.gif");
        //bitmap.x = 20;
        //bitmap.y = 200;

        //stage.addChild(bitmap);


        //var imgMonsterARun = "http://david.blob.core.windows.net/easeljstutorials/img/MonsterARun.png";

        //var spriteSheet = new createjs.SpriteSheet({
        //    // image to use
        //    images: [imgMonsterARun],
        //    // width, height & registration point of each sprite
        //    frames: { width: 64, height: 64, regX: 32, regY: 32 },
        //    animations: {
        //        walk: [0, 9, "walk"]
        //    }
        //});



        //var man = new createjs.Sprite(spriteSheet, 'walk');
        //man.x = 600
        //man.y = 70;

        //man.play();

        //stage.addChild(man);


        //var rect = new createjs.Rectangle(0, 0, 100, 100);
        //rect.addEventListener('mousedown', function () {
        //    alert("xxxx");
        //});
        //stage.addChild(rect);

    },


    //设置鼠标点击的对象,
    setMousedownTarget:function(target){
        var me = this;

        //if (target) {
        //    console.log('setMousedownTarget=' + target + ', text=' + target.text + ', isLine=' + target.isLine);
        //}

        me.mousedownTarget = target;

        return me;
    },


    onKeydown:function(evt){


    },


    onKeyup:function(evt){

    },


    //根据配置信息, 创建对象
    baseAddChildByConfig: function (itemCfg, rect) {
        "use strict";
        var me = this,
            obj = null,
            cfg = rect,
            cType = itemCfg.item_type,
            cFullname = itemCfg.item_fullname;

        console.log('创建对象...', itemCfg);

        if ('line' == cType) {
            cfg.points = [
                [0, 0],
                [100, 0]
            ];
        }

        if (cFullname) {
            console.debug("cfg", cfg);
            obj = Mini2.create(cFullname, cfg);
        }
        else {
            if ('line' == cType) {
                obj = Mini2.create('Mini2.flow.Line', cfg);
            }
            else if ('node' == cType) {
                obj = Mini2.create('Mini2.flow.Action', cfg);
            }
            else if ('action' == cType) {
                obj = Mini2.create('Mini2.flow.Action', cfg);
            }
            else {
                throw new Error('不能处理这种类型 cType = ' + cType);
            }
        }


        me.addChild(obj);

    },
    

    stage_mousedown:function(evt){

    },

    stage_mouseup: function (evt) {
        "use strict";
        var me = this,
            mdTarget = me.mousedownTarget;


        
        if (mdTarget && mdTarget.onPreEvent) {

            console.log("触发 stage_mouseup");

            mdTarget.onPreEvent.call(mdTarget, 'mouseup');

            me.setMousedownTarget(null);
        }
        else if (evt.relatedTarget) {

            //console.log("不触发");

            var target = evt.relatedTarget,
                m2Obj = target.Mini2_Object;

            if (m2Obj && m2Obj.onPreEvent) {
                m2Obj.onPreEvent.call(m2Obj, 'mouseup');
            }
        }
    },

    stage_mousemove: function (evt) {
        var me = this;

        //console.log('stage.MouseMove    FPS:' + me.fps);
    },

    baseRender: function () {
        "use strict";
        var me = this,
            el = el,
            stage = me.stage,
            mainPanel = me.mainPanel;
        
        me.lines = [];
        me.shapes = [];

        $('#' + me.renderTo).attr('width', me.width)
            .attr('height',me.height);

        stage.addChild(mainPanel);


        window.addEventListener("keydown", function (evt) {
            evt = evt || window.event;
            
            me.onKeydown.call(me, evt);
        })

        window.addEventListener("keyup", function (evt) {
            evt = evt || window.event;

            me.onKeyup.call(me, evt);
        })


        var sMouseOver = function (evt) {
            me.stage_mousemove.call(me, evt);
        };

        stage.addEventListener('stagemousedown', function (evt) {

            //console.log('stage.MouseDown');

            me.stage_mousedown.call(me, evt);


            stage.addEventListener('stagemousemove', sMouseOver);

            
        });

        stage.addEventListener('stagemouseup', function (evt) {

            //console.log('stage.MouseUp');

            stage.removeEventListener('stagemousemove', sMouseOver);


            me.stage_mouseup.call(me, evt);
        });


        //stage.addEventListener('dblclick', function (evt) {

        //    console.log("双击啊.");

        //});


        stage.enableMouseOver(30);

        if (me.autoTick) {
            createjs.Touch.enable(stage);

            createjs.Ticker.setFPS(me.fps);
            createjs.Ticker.addEventListener("tick", function (evt) {
                me.tick.call(me, evt);
            });
        }

        me.runding = true;


        stage.update();
        
    },


    run:function(){
        var me = this
        el = el,
        stage = me.stage;


    },

    proResize:function(){
        var me = this;

        me.onResize();
    },

    onResize: function () {
        "use strict";
        var me = this,
            gridPanel = me.gridPanel;

        if (gridPanel && gridPanel.onResize) {
            gridPanel.setSize(me.width, me.height );
            gridPanel.onResize();
        }
        else {
            console.log('没有网格层.');
        }
    },


    render: function () {
        "use strict";
        var me = this;

        me.baseRender();

        me.proResize();

        return me;
    },
    

    getConfig: function () {
        'user strict';
        
        var me = this,
            i,
            lines = me.lines,
            shapes = me.shapes,
            line,
            shape;


        var config = {
            v: 'v1',
            def_id: me.id,
            data:[]
        };

        var data = config.data;


        for (i = 0; i < shapes.length; i++) {
            shape = shapes[i];
            var cfg = shape.getConfig();
            data.push(cfg);            
        }

        for (i = 0; i < lines.length; i++) {
            line = lines[i];
            var cfg = line.getConfig();
            data.push(cfg);
        }

        return config;
    }



}, function () {
    "use strict";
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});