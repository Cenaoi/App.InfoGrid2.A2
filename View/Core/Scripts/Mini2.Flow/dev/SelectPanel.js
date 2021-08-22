/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

//矩形选择层
Mini2.define('Mini2.flow.SelectPanel', {

    extend: 'Mini2.flow.FlowComponent',
    
    
    x: 0,

    y: 0,

    width: 100,

    height: 100,

    //背景颜色
    backColor: createjs.Graphics.getRGB(216, 216, 216, 0.5),


    initComponent: function () {
        "use strict";
        var me = this,
            el;

        me.el = el = me.createRect();

        el.visible = false;
    },


    setVisible: function (value) {
        "use strict";
        var me = this,
            el = me.el;

        //el.visible = value;

        if (value != el.visible) {

            if (el.visible && !value) {
                createjs.Tween.get(el).to({ alpha: 0 }, 200).call(function () {
                    el.visible = false;
                });
            }
            else {
                el.alpha = 1;
                el.visible = true;
            }
        }

        return me;
    },

    render: function () {
        var me = this;


    },


    setPoint0: function (x, y) {
        "use strict";
        var me = this,
            el = me.el,
            g = el.graphics;

        me.setXY(x, y);

        me.x0 = me.x1 = x;
        me.y0 = me.y1 = y;

        g.clear();

        //me.width = el.width = 0;
        //me.height = el.height = 0;

        return me;
    },

    setPoint1: function (x, y) {
        "use strict";
        var me = this;

        me.setSize(x - me.x, y - me.y);        

        me.resetRest();

        if (x < me.x) {
            me.x0 = x;
            me.x1 = me.x;
        }
        else {
            me.x1 = x;
        }

        if (y < me.y) {
            me.y0 = y;
            me.y1 = me.y;
        }
        else {
            me.y1 = y;
        }

        return me;
    },

    //创建虚线矩形
    createRect: function () {
        "use strict";
        var me = this,
            rect;

        rect = new createjs.Shape();
        rect.x = me.x;
        rect.y = me.y;
        rect.width = me.width;
        rect.height = me.height;

        return rect;
    },



    resetRest: function () {
        "use strict";
        var me = this,
            el = me.el,
            width = me.width,
            height = me.height,
            g;

        el.x = me.x;
        el.y = me.y;

        g = el.graphics;


        g.clear();
        g.setStrokeStyle(1);

        g.beginStroke("#ababab");
        g.beginFill(me.backColor).drawRect(0.5, 0.5, width, height);


        return me;
    }

}, function () {
    "use strict";
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});