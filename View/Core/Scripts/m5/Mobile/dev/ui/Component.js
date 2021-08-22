


//ui 的组件类
Mini2.define('Mini2.ui.Component', {

    extend: false,

    el: null,

    isComponent: true,

    renderTo: Mini2.getBody(),

    onPreInitComponent:function(){

        var me = this;
                
        me.initComponent();
    },



    //初始化组件
    initComponent: Mini2.emptyFn,


    /**
     * 触发初始化
     */
    onPreInit: function () {
        var me = this;

        me.onInit();
    },

    /**
     * 初始化
     */
    onInit: Mini2.emptyFn,


    //事件类集合
    //private {}
    eventSet: null,


    //绑定事件
    bind: function (eventName, fun, data, owner) {
        "use strict";

        var me = this,
            evtSet = me.eventSet,
            evts;

        if (!fun) {
            throw new Error('[' + eventName + '] 必须指定绑定函数.');
        }

        if (!evtSet) {
            me.eventSet = evtSet = {};
        }

        evts = evtSet[eventName];

        if (!evts) {
            evts = evtSet[eventName] = [];
        }


        evts.push({
            fun: fun,
            owner: owner,
            data: data
        });

        return me;
    },

    //移除对应的事件
    off:function(eventName){
        "use strict";
        var me = this,
            eventSet = me.eventSet,
            evts;

        if (eventSet) {

            evts = eventSet[eventName];
            
            if (evts) {
                delete eventSet[eventName];
            }
        }

        return me;
    },


    //触发事件
    on: function (eventName, data, callback) {
        "use strict";

        if (this.eventSet) {

            var me = this,
                i,
                evt,
                fun,
                evtData,
                evtSet = me.eventSet,
                evts = evtSet[eventName] || []

            for (i = 0; i < evts.length; i++) {

                evt = evts[i];

                fun = evt.fun;

                if (!fun || !fun.call) {
                    console.log('函数不存在...');
                    console.log(fun);
                    continue;
                }

                if (evt.data) {
                    evtData = Mini2.clone(evt.data);
                }
                else {
                    evtData = {};
                }


                evtData = Mini2.applyIf(evtData, data)

                fun.call(evt.owner || me, me, evtData, callback);

            }

        }


        return this;
    },

    join:function(array){
        return array.join('');
    },

    $join:function(array){
        var str = array.join('');
        return $(str);
    },



    render: function () {
        var me = this;

        me.baseRender();
    }

}, function () {
    var me = this;

    Mini2.apply(me, arguments[0]);

    me.onPreInitComponent();

});