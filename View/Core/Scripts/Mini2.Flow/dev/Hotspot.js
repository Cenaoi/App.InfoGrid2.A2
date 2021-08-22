/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />


//热点
Mini2.define('Mini2.flow.Hotspot', {

    extend: 'Mini2.flow.FlowComponent',


    x: 0,
    y: 0,

    width : 20,
    height: 20,

    //开始线段的头
    lineStartPoints: false,

    //结束线段的头
    lineEndPoints: false,


    initComponent: function () {
        "use strict";
        var me = this,
            el;

        ////开始线段的头
        //me.lineStartPoints = [];

        ////结束线段的头
        //me.lineEndPoints = [];


        me.srcRect = Mini2.apply({
            x: 0,
            y: 0,
            w: 10,
            h: 10
        }, me.srcRect);

        me.el = el = new createjs.Shape();
        el.x = me.x;
        el.y = me.y;

        //el.regX = me.width / 2;
        //el.regY = me.height / 2;

        el.rotation = me.rotation;



        el.addEventListener("rollover", function (evt) {
            
            me.rollover_draw();

            me.on('rollover', {
                hotId: me.id,
                hotspot: me
            });
        });



        el.addEventListener("rollout", function (evt) {

            me.rollout_draw();

            me.on('rollout', {
                hotId: me.id,
                hotspot: me
            });

        });

    },



    //线段连接
    link: function (pointType, line) {
        "use strict";
        var me = this,
            i,
            srcLine = null,
            lines;

        if ('start' == pointType) {
            lines = me.lineStartPoints = me.lineStartPoints || [];
        }
        else if ('end' == pointType) {
            lines = me.lineEndPoints = me.lineEndPoints || [];
        }
        
        for (i = 0; i < lines.length; i++) {
            if (lines[i] === line) {
                srcLine = line;
                break;
            }
        }

        if (srcLine == null) {
            lines.push(line);
        }

        return me;
    },

    //断开线段连接
    unlink: function (pointType, line) {
        "use strict";
        var me = this,
            i,
            srcLine = null,
            lines;

        if ('start' == pointType) {
            lines = me.lineStartPoints;
        }
        else if ('end' == pointType) {
            lines = me.lineEndPoints;
        }

        if (lines) {
            for (i = 0; i < lines.length; i++) {
                if (lines[i] === line) {
                    srcLine = line;
                    break;
                }
            }

            if (srcLine) {
                lines.splice(i, 1);
            }
        }

        return me;
    },

    //断开全部
    unlinkAll: function () {
        "use strict";
        var me = this,
            i,
            line,
            lsPs = me.lineStartPoints,
            lePs = me.lineEndPoints;

        console.debug('断开全部....');

        console.debug('lsPs = ', lsPs);
        console.debug('lePs = ', lePs);

        if (lsPs) {
            for (i = 0; i < lsPs.length; i++) {
                line = lsPs[i];
                me.unlink('start', line);
                line.start_point = null;
            }
        }

        if (lePs) {
            for (i = 0; i < lePs.length; i++) {
                line = lePs[i];
                me.unlink('end', line);
                line.end_point = null;
            }
        }

        me.lineStartPoints = [];
        me.lineEndPoints = [];

    },


    render: function () {
        var me = this;

        me.drawReset();

        me.Mini2_Object = me;
    },


    drawReset: function () {
        var me = this;

        me.rollout_draw();
    },


    //移进的绘制
    rollover_draw: function () {
        "use strict";
        var me = this,
            el = me.el,
            g = el.graphics,
            width = me.width,
            height = me.height,
            w2 = width / 2,
            h2 = height / 2;


        g.clear();

        g.setStrokeStyle(2);

        g.beginStroke( "#000000");
        g.beginFill(createjs.Graphics.getRGB(255, 255, 255, 0.01));
        g.drawCircle(0, 0, w2);

    },

    //移出的绘制
    rollout_draw: function () {
        "use strict";
        var me = this,
            el = me.el,
            g = el.graphics,
            width = me.width,
            height = me.height,
            w2 = width / 2,
            h2 = height / 2;




        g.clear();

        g.setStrokeStyle(0);

        g.beginStroke(createjs.Graphics.getRGB(255, 255, 255, 0.01));
        g.beginFill(createjs.Graphics.getRGB(0, 255, 255, 0.01));
        g.drawCircle(0, 0, w2);

    },

    remove: function () {
        "use strict";
        var me = this,
            el = me;

        el.removeAllEventListeners();

        
        delete me.el;

        delete me.lineStartPoints;

        delete me.lineEndPoints;

        delete me.Mini2_Object;
    }


}, function () {
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});