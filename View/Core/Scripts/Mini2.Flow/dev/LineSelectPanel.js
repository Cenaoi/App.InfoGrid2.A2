/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

//矩形框的选择框
// 这个容器是从 0,0 坐标开始...只是绘制里面的内容.
Mini2.define('Mini2.flow.LineSelectPanel', {

    extend: 'Mini2.flow.FlowComponent',

    isLine: true,

    x: 0,

    y: 0,

    width: 100,

    height: 100,

    hands: false,
    
    //旧的热点
    oldhands: false,


    //线段编辑动作. move-没有动作, add-添加点, remove-移除点
    action: 'move',



    initComponent: function () {
        "use strict";
        var me = this,
            el;

        me.hands = [];
        me.oldhands = [];



        me.el = el = new createjs.Container();
        el.x = 0;
        el.y = 0;
        el.visible = me.visible;

        el.Mini2_Object = me;


        var lineObj = me.createDotLine();
        
        el.addChild(lineObj);
    },


    //设置编辑动作. none | add | remove
    setAction: function (actionName) {
        "use strict";
        var me = this;

        me.action = actionName;

        return me;
    },


    //创建虚线 
    createDotLine: function () {
        "use strict";
        var me = this,
            lineObj;

        me.lineObj = lineObj = new createjs.Shape();
        lineObj.x = 0;
        lineObj.y = 0;
        lineObj.width = me.width;
        lineObj.height = me.height;



        //lineObj.addEventListener("rollover", function (evt) {

        //});



        //lineObj.addEventListener("rollout", function (evt) {


        //});

        //lineObj.addEventListener('dbClick', function (evt) {
        //    me.action_add(evt);
        //});

        lineObj.addEventListener("mousedown", function (evt) {

            if ('add' == me.action) {
                me.action_add(evt);
            }

        });

        lineObj.addEventListener('dblclick', function (evt) {

            me.on('dblclick', {
                item: me.curShape
            });

        });


        return lineObj;
    },


    //绘制点线
    resetDotLine: function () {
        "use strict";
        var me = this,
            lineObj = me.lineObj,
            g = lineObj.graphics;
         
        g.clear();


        g.setStrokeStyle(6, "round");
        g.beginStroke(createjs.Graphics.getRGB(255, 255, 0, 0.5));

        me.drawLine(g);


        g.setStrokeDash([4, 4]);
        g.setStrokeStyle(1);

        g.beginStroke('#a8a8a8');

        me.drawLine(g);


        return me;
    },


    //绘制线
    drawLine: function (g) {
        "use strict";
        var me = this,
            i,
            hand,
            hands = me.hands || [],
            len = hands.length;

        if (len >= 2) {
            hand = hands[0];
            
            g.moveTo(hand.x, hand.y);

            for (i = 1; i < len; i++) {
                hand = hands[i];

                g.lineTo(hand.x, hand.y);
            }
        }
        else {
            g.clear();
        }

        return me;
    },


    //设置吸附点.
    setFixedPoint: function (point) {
        "use strict";
        var me = this,
            hands = me.hands,
            curHand = me.curHand,
            index ;

        

        me.fixedPoint = point;

        if (curHand && point) {

            index = curHand.index;

            var isFirst = (0 == index);
            var isLast = (hands.length - 1 == index);

            if (isFirst || isLast) {

                curHand.setXY(point.x, point.y);


                me.resetDotLine();

            }

        }

        return me;
    },


    setVisible: function (value) {
        var me = this,
            el = me.el;

        el.visible = value;

        //if (value != el.visible) {

        //    if (el.visible && !value) {
        //        createjs.Tween.get(me.el).to({ alpha: 0 }, 200).call(function () {
        //            el.visible = false;
        //        });
        //    }
        //    else {
        //        el.alpha = 1;
        //        el.visible = value;
        //    }
        //}

        return me;
    },

    render: function () {
        var me = this;


    },

    //创建虚线矩形
    createLine: function () {
        var me = this;


    },


    resetLine: function () {
        var me = this;

    },

    //获取新的手柄对象
    getNewHand: function () {
        "use strict";
        var me = this,
            sz = 7,
            hand,
            oldHands = me.oldhands;

        if (oldHands.length) {

            hand = oldHands.pop();

        }
        else {

            //console.log("page ==  " + me.page);

            hand = Mini2.create('Mini2.flow.Hand', {
                page: me.page,
                width: sz,
                height: sz,
                cursor: 'pointer'
            });

            hand.render();

            hand.bind('mousedown', me.head_MouseDown, null, me)
                .bind('mouseup', me.head_MouseUp, null, me)
                .bind('pressmove', me.head_PressMove, null, me);


            me.addPageItem(hand);

        }

        return hand;
    },


    // 设置坐标位置
    // @offsetX=偏移量 X
    // @offsetY=偏移量 Y
    setPoints: function (points, offsetX, offsetY) {
        "use strict";
        var me = this,
            el = me.el,
            i ,
            p,
            len,
            hand,
            oldHands = me.oldhands,
            hands = me.hands,
            lastLen = hands.length;

        len = points.length;

        offsetX = offsetX || 0;
        offsetY = offsetY || 0;

        if (lastLen > len) {
            for (i = 0; i < lastLen - len; i++) {

                hand = hands.pop();
                hand.setVisible(false);

                oldHands.push(hand);

            }
        }


        for (i=0; i < len; i++) {

            p = points[i];

            if (i < lastLen) {

                hand = hands[i];

            }
            else{
                hand = me.getNewHand();                
                hands.push(hand);
            }


            if (i > 0 && i < len - 1) {
                hand.setHandType(1);
            }
            else {
                hand.setHandType(0);
            }

            hand.index = i;
            hand.setXY(p.x + offsetX, p.y + offsetY);
            hand.setVisible(true);
        }


        me.resetDotLine();

        return me;
    },


    //允许移动
    allowMove:function(){
        var me = this,
            action = me.action;

        return 'move' == action;
    },

    //当前移动的手柄
    curHand : null,
    
    head_MouseUp: function (sender, e) {
        "use strict";
        var me = this,
            hands = me.hands,
            oldHands = me.oldhands,
            lineObj = me.lineObj,
            index = sender.index,
            isFirst = index == 0,
            isLast = (index == hands.length - 1);


        me.focusItemX = sender.getX();
        me.focusItemY = sender.getY();

        me.srcHeight = me.height;
        me.srcWidth = me.width;


        if ('add' == me.action) {

            return;
        }
        else if ('remove' == me.action && hands.length > 2) {

            me.action_remove(sender);

            return;
        }

        me.on('mouseup', {
            index: index,
            toX: sender.x,
            toY: sender.y,
            isFirst: isFirst,
            isLast: isLast
        });


        me.curHand = null;
    },


    action_add: function (evt) {
        "use strict";
        var me = this,
            hands = me.hands,
            oldHands = me.oldhands,
            lineObj = me.lineObj,
            stageX = evt.stageX,
            stageY = evt.stageY;

        //console.log(evt);

        var h0 = hands[0];

        var h1;

        var minJL = 1000000000; //存放最短距离
        var pIndex = -1;

        for (var i = 1; i < hands.length; i++) {
            h1 = hands[i];

            //console.log('sX=' + stageX + ', sY=' + stageY);
            //console.log('(' + h0.x + ', ' + h0.y + ') - (' + h1.x + ', ' + h1.y + ')');

            var jl = me.PointLine_Disp(stageX, stageY, h0.x, h0.y, h1.x, h1.y);

            if (jl < minJL) {
                minJL = jl;
                pIndex = i-1;
            }
            
            h0 = h1;
        }

        h0 = hands[pIndex];
        h1 = hands[pIndex + 1];


        var newHand = me.getNewHand();

        newHand.index = pIndex + 1;
        newHand.setXY(stageX, stageY)
            .setHandType(1)
            .setVisible(true);
                
        hands.splice(pIndex + 1, 0, newHand);

        for (var i = pIndex + 1; i < hands.length; i++) {
            hands[i].index = i;
        }

        me.resetDotLine();

        me.on('insert', {
            index: pIndex + 1,
            x: stageX,
            y: stageY
        });


    },

    action_remove: function (sender) {
        "use strict";
        var me = this,
            hands = me.hands,
            oldHands = me.oldhands,
            lineObj = me.lineObj,
            index = sender.index,
            isFirst = index == 0,
            isLast = (index == hands.length - 1);

        var delItems = hands.splice(index, 1);

        for (var i = 0; i < delItems.length; i++) {

            var oldHead = delItems[i];

            oldHead.setVisible(false);

            oldHands.push(oldHead);
        }

        var len = hands.length;

        for (var i = 0; i < len; i++) {
            hands[i].index = i;


            if (i > 0 && i < len - 1) {
                hands[i].setHandType(1);
            }
            else {
                hands[i].setHandType(0);
            }
        }



        me.resetDotLine();

        me.on('remove', {
            index: index,
            isFirst: isFirst,
            isLast: isLast
        });


    },

    head_MouseDown: function (sender, e) {
        "use strict";
        var me = this,
            hands = me.hands,
            lineObj = me.lineObj,
            index = sender.index,
            isFirst = index == 0,
            isLast = (index == hands.length - 1);


        me.curHand = sender;

        me.focusItemX = sender.getX();
        me.focusItemY = sender.getY();

        me.srcHeight = me.height;
        me.srcWidth = me.width;

                
        
        if ('add' == me.action) { 
            return;
        }

        if ('remove' == me.action) {
            return;
        }

        me.on('mousedown', {
            index: index,
            toX: sender.x,
            toY: sender.y,
            isFirst: isFirst,
            isLast: isLast
        });
    },

    head_PressMove: function (sender, e) {
        "use strict";
        var me = this,
            hands = me.hands,
            lineObj = me.lineObj,
            index = sender.index;

        if ('move' ==  me.action ) {

            var curHand = me.curHand,
                index,
                fixedPoint = me.fixedPoint;

            var isFirst = (0 == index),
                isLast = (hands.length - 1 == index);


            var srcX = sender.x;
            var srcY = sender.y;

            var mmX = srcX % 8;
            var mmY = srcY % 8;

            var xx = srcX - mmX;
            var yy = srcY - mmY;

            if (fixedPoint && curHand) {

                index = curHand.index;

                if (isFirst || isLast) {

                    curHand.setXY(fixedPoint.x, fixedPoint.y);
                }
                else {
                    curHand.setXY(xx, yy);
                }
            }
            else {
                curHand.setXY(xx, yy);
            }

            me.resetDotLine();

            var eventData = {
                index: index,
                toX: xx,
                toY: yy,
                isFirst: isFirst,
                isLast: isLast
            };

            me.on('changed', eventData);
        }
    },

    //设置手柄位置
    resetHandLocal: function () {


    },


    //点到线段的距离
    PointLine_Disp: function (xx, yy, x1, y1, x2, y2) {
        "use strict";
        var a, b, c, ang1, ang2, ang, m;
        var result = 0;

        //分别计算三条边的长度
        a = Math.sqrt((x1 - xx) * (x1 - xx) + (y1 - yy) * (y1 - yy));
        if (a == 0)
            return -1;
        b = Math.sqrt((x2 - xx) * (x2 - xx) + (y2 - yy) * (y2 - yy));
        if (b == 0)
            return -1;
        c = Math.sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

        //如果线段是一个点则退出函数并返回距离
        if (c == 0) {
            result = a;
            return result;
        }

        //如果点(xx,yy到点x1,y1)这条边短
        if (a < b) {
            //如果直线段AB是水平线。得到直线段AB的弧度
            if (y1 == y2) {
                if (x1 < x2)
                    ang1 = 0;
                else
                    ang1 = Math.PI;
            }
            else {
                m = (x2 - x1) / c;
                if (m - 1 > 0.00001)
                    m = 1;
                ang1 = Math.acos(m);
                if (y1 > y2)
                    ang1 = Math.PI * 2 - ang1;//直线(x1,y1)-(x2,y2)与折X轴正向夹角的弧度
            }
            m = (xx - x1) / a;
            if (m - 1 > 0.00001)
                m = 1;
            ang2 = Math.acos(m);
            if (y1 > yy)
                ang2 = Math.PI * 2 - ang2;//直线(x1,y1)-(xx,yy)与折X轴正向夹角的弧度

            ang = ang2 - ang1;
            if (ang < 0) ang = -ang;

            if (ang > Math.PI) ang = Math.PI * 2 - ang;
            //如果是钝角则直接返回距离
            if (ang > Math.PI / 2)
                return a;
            else
                return a * Math.sin(ang);
        }
        else//如果(xx,yy)到点(x2,y2)这条边较短
        {
            //如果两个点的纵坐标相同，则直接得到直线斜率的弧度
            if (y1 == y2)
                if (x1 < x2)
                    ang1 = Math.PI;
                else
                    ang1 = 0;
            else {
                m = (x1 - x2) / c;
                if (m - 1 > 0.00001)
                    m = 1;
                ang1 = Math.acos(m);
                if (y2 > y1)
                    ang1 = Math.PI * 2 - ang1;
            }
            m = (xx - x2) / b;
            if (m - 1 > 0.00001)
                m = 1;
            ang2 = Math.acos(m);//直线(x2-x1)-(xx,yy)斜率的弧度
            if (y2 > yy)
                ang2 = Math.PI * 2 - ang2;
            ang = ang2 - ang1;
            if (ang < 0) ang = -ang;
            if (ang > Math.PI) ang = Math.PI * 2 - ang;//交角的大小
            //如果是对角则直接返回距离
            if (ang > Math.PI / 2)
                return b;
            else
                return b * Math.sin(ang);//如果是锐角，返回计算得到的距离
        }


    }



});