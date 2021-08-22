
//菜单分隔符
Mini2.define('Mini2.ui.menu.Separator', {

    renderTpl: [
        '<div class="mi-component mi-box-item mi-component-default mi-menu-item-separator mi-menu-item mi-menu-item-plain" ',
            'style="border-width: 1px; right: auto; left: 0px; top: 50px; margin: 0px; width:100%;" >&nbsp;</div>'
    ],

    isSeparator :true,

    getHeight: function () {
        var me = this,
            el = me.el;

        return 5;
    },

    getWidth:function(){
        var me = this,
            el = me.el;

        return el.width();
    },

    baseRender: function () {
        var me = this,
            el;

        me.el = el = Mini2.$joinStr(me.renderTpl);

        el.css('top', me.top);

        $(me.renderTo).append(el);
    },


    render: function () {

        var me = this;

        me.baseRender();

    }

});