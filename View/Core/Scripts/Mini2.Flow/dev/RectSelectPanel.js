/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

//矩形框的选择框
// 这个容器是从 0,0 坐标开始...只是绘制里面的内容.
Mini2.define('Mini2.flow.RectSelectPanel', {

    extend: 'Mini2.flow.FlowComponent',





    x: 0,

    y: 0,

    width: 100,

    height: 100,

    //8 个热点
    hands: false,


    //半透明的复制图形
    ghostShapes: null,

    //克隆层
    ghost: null,  


    //当选中的多个元素, 就加入8个间距
    groupPadding: 8,

    //半透明的蒙层
    maskPanel: null,




    initComponent: function () {
        "use strict";
        var me = this,
            hands,
            el;



        me.el = el = new createjs.Container();
        el.x = 0;
        el.y = 0;
        el.visible = me.visible;

        el.Mini2_Object = me;



        me.craeteGhostInstace();    //创建克隆层


        me.rectObj = me.createRect();//创建虚线边框

        el.addChild(me.rectObj);


        //创建8个热点
        me.hands = hands = me.createHand8();


        for (var i = 0; i < hands.length; i++) {

            el.addChild(hands[i].el);
        }




    },
    

    isMoved: function () {
        "use strict";
        var me = this,
            ghsot = me.ghost;

        return ghsot.isMoved;
    },

    onMouseUp: function () {
        "use strict";
        var me = this,
            shape,
            shapes = me.ghostShapes || [],
            ghost = me.ghost;

        //console.log("onMouseUp 88888888");

        if (shapes.length == 1) {
            shape = shapes[0];

            shape.setXY(ghost.x,ghost.y);

            me.setXY(ghost.x, ghost.y);
        }
        else {

        }


        me.on('changed');
    },



    // 创建克隆层
    craeteGhostInstace: function () {
        "use strict";
        var me = this,
            el = me.el,
            ghost;




        me.ghost = ghost = Mini2.create('Mini2.flow.GhostShapePanel', {
            page : me.page
        });

        ghost.render();

        el.addChild(ghost.el);

        ghost.bind('mouseup', me.ghost_mouseup, null, me);

        //蒙层
        //me.maskPanel = mask = new createjs.Shape();
        //el.addChild(mask);


        ghost.el.addEventListener('dblclick', function (evt) {

            if (me.oneShape) {
                me.on('dblclick', {
                    item: me.oneShape
                });
            }

        });
        
    },


    ghost_mouseup:function(sender,evt){
        var me = this;


        console.log('RectSelectPanel.ghost_mouseup(...)');


        me.on('moved', evt)
    },


    offset:function(offsetX, offsetY){
        "use strict";
        var me = this,
            ghost = me.ghost;

        me.x += offsetX;
        me.y += offsetY;

        //ghost.srcSelectedRect.x += offsetX;
        //ghost.srcSelectedRect.y += offsetY;


        me.resetRest();
        me.resetHandLocal();



        return me;
    },


    //设置克隆对象
    setShapes: function (shapes, lines) {
        "use strict";
        var me = this,
            ghost = me.ghost,
            shape,
            shapes = shapes || [],
            lines = lines || [],
            padding = me.groupPadding,
            width,height;

        if (shapes  && !Mini2.isArray(shapes) ) {
            shapes = [shapes];
        }

        var rect = me.getShapesArea(shapes, lines);

        //console.log("RectSelectPanel.setShapes");
        //console.log(rect);

        rect = Mini2.clone(rect);

        if (shapes.length + lines.length > 1) {
            rect.x0 -= padding;
            rect.y0 -= padding;

            rect.x1 += padding;
            rect.y1 += padding;
        }

        if (shapes.length == 1) {
            shape = shapes[0]; 
            me.widthLock = shape.widthLock;
            me.heightLock = shape.heightLock;

            me.oneShape = shape;
        }
        else {
            me.widthLock = false;
            me.heightLock = false;

            me.oneShape = null;
        }


        ghost.isMoved = false;
        ghost.setShapes(shapes, rect);


        width = rect.x1 - rect.x0;
        height = rect.y1 - rect.y0;

        me.setRect(rect.x0, rect.y0, width, height)

        //console.log("RectSelectPanel.setShapes(...)");
        //console.log(rect);


        return me;
    },


    getShapesArea: function (shapeList, lineList) {
        "use strict";
        var me = this,
            shape,
            rect,
            aRect = false,
            i,
            shapeList = shapeList || [],
            len = shapeList.length;

        

        for (i = 0; i < len; i++) {

            shape = shapeList[i];

            rect = shape.getRect();
            
            if (aRect) {

                aRect.x0 = Math.min(aRect.x0, rect.x0);
                aRect.y0 = Math.min(aRect.y0, rect.y0);

                aRect.x1 = Math.max(aRect.x1, rect.x1);
                aRect.y1 = Math.max(aRect.y1, rect.y1);
            }
            else {
                aRect = Mini2.clone(rect);
            }
        }

        if (lineList) {
            for (i = 0; i < lineList.length; i++) {
                var line = lineList[i];

                rect = line.getRect();



                if (aRect) {

                    aRect.x0 = Math.min(aRect.x0, rect.x0);
                    aRect.y0 = Math.min(aRect.y0, rect.y0);

                    aRect.x1 = Math.max(aRect.x1, rect.x1);
                    aRect.y1 = Math.max(aRect.y1, rect.y1);
                }
                else {
                    aRect = Mini2.clone(rect);
                }
            }
        }

        if (aRect) {
            aRect.x = aRect.x0;
            aRect.y = aRect.y0;
        }

        return aRect;

    },



    render: function () {
        var me = this;


    },

    //创建虚线矩形
    createRect: function () {
        "use strict";
        var me = this,
            rectObj;

        rectObj = new createjs.Shape();
        rectObj.x = me.x;
        rectObj.y = me.y;
        rectObj.width = me.width;
        rectObj.height = me.height;
        
        //me.resetRest();

        return rectObj;
    },


    resetRest: function (graphics) {
        "use strict";
        var me = this,
            rectObj = me.rectObj,
            g;
        
        rectObj.x = me.x;
        rectObj.y = me.y;

        g = graphics || rectObj.graphics;

        g.clear();
        //g.setStrokeDash([4, 4]);
        g.setStrokeStyle(1);

        g.beginStroke("#939393");
        g.drawRect(0.5, 0.5, me.width, me.height);

        return me;
    },


    //创建8个热点
    createHand8: function () {
        "use strict";
        var me = this,
            i,
            hand,
            hands = [],
            sz = 7;

        for (i = 0; i < 8; i++) {
            
            hand = Mini2.create('Mini2.flow.Hand', {
                page:me.page,
                index: i,
                width: sz,
                height: sz
            });

            hand.render();

            hands.push(hand);

            hand.bind('mousedown', me.head_MouseDown, null, me)
                .bind('pressmove', me.head_PressMove, null, me)
                .bind('mouseup', me.head_MouseUp, null, me);

            //me.setHandReg(i, hand,sz);
            me.setHandCursor(i, hand);
        }


        return hands;
    },

    //设置鼠标
    setHandCursor: function (i, hand) {
        "use strict";
        var me = this,
            name ;

        switch (i) {
            case 0: name = 'nw-resize'; break;
            case 1: name = 's-resize'; break;
            case 2: name = 'sw-resize'; break;
            case 3: name = 'e-resize'; break;
            case 4: name = 'nw-resize'; break;
            case 5: name = 's-resize'; break;
            case 6: name = 'sw-resize'; break;
            case 7: name = 'e-resize'; break;
        }


        hand.setCursor(name);

        return me;
    },

    //设置重心
    setHandReg: function (i, hand, sz) {
        "use strict";
        var me = this;

        switch (i) {
            case 0: hand.setReg(sz, sz); break;
            case 1: hand.setReg(sz / 2, sz); break;
            case 2: hand.setReg(0, sz); break;
            case 3: hand.setReg(0, sz / 2); break;
            case 4: hand.setReg(0, 0); break;
            case 5: hand.setReg(sz / 2, 0); break;
            case 6: hand.setReg(sz, 0); break;
            case 7: hand.setReg(sz, sz / 2); break;
        }

        return me;
    },


    //缩小
    rectZoomOut: function (rect, padding) {
        "use strict";
        var p2 = padding * 2;
        
        rect.x += padding;
        rect.y += padding;

        rect.width -= p2;
        rect.height -= p2;
    },





    head_MouseUp: function (sender, e) {
        "use strict";
        var me = this,
            index = sender.index,
            padding = me.groupPadding,
            rect,
            srcRect = me.srcRect,
            ghost = me.ghost,
            shapes = ghost.shapes;
        

        ghost.visible = true;

        //gs.drawReset(gi.graphics);

        //console.log('hand_MouseUp: (' + me.x + ", " + me.y + ")  W=" + me.width + ", H=" + me.height);

        rect = {
            x: me.x,
            y: me.y,
            width: me.width,
            height: me.height
        };

        if (shapes.length > 1) {
            me.rectZoomOut(rect, padding);
        }

        me.on('changed', {
            shapes: shapes,
            ghosts: ghost.ghosts,
            hand: sender,
            srcRect: srcRect,
            rect: rect
        });
    },


    head_MouseDown: function (sender, e) {
        "use strict";
        var me = this,
            srcRect,
            padding = me.groupPadding,
            ghost = me.ghost;

        me.focusItemX = sender.getX();
        me.focusItemY = sender.getY();

        me.srcHeight = me.height;
        me.srcWidth = me.width;

        ghost.visible = false;


        me.srcRect = srcRect = {
            x: me.x,
            y: me.y,
            width: me.width,
            height: me.height
        };

        if (ghost.shapes.length > 1) {
            me.rectZoomOut(srcRect, padding);
        }

        srcRect.x0 = srcRect.x;
        srcRect.y0 = srcRect.y;
        srcRect.x1 = srcRect.x + srcRect.width;
        srcRect.y1 = srcRect.y + srcRect.height;


        ghost.resetSrcSizeForGhosts(srcRect);
    },


    head_PressMove: function (sender, e) {
        "use strict";
        var me = this,
            padding = me.groupPadding,
            lineObj = me.lineObj,
            index = sender.index,
            ghost = me.ghost,
            shapes = ghost.shapes,
            itemX = sender.getX(),
            itemY = sender.getY(),
            focusItemY = me.focusItemY,
            focusItemX = me.focusItemX,
            x,y,width,height;


        var mmX = itemX % 8;
        var mmY = itemY % 8;

        itemX -= mmX;
        itemY -= mmY;


        if (0 == index) {
            me.y = itemY;
            me.x = itemX;

            me.height = me.srcHeight - (itemY - focusItemY);
            me.width = me.srcWidth - (itemX - focusItemX);
        }
        else if (1 == index) {
            me.y = itemY;
            me.height = me.srcHeight - (itemY - focusItemY);
        }
        else if (2 == index) {
            me.y = itemY;
            me.width = itemX - me.x;
            me.height = me.srcHeight - (itemY - focusItemY);            
        }
        else if (3 == index) {
            me.width = itemX - me.x;            
        }
        else if (4 == index ) {
            me.width = itemX - me.x;
            me.height = itemY - me.y;
        }
        else if (5 == index) {

            me.height = itemY - me.y;
        }
        else if (6 == index) {            
            me.x = itemX;
            me.height = itemY - me.y;
            me.width = me.srcWidth - (itemX - focusItemX);
        }
        else if (7 == index) {            
            me.x = itemX;
            me.width = me.srcWidth - (itemX - focusItemX);
        }

        me.resetRest();
        me.resetHandLocal(sender);


        var ww = me.width;
        var hh = me.height;

        if (shapes.length > 1) {
            var p2 = padding * 2;
            ww -= p2;
            hh -= p2;
        }

        var srcRect = me.srcRect;
        var w2 = ww / srcRect.width;
        var h2 = hh / srcRect.height;

        //console.log(w2 + ", " + h2);

        var newRect = {
            x: me.x,
            y: me.y,
            width: ww,
            height: hh,
            x0: me.x,
            y0: me.y,
            x1: me.x + ww,
            y1: me.y + hh
        };

        if (shapes.length > 1) {
            newRect.x += padding;
            newRect.y += padding;
        }

        ghost.scale(w2, h2, newRect);


        //gi.visible = true;
        //gi.alpha = 0.5;
        //gi.x = me.x;
        //gi.y = me.y;

        //gs.drawReset(gi.graphics,me.width,me.height);

        //me.on('changed', {
        //    index: index
        //});
    },


    //获取克隆对象集的边界
    getGhostBounds: function () {
        "use strict";
        var me = this,
            padding = me.groupPadding,
            ghost = me.ghost,
            rect = ghost.getBounds();

        rect.x = me.x + rect.x;
        rect.y = me.y + rect.y;

        if (ghost.ghosts.length > 1) {
            rect.x += padding;
            rect.y += padding;
        }

        return rect;
    },

    //获取当前选择的边界
    getBounds: function () {
        "use strict";
        var me = this,
            padding = me.groupPadding,
            ghost = me.ghost,
            rect = {
                x: me.x,
                y: me.y,
                width: me.width,
                height : me.height
            };

        if (ghost.ghosts.length > 1) {
            me.rectZoomOut(rect, padding);
        }

        return rect;
    },

    setRectForCartoon: function (srcRect, targetRect, shapes, lines) {
        "use strict";
        var me = this,
            el = me.el,
            ghost = me.ghost,
            padding = me.groupPadding,
            src = srcRect,
            toRect,
            tar = targetRect;

        if (shapes.length + lines.length > 1) {
            tar.x0 -= padding;
            tar.y0 -= padding;
            tar.x1 += padding;
            tar.y1 += padding;
        }


        tar.width = tar.x1 - tar.x0;
        tar.height = tar.y1 - tar.y0;

        toRect = {
            x: tar.x0,
            y: tar.y0,
            width: tar.width,
            height: tar.height
        };


        me.x = src.x0;
        me.y = src.y0;

        me.width = src.x1 - src.x0;
        me.height = src.y1 - src.y0;

        //console.log(srcRect);
        //console.log(targetRect);

        me.hideHotAll();
        me.resetRest();

        createjs.Tween.get(me).to(toRect, 100, createjs.Ease.getPowInOut(2)).call(function(){


            me.showHotAll();

            me.resetRest();
            me.resetHandLocal();


        }).addEventListener("change", function () {

            me.resetRest();
            
        });

        return me;
    },

    setRect: function (x, y, width, height) {
        "use strict";
        var me = this;

        me.x = x;
        me.y = y;


        if (undefined != width) {
            me.width = width;
        }

        if (undefined != height) {
            me.height = height;
        }


        me.resetRest();
        me.resetHandLocal();

        return me;
    },


    setXY:function(x,y){
        var me = this;
        
        me.x = x;
        me.y = y;

        me.resetRest();
        me.resetHandLocal();

        return me;
    },

    setSize:function(width,height){
        var me = this;


        if (width) {
            me.width = width;
        }

        if (height) {
            me.height = height;
        }

        me.resetRest();
        me.resetHandLocal();

        return me;
    },

    //隐藏全部热点
    hideHotAll:function(){
        var me = this,
            i,
            hands = me.hands;

        for (i = 0; i < 8; i++) {
            hands[i].setVisible(false);
        }

        return me;
    },

    showHotAll: function () {
        "use strict";
        var me = this,
            i=0,
            hands = me.hands;

        for (; i < 8; i++) {
            hands[i].setVisible(true);
        }

        return me;
    },
    
    //重新设置热点的位置
    resetHandLocal: function () {
        "use strict";
        var me = this,
            x = me.x,
            y = me.y,
            w = me.width + x,
            h = me.height + y,
            w2 = me.width / 2 + x,
            h2 = me.height /2 + y,            
            hands = me.hands;

        hands[0].setXY(x, y);
        hands[1].setXY(w2, y);
        hands[2].setXY(w, y);

        hands[3].setXY(w, h2);
        hands[4].setXY(w, h);

        hands[5].setXY(w2, h);
        hands[6].setXY(x, h);
        hands[7].setXY(x, h2);

        if (me.widthLock && me.heightLock) {
            for (var i = 0; i < 8; i++) {
                hands[i].setLock(true);
            }
        }
        else if (me.widthLock && !me.heightLock) {
            hands[0].setLock(true);
            hands[1].setLock(false);
            hands[2].setLock(true);

            hands[3].setLock(true);
            hands[4].setLock(true);
            hands[5].setLock(true);

            hands[6].setLock(false);
            hands[7].setLock(true);
        }
        else if (!me.widthLock && me.heightLock) {
            hands[0].setLock(true);
            hands[1].setLock(true);
            hands[2].setLock(true);

            hands[3].setLock(false);
            hands[4].setLock(true);
            hands[5].setLock(false);

            hands[6].setLock(true);
            hands[7].setLock(true);

        }
        else {
            for (var i = 0; i < 8; i++) {
                hands[i].setLock(false);
            }
        }

        return me;
    }



}, function () {
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});