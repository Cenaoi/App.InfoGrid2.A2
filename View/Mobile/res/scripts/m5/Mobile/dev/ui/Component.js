


//弹出窗体
Mini2.define('Mini2.ui.Component', {

    extend: false,

    el: null,

    isComponent: true,

    renderTo: Mini2.getBody(),

    initComponent: Mini2.emptyFn,

    onInit: Mini2.emptyFn,

    render: function () {
        var me = this;

        me.baseRender();
    }

}, function () {
    var me = this;

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});