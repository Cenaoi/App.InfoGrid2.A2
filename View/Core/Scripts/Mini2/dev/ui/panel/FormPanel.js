
Mini2.define("Mini2.ui.panel.FormPanel", {

    extend: 'Mini2.ui.panel.Panel',

    log: Mini2.Logger.getLogger('Mini2.ui.panel.FormPanel', false),  //日志管理器


    flowDirection: 'topdown',

    /**
     * 查找所有 Mini2 控件
     * @param {domEl} parentEl 上级html节点
     * @param {Array} list 搜索到的集合 
     */
    findItems: function (parentEl, list) {
        var me = this;

        if (!list) {
            list = [];
        }

        var childItems = $(parentEl).children();

        $(childItems).each(function () {
            var childEl = this;

            var data = $(childEl).data('me');

            if (data) {
                list.push(childEl);
                return false;
            }
            else {
                me.findItems(childEl, list);
            }

        });

        return list;
    },

    /**
     * 获取需要绑定的项目
     */
    getBindItems: function () {
        var me = this,
            store = me.store,
            boxTarget = me.boxTarget,
            items = boxTarget.children(),
            len = items.length,
            i = 0,
            dbItems = [];


        items = me.prepareItems(items);

        var childItems = [];

        for (i = 0; i < len; i++) {

            var item = items[i];

            item = me.getItemObj(item);

            if (!item) {
                me.findItems(items[i], childItems);
            }
        }

        items = items.concat(childItems);

        len = items.length;


        for (i = 0; i < len; i++) {
            var item = items[i];

            item = me.getItemObj(item);

            if (!item) {
                continue;
            }


            if (item.isRangeControl) {

                if (item.startDataField || item.endDataField) {
                    dbItems.push(item);
                }

            }
            else if (item.dataField) {
                dbItems.push(item);;
            }


        }

        return dbItems;
    },


    //绑定项目焦点放开触发的事件
    //bindItemFocusout: function () {
    //    var me = this,
    //        log = me.log,
    //        item,
    //        items, len;


    //    items = me.getBindItems();
    //    len = items.length;

    //    for (i = 0; i < len; i++) {
    //        item = items[i];

    //        $(item).muBind('focusout', function () {
    //            me.item_focusout.call(me, this);
    //        });

    //        if (item.isCheckbox) {
    //            console.trace();
    //        }

    //        //如果是复选框, 就特殊处理,一旦值发生变化,直接触发事件
    //        if (item.isCheckbox) {
    //            item.bind('changed', function () {
    //                log.debug("触发事件 changed");
    //                me.setStoreRecrod.call(me, this);
    //            });

    //        }
    //    }
    //},

    // item 焦点离开触发的事件
    item_focusout: function (sender) {
        var me = this


        me.setStoreRecrod(sender)
    },


    /**
    * 设置当前焦点行,某个字段的值
    */
    setCurRectValue: function (control, dataField, value) {
        var me = this,
            curRect,
            store = me.store;


        if (store && store.isStore) {

            curRect = store.getCurrent();

            if (curRect) {

                curRect.set(dataField, value);


                control.setDirty(true);


            }
        }

        return me;
    },


    /**
     * 设置记录
     */
    setRecord: function (record) {

        var me = this;

        if (record) {

            if (record.isModel) {

                console.error('还没支持这种格式...可以加的...', record);

            }
            else {
                me.setObjectRecord(record);
            }
        }

    },

    /**
     * 普通对象
     */
    setObjectRecord: function (record) {
        var me = this,
            conList,
            conValue;

        for (var itemName in record) {

            conList = me.findByDataField(itemName);

            conValue = record[itemName];

            me.setValueToControl(conList, conValue);

        }


    },

    /**
     * 对控件进行赋值
     */
    setValueToControl: function (conList, value) {

        var me = this;

        $(conList).each(function () {
            this.setValue(value);
        });

        return me;
    },


    setStoreRecrod: function (sender) {
        var me = this,
            item = sender;

        if (item.readOnly || !item.isValueChanged()) {

            return me;
        }


        if (item.isRangeControl) {

            if (item.startDataField && item.isValueChanged('start')) {

                var value = item.getStartValue();

                me.setCurRectValue(item, item.startDataField, value);
            }

            if (item.endDataField && item.isValueChanged('end')) {

                var value = item.getEndValue();

                me.setCurRectValue(item, item.endDataField, value);

            }

        }
        else {
            var value = item.getValue();
            me.setCurRectValue(item, item.dataField, value);
        }


    },

    getItemObj: function (obj) {

        var itemObj = null;

        if (obj) {
            if (obj.muid) {
                itemObj = obj;
            }
            else {
                itemObj = $(obj).data('me');
            }
        }

        return itemObj;
    },



    bindStore: function (store) {
        "use strict";
        var me = this,
            log = me.log;

        me.store = store;

        $(store)
            .muBind('update', me, me.store_update)              //监听记录更新事件
            //.muBind('add', me, me.store_add)                    //监听记录添加事件
            //.muBind('datachanged', me, me.store_dataChanged)    //监听数据更改
            //.muBind('refresh', me, me.store_refresh)            //监听刷新事件
            .muBind('locked', me, me.store_locked)
            .muBind('load', me, me.store_load)                  //加载事件

            .muBind('bulkremove', me, me.store_bulkRemove)      //左上角标记移除事件
            .muBind('invalid', me, me.store_invalid)            //出现异常的单元格
            .muBind('clear', me, me.store_clear)                //删除全部记录事件

            .muBind('currentchanged', me, me.store_currentChanged);  //焦点行发生改变的事件


        me.store.onCurrentChanged();

    },

    store_bulkRemove: function (event, ex) {
        "use strict";
        var me = event.data;

        //console.log("store_bulkremove ", ex);
    },

    //出现异常信息
    store_invalid: function (event, ex) {
        "use strict";
        var me = event.data;

        var errs = ex.errors;

        if (errs.length > 0) {
            var items = errs.items;

            for (var itemName in items) {

                var item = items[itemName];

                var errStr = "";

                for (var i = 0; i < item.length; i++) {
                    if (item[i].type == "error") {
                        errStr += item[i].message;
                    }
                }

                //console.error("字段 " + itemName, errStr);

                var conList = me.findByDataField(itemName);


                //console.log("conList = ", conList.length);

                if (conList && conList.length) {
                    var conLen = conList.length;

                    for (var j = 0; j < conLen; j++) {

                        var con = conList[j];


                        con.markInvalid(errStr);
                    }
                }


            }
        }


    },


    //出现异常信息
    store_clear: function (event, ex) {
        "use strict";
        var me = event.data;

        //console.log("store_clear ", ex);
    },

    store_locked: function (event, ex) {
        "use strict";
        var me = event.data;

        //console.log("锁事件")
    },


    store_dataChanged: function (event, ex) {
        "use strict";
        var me = event.data;


        //console.log(ex);
    },



    store_update: function (event, record, operation, modifiedFieldNames) {
        "use strict";
        var me = event.data,
            i, j,
            modifiedField,
            items,
            len,
            isUpdateStyleField = false;


        //log.debug(operation + '  record["ROW_STYLE_JSON"] = ', record.get("ROW_STYLE_JSON"));
        

        if ('COMMIT' == operation) {


            for (i = 0; i < modifiedFieldNames.length; i++) {

                modifiedField = modifiedFieldNames[i];

                if ('ROW_STYLE_JSON' == modifiedField) {

                    var jsonStr = record.get('ROW_STYLE_JSON');

                    if ('' == jsonStr) {
                        continue;
                    }

                    var json = null;

                    try {
                        json = eval('(' + jsonStr + ')');
                    }
                    catch (ex) {
                        log.debug("json 解析数据错误" + jsonStr);
                        continue;
                    }

                    for (var k = 0; k < json.cols.length; k++) {

                        var fieldSX = json.cols[k];

                        //console.log("field = ", fieldSX);

                        items = me.findByDataField(fieldSX.name);

                        var len = items.length;

                        for (j = 0; j < len; j++) {

                            var item = items[j];

                            var msg = "";

                            for (var l = 0; l < fieldSX.msgs.length; l++) {
                                if (l > 0) { msg += "\n"; }

                                msg += fieldSX.msgs[l].message;
                            }

                            item.markInvalid(msg);
                        }


                    }


                }
                else {
                    items = me.findByDataField(modifiedField);

                    len = items.length;

                    for (j = 0; j < len; j++) {

                        var item = items[j];
                        var value = record.get(item.dataField);

                        item.setDirty(false);

                        if (item.oldValue != value) {
                            item.oldValue = item.getValue();

                            item.setValue(value);
                        }

                    }
                }
            }

        }
        else if ('EDIT' == operation) {

            for (i = 0; i < modifiedFieldNames.length; i++) {

                modifiedField = modifiedFieldNames[i];

                items = me.findByDataField(modifiedField);

                len = items.length;

                for (j = 0; j < len; j++) {

                    var item = items[j];
                    var value = record.get(item.dataField);

                    item.setDirty(false);

                    if (item.oldValue != value) {
                        item.oldValue = item.getValue();

                        item.setValue(value);
                    }


                }

            }
        }
        else {
            console.log('operation=', operation);

        }


        var isLocked = record.isLocked();

        me.record_locked(record, isLocked);


        return isUpdateStyleField;
    },


    //根据 dataField 字段进行查找
    findByDataField: function (dataField) {
        "use strict";
        var me = this,
            boxTarget = me.boxTarget,
            items = boxTarget.children(),
            len = items.length,
            newItems = [],
            item, i;

        items = me.prepareItems(items);


        var childItems = [];

        for (i = 0; i < len; i++) {

            var item = items[i];

            item = me.getItemObj(item);

            if (!item) {
                me.findItems(items[i], childItems);
            }
        }

        items = items.concat(childItems);

        len = items.length;

        if (items && items.length == 0) {
            return newItems;
        }


        for (i = 0; i < len; i++) {

            item = items[i];

            item = me.getItemObj(item);

            if (!item || !item.dataField) {
                continue;
            }

            if (dataField != item.dataField) {
                continue;
            }



            newItems.push(item);
        }

        return newItems;
    },

    store_load: function (event, store) {
        "use strict";
        var me = event.data,
            log = me.log,
            i,
            record,
            records = store.data,
            len;
        

    },


    //lockeState : undefined,

    //记录锁状态
    record_locked: function (record, lockeState) {
        var me = this,
            log = me.log,
            store = me.store,
            boxTarget = me.boxTarget,
            items = boxTarget.children(),
            len = items.length,
            i = 0;

        if (me.lockeState === lockeState) {
            return;
        }

        me.lockeState = lockeState;;

        items = me.prepareItems(items);

        var childItems = [];

        for (i = 0; i < len; i++) {

            var item = items[i];

            item = me.getItemObj(item);

            if (!item) {
                me.findItems(items[i], childItems);
            }
        }

        items = items.concat(childItems);

        len = items.length;


        for (i = 0; i < len; i++) {
            var item = items[i];

            item = me.getItemObj(item);

            if (!item) {
                continue;
            }

            if (!item.dataField) {
                //console.error('字段名为空，跳出. fieldLabel=' + item.fieldLabel, item);
                continue;
            }

            if (false === item.allowChangeReadonly) {
                continue;
            }

            if (record.isLockedExclusionField(item.dataField)) {
                item.setReadOnly(false);
            }
            else {
                item.setReadOnly(lockeState);
            }

        }

    },

    store_currentChanged: function (event, index, record) {
        "use strict";
        var me = event.data,
            log = me.log;


        if (index > -1) {


            var boxTarget = me.boxTarget;

            var items = boxTarget.children();

            items = me.prepareItems(items);


            //log.debug('rect', record);
            //log.debug('items', items.length);

            if (items && items.length == 0) {
                return;
            }

            if (!record) {
                return;
            }

            for (var i = 0; i < items.length; i++) {
                var item = items[i];

                item = me.getItemObj(item);

                if (!item) { continue; }

                if (item.isRangeControl) {

                    if (item.startDataField) {
                        var rectValue = record.get(item.startDataField);

                        if (item.setOldValue) {
                            item.setOldValue('start', rectValue);
                        }
                        else {
                            item.startOldValue = rectValue;
                        }

                        item.setValue('start', rectValue);
                    }

                    if (item.endDataField) {

                        var rectValue = record.get(item.endDataField);

                        if (item.setOldValue) {
                            item.setOldValue('end', rectValue);
                        }
                        else {
                            item.endOldValue = rectValue;
                        }

                        item.setValue('end', rectValue);

                    }

                }
                else {
                    if (!item.dataField || !item.setValue) {
                        continue;
                    }


                    var rectValue = record.get(item.dataField);

                    if (item.setOldValue) {
                        item.setOldValue(rectValue);
                    }
                    else {
                        item.oldValue = rectValue;
                    }

                    //console.debug("[          " + item.dataField + "=", rectValue);

                    item.setValue(rectValue);
                }
            }

        }


        if (record) {
            var isLocked = record.isLocked();

            me.record_locked(record, isLocked);
        }


    },

    afterRender: function () {
        var me = this,
            item,
            items,
            len,
            i;

        items = me.getBindItems();
        len = items.length;

        for (i = 0; i < len; i++) {
            item = items[i];


            $(item).muBind('focusout', function () {

                me.item_focusout.call(me, this);
            });


            //如果是复选框, 就特殊处理,一旦值发生变化,直接触发事件
            if (item.isCheckbox || item.isCheckGroup) {

                $(item).muBind('changed', function () {

                    me.setStoreRecrod.call(me, this);
                });

            }

            //先记录一下控件的原始状态
            if (item.getReadOnly()) {
                item.allowChangeReadonly = false;
            }

        }

    },

    render: function () {
        var me = this,
            log = me.log;

        //注入全局加载器, 在脚本执行结束后, 执行 afterRender 函数

        me.baseRender();


        if ('auto' == me.formMode) {

            if (me.store) {
                
                var store = Mini2.data.StoreManager.lookup(me.store);

                me.store = store;

                me.bindStore(store);

            }
        }
        else {

        }

        if (me.isDelayRender) {
            me.delayRenderTimer();
        }

        try {
            Mini2.ui.SecManager.reg(me.secFunCode, me);
        }
        catch (ex) {
            console.error('注册权限错误', ex);
        }
    }
});