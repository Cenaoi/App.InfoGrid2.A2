
/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define("Mini2.ui.panel.AbstractDataView", {

    log: Mini2.Logger,  //日志管理器


    extend: 'Mini2.ui.Component',


    renderTo: Mini2.getBody(),

    getStore: function () {
        "use strict";
        var me = this,
            newStore;

        if (!me.store) {
            newStore = Mini2.create('Mini2.data.Store', {
                storeId: "MiniStore_" + Mini2.newId(),
                model: 'Mini2.data.FreeModel',
                data : me.data
            });

            me.bindStore(newStore);
        }

        return me.store;

    },

    bindStore: function (store) {
        "use strict";
        var me = this;


        me.store = store;

        $(store)
            .muBind('update', me, me.store_update)              //监听记录更新事件
            .muBind('add', me, me.store_add)                    //监听记录添加事件
            .muBind('datachanged', me, me.store_dataChanged)    //监听数据更改
            .muBind('refresh', me, me.store_refresh)            //监听刷新事件
            .muBind('load', me, me.store_load)                  //加载事件
            .muBind('bulkremove', me, me.store_bulkRemove)      //左上角标记移除事件
            .muBind('invalid', me, me.store_invalid)            //出现异常的单元格
            .muBind('clear', me, me.store_clear)                //删除全部记录事件
            .muBind('currentchanged', me, me.store_currentChanged)  //焦点行发生改变的事件
            .muBind('beginloaded',me, me.store_beginloaded)
            .muBind('endloaded', me,me.store_endloaded); //数据加载完后, 触发的事件

        me.resetEmptyMsg();
    },


    /**
     * 根据记录获取行TR
     * @param {object} record 记录
     * @param {string} findTag .查找标记  first=只找第一个
     */
    getItemBoxForRecord: function (record, findTag) {
        "use strict";
        var me = this,
            boxList = [],
            itemlistEl = me.itemlistEl,
            rowRecord,
            onlyFirst = ('first' === findTag),
            rowEl,
            rowsEls = itemlistEl.children(".mi-dataview-item");

        $(rowsEls).each(function () {
            rowEl = this;
            rowRecord = $(rowEl).data("record")

            if (rowRecord === record) {
                boxList.push(rowEl);

                if (onlyFirst) { return false; }
            }

        });

        return boxList;
    },



    store_invalid: function (event, record, fieldNames) {
        "use strict";

        console.debug("store_invalid");
    },


    /**
     * 第一次焦点行
     */
    firstStoreCurrentChanged: true,

    store_currentChanged: function (event, index, record) {
        "use strict";
        var me = event.data,
            itemBox ;

        if (!record) {
            
            if (!me.readonly) {
                me.setFocusItem(null);
            }

            return;
        }

        itemBox = me.getItemBoxForRecord(record);

        if (me.firstStoreCurrentChanged && 'none' != me.checkedMode && itemBox.length) {
            
            me.firstStoreCurrentChanged = false;
            me.itemSelect(itemBox[0]);
        }

        if (!me.readonly) {

            me.setFocusItem(itemBox);
        }

    },

    store_clear: function (event) {
        "use strict";
        var me = event.data;

        me.firstStoreCurrentChanged = true;

        me.itemRemoteAll();

        me.resetEmptyMsg();
    },



    //删除记录
    store_bulkRemove: function (event, allRecords, indexes, isMove) {
        "use strict";

        console.debug("store_bulkRemove");

        var me = event.data,
            boxTarget = me.boxTarget,
            Array = Mini2.Array,
            exist,
            record,
            row,
            //checkedRecords = me.checkedRecords,
            rows = boxTarget.children(".mi-dataview-item");


        $(rows).each(function () {
            row = this;
            record = $(row).data("record");

            exist = Array.contains(allRecords, record);

            if (exist) {
                $(row).remove();
                //Array.remove(checkedRecords, record);
                //return false;
            }

        });

        me.resetItmesIndex();

        me.resetEmptyMsg();
    },




    

    store_update: function (event, record, operation, modifiedFieldNames) {
        "use strict";
        var me = event.data;


        //alert(record.isModel + "   " + operation + " " + modifiedFieldNames);


        if ('EDIT' == operation) {

            var fields = modifiedFieldNames;
            var len = fields.length;

            var newId;

            var boxEl = me.getItemBoxForRecord(record, 'first');

            boxEl = $(boxEl);

            var items = me.getBindItems(boxEl);

            $(items).each(function () {

                var item = this;

                if (Mini2.Array.contains(fields, item.dataField, true)) {

                    var value = record.get(item.dataField);


                    item.setDirty(false);

                    if (item.oldValue != value) {
                        item.oldValue = item.getValue();

                        item.setValue(value);
                    }


                }


            });



            //boxEl.html('');
            
            //var itemInnerStr = me.renderItemInner(0, record);
            
            //boxEl.append(itemInnerStr);

            //var cmdEls = boxEl.find('*[command],a[target]');

            //if (cmdEls.length) {
            //    cmdEls.muBind('click', function () {
            //        return me.item_command(this, boxEl);
            //    });
            //}
        }

    },



    store_refresh: function (event, store) {
       

        console.debug("store_refresh");
    },


    //延迟显示的数据
    delayData: {

    },

    
    //循环加载
    loopLoadItem: function () {
        "use strict";
        var me = this,
            i, n,
            initCount = 20, //初始时候默认加载20条
            record,

            delayData = me.delayData;


        if (delayData.i >= delayData.len) {
            return;
        }
        
        i = delayData.i;

        if (i < initCount) {
            n = delayData.len;

            if (n > initCount) { n = initCount; }

            for (i = 0; i < n; i++) {
                delayData.i++;

                record = delayData.items.get(i);

                me.itemInsert(i, record);
            }
        }
        else {
            delayData.i++;

            record = delayData.items.get(i);

            me.itemInsert(i, record);
        }

        setTimeout(function () {
            me.loopLoadItem();
        }, 1);

    },

    store_load: function (event, store) {
        var me = event.data,
            store = this,
            i,
            record,
            records = store.data,
            len;

        
        if (store.isIncr) {

            var recrods = store.curLoadData;

            me.itemInsert(i, recrods);

        }
        else {

            me.itemRemoteAll();

            len = records.length;

            me.delayData = {
                i: 0,
                len: len,
                items: records
            };

            me.loopLoadItem();
        }

        me.resetEmptyMsg();
    },


    /**
    * 特殊标记
    */
    __lastTimeStamp: null,

    store_dataChanged: function (event, ex) {
        "use strict";        
        var me = event.data,
            store = this;
        
        if (me.__lastTimeStamp === event.timeStamp) {
            return;
        }

        me.__lastTimeStamp = event.timeStamp;

        console.debug("发生变哈流....");

        me.resetEmptyMsg();
    },

    store_add: function (event, index, out) {
        "use strict";
       
        var me = event.data,
            i;


        console.debug("添加记录.....", out[i]);

        for (i = 0; i < out.length; i++) {
            me.itemInsert(index + i, out[i]);
        }

        me.resetEmptyMsg();
    },

    store_beginloaded:function(event,ex){
        var me = event.data;


        me.isLoading = true;

    },

    store_endloaded: function (event, ex) {

        var me = event.data;

        me.isLoading = false;
        
        console.debug(" $(itemlistEl).length = ", $(me.itemlistEl).children().length);

        me.resetEmptyMsg();
    }

});