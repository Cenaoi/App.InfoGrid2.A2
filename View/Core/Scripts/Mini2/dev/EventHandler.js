/// <reference path="define.js" />
/// <reference path="../../../jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define('Mini2.ui.EventHandler', {

    listenerList: null,

    owner: null,

    //事件名
    eventName: '',

    //空事件
    isEmpty: false,


    /**
    * 添加回调函数
    *
    * @param {Function} fn 回调函数
    */
    addListener: function (fn) {

        var me = this;

        me.listenerList.push(fn);

    },

    /**
    * 删除回调函数
    *
    * @param {Function} fn 回调函数
    */
    removeListener: function (fn) {

        var me = this;

    },

    /**
    * 回调函数的数量
    *
    * @return {Number} 
    */
    getCount: function () {
        var me = this;

        return me.listenerList.length;

    },

    trigger: function (e) {
        "use strict";
        var me = this,
            i,
            fun,
            funList = me.listenerList,
            len = funList.length;

        e = e || {};

        e.sender = me.owner;

        //alert(me.eventName + " = " + fnList.length);

        for (i = 0; i < len; i++) {
            fun = funList[i];
            fun(e);
        }


        //        delete e;
    },

    /**
    * 绑定回调函数
    *
    */
    on: function (fn) {
        "use strict";
        var me = this;

        me.listenerList.push(fn);

        //console.log("EventHeadle.on(fn)    muid = " + me.muid);
    },

    un: Mini2.emptyFn

}, function () {
    var me = this;

    me.listenerList = [];

});

Mini2.employEventHandler = Mini2.create('Mini2.ui.EventHandler', { isEmpty: true });