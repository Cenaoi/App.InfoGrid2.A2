<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserList.ascx.cs" Inherits="App.InfoGrid2.Sec.UIFilterTU.UserList" %>
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
<mi:Store runat="server" ID="store1" Model="SEC_LOGIN_ACCOUNT" IdField="SEC_LOGIN_ACCOUNT_ID" PageSize="0">
    <StringFields>SEC_LOGIN_ACCOUNT_ID,TRUE_NAME,BIZ_USER_CODE,IS_USER_SEC</StringFields>
    <FilterParams>
        <mi:Param Name="ROW_STATUS_ID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="ROW_IDENTITY_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>

    <DeleteRecycleParams>
        <mi:Param Name="ROW_STATUS_ID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">
    <mi:SearchFormLayout runat="server" ID="searchForm" StoreID="store1" Visible="false">

    </mi:SearchFormLayout>
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="West" Scroll="None" Width="400"  >

        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarTitle ID="tableNameTB1" Text="用户" />
            <mi:ToolBarButton Tooltip="刷新" Icon="/res/icon_sys/Refresh.png" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarButton Text="同步" Command="Synchronous" />
            <mi:ToolBarButton Text="清除" Command="Resetting"  BeforeAskText="重置后将清除原设置，是否继续？" />
            <mi:ToolBarButton Text="添加查询界面" Command="AddSearchView" />
            <mi:ToolBarButton Text="复制权限" Command="CopySec" />
            <mi:ToolBarButton Text="同步工具栏" Command="SysnToolbar" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" PagerVisible="false" CheckedMode="Single" AutoRowCheck="true" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:CheckColumn HeaderText="自定义" DataField="IS_USER_SEC" />
                <mi:BoundField HeaderText="真实名称" DataField="TRUE_NAME" EditorMode="None" />
                <mi:BoundField HeaderText="用户编码" DataField="BIZ_USER_CODE" EditorMode="None" />
            </Columns>
        </mi:Table>
    </mi:Panel>

    <mi:Panel ID="Panel1" runat="server" Dock="Full" Region="Center" Scroll="None">
        <iframe dock="full" region="center" id="iform1" frameborder="0">
    
            

        </iframe>
    </mi:Panel>
</mi:Viewport>

</form>