

Mini2.define('Mini2.ui.Preloader', {



    onInit: function () {

    },


    baseRender: function () {
        var me = this;

        var el = $('<div class="preloader-indicator-overlay"></div><div class="preloader-indicator-modal"><span class="preloader preloader-white"></span></div>');

        $(me.renderTo).append(el);

        me.el = el;
    },


    open: function () {
        var me = this,
            el = me.el;


        return me;
    },


    close: function () {
        var me = this,
            el = me.el;

        el.remove();

        return me;
    },


    render: function () {
        var me = this;

        me.baseRender();

        me.open();

    }

});

//显示一个消息，会在2秒钟后自动消失
$.preloader = function (text) {

    if (text) {
        var frm = Mini2.create('Mini2.ui.Window', {
            title:text,
            contentHtml: '<div class="preloader"></div>'
        });

        frm.render();

        return frm;
    }
    else {
        var frm = Mini2.create('Mini2.ui.Preloader', {});

        frm.render();

        return frm;
    }
};