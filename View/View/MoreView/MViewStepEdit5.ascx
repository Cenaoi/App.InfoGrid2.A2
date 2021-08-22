<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MViewStepEdit5.ascx.cs" Inherits="App.InfoGrid2.View.MoreView.MViewStepEdit5" %>

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
<% } %>
<form action="" id="form1" method="post">

<mi:Store runat="server" ID="store1" Model="IG2_TABLE_COL" IdField="IG2_TABLE_COL_ID" SortField="FIELD_SEQ" PageSize="200" >
    <SelectQuery>
        <mi:QueryStringParam Name="IG2_TABLE_ID" QueryStringField="id" DbType="Int32" />
<%--        <mi:Param Name="SEC_LEVEL" DefaultValue="6" Logic="<" />--%>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </SelectQuery>
    <UpdateParams>
        <mi:ServerParam Name="ROW_DATE_UPDATE" ServerField="TIME_NOW" />
    </UpdateParams>
</mi:Store>


<mi:Viewport runat="server" ID="viewport">

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None">
        <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarButton Text="保存" OnClick="ser:store1.SaveAll()"  />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="上移" OnClick="ser:store1.MoveUp()" />
            <mi:ToolBarButton Text="下移" OnClick="ser:store1.MoveDown()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="应用到全部视图" OnBeforeClick="queryDelete(this)" Command="GoApplyToViewAll" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="true" StoreID="store1" Sortable="false" Dock="Full" PagerVisible="false" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="TABLE_NAME" HeaderText="表名" EditorMode="None" Width="80" />
                <mi:BoundField DataField="VIEW_FIELD_SRC" HeaderText="内部字段名" EditorMode="None" />
                <mi:BoundField DataField="F_NAME" HeaderText="字段名" EditorMode="None" />
                <mi:BoundField DataField="DISPLAY" HeaderText="显示名" />
                <mi:SelectColumn DataField="DB_TYPE" HeaderText="数据类型" EditorMode="None" TriggerMode="None"  >
                    <mi:ListItem Value="string" Text="字符串" TextEx="字符串（例：张三）" />
                    <mi:ListItem Value="int" Text="整数" TextEx="整数（例：100）" />
                    <mi:ListItem Value="datetime" Text="日期" TextEx="日期（例：2012-12-30）" />
                    <mi:ListItem Value="boolean" Text="布尔型" TextEx="布尔型（例：True,False）" />
                    <mi:ListItem Value="currency" Text="货币" TextEx="货币（例：￥1,000.00）" />
                    <mi:ListItem Value="decimal" Text="数值" TextEx="数值（例：211.05）"  />
                </mi:SelectColumn>
                <mi:CheckColumn DataField="IS_BIZ_MANDATORY" HeaderText="必填" ItemAlign="Center" Width="60" />
                <mi:CheckColumn DataField="IS_READONLY" HeaderText="只读" ItemAlign="Center" Width="60"  />
                <mi:CheckColumn DataField="IS_VISIBLE" HeaderText="显示" ItemAlign="Center" Width="60"  />
                <mi:CheckColumn DataField="IS_LIST_VISIBLE" HeaderText="列表显示" ItemAlign="Center" Width="60"  />
                <mi:CheckColumn DataField="IS_SEARCH_VISIBLE" HeaderText="查询显示" ItemAlign="Center" Width="60"  />
                <mi:BoundField DataField="DEFAULT_VALUE" HeaderText="默认值" />
                
                    <mi:NumColumn DataField="FIELD_SEQ" HeaderText="顺序" />
                <mi:ActionColumn AutoHide="true" >
                    <mi:ActionItem Text="应用到全部视图" Icon="/res/icon/01664.png" />
                </mi:ActionColumn>
            </Columns>
        </mi:Table>

    </mi:Panel>

    <mi:WindowFooter ID="WindowFooter1" runat="server">
        <mi:Button runat="server" ID="Button1" Width="80" Height="26" Command="GoLast" Text="完成" Dock="Center" />
        <mi:Button runat="server" ID="Button2" Width="80" Height="26" Command="GoCancel" Text="取消" Dock="Center" />
    </mi:WindowFooter>
</mi:Viewport>

</form>

<script type="text/javascript">

    function queryDelete(owner) {
        var me = owner;

        Mini2.Msg.prompt("询问", "将影响到整个系统的界面 , 确定应用到全部视图?", function () {
            me.click();
        });

        return false;
    }


</script>