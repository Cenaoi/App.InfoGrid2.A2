
/// <reference path="define.js" />
/// <reference path="../../../jquery/jquery-1.4.1-vsdoc.js" />

/**
* 加载机
*/
Mini2.define("Mini2.LoaderManager", {

    //单件模式
    singleton: true,


    log: Mini2.Logger.getLogger('Mini2.LoaderManager'),  //日志管理器


    afterRenderItems: [],

    resizeItems: [],

    resize: function (item) {
        var me = this;

        me.resizeItems.push(item);
    },


    ready:function(fun){
        var me = this;

        if (!Mini2.isFunction(fun)) {
            throw new Error('必须是指定 fun 函数对象.');
        }

        me.afterRenderItems.push(fun);
    },

    afterRender: function (item) {
        var me = this;

        if (!Mini2.isFunction( item.afterRender)) {
            throw new Error('这个对象不存在 afterRender 函数.');
        }

        me.afterRenderItems.push(item);

    },

    callItemResize:function(){
        "use strict";
        var me = Mini2.LoaderManager,
            item,
            resizeFn,
            i = 0,
            items = me.resizeItems,
            len = items.length;

        for (; i < len; i++) {
            item = items[i];
            resizeFn = item.resize;


            if (resizeFn) {
                resizeFn.call(item);
            }
        }
    },

    //页面第一次加载的时候，执行的函数
    preResizeFirst: function () {
        var me = Mini2.LoaderManager;

        me.callItemResize();
        setTimeout(me.callItemResize, 50);  //临时解决方法， 50毫秒后重新计算一次
    },


    // 定时器函数
    resizeTimer : null,    

    preResize: function () {
        var me = Mini2.LoaderManager;


        Mini2.awaitRun(me.muid, 100, me, function () {
            me.callItemResize();
            setTimeout(me.callItemResize, 50);  //临时解决方法， 50毫秒后重新计算一次
        });
    },


    preRenderComplete: function () {
        "use strict";
        var me = this,
            log = me.log,
            item,
            i=0,
            items = me.afterRenderItems,
            len = items.length;

        for (; i < len; i++) {
            item = items[i];

            if (Mini2.isFunction(item)) {
                item();
            }
            else if (item.afterRender) {
                item.afterRender.call(item);
            }
        }

        delete me.afterRenderItems;

        if (Mini2.data && Mini2.data.StoreManager) {
            Mini2.data.StoreManager.loader();
        }

        me.preResizeFirst();
        
        $(window).bind('resize', me.preResize);


    }

});


/*
* 加载管理器
*/
Mini2.ready = function (fn) {

    Mini2.LoaderManager.ready(fn);

}




