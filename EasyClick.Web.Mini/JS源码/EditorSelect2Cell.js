/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

Mini.ui.Select2Box = function (options) {


    var defaults = {

    };

    var m_Con;

    var m_Select = $(m_Con).children(".select");
    var m_Input = $(m_Con).children(".input");

    var m_Guid = parseInt(Math.random() * 10000000 + 1);

    this.getObject = function () {
        return m_Con;
    };

    var m_BlurEvent = new Array();

    ///**begin 事件
    var m_KeyupEvent = new Array();

    this.keyup = function (fn) {
        m_KeyupEvent.push(fn);
    }

    function onKeyup(event) {
        for (var i = 0; i < m_KeyupEvent.length; i++) {
            var fn = m_KeyupEvent[i];
            fn(event);
        }
    }



    var m_SelectedChanged = new Array();

    this.selectedChanged = function (fn) {
        m_SelectedChanged.push(fn);
    }

    function onSelectedChanged() {
        for (var i = 0; i < m_SelectedChanged.length; i++) {
            var fn = m_SelectedChanged[i];
        }
    }




    ///**end 事件

    function init(options) {
        defaults = $.extend(defaults, options);


        var html =
            '<div class="selectDiv" style="position: fixed;display:none; top: 100px; left: 100px;">' +
                '<select class="select" style="width: 200px; ">' +
                '</select>' +
                '<input class="input" type="text" />' +
            '</div>';

        m_Con = $(html);



        m_Select = $(m_Con).children(".select");
        m_Input = $(m_Con).children(".input");

        m_Con.attr("guid", m_Guid);
        m_Select.attr("guid", m_Guid);
        m_Input.attr("guid", m_Guid);


        var width = $(m_Select).width();

        $(m_Input).css({
            'position': 'absolute',
            'left': '2px',
            'top': '2px',
            'z-index': '2',
            'width': (width - 18),
            'vertical-align': 'middle',
            'border': 'none'
        });

        $(m_Select).css({
            'position': 'relative',
            'z-index': '1',
            'right': '0',
            'vertical-align': 'middle'
        });



        $(m_Select).change(function () {
            var newValue = $(this).find('option:selected');
            $(m_Input).val(newValue.text());

            setTimeout(function () {
                $(m_Input).select();
                $(m_Input).focus();
            }, 10);

            onSelectedChanged();
        });

        $(m_Input).keyup(function () {

            if (event.keyCode == 13) {
                onBlur();
            }

            onKeyup(event);
        });

        $(m_Input).blur(function () {
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

            onBlur();
        });

        $(m_Select).blur(function () {
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

            onBlur();

            onSelectedChanged();
        });
    }

    this.css = function (cssName, cssValue) {
        $(m_Con).css(cssName, cssValue);
    }

    this.blur = function (fn) {
        m_BlurEvent.push(fn);
    }

    function onBlur() {
        for (var i = 0; i < m_BlurEvent.length; i++) {
            var fn = m_BlurEvent[i];

            try {
                fn();
            }
            catch (e) {
                alert("EdirorSelect2Cell.onBlur(...)   " + e.Message);
            }
        }
    }

    this.width = function (w) {
        if (w) {
            $(m_Con).width(w);

            $(m_Select).css({
                'width': w
            });

            $(m_Input).css({
                'width': (w - 18)
            });
        }
        else {
            return $(m_Con).width();
        }
    }

    this.height = function (h) {
        if (h) {
            $(m_Con).height(h);

            $(m_Input).css({
                'height': h - 4
            });

            $(m_Select).css({
                'height': h
            });
        }
        else {
            return $(m_Con).height();
        }
    }

    this.select = function () {
        m_Input.select();
    }

    this.focus = function () {
        m_Input.focus();
    }

    this.show = function () {

        $(m_Con).show();
    }

    this.hide = function () {
        $(m_Con).hide();
    }

    this.offset = function (offset) {

        $(m_Con).offset(offset);
    };

    this.val = function (value) {
        if (value != undefined) {
            m_Input.val(value);
        }
        else {
            return m_Input.val();
        }
    };

    this.html = function (html) {
        m_Select.html(html);
    }

    this.append = function (obj) {
        m_Select.append(obj);
    }

    init(options);
}

Mini.ui.EditorSelect2Cell = function (options) {

    var defaults = {
        type: 'text',
        defaultValue: '',
        box: document.body,

        //数据仓库状态ID
        dataStoreStatusId: undefined,
        imeMode: 'auto'
    };


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


    this.setGridView = function (gridView) {

        m_DataGridView = gridView;

        dataStore = gridView.getDataStore();

    }


    this.getDataView = function () {
        return m_DataGridView;
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

    var m_BlurTime = new Date();
    var m_ShowTime = new Date();

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

    function createChilds(targetObj) {
        m_Con = new Mini.ui.Select2Box(); // $("<select style='display:none;position: fixed; top: 100px; left: 100px;z-index: 999;'></select>");
        $(targetObj).append(m_Con.getObject());

        m_Con.selectedChanged(function () {
            var value = m_Con.val();
            toModelValue(value);

            onSelectedChanged();
        });

        m_Con.css("ime-mode", defaults.imeMode);

        m_Con.blur(function () {

            m_VisibleInt--;

            if (m_VisibleInt == 0) {
                m_Con.hide();
            }

            var value = m_Con.val();

            $(m_LastOwner).text(value)

            if (m_LastOwner != null) {
                toModelValue(value);
            }

        });

        m_Con.keyup(function () {
            if (event.keyCode == 13) {

                var value = m_Con.val();
                toModelValue(value);

                m_VisibleInt = 0;
                m_Con.hide();

                event.keyCode = 0;
                event.returnValue = false;

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

        //console.log(owner + " = " + m_LastOwner);

        if (m_LastOwner != null && owner != m_LastOwner) {
            var tmpValue = m_Con.val();

            $(m_LastOwner).text(tmpValue)

            toModelValue(tmpValue);
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
        m_Con.val(value);


        m_Con.show();
        m_Con.offset($(owner).offset());
        m_Con.width($(owner).width());

        var h = $(owner).height();

        if (h < 20) {
            h = 20;
        }
        m_Con.height(h);

        setTimeout(function () {

            m_Con.select();
            m_Con.focus();

        }, 10);
    }

    init(options);
}