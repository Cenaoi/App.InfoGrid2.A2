<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ValueTableMgr.ascx.cs" Inherits="App.InfoGrid2.View.OneValue.ValueTableMgr" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form action="" method="post" >
<mi:Store runat="server" ID="store1" Model="IG2_VALUE_TABLE" IdField="IG2_VALUE_TABLE_ID" PageSize="20">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="IG2_VALUE_TABLE_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
 
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >

        <mi:Toolbar ID="Toolbar1" runat="server">

            <mi:ToolBarTitle ID="tableNameTB1" Text="" />

            <mi:ToolBarButton Text="新增" OnClick="ser:store1.Insert()" />
            <mi:ToolBarButton Text="保存" OnClick="ser:store1.SaveAll()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="重新加载规则" Command="GoReload" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?" Command="DeleteAll" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField HeaderText="表名" DataField="TABLE_NAME" />
                <mi:BoundField HeaderText="表描述" DataField="TABLE_DISPAY" />
                <mi:BoundField HeaderText="备注" DataField="REMARKS" />
                <mi:CheckColumn HeaderText="激活" DataField="ENABLED" />
                <mi:ActionColumn AutoHide="true">
                    <mi:ActionItem Handler="TableEdit" Tooltip="修改" Icon="/res/icon/page_white_edit.png" />
                </mi:ActionColumn>
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>

</form>

<script>

    function TableEdit(view, cell, recordIndex, cellIndex, e, record, row) {

        var tableId = record.getId();

        var typeId = record.get('TABLE_TYPE_ID');
        //MiniHelper.Redirect("/App/EC52Demo/View/CopyData/CopyDataNew2.aspx?id=" + tableId);
        EcView.show("/App/InfoGrid2/View/OneValue/EditValue.aspx?id=" + tableId, "简单流程", 800, 1200);

    }

</script>