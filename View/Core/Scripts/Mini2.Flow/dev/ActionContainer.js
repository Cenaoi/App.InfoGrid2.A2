

/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

//容器
Mini2.define('Mini2.flow.ActionContainer', {

    extend: 'Mini2.flow.FlowNode',

    isContainer : true, // 容器的标记

    //容器提示的绿色边框
    shape: null,



    //线段集合
    lines: false,

    //图像集合
    shapes: false,



    initComponent: function () {
        "use strict";
        var me = this,
            el,
            shape;

        me.el = el = new createjs.Container();
        el.x = me.x;
        el.y = me.y;
        el.visible = me.visible;

        el.Mini2_Object = me;

        me.shape = shape = new createjs.Shape();
        shape.x = 0;
        shape.y = 0;
        shape.alpha = me.maskAlpha;
        
        el.addChild(shape);


        me.drawReset();
    },

    addChild: function (item) {
        "use strict";
        var me = this,
            shapes = me.shapes = me.shapes ||[],
            el = me.el;

        console.log("ActionContainer.addChild(...)");

        console.log(item);


        item.parent = me;

        el.addChild(item.el);


        shapes.push(item);


        return me;
    },

    removeChild: function (item) {
        "use strict";
        var me = this,
            el = me.el;


        console.log("ActionContainer.removeChild(...)");


        el.removeChild(item.el);


        Mini2.Array.remove(me.shapes, item);

        return me;
    },

    testRollover:function(x,y){
        var me = this,
            el = me.el,
            shape = me.shape;

        return el.hitTest(x, y);
    },


    drawReset: function () {
        "use strict";
        var me = this,
            shape = me.shape,
            g = shape.graphics,
            width = me.width,
            height = me.height;

        
        g.clear();

        g.setStrokeStyle(2);
        g.beginFill(createjs.Graphics.getRGB(255, 255, 255, 0.1));
        g.beginStroke(createjs.Graphics.getRGB(90, 200, 90, 1));
        
        g.drawRect(0, 0, width, height);
        
        return me;
    },



    //设置蒙城的透明度
    setMaskAlpha: function (value) {
        "use strict";
        var me = this,
            el = me.el,
            shape = me.shape;

        if (me.maskAlpha == value) {
            return;
        }

        me.maskAlpha = value;

        //shape.alpha = value;
        createjs.Tween.removeTweens(shape);

        createjs.Tween.get(shape).to({ alpha: value }, 500);

        return me;
    }


}, function () {
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});