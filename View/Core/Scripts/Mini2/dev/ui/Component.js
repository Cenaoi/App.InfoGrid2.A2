
/// <reference path="../Mini.js" />


/// <reference path="../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="FocusManager.js" />
/// <reference path="EventManager.js" />



//焦点管理
Mini2.define("Mini2.ui.Component", {

    el: null,

    log: Mini2.Logger.getLogger('Mini2.ui.Component'),

    /**
    * 设备类型 [auto | computer | touch ]
    */
    deviceType : 'auto', 

    //配合样式使用
    ui : 'default', 

    //区域范围
    scope: null,

    isComponent: true,


    renderTo: null, // Mini2.getBody(),

    visible: true,

    disabled: false,//无效

    //组件附带的事件
    eventNames: [],


    //父容器
    ownerParent: null,

    /**
    * 调用父容器,通知本容器已经刷新
    */
    callParent: function () {
        var me = this,
            ownerParent = me.ownerParent;

        if (ownerParent && ownerParent.updateLayout) {
            ownerParent.updateLayout();
        }

        return me;
    },


    //初始化组件
    initComponent: Mini2.emptyFn,

    defaultRenderer: Mini2.emptyFn,


    //触发事件
    trigger: function (eventName, eventParams) {
        "use strict";
        var me = this,
            owner,
            i,
            event = '_EVENT_' + eventName,
            funs,
            fun,
            cancel;

        // console.debug('触发事件', eventName);
        funs = me[event];

        if (funs) {
            for (i = 0; i < funs.length; i++) {
                fun = funs[i];

                if (fun.owner) {
                    owner = fun.owner;
                    fun = fun.fun;
                }
                else {
                    owner = me;
                }

                cancel = fun.call(owner, eventParams);
                
                if (true === cancel) {
                    break;
                }
            }
        }

        return cancel;
    },

    //绑定事件
    bind: function (eventName, owner, fun) {
        "use strict";
        var me = this,
            event = '_EVENT_' + eventName,
            funs;

        funs = me[event] || [];


        if (undefined === fun) {
            fun = owner;
        }
        else {
            fun = {
                owner: owner,
                fun: fun
            }
        }

        funs.push(fun);

        if (!me[event]) {
            me[event] = funs;
        }

        return me;
    },


    on: function (eventName, data, scope) {
        "use strict";
        /// <summary>绑定事件</summary>
        var me = this;

        switch (eventName) {
            case 'mouseup':
                Mini2.EventManager.setMouseUp(me,data);
                break;
            case 'mousemove':
                Mini2.EventManager.setMouseMove(me,data);
                break;
            default:
                $(me).muTriggerHandler(eventName, data);
                break;

        }

        return me;
    },



    addCls: function (cls) {
        "use strict";
        var me = this,
            el = me.rendered ? me.el : me.protoEl;

        //el.addCls.apply(el, arguments);
        $(me.el).addClass(cls);

        return me;
    },



    onResize: function (width, height, oldWidth, oldHeight) {

    },



    setLocation: function (x, y) {
        return this.setLeft(x).setTop(y);
    },
    
    setLocal: function (x, y) {
        var me = this;
        return me.setLocation(x, y);
    },

    setTop: function (v) {
        var me = this;
        me.top = v;
        me.el.css('top', v);

        return this;
    },

    getTop:function(){
        var me = this,
            el = me.el;
        return el.css('top');
    },



    setLeft: function (v) {
        var me = this,
            el = me.el;

        me.left = v;
        el.css('left', v);
        return me;
    },
    getLeft:function(){
        var me = this,
            el = me.el;
        return el.css('left');
    },


    setHeight: function (v) {
        this.setSize(undefined, v);
    },

    setWidth: function (v) {
        this.setSize(v);
    },


    getWidth: function (includeMargin) {
        var me = this,
            el = me.el;

        if (true === includeMargin) {
            return el.outerWidth(includeMargin);
        }
        else {
            return el.outerWidth();
        }
    },

    getHeight: function (includeMargin) {
        var me = this,
            el = me.el;

        if (true === includeMargin) {
            return el.outerHeight(includeMargin);
        }
        else {
            return el.outerHeight();
        }
    },

    setSize: function (w, h) {
        //throw new Error('未实现');
    },

    getSize: function (includeMargin ){
        var me = this;

        if (true === includeMargin) {
            return {
                width: me.getWidth(includeMargin),
                height: me.getHeight(includeMargin)
            };
        }
        else {
            return {
                width: me.getWidth(),
                height: me.getHeight()
            };
        }
    },
    
    /**
    * 隐藏组件
    */
    hide: function () {
        var me = this,
            el = me.el;

        me.visible = false;

        if (el) {
            el.hide();
        }

        me.callParent();

        return me;
    },

    /**
    * 显示组件
    */
    show: function () {
        var me = this,
            el = me.el;

        me.visible = true;

        if (el) {
            el.show();
        }

        me.callParent();

        return me;
    },

    /**
    * 设置组件可视状态
    * @param {bool} value 
    */
    setVisible: function (value) {
        "use strict";
        var me = this;
        value = !!value;
        me.visible = value;

        if (value) {
            me.show();
        }
        else {
            me.hide();
        }

        return me;
    },

    /**
    * 获取组件可视状态
    * @return {bool} 
    */
    getVisible: function () {
        "use strict";
        var me = this;

        return me.visible;
    },

    /**
    * 获取无效
    */
    setDisabled: function (value) {
        "use strict";
        var me = this;

        me.disabled = !!value;


    },

    getDisabled:function(){
        var me = this;

        return me.disabled;
    },




    onLostFocus: function () {


        //console.log("失去焦点");
    },


    /**
    * 重置层次
    */
    resetPaint: Mini2.emptyFn,

    /**
    * 触发焦点事件
    */
    onFocus: Mini2.emptyFn,


    focus: Mini2.emptyFn,

    dispose: Mini2.emptyFn

}, function () {
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});