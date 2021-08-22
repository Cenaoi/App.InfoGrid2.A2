
/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

Mini.ui.EditorTextButtonCell = function (options) {


    var defaults = {
        type: 'text',
        defaultValue: '',
        box: document.body,

        //数据仓库状态ID
        dataStoreStatusId: '',
        imeMode: 'auto'
    };

    var m_Con;

    var m_SelectBtn;
    var m_Input;

    var m_Guid = parseInt(Math.random() * 10000000 + 1);

    var m_ButtonClickEvent = new Array();

    var dataStore = {};
    var m_DataGridView;

    var m_VisibleInt = 0;

    var m_LastOwner = undefined;

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

    this.buttonClick = function (fn) {
        m_ButtonClickEvent.push(fn);
    }

    ///监听类
    function ButtonClickListener(owner, options) {

        var m_Owner;

        function init(owner, options) {
            m_Owner = owner;
        }

        this.click = function (fn, e) {
            fn(m_Owner, e);
        }

        init(owner, options);
    }

    var m_ButtonClickListener = new ButtonClickListener(this, {});

    function onButtonClick() {

        t_cell.val = function (value) {
            m_Input.val(value);
        }

        for (var i = 0; i < m_ButtonClickEvent.length; i++) {
            var fn = m_ButtonClickEvent[i];

            m_ButtonClickListener.click(fn, t_cell);
        }
    }


    this.getObject = function () {
        return m_Con;
    };

    function init(options) {
        defaults = $.extend(defaults, options);

        createControl(defaults.box);
    }

    this.setGridView = function (gridView) {
        m_DataGridView = gridView;

        dataStore = gridView.getDataStore();
    }

    this.getDataView = function () {
        return m_DataGridView;
    }

    this.val = function (value) {
        if (value != undefined) {
            m_Input.val(value);
        }
        else {
            return m_Input.val();
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


    function toModelValue() {

        if (m_IsSetValue) {
            return;
        }
        m_IsSetValue = true;


        var value = $(m_Input).val();


        var srcValue = t_cell.ItemData[t_cell.DBField];

        if (srcValue == value) {
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


        t_cell.ItemData[t_cell.DBField] = value;


        if (ds.rows[pkGuid] == undefined) {
            ds.rows[pkGuid] = { state: "modified", pk: pkValue, fs: {}, rowIndex: -1, cells: {} };


            if (ds.fixedFields != undefined && ds.fixedFields != "") {
                ds.rows[pkGuid].fixedFs = {};
                ds.rows[pkGuid].fixedFs[ds.fixedFields] = t_cell.ItemData[ds.fixedFields];
            }
        }

        var item = ds.rows[pkGuid];
        item.fs[t_cell.DBField] = value;

        if (defaults.dataStoreStatusId && defaults.dataStoreStatusId != '') {
            $("#" + defaults.dataStoreStatusId).val(Mini.JsonHelper.toJson(ds));
        }

    }




    function createControl(targetObj) {

        var html =
            '<div class="selectDiv" style="position: fixed;display:none; top: 100px; left: 100px;">' +
                '<table cellspacing="0" cellpadding="0" border="0">' +
                    '<tr>' +
                        '<td><input class="input" type="text" /></td>' +
                        '<td><button type="button" class="select" style=" width: 24px; height: 24px">…</button></td>' +
                    '</tr>' +
                '</table>' +
            '</div>';

        m_Con = $(html);

        $(targetObj).append(m_Con);

        m_Input = $(m_Con).find(".input:first");
        m_SelectBtn = $(m_Con).find(".select:first");

        $(m_Input).css("ime-mode", defaults.imeMode);

        m_Con.attr("guid", m_Guid);

        $(m_Input).keydown(function () {
            if (event.keyCode == 13) {
                toModelValue();
                m_Con.hide();

                m_VisibleInt = 0;
            }
        });



        $(m_Input).blur(function () {

            if (m_LastOwner != null) {
                toModelValue();
            }

            var focusObj;

            if (event.currentTarget) {
                focusObj = $(event.currentTarget);
            }
            else {
                focusObj = $(document.activeElement);
            }


            if (focusObj.attr("guid") == m_Guid) {
                return;
            }

            m_VisibleInt--;

            if (m_VisibleInt <= 0) {
                m_Con.hide();
            }

        });

        m_SelectBtn.attr("guid", m_Guid);
        m_SelectBtn.click(onButtonClick);

        m_Input.attr("guid", m_Guid);
    }

    this.width = function (w) {
        if (w != undefined) {

            $(m_Con).width(w);

            $(m_Input).css({
                "width": w - 24
            });
        }
        else {
            return $(m_Con).width();
        }
    }

    this.height = function (h) {
        if (h != undefined) {
            $(m_Con).css({ 'height': h });

            $(m_Input).css({ 'height': h });

            $(m_SelectBtn).css({ 'height': h });
        }
        else {
            return $(m_Con).height();
        }
    }

    this.hide = function () {

        m_Con.hide();
    }

    this.isSetValue = function (value) {
        m_IsSetValue = value;
    }

    this.show = function (owner) {

        if (m_DataGridView && typeof (m_DataGridView.readOnly) == "function") {
            if (m_DataGridView.readOnly()) {
                m_DataGridView.stopEditor();
                return;
            }
        }

        if ($(m_Con).css("display") == "none") {
            m_VisibleInt = 0;
            m_LastOwner = null;
        }


        m_VisibleInt++;


        if (m_LastOwner && m_LastOwner != owner) {

            m_IsSetValue = false;
            toModelValue();

        }

        m_IsSetValue = false;

        m_LastOwner = owner;

        var tr = $(owner).parent("tr");
        var value = "";


        if (m_DataGridView && m_DataGridView.setCurEditorControl) {
            m_DataGridView.setCurEditorControl(this);
        }

       

        if ($(tr).data("itemData")) {

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
                m_VisibleInt = 0;
                m_LastOwner = null;
                m_Con.hide();
                return;
            }

        }
        else {
            value = $(owner).text();
        }


        m_SrcValue = value;

        this.val(value);


        m_Con.show();
        m_Con.offset($(owner).offset());

        this.width($(owner).outerWidth());

        var h = $(owner).height();

        if (h < 12) {
            h = 12;
        }

        this.height(h);

        setTimeout(function () {
            m_Input.select();
            m_Input.focus();
        }, 10);
    }

    init(options);
};