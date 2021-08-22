﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MViewStepEdit2.ascx.cs" Inherits="App.EC52Demo.View.ViewSetup.MViewStepEdit2" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" method="post">

<mi:Store runat="server" ID="store2" Model="IG2_TMP_VIEW_TABLE" IdField="IG2_TMP_VIEW_TABLE_ID" PageSize="20" SortField="ROW_USER_SEQ" >
    <FilterParams>
        <mi:QueryStringParam Name="TMP_GUID" QueryStringField="uid" DbType="Guid"  />
        <mi:QueryStringParam Name="IG2_VIEW_ID" QueryStringField="id" DbType="Int32"  />
    </FilterParams>
    <SelectQuery>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" DbType="Int32" />
    </SelectQuery>
    <DeleteQuery>
      <mi:ControlParam Name="IG2_TMP_VIEW_TABLE_ID" ControlID="table2" PropertyName="CheckedRows" />
    </DeleteQuery>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1">
    <mi:Panel runat="server" ID="centerPanel" Dock="Center" Region="Center" Scroll="None">
        <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarButton Text="刷新" OnClick="ser:store2.Refresh()" />
            <mi:ToolBarButton Text="删除" OnClick="ser:store2.Delete()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton OnClick="ser:store2.MoveUp()" Text="上移" />
            <mi:ToolBarButton OnClick="ser:store2.MoveDown()" Text="下移" />
            <mi:ToolBarHr />

            <mi:ToolBarButton Text="选择表" OnClick="SelectTabel()" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table2" StoreID="store2" ReadOnly="true" CheckedMode="Single" Sortable="false" Dock="Full" PagerVisible="false">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="TABLE_NAME" HeaderText="真实表名" />
                <mi:BoundField DataField="TABLE_TEXT" HeaderText="显示表名" Width="300" />
                <mi:DateTimeColumn DataField="ROW_DATE_CREATE" HeaderText="创建时间"  />
            </Columns>

        </mi:Table>
    </mi:Panel>
    <mi:WindowFooter runat="server" ID="footer1">
        <%--<mi:Button runat="server" ID="Button1" Text="上一步" Width="80" Height="26" Command="GoBack"
            Dock="Center" />--%>
        <mi:Button runat="server" ID="OkBtn" Text="下一步" Width="80" Height="26" Command="GoNext"
            Dock="Center" />
        <mi:Button runat="server" ID="Button2" Text="取消" Width="80" Height="26" Dock="Right"
            OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>
</form>
<script >
    function SelectTabel() {
        var urlStr = "ShowTableAll.aspx";
        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '选择数据表',
            iframe: true,
            width: 800,
            height: 600,
            startPosition: 'center_screen',
            url: urlStr
        });

        win.show();

        win.formClosed(function (e) {
            if (e.result != 'ok') { return; }
            widget1.submit('form:first', {
                action: 'InsertTable',
                actionPs: e.ids
            });



        });
    }
    
</script>
<p>
    &lt;</p>

