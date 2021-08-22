

Mini2.define('Mini2.flow.GhostItem', {


    extend: 'Mini2.flow.FlowComponent',

    //原对象
    //src: null,

    //克隆的对象
    //el: null,

    //相对 x 轴
    //rX:0,

    //相对 y 轴
    //rY: 0,

    //srcWidth:0

    //srcHeight:0,


    initComponent: function () {
        "use strict";
        var me = this,
            src = me.src,
            aLocal = src.getALocal(),
            el;

        me.el = el = new createjs.Shape();
        el.Mini2_Object = me;


        me.width = el.width = src.width;
        me.height = el.height = src.height;

        me.x = el.x = aLocal.x;
        me.y = el.y = aLocal.y;

    },


    drawReset: function () {
        "use strict";
        var me = this,
            el = me.el,
            g = el.graphics,
            src = me.src;

        if (src.drawReset) {
            src.drawReset(g, me.width, me.height);
        }
        else {
            console.log("没有 drawReset 函数");
        }
    },


    render: function () {
        var me = this;

        me.drawReset();
    }

}, function () {
    var me = this;

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});

