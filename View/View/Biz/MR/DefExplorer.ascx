<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DefExplorer.ascx.cs" Inherits="App.InfoGrid2.View.Biz.MR.DefExplorer" %>


<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<link rel="stylesheet" type="text/css" href="/Core/Scripts/jstree/3.0.2/themes/default/style.css" />
<script src="/Core/Scripts/jstree/3.0.2/jstree.js" type="text/javascript"></script>

<script src="/Core/Scripts/webuploader-0.1.5/webuploader.js"></script>
<link href="/Core/Scripts/webuploader-0.1.5/webuploader.css" rel="stylesheet" />
<form action="" method="post">

<mi:Store runat="server" ID="store1" Model="IG2_MAP"  PageSize="0" AutoSave="true" >
    <TSqlQuery Enabeld="true"></TSqlQuery>
</mi:Store>

<mi:Viewport runat="server" ID="viewport1" Main="true">
     <mi:Panel runat="server" ID="leftPanel" Region="West" Width="320" Scroll="Auto">
        <mi:TreePanel runat="server" ID="TreePanel1" Dock="Full" Width="300" Region="West" OnSelected="TreePanel1_Selected" AllowDragDrop="true">
            <Types>
                <mi:TreeNodeType Name="default" Icon="/res/icon/application_view_columns.png" />
                <mi:TreeNodeType Name="table" Icon="/res/icon/table.png" />
                <mi:TreeNodeType Name="view" Icon="/res/icon/view.png" />
            </Types>
        </mi:TreePanel>
    </mi:Panel>
    <mi:Panel ID="Panel1" runat="server" Dock="Full" Region="Center" Scroll="None">
      


        <mi:Toolbar ID="Toolbar2" runat="server">
            <mi:ToolBarTitle ID="tableNameTB2" Text="域名列表" />


            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />

            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:store1.Delete()" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table2" StoreID="store1" Dock="Full" ReadOnly="true" JsonMode="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField  HeaderText="文件名" DataField="FILE_NAME" />
                <mi:BoundField HeaderText="文件大小" DataField="FILE_SIZE" />

            </Columns>
        </mi:Table>

        <mi:Panel runat="server" ID="footer1" Dock="Bottom" Height="100">

            <mi:FileUpload runat="server" ID="fileUpload1" PluginType="OtherFile" FieldLabel="文件上传"   />

        </mi:Panel>

    </mi:Panel>

</mi:Viewport>

</form>

