<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StepEdit2.ascx.cs" Inherits="App.InfoGrid2.View.OneTable.StepEdit2" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<% if (false)
   { %>
<link href="/App_Themes/Blue/common.css" rel="stylesheet" type="text/css" />
<link href="/App_Themes/Vista/table.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>
<link href="/Core/Scripts/ui-lightness/jquery-ui-1.8.6.custom.css" rel="stylesheet"
    type="text/css" />
<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
<script src="/Core/Scripts/validate/jquery.validate-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>
<script src="/Core/Scripts/Mini/_CodeHelp.js" type="text/javascript"></script>

<link href="/Core/Scripts/Mini2/Themes/theme-globel.css" rel="stylesheet" type="text/css" />
<link href="/Core/Scripts/Mini2/Themes/theme-window.css" rel="stylesheet" type="text/css" />
<% } %>
<form method="post">

<mi:Store runat="server" ID="store1" PageSize="0"
    Model="IG2_TMP_TABLECOL" IdField="IG2_TMP_TABLECOL_ID" SortField="FIELD_SEQ" DeleteRecycle="true">
    <StringFields>
        IG2_TMP_TABLECOL_ID,
        F_NAME,
        DB_TYPE,
        DB_FIELD,
        IS_REMARK,
        IS_MANDATORY,
        SEC_LEVEL,
        DB_LEN,
        FIELD_SEQ,
        IS_VIEW_FIELD,
        VIEW_SQL
    </StringFields>
    <FilterParams>
        <mi:QueryStringParam Name="TMP_GUID" QueryStringField="tmp_id" DbType="Guid" />
        <%--<mi:Param Name="SEC_LEVEL" DefaultValue="6" Logic="<" />--%>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <DeleteQuery>
        <mi:QueryStringParam Name="TMP_GUID" QueryStringField="tmp_id" DbType="Guid" />
        <mi:ControlParam Name="IG2_TMP_TABLECOL_ID" ControlID="table1" PropertyName="CheckedRows" />
        <mi:Param Name="SEC_LEVEL" DefaultValue="6" Logic="<" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:Param Name="TMP_OP_ID" DefaultValue="D" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
    </DeleteRecycleParams>
    <UpdateParams>
        <mi:Param Name="TMP_OP_ID" DefaultValue="E" />
        <mi:ServerParam Name="ROW_DATE_UPDATE" ServerField="TIME_NOW" />
    </UpdateParams>
    <InsertParams>
        <mi:QueryStringParam Name="TMP_GUID" QueryStringField="tmp_id" DbType="Guid" />
        <mi:ServerParam Name="TMP_SESSION_ID" ServerField="SESSION_ID"  />
        <mi:Param Name="TMP_OP_ID" DefaultValue="A" />
        <mi:Param Name="DB_TYPE" DefaultValue="string" />
        <mi:Param Name="ANGLE" DefaultValue="Left" />
        <mi:Param Name="IS_VISIBLE" DefaultValue="True" />
        <mi:Param Name="IS_SEARCH_VISIBLE" DefaultValue="True" />
        <mi:Param Name="IS_LIST_VISIBLE" DefaultValue="True" />
        <mi:Param Name="FIELD_SEQ" DefaultValue="999" />
    </InsertParams>
</mi:Store>


<mi:Viewport runat="server" ID="viewport">

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
            <mi:Toolbar runat="server" ID="toolbar2">
                <mi:ToolBarButton Text="新增" OnClick="ser:store1.Insert()" />
                <mi:ToolBarButton Text="保存" OnClick="ser:store1.SaveAll()"  />
                <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="获取实际长度" Command="GoSyncDBLen" />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="删除" OnClick="ser:store1.Delete()" />
            </mi:Toolbar>
            <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="true" StoreID="store1" Dock="Full" 
                Sortable="false" PagerVisible="false" >
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField DataField="F_NAME" HeaderText="字段名" />
                    <mi:SelectColumn DataField="DB_TYPE" HeaderText="数据类型" TriggerMode="None" DropDownWidth="200">
                        <mi:ListItem Value="string" Text="字符串" TextEx="字符串（例：张三）" />
                        <mi:ListItem Value="int" Text="整数" TextEx="整数（例：100）" />
                        <mi:ListItem Value="long" Text="长整数" TextEx="整数（例：10000000000）" />
                        <mi:ListItem Value="datetime" Text="日期" TextEx="日期（例：2012-12-30）" />
                        <mi:ListItem Value="boolean" Text="布尔型" TextEx="布尔型（例：True,False）" />
                        <mi:ListItem Value="currency" Text="货币" TextEx="货币（例：￥1,000.00）" />
                        <mi:ListItem Value="decimal" Text="数值" TextEx="数值（例：211.05）"  />
                        <mi:ListItem Value="guid" Text="全局唯一标识符" TextEx="全局唯一标识符（例：{0D1FD903-09BE-48C3-A938-FE8F22D637A4}）"  />
                    </mi:SelectColumn>
                    <mi:BoundField DataField="DB_FIELD" HeaderText="系统字段名" Width="200" />

                    <mi:CheckColumn DataField="IS_MANDATORY" HeaderText="不允许空" Width="80" />
                    <mi:CheckColumn DataField="IS_REMARK" HeaderText="大数据" Width="80" />
                    <mi:SelectColumn DataField="SEC_LEVEL" HeaderText="权限等级" TriggerMode="None" >
                        <mi:ListItem Value="0" Text="0-用户自定义" />
                        <mi:ListItem Value="2" Text="2-普通" />
                        <mi:ListItem Value="4" Text="4-主键" />
                        <mi:ListItem Value="6" Text="6-业务系统" />
                        <mi:ListItem Value="8" Text="8-系统内部" />
                    </mi:SelectColumn>

                    <mi:CheckColumn DataField="IS_VIEW_FIELD" HeaderText="视图字段" Width="80" />

                    <mi:NumColumn DataField="DB_LEN" HeaderText="长度" />
                    <mi:NumColumn DataField="FIELD_SEQ" HeaderText="顺序" />

                    <mi:TriggerColumn DataField="VIEW_SQL" HeaderText="视图字段 SQL 语句" OnButtonClick="showTextDialog(this)" ButtonType="More" TriggerMode="None" Width="300" />

                </Columns>
            </mi:Table>
    </mi:Panel>

    <mi:WindowFooter ID="WindowFooter1" runat="server">
        <mi:Button runat="server" ID="Button1" Width="80" Height="26" Command="GoLast" Text="完成" Dock="Center" />
        <mi:Button runat="server" ID="Button2" Width="80" Height="26" Command="GoCancel" Text="取消" Dock="Center" />
    </mi:WindowFooter>

</mi:Viewport>

    <script>

        function showTextDialog(owner) {

            var win = Mini2.create('Mini2.ui.extend.TextWindow');

            win.show();

            win.formClosed(function (e) {
                if (e.result != 'ok') { return; }

                var txt = this.getValue();
                owner.setValue(txt);
            });

            var ownerText = owner.getValue();
            win.setValue(ownerText);

        }

    </script>

</form>

