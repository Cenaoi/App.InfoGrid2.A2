Mini2.define('Mini2.flow.extend.Line', {


    extend: 'Mini2.flow.Line',

    step_count: 0,

    is_preview: false,

    labelVisible : true,

    draw: function (graphics) {
        "use strict";
        var me = this,
            el = me.el,
            lineObj = me.lineObj,
            g = graphics || lineObj.graphics;


        g.setStrokeStyle(me.size, 'round');
        g.beginStroke('#5b9bd5');

        me.drawLine(g);

        return me;
    },


    // 重新绘制
    resetDraw: function () {
        var me = this,
            g,
            lineObj = me.lineObj;

        g = lineObj.graphics;


        if (me.is_preview) {

            if (me.step_count > 0) {
                if (me._isMouseOver) {

                    g.clear();

                    g.setStrokeStyle(8, "round");
                    g.beginStroke(createjs.Graphics.getRGB(56, 181, 73, 0.5));

                    me.drawLine(g);


                    g.setStrokeStyle(4, "round");

                    g.beginStroke("#448CCB");

                    me.drawLine(g);
                }
                else {
                    g.clear();

                    g.setStrokeStyle(4, "round");
                    g.beginStroke("#448CCB");


                    me.drawLine(g);
                }
            }
            else {

                g.clear();

                g.setStrokeStyle(1, "round");
                g.beginStroke("#959595");

                g.setStrokeDash([4, 4]);

                me.drawLine(g);
            }

            //me.endAnchor.setSize(20, 20);
        }
        else {
            if (me._isMouseOver) {

                g.clear();

                g.setStrokeStyle(8, "round");
                g.beginStroke(createjs.Graphics.getRGB(56, 181, 73, 0.5));

                me.drawLine(g);


                g.setStrokeStyle(2, "round");

                g.beginStroke("#448CCB");

                me.drawLine(g);
            }
            else {
                g.clear();

                g.setStrokeStyle(8, "round");
                g.beginStroke(createjs.Graphics.getRGB(255, 255, 255, 0.5));

                me.drawLine(g);


                g.setStrokeStyle(2, "round");
                g.beginStroke("#448CCB");


                if (!me.hasLinked()) {
                    g.setStrokeDash([4, 4]);
                }

                me.drawLine(g);
            }

            //me.endAnchor.setSize(10, 10);
        }

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
            item_fullname: 'Mini2.flow.extend.Line',

            style_name: me.style_name,


            //start_node_id: '',

            //end_node_id: '',

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