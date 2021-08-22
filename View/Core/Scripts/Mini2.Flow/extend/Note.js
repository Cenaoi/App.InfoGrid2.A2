Mini2.define('Mini2.flow.extend.Note', {


    extend: 'Mini2.flow.Action',

    //标签
    isNote : true,

    initComponent:function(){
        var me = this;

        me.style['0'].font_color = '#000000';


        me.initComponentBase();
    },

    rollover: function () {
        var me = this;

        //me.baseRollover();

        return me;
    },


    rollout: function () {
        var me = this;

        // me.baseRollout();

        return me;
    },


    setText: function (txt) {
        var me = this,
            txtObj = me.txtObj;

        me.text = txt;
        txtObj.text = txt;

        return me;
    },




    drawReset: function (graphics, exWidth, exHeight) {
        "use strict";
        var me = this,
            style_name = me.style_name;

        if (style_name == 'no-bg') {
            me.draw_nobg(graphics, exWidth, exHeight);
        }
        else {
            me.draw_default(graphics, exWidth, exHeight);
        }

        return me;
    },

    //绘制默认图形
    draw_default: function (graphics, exWidth, exHeight) {

        "use strict";
        var me = this,
            style = me.curStyle,
            boxObj = me.boxObj,
            width = exWidth || me.width,
            height = exHeight || me.height,
            g = graphics || boxObj.graphics;


        var dw = 14.5;

        var x1 = width - dw ;
        var y1 = 0;

        var x2 = width;
        var y2 = dw;

        var x3 = width - dw;
        var y3 = dw;

        g.clear();

        g.beginFill(createjs.Graphics.getRGB(255, 247, 153));
        g.beginStroke(createjs.Graphics.getRGB(60, 60, 60))
        g.setStrokeStyle(1);


        g.moveTo(x1, y1);
        g.lineTo(x2, y2);
        g.lineTo(width, height);
        g.lineTo(0, height);
        g.lineTo(0, 0);
        g.closePath();

        g.beginFill(createjs.Graphics.getRGB(255, 247, 153));
        g.beginStroke(createjs.Graphics.getRGB(60, 60, 60))
        g.setStrokeStyle(1);
        g.moveTo(x1, y1);
        g.lineTo(x3, y3);
        g.lineTo(x2, y2);
        g.closePath();

        //g.beginFill("f00").drawCircle(w2, w2, w2);

        return me;
    },

    //绘制没有背景的样式
    draw_nobg: function (graphics, exWidth, exHeight) {
        "use strict";
        var me = this,
            style = me.curStyle,
            txtObj = me.txtObj,
            boxObj = me.boxObj,
            width = exWidth || me.width,
            height = exHeight || me.height,
            g = graphics || boxObj.graphics;


        var bounds = txtObj.getBounds();

        console.log(bounds);


        g.clear();

        g.beginFill(createjs.Graphics.getRGB(255, 255, 255, 1));
        //g.beginStroke(createjs.Graphics.getRGB(0, 0, 0))
        //g.setStrokeStyle(0);
        
        g.drawRect(0, 0, bounds.width, bounds.height);

        g.endFill();

        return me;
    }


});