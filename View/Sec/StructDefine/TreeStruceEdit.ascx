<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreeStruceEdit.ascx.cs" Inherits="App.InfoGrid2.Sec.StructDefine.TreeStruceEdit" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<link rel="stylesheet" type="text/css" href="/Core/Scripts/jstree/3.0.2/themes/default/style.css" />
<script src="/Core/Scripts/jstree/3.0.2/jstree.js" type="text/javascript"></script>

<form method="post"  >
<mi:Viewport runat="server" ID="viewport1">

    <mi:TreePanel runat="server" ID="TreePanel1" Dock="Left" Width="300" Region="West" OnSelected="TreePanel1_Selected">
        <Types>
            <mi:TreeNodeType Name="default" Icon="/res/icon/dir.png" />
            <mi:TreeNodeType Name="table" Icon="/res/icon/table.png" />
            <mi:TreeNodeType Name="view" Icon="/res/icon/view.png" />
        </Types>
    </mi:TreePanel>
    <mi:Panel ID="Panel1" runat="server" Dock="Full" Region="Center" Scroll="None">
        <iframe dock="full" region="center" id="iform1" frameborder="0">

        </iframe>
    </mi:Panel>
</mi:Viewport>

</form>

