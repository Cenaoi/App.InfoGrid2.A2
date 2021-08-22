/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

Mini.ui.EditorCheckBoxCell = function (options) {

    var defaults = {
        type: 'text',
        defaultValue: '',
        box: document.body,

        //数据仓库状态ID
        dataStoreStatusId: ''
    };

    var dataStore = {};
    var m_DataGridView;

    var m_Con;

    var m_LastOwner = null;

    var m_VisibleInt = 0;

    var t_cell = {
        Row: undefined,
        Cell: undefined,
        RowGuid: '',
        ItemData: {},
        DBField: '',
        SrcValue: '',
        ColumnID: ''
    };


    var m_IsSetValue = false;

    function init(options) {
        defaults = $.extend(defaults, options);

    }

    this.setDataStore = function (store) {
        dataStore = store;
    }


    this.setGridView = function (gridView) {

        m_DataGridView = gridView;

        dataStore = gridView.getDataStore();

    }

    this.bind = function (obj) {

        $(obj).change(function () {

            var value = $(this).attr("checked");

            alert(value);

        });

    };

    this.click = function (obj) {

        var value = $(obj).attr("checked");

        toModelValue(obj, value);
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

        if (m_DataGridView && m_DataGridView.readOnly) {
            if (m_DataGridView.readOnly()) {
                return;
            }
        }

        return false;
    }

    function toModelValue(owner, value) {

        var td = $(owner).parent("td");

        var tr = $(td).parent("tr");
        var value = "";

        t_cell.Row = tr;

        if ($(tr).data("itemData")) {
            t_cell.RowGuid = "R" + $(tr).data("TemplateItemGuid");    //Template.js 模板特有标记

            var itemData = $(tr).data("itemData");
            var dbField = $(owner).attr("DBField");

            t_cell.DBField = dbField;
            t_cell.ItemData = itemData;

            value = ($(owner).attr("checked") == 'checked');

            if (isRowLocked(itemData)) {
                var srcValue = t_cell.ItemData[t_cell.DBField];

                if (srcValue) {
                    $(owner).attr("checked", "checked");
                }
                else {
                    $(owner).removeAttr("checked");
                }

                return;
            }
        }
        else {
            value = $(owner).text();
        }



        var srcValue = t_cell.ItemData[t_cell.DBField];


        var ds = dataStore;

        if (!ds.rows) {
            ds.rows = {};
        }

        var keys = ds.dataKeys;
        var pkValue;
        var pkGuid;

        if (keys == "$P.guid") {
            pkValue = $(t_cell.Row).data("TemplateItemGuid");
        }
        else {
            pkValue = t_cell.ItemData[keys];
        }

        pkGuid = "PK_" + pkValue;

        if (ds.rows[pkGuid] == undefined) {
            ds.rows[pkGuid] = { state: "modified", pk: pkValue, fs: {} }

            if (ds.fixedFields != undefined && ds.fixedFields != "") {
                ds.rows[pkGuid].fixedFs = {};
                ds.rows[pkGuid].fixedFs[ds.fixedFields] = t_cell.ItemData[ds.fixedFields];
            }
        }

        var item = ds.rows["PK_" + pkValue];

        if (t_cell.DBField != "$P.guid") {
            item.fs[t_cell.DBField] = value;
            t_cell.ItemData[t_cell.DBField] = value;
        }
        else {
            item.checked = value;
        }

        if (defaults.dataStoreStatusId && defaults.dataStoreStatusId != '') {
            $("#" + defaults.dataStoreStatusId).val(Mini.JsonHelper.toJson(ds));
        }

    }

    function createChilds(targetObj) {
        m_Con = $('<input type="checkbox" />');

        $(targetObj).append(m_Con);

        $(m_Con).keyup(function () {
            if (event.keyCode == 13) {
                toModelValue();
                m_Con.hide();
            }
        });


        $(m_Con).blur(function () {

            m_VisibleInt--;

            if (m_VisibleInt == 0) {
                toModelValue();
                m_Con.hide();
            }
        });
    }


    this.show = function (owner) {


        if (m_DataGridView && m_DataGridView.readOnly) {
            if (m_DataGridView.readOnly()) {
                m_DataGridView.stopEditor();
                return;
            }
        }

        m_VisibleInt++;

        if (m_LastOwner && m_LastOwner != owner) {
            toModelValue();
        }

        m_IsSetValue = false;

        m_LastOwner = owner;


        var tr = $(owner).parent("tr");
        var value = "";

        if (m_DataGridView && m_DataGridView.setCurEditorControl) {
            m_DataGridView.setCurEditorControl(m_Con);
        }

        if ($(tr).data("itemData")) {

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
        }
        else {
            value = $(owner).text();
        }



    }

    init(options);
}