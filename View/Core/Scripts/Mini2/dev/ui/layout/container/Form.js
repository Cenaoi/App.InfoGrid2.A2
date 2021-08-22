

/// <reference path="../Mini.js" />
/// <reference path="../../../jquery/jquery-1.4.1-vsdoc.js" />



Mini2.define("Mini2.ui.layout.container.Form", {


    targetCls: Mini2.baseCSSPrefix + 'form-layout-ct',

    itemCls: [Mini2.baseCSSPrefix + 'form-item', Mini2.baseCSSPrefix + 'box-item'],

    type: 'form',

    isFormLayout: true,


    //初始化组件
    initComponent: function () {
        var me = this;

        me.length = 0;


    },

    configureItem: function (item) {
        "use strict";
        var me = this,
            itemCls = me.itemCls,
            ownerItemCls = me.owner.itemCls,
            addClasses;

        item.ownerLayout = me;

        if (addClasses) {
            item.addCls(addClasses);
        }
    },

    updateLayout: function (owner) {
        var me = this;

        //console.log(me.owner.flowDirection);

    },

    beginLayout: function (ownerContext) {

        console.log(me.owner.flowDirection);

    },


    getLayoutItems: function () {


    }

});