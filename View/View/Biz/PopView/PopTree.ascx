<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopTree.ascx.cs" Inherits="App.InfoGrid2.View.Biz.PopView.PopTree" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<link rel="stylesheet" type="text/css" href="/Core/Scripts/jstree/3.0.2/themes/default/style.css" />
<script src="/Core/Scripts/jstree/3.0.2/jstree.js" type="text/javascript"></script>

<form action="" method="post">

<mi:Viewport runat="server" ID="viewport1" Main="true">
     <mi:Panel runat="server" ID="leftPanel" Region="Center" Dock="Full" Scroll="Auto">
        <mi:TreePanel runat="server" ID="TreePanel1" Dock="Full" Width="300" Region="West"  AllowDragDrop="true">
            <Types>
                <mi:TreeNodeType Name="default" Icon="/res/icon/application_view_columns.png" />
                <mi:TreeNodeType Name="table" Icon="/res/icon/table.png" />
                <mi:TreeNodeType Name="view" Icon="/res/icon/view.png" />
            </Types>
        </mi:TreePanel>
    </mi:Panel>
    <mi:WindowFooter ID="WindowFooter1" runat="server">
         <mi:Button runat="server" ID="GoNextBtn" Width="80" Height="26" OnClick="ownerWindow.close();" Text="关闭" Dock="Center" />
     </mi:WindowFooter>
</mi:Viewport>

</form>