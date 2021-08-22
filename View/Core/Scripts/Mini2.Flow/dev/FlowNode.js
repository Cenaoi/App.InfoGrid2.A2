/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

// 流程图的节点

Mini2.define('Mini2.flow.FlowNode', {
    
    extend: 'Mini2.flow.FlowComponent',

    isFlowNode: true,

    text: '节点',


    //宽度锁
    widthLock: false,
    
    //高度锁
    heightLock: false,

    
    //样式名称
    style_name:'',



    setStyleName :function( value){
        var me = this;
        me.style_name = value;

        return me;
    },



    //el:null,

    initComponent: function () {
        "use strict";
        var me = this,
            el;


        //var bitmap = new createjs.Bitmap("http://www.baidu.com/img/baidu_jgylogo3.gif");
        //bitmap.x = 20;
        //bitmap.y = 200;



        //bitmap.addEventListener("rollover", function (evt) {
        //    console.log("bitmap 进入");
        //});



        //bitmap.addEventListener("rollout", function (evt) {

        //    console.log("bitmap 出来");
        //});



        //bitmap.addEventListener("mouseout", function (evt) {

        //    console.log("bitmap mouseout");
        //});


        //bitmap.addEventListener("mousedown", function (evt) {

        //    console.log("bitmap 点击...");

        //});






        //me.el = bitmap;
    },


    baseRender: function () {
        var me = this;

    },



    render: function () {
        var me = this;

        me.baseRender();

    }
    

}, function () {
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});