Mini2.define('Mini2.flow.extend.AutoNode', {


    extend: 'Mini2.flow.Action',

    isAutoNode: true,

    state: 1,


    step_count: 0,

    is_preview: false,

    getRectX: function () {
        var me = this,
            sx = 4,
            rect = {
                x: sx,
                y: sx,
                width: me.width - sx * 2,
                height: me.height - sx * 2
            };

        return rect;
    },

    rollover: function () {
        var me = this;

        me.baseRollover();

        return me;
    },


    rollout: function () {
        var me = this;

        me.baseRollout();

        return me;
    },



    baseRollover: function () {
        var me = this,
            boxObj = me.boxObj,
            step_count = me.step_count,
            g = boxObj.graphics,
            rect = me.getRectX(),
            style = me.curStyle;

        if (!me.is_preview || step_count > 0) {
            g.beginFill(style.back_color_over);
        }
        else {
            g.beginFill('#e8e8e8');
        }

        g.drawRoundRect(rect.x, rect.y, rect.width, rect.height, 4);

        return me;
    },


    baseRollout: function () {
        var me = this,
            boxObj = me.boxObj,
            step_count = me.step_count,
            g = boxObj.graphics,
            rect = me.getRectX(),
            style = me.curStyle;

        if (!me.is_preview || step_count > 0) {
            g.beginFill(style.back_color);
        }
        else {
            g.beginFill('#e8e8e8');
        }

        g.drawRoundRect(rect.x, rect.y, rect.width, rect.height, 4);

        return me;
    },

    //获取状态
    setState: function (value) {
        "use strict";
        var me = this,
            txtObj = me.txtObj,
            state = me.state = value,
            style;

        style = me.style[state];


        me.curStyle = style;

        me.drawReset();


        //txtObj.x = me.width / 2;
        //txtObj.y = me.height / 2;
        if (txtObj) {
            if (!me.is_preview || me.step_count > 0) {
                txtObj.color = '#FFFFFF';
            }
            else {
                txtObj.color = '#333333';
            }

        }

        return me;
    },

    createTextObj: function () {
        "use strict";
        var me = this,
            step_count = me.step_count,
            style = me.curStyle || me.style['0'],
            textObj,
            g;


        if (!me.is_preview || me.step_count > 0) {
            textObj = new createjs.Text(me.text, '10pt 微软雅黑', '#FFFFFF');
        }
        else {
            textObj = new createjs.Text(me.text, '10pt 微软雅黑', '#333333');
        }

        textObj.x = me.width / 2;
        textObj.y = me.height / 2;

        textObj.textAlign = 'center';
        textObj.textBaseline = 'middle';



        //textObj.setBounds(0, 0, me.width, me.height);

        return textObj;
    },





    drawReset: function (graphics, exWidth, exHeight) {
        "use strict";
        var me = this,
            step_count = me.step_count || 0,
            style = me.curStyle,
            boxObj = me.boxObj,
            width = exWidth || me.width,
            height = exHeight || me.height,
            rect = me.getRectX(),
            g;


        g = graphics || boxObj.graphics;

        g.clear();

        if (!me.is_preview || step_count > 0) {
            g.setStrokeStyle(2);
            g.beginStroke('#365422');

            g.beginFill(style.back_color);


        }
        else {
            g.setStrokeStyle(style.border_width);
            g.beginStroke(createjs.Graphics.getRGB(202, 201, 199, 1));


            if ('dashed' == style.border_style) {
                g.setStrokeDash([4, 4]);
            }

            g.beginFill('#e8e8e8');

        }

        g.drawRect(rect.x, rect.y, rect.width, rect.height);


        return me;
    }

});