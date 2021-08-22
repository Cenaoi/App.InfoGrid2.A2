<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustList.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.Transform.CustList" %>

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
<mi:Store runat="server" ID="store1" Model="UT_005" IdField="ROW_IDENTITY_ID" PageSize="0" ReadOnly="true">
    <StringFields>CLIENT_CODE,CLIENT_TEXT,ROW_IDENTITY_ID</StringFields>
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">
    <mi:SearchFormLayout runat="server" ID="searchForm" StoreID="store1" Visible="false">

    </mi:SearchFormLayout>
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="West" Scroll="None" Width="200"  >

        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarTitle ID="tableNameTB1" Text="客户" />
            <mi:ToolBarButton Tooltip="刷新" Icon="/res/icon_sys/Refresh.png" OnClick="ser:store1.Refresh()" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" PagerVisible="false" ReadOnly="true" CheckedMode="Single" AutoRowCheck="true" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField HeaderText="客户名称" DataField="CLIENT_TEXT" />
                <mi:BoundField HeaderText="客户编码" DataField="CLIENT_CODE" />
            </Columns>
        </mi:Table>
    </mi:Panel>

    <mi:Panel ID="Panel1" runat="server" Dock="Full" Region="Center" Scroll="None">
        <iframe dock="full" region="center" id="iform1" frameborder="0">
    
            

        </iframe>
    </mi:Panel>
</mi:Viewport>

</form>

