<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StepNew2.ascx.cs" Inherits="App.InfoGrid2.View.OneView.StepNew2" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

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

<form action="" id="form1" method="post">


<mi:Store runat="server" ID="store1" Model="IG2_TABLE" IdField="IG2_TABLE_ID" >
    <FilterParams>
        <mi:TSqlWhereParam Where="TABLE_TYPE_ID in ('TABLE','MORE_VIEW')" />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:QueryStringParam Name="IG2_TABLE_ID" QueryStringField="owner_table_id" DbType="Int32" Logic="<&gt;" />
    </FilterParams>
</mi:Store>

<mi:Viewport runat="server" ID="viewport">
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="true" StoreID="store1" CheckedMode="Single"  ReadOnly="true"
            AutoRowCheck="True" Dock="Full" Region="Center" Layout="VBox">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="TABLE_NAME" HeaderText="数据表名" Width="200" />
                <mi:BoundField DataField="DISPLAY" HeaderText="工作表" Width="200" />
                <mi:BoundField DataField="REMARK" HeaderText="备注" />
                <mi:DateTimeColumn DataField="ROW_DATE_CREATE" HeaderText="创建时间" />
            </Columns>
        </mi:Table>
    </mi:Panel>
    <mi:WindowFooter runat="server" ID="footer1">
        <mi:Button runat="server" ID="OkBtn" Text="下一步" Width="80" Height="26" Command="GoNext" Dock="Center" />
        <mi:Button runat="server" ID="Button1" Text="取消" Width="80" Height="26" Dock="Right" OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>


</form>


