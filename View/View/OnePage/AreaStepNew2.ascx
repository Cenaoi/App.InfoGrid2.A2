<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AreaStepNew2.ascx.cs" Inherits="App.InfoGrid2.View.OnePage.AreaStepNew2" %>
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


<mi:Store runat="server" ID="store1" Model="IG2_TMP_TABLE" IdField="IG2_TMP_TABLE_ID" PageSize="0" >
    <SelectQuery>
        <mi:QueryStringParam Name="TMP_GUID" QueryStringField="TMP_GUID" DbType="Guid" />
        <mi:ServerParam Name="TMP_SESSION_ID" ServerField="SESSION_ID" />
    </SelectQuery>
</mi:Store>

<mi:Store runat="server" ID="store2" Model="IG2_TMP_TABLECOL" IdField="IG2_TMP_TABLECOL_ID" SortField="FIELD_SEQ"  PageSize="0" >
    <SelectQuery>
        <mi:QueryStringParam Name="TMP_GUID" QueryStringField="TMP_GUID" DbType="Guid" />
        <mi:ServerParam Name="TMP_SESSION_ID" ServerField="SESSION_ID" />
        <mi:Param Name="SEC_LEVEL" DefaultValue="6" Logic="<=" />
    </SelectQuery>
</mi:Store>

<mi:Viewport runat="server" ID="viewport">

    <mi:Table runat="server" ID="table1"  StoreID="store1" Height="80" PagerVisible="false"  Dock="Top" Region="North"  >
        <Columns>
            <mi:RowNumberer />
            <mi:BoundField DataField="DISPLAY" HeaderText="表名" Width="200" />
            <mi:CheckColumn DataField="SINGLE_SELECTION" HeaderText="单选" />
            <mi:SelectColumn DataField="UI_TYPE_ID" HeaderText="界面控件" TriggerMode="None">
                <mi:ListItem Value="" Text="" TextEx="--没有--" />
                <mi:ListItem Value="TABLE" Text="表格" />
                <mi:ListItem Value="FORM" Text="表单" />
                <mi:ListItem Value="SEARCH" Text="查询" />
            </mi:SelectColumn>
            <mi:CheckColumn DataField="SUMMARY_VISIBLE" HeaderText="汇总显示" />
        </Columns>
    </mi:Table>
    
    <mi:Panel runat="server" ID="buttonPanel" Dock="Full" Region="Center" Scroll="Horizontal" Layout="Auto">
        <mi:Toolbar runat="server" ID="toolbar1" Scroll="None">
            <mi:ToolBarButton Text="刷新" OnClick="ser:store2.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton OnClick="ser:store2.MoveUp()" Text="上移" />
            <mi:ToolBarButton OnClick="ser:store2.MoveDown()" Text="下移" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table2"  StoreID="store2"  Dock="Full" Region="Center" PagerVisible="false">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="F_NAME" HeaderText="字段名" EditorMode="None" />
                <mi:BoundField DataField="DISPLAY" HeaderText="显示名" />
                <mi:CheckColumn DataField="IS_VISIBLE" HeaderText="显示" ItemAlign="Center" Width="60" />
                <mi:CheckColumn DataField="IS_READONLY" HeaderText="只读" ItemAlign="Center" Width="60"  />
                <mi:BoundField DataField="DEFAULT_VALUE" HeaderText="默认值" />
                
                    <mi:NumColumn DataField="FIELD_SEQ" HeaderText="顺序" />
            </Columns>
        </mi:Table>

    </mi:Panel>
    <mi:WindowFooter runat="server" ID="Panel1" >
        <mi:Button runat="server" ID="OkBtn" Text="上一步" Width="80" Height="26" Command="GoPre" Dock="Center" />
        <mi:Button runat="server" ID="Button3" Text="下一步" Width="80" Height="26" Command="GoNext" Dock="Center" />
        <mi:Button runat="server" ID="Button2" Text="完成" Width="80" Height="26" Command="GoLast" Dock="Center"  />
        <mi:Button runat="server" ID="Button1" Text="取消" Width="80" Height="26" Dock="Right" OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>


</form>
