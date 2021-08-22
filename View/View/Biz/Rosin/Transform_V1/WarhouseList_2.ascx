<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WarhouseList_2.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.Transform_V1.WarhouseList_2" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>



<style type="text/css">
    .page-head {
        font-size: 26px;
        font-weight: bold;
    }
</style>


<!--过户单明细-货物-->
<form action="" method="post" >
<mi:Store runat="server" ID="store1" Model="UT_019" IdField="ROW_IDENTITY_ID" PageSize="20">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:Param Name="BIZ_SID" DefaultValue="0" Logic="&gt;" />
        <mi:QueryStringParam Name="DOC_PARENT_ID" QueryStringField="row_id" />
    </FilterParams>
</mi:Store>



<mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="0">
    <mi:Label runat="server" ID="headLab" Value="单号" HideLabel="true" Mode="Transform" BodyAlign="Center" />
</mi:Panel>

<mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="40">

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >

           <mi:FormLayout runat="server" ID="FormLayout1" AutoSize="true" ItemWidth="300" ItemLabelAlign="Right" FlowDirection="LeftToRight">

                <mi:TextBox runat="server" ID="tbx_change_bill_no" ReadOnly="true" FieldLabel="过户单号" />


                <mi:ComboBox runat="server" ID="cb_ut_018_biz_sid" FieldLabel="单据状态" ReadOnly="true" TriggerMode="None">
                        <mi:ListItem Value="0" Text="草稿"  />
                        <mi:ListItem Value="2" Text="审核中" />
                        <mi:ListItem Value="999" Text="审核完成" />
                </mi:ComboBox>

                <mi:TextBox runat="server" ID="tbx_now_cust_name" ReadOnly="true" FieldLabel="现在客户" />

                <mi:TextBox runat="server" ID="tbx_old_cust_name" ReadOnly="true" FieldLabel="原来客户" />

            </mi:FormLayout>

        <mi:Toolbar ID="Toolbar1" runat="server">

            <mi:ToolBarTitle ID="tableNameTB1" Text="过户单明细-货物" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:SelectColumn  HeaderText="业务状态" DataField="BIZ_SID" TriggerMode="None" EditorMode="None" >
                        <mi:ListItem Value="0" Text="草稿" />
                        <mi:ListItem Value="2" Text="审核中" />
                        <mi:ListItem Value="999" Text="审核完成" />
                </mi:SelectColumn>
                <mi:BoundField HeaderText="条码" DataField="PROD_CODE" />
                <mi:BoundField HeaderText="客户编码" DataField="CLIENT_CODE" />
                <mi:BoundField HeaderText="客户名称" DataField="CLIENT_TEXT" />
                <mi:BoundField HeaderText="产品编码" DataField="S_PROD_CODE" />
                <mi:BoundField HeaderText="产品名称" DataField="S_PROD_TEXT" />

            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>

</form>
