


Mini2.define('Mini2.flow.ac3.Node', {

    extend: 'Mini2.flow.Action',

    isHover : false,

    state: 2,



    step_count: 0,

    is_preview: false,


    initComponent: function () {
        var me = this,
            el ;



        if (me.ex_node_type == 'listen_table') {
            me.className = 'node-default node-sun-flower';
        }
        else if (me.ex_node_type == 'operate_table') {
            me.className = 'node-default node-peter-river';
        }

        me.proClassName();

        me.initComponentBase();

        el = me.el;

        var icoUrl = null;
        var icon = null;

        if (me.ex_node_type == 'listen_table') {
            icoUrl = '/Core/Scripts/Mini2.Flow/Themes/ac3/Images/lightning.png';
        }
        else if (me.ex_node_type == 'operate_table') {
            icoUrl = '/Core/Scripts/Mini2.Flow/Themes/ac3/Images/gear.png';
        }

        if (icoUrl) {
            icon = new createjs.Bitmap(icoUrl);
            icon.scaleX = 0.25;
            icon.scaleY = 0.25;
            icon.x = 8;
            icon.y = 8;
            el.addChild(icon);
        }


        try {
            me.setSize(me.width, me.height);
        }
        catch (ex) {
            console.error('初始化错误', ex);
        }

    },


    getRectX: function(){
        var me = this,
            sx = 0,
            rect = {
                x: sx,
                y: sx,
                width: me.width - sx * 2,
                height:me.height - sx * 2
            };

        return rect;
    },

    rollover: function () {
        var me = this;

        me.isHover = true;
        me.drawReset();

        return me;
    },


    rollout: function () {
        var me = this;

        me.isHover = false;
        me.drawReset();

        return me;
    },




    //获取状态
    setState: function (value) {
        "use strict";
        var me = this;

        return me;
    },






    drawReset: function (graphics, exWidth, exHeight) {
        "use strict";
        var me = this,
            txtObj = me.txtObj,
            boxObj = me.boxObj,
            width = exWidth || me.width,
            height = exHeight || me.height,
            rect = me.getRectX(),
            pseudoName = (me.isHover ? 'hover' : false),
            g;


        g = graphics || boxObj.graphics;

        txtObj.color = me.css('color', pseudoName);


        g.clear();
        
        g.setStrokeStyle(me.css('border-width',pseudoName));
        g.beginStroke(me.css('border-color', pseudoName));

        //if ('dashed' == style.border_style) {
        //    g.setStrokeDash([4, 4]);
        //}

        g.beginFill(me.css('background-color', pseudoName));             


        var borderRadius = me.css('border-radius', pseudoName);

        if (borderRadius) {
            g.drawRoundRect(rect.x, rect.y, rect.width, rect.height, borderRadius);
        }
        else {
            g.drawRect(rect.x, rect.y, rect.width, rect.height);
        }

        return me;
    }


});