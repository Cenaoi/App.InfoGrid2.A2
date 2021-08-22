<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransformList.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.Transform.TransformList" %>



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

<style type="text/css">
    .page-head
    {
         font-size:26px;
         font-weight:bold;
         
    }
</style>
  
<form action="" method="post">
<mi:Store runat="server" ID="store1" Model="UT_001" IdField="ROW_IDENTITY_ID" PageSize="20" LockedField="BIZ_SID" DeleteRecycle="true"  ReadOnly="true" >
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:Param Name="BIZ_SID" DefaultValue="999" />
        <mi:Param Name="IO_TAG" DefaultValue="转移" />
    </FilterParams>

</mi:Store>


<mi:Viewport runat="server" ID="viewport1" Main="true">

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        
        <mi:SearchFormLayout runat="server" ID="searchFrom" StoreID="store1" Visible="false">
            <mi:TextBox runat="server" ID="textBox1" DataField="BILL_NO" FieldLabel="单号" DataLogic="like" />
            <mi:TextBox runat="server" ID="textBox2" DataField="CCK_NO" FieldLabel="储存卡号" DataLogic="like" />
            <mi:TextBox runat="server" ID="textBox3" DataField="CLIENT_NAME" FieldLabel="客户名称" DataLogic="like" />
            <mi:TextBox runat="server" ID="textBox4" DataField="COL_1" FieldLabel="开单人" DataLogic="like" />
            <mi:TextBox runat="server" ID="textBox5" DataField="COL_2" FieldLabel="审核人" DataLogic="like" />
            <mi:SearchButtonGroup runat="server" ID="serachBtnGroup1"></mi:SearchButtonGroup>
        </mi:SearchFormLayout>
        
        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" JsonMode="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:ActionColumn HeaderText="功能" Width="60">
                    <mi:ActionItem Text="编辑" Command="GoEdit" DisplayMode="Text"  />
                </mi:ActionColumn>

                <mi:BoundField HeaderText="单号" DataField="BILL_NO" />
                <mi:BoundField HeaderText="原来客户名称" DataField="SRC_CLIENT_TEXT"  Required="true" />
                <mi:BoundField HeaderText="现在客户名称" DataField="CLIENT_NAME"  Required="true" />
                <mi:NumColumn HeaderText="合计" DataField="ITEM_TOTAL" ItemAlign="Right"  Width="80" NotDisplayValue="0" />
                <mi:NumColumn HeaderText="数量" DataField="NUM_TOTAL" ItemAlign="Right" Width="80"  NotDisplayValue="0" />
                <mi:NumColumn HeaderText="重量" DataField="WEIGHT_TOTAL" ItemAlign="Right" Width="80"  NotDisplayValue="0" />
                                
                <mi:BoundField HeaderText="到货方式" DataField="DH_TYPE" Width="70" />

                <mi:BoundField HeaderText="备注" DataField="REMARK" />
                <mi:BoundField HeaderText="开单人" DataField="COL_1" Width="80"  />
                <mi:BoundField HeaderText="审核人" DataField="COL_2"  Width="80" />
                <mi:BoundField HeaderText="开单时间" DataField="COL_3" />

            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>


</form>