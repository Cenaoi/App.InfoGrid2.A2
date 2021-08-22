Mini2.define('Mini2.flow.ac3.Note', {


    extend: 'Mini2.flow.Action',

    //标签
    isNote: true,

    isHover: false,

    className:'note-default',

    initComponent: function () {
        var me = this;

        me.proClassName();

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


    createTextObj: function () {
        "use strict";
        var me = this,
            textObj,
            pseudoName = (me.isHover ? 'hover' : false),
            font = me.css('font', pseudoName),
            color = me.css('color', pseudoName),
            g;


        textObj = new createjs.Text(me.text, font, color);

        textObj.x = me.width / 2;
        textObj.y = me.height / 2;
        //textObj.width = me.width;
        //textObj.height = me.height;
        textObj.textAlign = 'center';
        textObj.textBaseline = 'middle';

        return textObj;
    },

    drawReset: function (graphics, exWidth, exHeight) {
        "use strict";
        var me = this;

        me.draw_default(graphics, exWidth, exHeight);
        

        return me;
    },

    //绘制默认图形
    draw_default: function (graphics, exWidth, exHeight) {

        "use strict";
        var me = this,
            boxObj = me.boxObj,
            txtObj = me.txtObj,
            width = exWidth || me.width,
            height = exHeight || me.height,
            g = graphics || boxObj.graphics,            
            pseudoName = (me.isHover ? 'hover' : false);


        var dw = 14.5;

        var x1 = width - dw;
        var y1 = 0;

        var x2 = width;
        var y2 = dw;

        var x3 = width - dw;
        var y3 = dw;


        var bgColor = me.css('background-color', pseudoName);
        var borderColor = me.css('border-color', pseudoName);
        var borderWidth = me.css('border-width', pseudoName);


        if (txtObj) {
            txtObj.color = me.css('color', pseudoName);

        }
        

        g.clear();

        g.beginFill(bgColor);
        g.beginStroke(borderColor)
        g.setStrokeStyle(borderWidth);


        g.moveTo(x1, y1);
        g.lineTo(x2, y2);
        g.lineTo(width, height);
        g.lineTo(0, height);
        g.lineTo(0, 0);
        g.closePath();

        g.beginFill(bgColor);
        g.beginStroke(borderColor)
        g.setStrokeStyle(borderWidth);

        g.moveTo(x1, y1);
        g.lineTo(x3, y3);
        g.lineTo(x2, y2);
        g.closePath();

        //g.beginFill("f00").drawCircle(w2, w2, w2);

        return me;
    }

});