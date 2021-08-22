
/**
* 加载提示框
*
*/
Mini2.define('Mini2.ui.Preloader', {


    extend: 'Mini2.ui.Component',

    tpl: [
        '<div class="preloader-indicator-overlay"></div>',
        '<div class="preloader-indicator-modal">',
        '<span class="preloader preloader-white"></span>',
        '</div>'
    ],

    onInit: function () {

    },


    baseRender: function () {
        var me = this,
            el = me.$join(me.tpl);

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

/**
* 显示一个消息，会在2秒钟后自动消失
*
* @param {string} text 消息内容
* @return {Object} 'Mini2.ui.Window' 对象 或 'Mini2.ui.Preloader'
*/
$.preloader = function (text) {
    var frm;

    if (text) {
        frm = Mini2.create('Mini2.ui.Window', {
            title:text,
            contentHtml: '<div class="preloader"></div>'
        });
    }
    else {
        frm = Mini2.create('Mini2.ui.Preloader', {});
    }

    frm.render();
    return frm;
};