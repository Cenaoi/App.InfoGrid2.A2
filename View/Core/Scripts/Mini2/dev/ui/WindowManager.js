/// <reference path="../Mini.js" />
/// <reference path="../lang/Array.js" />

//页面
Mini2.define('Mini2.ui.Page', {

    singleton: true,

    margin: {
        top: 40
    },

    curMax:{
        con:null,
        srcWidth:0,
        srcHeight:0,
        srcTop:0,
        srcLeft:0
    },

    

    //初始化组件
    initComponent: function () {
        var me = this;


    },

    //是否有最大化的控件
    hasMaxControl:function(){
        return !!this.curMax.con;
    },

    resize: function () {
        "use strict";
        var me = this,
            cur = me.curMax,
            con = cur.con;

        if (con) {


            var win = $(window);
            con.setSize(undefined, win.height() - me.margin.top);

        }
    },

    //设置当前最大化的控件
    maxControl: function (con) {
        "use strict";
        var me = this,
            cur = me.curMax,
            el = $;
        
        if (con.el) {
            el = $(con.el);
        }

        if (el.hasClass('mi-state-max')) {
            return;
        }

        el = con.el;

        try{
            cur.con = con;
            cur.srcHeight = con.getHeight();
            cur.srcWidth = con.getWidth();
            cur.srcTop = con.getTop();
            cur.srcLeft = con.getLeft();

            el.addClass('mi-state-max');

            var win = $(window);
            con.setSize(undefined, win.height() - me.margin.top);
        }
        catch (ex) {
            var miType = con.getType();
            console.log('最大化错误. con.fullName=' + miType.fullName);
            console.log(ex);
        }

        //log.debug("放大");
        
    },

    
    //恢复控件
    resetMaxControl: function (con) {
        "use strict";
        var me = this,
            cur = me.curMax,
            el = $;

        if (!cur.con ) { return; }

        if (con.el) {
            el = $(con.el);
        }

        //log.debug("缩小");

        el.removeClass('mi-state-max');

        con.setLeft(cur.srcLeft)
            .setTop(cur.srcTop)
            .setSize(cur.srcWidth, cur.srcHeight);

        cur.con = null;


    }


});


//弹出窗口管理器
Mini2.define('Mini2.ui.WindowManager', {


    //单件模式
    singleton: true,

    items: [],

    defaultZIndex :19000,

    zIndex: 19000,

    //
    isUpdateLayout:false,

    //注册弹出的窗体
    regWin: function (win) {
        "use strict";
        var me = this,
            items = me.items,
            len = items.length;

        Mini2.Array.add(items, win);

        win.setZIndex(me.zIndex + len * 20);

        //log.debug("Win 数量:" + me.items.length);
    },

    //移动窗体到最前方
    moveFirst: function (win) {
        "use strict";
        var me = this,
            item,
            i,
            len,
            index,
            items = me.items;
            
        index = Mini2.Array.lastIndexOf(items, win);
        len = items.length;

        if (index == len - 1) {
            return;
        }

        for (i = index; i < len - 1; i++) {
            items[i] = items[i + 1];
        }

        items[len - 1] = win;

        for (i = index; i < len; i++) {
            item = items[i];

            item.setZIndex(me.zIndex + i * 20);
        }

    },

    //删除窗体
    removeWin: function (win) {
        "use strict";
        var me = this,
            items = me.items;

        Mini2.Array.remove(items, win);

        if (!items.length) {
            me.zIndex = me.defaultZIndex;
        }

        //log.debug("Win 数量:" + me.items.length);
    }
});
