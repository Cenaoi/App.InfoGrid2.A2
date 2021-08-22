<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LogActList.ascx.cs" Inherits="App.InfoGrid2.View.LogAct.LogActList" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<form method="post">
<mi:Store runat="server" ID="store1" Model="LOG_ACT" IdField="LOG_ACT_ID" SortText="DATE_EXEC_FROM desc">
    <DeleteQuery>
        <mi:ControlParam Name="LOG_ACT_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">

    
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarButton Tooltip="刷新" Icon="/res/icon_sys/Refresh.png" OnClick="ser:store1.Refresh()"  />
            <mi:ToolBarButton Tooltip="查询" Icon="/res/icon_sys/Search.png"   />
            <mi:ToolBarHr />
            <mi:ToolBarButton Tooltip="删除" Icon="/res/icon_sys/Delete.png" OnClick="ser:store1.Delete()" BeforeAskText="确定删除选中的记录?" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true"  >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:NumColumn HeaderText="执行总时间" DataField="TIME_EXEC" />
                <mi:DateTimeColumn HeaderText="执行开始时间" DataField="DATE_EXEC_FROM" />
                <mi:DateTimeColumn HeaderText="执行结束时间" DataField="DATE_EXEC_TO" />
                <mi:BoundField HeaderText="SessionID" DataField="SESSION_ID" />
                <mi:BoundField HeaderText="操作名称" DataField="OP_TEXT" />
                <mi:BoundField HeaderText="页面Url" DataField="PAGE_URL" />
                <mi:BoundField HeaderText="备注" DataField="REMARK" />
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

        EcView.show("/App/InfoGrid2/View/LogAct/LogActMgr.aspx?id=" + tableId,"调试信息");

    }
</script>

