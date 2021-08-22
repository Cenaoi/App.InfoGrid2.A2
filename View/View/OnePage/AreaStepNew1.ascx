<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AreaStepNew1.ascx.cs"
    Inherits="App.InfoGrid2.View.OnePage.AreaStepNew1" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<link rel="stylesheet" type="text/css" href="/Core/Scripts/jstree/3.0.2/themes/default/style.css" />
<script src="/Core/Scripts/jstree/3.0.2/jstree.js" type="text/javascript"></script>

<form action="" id="form1" method="post">
<mi:Store runat="server" ID="store1" Model="IG2_TABLE" IdField="IG2_TABLE_ID" PageSize="20">
    <SelectQuery>
        <mi:Param Name="TABLE_TYPE_ID" DefaultValue="TABLE" />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:QueryStringParam Name="IG2_TABLE_ID" QueryStringField="owner_table_id" DbType="Int32" Logic="<&gt;" />
    </SelectQuery>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1">
    <mi:TreePanel runat="server" ID="TreePanel1" Dock="Left" Width="200" Region="West"  OnSelected="TreePanel1_Selected" >
        <Types>
            <mi:TreeNodeType Name="default" Icon="/res/icon/application_view_columns.png" />
            <mi:TreeNodeType Name="table" Icon="/res/icon/table.png" />
            <mi:TreeNodeType Name="view" Icon="/res/icon/view.png" />
        </Types>
    </mi:TreePanel>
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None">
        <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" ReadOnly="true" CheckedMode="Single" 
            AutoRowCheck="true" Dock="Full">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="DISPLAY" HeaderText="表名" Width="300" />
                <mi:DateTimeColumn DataField="ROW_DATE_CREATE" HeaderText="创建时间" />
            </Columns>
        </mi:Table>
    </mi:Panel>
    <mi:WindowFooter runat="server" ID="footer1">
        <mi:Button runat="server" ID="OkBtn" Text="下一步" Width="80" Height="26" Command="GoNext"
            Dock="Center" />
        <mi:Button runat="server" ID="Button2" Text="取消" Width="80" Height="26" Dock="Right"
            OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>
</form>
