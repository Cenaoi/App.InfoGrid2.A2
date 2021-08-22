<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StepEdit5_DialogMode.ascx.cs" Inherits="App.InfoGrid2.View.OneView.StepEdit5_DialogMode" %>
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


<mi:Store runat="server" ID="store1" Model="IG2_TABLE" IdField="IG2_TABLE_ID" >
    <SelectQuery>
        <mi:QueryStringParam Name="IG2_TABLE_ID" QueryStringField="view_id" DbType="Int32" />
    </SelectQuery>
</mi:Store>
<mi:Store runat="server" ID="store2" Model="IG2_TABLE_COL" IdField="IG2_TABLE_COL_ID" >
    <SelectQuery>
        <mi:QueryStringParam Name="IG2_TABLE_ID" QueryStringField="view_id" DbType="Int32" />
        <mi:Param Name="SEC_LEVEL" DefaultValue="6" Logic="<=" />
        <mi:Param Name="ROW_SID" DefaultValue="-1" Logic=">" />
    </SelectQuery>
</mi:Store>

<mi:Viewport runat="server" ID="viewport">
    
    <mi:Panel runat="server" ID="topPanel" Dock="Top" Scroll="None" Height="110" Region="North">
        <mi:Toolbar runat="server" ID="toolbar2" Scroll="None">
            <mi:ToolBarTitle Text="模式窗体" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1"  StoreID="store1" Height="80" PagerVisible="false"  Dock="Top" Region="North" Sortable="false"  >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="DISPLAY" HeaderText="表名" />
                <mi:CheckColumn DataField="SINGLE_SELECTION" HeaderText="单选" />
                <mi:SelectColumn DataField="ENUM_VALUE_FIELD" HeaderText="下拉列表-值字段名" TriggerMode="None" >
                </mi:SelectColumn>
                <mi:SelectColumn DataField="ENUM_TEXT_FIELD" HeaderText="下拉列表-文本字段名"  TriggerMode="None">
                </mi:SelectColumn>
            </Columns>
        </mi:Table>
    </mi:Panel>

    
    <mi:Panel runat="server" ID="buttonPanel" Dock="Full" Region="Center" Scroll="None" Layout="Auto" >
        <mi:Toolbar runat="server" ID="toolbar1" Scroll="None">
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table2"  StoreID="store2"  Dock="Full" Region="Center" Sortable="false">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="DB_FIELD" HeaderText="字段名" />
                <mi:BoundField DataField="F_NAME" HeaderText="字段名" />
                <mi:CheckColumn DataField="IS_VISIBLE" HeaderText="显示" Width="60" />
                <mi:CheckColumn DataField="IS_SEARCH_VISIBLE" HeaderText="查询显示" Width="60" />
                <mi:CheckColumn DataField="IS_LIST_VISIBLE" HeaderText="列显示" Width="60" />

                <mi:SelectColumn DataField="FILTER_LOGIC" HeaderText="筛选逻辑" TriggerMode="None">
                    <mi:ListItem Value="" Text="" TextEx="-- N/A --" />
                    <mi:ListItem Value="==" Text="等于" />
                    <mi:ListItem Value=">" Text="大于" />
                    <mi:ListItem Value="<" Text="小于" />
                    <mi:ListItem Value="!=" Text="不等于" />
                    <mi:ListItem Value="&lt;=" Text="小于或等于" />
                    <mi:ListItem Value="&gt;=" Text="大于或等于" />
                    <mi:ListItem Value="In" Text="In" />
                    <mi:ListItem Value="NotIn" Text="NotIn" />
                    <mi:ListItem Value="Like" Text="Like" />
                    <mi:ListItem Value="NotLike" Text="NotLike" />
                </mi:SelectColumn>

                <mi:BoundField DataField="FILTER_VALUE" HeaderText="筛选值"/>
                <mi:SelectColumn DataField="EVENT_AFTER_FIELD_ID" HeaderText="事件字段名"  >
                    
                </mi:SelectColumn>
                <mi:BoundField DataField="EVENT_AFTER_FIELD_DESC" HeaderText="事件字段描述" />

            </Columns>
        </mi:Table>

    </mi:Panel>
    <mi:WindowFooter runat="server" ID="Panel1" >
        <mi:Button runat="server" Visible="false" ID="OkBtn" Text="上一步" Width="80" Height="26" Command="GoPre" Dock="Center" BeforeAskText="返回上一步将清除当前设置, 确定上一步?" />
        <mi:Button runat="server" ID="Button2" Text="完成" Width="80" Height="26" Command="GoLast" Dock="Center" OnClick="ownerWindow.close()"  />
        <mi:Button runat="server" ID="Button1" Text="取消" Width="80" Height="26" Dock="Right" OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>


</form>

