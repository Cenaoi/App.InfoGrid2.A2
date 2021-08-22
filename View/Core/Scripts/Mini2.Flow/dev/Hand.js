/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

Mini2.define('Mini2.flow.Hand', {


    extend: 'Mini2.flow.FlowComponent',

    x: 0,

    y: 0,

    width: 5,

    height: 5,

    el: null,

    eventSet: false,
    
    //手柄类型. 0-正方形, 1-菱形, 2-圆形
    handType: 0,

    lock : false,

    initComponent: function () {
        "use strict";
        var me = this,
            el;

        me.eventSet = {};   //时间

        me.el = el = new createjs.Shape();
        el.x = me.x;
        el.y = me.y;
        el.rotation = me.rotation;

        el.regX = me.width / 2;
        el.regY = me.height / 2;

        el.alpha = 1;

        el.cursor = me.cursor || 'move';
        me.srcCursor = el.cursor;

        el.Mini2_Object = me;



        el.addEventListener("mousedown", function (evt) {

            me.downX = evt.stageX - el.x;
            me.downY = evt.stageY - el.y;

            if (me.page) {
                me.page.setMousedownTarget.call(me.page, me);
            }

            me.on('mousedown');
            
            //console.log("mousedown");
        });


        //el.addEventListener('click', function (evt) {

        //    console.log("xxxxxxxxxxxxxxxx");
        //});


        el.on("pressmove", function (evt) {

            if (me.lock) {
                return;
            }
            
            if (me.allowMove()) {

                var px = evt.stageX - me.downX;

                var py = evt.stageY - me.downY;

                var pmX = px % 12;
                var pmY = py % 12;

                if (pmX <= 4 && pmY <= 4) {
                    return;
                }

                //px -= pmX;
                //py -= pmY;


                me.x = el.x = px;
                me.y = el.y = py;

                me.on('pressmove');
            }

        });
    },


    //设置锁状态
    setLock:function(value){
        var me = this,
            el = me.el,
            lock = me.lock;

        if (lock != value) {
            me.lock = lock = !!value;

            el.alpha = (lock ? 0.5 : 1);

            el.cursor = (lock ? 'hand' : me.srcCursor);
        }

        return me;
    },


    onMouseUp: function () {
        var me = this;

        me.on('mouseup');

        return me;
    },



    //允许移动
    allowMove:function(){
        var me = this,
            parent = me.parent;

        if (parent && parent.allowMove) {
            return parent.allowMove();
        }

        return true;
    },

    //设置鼠标指针
    setCursor:function(value){
        var me = this,
            el = me.el;

        el.cursor = value;

        me.srcCursor = value;

        return me;
    },

    //设置重心
    setReg:function(x,y){
        var me = this,
            el = me.el;

        el.regX = x;
        el.regY = y;

        return me;
    },


    render: function () {
        var me = this;

        me.drawReset();

        return me;
    },

    setVisible:function(value){
        var me = this,
            el= me.el;

        el.visible = value;

        return me;
    },

    setHandType: function (value) {
        "use strict";
        var me = this,
            srcHandType = me.handType;

        if (srcHandType != value) {

            me.handType = value;

            me.drawReset();

        }

        return me;
    },

    drawReset: function(){
        var me = this,
            type = me.handType;

        switch (type) {
            case 0: me.draw0(); break;
            case 1: me.draw1(); break;
            case 2: me.draw2(); break;
        }

        return me;
    },



    //正方形
    draw0: function () {
        "use strict";
        var me = this,
            el = me.el,
            g = el.graphics,
            width = me.width,
            height = me.height;


        g.clear();
        g.setStrokeStyle(1);

        g.beginStroke("#a8a8a8");
        g.beginFill('#FFFFFF').drawRect(0, 0, width, height);

        return me;
    },

    //菱形
    draw1: function () {
        var me = this,
            el = me.el,
            g = el.graphics,
            width = me.width,
            height = me.height,
            w2 = width / 2,
            h2 = height / 2;


        g.clear();
        
        g.setStrokeStyle(1);

        g.beginStroke("#a8a8a8");
        g.beginFill('#FFFFFF');

        g.moveTo(w2, 0);
        g.lineTo(width, h2);
        g.lineTo(w2, height);
        g.lineTo(0, h2);

        g.closePath();

        return me;
    },

    //圆形
    draw2: function () {
        var me = this;

        return me;
    }



}, function () {
    var me = this;

    me.renderTo = Mini2.getBody();

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});