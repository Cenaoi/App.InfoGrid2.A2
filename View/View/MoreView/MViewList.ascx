<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MViewList.ascx.cs" Inherits="App.EC52Demo.View.ViewSetup.MViewList" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" method="post">
<mi:Store runat="server" ID="store1" Model="IG2_VIEW" IdField="IG2_VIEW_ID" PageSize="20" DeleteRecycle="true">
    <DeleteQuery>
        <mi:ControlParam Name="IG2_VIEW_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
    </DeleteRecycleParams>
    <SelectQuery>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
    </SelectQuery>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1">
    <mi:Toolbar runat="server" ID="toolbar2">
        <mi:ToolBarTitle Text="视图关联表" />
        <mi:ToolBarButton Text="新建" OnClick="EcView.show('MViewStepNew1.aspx','新建关联视图')" />
        <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
        <mi:ToolBarHr />
        <mi:ToolBarButton Text="删除" OnClick="ser:store1.Delete()" BeforeAskText="确定删除记录?" />
    </mi:Toolbar>
    <mi:Table runat="server" ID="table1" StoreID="store1" ReadOnly="true" Dock="Full" Region="Center">
        <Columns>
            <mi:RowNumberer />
            <mi:RowCheckColumn />
            <mi:BoundField DataField="VIEW_NAME" HeaderText="真实视图名" />
            <mi:BoundField DataField="DISPLAY" HeaderText="显示视图名" Width="400" />
            <mi:DateTimeColumn DataField="ROW_DATE_CREATE" HeaderText="创建时间"  />
            <mi:ActionColumn>
                <mi:ActionItem Icon="/res/icon/application_view_columns.png" Text="预览" Handler="TablePreview"  />
            </mi:ActionColumn>
        </Columns>
    </mi:Table>
</mi:Viewport>
</form>
<script>


    function TablePreview(view, cell, recordIndex, cellIndex, e, record, row) {

        var id = record.getId();

        EcView.show("/App/InfoGrid2/View/MoreView/MoreViewPreview.aspx?id=" + id, "预览-关联视图表");

    }
</script>