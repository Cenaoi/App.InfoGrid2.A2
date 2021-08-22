<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreeStruceCheckDialog.ascx.cs" Inherits="App.InfoGrid2.Sec.StructDefine.TreeStruceCheckDialog" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<link rel="stylesheet" type="text/css" href="/Core/Scripts/jstree/3.0.2/themes/default/style.css" />
<script src="/Core/Scripts/jstree/3.0.2/jstree.js" type="text/javascript"></script>

<form method="post"  >
<mi:Viewport runat="server" ID="viewport1">
    <mi:Panel runat="server" ID="panel1" Region="Center">
        <mi:TreePanel runat="server" ID="TreePanel1" Dock="Full" ShowCheckBox="true" CheckValueField="SEC_STRUCT_ID">
            <Types>
                <mi:TreeNodeType Name="default" Icon="/res/icon/dir.png" />
                <mi:TreeNodeType Name="table" Icon="/res/icon/table.png" />
                <mi:TreeNodeType Name="view" Icon="/res/icon/view.png" />
            </Types>
        </mi:TreePanel>
    </mi:Panel>
    <mi:WindowFooter runat="server" ID="footer1" >
        <mi:Button ID="Button1" runat="server" Text="确定" Command="GoSubmit" Width="80" Dock="Center" />
        <mi:Button ID="Button2" runat="server" Text="取消" OnClick="ownerWindow.close()" Width="80" Dock="Center" />
    </mi:WindowFooter>
</mi:Viewport>

</form>


