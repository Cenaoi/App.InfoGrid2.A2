Mini2.define('Mini2.flow.extend.StartNode', {


    extend: 'Mini2.flow.Action',

    isStartNode: true,

    minWidth: 32,
    maxWidth: 32,

    minHeight: 32,
    maxHeight: 32,

    //宽度锁
    widthLock: true,

    //高度锁
    heightLock: true,




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
    },

    createTextObj: function () {

        "use strict";
        var me = this,
            style = me.curStyle || me.style['0'],
            textObj,
            g;

        textObj = new createjs.Text('', '9px 微软雅黑', style.font_color);

        textObj.x = me.width / 2;
        textObj.y = me.width / 2;
        //textObj.width = me.width;
        //textObj.height = me.height;
        textObj.textAlign = 'center';
        textObj.textBaseline = 'middle';

        //textObj.setBounds(0, 0, me.width, me.height);

        return textObj;
    },




    drawReset: function (graphics, exWidth, exHeight) {
        "use strict";
        if (!this.boxObj){
            return this;
        }
        var me = this,
            style = me.curStyle,
            boxObj = me.boxObj,

            width = exWidth || me.width,
            height = exHeight || me.height,
            g = graphics || boxObj.graphics;

        var w2 = width / 2;

        g.clear();

        g.beginFill(createjs.Graphics.getRGB(255, 255, 255));
        g.drawCircle(w2, w2, w2 - 2);

        if (me.step_count > 0) {
            g.beginFill(createjs.Graphics.getRGB(100, 100, 100, 1));
        }
        else {
            g.beginFill(createjs.Graphics.getRGB(0, 0, 0, 1));
        }

        g.drawCircle(w2, w2, w2 - 4);

        return me;
    }


});