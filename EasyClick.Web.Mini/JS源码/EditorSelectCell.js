/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

Mini.ui.EditorSelectCell = function (options) {

    var defaults = {
        type: 'text',
        defaultValue: '',
        box: document.body,

        //数据仓库状态ID
        dataStoreStatusId: ''
    }

    var t_cell = {
        Row: undefined,
        Cell: undefined,
        RowGuid: '',
        ItemData: {},
        DBField: '',
        SrcValue: '',
        ColumnID: ''
    };



    var dataStore = {};
    var m_DataGridView;

    var m_Con;

    var m_LastOwner = null;

    var m_Items = new Array();

    var m_SelectedChanged = new Array();

    this.selectedChanged = function (fn) {
        m_SelectedChanged.push(fn);
    }


    this.setGridView = function (gridView) {

        m_DataGridView = gridView;

        dataStore = gridView.getDataStore();

    }

    this.getDataView = function () {
        return m_DataGridView;
    }

    this.setDataStore = function (store) {
        dataStore = store;
    }

    this.itemAdd = function (item) {
        m_Items.push(item);
    }

    this.setItems = function (items) {
        m_Items = items;
    }

    this.itemClear = function () {
        m_Items = new Array();
    }

    function init(options) {
        defaults = $.extend(defaults, options);

        createChilds(defaults.box);
    }

    var m_VisibleInt = 0;

    function toModelValue(value) {

        var srcValue = t_cell.ItemData[t_cell.DBField];

        if (srcValue == value) {
            return;
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

        if (pkValue == undefined) {
            return;
        }

        var pkGuid = "PK_" + pkValue;

        if (ds.rows[pkGuid] == undefined) {
            ds.rows[pkGuid] = { state: "modified", pk: pkValue, fs: {} };

            if (ds.fixedFields != undefined && ds.fixedFields != "") {
                ds.rows[pkGuid].fixedFs = {};
                ds.rows[pkGuid].fixedFs[ds.fixedFields] = t_cell.ItemData[ds.fixedFields];
            }
        }

        var item = ds.rows[pkGuid];


        item.fs[t_cell.DBField] = value;

        t_cell.ItemData[t_cell.DBField] = value;

        if ($(m_LastOwner).hasClass("Modified") == false) {
            $(m_LastOwner).addClass("Modified");
        }

        if (defaults.dataStoreStatusId && defaults.dataStoreStatusId != '') {
            $("#" + defaults.dataStoreStatusId).val(Mini.JsonHelper.toJson(ds));
        }
    }

    ///监听类
    function SelectedChangedListener(owner, options) {

        var m_Owner;

        function init(owner, options) {
            m_Owner = owner;
        }

        this.selectedChanged = function (fn, e) {
            fn(m_Owner, e);
        }

        init(owner, options);
    }

    var m_SelectedChangedListener = new SelectedChangedListener(this, {});

    function onSelectedChanged() {

        for (var i = 0; i < m_SelectedChanged.length; i++) {
            var fn = m_SelectedChanged[i];

            m_SelectedChangedListener.selectedChanged(fn, t_cell);
        }
    }

    function createChilds(targetObj) {
        m_Con = $("<select style='display:none;position: fixed; top: 100px; left: 100px;z-index: 999;'></select>");
        $(targetObj).append(m_Con);

        $(m_Con).change(function () {

            if (m_VisibleInt == 0) {
                return;
            }

            var value = this.options[this.selectedIndex].value;
            var text = this.options[this.selectedIndex].text;

            $(m_LastOwner).text(text);

            toModelValue(value);

            onSelectedChanged();
        });

        $(m_Con).blur(function () {

            m_VisibleInt--;

            if (m_VisibleInt == 0) {
                m_Con.hide();
            }
        });
    }

    function createDropItems() {

        m_Con.html("");

        var op = $("<option></option>");
        m_Con.append(op)

        for (var i = 0; i < m_Items.length; i++) {
            var item = m_Items[i];

            if (typeof (item) == "object") {
                var op = $("<option></option>");

                $(op).attr("value", item.value);
                $(op).text(item.text);

                m_Con.append(op);
            }
            else {
                var op = $("<option></option>");

                $(op).attr("value", item);
                $(op).text(item);

                m_Con.append(op);
            }
        }

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


        m_LastOwner = owner;

        createDropItems();

        var value = "";

        if (m_DataGridView && m_DataGridView.setCurEditorControl) {
            m_DataGridView.setCurEditorControl(m_Con);
        }

        if (itemData) {

            t_cell.RowGuid = "R" + $(tr).data("TemplateItemGuid");    //Template.js 模板特有标记

            var itemData = $(tr).data("itemData");
            var dbField = $(owner).attr("DBField");
            var colId = $(owner).attr("ColumnID");


            if (dbField == "" && colId != "") {
                dbField = colId;
            }

            t_cell.Row = tr;
            t_cell.Cell = owner;
            t_cell.ColumnID = colId;
            t_cell.DBField = dbField;
            t_cell.ItemData = itemData;

            value = itemData[dbField];


            if (isRowLocked(itemData)) {
                m_LastOwner = null;
                m_Con.hide();
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

        var h = $(owner).height();

        if (h < 20) {
            h = 20;
        }
        m_Con.css("height", h);

        setTimeout(function () {

            m_Con.select();
            m_Con.focus();

        }, 50);
    }

    init(options);
}