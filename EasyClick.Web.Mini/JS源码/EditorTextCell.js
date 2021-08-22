/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

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

    function toModelValue() {

        if (m_IsSetValue) {
            return;
        }
        m_IsSetValue = true;


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



    function createChilds(targetObj) {
        m_Con = $("<input type='text' style='display:none;position: fixed; top: 100px; left: 100px;z-index: 999;' />");

        $(targetObj).append(m_Con);

        $(m_Con).css("ime-mode", defaults.imeMode);

        $(m_Con).keydown(function () {
            if (event.keyCode == 114) {
                var now = new Date();

                $(m_Con).val(now.toLongTimeString());

                event.keyCode = 0;
                event.returnValue = false;
            }
        });

        $(m_Con).keyup(function () {
            if (event.keyCode == 13) {
                toModelValue();
                m_Con.hide();

                m_VisibleInt = 0;
            }

        });



        $(m_Con).blur(function () {

            m_VisibleInt--;

            if (m_VisibleInt <= 0) {

                m_Con.hide();

                if (m_LastOwner != null) {
                    toModelValue();
                }
            }

        });

        $(m_Con).keydown(function () {



        });
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

        toModelValue();

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
            toModelValue();
        }

        m_IsSetValue = false;

        m_LastOwner = owner;

        if (m_DataGridView && m_DataGridView.setCurEditorControl) {
            m_DataGridView.setCurEditorControl(m_Con);
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
                m_Con.hide();
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