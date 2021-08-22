

Mini2.define('Mini2.EventBuilder', {

    /**
    * 触发事件
    *
    * @param {String} eventName 事件名称
    * @parma {Object} data 事件附带的数据
    */
    trigger : function (eventName, data) {
        "use strict";
        var me = this,
            sender,
            i,
            event = '_EVENT_' + eventName,
            funs,
            fun,
            cancel;

        // console.debug('触发事件', eventName);
        funs = me[event];


        if (funs) {
            for (i = 0; i < funs.length; i++) {
                fun = funs[i];

                if (fun.sender) {
                    sender = fun.sender;
                    fun = fun.fun;
                }
                else {
                    sender = me;
                }

                cancel = fun.call(sender, data);

                if (true === cancel) {
                    break;
                }
            }
        }

        return cancel;
    },

    /**
    * 绑定事件
    *
    * @param {String} eventName 事件名称
    * @param {Object} trigger 触发源
    * @param {Function} fun 函数
    * 
    */
    on: function (eventName, sender, fun) {
        "use strict";
        var me = this,
            event = '_EVENT_' + eventName,
            funs = me[event];

        if (typeof sender === 'function') {
            fun = sender;
            sender = null;
        }

        if (!Mini2.isFunction(fun)) {
            throw new Error('必须指定一个函数类型.');
        }

        if (!funs) {
            me[event] = funs = [];
        }


        if (sender) {
            fun = {
                sender:sender,
                fun: fun
            };
        }

        funs.push(fun)

        return me;
    }
})