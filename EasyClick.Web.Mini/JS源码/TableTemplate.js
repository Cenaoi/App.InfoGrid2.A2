/**
 * 创建时间:2013-2-19
**/
/// <reference path="../jquery/jquery-1.4.1-vsdoc.js" />

Mini.ui.TableTemplate = function (options) {
    /// <summary>模板</summary>


    var defaults = {
        id: '',
        htmlTemplate: '',
        colCount: 4
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

    //最后一行的索引
    var m_LastRowIndex = -1;
    //最后列的索引
    var m_LastColIndex = -1;

    //列数量
    var m_ColCount = 4;


    this.onItemsChanged = function () {

        for (var i = 0; i < m_EventItemsChanged.length; i++) {
            var fn = m_EventItemsChanged[i];
            fn(this);
        }
    }

    this.itemsChanged = function (fn) {

        m_EventItemsChanged.push(fn);
    }


    function init(options) {

        defaults = $.extend(defaults, options);
        commonParams.id = defaults.id;
        m_ColCount = defaults.colCount;

        var htmlObj = $("#" + defaults.id + "_TemplateCode");

        if (htmlObj.length == 0) {
            alert("TableTemplate.js \nfunction init(options) 初始化错误,不存在 html 对象'" + defaults.id + "'");
            return;
        }

        //显示 html 的容器
        m_Box = $("#" + defaults.id); // htmlObj.parent();

        //创建模板
        m_Template = $.createTemplate(htmlObj.html());

        //htmlObj.remove();

        //m_Box.html("");

        //查找一个 form ，并注入当前的记录 Id
        var forms = $(m_Box).closest("form");

        var txt = "<input type='hidden' name='" + defaults.id + "__ItemGudisBox__' value='' />";
        m_ItemGuidsBox = $(txt);

        if (forms.length > 0) {
            var frm = forms[0];
            $(frm).append(m_ItemGuidsBox);
        }
        else {
            $("body").append(m_ItemGuidsBox);
        }
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

            var cellObj = $(txt);

            if (m_LastRowIndex == -1 || m_LastColIndex >= m_ColCount - 1) {

                var row = $("<tr></tr>");
                $(m_Box).children("tbody").append(row);

                for (var i = 0; i < m_ColCount; i++) {
                    var col = $("<td></td>");
                    row.append(col);
                }

                m_LastRowIndex++;
                m_LastColIndex = -1;
            }


            var curRow = $(m_Box).children("tbody").children("tr").eq(m_LastRowIndex);
            var curCol = $(curRow).children("td").eq(m_LastColIndex + 1);
            m_LastColIndex++;

            $(curCol).append(txt);

            $(curCol).attr("TemplateItem", "true");
            $(curCol).data("TemplateItemGuid", m_ItemGuid);

            $(curCol).data("itemData", data);

            //把新guid索引号放进列队里面
            var srcVal = m_ItemGuidsBox.val();
            srcVal += ("," + m_ItemGuid);

            m_ItemGuidsBox.val(srcVal);


            onItemAdded(curCol, m_ItemGuid);

            return curCol;
        }
        catch (ex) {

            alert("Error: Template.js \n\n Template.addItem(data) \n\n" + ex.Message);
        }

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

        var items = getItemBoxList();   // $(m_Box).children();

        try {
            for (var i = 0; i < items.length; i++) {

                var itemGuid = $(items[i]).data("TemplateItemGuid");

                fn(this, items[i], itemGuid);
            }
        }
        catch (ex) {
            alert("Error: TableTemplate.js \n\n Template.itemAdded(fn) \n\n" + ex.Message);
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
            alert("Error: TableTemplate.js \n\n Template.itemRemoved(fn) \n\n" + ex.Message);
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

        $(m_Box).children("tbody").html("");

        m_ItemGuidsBox.val("");
        m_ItemGuid = 0;
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

    function getItemBoxList() {
        var items = $(m_Box).children("tbody").children("tr").children("td[templateitem='true']");

        return items;
    }

    this.getItemForGuid = function (guid) {
        /// <summary>按 Guid 获取记录</summary>

        var items = getItemBoxList();   // $(m_Box).children("tbody").children("tr").children("td[templateitem='true']");

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


        var items = getItemBoxList();

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

        var items = getItemBoxList();

        return items.length;
    }

    this.getItems = function () {
        var items = getItemBoxList();

        return items;
    };

    this.get = function (itemIndex) {
        /// <summary>获取记录数量</summary>
        /// <param name="itemIndex" type="int">获取记录的索引</param>
        /// <returns type="html" />
        var items = getItemBoxList();
        var item = $(items).get(itemIndex);

        return item;
    };

    init(options);
};

