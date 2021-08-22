<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OneStepEdit1.ascx.cs" Inherits="App.InfoGrid2.View.InputExcel.OneStepEdit1" %>

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


<form action="" method="post">
<mi:Store runat="server" ID="store1" Model="IG2_IMPORT_RULE" IdField="IG2_IMPORT_RULE_ID" DeleteRecycle="true" SortText="ROW_DATE_CREATE DESC" >
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="IG2_IMPORT_RULE_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
    </DeleteRecycleParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">

    <mi:SearchFormLayout runat="server" ID="SearchFormLayout1" Visible="false">
    
    </mi:SearchFormLayout>
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarTitle ID="tableNameTB2" Text="导入规则" />
            <mi:ToolBarButton Tooltip="新建" Icon="/res/icon_sys/Insert.png" OnClick="ser:store1.Insert()"  />
            <mi:ToolBarButton Tooltip="刷新" Icon="/res/icon_sys/Refresh.png" OnClick="ser:store1.Refresh()"  />
            <mi:ToolBarButton Tooltip="查询" Icon="/res/icon_sys/Search.png"   />
            <mi:ToolBarHr />
            <mi:ToolBarButton Tooltip="删除" Icon="/res/icon_sys/Delete.png" OnClick="ser:store1.Delete()" BeforeAskText="确定删除选中的记录?" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full"  >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="RULE_NAME" HeaderText="规则名称" EditorMode="None" />
                <mi:BoundField DataField="TARGET_TABLE" HeaderText="主表名称" EditorMode="None" />
                <mi:BoundField DataField="TARGET_TABLE_TEXT" HeaderText="主表名称" EditorMode="None" />

                <mi:DateTimeColumn DataField="ROW_DATE_CREATE" HeaderText="创建时间" EditorMode="None" />

                <mi:ActionColumn AutoHide="true">
                    <mi:ActionItem Handler="RuleEdit" Tooltip="修改" Icon="/res/icon/page_white_edit.png" />
                </mi:ActionColumn>

            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>

</form>
<script>

    function RuleEdit(view, cell, recordIndex, cellIndex, e, record, row) {

        var tableId = record.getId();

        var typeId = record.get('IG2_IMPORT_RULE_ID');

        var text = record.get('RULE_NAME');

        EcView.show("StepNew3.aspx?id=" + tableId, text + "-导入规则");
        
    }
</script>