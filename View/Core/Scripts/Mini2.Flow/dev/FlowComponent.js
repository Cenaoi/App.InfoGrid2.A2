
/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

// 流程图的节点

Mini2.define('Mini2.flow.FlowComponent', {

    isFlowNode: true,

    text: null,

    el: null,

    pageItems: false,



    minWidth: false,
    
    minHeigth: false,
    
    maxWidth: false,
    
    maxHeight: false,



    //实例化时候的尺寸
    //_srcRect: {
    //    x: 0,
    //    y: 0,
    //    w: 0, //宽度
    //    h: 0  //高度
    //},

    

    //1-左,3-上 ,5-右 ,7-下
    //绑定边缘
    //anchor : 0,


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
            css,
            className = me.className;

        me.styleList = styleList = [];

        classStrList = className.split(' ');

        for (var i = 0; i < classStrList.length; i++) {

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

        i = len;

        if (undefined === cssValue) {
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



    //设置绑定边缘
    setAnchor:function(anchor){
        var me = this;

        me.anchor = anchor;


        return me;
    },

    initComponent: function () {
        var me = this,
            el;


        //var bitmap = new createjs.Bitmap("http://www.baidu.com/img/baidu_jgylogo3.gif");
        //bitmap.x = 20;
        //bitmap.y = 200;



        //bitmap.addEventListener("rollover", function (evt) {
        //    console.log("bitmap 进入");
        //});



        //bitmap.addEventListener("rollout", function (evt) {

        //    console.log("bitmap 出来");
        //});



        //bitmap.addEventListener("mouseout", function (evt) {

        //    console.log("bitmap mouseout");
        //});


        //bitmap.addEventListener("mousedown", function (evt) {

        //    console.log("bitmap 点击...");

        //});


        //me.el = bitmap;
    },



    onMouseUp: function () {
        var me = this;

        console.log("onMouseUp");
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

    //添加页面的 item 
    addPageItem: function () {
        "use strict";
        var me = this,
            i,
            item,
            args = arguments,
            pageItems = me.pageItems = me.pageItems || [];



        for (i = 0; i < args.length; i++) {
            item = args[i];

            if (!item.page) {
                item.setPage(me.page);
            }

            pageItems.push(item);

            item.parent = me;

            if (me.el && item.el) {
                me.el.addChild(item.el);
            }
        }


        return me;
    },


    setPage:function(page){
        var me = this;

        me.page = page;



        return me;
    },


    baseRender: function () {
        var me = this;

    },



    render: function () {
        var me = this;

        me.baseRender();

    },

    //绑定事件
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



    onPre:function(eventName,data){
        var me = this;

        me.on(eventName, data);

        return me;
    },


    //触发事件
    on: function (eventName, data) {
        "use strict";

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
                    console.trace();
                    continue;
                }

                if (evt.data) {
                    evtData = Mini2.clone(evt.data);
                }
                else {
                    evtData = {};
                }


                evtData = Mini2.applyIf(evtData, data)

                fun.call(evt.owner || me, me, evtData);

            }

        }


        return this;
    },



    //获取旋转角度
    getRotation: function () {
        var me = this,
            el = me.el;

        return el.rotation;
    },

    //设置旋转角度
    setRotation: function (value) {
        var me = this,
            el = me.el;

        el.rotation = value;

        return me;
    },


    offset: function (offsetX, offsetY) {
        "use strict";
        var me = this,
            el = me.el;

        me.x = el.x = (el.x + offsetX);

        me.y = el.y = (el.y + offsetY);

        return me;
    },


    setXY: function (x, y) {
        "use strict";
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

    getXY:function(){
        var me = this,
            el = me.el;

        return { x: me.x, y: el.y };
    },

    getX: function () {
        var me = this,
            el = me.el;

        return el.x;
    },

    getY: function () {
        var me = this,
            el = me.el;

        return el.y;
    },

    setX: function (value) {
        var me = this;

        me.setXY(value);

        return me;
    },

    setY: function (value) {
        var me = this,
            el = me.el;

        me.setXY(undefined,value);

        return me;
    },

    getSize: function(){
        var me = this;

        return { width: me.width, height: me.height };
    },

    //重新计算
    baseResetSize: function (width, height) {
        var me = this,
                    wChanged = undefined != width,
                    hChanged = undefined != height;

        if (wChanged) {

            if (me.minWidth && width < me.minWidth) {
                width = me.minWidth;
            }
            if (me.maxWidth && width > me.maxWidth) {
                width = me.maxWidth;
            }

            me.width = width;
        }

        if (hChanged) {

            if (me.minHeigth && height < me.minHeigth) {
                height = me.minHeigth;
            }

            if (me.maxHeight && height > me.maxHeight) {
                height = me.maxHeight;
            }

            me.height = height;
        }

        return (wChanged || hChanged);
    },

    baseSetSize:function(width, height){
        var me = this,
            isChanged ;

        isChanged = me.baseResetSize(width, height);

        if (isChanged) {

            if (me.el) {
                me.drawReset();
            }
        }

        return me;
    },

    setSize: function (width, height ) {
        var me = this;
        me.baseSetSize(width, height);

        return me;
    },


    getWidth:function(){
        var me = this;

        return me.width;
    },

    
    getHeight:function(){
        var me = this;

        return me.height;
    },


    //获取绝对位置
    getALocal: function () {
        "use strict";
        var me = this,
            el = me.el,
            parent = el.parent,
            local = {
                x: me.x || 0,
                y: me.y || 0
            };

        while (parent) {
            local.x += parent.x;
            local.y += parent.y;

            parent = parent.parent;
        }

        return local;
    },



    getVisible: function () {
        "use strict";
        var me = this,
            el = me.el;

        return el.visible;
    },

    setVisible: function (value) {
        "use strict";
        var me = this,
            el = me.el;

        el.visible = value;

        return me;
    },


    hide: function () {
        "use strict";
        var me = this;
        me.setVisible(false);
        return me;
    },

    show: function () {
        "use strict";
        var me = this;
        me.setVisible(true);
        return me;
    },

    drawReset: Mini2.emptyFn,

    //获取矩形区域
    getRect: function () {
        var me = this;

        return {
            x0: me.x0,
            y0: me.y0,
            x1: me.x1,
            y1: me.y1
        };
    },

    baseSetRect:function(x,y,width, height){
        "use strict";
        var me = this,
            el = me.el;

        me.setXY(x, y);
        me.setSize(width, height);

        return me;
    },
    
    setRect: function (x, y, width, height) {
        "use strict";
        var me = this;

        me.baseSetRect(x, y, width, height);
    },

    hitTest: function (x, y) {
        var me = this,
            el = me.el;

        return el.hitTest(x, y);
    },

    toString: function () {
        "use strict";
        var me = this;

        return '[' + me.text + '] - ' + me.getType().name + ' - ' + me.muid;
    },
    

}, function () {
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});