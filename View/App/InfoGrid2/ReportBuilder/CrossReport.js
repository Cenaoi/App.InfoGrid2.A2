
/// <reference path="/Core/Scripts/jquery/jquery-1.8.3.min.js" />
/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="/Core/Scripts/Mini2/Mini2.min.js" />
/// <reference path="/Core/Scripts/MiniHtml.js" />


Mini2.define('Mini2.report.CrossReport', {
    //表名
    tableName: '',
    //表ID
    tableID: '',

    //编辑表格字段路径
    EditTableColUrl: '/App/InfoGrid2/View/ReportBuilder/EditTableCol.aspx',

    //显示交叉报表路径，点确定
    ShowReportUrl: '/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx',

    //返回页面路径
    backUrl: '/app/infogrid2/view/manager/treepagemanager.aspx',

    debug: true,
    ///这是描述
    description: "",

    ///这是字段集合
    dbFields: [],

    //存放编辑节点
    curEditCol: null,

    ///放整体数据
    init_json: '',

    ///大表的div
    fieldBoxEl: '',

    ///行标签对象
    rowBoxEl: '',

    ///列标签对象
    colBoxEl: '',

    saveUrl: "SaveData.aspx",

    ///值标签对象
    valueBoxEl: '',

    ///这是执行开始的函数
    init: function () {

        var me = this;

        me.newDiv();

        me.InitData(me.fieldBoxEl);

        me.Run(me.colBoxEl.find("ol"));

        me.Run(me.rowBoxEl.find("ol"));

        me.Run(me.valueBoxEl.find("ol"));


        

    },
    ///新建div
    newDiv: function () {
        var me = this;

        ///新建一个大的div
        var divEl = $("<div class='products' '></div>");

        var h1El = $("<h1 class='ui-widget-header'>设置交叉报表</h1>");



        ///这是数据库表div
        var divTable = Mini2.$joinStr([
            "<div style=' overflow-y:auto;overflow-y:auto;'>",
                   "<h1>",
                       "<span> 表名：" + me.tableName + me.description + "</span>",
                  "</h1>",
               "<div >",
                  "<div  class='setOverflow'>",
                      "<ul class='talbeNameUl'>",
                      "</ul>",
                  "</div>",
               "</div>",
            "</div>"
        ]);


        me.fieldBoxEl = divTable;

        me.dbFields = eval(me.dbFields);

        for (var i = 0; i < me.dbFields.length; i++) {

            var item = me.dbFields[i];


            var liEl = $.format("<li dbfield='{0}' data-tableName='{4}' dbdesc='{1}' data-viewFieldSrc='{5}'>{2}({3})</li>", item.dbField, item.description, item.dbField, item.description, item.talbeName, item.viewFieldSrc);


            divTable.find(".talbeNameUl").append(liEl); ;

        }

        //工具栏
        var toobarEl = $("<div style=' height:50px;'></div>");


        ///这是下面表格
        var tableEl = Mini2.$joinStr([
            "<div style='float:left; width:auto;'>",
                "<table border='0' cellpadding='4' cellspacing='0' >",
                      "<tr>",
                           "<td>",
                           "</td>",
                           "<td valign='top'>",
                               "<div  class='cross-cols divCol'>",
                                    "<h1 class='ui-widget-header'>列标签</h1>",
                                    "<div class='ui-widget-content'>",
                                        "<ol>",
                                        "</ol>",
                                     "</div>",
                               "</div>",
                           "</td>",
                      "<tr>",
                      "<tr>",
                            "<td valign='top'>",
                               "<div class='cross-cols divRow'>",
                                    "<h1 class='ui-widget-header'>行标签</h1>",
                                    "<div class='ui-widget-content'>",
                                        "<ol>",
                                        "</ol>",
                                     "</div>",
                               "</div>",
                           "</td>",
                           "<td valign='top'>",
                               "<div  class='cross-cols divValue'>",
                                    "<h1 class='ui-widget-header'>值标签</h1>",
                                    "<div class='ui-widget-content'>",
                                        "<ol>",
                                        "</ol>",
                                     "</div>",
                               "</div>",
                           "</td>",
                      "<tr>",
                "</table>",
            "</div>"
        ]);


        me.colBoxEl = tableEl.find(".divCol");

        me.rowBoxEl = tableEl.find(".divRow");

        me.valueBoxEl = tableEl.find(".divValue");

        var btnSaveEl = $("<button type='button' style='width:80px;height:30px;margin:10px;'>确定</button>");

        var btnCheckEl = $("<button type='button' style='width:80px;height:30px;margin:10px;'>验证</button>");

        var btnEditTableCo = $("<button type='button' style='width:80px;height:30px;margin:10px;'>编辑列属性</button>");

        var btnBackEl = $("<button type='button' style='width:80px;height:30px;margin:10px;'>取消</button>");

        


        btnSaveEl.click(function () {
            me.Save();
        });

        btnCheckEl.click(function () {
            me.ChenkValue 
        });

        //显示编辑表格字段属性界面
        btnEditTableCo.click(function () {

            var win = Mini2.create('Mini2.ui.Window', {
                mode: true,
                text: '列表框架',
                width: 800,
                height: 500,
                state: 'max',
                startPosition: 'center_screen',
                url: me.EditTableColUrl
            });
            win.show();
        });

        btnBackEl.click(function () {
            location.href = me.backUrl;
            
        });


        //默认显示字段表格 比 窗口高度 小 50 
        divTable.css("height", $(window).outerHeight()-50);

        //窗口大小改变事件
        $(window).resize(function () {
            divTable.css("height", $(window).outerHeight() - 50);
        });


        toobarEl.append(btnCheckEl).append(btnEditTableCo);


        tableEl.append(btnSaveEl).append(btnBackEl);


        divEl.append(h1El).append(divTable);
        


        $("body").children("form").append(divEl).append(toobarEl).append(tableEl);
        


    },
    ///返回所有数据
    toJson: function () {
        var me = this;

        var obj = {
            table_name: me.tableName,
            table_id :me.tableID,
            col_value: [],
            row_value: [],
            values: []

        };
        me.GetData(me.colBoxEl, obj.col_value);

        me.GetData(me.rowBoxEl, obj.row_value);

        me.GetData(me.valueBoxEl, obj.values);


        return obj;
    },

    ///拿到行、列、值标签下面的所有数据
    GetData: function (divID, obj) {
        var me = this;

        var cellList = $(divID).find("li");

        //循环li集合
        cellList.each(function () {

            var data = $(this).data("EX") || {};
            
            //console.log(data); ////////////////////////////////////////////////////////////////////

            obj.push(data);
        });

    },
    ///循环添加数据，传div的ID，和要添加进去的数据
    InputData: function (colsId, colsData) {
        var me = this;


        //拿到标签下面的ol节点
        var cellList = $(colsId).find("ol");

        for (var i = 0; i < colsData.length; i++) {
            var dataJson = colsData[i];

            var dragEl = Mini2.$joinStr([
                 '<li style="height:22px;" >',
                    '<a href="#" class="edit" style="width:50px;">编辑</a>',
                    '<span style="padding:0 6px;">',
                        dataJson.field , ' (' , dataJson.desc , ')',
                    '</span>',
                    '<a href="#" class="close" style="float:right;"><img src="/res/icon/close.png" border="0" /></a>',
                '</li>'
            ]);

            $(dragEl).data("EX", dataJson);


            $(cellList).append(dragEl);


            // 拿到编辑节点
            var aEl = $(dragEl).children(".edit");

            // 这是点击编辑事件
            $(aEl).click(function () {

                me.curEditCol = $(this).parent();

                me.EditorCol();

            });


            var imgEl = $(dragEl).children('.close');

            // 这是删除列事件
            $(imgEl).click(function () {

                me.DeleteCol(this);

            });


        }

    },
    ///删除节点事件
    DeleteCol: function (btnEl) {
        ///拿到父节点
        var me = this;

        var liEl = $(btnEl).parent();

        $(liEl).remove();

    },
    ///加载数据
    InitData: function (divID) {
        var me = this;

        $(divID).accordion();

        $(divID).find("li").draggable({
            appendTo: "body",
            helper: "clone"
        });

        var json = me.init_json;

        var obj = null;


        if (json == '') {

            return;
        }
        json = '(' + json + ')';

        try {

            obj = eval(json);
        }
        catch (ex) {
            alert(ex.Message);
            return;
        }



        ///添加列数据
        me.InputData(me.colBoxEl, obj.col_value);

        ///添加行标签
        me.InputData(me.rowBoxEl, obj.row_value);

        ///添加值标签
        me.InputData(me.valueBoxEl, obj.values);



    },


    ShowDrop: function (boxEl, ui) {

        var me = this;

        $(boxEl).find(".placeholder").remove();

        var dragItemEl = $(ui.draggable);

        //从节点中把字段名拿出来
        var field = dragItemEl.attr('dbfield');

        //从节点把描述拿出来
        var desc = dragItemEl.attr('dbdesc');

        //从节点中把表名拿出来
        var tableName = dragItemEl.attr('data-tableName');

        //视图原字段
        var viewFieldSrc = dragItemEl.attr('data-viewFieldSrc');



        var dragEl = Mini2.$joinStr([
            '<li style="height:22px;" >',
                '<a href="#" class="edit" style="width:50px;">编辑</a>',
                '<span style="padding:0 6px;">',
                    field , ' (' , desc , ')',
                '</span>',
                '<a href="#" class="close" style="float:right;"><img src="/res/icon/close.png" border="0" /></a>',
            '</li>'
        ]);

        $(boxEl).append(dragEl);

        ///初始化json数据
        var newData = {
            tableName: tableName,
            field: field,
            desc: desc,
            total: true,
            title: "",
            value_mode: "",
            width: "",
            fun_name: "SUM",
            format: "",
            db_value: "",
            style: "",
            one_child: false,
            viewFieldSrc: viewFieldSrc,
            fixed_values: []
        };

        dragEl.data("EX", newData);

        var aEl = dragEl.children('.edit');

        ///这是点击编辑按钮事件
        $(aEl).click(function () {

            me.curEditCol = $(this).parent();

            me.EditorCol();
        });


        var imgEl = dragEl.children('.close');

        ///这是删除列事件
        $(imgEl).click(function () {

            me.DeleteCol(this);
        });


    },

    Run: function (name) {
        var me = this;


        $(name).droppable({
            activeClass: "ui-state-default",
            hoverClass: "ui-state-hover",
            accept: ":not(.ui-sortable-helper)",
            drop: function (event, ui) {

                me.ShowDrop.call(me, this, ui);

            }
        }).sortable({
            items: "li:not(.placeholder)",
            sort: function () {
                // gets added unintentionally by droppable interacting with sortable
                // using connectWithSortable fixes this, but doesn't allow you to customize active/hoverClass options
                $(this).removeClass("ui-state-default");
            }
        });
    },

    ///这是保存整个数据
    Save: function () {
        var me = this;
        var objJson = me.toJson();


        var josnStr = Mini.JsonHelper.toJson(objJson);

        if (josnStr == "") {
            alert("没有数据，不能保存！！");
            return;
        }


        var url = me.saveUrl + "?id=" + $("#hfID").val();

        $("#hdJson").val(josnStr);

        try {
            ///把数据提交上去保存！
            $.post(url, { name: josnStr }, function (result, textStatus, jqXHR) {

                var res = '(' + result + ')';

                var obj = eval(res);

                if (obj.result != 'ok') { alert("保存失败！！"); return; }

                $("#hfID").val(obj.id);
                
                location.href = me.ShowReportUrl +"?id="+obj.id;

            });
        }
        catch (ex) {
            alert("error: " + ex.Message);
        }


    },
    ///验证数据
    ChenkValue: function () {
        var me = this;

        var $rows = me.rowBoxEl.find("li");


        if (!($rows.length > 0)) {
            alert("行标签不能没有值！！");
            return;
        }


        var $cols = me.colBoxEl.find("li");


        if (!($cols.length > 0)) {
            alert("列标签不能没有值！！");
            return;
        }



        var $values = me.valueBoxEl.find("li");


        if (!($values.length > 0)) {
            alert("值标签不能没有值！！");
            return;
        }


        alert("验证通过了！！！！");

    },
    ///点击编辑按钮事件
    EditorCol: function () {
        var me = this;

        

        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '列表框架',
            width: 800,
            height: 500,
            startPosition: 'center_screen',
            url: "EdiorReportItem.aspx"
        });

        win.show();


    }

});
