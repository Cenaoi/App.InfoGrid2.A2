
/// <reference path="../Mini.js" />


/// <reference path="../../../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="FocusManager.js" />
/// <reference path="EventManager.js" />



//图标插件
Mini2.define("Mini2.ui.icon.Plugin", {


    cfg: null,

    buffer: null,

    initComponent: function () {

        var me = this;

        me.buffer = {};

        if (me.cfg) {
            me.setConfig(me.cfg);

            delete me.cfg;
        }
    },

    /**
     * 根据 关键字, 获取按钮配置信息
     */
    getItem: function (btnKey) {
        var me = this,
            buffer = me.buffer,
            item = buffer[btnKey];

        return item;
    },

    /**
     * 模糊查找
     */
    getItemForLink: function (btnKey) {
        var me = this,
            buffer = me.buffer,
            item = null;

        if (Mini2.isBlank(btnKey)) { return item; }

        for (var i in buffer) {

            if (btnKey.indexOf(i) >= 0) {
                item = buffer[i];
                break;
            }

        }


        return item;
    },

    getIcon: function (btnKey) {
        var me = this,
            item = me.getItem(btnKey);

        if (!item) {
            item = me.getItemForLink(btnKey);
        }

        if (item && !Mini2.isBlank(item.icon)) {
            return item.icon;
        }


        return null;
    },

    getIconCls: function (btnKey) {
        var me = this,
            item = me.getItem(btnKey);

        if (!item) {
            item = me.getItemForLink(btnKey);
        }


        if (item && !Mini2.isBlank(item.iconCls)) {
            return item.iconCls;
        }

        return null;
    },

    getCls: function (btnKey) {
        var me = this,
            item = me.getItem(btnKey);

        if (!item) {
            item = me.getItemForLink(btnKey);
        }

        if (item && !Mini2.isBlank(item.cls)) {
            return item.cls;
        }

        return null;
    },


    getItemConfig: function (btnKey) {
        var me = this,
            item = me.getItem(btnKey);

        if (!item) {
            item = me.getItemForLink(btnKey);
        }

        return item;
    },



    /**
     * 设置配置内容
     */
    setConfig: function (cfg) {

        var me = this,
            buffer = me.buffer;

        $(cfg).each(function () {
            var item = this;
            var keys = item.keys;

            var itemCfg = {};

            Mini2.apply(itemCfg, item);


            if (Mini2.isString(keys)) {
                keys = [keys];
            }

            for (var i = 0; i < keys.length; i++) {

                var key = keys[i];

                buffer[key] = itemCfg;
            }
        });

    }

}, function () {
    var me = this;

    Mini2.apply(me, arguments[0]);

    me.initComponent();

});