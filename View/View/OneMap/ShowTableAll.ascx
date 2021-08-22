<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShowTableAll.ascx.cs" Inherits="App.InfoGrid2.View.OneMap.ShowTableAll" %>


<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<link rel="stylesheet" type="text/css" href="/Core/Scripts/jstree/3.0.2/themes/default/style.css" />
<script src="/Core/Scripts/jstree/3.0.2/jstree.js" type="text/javascript"></script>
<form action="" id="form1" method="post">

<mi:Store runat="server" ID="store1" Model="IG2_TABLE" IdField="IG2_TABLE_ID"  >
    <FilterParams>
       <mi:TSqlWhereParam Where="ROW_SID >= 0 and TABLE_TYPE_ID in ('TABLE','MORE_VIEW') " />
    </FilterParams>
</mi:Store>

<mi:Viewport runat="server" ID="viewport">
    
    <mi:TreePanel runat="server" ID="TreePanel1" Dock="Left" Width="200" Region="West"  OnSelected="TreePanel1_Selected" >
        <Types>
            <mi:TreeNodeType Name="default" Icon="/res/icon/application_view_columns.png" />
            <mi:TreeNodeType Name="table" Icon="/res/icon/table.png" />
            <mi:TreeNodeType Name="view" Icon="/res/icon/view.png" />
        </Types>
    </mi:TreePanel>
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="true" StoreID="store1" Dock="Full" 
        CheckedMode="Single" AutoRowCheck="true" ReadOnly="true" PagerVisible="false" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="DISPLAY" HeaderText="表名" Width="300"  />
            </Columns>
        </mi:Table>
    </mi:Panel>
    <mi:WindowFooter runat="server" ID="footer1">
        <mi:Button runat="server" ID="OkBtn" Text="确定选择" Width="80" Height="26" Command="SelectTable"
            Dock="Center" />
        <mi:Button runat="server" ID="Button2" Text="取消" Width="80" Height="26" Dock="Right"
            OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>

</form>