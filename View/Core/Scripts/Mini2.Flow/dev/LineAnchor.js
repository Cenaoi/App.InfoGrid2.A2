/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

Mini2.define('Mini2.flow.LineAnchor', {

    //extend: 'Mini2.flow.FlowNode',


    x: 0,

    y: 0,

    width: 10,

    height: 10,

    points: [],

    anchorType: '',


    initComponent: function () {
        "use strict";
        var me = this,
            el;


        me.el = el = new createjs.Shape();
        el.x = me.x;
        el.y = me.y;
        el.rotation = me.rotation;

        el.regX = me.width / 2;
        el.regY = 0;
    },


    render: function () {
        var me = this;

        me.draw1();
    },

    //设置旋转角度
    setRotation:function(value){
        var me = this,
            el = me.el;

        el.rotation = value;

        return me;
    },

    setXY : function(x,y){
        var me = this,
            el = me.el;

        el.x = x;
        el.y = y;

        return me;
    },

    setX:function(value){
        var me = this;

        me.el.x = value;

        return me;
    },

    setY : function(value){
        var me = this;

        me.el.x = value;

        return me;
    },

    setSize:function(width,height){
        var me = this,
            el = me.el;

        if (undefined != width) {
            el.width = width;
            me.width = width;
        }

        if (undefined != height) {
            el.height = height;
            me.height = height;
        }

        me.draw1();
    },

    draw1: function () {
        "use strict";
        var me = this,
            el = me.el,
            g = el.graphics,
            width = me.width,
            height = me.height,
            w2 = width / 2;
        
        g.clear();

        g.beginFill('#448CCB');
        g.moveTo(0, height);
        g.lineTo(w2, 0);
        g.lineTo(width, height);

        g.closePath();


        //g.setStrokeStyle(1);
        //g.beginStroke("#FF0000");

        //g.moveTo(0, height);
        //g.lineTo(w2, 0);
        //g.lineTo(width, height);

    }



}, function () {
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});