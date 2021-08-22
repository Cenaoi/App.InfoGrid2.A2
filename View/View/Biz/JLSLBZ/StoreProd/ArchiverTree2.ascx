<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArchiverTree2.ascx.cs" Inherits="App.InfoGrid2.View.Biz.JLSLBZ.StoreProd.ArchiverTree2" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi2" %>

<link rel="stylesheet" type="text/css" href="/Core/Scripts/jstree/3.0.2/themes/default/style.css" />
<script src="/Core/Scripts/jstree/3.0.2/jstree.js" type="text/javascript"></script>

<form action="" method="post">


<mi2:Viewport runat="server" ID="viewport1">

    <mi2:TreePanel runat="server" ID="TreePanel1"  Width="200" Scroll="Auto" Region="West">
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