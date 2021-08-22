/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="Template.js" />
/// <reference path="Pagination.js" />


/**
 * 可编辑的表格控件
 */
Mini.ui.DataGridView = function (options) {


    var defaults = {
        id: '',
        dataStoreStatusId: '',
        selectedStatusId: '',
        focusConId: '',

        dataKeys: '',
        fixedFields: '',
        lockedKey: '',
        readOnly: false,
        oddClass: ''
    };

    //模板对象
    var m_TemplateObj;

    //分页对象
    var m_PaginatorObj;

    var m_ResetEvent = new Array();

    var m_FocusRow = null;
    var m_FocusCell = null;    //拥有焦点的单元格
    var m_FocusPk = null;  //拥有焦点的主键
    var m_FocusRowIndex = -1;    //焦点行的记录

    var m_DataStore;    //数据仓库
    var m_SelectedStore;

    var m_EventFocusedChanged = new Array();

    var m_FocusedItem = null;

    //编辑控件对象
    var m_CurEditorControl = null;

    var m_ColumnEditor = {};

    this.setColumnEditor = function (columnId, editorObj) {
        /// <summary>设置列的编辑器</summary>
        /// <param name="columnId" type="string">列唯一ID</param>
        /// <param name="editorObj" type="object">单元格</param>

        m_ColumnEditor[columnId] = editorObj;

    }

    this.getColumnEditor = function (columnId) {
        /// <summary>获取列当前可编辑的控件</summary>
        /// <param name="columnId" type="string">列唯一ID</param>

        return m_ColumnEditor[columnId];
    }



    this.setCurEditorControl = function (con) {
        /// <summary>设置当前可编辑的控件</summary>
        /// <param name="con" type="conObject">控件名称</param>

        if (m_CurEditorControl == con) {
            return;
        }

        try {
            if (m_CurEditorControl && m_CurEditorControl.hide) {
                m_CurEditorControl.hide();
            }
        }
        catch (ex) {

        }

        m_CurEditorControl = con;
    }

    this.stopEditor = function () {
        /// <summary>取消编辑</summary>

        if (m_CurEditorControl == null) { return; }
        var con = m_CurEditorControl;
        if (con && con.hide) {
            if (con.isSetValue) {
                con.isSetValue(true);
            }
            con.hide();
        }
        m_CurEditorControl = null;
    }

    function onStopEditor() {
        if (m_CurEditorControl == null) { return; }
        var con = m_CurEditorControl;
        if (con && con.hide) {
            if (con.isSetValue) {
                con.isSetValue(true);
            }
            con.hide();
        }
        m_CurEditorControl = null;
    }

    this.getLockedKey = function () {
        /// <summary>获取锁行的字段名称</summary>

        return defaults.lockedKey;
    }


    this.focusedChanged = function (fn) {
        /// <summary>设置焦点发生变化，执行的事件</summary>
        /// <param name="fn" type="function">执行的方法</param>

        m_EventFocusedChanged.push(fn);
    }


    this.readOnly = function (value) {
        /// <summary>获取或设置 DataGridView 只读状态</summary>
        /// <param name="value" type="bool">开关值</param>

        if (value != undefined) {
            defaults.readOnly = value;

            if (!value) {
                this.stopEditor();
            }
        }
        else {
            return defaults.readOnly;
        }
    }


    function onFocusedChanged(e) {
        /// <summary>触发焦点发生变化的事件</summary>
        /// <param name="e" type="object">参数对象</param>

        for (var i = 0; i < m_EventFocusedChanged.length; i++) {
            var fn = m_EventFocusedChanged[i];

            try {
                fn(this, e);
            }
            catch (ex) {
                alert("DataGridView.onFocusedChanged(...)   " + ex.Message);
            }
        }
    }

    function isRowLocked(itemData) {
        /// <summary>判断行锁状态</summary>
        /// <param name="itemData" type="object">数据对象</param>

        var lockedKey = defaults.lockedKey;

        if (lockedKey && lockedKey != "") {

            var lockedValue = itemData[lockedKey]

            if (lockedValue == true
                || lockedValue == "1"
                || lockedValue == 'Y'
                || lockedValue == 1
                || lockedValue == "true") {

                return true;
            }
        }

        return false;
    }


    this.clearDataStoreStatus = function () {
        /// <summary>清理数据仓库</summary>

        m_DataStore.rows = {};

        if (defaults.dataStoreStatusId && defaults.dataStoreStatusId != '') {
            $(defaults.dataStoreStatusId).val(Mini.JsonHelper.toJson(m_DataStore));
        }
    };

    this.clearSelectedStatus = function () {
        /// <summary>清理被选中的行</summary>

        m_SelectedStore.rows = {};

        if (defaults.selectedStatusId && defaults.selectedStatusId != '') {
            $(defaults.selectedStatusId).val(Mini.JsonHelper.toJson(m_SelectedStore));
        }
    };

    this.setDataStore = function (ds) {
        /// <summary>设置 DataGridView 对应的数据仓库</summary>

        m_DataStore = ds;
    };

    this.getDataStore = function () {
        /// <summary>获取 DataGridView 对应的数据仓库</summary>

        return m_DataStore;
    };

    this.getSelectedStoreID = function () {

        return defaults.selectedStatusId;
    };

    this.getSelectedStore = function () {

        return m_SelectedStore;
    };

    function getRowPkValue(row) {
        /// <summary>获取行的主键值</summary>

        var itemData = $(row).data("itemData");

        var keys = m_DataStore.dataKeys;
        var pkValue;

        if (keys == "$P.guid") {
            pkValue = $(row).data("TemplateItemGuid");
        }
        else {
            pkValue = itemData[keys];
        }

        return pkValue;
    }

    this.getItems = function () {

        return m_TemplateObj.getItems();
    }


    this.getItem = function (itemIndex) {
        /// <summary>获取焦点行</summary>
        /// <param name="itemIndex" type="int">行索引</param>
        /// <returns type="" ></returns>

        if (m_TemplateObj.itemCount() == 0) {
            return null;
        }

        return m_TemplateObj.get(itemIndex);
    }


    this.setItemFocus = function (itemIndex) {
        /// <summary>设置焦点行</summary>
        /// <param name="itemIndex" type="int">行索引</param>
        /// <returns type="" ></returns>

        var count = m_TemplateObj.itemCount();

        if (count == 0) {
            return;
        }

        if (itemIndex >= count) {
            itemIndex = count - 1;
        }

        var item = this.getItem(itemIndex);

        $(item).children("td:first").mousedown();

    }



    this.fillDataStore = function () {
        /// <summary>把所有记录填充到数据仓库</summary>

        var rowList = m_TemplateObj.getItems();

        var ds = m_DataStore;

        if (!ds.rows) {
            ds.rows = {};
        }

        this.stopEditor();

        $(rowList).each(function () {

            var itemData = $(this).data("itemData");

            var keys = ds.dataKeys;
            var pkValue;
            var pkGuid;

            pkValue = getRowPkValue(this);
            pkGuid = "PK_" + pkValue;


            if (ds.rows[pkGuid] == undefined) {
                ds.rows[pkGuid] = { state: "unchanged", pk: pkValue, fs: {}, rowIndex: -1, cells: {} }
            }

            var item = ds.rows[pkGuid];

            for (var i in itemData) {
                item.fs[i] = itemData[i];
            }

        });


        if (defaults.dataStoreStatusId && defaults.dataStoreStatusId != '') {
            $(defaults.dataStoreStatusId).val(Mini.JsonHelper.toJson(ds));
        }


    }

    this.setTemplate = function (templateObj) {
        /// <summary>设置 DataGridView 对应的模板对象</summary>
        /// <param name="templateObj" type="object">表格模板对象</param>

        if ($(templateObj).length == 0) {
            alert("DataGridView 模板错误! \n\n函数: DataGridView.setTemplate(...)");
            return;
        }

        this.stopEditor();

        m_TemplateObj = templateObj;

        templateObj.itemsChanged(function () {

            onStopEditor();

            try {

                var emptyRow = $('#' + defaults.id + ' .empty_row:first');

                if (templateObj.itemCount && templateObj.itemCount() == 0) {
                    $(emptyRow).show();
                }
                else {
                    $(emptyRow).hide();
                }
            }
            catch (ex) {
                alert("DataGridView.setTemplate();\n\n" + ex.Message);
            }
        });

        templateObj.itemAdded(function (sender, row, itemGuid) {

            onStopEditor();
            setCellFocusMousedownEvent(row);

            var isLocked = isRowLocked($(row).data("itemData"));

            if (isLocked) {
                var rowHeader = $(row).children(".RowHeader");
                $(rowHeader).addClass("Locked");
            }


        });

        templateObj.itemRemoved(function (sender, e) {

            onStopEditor();
            var ds = m_DataStore;

            if (!ds.rows) {
                ds.rows = {};
            }


            var pkValue = getRowPkValue(e.item);

            var pkGuid = "PK_" + pkValue;

            if (ds.rows[pkGuid] == undefined) {
                //alert("删除记录错误,\"" + pkValue + "\" 主键值不存在");
                return;
            }

            var row = ds.rows[pkGuid];
            row.state = "deleted";

            if (defaults.dataStoreStatusId && defaults.dataStoreStatusId != '') {
                $(defaults.dataStoreStatusId).val(Mini.JsonHelper.toJson(ds));
            }

        });

        templateObj.onItemsChanged();
    }

    this.itemAdded = function (fn) {

        m_TemplateObj.itemAdded(fn);
    }


    this.getFocusedRowPk = function () {
        /// <summary>获取焦点行的主键值</summary>

        return m_FocusPk;
    }

    this.getFocusedItemValue = function () {
        /// <summary>获取焦点单元格的值</summary>

        return $(defaults.focusConId).val();
    }

    this.getFocusedItem = function () {
        /// <summary>获取焦点单元格对象</summary>

        return m_FocusedItem;
    }



    function setCellFocusMousedownEvent(tr) {

        var tdList = $(tr).children("td[checkMode!='true']");

        $(tdList).mousedown(function () {

            if ($(this).attr("checkMode") == "true") {
                return;
            }

            var ds = m_DataStore;
            var pTR = $(this).parent("tr");

            var checkTR = $(pTR).children("td[checkMode='true']:first");

            var colIndex = $(checkTR).index();

            var checkBox = $(checkTR).children(":checkbox");

            var trList = $(pTR).parent().children();

            $(trList).each(function () {

                var cb = $(this).children("td").eq(colIndex).children(":checkbox");

                cb.removeAttr('checked');
            });

            checkBox.attr("checked", "true");


            //设置固定焦点
            try {
                if (m_FocusCell) {
                    $(m_FocusCell).removeClass("Check");
                }
                if (m_FocusRow) {
                    $(m_FocusRow).removeClass("Check");
                }

                var fixedTD = $(pTR).children(".RowHeader");

                $(fixedTD).addClass("Check");

                $(pTR).addClass("Check");

                m_FocusCell = $(fixedTD);
                m_FocusRow = pTR;
                m_FocusRowIndex = $(pTR).index();

                //设置焦点行索引
                $("#" + defaults.id + "_FocusRowIndex").val(m_FocusRowIndex);

            }
            catch (ex) {
                alert("DataGridView.setCellFocusMousedownEvent();\n\n" + ex.Message);
            }


            if ($(pTR).data("itemData") != undefined) {

                var rowGuid = "R" + $(tr).data("TemplateItemGuid");    //Template.js 模板特有标记

                var itemData = $(tr).data("itemData");

                var keys = ds.dataKeys;
                var pkValue;

                if (keys == "$P.guid") {
                    pkValue = $(tr).data("TemplateItemGuid");
                }
                else {
                    pkValue = itemData[keys];
                }

                if (m_FocusPk != null && m_FocusPk == pkValue) {
                    return;
                }

                m_FocusPk = pkValue;

                m_FocusedItem = itemData;
                $(defaults.focusConId).val(pkValue);

                if (defaults.fixedFields != "") {
                    var fixedValue = itemData[defaults.fixedFields];

                    if (fixedValue != undefined) {
                        $("#" + defaults.id + "_FixedRow").val(fixedValue);
                    }
                    else {
                        $("#" + defaults.id + "_FixedRow").val("");
                    }
                }

                onFocusedChanged({
                    "tr": pTR,
                    "itemData": itemData,
                    "focusPk": pkValue
                });

            }


        });
    }

    this.getValueForStore = function (e) {
        var cell = $(e.Row).children("[ColumnID=" + e.TargetColumnID + "]");

        var dbField = $(cell).attr("DBField");

        var itemData = $(e.Row).data("itemData");

        return itemData[dbField];


    }

    this.setFixedValueForGuid = function (pkValue, dbField, value) {

        var ds = m_DataStore;

        var keys = ds.dataKeys;

        var pkGuid = "PK_" + pkValue;

        var row = null;

        if (keys == "$P.guid") {
            row = m_TemplateObj.getItemForGuid(pkValue);
        }
        else {
            alert("未实现数据库主键作为表格的主键值!");
            return;
        }



        var cell = $(row).children("[ColumnID=" + dbField + "]");
        $(cell).text(value);

        var itemData = $(row).data("itemData");


        itemData[dbField] = value;

        if (ds.rows[pkGuid] == undefined) {
            ds.rows[pkGuid] = { state: "modified", pk: pkValue, fs: {}, rowIndex: -1, cells: {} }
        }

        var item = ds.rows[pkGuid];

        if (item.state == "added" || item.state == "unchanged") {
            item.state = "modified";
        }

        if (ds.fixedFields != undefined && ds.fixedFields != "" && ds.fixedFields == dbField) {

            if (ds.rows[pkGuid].fixedFs == undefined) {
                ds.rows[pkGuid].fixedFs = {};
            }

            ds.rows[pkGuid].fixedFs[ds.fixedFields] = value;
        }

        if (m_FocusedItem == itemData) {

            if (defaults.fixedFields != "") {
                var fixedValue = itemData[defaults.fixedFields];

                if (fixedValue != undefined) {
                    $("#" + defaults.id + "_FixedRow").val(fixedValue);
                }
                else {
                    $("#" + defaults.id + "_FixedRow").val("");
                }
            }

        }


        if (defaults.dataStoreStatusId && defaults.dataStoreStatusId != '') {
            $(defaults.dataStoreStatusId).val(Mini.JsonHelper.toJson(ds));
        }

    };

    this.resetItem = function (rowGuid) {

        this.stopEditor();

        var row = m_TemplateObj.resetItem(rowGuid);


        setCellFocusMousedownEvent(row);

        var isLocked = isRowLocked($(row).data("itemData"));

        if (isLocked) {
            var rowHeader = $(row).children(".RowHeader");
            $(rowHeader).addClass("Locked");
        }


        if (m_FocusCell) {
            $(m_FocusCell).removeClass("Check");
        }

        if (m_FocusRow) {
            $(m_FocusRow).removeClass("Check");
        }

        m_FocusCell = null;
        m_FocusedItem = null;
        m_FocusPk = null;
        m_FocusRow = null;
        m_FocusRowIndex = -1;
    };

    this.setCellValueForGuid = function (pkValue, dbField, value) {

        var ds = m_DataStore;

        var keys = ds.dataKeys;

        var pkGuid = "PK_" + pkValue;

        var row = null;



        if (keys == "$P.guid") {
            row = m_TemplateObj.getItemForGuid(pkValue);
        }
        else {
            alert("未实现数据库主键作为表格的主键值!");
            return;
        }


        var cell = $(row).children("[ColumnID=" + dbField + "]");

        $(cell).text(value);

        var itemData = $(row).data("itemData");
        itemData[dbField] = value;

        //        var dbField = $(cell).attr("DBField");


        if (ds.rows[pkGuid] == undefined) {
            ds.rows[pkGuid] = { state: "modified", pk: pkValue, fs: {}, rowIndex: -1, cells: {} }
        }

        //修改已经改变的固定主键
        if (dbField == ds.fixedFields) {
            var tmpRow = ds.rows[pkGuid];

            if (tmpRow.fixedFs == undefined) {
                tmpRow.fixedFs = {};
            }

            tmpRow.fixedFs[dbField] = value;
        }



        var item = ds.rows[pkGuid];

        if (item.state == "added" || item.state == "unchanged") {
            item.state = "modified";
        }

        item.fs[dbField] = value;

        if (ds.fixedFields != undefined && ds.fixedFields != "" && ds.fixedFields == dbField) {

            if (ds.rows[pkGuid].fixedFs == undefined) {
                ds.rows[pkGuid].fixedFs = {};
            }

            ds.rows[pkGuid].fixedFs[ds.fixedFields] = value;
        }


        if (defaults.dataStoreStatusId && defaults.dataStoreStatusId != '') {
            $(defaults.dataStoreStatusId).val(Mini.JsonHelper.toJson(ds));
        }
    };


    this.setValueForFoucsRow = function (colId, dbField, value) {

        var row = m_FocusRow;
        var itemData = m_FocusedItem;

        var cell = $(row).children("[ColumnID=" + colId + "]");

        $(cell).text(value);


        if ($(cell).hasClass("Modified") == false) {
            $(cell).addClass("Modified");
        }

        var ds = m_DataStore;

        if (!ds.rows) {
            ds.rows = {};
        }

        var pkValue = getRowPkValue(row);

        var pkGuid = "PK_" + pkValue;

        if (ds.rows[pkGuid] == undefined) {
            ds.rows[pkGuid] = { state: "modified", pk: pkValue, fs: {}, rowIndex: -1, cells: {} }

            if (ds.fixedFields != undefined && ds.fixedFields != "") {
                ds.rows[pkGuid].fixedFs = {};
                ds.rows[pkGuid].fixedFs[ds.fixedFields] = itemData[ds.fixedFields];
            }
        }

        var item = ds.rows[pkGuid];

        item.fs[dbField] = value;

        var itemData = $(row).data("itemData");
        itemData[dbField] = value;



        if (defaults.dataStoreStatusId && defaults.dataStoreStatusId != '') {
            $(defaults.dataStoreStatusId).val(Mini.JsonHelper.toJson(ds));
        }
    }


    this.setValueForStore = function (e) {

        var cell = $(e.Row).children("[ColumnID=" + e.TargetColumnID + "]");

        $(cell).text(e.Value);

        var itemData = $(e.Row).data("itemData");

        var dbField = $(cell).attr("DBField");

        itemData[dbField] = e.Value;


        if ($(cell).hasClass("Modified") == false) {
            $(cell).addClass("Modified");
        }

        var ds = m_DataStore;

        if (!ds.rows) {
            ds.rows = {};
        }


        //        var keys = ds.dataKeys;
        //        var pkValue;

        //        if (keys == "$P.guid") {
        //            pkValue = $(e.Row).data("TemplateItemGuid");
        //        }
        //        else {
        //            pkValue = itemData[keys];
        //        }

        var pkValue = getRowPkValue(e.Row);

        var pkGuid = "PK_" + pkValue;

        if (ds.rows[pkGuid] == undefined) {
            ds.rows[pkGuid] = { state: "modified", pk: pkValue, fs: {}, rowIndex: -1, cells: {} }
        }

        var item = ds.rows[pkGuid];

        item.fs[dbField] = e.Value;


        if (defaults.dataStoreStatusId && defaults.dataStoreStatusId != '') {
            $(defaults.dataStoreStatusId).val(Mini.JsonHelper.toJson(ds));
        }
    }




    this.showCellError = function (cellGuid, msg) {

        try {
            var items = $("input[name='" + cellGuid + "']");

            $(items).addClass("error");
        }
        catch (ex) {
            alert("DataGridView.showCellError(...)\n\n" + ex.Message);
        }

    }

    this.onReset = function (fn) {
        m_ResetEvent.push(fn);
    }

    this.reset = function () {

        this.stopEditor();

        for (var i = 0; i < m_ResetEvent.length; i++) {
            var fn = m_ResetEvent[i];
            fn();
        }

        if (defaults.oddClass != '') {
            $("#" + defaults.id + " tbody tr:odd").addClass(defaults.oddClass);
        }

        var initEditorCell = window[defaults.id + "_InitEditorCell"];

        initEditorCell();

        if (m_FocusCell) {
            $(m_FocusCell).removeClass("Check");
        }

        if (m_FocusRow) {
            $(m_FocusRow).removeClass("Check");
        }

        m_FocusCell = null;
        m_FocusedItem = null;
        m_FocusPk = null;
        m_FocusRow = null;
        m_FocusRowIndex = -1;


        if (m_DataStore && m_DataStore.rows) {

            var rows = m_DataStore.rows;

            var deleteObjs = Array();

            for (var i in rows) {
                var row = rows[i];

                if (row.state == "deleted") {
                    deleteObjs.push(i);
                }
            }

            for (var i = 0; i < deleteObjs.length; i++) {
                var row = deleteObjs[i];
                delete rows[row];
            }

            //ds.rows[pkGuid] = { state: "modified", pk: pkValue, fs: {}, rowIndex: -1, cells: {} }
        }


    }


    this.newRow = function () {

        this.stopEditor();

        var data = {};

        var row = m_TemplateObj.addItem(data);

        if (row == undefined) {
            alert("新建行错误!");
            return;
        }

        var guid = $(row).data("TemplateItemGuid");

        var ds = m_DataStore;

        var keys = ds.dataKeys;
        var pkValue;

        if (keys == "$P.guid") {
            pkValue = guid;
        }

        var pkGuid = "PK_" + pkValue;

        if (ds.rows[pkGuid] == undefined) {
            ds.rows[pkGuid] = { state: "added", pk: pkValue, fs: {}, rowIndex: -1, cells: {}, fixedFs: {} };
        }

        if (defaults.dataStoreStatusId && defaults.dataStoreStatusId != '') {
            $(defaults.dataStoreStatusId).val(Mini.JsonHelper.toJson(ds));
        }


        var initEditorCell = window[defaults.id + "_InitEditorCell"];
        initEditorCell();


        m_TemplateObj.onItemsChanged();


        return data;
    }

    this.itemsRemoveAll = function () {
        m_TemplateObj.clear();
    }

    this.removeFocusRow = function () {
        this.stopEditor();

        var tbody = $("#" + defaults.id).children("tbody");

        var rowList = $(tbody).children("tr");
        var rowCount = rowList.length;

        if (rowCount == 0) {
            return;
        }


        var lastRowIndex = m_FocusRowIndex;

        var key = this.getFocusedRowPk();

        this.removeRowsAtGuids([key]);

        m_TemplateObj.onItemsChanged();

    }

    this.removeRowsAtGuids = function (guidList) {

        this.stopEditor();

        for (var i = 0; i < guidList.length; i++) {

            var guid = guidList[i];

            m_TemplateObj.remoteAtGuid(guid);

            var ds = m_DataStore;

            var keys = ds.dataKeys;
            var pkValue;

            if (keys == "$P.guid") {
                pkValue = guid;
            }
            else {
                pkValue = itemData[keys];
            }


            var pkGuid = "PK_" + pkValue;

            if (ds.rows[pkGuid] == undefined) {
                continue;
            }

            if (ds.rows[pkGuid].state == "added") {
                delete ds.rows[pkGuid];
            }
            else {
                var row = ds.rows[pkGuid];
                row.state = "deleted";
            }
        }

        if (defaults.dataStoreStatusId && defaults.dataStoreStatusId != '') {
            $(defaults.dataStoreStatusId).val(Mini.JsonHelper.toJson(ds));
        }
    }


    //最后排序的列对象
    var m_LastSortColumnObj = undefined;

    //最后排序的
    var m_LastSortAObj;

    this.sort = function (owner, sortExp, fieldIndex) {

        this.stopEditor();

        if (fieldIndex == undefined) { fieldIndex = -1; }

        var sortId = $.format("#{0}_Sort", defaults.id);
        var sortFIndexId = $.format("#{0}_SortFIndex", defaults.id);
        var sortFIndexSrcId = $.format("#{0}_SortFIndexSrc", defaults.id);
        var sortModeId = $.format("#{0}_SortMode", defaults.id);

        var sortObj = $(sortId);
        var sortFIndexObj = $(sortFIndexId);
        var sortModeObj = $(sortModeId);
        var sortFIndexSrcObj = $(sortFIndexSrcId);

        sortObj.val(sortExp);
        sortFIndexObj.val(fieldIndex);

        if (m_LastSortColumnObj != undefined) {
            $(m_LastSortColumnObj).html(m_LastSortAObj);
        }

        m_LastSortAObj = $(owner).html();
        m_LastSortColumnObj = owner;

        var colIndexSrc = parseInt(sortFIndexSrcObj.val());

        if (colIndexSrc != fieldIndex) {
            $(owner).append("▼");
        }
        else {
            var ad = sortModeObj.val();

            if (ad == "DESC") {
                $(owner).append("▼");
            }
            else {
                $(owner).append("▲");
            }
        }

        widget1.submit(owner, {
            subName: defaults.id,
            subEvent: 'Sort'
        });
    }


    function init(options) {
        defaults = $.extend(defaults, options);

        m_DataStore = {
            dataKeys: defaults.dataKeys,
            fixedFields: defaults.fixedFields,
            rows: {}
        };

        m_SelectedStore = {
            dataKeys: defaults.dataKeys,
            fixedFields: defaults.fixedFields,
            rows: {}
        };


        $(window).scroll(function () {
            if (m_CurEditorControl && m_CurEditorControl.hide) {
                m_CurEditorControl.hide();
                m_CurEditorControl = null;
            }
        });

        var parent = $("#" + defaults.id).parent(".DataGridPanel");

        $(parent).scroll(function () {
            if (m_CurEditorControl && m_CurEditorControl.hide) {
                m_CurEditorControl.hide();
                m_CurEditorControl = null;
            }
        });


    }


    init(options);
};