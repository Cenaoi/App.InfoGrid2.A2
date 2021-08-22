<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OnTableSelect.ascx.cs" Inherits="App.InfoGrid2.View.OneTable.OnTableSelect" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<link rel="stylesheet" type="text/css" href="/Core/Scripts/jstree/3.0.2/themes/default/style.css" />
<script src="/Core/Scripts/jstree/3.0.2/jstree.js" type="text/javascript"></script>

<form action="" id="form1" method="post">


    <mi:Store runat="server"  ID="store1" Model="IG2_TABLE" IdField="IG2_TABLE_ID"  PageSize="0" ReadOnly="true">
        <StringFields>IG2_TABLE_ID,TABLE_NAME,DISPLAY,IS_BIG_TITLE_VISIBLE,REMARK,ROW_DATE_CREATE</StringFields>
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
    </mi:Store>

<mi:Viewport runat="server" ID="viewport1">
    
    <mi:TreePanel runat="server" ID="TreePanel1" Dock="Left" Width="200" Region="West" OnSelected="TreePanel1_Selected"  >
        <Types>
            <mi:TreeNodeType Name="default" Icon="/res/icon/dir.png" />
            <mi:TreeNodeType Name="table" Icon="/res/icon/table.png" />
            <mi:TreeNodeType Name="view" Icon="/res/icon/view.png" />
        </Types>
    </mi:TreePanel>

    <mi:Panel runat="server" ID="panel1" Region="Center" Scroll="None" >
        <mi:Toolbar runat="server" ID="toolbar1">
            <mi:ToolBarButton Text="新建" OnClick="ser:store1.Insert()" />
        </mi:Toolbar>

        <mi:Table runat="server" ID="table1" StoreID="store1" Region="Center" Dock="Full" PagerVisible="false" JsonMode="Full">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="IG2_TABLE_ID" HeaderText="#" Width="60" ItemAlign="Center" EditorMode="None" />
                <mi:BoundField DataField="TABLE_NAME" HeaderText="数据表" Width="160" EditorMode="None" />
                <mi:BoundField DataField="DISPLAY" HeaderText="表名" Width="300"  />

                <mi:BoolColumn DataField="IS_BIG_TITLE_VISIBLE" HeaderText="大标题" Width="100" TrueText="显示" FalseText="隐藏" />
              
                <mi:BoundField DataField="REMARK" HeaderText="备注" Width="300"  />
                <mi:DateColumn DataField="ROW_DATE_CREATE" HeaderText="创建时间" Format="Y-m-d H:i" Width="140"  EditorMode="None"/>
            </Columns>
        </mi:Table>
    </mi:Panel>
    <mi:WindowFooter runat="server" ID="footer1">
        <mi:Button runat="server" ID="okBtn" Text="确定" Width="80" Dock="Center" Command="GoOk_Click" />
        <mi:Button runat="server" ID="Button1" Text="取消" OnClick="ownerWindow.close()" Width="80" Dock="Center" />
    </mi:WindowFooter>
</mi:Viewport>

</form>