
//菜单管理类
Mini2.define('Mini2.ui.menu.Manager', {

    singleton: true,


    items: null,

    /*
    * 延时显示, 默认 200毫秒.
    */
    delay: 300,

    /*
    * 当前显示的菜单
    */
    curMenu: null,

    timer: null,

    onInit:function(){
        var me = this;

        me.items = {};
    },

    show:function(menu){
        var me = this,
            cur = me.curMenu,
            timer = me.timer;

        if (cur === menu) {
            return;
        }

        if (me.curMenu) {
            me.curMenu.callHide();
        }

        me.curMenu = menu;

        if (!timer) {
            me.timer = timer = Mini2.setTimer(function () {
                if (me.curMenu) {
                    me.curMenu.callShow();
                }
            }, {
                interval: me.delay,
                limit:1
            });
        }
        else {
            timer.reset().start();
        }
                       

    },

    //注册
    register: function (menu) {
        var me = this,
            items = me.items;

        //menu.bind('showed',function(){

        //    me.hideAll(this);

        //});

        menu.bind('showing', function () {

            me.hideAll(this);

        });
        
        items[menu.muid] = menu;
    },

    //撤销注册
    unregister: function (menu) {
        var me = this,
            items = me.items;

        delete items[menu.muid];
    },


    //隐藏全部
    hideAll: function (self) {

        var me = this,
            items = me.items;

        for (var i in items) {
            var item = items[i];
            
            if (item == self) {
                continue;
            }

            if (item === me.curMenu) {
                me.curMenu = null;
            }

            item.hide();
        }

    }

}, function () {

    var me = this;

    me.onInit();

});