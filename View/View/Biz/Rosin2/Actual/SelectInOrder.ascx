<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectInOrder.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin2.Actual.SelectInOrder" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<!--进出库明细-货物-->
<form action="" method="post" >
<mi:Store runat="server" ID="store1" Model="UT_017_PROD" IdField="ROW_IDENTITY_ID" PageSize="20" SortText="ROW_DATE_CREATE desc">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:Param Name="BIZ_SID" DefaultValue="4" Logic="&gt;=" />
        <mi:Param Name="OUT_SID"  DefaultValue="0" />
        <mi:Param Name="IO_TAG" DefaultValue="I" />
    </FilterParams>

</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >


        
        <mi:SearchFormLayout runat="server" ID="searchForm" StoreID="store1" Visible="true">

            <mi:TriggerBox ID="TriggerBox1" runat="server" FieldLabel="客户编码" DataField="CLIENT_CODE"  DataLogic="like"  ButtonType="More" >
                   <MapItems>
                        <mi:MapItem SrcField="CLIENT_CODE" TargetField="CLIENT_CODE" />
                        <mi:MapItem SrcField="CLIENT_TEXT" TargetField="CLIENT_TEXT" />
                    </MapItems>
            </mi:TriggerBox>
            <mi:TextBox ID="TextBox1" runat="server" FieldLabel="客户名称" DataField="CLIENT_TEXT" DataLogic="like" />
            <mi:TriggerBox ID="TriggerBox3" runat="server" FieldLabel="产品编号" DataField="S_PROD_CODE" DataLogic="like"  ButtonType="More" >
                 <MapItems>
                        <mi:MapItem SrcField="PROD_CODE" TargetField="S_PROD_CODE" />
                        <mi:MapItem SrcField="PROD_TEXT" TargetField="S_PROD_TEXT" />
                </MapItems>
            </mi:TriggerBox>

            <mi:TextBox ID="TextBox2" runat="server" FieldLabel="产品名称" DataField="S_PROD_TEXT" DataLogic="like" />


            <mi:SearchButtonGroup runat="server" ID="serachBtnGroup1"></mi:SearchButtonGroup>
         </mi:SearchFormLayout>

        <mi:Toolbar ID="Toolbar1" runat="server">


            <mi:ToolBarTitle ID="tableNameTB1" Text="进库明细-货物" />

            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm.toggle()" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField HeaderText="客户名称" DataField="CLIENT_TEXT" />
                <mi:BoundField HeaderText="客户编码" DataField="CLIENT_CODE" />
                <mi:BoundField HeaderText="产品编号" DataField="S_PROD_CODE" />
                <mi:BoundField HeaderText="产品名称" DataField="S_PROD_TEXT" />
                <mi:BoundField HeaderText="入库单号" DataField="IN_BILL_NO" />
                <mi:BoundField HeaderText="条码" DataField="PROD_CODE" />
                <mi:BoundField HeaderText="检验" DataField="CHECK_TEXT" />
                <mi:BoundField HeaderText="检验等级" DataField="CHECK_CODE" />
                <mi:BoundField HeaderText="检验号" DataField="CHECK_NO" />
                <mi:DateColumn HeaderText="入库时间" DataField="ROW_DATE_CREATE" />
            </Columns>
        </mi:Table>
    </mi:Panel>
    <mi:WindowFooter runat="server" ID="footer1">
        <mi:Button runat="server" ID="OkBtn" Text="确定" Width="80" Height="26" Command="GoSuccess" Dock="Center" />
        <mi:Button runat="server" ID="Button1" Text="取消" Width="80" Height="26" Dock="Center" OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>

</form>

