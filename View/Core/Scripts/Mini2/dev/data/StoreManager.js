
/// <reference path="../Mini2.js" />
/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />


Mini2.define('Mini2.data.StoreManager', {

    singleton: true,

    log: Mini2.Logger.getLogger('Mini2.data.StoreManager'),

    data: [],

    lookup: function (store) {

        var me = this,
            storeObj = store;

        if (typeof store == 'string') {
            storeObj = me.lookup_ForId(store);
        }

        return storeObj;
    },

    lookup_ForId: function (storeId) {
        var me = this,
            i,
            data = me.data,
            len = data.length,
            store;

        for (i = 0; i < len; i++) {

            store = data[i];

            if (storeId == store.storeId) {

                return store;
            }

        }

        return null;
    },

    //加载以后触发的事件
    loader: function () {
        var me = this,
            i,
            data = me.data,
            len = data.length,
            store;

        for (i = 0; i < len; i++) {
            store = me.data[i];

            store.onLoader();
        }

    },

    regStore: function (store) {

        var me = this,
            data = me.data;

        data.push(store);

    }
});