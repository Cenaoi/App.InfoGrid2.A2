
/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="/Core/Scripts/Mini2/Mini2.js" />


Mini2.define('Mini2.ui.filter.Item', {

    mixins: ['Mini2.EventBuilder'],

    //renderTo : Mini2.getBody(),
    renderTpl: '<div></div>',

    role: 'item',


    initComponentBase:function(){

    },

    initComponent: function () {
        var me = this;

        me.initComponentBase();
    },


    /**
    * 绑定事件
    *
    */
    bindEvent: function (el) {
        var me = this,
            closeEl;

        closeEl = el.children('.mi-filter-remove');
                
        $(closeEl).muBind('click', function () {

            me.remove();

            return false;
        });

    },


    remove: function () {
        var me = this,
            el = me.el;

        el.remove();
    },


    baseRender: function () {
        var me = this,
            el;

        me.el = el = $(me.renderTpl);

        me.textEl = el.children('.mi-filter-text');

        

        $(me.renderTo).append(el);

        //$('#mainBox_t2').append(el);


        me.bindEvent(el);
        


        
        return me;
    },


    render: function () {
        var me = this;

        me.baseRender();

        $(me.el).data('me', me);

        return me;
    },

    getJson: function () {


    }

}, function () {
    var me = this;


    Mini2.apply(me, arguments[0]);

    me.initComponent();

});
