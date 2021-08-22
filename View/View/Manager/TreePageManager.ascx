<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreePageManager.ascx.cs" Inherits="App.InfoGrid2.View.Manager.TreePageManager" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi2" %>


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


<mi2:Viewport runat="server" ID="viewport1">

    <mi2:TreePanel runat="server" ID="TreePanel1" Dock="Left" Width="200" Region="West" OnSelected="TreePanel1_Selected" Scroll="Auto" >
        <Types>
            <mi2:TreeNodeType Name="default" Icon="/res/icon/dir.png" />
            <mi2:TreeNodeType Name="table" Icon="/res/icon/table.png" />
            <mi2:TreeNodeType Name="view" Icon="/res/icon/view.png" />
        </Types>
    </mi2:TreePanel>
    <mi2:Panel ID="Panel1" runat="server" Dock="Full" Region="Center" Scroll="None">
        <iframe dock="full" region="center" id="iform1" frameborder="0">
    
            

        </iframe>
    </mi2:Panel>
</mi2:Viewport>


</form>

<script >

    $(function () {

        $('#widget1_I_TreePanel1').css({ 'overflow': 'auto' });

    });

</script>