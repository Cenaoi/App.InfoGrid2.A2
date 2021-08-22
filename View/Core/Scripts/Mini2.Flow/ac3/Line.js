Mini2.define('Mini2.flow.ac3.Line', {


    extend: 'Mini2.flow.Line',

    step_count: 0,

    is_preview: false,

    labelVisible: true,


    initComponent: function () {
        "use strict";
        var me = this;
        me.className = 'line-default line-peter-river';
        me.proClassName();
        me.initComponentBase();
    },


    setText: function(value){
        var me = this;

        if (me.textObj && me.labelVisible) {

            me.textObj.text = value;

            me.desc = value;

            me.resetDraw();
        }


    },

    draw: function (graphics) {
        "use strict";
        var me = this,
            el = me.el,
            lineObj = me.lineObj,
            g = graphics || lineObj.graphics;


        //g.setStrokeStyle(me.size, 'round');
        //g.beginStroke('#5b9bd5');

        me.drawLine(g);

        return me;
    },


    // 重新绘制
    resetDraw: function () {
        var me = this,
            g,
            pseudoName = (me.isHover ? 'hover' : false),
            lineObj = me.lineObj;

        g = lineObj.graphics;


        g.clear();


        if (me.isHover) {

            g.setStrokeStyle(8, "round");
            g.beginStroke(createjs.Graphics.getRGB(56, 181, 73, 0.5));
            me.drawLine(g);



            g.setStrokeStyle(6, "round");
            g.beginStroke(createjs.Graphics.getRGB(255, 255, 255, 1));

            me.drawLine(g);

        }
        else {
            g.setStrokeStyle(8, "round");
            g.beginStroke(createjs.Graphics.getRGB(255, 255, 255, 0.2));
            me.drawLine(g);
        }

        

        g.setStrokeStyle(me.css('border-width', pseudoName), "round");
        g.beginStroke(me.css('border-color', pseudoName));

        me.drawLine(g);

        g.endStroke();



        var xy = me.getCentrePoint();

        if (me.textObj && me.labelVisible) {
            var dd2 = me.textObj.getBounds();

            if (dd2) {
                var rectX = dd2.x + xy.x - 2,
                    rectY = dd2.y + xy.y - 2,
                    rectW = dd2.width + 4,
                    rectH = dd2.height + 4;

                g.beginFill('#FFFFFF');
                g.drawRect(rectX, rectY, rectW, rectH);
            }
        }


        return me;
    },



    //获取配置信息, json 格式
    getConfig: function () {
        var me = this;

        var psStr = me.pointsToStr(me.points);


        //console.debug('startAnchor : ', me.startAnchor);


        var t = {
            item_id: me.item_id,
            item_code: me.item_code,

            item_type: 'line',
            item_fullname: 'Mini2.flow.ac3.Line',

            style_name: me.style_name,

            className:me.className,

            desc: me.desc,
            x: me.x,
            y: me.y,

            size: 2,

            start_point: me.start_point,
            end_point: me.end_point,


            points_str: psStr
        };


        return t;

    }


});