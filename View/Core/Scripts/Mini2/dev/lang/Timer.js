

/**
* 定时器
*/ 
Mini2.define('Mini2.Timer', {

    owner: null,

    enabled : false,

    /**
    * 执行的间隔
    */
    interval: 100,

    /**
    * 已经执行的次数
    */
    count: 0,

    /**
    * 限制执行的次数, 0=没有限制
    */
    limit: 0,

    /**
    * 超时,自动停止
    */
    overtime: 0,

    /**
    * 开始执行的时间
    */ 
    timeStart : null,
    
    tick: Mini2.emptyFn,

    
    timer:null,


    /**
    * 停止计时
    */
    stop: function () {
        var me = this;
        me.enabled = false;

        if (me.timer) {
            clearTimeout(me.timer);

            delete me.timer;
        }
        
        return me;
    },
    
    /**
    * 开始计时
    */
    start: function () {
        var me = this;

        if (!me.enabled) {
            me.timeStart = new Date().getTime();
            me.enabled = true;

            me.callTick();
        }
        return me;
    },

    /**
    * 重置
    */
    reset: function(){
        var me = this;

        me.count = 0;
        me.timeStart = new Date().getTime();

        return me;
    },

    /**
    * 重置后再启动
    */
    resetStart:function(){
        var me = this;
        
        me.reset().start();

        return me;
    },

    /**
    * 重置后再启动
    */
    restart: function(){
        var me = this;

        me.reset().start();

        return me;
    },

    /**
    * 调用定时函数
    */
    callTick: function () {

        var me = this,
            owner = me.owner,
            nowDate,
            limit = me.limit,
            overtime = me.overtime,
            result;


        if (!me.enabled) {
            return;
        }

        if (limit > 0 && me.count >= limit) {
            me.stop();
            return;
        }

        if (overtime > 0) {
            nowDate = new Date();
            span = nowDate.getTime() - me.timeStart;

            if (span >= overtime) {
                me.stop();
                return;
            }
        }

        me.timer = setTimeout(function () {

            nowDate = new Date();
            span = nowDate.getTime() - me.timeStart;

            if (span >= me.interval) {

                result = me.tick.call(owner);

                me.count++;

                if (false === result) {
                    me.stop();
                    return;
                }
            }

            me.callTick.call(me);
            
        }, me.interval);

    }

}, function () {
    var me = this;

    Mini2.apply(this, arguments[0]);

    this.start();
});

/**
* 设置定时器, 简化 Mini2.Timer 的配置
* 
* @param {Function} tickFn 触发执行的函数
* @parma {interval} 定时间隔(毫秒), 也可以是参数设置
*/
Mini2.setTimer = function (tickFn, interval) {
    var timer,
        cfg = {};

    if (undefined == interval) {

    }
    else if (Mini2.isArray(interval)) {
        cfg = Mini2.apply(cfg, {
            interval: interval[0],
            limit: interval[1]
        });
    }
    else if (Mini2.isNumber(interval)) {
        cfg.interval = interval;
    }
    else {
        cfg = Mini2.apply(cfg, interval);
    }

    cfg.tick = tickFn;

    timer = Mini2.create('Mini2.Timer', cfg);

    return timer;
}


/**
 * 运行一次的数据区
 */
Mini2.run1_data = {};

/**
 * 运行一次
 * @param code {String} 执行此函数的唯一编码
 * @param interval {int} 时间范围内 (毫秒)
 * @param owner {Object} 执行函数所属的对象
 * @param tickFun {Function} 执行函数
 */
Mini2.awaitRun = function (code, interval, owner, tickFun) {

    var me = this;

    var data = Mini2.run1_data;

    var item = data[code];

    if (item) {

        item.restart();

    }
    else {
        item = Mini2.setTimer(function () {
            tickFun.call(owner);

            delete me.run1_data[code];
            
        }, [interval, 1]);

        item.owner = me;
         
        data[code] = item;
    }

}