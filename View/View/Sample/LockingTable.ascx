<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LockingTable.ascx.cs" Inherits="App.InfoGrid2.View.Sample.LockingTable" %>
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


    <mi:Store runat="server" ID="store1" Model="BIZ_FILE" IdField="BIZ_FILE_ID" AutoSave="false" AutoCallback="false" ReadOnly="true">

    </mi:Store>
<mi:Viewport runat="server" ID="viewport1">

    <mi:FormLayout runat="server" Region="North" Height="100">
        <mi:DatePicker runat="server" ID="dddd" Format="Y-m-d H:i:s"  ShowTime="true" />
    </mi:FormLayout>

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >

        <mi:Table runat="server" ID="table2" StoreID="store1" Dock="Left" PagerVisible="false" Width="300px" Scroll="None">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:NumColumn DataField="TABLE_NAME" HeaderText="姓名" Format="0" Width="200" NotDisplayValue="0" />
            </Columns>
        </mi:Table>


        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" PagerVisible="false">
            <Columns>
                <mi:NumColumn DataField="TABLE_ID" HeaderText="数值" Format="0" Width="200" NotDisplayValue="0" />
                <mi:DateColumn DataField="DATE_1" HeaderText="日期" />
                <mi:NumColumn DataField="STR_1" HeaderText="字符串" />

            </Columns>
        </mi:Table>


    </mi:Panel>
</mi:Viewport>
