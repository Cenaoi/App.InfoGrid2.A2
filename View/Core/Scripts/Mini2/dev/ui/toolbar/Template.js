

/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../Mini.js" />

/**
* 工具栏模板项目
* 
*/
Mini2.define('Mini2.ui.toolbar.Template', {

    extend: 'Mini2.ui.Component',

    renderTpl: [
        '<div class="mi-box-item mi-toolbar-item mi-toolbar-template" ',
            'style="right: auto; top: 5px; margin: 0px;">',



        '</div>'
    ],


    /**
    * 模板内容
    *
    */
    content: null,

    getInputEl: function () {
        "use strict";
        var me = this,
            el;

        el = Mini2.$joinStr(me.renderTpl);

        el.html(me.content);

        if (me.id) {
            $(el).attr('id', me.id);
        }

        if (!me.visible) {
            el.css('display', 'none');
        }

        return el;
    },


    getWidth: function () {
        var me = this,
            el = me.el;

        return $(el).outerWidth();
    },

    render: function () {
        "use strict";
        var me = this,
            el;

        me.el = el = me.getInputEl();

        if (me.applyTo) {
            $(me.applyTo).replaceWith(el);
        }
        else {
            $(me.renderTo).append(el);
        }

        $(el).data('me', me);

        try {
            Mini2.ui.SecManager.reg(me.secFunCode, me);
        }
        catch (ex) {
            console.error('注册权限错误', ex);
        }

        return me;
    }

});
