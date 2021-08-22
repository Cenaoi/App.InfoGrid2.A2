/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />


// 克隆对象的
Mini2.define('Mini2.flow.GhostShapePanel', {
    
    extend: 'Mini2.flow.FlowComponent',
    
    el: null,

    text : 'Ghost 精灵',

    //复制
    ghosts : null,

    //边框线段
    box: null,


    //原选择区域
    srcSelectedRect: null,


    isMoved: false,

    initComponent: function () {
        "use strict";
        var me = this,
            el,
            box;
           
        me.ghosts = [];

        me.el = el = new createjs.Container();


        me.box = box = new createjs.Shape();

        el.addChild(box);
        



        el.addEventListener("mousedown", function (evt) {
            me.el_mousedown.call(me, el, evt);
        });


        el.on("pressmove", function (evt) {
            me.el_pressmove.call(me, el, evt);
        });


    },

    rectBounds:false,

    el_pressmove: function (sender, evt) {
        "use strict";
        var me = this,
            el = me.el,
            box = me.box,
            page = me.page,
            px, py,
            pmX, pmY,
            rectBounds = me.rectBounds = me.rectBounds || {},
            rect = me.srcSelectedRect;



        if (!page || !page.designMode) {
            return;
        }

        px = evt.stageX - me.downX;
        py = evt.stageY - me.downY;


        if (Math.abs(px) <= 4 && Math.abs(py) <= 4) {
            return;
        }

        pmX = (box.x + px) % 12;
        pmY = (box.y + py) % 12;

        me.isMoved = true;


        el.alpha = 0.8;

        px -= pmX;
        py -= pmY;

        el.x = px;
        el.y = py;


        rectBounds.x = px;
        rectBounds.y = py;
        rectBounds.width = rect.x1 - rect.x0;
        rectBounds.height = rect.y1 - rect.y0;


    },

    el_mousedown: function (sender, evt) {
        "use strict";
        var me =this,
            el = me.el,
            box = me.box,
            page = me.page,
            rect = me.srcSelectedRect;


        me.isMoved = false;

        me.downX = evt.stageX;
        me.downY = evt.stageY;

        //console.log(evt);

        if (page) {
            page.setMousedownTarget.call(page, me);
        }

        el.alpha = 0.3;



        box.x = rect.x0;
        box.y = rect.y0;
        box.visible = true;


        me.resetBox();

        me.on('mousedown');
    },


    resetBox: function () {
        "use strict";
        var me = this,
            box = me.box,
            g = box.graphics,
            rect = me.srcSelectedRect;

        //box.x = rect.x0;
        //box.y = rect.y0;

        //console.log(rect);

        g.clear();
        g.setStrokeDash([3, 3]);
        g.setStrokeStyle(1);

        g.beginStroke("#69ccff");
        g.drawRect(0, 0, rect.x1 - rect.x0, rect.y1 - rect.y0);
    },

    getBounds:function(){
        var me = this,
            src = me.srcSelectedRect,
            box = me.box;

        return me.rectBounds;
    },
    

    onMouseUp: function () {
        "use strict";
        var me = this,
            el = me.el,
            isMoved = me.isMoved,
            i,
            box = me.box,
            ghost,
            ghosts = me.ghosts,
            rect = me.srcSelectedRect;

        if (!isMoved) {

            box.visible = false;


            return;
        }


        rect.x = box.x;
        rect.y = box.y;

        rect.x0 = rect.x;
        rect.y0 = rect.y;
        rect.x1 = rect.x + rect.width;
        rect.y1 = rect.y + rect.height;



        me.on('mouseup', {
            shapes: me.shapes,
            moveX: el.x,
            moveY: el.y
        });


        for (i = 0; i < ghosts.length; i++) {
            ghost = ghosts[i];

            ghost.offset(el.x, el.y);
        }


        //console.log(rect);

        //console.log('x=' + el.x + ',y=' + el.y);

        rect.x0 += el.x;
        rect.y0 += el.y;
        rect.x1 += el.x;
        rect.y1 += el.y;

        //console.log(rect);


        box.x = rect.x0;
        box.y = rect.y0;

        el.x = 0;
        el.y = 0;

        box.visible = false;

        //box.x = rect.x;
        //box.y = rect.y;
    },


    //比率
    scale: function (scaleX, scaleY, srcSelectedRect) {
        "use strict";
        var me = this,
            el = me.el,
            i,
            srcSRect = srcSelectedRect || me.srcSelectedRect,
            xx,yy,sW,sH,sX,sY,
            ghost,
            ghosts = me.ghosts;


        el.alpha = 0.5;


        for (i = 0; i < ghosts.length; i++) {

            ghost = ghosts[i];

            
            xx = ghost.rX - ghost.x;
            yy = ghost.rY - ghost.y;

            sW = ghost.srcWidth * scaleX;
            sH = ghost.srcHeight * scaleY;

            sX = srcSRect.x - ghost.rX * scaleX;
            sY = srcSRect.y - ghost.rY * scaleY;


            ghost.setRect(sX, sY, sW, sH);

        }


        return me;
    },

    setShapes: function (shapes, selectRect) {
        "use strict";
        var me = this,
            el = me.el,
            i,
            len ,
            ghost,
            ghosts = me.ghosts,
            srcLen = ghosts.length;

        me.shapes = shapes = shapes || [];
        len = shapes.length;

        for (i = 0; i < srcLen; i++) {

            ghost = ghosts.pop();

            el.removeChild(ghost.el)
        }

        el.x = 0;
        el.y = 0;
        el.alpha = 0.4;


        for (i = 0; i < len; i++) {

            ghost = me.createGhost(shapes[i]);

            el.addChild(ghost.el);

            ghosts.push(ghost);
        }

        me.resetSrcSizeForGhosts(selectRect);

    },

    resetSrcSizeForGhosts: function (selectRect) {
        "use strict";
        var me = this,
            i,
            box = me.box,
            ghost,
            ghosts = me.ghosts;

        me.srcSelectedRect = selectRect;

        for (i = 0; i < ghosts.length; i++) {
            ghost = ghosts[i];

            ghost.srcWidth = ghost.width;
            ghost.srcHeight = ghost.height;

            //console.log(ghost.x + ",   " + ghost.y);

            ghost.rX = selectRect.x - ghost.x;
            ghost.rY = selectRect.y - ghost.y;

           

            //console.log("**************************************************");
            //console.log("ghost.x=" + ghost.x + ", ghost.y=" + ghost.y);
        }

        //console.log("+++++++++++++++++++++++++++++++------------------------------");
        //console.log(selectRect);

        //me.resetBox();
    },

    //克隆灵魂
    createGhost: function (shape) {
        "use strict";
        var me = this,
            ghost;

        ghost = Mini2.create('Mini2.flow.GhostItem', {
            src : shape
        });

        ghost.render();

        return ghost;
    },



    //允许移动
    allowMove: function () {
        "use strict";
        var me = this,
            parent = me.parent;

        if (parent && parent.allowMove) {
            return parent.allowMove();
        }

        return true;
    },




    render: function () {
        var me = this;

        me.drawReset();

        return me;
    }


}, function () {
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});