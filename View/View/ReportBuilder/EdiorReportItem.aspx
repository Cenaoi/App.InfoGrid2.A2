<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EdiorReportItem.aspx.cs" Inherits="App.InfoGrid2.View.ReportBuilder.EdiorReportItem" %>
<%@ Register Assembly="EasyClick.Web.Mini" Namespace="EasyClick.Web.Mini" TagPrefix="mi" %>
<!DOCTYPE html>
<html>
<head>
    <title></title>

    <style type="text/css">
        * { font-size:12px; }
    </style>
   
    <script src="/Core/Scripts/jquery/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/_Default.js"></script>

    <link href="/Core/Scripts/jquery.ui/themes/ui-lightness/jquery-ui.css" rel="stylesheet" type="text/css" />

    
    <script src="/Core/Scripts/ui-lightness/jquery-ui-1.9.2.custom.js" type="text/javascript"></script>
    <link href="/Core/Scripts/ui-lightness/redmond/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <link href="/Core/Scripts/ui-lightness/jquery-ui-1.9.0.custom.min.css" rel="stylesheet" type="text/css" />
    <script src="/Core/Scripts/jtemplates/jquery-jtemplates.js"></script>
    <% if (true)
       {

           string mapPath = MapPath("~/Core/Scripts/Mini2/Mini2.join.ini");
           string[] jsLines = System.IO.File.ReadAllLines(mapPath);
           
           foreach (string item in jsLines)
           {
                if(string.IsNullOrEmpty(item) || item.Trim().StartsWith("--")){
                    continue;   
                }

                Response.Write(string.Format("<script src='{0}/Core/Scripts/Mini2/dev/{1}' ></script>", JsHome, item));
           }
                
            //Response.WriteFile("~/Core/Scripts/Mini2/Mini2.script.txt");
       } %>
    <script src="/App/InfoGrid2/ReportBuilder/CrossReport.js" type="text/javascript"></script>
    <script src="/App/InfoGrid2/ReportBuilder/Builder.js" type="text/javascript"></script>
    <style>
        .tbSubItem tr th
        {

            border: 1px solid #CCCCCC;
        }
        
        .tbSubItem tbody tr td
        {

            border: 1px solid #CCCCCC;
        }
    
    </style>
</head>
<body>
   
        <table style="width:600px;">
            <tr>
                <td>
                    字段名:
                </td>
                <td>
                    <input type="text"  class="dbFileld  cr-item" data-dbf="aaa" disabled="disabled"  style="width:120px;"/>
                </td>
                <td>
                    DBValue:
                </td>
                <td>
                    <input type="text"  class="dbValue cr-item" style="width:120px;" />
                </td>
            </tr>
            <tr>
                <td>
                    小计:
                </td>
                <td>
                    <input class="enabledTotal cr-item" type="checkbox" value="true"  />
                </td>
                <td>
                    格式化显示:
                </td>
                <td>
                    <input type="text"  class="format cr-item"  style="width:120px;"/>
                </td>
            </tr>
            <tr>
                <td>
                    标题:
                </td>
                <td>
                    <input type="text" class="title cr-item"  style="width:120px;"/>
                </td>
                <td>
                    函数:
                </td>
                <td>
                    <select  class="funName cr-item" style="width:120px;">
                        <option value="SUM">求和</option>
                        <option value="COUNT" selected="selected">计数</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    值模式:
                </td>
                <td>
                    <select  class="valueMode cr-item" style="width:120px;">
                        <option value="DBValue" selected="selected">数据值</option>
                        <option value="FixedValue">固定值</option>
                        <option value="Code">代码</option>
                    </select>
                </td>
                <td>
                    Style:
                </td>
                <td>
                    <input type="text" class="tbxStyle cr-item"  style="width:120px;"/>
                </td>
            </tr>
            <tr>
                <td>
                    宽度:
                </td>
                <td>
                    <input type="number"  class="tbxWidth"  style="width:120px;"/>
                </td>
                <td>
                    表名:
                </td>
                <td>
                    <input type="text"  class="tbxTableName" disabled="disabled"  style="width:120px;"/>
                </td>
            </tr>
            <tr>
                <td>
                    一个子节点:
                </td>
                <td>
                    <input type="checkbox"  class="tbxOneChild"   />
                </td>
                <td>
                     
                </td>
                <td>
                     
                </td>
            </tr>
        </table>



        <br />
        <input type="button" value="新增行" style="margin:5px;" onclick="CreateRow()" />
        <input type="button" value="删除行" style="margin:5px;" onclick="DeleteRow()" />
        <table  class="tbSubItem" style="border: 0px solid #CCCCCC;" width="500" cellspacing="0"
            cellpadding="0" border="0">
            <thead>
                <tr>
                    <th style="width:20px;">
                        <input type="checkbox" name="rowAll" onclick="CheckAll(this)"/>
                    </th>
                    <th >固定值</th>
                    <th >标签</th>
                    <th>数据类型</th>
                    <th>逻辑条件</th>
                </tr>
            </thead>
            <tbody id="tempTbody">
            <script type="text/html" id="tempTBodyTemplate">
               <tr >
                    <td class="column-check"><input type="checkbox" name="row" class="row-checkbox" /></td>
                    <td><input type="text" class="pro-value" value="{$T.value}" /></td>
                    <td><input type="text" class="pro-text" value="{$T.text}"  /></td>
                    <td><input type="text" class="pro-type" value="{$T.type}" /></td>
                    <td>
                        <select class="pro-operator" value="{$T.operators}" >
                            <option value="Equals" {#if $T.operators=='Equals' }selected{#/if}>等于</option>
                            <option value="Inequality" {#if $T.operators=='Inequality' }selected{#/if}>不等于</option>
                            <option value="GreaterThan" {#if $T.operators=='GreaterThan' }selected{#/if}>大于</option>
                            <option value="LessThan" {#if $T.operators=='LessThan' }selected{#/if}>小于</option>
                            <option value="GreaterThanOrEqual" {#if $T.operators=='GreaterThanOrEqual' }selected{#/if}>大于或等于</option>
                            <option value="LessThanOrEqual" {#if $T.operators=='LessThanOrEqual' }selected{#/if}>小于或等于</option>
                            <option value="Like" {#if $T.operators=='Like' }selected{#/if}>模糊查询</option>
                            <option value="LeftLike" {#if $T.operators=='LeftLike' }selected{#/if}>左模糊查询</option>
                            <option value="RightLike" {#if $T.operators=='RightLike' }selected{#/if}>右模糊查询</option>
                            <option value="In" {#if $T.operators=='In' }selected{#/if}>列表</option>
                        </select>
                    </td>
                </tr>
            </script>
            </tbody>
        </table>
        <br />
       <div style=" text-align:center;">

        <input type="button" value="保存" style="width:80px;height:30px;" onclick="SaveCell()" />

        <input type="button" value="取消" style="width:80px;height:30px;" onclick="winColse()" />


       </div>


</body>
</html>

<script>

    var win;
    var data;
    //创建模板
    var m_Template;

    $(document).ready(function () {


        //console.log($("#tempTBodyTemplate").html());

        m_Template = $.createTemplate($("#tempTBodyTemplate").html());


        win = window.parent;

        data = win.clinet_GetItemJson();

        InitData(data);


    });

    //全选或全不选
    function CheckAll(me) {
        var b = $(me).attr("checked");

        $("#tempTbody tr td.column-check .row-checkbox").attr("checked", !!b);
    }



    //删除行
    function DeleteRow(){
        var checkBoxList = $("#tempTbody").find("input[type='checkbox']");

        checkBoxList.each(function (i, v) {
            var b = $(v).attr("checked");
            if (b) {
                $(v).parent().parent().remove();
            }
        });
    }


    //新增行
    function CreateRow() {
        var html = m_Template.get({ operators: "Equals" });

        $("#tempTbody").append(html);
    }




    ///初始化数据
    function InitData(data) {

        $("#tempTbody").children().remove();


        if (!data) {

            $(".enabledTotal").attr("checked", true);
            $(".title").val("");
            $(".valueMode").val("");
            $(".tbxWidth").val("");

            $(".dbValue").val("");
            $(".format").val("");
            $(".funName").val("");
            $(".tbxStyle").val("");
            $('.tbxTableName').val("");

            return;
        }

        console.log(data);

        $(".dbFileld").val(data.field);

        if (data.total == "True" || data.total) {
            $(".enabledTotal").attr("checked", "checked");
        } else {
            $(".enabledTotal").attr("checked", null);
        }


        $(".tbxOneChild").attr("checked", data.one_child);//只有一个子项

        $(".title").val(data.title);
        $(".valueMode").val(data.value_mode);
        $(".tbxWidth").val(data.width);
        $(".tbxTableName").val(data.tableName);
        $(".dbValue").val(data.db_value);
        $(".format").val(data.format);
        $(".funName").val(data.fun_name);
        $(".tbxStyle").val(data.style);


        var fixedValues = data.fixed_values;

        $(fixedValues).each(function (i,v) {

            var html = m_Template.get(v);
            $("#tempTbody").append(html);

        });

    };

    ///这是保存事件
    function SaveCell() {


        data.field = $(".dbFileld").val();

        data.tableName = $('.tbxTableName').val();

        data.total = !!($(".enabledTotal").attr("checked"));
        
        data.one_child = !!($(".tbxOneChild").attr("checked"));
        


        data.title = $(".title").val();

        data.value_mode = $(".valueMode").val();

        data.width = $(".tbxWidth").val();


        data.fun_name = $(".funName").val();

        data.format = $(".format").val();

        data.db_value = $(".dbValue").val();

        data.style = $(".tbxStyle").val();



        //拿到ID为tbSubItem的表格下面的行集合
        var trList = $(".tbSubItem").children("tbody").children("tr");

        //清空数组
        data.fixed_values = [];

        //循环读取行数据
        trList.each(function () {

            var lineObj = {};

            lineObj.value = $(this).find(".pro-value").val();
            lineObj.text = $(this).find(".pro-text").val();
            lineObj.type = $(this).find(".pro-type").val();
            lineObj.operators = $(this).find(".pro-operator").val();

            data.fixed_values.push(lineObj);

            //console.log(lineObj.value);

        });

        win.client_SetItemJson(data);

        
        ownerWindow.close();


        
    };


    function winColse() {


        ownerWindow.close();



    }



    

       
    
 
</script>
<script runat="server">

    public string JsHome = string.Empty;
</script>