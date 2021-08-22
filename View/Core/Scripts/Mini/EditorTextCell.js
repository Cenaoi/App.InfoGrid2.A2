﻿/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

Mini.ui.EditorTextCell = function (options) {

    var defaults = {
        type: 'text',
        defaultValue: '',
        box: document.body,

        //数据仓库状态ID
        dataStoreStatusId: '',
        imeMode: 'auto'
    };

    var dataStore = {};
    var m_DataGridView;

    var m_Con;

    var m_LastOwner = null;

    var m_VisibleInt = 0;

    var t_cell = {
        RowGuid: '',
        ItemData: {},
        DBField: '',
        SrcValue: '',
        Row: undefined
    };

    var m_IsSetValue = false;

    // region 事件

    var m_TextChangedEvent = new Array();

    this.textChanged = function (fn) {
        m_TextChangedEvent.push(fn);
    }

    function onTextChanged() {
        var value = m_Con.val();

        for (var i = 0; i < m_TextChangedEvent.length; i++) {
            var fn = m_TextChangedEvent[i];

            fn(this, {
                value: value,
                ItemData: t_cell.ItemData,
                DBField: t_cell.DBField,
                Row: t_cell.Row
            });
        }
    }

    // Endregion

    this.val = function (value) {
        if (value == undefined) {
            return m_Con.val();
        }
        else {
            m_Con.val(value);
        }
    }


    function init(options) {
        defaults = $.extend(defaults, options);


        createChilds(defaults.box);
    }

    this.setDataStore = function (store) {
        dataStore = store;
    }

    this.setGridView = function (gridView) {

        m_DataGridView = gridView;

        dataStore = gridView.getDataStore();

    }


    //作为补救的方法, 替换 toModelValue() 不足
    function toValue2() {

        var value = m_Con.val();


        var srcValue = t_cell.ItemData[t_cell.DBField];

        if ((srcValue == undefined && value == "") || srcValue == value) {
            return;
        }

        $(m_LastOwner).text(value);

        if ($(m_LastOwner).hasClass("Modified") == false) {
            $(m_LastOwner).addClass("Modified");
        }

        var ds = dataStore;

        if (!ds.rows) {
            ds.rows = {};
        }

        var keys = ds.dataKeys;
        var pkValue;

        if (keys == "$P.guid") {
            pkValue = $(t_cell.Row).data("TemplateItemGuid");
        }
        else {
            pkValue = t_cell.ItemData[keys];
        }

        var pkGuid = "PK_" + pkValue;

        if (ds.rows[pkGuid] == undefined) {
            ds.rows[pkGuid] = { state: "modified", pk: pkValue, fs: {}, rowIndex: -1, cells: {} }

            if (ds.fixedFields != undefined && ds.fixedFields != "") {
                ds.rows[pkGuid].fixedFs = {};
                ds.rows[pkGuid].fixedFs[ds.fixedFields] = t_cell.ItemData[ds.fixedFields];
            }
        }

        var item = ds.rows[pkGuid];

        item.fs[t_cell.DBField] = value;
        t_cell.ItemData[t_cell.DBField] = value;


        if (defaults.dataStoreStatusId && defaults.dataStoreStatusId != '') {
            $("#" + defaults.dataStoreStatusId).val(Mini.JsonHelper.toJson(ds));
        }

        onTextChanged();
    }

    function toModelValue() {

        var value = m_Con.val();


        var srcValue = t_cell.ItemData[t_cell.DBField];


        if ((srcValue == undefined && value == "") || srcValue == value) {
            return;
        }

        $(m_LastOwner).text(value);

        if ($(m_LastOwner).hasClass("Modified") == false) {
            $(m_LastOwner).addClass("Modified");
        }

        var ds = dataStore;

        if (!ds.rows) {
            ds.rows = {};
        }

        var keys = ds.dataKeys;
        var pkValue;

        if (keys == "$P.guid") {
            pkValue = $(t_cell.Row).data("TemplateItemGuid");
        }
        else {
            pkValue = t_cell.ItemData[keys];
        }

        var pkGuid = "PK_" + pkValue;

        if (ds.rows[pkGuid] == undefined) {
            ds.rows[pkGuid] = { state: "modified", pk: pkValue, fs: {}, rowIndex: -1, cells: {} }

            if (ds.fixedFields != undefined && ds.fixedFields != "") {
                ds.rows[pkGuid].fixedFs = {};
                ds.rows[pkGuid].fixedFs[ds.fixedFields] = t_cell.ItemData[ds.fixedFields];
            }
        }

        var item = ds.rows[pkGuid];

        item.fs[t_cell.DBField] = value;
        t_cell.ItemData[t_cell.DBField] = value;


        if (defaults.dataStoreStatusId && defaults.dataStoreStatusId != '') {
            $("#" + defaults.dataStoreStatusId).val(Mini.JsonHelper.toJson(ds));
        }

        onTextChanged();
    }

    function Con_keyup() {
        if (event.keyCode == 13) {

            if (m_Con.is(":visible")) {

                //toModelValue();

                m_Con.hide();
                m_VisibleInt = 0;

                event.keyCode = 0;
                event.returnValue = false;
            }

            return;
        }

    }

    //Shift + Tab
    function SetKeyPrev() {

        var index = $(m_LastOwner).index();
        var tr = $(m_LastOwner).parent();
        var body = $(tr).parent();

        var tdLen = $(tr).children("td").length;

        var nextIndex = tdLen - index;

        var nextTD = m_LastOwner;

        for (var i = index - 1; i >= 0; i--) {
            var nTD = $(nextTD).prev();

            nextTD = nTD;

            if ($(nTD).hasClass("editor") == false) {
                continue;
            }

            if ($(nTD).hasClass("readonly")) {
                continue;
            }

            $(nTD).mousedown();

            break;
        }
    }

    //获取 Tab 下一对象
    function SetKeyNext() {

        var index = $(m_LastOwner).index();
        var tr = $(m_LastOwner).parent();
        var body = $(tr).parent();

        var tdLen = $(tr).children("td").length;

        var nextIndex = tdLen - index;

        var nextTD = m_LastOwner;

        var isMoveNext = false;

        for (var i = index + 1; i < tdLen; i++) {
            var nTD = $(nextTD).next();

            nextTD = nTD;

            if ($(nTD).hasClass("editor") == false) {
                continue;
            }

            if ($(nTD).hasClass("readonly")) {
                continue;
            }

            isMoveNext = true;
            $(nTD).mousedown();

            break;
        }


        // 判断是否移动到下一单元格,如果没有移动到下一行的第一个单元格
        if (isMoveNext) {
            return;
        }

        var trIndex = $(tr).index();
        var trLen = $(body).children().size();

        var nextTR = tr.next();

        var isNewTR = false;

        //查找不锁上的行
        for (var i = trIndex + 1; i < trLen; i++) {

            var itemData = $(nextTR).data("itemData");

            if (isRowLocked(itemData) == false) {
                isNewTR = true;
                break;
            }

            nextTR = $(nextTR).next();
        }

        if (!isNewTR) {
            return;
        }

        nextTD = $(nextTR).children(":eq(0)");

        for (var i = 1; i < tdLen; i++) {
            var nTD = $(nextTD).next();

            nextTD = nTD;

            if ($(nTD).hasClass("editor") == false || $(nTD).hasClass("readonly")) {
                continue;
            }

            $(nTD).mousedown();

            break;
        }
    }


    function SetKeyDown() {

        var index = $(m_LastOwner).index();
        var tr = $(m_LastOwner).parent();
        var body = $(tr).parent();

        var trLen = body.children().length;
        var trIndex = tr.index();

        var nextTR = tr.next();

        //查找不锁上的行
        for (var i = trIndex + 1; i < trLen; i++) {

            var itemData = $(nextTR).data("itemData");

            if (isRowLocked(itemData) == false) {

                break;
            }

            nextTR = $(nextTR).next();
        }

        if ($(nextTR).size() == 0) {
            return;
        }


        var nTD = $(nextTR).children("td:eq(" + index + ")");

        if ($(nTD).length == 0) {
            return;
        }

        if ($(nTD).hasClass("editor") == false) {
            return;
        }

        if ($(nTD).hasClass("readonly")) {
            return;
        }

        nTD.mousedown();
    }



    function SetKeyTop() {

        var index = $(m_LastOwner).index();
        var tr = $(m_LastOwner).parent();
        var body = $(tr).parent();

        var trIndex = $(tr).index();

        var nextTR = tr.prev();

        //查找不锁上的行
        for (var i = trIndex - 1; i >= 0; i--) {

            var itemData = $(nextTR).data("itemData");

            if (isRowLocked(itemData) == false) {

                break;
            }

            nextTR = $(nextTR).prev();
        }

        if ($(nextTR).size() == 0) {
            return;
        }


        var nTD = $(nextTR).children("td:eq(" + index + ")");

        if ($(nTD).length == 0) {
            return;
        }

        if ($(nTD).hasClass("editor") == false) {
            return;
        }

        if ($(nTD).hasClass("readonly")) {
            return;
        }

        nTD.mousedown();
    }


    function Con_keydown() {

        var keyCode = event.keyCode;

        if (keyCode == 114 || keyCode == 9 || keyCode == 40 || keyCode == 38) {
            event.keyCode = 0;
            event.returnValue = false;

            try {
                if (event.shiftKey && keyCode == 9) {
                    SetKeyPrev();
                    return;
                }

                switch (keyCode) {
                    case 114:
                        var now = new Date();
                        $(m_Con).val(now.toLongTimeString());
                        break;
                    case 9: SetKeyNext(); break;
                    case 40: SetKeyDown(); break;
                    case 38: SetKeyTop(); break;

                }
            }
            catch (ex) {

            }

            return;
        }

    }

    function Con_Blur() {
        m_VisibleInt--;

        if (m_VisibleInt <= 0) {

            if (m_LastOwner != null) {

                m_Con.hide();
                m_VisibleInt = 0;
            }
        }
    }



    function createChilds(targetObj) {
        m_Con = $("<input type='text' style='display:none;position: fixed; top: 100px; left: 100px;z-index: 999;' />");

        $(targetObj).append(m_Con);

        $(m_Con).css("ime-mode", defaults.imeMode);

        $(m_Con).bind("input propertychange",function(){
            toModelValue();
        });

        $(m_Con).keydown(Con_keydown);
        $(m_Con).keyup(Con_keyup);
        $(m_Con).blur(Con_Blur);

    }

    function isRowLocked(itemData) {

        if (m_DataGridView) {
            var lockedKey = m_DataGridView.getLockedKey();

            if (lockedKey) {
                var v = itemData[lockedKey];

                if (typeof (v) == "string") {
                    v = v.toLowerCase();
                }
                if (v == "1" || v == 'Y' || v == 1 || v == "true" || v == true) {
                    return true;
                }
            }
        }


        return false;
    }



    this.hide = function () {

        if (m_Con.is(":hidden")) { return; }

        toValue2();
        m_Con.hide();

        m_VisibleInt = 0;
    }

    this.show = function (owner) {


        if (m_DataGridView && m_DataGridView.readOnly) {
            if (m_DataGridView.readOnly()) {
                m_DataGridView.stopEditor();
                return;
            }
        }


        var tr = $(owner).parent("tr");
        var itemData = $(tr).data("itemData");

        if (isRowLocked(itemData)) {
            return;
        }


        m_VisibleInt++;

        if (m_LastOwner != null && m_LastOwner != owner) {

            //alert("隐藏:" + m_Con.is(":hidden"));

            if (!m_Con.is(":hidden")) {

                //toModelValue();
                //toValue2();
            }
        }

        m_IsSetValue = false;

        m_LastOwner = owner;

        if (m_DataGridView && m_DataGridView.setCurEditorControl) {
            m_DataGridView.setCurEditorControl(this); //(m_Con);
        }


        var value = "";

        t_cell.Row = tr;


        if (itemData) {

            t_cell.RowGuid = "R" + $(tr).data("TemplateItemGuid");    //Template.js 模板特有标记

            var itemData = $(tr).data("itemData");
            var dbField = $(owner).attr("DBField");

            var colId = $(owner).attr("ColumnID");

            if (dbField == "" && colId != "") {
                dbField = colId;
            }


            t_cell.DBField = dbField;
            t_cell.ItemData = itemData;

            value = itemData[dbField];

            if (isRowLocked(itemData)) {
                m_LastOwner = null;
                //m_Con.hide();
                m_VisibleInt = 0
                return;
            }
        }
        else {
            value = $(owner).text();
        }

        m_SrcValue = value;
        m_Con.val(value);


        m_Con.show();
        m_Con.offset($(owner).offset());
        m_Con.css("width", $(owner).width());
        m_Con.css("height", $(owner).height());

        setTimeout(function () {
            m_Con.select();
            m_Con.focus();

        }, 10);
    }

    init(options);
}