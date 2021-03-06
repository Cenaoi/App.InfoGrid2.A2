/// MiniHtml.js 创建日期:2014-3-15 2:15:39
/// <reference path="jquery-1.4.1-vsdoc.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery-jtemplates_uncompressed.js" />
/// <reference path="jquery.form.js" />
/// <reference path="../SWFUpload/swfupload.js" />
/// <reference path="../SWFUpload/handlers.js" />
/// <reference path="jquery-ui-1.8.9.custom.min.js" />

$.format = function (source, params) {
    if (arguments.length == 1)
        return function () {
            var args = $.makeArray(arguments);
            args.unshift(source);
            return $.format.apply(this, args);
        };
    if (arguments.length > 2 && params.constructor != Array) {
        params = $.makeArray(arguments).slice(1);
    }
    if (params.constructor != Array) {
        params = [params];
    }
    $.each(params, function (i, n) {
        source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
    });
    return source;
};

String.prototype.startsWith = function (str) {
    if (str == null) return false;

    return (this.substr(0, str.length) == str);
}



String.prototype.endWith = function (str) {
    if (str == null) return false;
    return (this.substr(this.length - str.length) == str);
}



Date.prototype.format = function (formatter) {
    if (!formatter || formatter == "") {
        formatter = "yyyy-MM-dd";
    }
    var year = this.getYear().toString();
    var month = (this.getMonth() + 1).toString();
    var day = this.getDate().toString();
    var yearMarker = formatter.replace(/[^y|Y]/g, '');
    if (yearMarker.length == 2) {
        year = year.substring(2, 4);
    }
    var monthMarker = formatter.replace(/[^m|M]/g, '');
    if (monthMarker.length > 1) {
        if (month.length == 1) {
            month = "0" + month;
        }
    }
    var dayMarker = formatter.replace(/[^d]/g, '');
    if (dayMarker.length > 1) {
        if (day.length == 1) {
            day = "0" + day;
        }
    }
    return formatter.replace(yearMarker, year).replace(monthMarker, month).replace(dayMarker, day);
}

Date.prototype.toLongTimeString = function () {

    var str = this.getYear() + "-" +
                (this.getMonth() + 1) + "-" +
                this.getDate() + " " +
                this.getHours() + ":" +
                this.getMinutes() + ":" +
                this.getSeconds() + "." +
                this.getMilliseconds();

    return str;
}



Mini = { version: '3.1' };

Mini.namespace = function () {
    var a = arguments, o = null, i, j, d, rt;
    for (i = 0; i < a.length; ++i) {
        d = a[i].split(".");
        rt = d[0];
        eval('if (typeof ' + rt + ' == "undefined"){' + rt + ' = {};} o = ' + rt + ';');
        for (j = 1; j < d.length; ++j) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
}

Mini.ns = Mini.namespace;

Mini.ns("Mini", "Mini.ui", "Mini.util", "Mini.ui.desings");

Mini.JsonHelper = {};

Mini.JsonHelper.toJson = function (o) {
    if (o == undefined) {
        return "";
    }

    var r = [];
    if (typeof o == "string") return "\"" + o.replace(/([\"\\])/g, "\\$1").replace(/(\n)/g, "\\n").replace(/(\r)/g, "\\r").replace(/(\t)/g, "\\t") + "\"";
    if (typeof o == "object") {
        if (!o.sort) {
            for (var i in o)
                r.push("\"" + i + "\":" + Mini.JsonHelper.toJson(o[i]));
            if (!!document.all && !/^\n?function\s*toString\(\)\s*\{\n?\s*\[native code\]\n?\s*\}\n?\s*$/.test(o.toString)) {
                r.push("toString:" + o.toString.toString());
            }
            r = "{" + r.join() + "}"
        } else {
            for (var i = 0; i < o.length; i++)
                r.push(Mini.JsonHelper.toJson(o[i]))
            r = "[" + r.join() + "]";
        }
        return r;
    }
    return o.toString().replace(/\"\:/g, '":""');
};



Mini.globel = {};

Date.prototype.format = function (formatter) {
    if (!formatter || formatter == "") {
        formatter = "yyyy-MM-dd";
    }
    var year = this.getYear().toString();
    var month = (this.getMonth() + 1).toString();
    var day = this.getDate().toString();
    var yearMarker = formatter.replace(/[^y|Y]/g, '');
    if (yearMarker.length == 2) {
        year = year.substring(2, 4);
    }
    var monthMarker = formatter.replace(/[^m|M]/g, '');
    if (monthMarker.length > 1) {
        if (month.length == 1) {
            month = "0" + month;
        }
    }
    var dayMarker = formatter.replace(/[^d]/g, '');
    if (dayMarker.length > 1) {
        if (day.length == 1) {
            day = "0" + day;
        }
    }
    return formatter.replace(yearMarker, year).replace(monthMarker, month).replace(dayMarker, day);
}


Mini.util.DateHelper = function () {

    this.StartByToday = function () {
        var d = new Date();

        return d.format("yyyy-MM-dd");
    }

    this.StartByMonth = function () {
        var d = new Date()
        var dd = new Date(d.getYear(), d.getMonth(), 1);
        return dd.format("yyyy-MM-dd");
    };

    this.StartByYear = function () {
        var d = new Date()
        var dd = new Date(d.getYear(), 0, 1);
        return dd.format("yyyy-MM-dd");
    };



    this.Today = function () {
        var d = new Date();
        return d.format("yyyy-MM-dd");
    };
};

Mini.DateHelper = new Mini.util.DateHelper();



Mini.ui.AutoCompleteBox = function (ps) {

} /// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

Mini.ui.Template = function (options) {
    /// <summary>模板</summary>

    var defaults = {
        id: '',
        htmlTemplate: ''
    };

    //公共参数
    var commonParams = {
        id: '',
        itemGuid: ""
    };

    var m_ItemGuid = 0;

    //容器
    var m_Box;

    //存放记录唯一索引的
    var m_ItemGuidsBox;

    var m_Template;

    //当前显示的记录
    var m_ItemDatas = new Array();

    //"添加记录后"触发的事件
    var m_EventAdded = new Array();

    //"删除后"触发的事件
    var m_EventRemoved = new Array();

    //"删除前"触发的事件
    var m_EventRemoving = new Array();

    var m_EventItemsChanged = new Array();

    function init(options) {

        defaults = $.extend(defaults, options);
        commonParams.id = defaults.id;


        var htmlObj = $("#" + defaults.id);

        if (htmlObj.length == 0) {
            alert("Template.js \nfunction init(options) 初始化错误,不存在 html 对象'" + defaults.id + "'");
        }

        //显示 html 的容器
        m_Box = htmlObj.parent();

        //创建模板
        m_Template = $.createTemplate(htmlObj.html());

        htmlObj.remove();

        m_Box.html("");

        //查找一个 form ，并注入当前的记录 Id
        var forms = $(m_Box).closest("form");

        if (forms.length > 0) {
            var frm = forms[0];

            var txt = "<input type='hidden' name='" + defaults.id + "__ItemGudisBox__' value='' />";

            m_ItemGuidsBox = $(txt);

            $(frm).append(m_ItemGuidsBox);

        }
        else {
            var txt = "<input type='hidden' name='" + defaults.id + "__ItemGudisBox__' value='' />";

            m_ItemGuidsBox = $(txt);

            $("body").append(m_ItemGuidsBox);
        }
    }

    this.onItemsChanged = function () {

        for (var i = 0; i < m_EventItemsChanged.length; i++) {
            var fn = m_EventItemsChanged[i];
            fn(this);
        }
    }

    this.itemsChanged = function (fn) {

        m_EventItemsChanged.push(fn);
    }


    this.itemTemplate = function (data) {
        /// <summary>重新设置模板</summary>
        /// <param name="data" type="string">模板内容</param>

        m_Template = $.createTemplate(data);

        var tmpData = new Array();

        var items = $(m_Box).children();

        for (var i = 0; i < items.length; i++) {
            tmpData.push($(items[i]).data("itemData"));
        }

        this.clear();

        this.addItemRange(tmpData);
    }

    function onItemAdded(row, itemGuid) {
        for (var i = 0; i < m_EventAdded.length; i++) {
            var fn = m_EventAdded[i];

            fn(this, row, itemGuid);
        }
    }

    this.itemAdded = function (fn) {
        /// <summary>新增记录触发的事件</summary>
        /// <param name="fn" type="Function">添加记录后的方法</param>

        m_EventAdded.push(fn);

        var items = $(m_Box).children();

        try {
            for (var i = 0; i < items.length; i++) {

                var itemGuid = $(items[i]).data("TemplateItemGuid");

                fn(this, items[i], itemGuid);
            }
        }
        catch (ex) {
            alert("Error: Template.js \n\n Template.itemAdded(fn) \n\n" + ex.Message);
        }
    };

    this.itemRemoved = function (fn) {
        /// <summary>"删除记录后"的方法</summary>
        /// <param name="fn" type="Function">删除记录后的方法</param>

        m_EventRemoved.push(fn);

    }

    var onItemRemoved = function (e) {
        try {
            for (var i = 0; i < m_EventRemoved.length; i++) {

                var fn = m_EventRemoved[i];

                fn(this, e);
            }
        }
        catch (ex) {
            alert("Error: Template.js \n\n Template.itemRemoved(fn) \n\n" + ex.Message);
        }
    }

    this.itemRemoveing = function (fn) {
        /// <summary>"删除记录过程"的方法</summary>
        /// <param name="fn" type="Function">删除记录过程的方法</param>

        m_EventRemoving.push(fn);

    };

    var onItemRemoveing = function (e) {
        try {
            for (var i = 0; i < m_EventRemoving.length; i++) {

                var fn = m_EventRemoving[i];

                var resut = fn(this, e);

                if (resut == false) {
                    return false;
                }
            }
        }
        catch (ex) {
            alert("Error: Template.js \n\n Template.itemRemoveing(fn) \n\n" + ex.Message);
        }

        return true;
    }



    this.clear = function () {
        /// <summary>删除所有记录</summary>

        $(m_Box).html("");
        m_ItemGuidsBox.val("");
        m_ItemGuid = 0;
    }

    this.addItem = function (data) {
        /// <summary>添加记录</summary>
        /// <param name="data" type="object">记录的数据</param>
        /// <return type="int" ></return>

        m_ItemGuid++;

        try {
            //构造记录的 GUID
            commonParams.itemGuid = defaults.id + "_Items_" + m_ItemGuid + "_";
            commonParams.guid = m_ItemGuid;

            var txt = m_Template.get(data, commonParams);

            var row = $(txt);


            $(m_Box).append(row);

            $(row).attr("TemplateItem", "true");
            $(row).data("TemplateItemGuid", m_ItemGuid);

            $(row).data("itemData", data);

            //把新guid索引号放进列队里面
            var srcVal = m_ItemGuidsBox.val();
            srcVal += ("," + m_ItemGuid);

            m_ItemGuidsBox.val(srcVal);


            onItemAdded(row, m_ItemGuid);

            return row;
        }
        catch (ex) {

            alert("Error: Template.js \n\n Template.addItem(data) \n\n" + ex.Message);
        }

    }

    this.addItemRange = function (datas) {
        /// <summary>添加一堆记录</summary>
        /// <param name="data" type="object">记录的数据</param>
        /// <return type="int" ></return>

        try {
            for (var i = 0; i < datas.length; i++) {

                var data = datas[i];

                this.addItem(data);

            }
        }
        catch (ex) {
            alert("Error: Template.js \n\n Template.addItemRange(data) \n\n" + ex.Message);
        }
    };

    this.getItemForGuid = function (guid) {
        /// <summary>按 Guid 获取记录</summary>

        var items = $(m_Box).children();

        var item = null;

        for (var i = 0; i < items.length; i++) {
            var tmpItem = items[i];

            var itemGuid = $(tmpItem).data("TemplateItemGuid");

            if (itemGuid == guid) {
                item = tmpItem;

                break;
            }
        }

        return item;

    };


    this.resetItem = function (rowGuid) {

        var oldRow = this.getItemForGuid(rowGuid);

        var data = $(oldRow).data("itemData");


        var txt = m_Template.get(data, commonParams);

        var row = $(txt);


        $(oldRow).replaceWith(row);

        $(row).attr("TemplateItem", "true");
        $(row).data("TemplateItemGuid", rowGuid);

        $(row).data("itemData", data);

        return row;
    }

    this.remoteAtGuid = function (guid) {
        /// <summary>按 Guid 删除记录</summary>
        /// <param name="itemIndex" type="int">记录索引</param>

        var items = $(m_Box).children();

        var item = null;

        for (var i = 0; i < items.length; i++) {
            var tmpItem = items[i];

            var itemGuid = $(tmpItem).data("TemplateItemGuid");

            if (itemGuid == guid) {
                item = tmpItem;

                break;
            }
        }

        if (item != null) {
            this.removeItem(item);
        }
    };

    this.removeAt = function (itemIndex) {
        /// <summary>按索引号删除记录</summary>
        /// <param name="itemIndex" type="int">记录索引</param>


        var items = $(m_Box).children();

        var m = $(items).get(itemIndex);

        if (m == undefined || m == null) {

            return;
        };

        this.removeItem(m);

    };

    this.removeItem = function (owner) {
        /// <summary>记录自己删除自己</summary>
        /// <param name="owner" type="object">触发源对象</param>

        if (owner == undefined || owner == null) {

            return;
        };

        if (owner.currentTarget) {
            owner = owner.currentTarget;
        }


        var item = $(owner).closest("[TemplateItem]");

        var itemGuid = $(item).data("TemplateItemGuid");

        var itemData = $(item).data("itemData");

        itemData.state = "deleted";


        if (!itemGuid || !itemData) {
            alert("发生严重错误，请报告技术员：黄伟钦！\n\nTEL:13242300623\r\n\r\n错误代码: Mini-10005");

            return;
        }

        if (m_EventRemoving.length > 0) {
            var allowCancel = onItemRemoveing({ "item": item, "itemData": itemData, "itemGuid": itemGuid });

            if (allowCancel == false) {
                return;
            }
        }

        onItemRemoved({ "item": item, "itemData": itemData, "itemGuid": itemGuid });

        $(item).remove();

        //把新guid索引号删除
        var srcVal = m_ItemGuidsBox.val();
        srcVal = srcVal.replace("," + itemGuid, "");

        m_ItemGuidsBox.val(srcVal);

    };

    this.itemCount = function () {
        /// <summary>获取记录数量</summary>
        /// <returns type="int" />

        var items = $(m_Box).children();

        return items.length;
    }

    this.getItems = function () {
        var items = $(m_Box).children();

        return items;
    };

    this.get = function (itemIndex) {
        /// <summary>获取记录数量</summary>
        /// <param name="itemIndex" type="int">获取记录的索引</param>
        /// <returns type="html" />

        var item = $(m_Box).children().get(itemIndex);

        return item;
    };

    init(options);
}

Mini.ui.Pagination = function (options) {
    /// <summary>分页组件</summary>

    var defaults = {
        id: '',
        itemTotal: 500,
        urlFormat: "pageIndex={0}",
        rowCount: 10,
        buttonCount: 10,
        curPage: 1,
        pageCount: 0,
        command: '',
        buttonText: { first: "First", prev: "Prev", next: "Next", last: "Last" }
    };

    var m_Params = {
        id: '',
        itemTotal: 500,
        urlFormat: "pageIndex_{0}",
        rowCount: 10,
        buttonCount: 10,
        curPage: 0,
        pageCount: 0,
        command: '',
        buttonText: { first: "First", prev: "Prev", next: "Next", last: "Last" }
    };

    var m_MainBox;

    var m_RowCountEl;
    var m_Box;
    var m_PageInfoEl;

    var m_InputRowCountEl;

    var m_StartPageIndex = 0;
    var m_EndPageIndex = 0;

    var m_ClickEvent;

    this.click = function (c) {
        var me = this;
        var items = m_Box.children("a");

        $(items).click(function () {
            c.call(this, { owner: me });

            return false;
        });

        m_ClickEvent = c;

    };


    function init(options) {
        /// <summary>初始化</summary>
        var me = this;

        m_Params = $.extend(defaults, options);

        m_MainBox = $("#" + m_Params.id);

        m_Box = $('<span class="box"></span>');
        m_PageInfoEl = $('<span class="page-info"></span>');



        if (m_Params.allowSetupRowCount) {

            m_RowCountEl = $('<span class="row-count" style="float:left;">' +
                '<table border="0" cellpadding="0" cellspacing="0" width="100px">' +
                        '<tr>' +
                            '<td>每页</td>' +
                            '<td><input type="text" style="width:40px;text-align: center;" value="10" /></td>' +
                            '<td>条</td>' +
                        '</tr>' +
                    '</table>' +
                '</span>');

            m_MainBox.append(m_RowCountEl);

            m_InputRowCountEl = m_RowCountEl.find('input:first');

            m_InputRowCountEl.attr('command', m_Params.command);
            m_InputRowCountEl.attr('commandParam', '0');
            m_InputRowCountEl.attr('valid', 'false');

            m_InputRowCountEl.keydown(function (e) {
                if (e.which == 13) {
                    var srcValue = m_Params.rowCount;

                    try {
                        m_Params.rowCount = parseInt($(this).val());
                    }
                    catch (ex) {
                        m_Params.rowCount = srcValue;

                        $(this).val(srcValue)

                        return;
                    }

                    $('#' + m_Params.id + '_RowCount').val(m_Params.rowCount);

                    if (m_ClickEvent) {
                        m_ClickEvent.call(m_InputRowCountEl, { owner: me });
                    }

                    return false;
                }
            });
        }

        m_MainBox.append(m_Box);
        m_MainBox.append(m_PageInfoEl);

        if (m_Box.length == 0) {
            alert("Pagination.js \nfunction init(options) 初始化错误,不存在 html 对象'" + m_Params.id + "'");
        }
    }

    function getFirstUrl() {
        /// <summary>获取"首页"的超链接</summary>

        return $.format(m_Params.urlFormat, "0");
    }

    function getPrevUrl() {
        /// <summary>获取"上一页"的链接</summary>
        var p = m_Params.curPage - 1;

        if (p < 0) { p = 0; }

        return $.format(m_Params.urlFormat, p);
    }

    function getNextUrl() {
        /// <summary>获取"下一页"的链接</summary>

        var p = m_Params.curPage + 1;

        if (p >= m_Params.pageCount) {
            p = m_Params.pageCount - 1;
        }

        return $.format(m_Params.urlFormat, p);
    }

    function getLastUrl() {
        /// <summary>获取"尾页"的链接</summary>

        return $.format(m_Params.urlFormat, m_Params.pageCount - 1);
    }

    this.reset = function (options) {
        /// <summary>重新设置</summary>

        var me = this;

        for (var i in options) {
            if (m_Params[i] != undefined) {
                m_Params[i] = options[i];
            }
        }


        var inputBox = $("#" + m_Params.id + "_CurPIndex");
        $(inputBox).val(m_Params.curPage);

        var m = m_Params.itemTotal % m_Params.rowCount;

        m_Params.pageCount = (m_Params.itemTotal - m) / m_Params.rowCount;

        if (m > 0) {
            m_Params.pageCount++;
        }

        m_StartPageIndex = m_Params.curPage - 4;
        m_EndPageIndex = m_Params.curPage + 5;

        if (m_StartPageIndex < 0) {
            m_StartPageIndex = 0;

            //            m_EndPageIndex += 4 - m_Params.curPage;
        }

        if (m_EndPageIndex >= m_Params.pageCount) {
            m_EndPageIndex = m_Params.pageCount - 1;
        }

        //重新构造分页的所有按钮
        m_Box.html("");

        var btnFormat = "<a href='{0}' command='{3}' commandParam='{2}' valid='false'>{1}</a>";

        var btnTxt = m_Params.buttonText;

        if (m_Params.curPage > 0) {
            m_Box.append($.format(btnFormat, getFirstUrl(), btnTxt.first, 0, m_Params.command));
        }
        else {
            m_Box.append($.format("<span class='disabled'>{0}</span>", btnTxt.first));
        }

        if (m_Params.curPage > 0) {
            m_Box.append($.format(btnFormat, getPrevUrl(), btnTxt.prev, m_Params.curPage - 1, m_Params.command));
        }
        else {
            m_Box.append($.format("<span class='disabled'>{0}</span>", btnTxt.prev));
        }

        for (var i = m_StartPageIndex; i <= m_EndPageIndex; i++) {

            var url = $.format(m_Params.urlFormat, i);
            if (i == m_Params.curPage) {
                m_Box.append($.format("<span class='current'>{0}</span>", i + 1));
            }
            else {
                m_Box.append($.format(btnFormat, url, i + 1, i, m_Params.command));
            }

        }

        if (m_Params.curPage < m_Params.pageCount - 1) {
            m_Box.append($.format(btnFormat, getNextUrl(), btnTxt.next, m_Params.curPage + 1, m_Params.command));
            m_Box.append($.format(btnFormat, getLastUrl(), btnTxt.last, m_Params.pageCount - 1, m_Params.command));
        }
        else {
            m_Box.append($.format("<span class='disabled'>{0}</span>", btnTxt.next));
            m_Box.append($.format("<span class='disabled'>{0}</span>", btnTxt.last));
        }

        //给各个按钮加上事件
        if (m_ClickEvent) {
            var items = m_Box.children("a");
            $(items).click(function () {
                m_ClickEvent.call(this, { owner: me });
                return false;
            });
        }

        var startRowIndex = m_Params.curPage * m_Params.rowCount;
        var endRowIndex = startRowIndex + m_Params.rowCount;

        if (endRowIndex > m_Params.itemTotal) {
            endRowIndex = m_Params.itemTotal;
        }


        if (m_Params.allowSetupRowCount) {
            m_InputRowCountEl.attr('command', m_Params.command);
            m_InputRowCountEl.val(m_Params.rowCount);
        }

        m_PageInfoEl.html($.format("当前记录 {0}--{1} 条,共 {2} 条记录", startRowIndex + 1, endRowIndex, m_Params.itemTotal));

        //m_Box.append($.format("当前记录 {0}--{1} 条,共 {2} 条记录", startRowIndex + 1, endRowIndex, m_Params.itemTotal));
    }

    init(options);
}; /// <reference path="../jquery/jquery-1.7.1.js" />
/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../SWFUpload/swfupload.js" />


Mini.ui.SWFUpload = function (options) {
    /// <summary>模板</summary>

    var m_Swfu;

    var defaults = {
        ID: ''
        , FileProgressContainer: "FileProgressContainer"
        , ButtonID: 'spanButtonPlaceHolder'
        , TargetNameID: ''    //目标文件的存放路径
        , SrcNameID: ''      //原图片文本框的id
        , ASPSESSID: ''
        , Upload_Url: ''      //文件上传的接受路径
        , File_Post_Name: 'Filedata'
        , AppRelativeVirtualPath: ''
    };



    var m_Options = {
        ID: ''
        , FileProgressContainer: "FileProgressContainer"
        , ButtonID: 'spanButtonPlaceHolder'
        , TargetNameID: ''    //目标文件的存放路径
        , SrcNameID: ''      //原图片文本框的id
        , ASPSESSID: ''
        , Upload_Url: ''      //文件上传的接受路径
        , File_Post_Name: 'Filedata'
        , AppRelativeVirtualPath: ''
    };

    function request() {
        var url = location.href;
        var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");

        var newStr = "";

        for (i = 0; j = paraString[i]; i++) {
            newStr += (j + "$$$");
        }

        return newStr;
    }

    function init(options) {

        m_Options = $.extend(defaults, options);

        m_Options.File_Post_Name = options.ID;

        if (options.ID != undefined && options.FileProgressContainer == undefined) {
            m_Options.FileProgressContainer = options.ID + "_FileProgressContainer";
        }

        if (options.ID != undefined && options.ButtonID == undefined) {
            m_Options.ButtonID = options.ID + "_Button";
        }

        if (options.ID != undefined && options.TargetNameID == undefined) {
            m_Options.TargetNameID = options.ID; // +"_TargetName";
        }

        if (options.ID != undefined && options.SrcNameID == undefined) {
            m_Options.SrcNameID = options.ID + "_SrcName";
        }

        var widgetUriObj = {
            __SubEvent: "Uploader",
            __ReturnFormat: "script",
            __SubName: options.SubName,
            __Path: encodeURI(m_Options.AppRelativeVirtualPath),
            __CID: options.CID,
            __Query: request()
        };

        if (options.params) {
            widgetUriObj.__Query += options.params;
        }

        //        alert(widgetUriObj.__Query);

        var settings = {
            upload_url: (m_Options.Upload_Url == "" ? ("/Core/Mini/EcWidgetAction.aspx?" + $.param(widgetUriObj)) : m_Options.Upload_Url),

            // Flash Settings
            flash_url: "/Core/Scripts/SWFUpload/swfupload.swf", // Relative to this file
            flash9_url: "/Core/Scripts/SWFUpload/swfupload_FP9.swf", // Relative to this file

            file_post_name: m_Options.File_Post_Name,

            post_params: { "ASPSESSID": m_Options.ASPSESSID },
            file_size_limit: "1900000",
            file_types: "*",
            file_types_description: "All Files",
            file_upload_limit: 100,
            file_queue_limit: 0,

            custom_settings: {
                upload_target: m_Options.FileProgressContainer
            },


            // Button settings
            button_image_url: "/Core/Scripts/SWFUpload/images/XPButtonUploadText_61x22.png",
            button_width: "61",
            button_height: "22",
            button_placeholder_id: m_Options.ButtonID,
            button_text: '<span class="theFont">选择</span>',
            button_text_style: ".theFont { font-size: 12px; }",
            button_text_left_padding: '16',


            // The event handler functions are defined in handlers.js
            swfupload_preload_handler: preLoad,
            swfupload_load_failed_handler: loadFailed,
            file_queued_handler: function (file) {
                $("#" + m_Options.FileProgressContainer).show();
            },
            file_queue_error_handler: fileQueueError,
            file_dialog_complete_handler: fileDialogComplete,
            upload_progress_handler: uploadProgress,
            upload_error_handler: uploadError,
            upload_success_handler: UploadSuccessHeandler,
            upload_complete_handler: uploadComplete,

            upload_start_handler: OnUploadStart,

            debug: false

        };


        m_Swfu = new SWFUpload(settings);


    }

    this.addPostParam = function (name, value) {

        m_Swfu.addPostParam(name, value);

    }

    var m_Event_UploadStart = null;

    function OnUploadStart(fileObj) {

        if (m_Event_UploadStart == null) {
            return;
        }

        m_Event_UploadStart(fileObj);
    }


    this.uploadStart = function (fn) {

        m_Event_UploadStart = fn;
    }

    function UploadSuccessHeandler(file, serverData) {
        try {
            //            alert("serverData = " + serverData);
            eval(serverData);

        } catch (ex) {
            this.debug(ex);
        }
    }


    function uploadComplete(file) {
        try {
            /*  I want the next upload to continue automatically so I'll call startUpload here */
            if (this.getStats().files_queued > 0) {
                this.startUpload();
            } else {
                var progress = new FileProgress(file, this.customSettings.upload_target);
                progress.setComplete();
                progress.setStatus("文件上传成功..");
                progress.toggleCancel(false);

                setInterval(function () {
                    $("#" + m_Options.FileProgressContainer).hide(500);
                }, 8000);
            }
        } catch (ex) {
            this.debug(ex);
        }
    }


    init(options);
}

Mini.ui.Fileupdate = function (options) {
    /// <summary>文件上传组件</summary>

    var m_Swfu = null;

    var defaults = {
        file_types: "*.*",
        file_size_max: 100000, //文件的大小
        file_queue_limit: 1  //每次只能上传的文件数
    };

    function init(options) {


    }

    this.show = function () {
        /// <summary>显示上传窗体</summary>


        if (m_Swfu == null) {
            m_Swfu = new SWFUpload({

                upload_url: "/Core/SWFUpload/Upload.aspx",
                flash_url: "/Core/SWFUpload/swfupload.swf",

                flash_width: "100px",
                flash_height: "20px"

            });
        }

        m_Swfu.selectFiles();
    }

    init(options);
}

Mini.ui.TreeView = function (options) {
    /// <summary>树组件</summary>



}; /// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../jquery.form.js" />
/// <reference path="_Default.js" />
/// <reference path="../validate/jquery.validate-vsdoc.js" />


Mini.ui.Widget = function (ps) {

    this.ActionPath = '/Core/Mini/WidgetAction.aspx';

    var m_EventSuccess = new Array();

    var m_EventErrors = new Array();



    var m_Params = {
        __Path: '',
        __CID: '',
        __PID: '',
        __IsUser: 1,
        __Lang: '',
        __WidgetPs: '',
        __Action: '',
        __ActionPs: '',
        __ReturnFormat: '',
        __IsPost: '1',
        __SubName: '',
        __SubEvent: ''
    };

    this.item = function (itemName) {

        var fullName = m_Params.__CID + "_" + itemName;

        var obj = window[fullName];

        return obj;
    }

    this.success = function (fn) {
        m_EventSuccess.push(fn);
    }

    this.error = function (fn) {
        m_EventErrors.push(fn);
    }

    function onSuccess() {
        for (var i = 0; i < m_EventSuccess.length; i++) {
            var fn = m_EventSuccess[i];

            fn(this);
        }
    }

    function onError() {

        for (var i = 0; i < m_EventErrors.length; i++) {
            var fn = m_EventErrors[i];
            fn(this);
        }

    }


    this.getValues = function (filter) {
        /// <summary>
        /// 获取各个控件的值
        /// </summary>
        /// <param name="values" type="values">
        /// 配置一组键/值对。
        /// </param>
        /// <returns type="" />

        var cid = m_Params.__CID;
        var cLen = cid.length;

        var objs = null;

        if (filter) {
            objs = $(filter);
        }
        else {
            objs = $(":input");
        }


        var items = new Array();

        var values = {};

        for (var i = 0; i < objs.length; i++) {

            var obj = objs[i];
            var id = $(obj).attr("id");


            if (id.length <= (cLen + 1) || id.substring(0, cLen) != cid) {
                continue;
            }

            items.push(obj);

            var wId = id.substring(cLen + 1);

            if ($(obj).attr("type") == "checkbox") {

                var v = $(obj).attr("checked");

                values[wId] = (v == undefined) ? false : v;
            }
            else {
                values[wId] = $(obj).val();
            }

        }

        return values;

    };

    this.setValues = function (values) {
        /// <summary>
        /// 对各个控件赋值
        /// </summary>
        /// <param name="values" type="values">
        /// 配置一组键/值对。
        /// </param>
        /// <returns type="" />

        var cid = m_Params.__CID;

        for (var key in values) {

            var value = values[key];

            var wId = cid + "_" + key;     //构造 input 对象的 id

            var wObj = $("#" + wId);


            if (wObj.length == 0) {
                continue;
            }


            if (wObj.attr("type") == "checkbox") {

                wObj.attr("checked", value);
            }
            else {
                wObj.val(value);
            }

        }
    };

    this.getWidgetParams = function (owner) {

        if (owner == undefined || owner == null || owner == '') {
            m_Params.__Action = '';
            m_Params.__Rum__ = Math.ceil(Math.random() * 1000);
            return m_Params;
        }

        if (owner.length && owner[0].nodeName == "BUTTON") {
            m_Params.__Action = (owner.attr("command") ? owner.attr("command") : owner[0].name);
        }
        else {
            m_Params.__Action = owner.attr("command");
        }

        m_Params.__ActionPs = (owner.attr("commandParam") ? owner.attr("commandParam") : '');
        m_Params.__ReturnFormat = (owner.attr("returnFormat") ? owner.attr("returnFormat") : m_Params.__ReturnFormat);
        m_Params.__Rum__ = Math.ceil(Math.random() * 1000);

        var actPs = m_Params.__ActionPs;
        if (actPs.startsWith("(") && actPs.endWith(")")) {
            m_Params.__ActionPs = "{" + actPs.substr(1, actPs.length - 2) + "}";
        }

        return m_Params;

    };


    this.Invoke = function (owner) {
        owner = $(owner);


        var exPath = this.getWidgetParams(owner);

        var options = {
            url: this.ActionPath + "?" + $.param(exPath),
            success: function (responseText, statusText, xhr, $form) {

                if (exPath.__ReturnFormat == 'html') {

                    $(exPath.__PID).html(responseText);
                }
                else {
                    try {

                        eval(responseText);
                    }
                    catch (ex) {

                        alert("错误\n\n" + ex.Message);
                    }
                }
            }
        };

        $.get(options.url, null, options.success);
    }

    this.getUrl = function (owner, command) {

        var exPath = this.getWidgetParams(owner);

        if (exPath.__Action == '' || exPath.__Action == undefined) {
            exPath.__Action = command;
        }

        var url = this.ActionPath + "?" + $.param(exPath);

        return url;
    }

    this.event = function (owner, formMethod, exData) {

        owner = $(owner);

        var formObjs = owner.closest('form');

        var loadingBoxId = owner.attr("loadingBoxId");

        if (loadingBoxId == undefined && Mini.globel.loadingBoxId) {
            loadingBoxId = Mini.globel.loadingBoxId;
        }

        if (formMethod == "post" && formObjs.length == 0) {
            alert("没有提供 Form 表单，无法提交!");
            return;
        }

        var formObj = formObjs[0];

        ProSubmitBufter(formObj);

        var exPath = this.getWidgetParams(owner);

        var eventSuccess = undefined;

        var srcReturnFormat = exPath.__ReturnFormat;



        if (exData) {

            if (exData.commandParam != undefined) {
                exPath.__ActionPs = exData.commandParam;
            }
            else if (exData.actionPs != undefined) {
                exPath.__ActionPs = exData.actionPs;
            }

            if (exData.command != undefined) {
                exPath.__Action = exData.command;
            }
            else if (exData.action != undefined) {
                exPath.__Action = exData.action;
            }

            if (exData.__IsPost != undefined) {
                exPath.__IsPost = exData.__IsPost;
            }

            if (exData.returnFormat != undefined) {
                exPath.__ReturnFormat = exData.returnFormat;
            }

            if (exData.success != undefined) {
                eventSuccess = exData.success;
            }
        }

        if (exPath.__Action == '' || exPath.__Action == undefined) {

            alert("没有提供 Command 命令参数，无法提交!");
            return;
        }

        if (typeof exPath.__ActionPs == "object") {
            exPath.__ActionPs = Mini.JsonHelper.toJson(exPath.__ActionPs);
        }

        $(owner).enable(false);


        var options = {
            url: this.ActionPath + "?" + $.param(exPath),
            success: function (responseText, statusText, xhr, $form) {
                $(owner).enable(true);

                if (exPath.__ReturnFormat == 'html') {

                    try {
                        $(exPath.__PID).html(responseText);
                        onSuccess();
                    }
                    catch (ex) {
                        onError();
                        alert("浏览器脚本解析错误: \n\n" + ex.Message + "\n\n" + responseText);
                    }
                }
                else {
                    try {
                        eval(responseText);

                        onSuccess();
                    }
                    catch (ex) {

                        onError();
                        alert("浏览器脚本解析错误: \n\n" + ex.Message + "\n\n" + responseText);

                    }
                }

                if (loadingBoxId != undefined) {
                    $("#" + loadingBoxId).hide();
                }
            }

        };

        if (eventSuccess != undefined && eventSuccess) {
            options.success = eventSuccess;
        }

        var form_method = formMethod;

        if (formMethod == undefined && $(owner).attr("form_method")) {
            form_method = $(owner).attr("form_method");
        }


        if (form_method == "get") {
            $.get(options.url, null, options.success);
        }
        else if (form_method == "post") {
            $.post(options.url, null, options.success);
        }
        else {
            if (formObj.method == "post") {
                $(formObj).ajaxSubmit(options);
            }
            else {
                $.get(options.url, null, options.success);
            }
        }

        m_Params.__ReturnFormat = srcReturnFormat;

        $(owner).enable(true);

        return false;
    };

    function ProSubmitBufter(formObj) {
        var textareList = $(formObj).find(":input");

        for (var i = 0; i < textareList.length; i++) {
            var conObj = $(textareList).get(i);
            var code = $(conObj).attr("SubmitBufore");

            if (!code) { continue; }

            eval(code);
        }
    }

    var m_WaitVisibleNum = 0;


    function ProExData(exData, exPath) {
        if (exData == undefined) {
            return;
        }

        if (exData.action) {
            exPath.__Action = exData.action;
        }
        else if (exData.command) {
            exPath.__Action = exData.command;
        }

        if (exData.returnFormat) {
            exPath.__ReturnFormat = exData.returnFormat;
        }

        if (exData.actionPs) {
            exPath.__ActionPs = exData.actionPs;
        }
        else if (exData.commandParam) {
            exPath.__ActionPs = exData.commandParam;
        }

        if (exData.returnFormat != undefined) {
            exPath.__ReturnFormat = exData.returnFormat;
        }

        if (exData.subName) {
            exPath.__SubName = exData.subName;
        }

        if (exData.subEvent) {
            exPath.__SubEvent = exData.subEvent;
        }

    }


    this.submit = function (owner, exData) {
        /// <summary>提交表单</summary>
        /// <param name="owner" type="object">触发源对象</param>
        /// <returns type="jQuery" />

        if (owner != null) {
            owner = $(owner);
        }

        var formObjs;
        var loadingBoxId;


        if (owner != null && owner.length > 0 && "FORM" == owner.get(0).tagName) {
            formObjs = owner;
        }
        else if (owner == null) {
            formObjs = $("form");
        }
        else {
            formObjs = owner.closest('form');
        }

        if (owner != null) {
            loadingBoxId = owner.attr("loadingBoxId");

            if (loadingBoxId == undefined && Mini.globel.loadingBoxId) {
                loadingBoxId = Mini.globel.loadingBoxId;
            }
        }

        if (formObjs.length == 0) {

            alert("没有提供 Form 表单，无法提交!");
            return;
        }


        var formObj = $(formObjs[0]);

        ProSubmitBufter(formObj);

        try {

            //以下验证代码 $(formObj).valid()  在 IE6 会出错。
            if (owner != null && owner.attr('valid') != "false") {

                var isValid = formObj.valid();

                if (isValid == false) {
                    alert("您的资料未填写完整!\n\n无法提交!");

                    return false;
                }
            }
        }
        catch (ex) {

        }

        var exPath = this.getWidgetParams(owner);

        var eventSuccess;

        ProExData(exData, exPath);

        if (exData && exData.success != undefined) {
            eventSuccess = exData.success;
        }


        if ((exPath.__Action == '' || exPath.__Action == undefined) && (exPath.__SubName == '' || exPath.__SubName == undefined)) {

            alert("没有提供 Command 命令参数，无法提交!");
            return;
        }


        if (typeof exPath.__ActionPs == "object") {
            exPath.__ActionPs = Mini.JsonHelper.toJson(exPath.__ActionPs);
        }

        if (owner != null && owner.length > 0 && "FORM" != owner.get(0).tagName) {
            $(owner).enable(false);
        }

        var options = {
            url: this.ActionPath + "?" + $.param(exPath),
            success: function (responseText, statusText, xhr, $form) {

                if (owner != null) {
                    $(owner).enable(true);
                }

                if (loadingBoxId != "") {
                    m_WaitVisibleNum--;
                    $("#" + loadingBoxId).hide();
                }

                if (exPath.__ReturnFormat == 'html') {

                    try {
                        $(exPath.__PID).html(responseText);
                        onSuccess();
                    }
                    catch (ex) {
                        onError();
                        alert("浏览器脚本解析错误: \n\n" + ex.Message + "\n\n" + responseText);
                    }
                }
                else {
                    try {

                        eval(responseText);

                        onSuccess();
                    }
                    catch (ex) {

                        onError();
                        alert("浏览器脚本解析错误: \n\n" + ex.Message + "\n\n" + responseText);

                    }
                }

            },
            error: function (responseText, statusText, xhr, $form) {
                if (owner != null) {
                    $(owner).enable(true);
                }

                if (loadingBoxId != "") {
                    m_WaitVisibleNum--;
                    $("#" + loadingBoxId).hide();
                }

                alert("链接远程服务器超时或服务器关闭.\n\n请重新登录或检查网络畅通.");
            }

        };


        if (eventSuccess != undefined && eventSuccess) {
            options.success = eventSuccess;
        }

        if (loadingBoxId != undefined) {

            m_WaitVisibleNum++;

            setTimeout(function () {
                if (m_WaitVisibleNum > 0) {
                    $("#" + loadingBoxId).show();
                }
            }, 500);
        }

        try {
            $(formObj).ajaxSubmit(options);
        }
        catch (ex) {
            alert(ex.Message);
        }

        return false;
    };

    this.reset = function () {
        var formObjs = $(owner).closest('form');

        if (formObjs.length > 0) {
            formObjs[0].clearForm();
        }
    };

    this.init = function (ps) {

        for (var p in ps) {

            m_Params[p] = ps[p];
        }
    };

    this.init(ps);

    return this;

};
/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../ui-lightness/jquery-ui-1.8.16.custom.min.js" />

Mini.ui.Window = function (ps) {

    var defaults = {
        resizable: true,
        stack: true,
        width: 200,
        height: 150,
        modal: true,
        title: "Window",
        autoOpen: false
    };

    var m_Options = {
        resizable: true,
        stack: true,
        width: 200,
        height: 150,
        modal: true,
        title: "Window",
        autoOpen: false
    };



    var m_contentHtml;
    var m_owner;
    var m_contentObj;

    var m_Box;

    function getIeVersion() {
        try {
            var browser = navigator.appName
            var b_version = navigator.appVersion
            var version = b_version.split(";");
            var trim_Version = version[1].replace(/[ ]/g, "");

            if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE6.0") {
                return ("IE6");
            }
            else if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE7.0") {
                return ("IE7");
            }
            else if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE8.0") {
                return ("IE8");
            }
            else if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE9.0") {
                return ("IE9");
            }
        }
        catch (ex) {
            return "";
        }


        return "";
    }

    function resizeEvent(event, ui) {
        var sz = ui.size;


        $(m_contentObj).width(m_Box.width());
        $(m_contentObj).height(m_Box.height());

    }

    function openEvent(event, ui) {
        var sz = ui.size;

        $(m_contentObj).width(m_Box.width());
        $(m_contentObj).height(m_Box.height());

    }

    function GetParentWin() {

        //        var ieVersion = getIeVersion();

        //        if (ieVersion == "IE6" || ieVersion == "IE7") {
        //            return window;
        //        }


        var lastParent = window;

        var i = 0;
        for (i = 0; i < 9; i++) {
            if (lastParent.parent == lastParent || lastParent.parent == undefined) {
                break;
            }

            lastParent = lastParent.parent;
        }

        return lastParent;
    }

    var m_closedEvent = new Array();

    this.closed = function (fn) {
        m_closedEvent.push(fn);
    };

    function onClosed(e) {
        for (var i = 0; i < m_closedEvent.length; i++) {
            var fn = m_closedEvent[i];

            try {
                fn(this, e);
            }
            catch (ex) {
                alert("MiniHtml Error: Window.onClosed(...) \n\n" + ex.Message);
            }
        }
    }

    this.close = function (e) {

        if (e == undefined) {
            e = {};
        }

        var win = window; //GetParentWin();

        onClosed(e);

        win.curDialog.dialog("close");

        //释放内存,有错误.暂时注释掉


        //        win.curDialog.dialog("destroy");
        //        delete win.curDialog;
        //$(win.curDialog).remove();
    }

    this.show = function () {

        var win = window; // GetParentWin();


        m_contentObj = $(m_Options.contentHtml);

        m_Box = $("<div style='border:0px solid #000000;'></div>");
        m_Box.attr("title", m_Options.title);


        m_Options.resizeStop = resizeEvent;
        m_Options.create = openEvent;


        //m_Options.buttons = [{ text: "Ok", click: function () { $(this).dialog("close"); } }];

        m_owner = $(m_Box).dialog(m_Options);

        curDialog = m_owner;


        m_owner.dialog("open");

        m_Box.append(m_contentObj);
        m_contentObj.curDialog = m_owner;

        var obj = $(m_contentObj).get(0);

        //如果是 iframe 模式窗体，就加入 curDialog 字段
        if (obj.contentWindow) {
            obj.contentWindow.curDialog = m_owner;
        }

        $(m_contentObj).width(m_Box.width());
        $(m_contentObj).height(m_Box.height());


    }

    this.getContentObj = function () {
        return m_contentObj;
    }

    function init(options) {
        m_Options = $.extend(defaults, options);

        m_contentHtml = options.contentHtml;
    }

    init(ps);
} /* 
 * 文件版本号：2013-1-30 22:56
 *
 */

/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />


Mini.ui.Tooltip = function (ps) {


    function getIeVersion() {
        try {
            var browser = navigator.appName
            var b_version = navigator.appVersion
            var version = b_version.split(";");
            var trim_Version = version[1].replace(/[ ]/g, "");

            if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE6.0") {
                return ("IE6");
            }
            else if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE7.0") {
                return ("IE7");
            }
            else if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE8.0") {
                return ("IE8");
            }
            else if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE9.0") {
                return ("IE9");
            }
        }
        catch (ex) {

            return "";
        }


        return "";
    }

    function GetParentWin() {

        var ieVersion = getIeVersion();

        if (ieVersion == "IE9") {
            return window;
        }

        var lastParent = window;

        var i = 0;

        for (i = 0; i < 9; i++) {
            if (lastParent.parent == lastParent || lastParent.parent == undefined) {
                break;
            }

            lastParent = lastParent.parent;
        }

        return lastParent;
    }

    function sticky(win, msg) {
        try {
            win.$.sticky(msg);
        }
        catch (ex) {
            alert(msg);
        }
    }

    this.show = function (messsage) {

        var win = GetParentWin();

        if (win.Mini_Tooltop == undefined) {

            win.In.ready("mi.Tooltip", function () {
                sticky(win, messsage);
            });
        }
        else {
            sticky(win, messsage);
        }
    }

};

window.Mini_Tooltop = new Mini.ui.Tooltip(); /// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />


Mini.ui.DropDownText = function (options) {
    /// <summary>可编辑的选择下拉框</summary>

    var defaults = {
        id: '',
        width: 130,
        officeY: 0
    };

    var m_Params = {
        id: '',
        width: 130
    };

    var m_Box;
    var m_SelectDiv;

    var m_Select;
    var m_Input;

    var m_ItemPanel;

    var m_OfficeY = 0;



    function init(options) {
        /// <summary>初始化</summary>

        m_Params = $.extend(defaults, options);

        m_Box = $("#" + m_Params.id);

        m_Select = m_Box.find(".select:first");
        m_Input = m_Box.find(".input:first");

        if (m_Params.width) {
            var ww = parseInt(m_Params.width);

            $(m_Box).css("width", m_Params.width);
            $(m_Input).css("width", ww - 20);
        }

        m_ItemPanel = $(m_Select).next(".Mini_DropDownPanel");



        $(m_ItemPanel).children("div").click(function () {
            $(m_Input).val($(this).text());

            $.powerFloat.hide();
        });

        var toY = ((window.curDialog != undefined) ? -30 : 0);

        $(m_Select).powerFloat({
            width: m_Params.width,
            height: 20 * 8,
            eventType: "click",
            position: "3-2",
            offsets: {
                x: 0,
                y: m_Params.officeY
            },
            target: $(m_ItemPanel)
        });

    }

    this.width = function (w) {

        if (w == undefined) {
            return m_Params.width;
        }
        else {
            var ww = parseInt(w);

            m_Params.width = w;

            if (m_Params.width) {
                $(m_Box).css("width", m_Params.width);
                $(m_Input).css("width", ww - 20);
            }
        }
    }

    this.officeY = function (value) {
        if (value == undefined) {
            return m_OfficeY;
        }
        else {
            m_OfficeY = value;
        }
    }

    init(options);
}; /// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />
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
        readOnly: false
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
            alert(ex.Message);
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


    this.getValueForRow = function (row, dbField) {

        var itemData = $(row).data("itemData");
        var value = itemData[dbField];

        return value;
    }


    this.setValueForRow = function (row, dbField, value) {

        //var itemData = m_FocusedItem;

        var cell = $(row).children("[ColumnID=" + dbField + "]");


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


    this.setValueForFoucsRow = function (colId, dbField, value) {

        this.setValueForRow(m_FocusRow, dbField, value);

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
}; /// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

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

        $(m_Con).bind("input propertychange", function () {
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
} /// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

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

    function Con_keydown() {

        var keyCode = event.keyCode;

        if (keyCode == 9) {
            event.keyCode = 0;
            event.returnValue = false;

            try {
                if (event.shiftKey && keyCode == 9) {
                    SetKeyPrev();
                    return;
                }


                switch (keyCode) {
                    case 9: SetKeyNext(); break;
                }
            }
            catch (ex) {

            }

            return;
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

        $(m_Con).keydown(Con_keydown);

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

    this.hide = function () {

        m_Con.hide();
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
            m_DataGridView.setCurEditorControl(this);
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
} /// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

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
    var m_KeydownEvent = new Array();

    this.keydown = function (fn) {
        m_KeydownEvent.push(fn);
    }

    function onKeydown(event) {
        for (var i = 0; i < m_KeydownEvent.length; i++) {
            var fn = m_KeydownEvent[i];
            fn(event);
        }
    }



    var m_SelectedChanged = new Array();

    this.selectedChanged = function (fn) {
        m_SelectedChanged.push(fn);
    }

    function onSelectedChanged(e) {
        /// <summary>选择触发的事件</summary>

        for (var i = 0; i < m_SelectedChanged.length; i++) {
            var fn = m_SelectedChanged[i];
            fn(this, e);
        }
    }

    /// <summary>事件监听</summary>
    var m_TextChanged = new Array();

    this.textChanged = function (fn) {
        /// <summary>添加监听事件的方法</summary>

        m_TextChanged.push(fn);
    }

    function onTextChanged(e) {
        /// <summary>文本框发生变化触发的事件</summary>

        for (var i = 0; i < m_TextChanged.length; i++) {
            var fn = m_TextChanged[i];
            fn(this, e);
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



        m_Select = $(m_Con).find(".select:first");
        m_Input = $(m_Con).find(".input:first");

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


        $(m_Input).bind("input propertychange", function () {
            onTextChanged();
        });



        $(m_Select).change(function () {
            var newValue = $(this).find('option:selected');
            $(m_Input).val(newValue.text());

            setTimeout(function () {
                $(m_Input).select();
                $(m_Input).focus();
            }, 10);

            onTextChanged();
            onSelectedChanged();
        });

        $(m_Input).keydown(onKeydown);



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

        if ($(m_Con).is(":hidden")) { return; }

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
        m_Con = new Mini.ui.Select2Box();
        $(targetObj).append(m_Con.getObject());


        m_Con.css("ime-mode", defaults.imeMode);

        m_Con.textChanged(function () {

            var value = m_Con.val();

            $(m_LastOwner).text(value)

            toModelValue(value);


        });

        m_Con.keydown(Con_keydown);

        m_Con.blur(function () {


            m_Con.hide();

            Mini_Tooltop.show("xxx");
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

        if (keyCode == 13 || keyCode == 114 || keyCode == 9 || keyCode == 40 || keyCode == 38) {
            event.keyCode = 0;
            event.returnValue = false;

            try {
                if (event.shiftKey && keyCode == 9) {
                    SetKeyPrev();
                    return;
                }

                switch (keyCode) {
                    case 13: m_Con.hide(); ; break;
                    case 9: SetKeyNext(); break;
                    case 40: SetKeyDown(); break;
                    case 38: SetKeyTop(); break;

                }
            }
            catch (ex) {
                alert("");
            }

            return;
        }

    }


    this.hide = function () {

        m_Con.hide();
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


        m_VisibleInt++;

        m_LastOwner = owner;

        createDropItems();

        var value = "";

        if (m_DataGridView && m_DataGridView.setCurEditorControl) {
            m_DataGridView.setCurEditorControl(this);
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
} /// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

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
} /// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

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
    var m_Button;

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

        if (keyCode == 13 || keyCode == 114 || keyCode == 9 || keyCode == 40 || keyCode == 38) {
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
                        var nowStr = now.getFullYear() + "-" + now.getMonth() + "-" + now.getDay();

                        $(m_Input).val(nowStr);
                        toModelValue(nowStr);

                        break;
                    case 13:
                        toModelValue(m_Input.val());
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


    function createChilds(targetObj) {
        m_Con = $("<div style='position: fixed;display:none; top: 100px; left: 100px;background-color:transparent;'>" +
                '<table cellspacing="0" cellpadding="0" border="0">' +
                    '<tr>' +
                        '<td><input class="input" type="text" /></td>' +
                        '<td><button type="button" class="select" style=" width: 24px; height: 24px">…</button></td>' +
                    '</tr>' +
                '</table>' +
            "</div>");

        $(targetObj).append(m_Con);

        m_Input = $(m_Con).find(".input:first");

        m_Button = $(m_Con).find(".select:first");

        $(m_Button).click(function () {
            $(m_Input).datepicker("show");
        });

        $(m_Input).css("ime-mode", defaults.imeMode);

        m_Input.datepicker({
            showOn: "",
            dateFormat: 'yy-mm-dd',
            showButtonPanel: false,
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

        $(m_Input).keydown(Con_keydown);


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

        var w = $(owner).width();
        var h = $(owner).height();

        m_Con.css("width", w + 24);
        m_Con.css("height", h);

        m_Input.css("width", w);
        m_Input.css("height", h);

        m_Con.show();
        m_Con.offset($(owner).offset());


        setTimeout(function () {

            m_Input.select();
            m_Input.focus();

        }, 10);
    }

    init(options);
}
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

        //        if (m_IsSetValue) {
        //            return;
        //        }
        //        m_IsSetValue = true;


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

    //监听的键盘特殊按键
    var m_KeyListening = [13, 114, 9, 40, 38];

    function Con_keydown() {

        var keyCode = event.keyCode;


        if (keyCode == 13 || keyCode == 114 || keyCode == 9 || keyCode == 40 || keyCode == 38) {
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
                    case 13: m_Con.hide(); break;

                }
            }
            catch (ex) {

            }

            return;
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

        $(m_Input).bind("input propertychange", function () {
            toModelValue();
        });


        $(m_Input).css("ime-mode", defaults.imeMode);

        m_Con.attr("guid", m_Guid);

        $(m_Input).keydown(Con_keydown);



        $(m_Input).blur(function () {

            if (m_LastOwner != null) {
                //toModelValue();
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

        if (m_Con.is(":hidden")) { return; }

        toModelValue();
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
            //toModelValue();

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
}; /// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

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
} /// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

Mini.ui.DataGrid = function (options) {


    var defaults = {
        id: ''
    };

    //模板对象
    var m_TemplateObj;

    //分页对象
    var m_PaginatorObj;

    var m_ResetEvent = new Array();

    function init(options) {
        defaults = $.extend(defaults, options);



    }

    this.showCellError = function (cellGuid, msg) {

        try {
            var items = $("input[name='" + cellGuid + "']");

            $(items).addClass("error");
        }
        catch (ex) {
            alert("Mini.ui.DataGrid.showCellError(,,,);\n\n" + ex.Message);
        }

    }

    this.onReset = function (fn) {
        m_ResetEvent.push(fn);
    }

    this.reset = function () {

        for (var i = 0; i < m_ResetEvent.length; i++) {
            var fn = m_ResetEvent[i];

            fn();
        }

    }

    init(options);
}; /// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="../ui-lightness/jquery-ui-1.8.16.custom.min.js" />


Mini.ui.EcView = function (options) {

    var m_ShowEvent;
    var m_ShowDialogEvent;

    this.setShowEvent = function (event) {
        m_ShowEvent = event;
    };


    this.setShowDialogEvent = function (event) {
        m_ShowDialogEvent = event;
    }


    function GetParentWin() {

        var lastParent = window;

        var i = 0;

        for (i = 0; i < 9; i++) {
            if (lastParent.parent == lastParent || lastParent.parent == undefined) {
                break;
            }

            lastParent = lastParent.parent;
        }

        return lastParent;
    }


    ///转换 Url 地址，配合 EcView 使用
    function ChangeAppUri(srcUri) {

        if (srcUri.substr(0, 1) == '/') {
            return srcUri;
        }

        var path = window.location.pathname;
        var search = window.location.search;

        var n = path.lastIndexOf('/');

        var dir = path.substr(0, n + 1);


        while ((srcUri.length > 3 && srcUri.substr(0, 3) == "../")) {
            n = dir.lastIndexOf('/', dir.length - 2);

            dir = dir.substr(0, n + 1);

            srcUri = srcUri.substr(3);
        }

        var newPath = dir + srcUri;

        return newPath;
    }


    this.show = function (uri, title, width, height) {

        uri = ChangeAppUri(uri);

        var win = GetParentWin();


        if (win.AddTab) {

            win.AddTab(uri, title);

            return;
        }

        if (width == undefined) { width = 800; }
        if (height == undefined) { height = 600; }

        window.open(uri, title, "height=" + width + ",width=" + height + ",toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no");
    }



    this.showDialog = function (uri, title, width, height, modal) {

        if (width == undefined) { width = 800; }
        if (height == undefined) { height = 600; }

        var win = GetParentWin();

        if (win == undefined) {
            alert("Error code:593\nEcView.shoDialgo(...)");
            return;
        }

        var nowUri = ChangeAppUri(uri);

        var txt = "<iframe frameborder='0'  src='" + nowUri + "' ></iframe>";

        try {

            var frm = new win.Mini.ui.Window({
                title: title,
                contentHtml: txt,
                width: width,
                height: height,
                modal: modal
            });

            frm.ownerWindow = window;
            frm.show();

            win.DialogEcView = frm;

            return frm;
        }
        catch (ex) {
            alert("EcView.showDialog(...)\n\n" + ex.Message);
        }

    }


    this.close = function (e) {

        if (window.curDialog && window.curDialog.close) {
            window.curDialog.close();
            return;
        }

        var win = GetParentWin();

        //关闭模式窗体
        if (win.DialogEcView) {
            var frm = win.DialogEcView;

            if (e == undefined) {
                e = {};
            }

            try {
                frm.close(e);

                var ownerWindow = frm.ownerWindow;

                win.DialogEcView = {};
            }
            catch (ex) {

                alert("关闭窗体错误:" + ex.Message);
            }

            return;
        }


        if (win.RemoveTab) {
            var url = window.location.href;

            win.RemoveTab(url);
        }

    }

    this.create = function () {

        var frm = new Mini.ui.EcForm();

        return frm;

    };
};

var EcView = new Mini.ui.EcView();