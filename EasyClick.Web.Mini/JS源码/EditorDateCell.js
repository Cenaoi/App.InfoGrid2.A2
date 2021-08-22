/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

Mini.ui.EditorDateCell = function (options) {

    var defaults = {
        type: 'text',
        defaultValue: '',
        box: document.body,

        //数据仓库状态ID
        dataStoreStatusId: '',
        imeMode: 'auto'
    };
    var m_Guid = parseInt(Math.random() * 10000000 + 1);
    var dataStore = {};
    var m_DataGridView;

    var m_Con;

    var m_LastOwner = null;

    var m_VisibleInt = 0;

    var m_Input;

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


        createChilds(defaults.box);
    }


    this.setGridView = function (gridView) {

        m_DataGridView = gridView;

        dataStore = gridView.getDataStore();

    }

    this.setDataStore = function (store) {
        dataStore = store;
    }


    function toModelValue(value) {

        if (m_IsSetValue) {
            return;
        }
        m_IsSetValue = true;



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

        if (defaults.dataStoreStatusId && defaults.dataStoreStatusId != '') {
            $("#" + defaults.dataStoreStatusId).val(Mini.JsonHelper.toJson(ds));
        }


    }

    function createChilds(targetObj) {
        m_Con = $("<div style='position: fixed;display:none; top: 100px; left: 100px;'>" +
            "<input class='input' type='text' />" +
            "</div>");

        $(targetObj).append(m_Con);

        m_Input = $(m_Con).children(".input:first");

        $(m_Input).css("ime-mode", defaults.imeMode);

        m_Input.datepicker({
            showOn: "button",
            dateFormat: 'yy-mm-dd',
            showButtonPanel: true,
            changeMonth: true,
            changeYear: true,
            buttonImageOnly: false,
            showOptions: { direction: '' },

            onSelect: function (dateText, inst) {
                inst.inline = false;
                toModelValue(dateText);

            }
        });

        $(m_Con).attr("guid", m_Guid)
        $(m_Con).children().attr("guid", m_Guid);

        m_Input.datepicker("option", $.datepicker.regional['zh-CN']);

        $(m_Input).keyup(function () {

            if (event.keyCode == 13) {

                m_VisibleInt--;

                if (m_LastOwner != null) {
                    toModelValue(m_Input.val());
                }

                m_Con.hide();
            }
        });


        $(m_Con).children().blur(function () {

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

            if (m_VisibleInt == 0) {
                m_Con.hide();
            }

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

    this.show = function (owner) {

        if (m_DataGridView && m_DataGridView.readOnly) {
            if (m_DataGridView.readOnly()) {
                m_DataGridView.stopEditor();
                return;
            }
        }

        m_VisibleInt++;



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
        m_Input.val(value);


        m_Con.show();
        m_Con.offset($(owner).offset());
        m_Con.css("width", $(owner).width() + 60);
        m_Con.css("height", $(owner).height());

        m_Input.css("width", $(owner).width());

        setTimeout(function () {

            //            m_Input.select();
            //            m_Input.focus();

        }, 10);
    }

    init(options);
}