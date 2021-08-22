<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LogActMgr.ascx.cs" Inherits="App.InfoGrid2.View.LogAct.LogActMgr" %>
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

    
    <mi:Panel runat="server" ID="panel2" Region="West" Width="400">
    <mi:TreePanel runat="server" ID="TreePanel1" Dock="Full" Width="400" Region="West" Scroll="Auto">
        <Types>
            <mi:TreeNodeType Name="default" Icon="/res/icon/dir.png" />
            <mi:TreeNodeType Name="table" Icon="/res/icon/table.png" />
            <mi:TreeNodeType Name="view" Icon="/res/icon/view.png" />
        </Types>
    </mi:TreePanel>
    </mi:Panel>
    <mi:Panel ID="Panel1" runat="server" Dock="Full" Region="Center" Scroll="None">
        <iframe dock="full" region="center" id="iform1" frameborder="0">
    
            

        </iframe>
    </mi:Panel>
</mi:Viewport>


</form>

