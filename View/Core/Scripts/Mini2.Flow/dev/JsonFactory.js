/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

//热点
Mini2.define('Mini2.flow.JsonFactory', {

    extend: false,

    initComponent: function () {

    },


    getJson: function (flowPage) {
        var me = this,
            page = flowPage,
            data = {};




        return data;
    }


}, function () {
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});