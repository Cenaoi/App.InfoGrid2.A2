<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MViewStepEdit3.ascx.cs" Inherits="App.EC52Demo.View.ViewSetup.MViewStepEdit3" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" method="post">

<mi:Store runat="server" ID="store1" Model="IG2_TMP_VIEW" IdField="IG2_TMP_VIEW_ID" PageSize="20">
    <FilterParams>
        <mi:QueryStringParam Name="TMP_GUID" QueryStringField="uid" DbType="Guid"  />
    </FilterParams>
    <SelectQuery>
        <mi:QueryStringParam Name="IG2_VIEW_ID" QueryStringField="id" DbType="Int32"  />
    </SelectQuery>
</mi:Store>

<mi:Store runat="server" ID="store3" Model="IG2_TMP_VIEW_TABLE" IdField="IG2_TMP_VIEW_TABLE_ID" PageSize="0" SortField="ROW_USER_SEQ" DeleteRecycle="true">
    <FilterParams>
        <mi:QueryStringParam Name="TMP_GUID" QueryStringField="uid" DbType="Guid" />
        <mi:QueryStringParam Name="IG2_VIEW_ID" QueryStringField="id" DbType="Int32"  />
    </FilterParams>
    <DeleteQuery>
      <mi:ControlParam Name="IG2_TMP_VIEW_TABLE_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:SessionParam Name="ROW_DATE_DELETE" SessionField="TIME_NOW" />
    </DeleteRecycleParams>
</mi:Store>

<mi:Store runat="server" ID="store2" Model="IG2_TMP_VIEW_FIELD" IdField="IG2_TMP_VIEW_FIELD_ID" PageSize="0" SortField="ROW_USER_SEQ" DeleteRecycle="true">
    <FilterParams>
        <mi:QueryStringParam Name="TMP_GUID" QueryStringField="uid" DbType="Guid"  />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="IG2_TMP_VIEW_FIELD_ID" ControlID="table2" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="TMP_OP_ID" DefaultValue="D" />
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
    </DeleteRecycleParams>
    <UpdateParams>
        <mi:ServerParam Name="ROW_DATE_UPDATE" ServerField="TIME_NOW" />
        <mi:Param Name="TMP_OP_ID" DefaultValue="E" />
    </UpdateParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1">
    <mi:Panel runat="server" ID="top_panel" Dock="Top" Region="North" Height="160" ItemMarginRight="4" ItemMarginLeft="4">

        <mi:Panel runat="server" ID="Panel2" Dock="Full" Region="Center" Scroll="None">
            <mi:Toolbar runat="server" ID="toolbar1">
                <mi:ToolBarButton Text="刷新" OnClick="ser:store3.Refresh()" />
            </mi:Toolbar>
            <mi:Table runat="server" ID="table3" StoreID="store3" Sortable="false" PagerVisible="false" Dock="Full">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField DataField="TABLE_NAME" HeaderText="数据表名" EditorMode="None" />
                    <mi:BoundField DataField="TABLE_TEXT" HeaderText="工作表名"  EditorMode="None" Width="200" />
                    <mi:BoundField DataField="DB_FIELD" HeaderText="字段名"   />
                    <mi:SelectColumn DataField="RELATION_TYPE" HeaderText="关联类型" TriggerMode="None">
                        <mi:ListItem Text="左连接" Value="left" />
                        <mi:ListItem Text="右连接" Value="right" />
                        <mi:ListItem Text="内连接" Value="inner" />
                        <mi:ListItem Text="全连接" Value="full" />
                    </mi:SelectColumn>
                    <mi:SelectColumn DataField="JOIN_TABLE_NAME" HeaderText="关联表名" TriggerMode="None" Width="200">
                        
                    </mi:SelectColumn>
                    <mi:BoundField DataField="JOIN_DB_FIELD" HeaderText="关联字段" />

                    <mi:NumColumn DataField="FIELD_SEQ" HeaderText="顺序" />

                </Columns>

            </mi:Table>
        </mi:Panel>
        <mi:Panel runat="server" ID="Panel1" Dock="Right" Region="North" Scroll="None" Width="400">
            <mi:Toolbar runat="server" ID="toolbar3">
                <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            </mi:Toolbar>
            <mi:Table runat="server" ID="table1" StoreID="store1" CheckedMode="Single"  PagerVisible="false" Sortable="false"
                    AutoRowCheck="true" Dock="Full">
                    <Columns>
                        <mi:RowNumberer />
                        <mi:SelectColumn DataField="MAIN_TABLE_NAME" HeaderText="主表" TriggerMode="None" Width="200">
                        </mi:SelectColumn>
                        <mi:BoundField DataField="MAIN_ID_FIELD" HeaderText="主键" />
                    </Columns>
            </mi:Table>
        </mi:Panel>

    </mi:Panel>

    <mi:Panel runat="server" ID="centerPanel" Dock="Center" Region="Center" Scroll="None">
        <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarTitle Text="字段集" />
            <mi:ToolBarButton Text="添加字段" Command="SelectField" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store2.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton OnClick="ser:store2.MoveUp()" Text="上移" />
            <mi:ToolBarButton OnClick="ser:store2.MoveDown()" Text="下移" />
            <mi:ToolBarHr />
            
            <mi:ToolBarButton Text="删除" OnClick="ser:store2.Delete()" BeforeAskText="确定删除字段?" />

            <mi:ToolBarButton Text="修复字段" Command="RepairField" Align="Right" />


        </mi:Toolbar>
        <mi:Table runat="server" ID="table2" StoreID="store2"  Sortable="false" Dock="Full" PagerVisible="false">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="FIELD_NAME" HeaderText="字段名" EditorMode="None" />
                <mi:BoundField DataField="FIELD_TEXT" HeaderText="列名" Width="200" />
                <mi:BoundField DataField="TABLE_TEXT" HeaderText="表名" EditorMode="None" Width="200" />
                <mi:BoundField DataField="ALIAS" HeaderText="别名" />
                <mi:CheckColumn DataField="FIELD_VISIBLE" HeaderText="输出" />

                <mi:SelectColumn DataField="SORT_TYPE" HeaderText="排序类型" TriggerMode="None">
                    <mi:ListItem Value="" Text="" TextEx="----空----" />
                    <mi:ListItem Value="ASC" Text="降序"  />
                    <mi:ListItem Value="DESC" Text="升序"  />
                </mi:SelectColumn>
                <mi:NumColumn DataField="SORT_ORDER" HeaderText="排序顺序"  NotDisplayValue="0" />
                <mi:BoundField DataField="FILTER_1" HeaderText="筛选器1" />
                <mi:BoundField DataField="FILTER_2" HeaderText="筛选器2" />
                <mi:BoundField DataField="FILTER_3" HeaderText="筛选器3" />
                
                    <mi:NumColumn DataField="FIELD_SEQ" HeaderText="顺序" />
            </Columns>
        </mi:Table>
    </mi:Panel>
    <mi:WindowFooter runat="server" ID="footer1">
<%--        <mi:Button runat="server" ID="Button1" Text="上一步" Width="80" Height="26" Command="GoBack"
            Dock="Center" />--%>
        <mi:Button runat="server" ID="OkBtn" Text="完成" Width="80" Height="26" Command="GoLast"
            Dock="Center" />
        <mi:Button runat="server" ID="Button2" Text="取消" Width="80" Height="26" Dock="Right"
            OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>
</form>
<script>
    function SelectField(urlStr) {
        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '选择字段',
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
                action: 'InsertField',
                actionPs: e.ids
            });



        });
    }
    
</script>
