<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DockTest.ascx.cs" Inherits="App.InfoGrid2.View.Explorer.DockTest" %>

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

<link href="/Core/Scripts/Mini2/Themes/theme-globel.css" rel="stylesheet" type="text/css" />
<link href="/Core/Scripts/Mini2/Themes/theme-window.css" rel="stylesheet" type="text/css" />
<% } %>
<form method="post">

<mi:Store runat="server" ID="store1" Model="IG2_ACTION" IdField="IG2_ACTION_ID" >
    <SelectQuery>
        <mi:QueryStringParam Name="IG2_ACTION_ID" QueryStringField="id" DbType="Int32" />
    </SelectQuery>
</mi:Store>

<mi:Store runat="server" ID="store2" Model="IG2_ACTION_ITEM" IdField="IG2_ACTION_ITEM_ID" DeleteRecycle="true" >
    <FilterParams>
        <mi:QueryStringParam Name="IG2_ACTION_ID" QueryStringField="id" DbType="Int32"  />
    </FilterParams>
    <SelectQuery>
        <mi:Param Name="ROW_SID" DefaultValue="-1" Logic=">" />
    </SelectQuery>
    <InsertParams>
        <mi:QueryStringParam Name="IG2_ACTION_ID" QueryStringField="id" DbType="Int32"  />
        <mi:Param Name="ITEM_TYPE_ID" DefaultValue="SET" />
        <mi:Param Name="LINK_TYPE_ID" DefaultValue="=" />
    </InsertParams>
    <DeleteQuery>
        <mi:ControlParam Name="IG2_ACTION_ITEM_ID" ControlID="table2" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
    </DeleteRecycleParams>
</mi:Store>


<mi:Viewport runat="server" ID="viewport">

    <mi:Panel runat="server" ID="buttonPanel1" Dock="Full" Region="Center" Scroll="None" Padding="4" ItemMarginLeft="6">

        <mi:TabPanel runat="server" ID="tablePanel1" Dock="Full" Plain="true" Width="0">
            <mi:Tab runat="server" ID="tab1" Text="筛选-L" Scroll="None">

            </mi:Tab>
        </mi:TabPanel>

        <mi:TabPanel runat="server" ID="TabPanel2" Dock="Right" Plain="true" Width="800" >

            <mi:Tab runat="server" ID="tab2" Text="筛选-R" Scroll="None">
                

            </mi:Tab>
            
            <mi:Tab runat="server" ID="tab3" Text="筛选-R" Scroll="None">
                

            </mi:Tab>
            <mi:Tab runat="server" ID="tab4" Text="筛选-R" Scroll="None">
                

            </mi:Tab>

        </mi:TabPanel>

    </mi:Panel>

</mi:Viewport>
</form>