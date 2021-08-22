

Mini2.define('Mini2.ui.form.field.QuickSearch', {

    extend: 'Mini2.ui.Component',

    tpl: [
        '<div class="mi-field-quicksearch">',
            '<input type="text" class="mi-field-search-input" placeholder="搜索">',
            '<span class="mi-field-search-btn"><i class="fa fa-search"></i></span>',
        '</div>'],

    clientId: null,

    id: null,


    //初始化组件
    initComponent: function () {
        "use strict";
        var me = this;


    },


    render: function () {

        var me = this,
            el;

        me.el = el = Mini2.$join(me.tpl);



        el.attr('data-muid', me.muid);
        el.data('me', me);
    }

});