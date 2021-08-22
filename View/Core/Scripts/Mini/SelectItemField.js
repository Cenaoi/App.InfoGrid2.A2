/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

Mini.ui.SelectItemField = function (options) {

    var defaults = {
        type: 'text',
        defaultValue: '',
        box: document.body,

        //选择记录的状态ID
        selectedStatusId: '',

        //数据仓库状态ID
        dataStoreStatusId: '',

        colGuid: "CXX",
        ownerAllId: ""
    };

    var m_SelectedStore = {
        dataKeys: defaults.dataKeys,
        fixedFields: defaults.fixedFields,
        rows: {}
    };

    var dataStore = {
        dataKeys: defaults.dataKeys,
        fixedFields: defaults.fixedFields,
        rows: {}
    };

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

        m_SelectedStore = gridView.getSelectedStore();
        defaults.selectedStatusId = gridView.getSelectedStoreID();
    };

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

        var rowObj = $(owner).closest("tr");


        var ds = m_SelectedStore;

        if (!ds.rows) {
            ds.rows = {};
        }

        var itemData = $(rowObj).data("itemData");

        var keys = ds.dataKeys;
        var fixedFields = ds.fixedFields;

        var pkValue;

        var pkGuid = $(rowObj).data("TemplateItemGuid");

        if (fixedFields == "$P.guid") {
            pkValue = $(rowObj).data("TemplateItemGuid");
        }
        else {
            pkValue = itemData[fixedFields];
        }

        var propName = "S" + pkGuid;

        if (value == "checked") {
            if (ds.rows[propName] == undefined) {

                if (pkValue == undefined) { pkValue = "" };

                ds.rows[propName] = { guid: pkGuid, pk: pkValue };
            }

            //var row = ds.rows[propName];

        }
        else {
            if (ds.rows[propName] != undefined) {
                delete ds.rows[propName];
            }
        }

        $("#" + defaults.colGuid).removeAttr("checked");

        if (defaults.selectedStatusId && defaults.selectedStatusId != '') {
            $(defaults.selectedStatusId).val(Mini.JsonHelper.toJson(ds));
        }
    }

    function createChilds(targetObj) {


    }


    this.show = function (owner) {



    }


    this.clearAll = function () {

        $("#" + defaults.colGuid).removeAttr("checked");

        var checkObjs = $(":checkbox[" + defaults.colGuid + "]");

        var check = ($(defaults.ownerAllId).attr('checked') != undefined);

        $(checkObjs).removeAttr('checked');

        var ds = m_SelectedStore;

        if (!ds.rows) {
            ds.rows = {};
        }

        ds.rows = {};

        if (defaults.selectedStatusId && defaults.selectedStatusId != '') {
            $(defaults.selectedStatusId).val(Mini.JsonHelper.toJson(ds));
        }
    }

    this.CheckedAll = function (owner) {

        var checkObjs = $(":checkbox[" + defaults.colGuid + "]");

        var check = ($(owner).attr('checked') != undefined);

        if (!check) {
            $(checkObjs).removeAttr('checked');
        }
        else {
            $(checkObjs).attr('checked', 'checked');
        }

        var items = m_DataGridView.getItems();


        var ds = m_SelectedStore;

        if (!ds.rows) {
            ds.rows = {};
        }

        $(items).each(function () {


            var itemData = $(this).data("itemData");

            var keys = ds.dataKeys;
            var fixedFields = ds.fixedFields;

            var pkValue;

            var pkGuid = $(this).data("TemplateItemGuid");

            if (fixedFields == "$P.guid") {
                pkValue = $(this).data("TemplateItemGuid");
            }
            else {
                pkValue = itemData[fixedFields];
            }


            var propName = "S" + pkGuid;

            if (check) {
                if (ds.rows[propName] == undefined) {

                    if (pkValue == undefined) { pkValue = "" };

                    ds.rows[propName] = { guid: pkGuid, pk: pkValue };
                }
            }
            else {
                if (ds.rows[propName] != undefined) {
                    delete ds.rows[propName];
                }
            }


        });

        if (defaults.selectedStatusId && defaults.selectedStatusId != '') {
            $(defaults.selectedStatusId).val(Mini.JsonHelper.toJson(ds));
        }
    }


    init(options);
}