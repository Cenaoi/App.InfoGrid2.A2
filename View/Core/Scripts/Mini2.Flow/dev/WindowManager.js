/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

//窗体管理, 提示框也属于这个窗体管理的子
Mini2.define('Mini2.flow.WindowManager', {

    //继承
    extend: false,
    

    items : false,

    initComponent: function () {
        "use strict";
        var me = this,
            el;

        me.items = [];

    },


    addChild: function (item) {
        "use strict";
        var me = this,
            el = me.el,
            items = me.items;

        items.push(item);

        el.addChild(item.el);
    },


    render: function () {
        var me = this,
            el = me.el;

        me.el = el = new createjs.Container();

    }




}, function () {
    "use strict";
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});