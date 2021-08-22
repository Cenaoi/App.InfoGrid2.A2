<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WarehouseList.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.Transform.WarehouseList" %>



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
    .page-head {
        font-size: 26px;
        font-weight: bold;
    }
</style>

<form action="" method="post">

    <mi:Store runat="server" ID="store1" Model="UT_002" IdField="ROW_IDENTITY_ID" PageSize="20" LockedField="BIZ_SID" DeleteRecycle="true">
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:Param Name="IO_TAG" DefaultValue="I" />
            <mi:Param Name="SURPLUS_NUM" DefaultValue="0" Logic="&gt;" />
            <mi:Param Name="SURPLUS_WEIGHT" DefaultValue="0" Logic="&gt;" />
        </FilterParams>
        <SummaryFields>
            <mi:SummaryField DataField="SURPLUS_NUM" SummaryType="SUM">
            </mi:SummaryField>
            <mi:SummaryField DataField="SURPLUS_WEIGHT" SummaryType="SUM">
            </mi:SummaryField>
        </SummaryFields>
    </mi:Store>

    <mi:Store runat="server" ID="store2" Model="UT_002" IdField="ROW_IDENTITY_ID" PageSize="0" LockedField="BIZ_SID" DeleteRecycle="true">
        <FilterParams>
            <mi:Param Name="BIZ_SID" DefaultValue="0" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:Param Name="IO_TAG" DefaultValue="转移" />
            <mi:Param Name="SURPLUS_NUM" DefaultValue="0" Logic="&gt;" />
            <mi:Param Name="SURPLUS_WEIGHT" DefaultValue="0" Logic="&gt;" />
        </FilterParams>
        <SummaryFields>
            <mi:SummaryField DataField="SURPLUS_NUM" SummaryType="SUM">
            </mi:SummaryField>
            <mi:SummaryField DataField="SURPLUS_WEIGHT" SummaryType="SUM">
            </mi:SummaryField>
        </SummaryFields>
    </mi:Store>

    <mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="0">
        <mi:Label runat="server" ID="headLab" Value="单号" HideLabel="true" Mode="Transform" BodyAlign="Center" />
    </mi:Panel>

    <mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="40">

        <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None">

            <mi:SearchFormLayout runat="server" ID="searchFrom" StoreID="store1">

                <mi:ComboBox runat="server" ID="cb_cust_name" FieldLabel="客户名称" DataField="P_CLIENT_TEXT" TriggerMode="None">
                </mi:ComboBox>
                <mi:SearchButtonGroup runat="server" ID="serachBtnGroup1"></mi:SearchButtonGroup>
            </mi:SearchFormLayout>

            <mi:Toolbar ID="Toolbar1" runat="server">
                <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />

            </mi:Toolbar>
            <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" SummaryVisible="false">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField HeaderText="单号代码" DataField="P_BILL_CODE" EditorMode="None" />
                    <mi:BoundField HeaderText="客户名称" DataField="P_CLIENT_TEXT" EditorMode="None" />
                    <mi:BoundField HeaderText="货主" DataField="COL_27" EditorMode="None" />
                    <mi:BoundField HeaderText="货物品名" DataField="COL_3" EditorMode="None" />
                    <mi:NumColumn HeaderText="剩余数量" DataField="SURPLUS_NUM" SummaryType="SUM" SummaryFormat="数量: {0}" />
                    <mi:BoundField HeaderText="数量单位" DataField="COL_22" Width="60" EditorMode="None" />
                    <mi:NumColumn HeaderText="剩余重量" DataField="SURPLUS_WEIGHT" SummaryType="SUM" SummaryFormat="数量: {0}" />
                    <mi:BoundField HeaderText="重量单位" DataField="COL_24" Width="60" EditorMode="None" />
                    
                    <mi:BoundField HeaderText="条码" DataField="BIZ_ROW_CODE" />

                </Columns>
            </mi:Table>

            <mi:Panel runat="server" ID="pppp1" Height="30" Dock="Bottom">
                <mi:Button runat="server" ID="MoveDownBtn1" Dock="Center" Height="30"  Text="↓ ↓ ↓  选到转移表格中 ↓ ↓ ↓ " Command="GoCheckedUT_002" />
            </mi:Panel>
        </mi:Panel>

        <mi:Panel runat="server" ID="Panel1" Dock="Full" Region="South" Scroll="None" Height="400">

            <mi:SearchFormLayout runat="server" ID="SearchFormLayout1" StoreID="store2">

                <mi:ComboBox runat="server" ID="cb_cust_name_2" FieldLabel="目标客户" TriggerMode="None">

                </mi:ComboBox>

            </mi:SearchFormLayout>

            <mi:Toolbar ID="Toolbar2" runat="server">
                <mi:ToolBarButton Text="刷新" OnClick="ser:store2.Refresh()" />
                <mi:ToolBarButton Text="删除"  BeforeAskText="您确定删除记录?" OnClick="ser:store2.Delete()" />
                <mi:ToolBarButton Text="转移" Command="GoTransform" />

            </mi:Toolbar>
            <mi:Table runat="server" ID="table2" StoreID="store2" Dock="Full" SummaryVisible="false" PagerVisible="false">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField HeaderText="单号代码" DataField="P_BILL_CODE" EditorMode="None" />
                    <mi:BoundField HeaderText="客户名称" DataField="P_CLIENT_TEXT" EditorMode="None" />
                    <mi:BoundField HeaderText="货主" DataField="COL_27" EditorMode="None" />
                    <mi:BoundField HeaderText="货物品名" DataField="COL_3" EditorMode="None" />
                    <mi:NumColumn HeaderText="剩余数量" DataField="SURPLUS_NUM" SummaryType="SUM" SummaryFormat="数量: {0}" />
                    <mi:BoundField HeaderText="数量单位" DataField="COL_22" Width="60" EditorMode="None" />
                    <mi:NumColumn HeaderText="剩余重量" DataField="SURPLUS_WEIGHT" SummaryType="SUM" SummaryFormat="数量: {0}" />
                    <mi:BoundField HeaderText="重量单位" DataField="COL_24" Width="60" EditorMode="None" />
                    
                    <mi:BoundField HeaderText="条码" DataField="BIZ_ROW_CODE" EditorMode="None" />

                </Columns>
            </mi:Table>
        </mi:Panel>

    </mi:Viewport>
</form>