


/// <reference path="../Mini2.js" />
/// <reference path="../../../../jquery/jquery-1.4.1-vsdoc.js" />


Mini2.define('Mini2.collection.ArrayList', {



    log: Mini2.Logger,  //日志管理器

    constructor: function (config, getFn) {
        "use strict";
        var me = this,
            args = arguments;

        me.length = 0;
        me.items = [];

        if (Mini2.isArray(args[0])) {

            me.items = args[0];

            me.length = me.items.length;

        }


        if (Mini2.isFunction(getFn)) {
            me.getFun = getFn;
        }
    },


    getCount: function () {

        return this.items.length;
    },

    add: function (item) {
        "use strict";
        var me = this,
            srcItems = me.items;


        if (item && item.length) {
            me.items = srcItems = srcItems.concat(item);
        }
        else {
            srcItems.push(item);
        }

        me.length = srcItems.length;

    },

    erase: function (index, removeCount) {



    },

    insert: function (index, item) {
        "use strict";
        var me = this,
            subLen = 0,
            lastList,
            items = me.items,
            i, n;

        if (index > items.length) {
            index = items.length;
        }

        lastList = me.getRange(index, null);

        if (typeof item == 'array') {

            for (i = 0; i < item.length; i++) {
                n = i + index;
                items[n] = item[i];
            }

            subLen = index + item.length;

        }
        else {
            items[index] = item;

            subLen = index + 1;

        }


        for (i = 0; i < lastList.length; i++) {
            n = subLen + i;

            items[n] = lastList[i];
        }


        me.length = items.length;
    },

    remoteAt: function (index) {
        "use strict";
        var me = this,
            items = me.items,
            item;

        if (index < me.length && index >= 0) {

            item = items[index];

            items.splice(index, 1);

            me.length = items.length;

            return item;
        }

        return false;
    },

    removeAll: function () {
        "use strict";
        var me = this;

        while (me.length) {
            me.removeAt(0);
        }


        me.length = me.items.length;
    },

    remove: function (item) {
        "use strict";
        var me = this,
            curItem,
            i = 0,
            n = -1,
            items = me.items,
            len = items.length;

        for (; i < len; i++) {

            curItem = items[i];

            if (curItem === item) {
                n = i;
                break;
            }
        }

        me.remoteAt(n);


        me.length = items.length;

        return n;
    },

    removeRange: function (start, end) {
        "use strict";
        var me = this,
            items = me.items,
            range = [],
            len = items.length,
            tmp, reverse;


        if (len < 1) {
            return range;
        }

        if (start > end) {
            reverse = true;
            tmp = start;
            start = end;
            end = tmp;
        }

        if (start < 0) {
            start = 0;
        }

        if (end == null || end >= len) {
            end = len - 1;
        }

        range = items.splice(start, end + 1);


        me.length = items.length;

        return range;
    },

    getByProp: function (propName, value) {
        "use strict";
        var me = this,
            item = null,
            items = me.items,
            curItem,
            c,i=0;


        for (; i < items.length; i++) {
            curItem = items[i];
            c = curItem[propName];
            if (c == value) {
                item = curItem;
                break;
            }
        }


        return item;
    },

    get: function (index) {
        "use strict";
        var me = this,
            item = null;

        item = me.items[index];

        return item;
    },

    set: function (index, value) {

        this.items[index] = value;
    },

    first: function () {

        return this.items[0];
    },

    last: function () {

        return this.items[this.length - 1];
    },




    getRange: function (start, end) {
        "use strict";
        var me = this,
            items = me.items,
            range = [],
            len = items.length,
            tmp, reverse;



        if (len < 1) {
            return range;
        }

        if (end != undefined && end != null && start > end) {
            reverse = true;
            tmp = start;
            start = end;
            end = tmp;
        }


        if (start < 0) {
            start = 0;
        }

        if (end == null || end >= len) {
            end = len - 1;
        }


        range = items.slice(start, end + 1);

        if (reverse && range.length) {
            range.reverse();
        }
        return range;
    },

    //查找第一个
    filterFirstBy: function (fn, scope) {
        "use strict";
        var me = this,
            newObj = null,
            items = me.items,
            length = items.length,
            i=0;


        for (; i < length; i++) {
            if (fn.call(scope || me, items[i])) {
                newObj = items[i];
                break;
            }
        }

        return newObj;
    },

    //查找全部
    filterBy: function (fn, scope) {
        "use strict";
        var me = this,
            newList = new Mini2.collection.ArrayList(),
            items = me.items,
            length = items.length,
            i=0;


        for (; i < length; i++) {
            if (fn.call(scope || me, items[i])) {
                newList.add(items[i]);
            }
        }

        return newList;
    },

    findIndexBy: function (fn, scope, start) {
        "use strict";
        var me = this,
            item,
            items = me.items,
            length = items.length,
            i = start,
            index = -1;

        if (start == undefined) {
            i = 0;
        }


        for (; i < length; i++) {

            if (fn.call(scope || me, items[i])) {
                index = i;
                break;
            }
        }

        return index;
    },

    toArray: function () {
        return this.items;
    }


}, function (args) {
    "use strict";
    var me = this,
        newArgs = [],
        srcArgs = arguments,
        i=0;

    for (; i < srcArgs.length; i++) {
        newArgs[i] = srcArgs[i];
    }

    me.muid = Mini2.getIdentity();

    me.constructor(newArgs[0], newArgs[1]);

});