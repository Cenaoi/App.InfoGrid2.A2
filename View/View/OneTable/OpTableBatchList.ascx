<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OpTableBatchList.ascx.cs" Inherits="App.InfoGrid2.View.OneTable.OpTableBatchList" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form action="" id="form1" method="post">

    <mi:Store runat="server"  ID="store1" Model="IG2_BATCH_TABLE_OPERATE" IdField="IG2_BATCH_TABLE_OPERATE_ID">

    </mi:Store>

<mi:Viewport runat="server" ID="viewport1">

    <mi:Toolbar runat="server" ID="toolbar1">
        <mi:ToolBarButton Text="新建" OnClick="ser:store1.Insert()" />
    </mi:Toolbar>

    <mi:Table runat="server" ID="table1" StoreID="store1" Region="Center">
        <Columns>

            <mi:RowNumberer />
            <mi:RowCheckColumn />

            <mi:BoundField HeaderText="名称" DataField="TBO_TEXT" />
            <mi:BoundField HeaderText="备注" DataField="REMARK" />

            <mi:BoundField HeaderText="反馈" DataField="RESULT_TEXT" />

            <mi:ActionColumn>
                <mi:ActionItem Text="修改" Command="GoEdit" Icon="/res/icon/page_white_edit.png" />
            </mi:ActionColumn>

        </Columns>
    </mi:Table>

</mi:Viewport>

</form>