
/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define("Mini2.ui.panel.AbstractTable", {

    log: Mini2.Logger,  //日志管理器

    StrJoin: function (array) {
        "use strict";
        var str = "";
        for (var i = 0; i < array.length; i++) {
            str += array[i];
        }

        return str;
    },

    renderTo: Mini2.getBody(),



    getStore: function () {
        "use strict";
        var me = this,
            newStore;

        if (!me.store) {
            newStore = Mini2.create('Mini2.data.Store', {
                storeId: "MiniStore_" + Mini2.newId(),
                model: 'Mini2.data.FreeModel'
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
            .muBind('currentchanged', me, me.store_currentChanged);  //焦点行发生改变的事件

    },


    store_invalid: function (event, record, fieldNames) {
        "use strict";
        var me = event.data,
            rowEl,
            rowIndex,
            i,
            col;

        rowEl = me.getRowForRecord(record);
        rowIndex = $(rowEl).index();


        for (i = 0; i < fieldNames.length; i++) {

            col = me.getColumnForDataIndex(fieldNames[i]);

            me.afterCellUpdate(col, rowEl, record, rowIndex, fieldNames);
        }


    },


    getColumnForDataIndex: function (dataIndex) {
        "use strict";
        var me = this,
            col,
            cols = me.m_Cols,
            i = cols.length,
            list = [];

        while (i--) {
            col = cols[i];

            if (col.dataIndex == dataIndex) {
                list.push(col);
            }
        }

        return list;
    },


    //根据记录获取行TR
    getRowForRecord: function (record, onlyFirst) {
        "use strict";
        var me = this,
            rows = [],
            tbodyEl = me.tbodyEl,
            rowRecord,
            rowEl,
            rowsEls = tbodyEl.children("tr");

        $(rowsEls).each(function () {
            rowEl = this;
            rowRecord = $(rowEl).data("record")

            if (rowRecord === record) {
                rows.push(rowEl);

                if (onlyFirst) { return false; }
            }

        });

        return rows;
    },




    //更新记录的显示内容
    afterCellUpdate: function (column, rowEl, record, rowIndex, modifiedFieldNames) {
        "use strict";
        var me = this,
            cellValues,
            columnIndex = column.dataIndex,
            recordIndex = record.index,
            columnRenderer = column.renderer,
            cellEl,
            isModified,
            fieldValue,
            cellInnerEl,
            value;

        cellEl = $(rowEl).children(':eq(' + column.index + ')');

        fieldValue = record.get(columnIndex);



        if (columnRenderer && columnRenderer.call) {
            try {
                value = columnRenderer.call(column, fieldValue, cellValues, cellValues, record, rowIndex, columnIndex, me.store, me);
            }
            catch (ex) {
                //throw new Error('更新记录内容错误。'+ ex.message);
            }
        }
        else {
            value = fieldValue;
        }

        value = Mini2.isBlank(value) ? '&#160;' : value;


        cellInnerEl = $(cellEl).children(".mi-grid-cell-inner");

        if (record.isDisabledForField(columnIndex)) {
            cellInnerEl.html('<hr style="border:none;border-top:1px dashed #b0b0b0;width:90%" />');
        }
        else {
            cellInnerEl.html(value);
        }
        

        isModified = record.isModified(columnIndex);

        $(cellEl).toggleClass('mi-grid-dirty-cell', isModified);





        ////创建错误标记
        //if (record && record.errors && column.dataIndex && column.dataIndex != '') {

        //    if (record.errors) {
        //        column.markInvalid(record, cellEl, column.dataIndex);
        //    }
        //    else {
        //        column.clearInvalid(record, cellEl, column.dataIndex);
        //    }
        //}



        if (column.bindEvent) {
            column.bindEvent.call(column, me, cellEl, recordIndex, columnIndex, null, record, rowEl);
        }

        //创建错误标记
        me.preColumnInvalid(column, cellEl, record);
    },



    store_currentChanged: function (event, index, record) {
        "use strict";
        var me = event.data,
            focusedCls = 'mi-grid-row-focused',
            row,
            rowRecord,
            rows = $(me.tbodyEl).children(),
            lastRow = me.lastRowFocused,
            curRow = null;

        if (index > -1) {
            $(rows).each(function () {
                row = this;
                rowRecord = $(row).data('record');

                if (rowRecord === record) {
                    curRow = row;
                    return false;
                }
            });

            $(curRow).addClass(focusedCls);
        }

        $(lastRow).removeClass(focusedCls);

        me.lastRowFocused = curRow;

    },

    store_clear: function (event) {
        "use strict";
        var me = event.data;

        me.itemRemoteAll();

        if (me.rowCheckColumn) {
            me.rowCheckColumn.updateHeaderState();
        }
        //var rows = $(me.tbodyEl).children();

        //$(rows).remove();
    },

    //删除记录
    store_bulkRemove: function (event, allRecords, indexes, isMove) {
        "use strict";
        var me = event.data,
            Array = Mini2.Array,
            exist,
            record,
            row,
            checkedRecords = me.checkedRecords,
            rows = me.tbodyEl.children('tr');


        $(rows).each(function () {
            row = this;
            record = $(row).data("record");

            exist = Array.contains(allRecords, record);

            if (exist) {
                $(row).remove();
                Array.remove(checkedRecords, record);
                return false;
            }

        });

        me.itemsReset();

    },


    store_update_forRow: function (rowEl, rowIndex, record, modifiedFieldNames) {
        "use strict";
        var me = this,
            i, j, col, cols,
            modifiedField,
            rowStyleType = me.rowStyleType,
            rowStyleField = me.rowStyleField,
            isUpdateStyleField = false;


        for (i = 0; i < modifiedFieldNames.length; i++) {

            modifiedField = modifiedFieldNames[i];


            if (rowStyleType && modifiedField == rowStyleField) {
                isUpdateStyleField = true;
                continue;
            }

            cols = me.getColumnForDataIndex(modifiedField);
            j = cols.length;

            while (j--) {
                col = cols[j];

                me.afterCellUpdate(col, rowEl, record, rowIndex, modifiedFieldNames);
            }
        }

        return isUpdateStyleField;
    },


    store_update_setInvalid: function (rowEl, record) {
        "use strict";
        var me = this,
            log = me.log,
            i, j,
            rowStyleType = me.rowStyleType,
            rowStyleField = me.rowStyleField,
            jsonStr = record.get(rowStyleField),
            json,
            cellEl,
            colStyle, colMsgs,
            colName,
            col, cols;

        if (!jsonStr) {
            return;
        }

        if ('' == jsonStr) {
            return;
        }


        try {
            json = eval('(' + jsonStr + ')');
        }
        catch (ex) {
            log.debug("json 解析数据错误" + jsonStr);
            return;
        }

        var invalidCols = {};

        record.clearInvalidAll();

        me.clearRowInvalid(rowEl, record);

        for (i = 0; i < json.cols.length; i++) {

            colStyle = json.cols[i];
            colMsgs = colStyle.msgs;

            colName = colStyle.name;


            cols = me.getColumnForDataIndex(colName);
            j = cols.length;

            while (j--) {
                col = cols[j];

                cellEl = $(rowEl).children(':eq(' + col.index + ')');

                if (colMsgs && colMsgs.length) {
                    col.markInvalid.call(col, record, cellEl, col.dataIndex, colMsgs);

                    invalidCols[colName] = true;
                }
                else {
                    col.clearInvalid.call(col, record, cellEl, col.dataIndex);
                }
            }
        }



    },


    //清理行异常提示信息
    clearRowInvalid: function (rowEl, record) {
        "use strict";
        var me = this,
            i,
            cellEl,
            col,
            cols = me.m_Cols


        for (i = 0; i < cols.length; i++) {

            col = cols[i];

            cellEl = $(rowEl).children(':eq(' + col.index + ')');

            col.clearInvalid.call(col, record, cellEl, col.dataIndex);
        }



    },


    store_update: function (event, record, operation, modifiedFieldNames) {
        "use strict";
        //alert(record.isModel + "   " + operation + " " + modifiedFieldNames);

        var me = event.data,
            dirtyCellCls = 'mi-grid-dirty-cell',
            cellValues,
            rowIndex,
            rowEl,
            cells,
            modifiedField,
            isUpdateStyleField,
            rows = me.getRowForRecord(record);

        $(rows).each(function () {
            rowEl = this;
            rowIndex = $(rowEl).index();

            if (modifiedFieldNames) {

                isUpdateStyleField = me.store_update_forRow(rowEl, rowIndex, record, modifiedFieldNames);


                if (isUpdateStyleField) {
                    try {
                        me.store_update_setInvalid(rowEl, record);
                    }
                    catch (ex) {
                        console.error('出现特殊错误', ex);
                        alert("处理单元格提示信息失败。\n\n" + ex.message);
                    }
                }
            }
            else {

                console.debug("-------------------------------    ", modifiedFieldNames);

                cells = $(rows).children('.' + dirtyCellCls);

                $(cells).removeClass(dirtyCellCls);
            }
        });




        if (me.rowCheckColumn) {
            me.rowCheckColumn.updateHeaderState();
        }
    },



    store_refresh: function (event, store) {
        "use strict";
        var me = event.data,
            i,
            len,
            rowCheckColumn = me.rowCheckColumn,
            records;

        me.itemRemoteAll();

        records = me.store.data;
        len = records.length;
        for (i = 0; i < len; i++) {
            me.itemAdd(records.get(i));
        }

        me.itemsReset();

        if (rowCheckColumn) {
            rowCheckColumn.updateHeaderState();
        }
    },


    //延迟显示的数据
    delayData: {

    },



    
    //循环加载
    loopLoadItem: function () {
        "use strict";
        var me = this,
            i, n,
            record,
            delayData = me.delayData,

            initCount = delayData.initCount || 20; //初始时候默认加载20条;

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


    /**
     * 延迟加载
     *
     */
    delayLoad: true,

    store_load: function (event, store) {
        "use strict";
        var me = event.data,
            i,
            record,
            records = store.data,
            len,
            rowCheckColumn = me.rowCheckColumn;


        me.itemRemoteAll();

        len = records.length;


        //延迟加载
        if (me.delayLoad) {

            me.delayData = {
                i: 0,
                len: len,
                initCount: 20,
                items: records
            };

            me.loopLoadItem();
        }
        else {

            if (records.items) {
                me.itemInsert(0, records.items);
            }
            else {
                me.itemInsert(0, records);
            }
            
        }

        if (rowCheckColumn) {
            rowCheckColumn.updateHeaderState();
        }

    },

    store_dataChanged: function (event, ex) {
        "use strict";
        var me = event.data,
            editor = me.editor;

        if (editor) {
            editor.hide();
        }
    },

    store_add: function (event, index, out) {
        "use strict";
        var me = event.data,
            i,
            rowCheckColumn = me.rowCheckColumn;

        for (i = 0; i < out.length; i++) {
            me.itemInsert(index + i, out[i]);
        }

        if (rowCheckColumn) {
            rowCheckColumn.updateHeaderState();
        }
    },


    //事件类集合
    //private 
    //eventSet:{},


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
    off: function (eventName) {
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
    on: function (eventName, data) {
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

                fun.call(evt.owner || me, me, evtData);

            }

        }


        return this;
    }


});