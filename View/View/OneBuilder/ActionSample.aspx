<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActionSample.aspx.cs" Inherits="App.InfoGrid2.View.OneBuilder.ActionSample" %>
<%@ Register Assembly="EasyClick.BizWeb" Namespace="EasyClick.BizWeb.UI" TagPrefix="biz" %>
<%@ Register Assembly="EasyClick.Web.Mini" Namespace="EasyClick.Web.Mini" TagPrefix="mi" %>
<% if (false)
   { %>
<link href="/App_Themes/Blue/common.css" rel="stylesheet" type="text/css" />
<link href="/App_Themes/Vista/table.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>
<link href="/Core/Scripts/ui-lightness/jquery-ui-1.8.6.custom.css" rel="stylesheet"
    type="text/css" />
<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
<script src="/Core/Scripts/validate/jquery.validate-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>
<script src="/Core/Scripts/Mini/_CodeHelp.js" type="text/javascript"></script>
<% } %>

<mi:ToolBar ID="ToolBar1" runat="server">
    <mi:ToolBarTitle Text="表格设置" />

    <mi:ToolBarButton Text="保存" Command="SaveRows" OnClick="widget1.submit(this)" Icon="/res/icon/save.png" />
    <mi:ToolBarButton Text="预览" Command="ToUrl" OnClick="widget1.submit(this)"  />


</mi:ToolBar>
<mi:DataGridView ID="DataGrid1" runat="server" class="table1" cellspacing="0"
    rules="all" DataKeys="IG2_DEF_TABLE_TOOL_CUS_ID" ScrollBars="Horizontal" AllowSorting="True"
    FixedFields="" FocusedItem="EasyClick.Web.Mini.DataSeletedItem" Height="" SortExpression=""
    Title="">
    <Columns>

    </Columns>
    <EmptyDataText>没有找到记录! 请您重新输入查询条件,进行查询!</EmptyDataText>
    <Pagination IsAjax="True" Command="GoPage2" ClassName="flickr" RowCount="20" UrlFormat="PageIndex={0}"
        ButtonCount="10" ID="Pagination1">
    </Pagination>
</mi:DataGridView>