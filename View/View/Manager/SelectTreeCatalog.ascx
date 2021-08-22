<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectTreeCatalog.ascx.cs" Inherits="App.InfoGrid2.View.Manager.SelectTreeCatalog" %>
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
<link rel="stylesheet" type="text/css" href="/Core/Scripts/jstree/3.0.2/themes/default/style.css" />
<script src="/Core/Scripts/jstree/3.0.2/jstree.js" type="text/javascript"></script>

<form action="" method="post">


<mi:Viewport runat="server" ID="viewport1">

    <mi:TreePanel runat="server" ID="TreePanel1" Dock="Left" Width="200" Region="Center" OnSelected="TreePanel1_Selected">
        <Types>
            <mi:TreeNodeType Name="default" Icon="/res/icon/dir.png" />
            <mi:TreeNodeType Name="table" Icon="/res/icon/table.png" />
            <mi:TreeNodeType Name="view" Icon="/res/icon/view.png" />
        </Types>
    </mi:TreePanel>
    <mi:WindowFooter runat="server" ID="footer1">
        <mi:Button runat="server" ID="OkBtn" Text="确定" Width="80" Height="26" Command="GoSubmit" Dock="Center" />
        <mi:Button runat="server" ID="Button1" Text="取消" Width="80" Height="26" Dock="Center" OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>


</form>
