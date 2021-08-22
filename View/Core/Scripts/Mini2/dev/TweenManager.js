

/**
* 动画管理器
*/
Mini2.TweenManager = new function () {
    "use strict";

    var TweenManager = this;

    Mini2.apply(TweenManager, {

        log: Mini2.Logger.getLogger('Mini2.TweenManager'),  //日志管理器

        items: {},

        len: 0,

        /**
        * 运行中
        */
        isRunning : false,

        /**
        * 添加动画进程
        */
        add: function (tween) {
            var me = this,
                items = me.items,
                id = tween.muid;

            items[id] = tween;

            me.len++;

            if (!me.isRunning) {
                me.isRunning = true;
                requestAnimationFrame(function (time2) {
                    me.animate(time2);
                });
            }

            return me;
        },

        /**
        * 删除动画进程
        */
        remove: function (tween) {
            var me = this,
                items = me.items,
                id = tween.muid;

            if (items[id]) {
                delete items[id];

                me.len--;
            }

            return me;
        },

        animate:function(time){
            var me = this;

            if (me.len <= 0) {
                me.isRunning = false;
                return;
            }

            requestAnimationFrame(function (time2) {
                me.animate(time2);
            });

            TWEEN.update(time);
            
        }
        
        //requestAnimationFrame(animate);

        //    function animate(time) {
        //        requestAnimationFrame(animate);
        //        TWEEN.update(time);
        //    }
        


    });


}