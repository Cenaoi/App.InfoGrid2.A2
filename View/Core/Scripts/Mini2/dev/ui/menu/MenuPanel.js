/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../Mini.js" />



//菜单
Mini2.define('Mini2.ui.menu.MenuPanel', {


    extend: 'Mini2.ui.Component',



    //初始化组件
    initComponent: function () {

        //        log.debug("Mini2.ui.Component.initComponent()");

    },

    contentEl:null,


    item_click: function (menuItem) {
        var me = this;

        me.hide();
    },


    callShow: function () {
        var me = this,
            el = me.el;

        me.trigger('showing');
        el.show();
        me.trigger('showed');

        return me;
    },


    callHide: function () {
        var me = this,
            el = me.el;

        if (me.visible) {
            el.hide();

            me.trigger('hidded');
        }

        return me;
    },


    /**
    * 延迟显示
    *
    */
    delayShow: function () {
        var me = this;
        Mini2.ui.menu.Manager.show(me);

        return me;
    },

    show: function () {
        var me = this;

        me.callShow();

        return me;
    },


    hide: function () {
        var me = this,
            el = me.el;

        if (me.visible) {
            el.hide();

            me.trigger('hidded');
        }

        return me;
    },




    baseRender: function () {
        var me = this,
            el;

        me.el = el = $(me.contentEl);
        

        el.css({
            'top': me.top,
            'left': me.left
        });
                

        $(el).mousedown(function (e) {

            //log.debug("form.field.Trigger.mousedown() ");

            //Mini2.EventManager.setScope(this, [me.el, me.dropDownPanel]);
        })
        .mousemove(function (e) {

        })
        .mouseup(function (e) {
            //console.log("form.field.Trigger.mouseup() ");

            Mini2.EventManager.stopEvent(e);
            Mini2.EventManager.clear(me.scope || me.el);

        });


        Mini2.ui.menu.Manager.register(me);

        return me;
    },


    render: function () {
        var me = this;

        me.baseRender();

        me.show();

        return me;
    }


});