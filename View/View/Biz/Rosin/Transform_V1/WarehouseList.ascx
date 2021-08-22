<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WarehouseList.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.Transform_V1.WarehouseList" %>





<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<style type="text/css">
    .page-head {
        font-size: 26px;
        font-weight: bold;
    }
</style>

<form action="" method="post">

    <mi:Store runat="server" ID="store1" Model="UT_017_PROD" IdField="ROW_IDENTITY_ID" PageSize="20" LockedField="BIZ_SID" DeleteRecycle="true">
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:Param Name="IO_TAG" DefaultValue="I" />
            <mi:Param Name="BIZ_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:Param Name="OUT_SID" DefaultValue="0" />
        </FilterParams>
    </mi:Store>

    <mi:Store runat="server" ID="store2" Model="UT_019" IdField="ROW_IDENTITY_ID" PageSize="0" LockedField="BIZ_SID" DeleteRecycle="true">
        <FilterParams>
            <mi:Param Name="BIZ_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
           <mi:QueryStringParam Name="DOC_PARENT_ID" QueryStringField="row_id" />
        </FilterParams>
    </mi:Store>

    <mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="0">
        <mi:Label runat="server" ID="headLab" Value="单号" HideLabel="true" Mode="Transform" BodyAlign="Center" />
    </mi:Panel>

    <mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="40">

        <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" MinHeight="400">

            <mi:SearchFormLayout runat="server" ID="searchFrom" StoreID="store1">

                <mi:ComboBox runat="server" ID="cb_cust_name" FieldLabel="客户名称" TriggerMode="None">
                </mi:ComboBox>

                <mi:TextBox runat="server" ID="bill_no_tb" FieldLabel="入库单号" />

                <mi:SearchButtonGroup runat="server" ID="serachBtnGroup1"></mi:SearchButtonGroup>
            </mi:SearchFormLayout>

            <mi:Toolbar ID="Toolbar1" runat="server">
                <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />

            </mi:Toolbar>
            <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" SummaryVisible="false">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:SelectColumn HeaderText="锁状态" DataField="LOCK_SID" TriggerMode="None" Width="60" ItemAlign="Center">
                        <mi:ListItem Value="0" Text="" />
                        <mi:ListItem Value="2" Text="锁住" />
                    </mi:SelectColumn>
                    <mi:BoundField HeaderText="条码" DataField="PROD_CODE" />
                    <mi:BoundField HeaderText="入库单号" DataField="IN_BILL_NO" />
                    <mi:BoundField HeaderText="客户编码" DataField="CLIENT_CODE" />
                    <mi:BoundField HeaderText="客户名称" DataField="CLIENT_TEXT" />
                    <mi:BoundField HeaderText="产品编码" DataField="S_PROD_CODE" />
                    <mi:BoundField HeaderText="产品名称" DataField="S_PROD_TEXT" />
                </Columns>
            </mi:Table>

            <mi:Panel runat="server" ID="pppp1" Height="30" Dock="Bottom">
                <mi:Button runat="server" ID="MoveDownBtn1" Dock="Center" Height="30"  Text="↓ ↓ ↓  选到转移表格中 ↓ ↓ ↓ " Command="GoCheckedUT_002" />
            </mi:Panel>
        </mi:Panel>

        <mi:Panel runat="server" ID="Panel1" Dock="Full" Region="South" Scroll="None" Height="400">

            <mi:FormLayout runat="server" ID="FormLayout1" AutoSize="true" ItemWidth="300" ItemLabelAlign="Right" FlowDirection="LeftToRight">

                <mi:TextBox runat="server" ID="tbx_change_bill_no" ReadOnly="true" FieldLabel="过户单号" />


                <mi:ComboBox runat="server" ID="cb_ut_018_biz_sid" FieldLabel="单据状态" ReadOnly="true" TriggerMode="None">
                        <mi:ListItem Value="0" Text="草稿"  />
                        <mi:ListItem Value="2" Text="审核中" />
                        <mi:ListItem Value="999" Text="审核完成" />
                </mi:ComboBox>


                <mi:ComboBox runat="server" ID="cb_cust_name_2" FieldLabel="目标客户" TriggerMode="None">

                </mi:ComboBox>

            </mi:FormLayout>

            <mi:Toolbar ID="Toolbar2" runat="server">
                <mi:ToolBarButton Text="刷新" OnClick="ser:store2.Refresh()" />
                <mi:ToolBarButton Text="删除"  BeforeAskText="您确定删除记录?" OnClick="ser:store2.Delete()" />
                <mi:ToolBarButton Text="提交" Command="GoChangeBizsid0_2" />
                <mi:ToolBarButton Text="审核" Command="GoTransform" />
                <mi:ToolBarButton Text="作废" Command="GoChangeBizSID_0_F3" BeforeAskText="确定‘作废’操作？" />

            </mi:Toolbar>
            <mi:Table runat="server" ID="table2" StoreID="store2" Dock="Full" SummaryVisible="false" PagerVisible="false">
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