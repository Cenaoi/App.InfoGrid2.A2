<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AreaStepEdit2.ascx.cs" Inherits="App.InfoGrid2.View.OnePage.AreaStepEdit2" %>
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
        <mi:QueryStringParam Name="IG2_TABLE_ID" QueryStringField="view_Id" DbType="Int32" />
    </SelectQuery>
</mi:Store>

<mi:Store runat="server" ID="store2" Model="IG2_TABLE_COL" IdField="IG2_TABLE_COL_ID" SortField="FIELD_SEQ" PageSize="0" >
    <SelectQuery>
        <mi:QueryStringParam Name="IG2_TABLE_ID" QueryStringField="view_Id" DbType="Int32" />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
        <mi:Param Name="SEC_LEVEL" DefaultValue="6" Logic="<=" />
    </SelectQuery>
</mi:Store>

<mi:Viewport runat="server" ID="viewport">
    


    <mi:Panel runat="server" ID="Panel2" Dock="Full" Region="North"  Scroll="None" Layout="Auto"  Height="114" >
        <mi:Toolbar runat="server" ID="tooblarX">
            <mi:ToolBarTitle Text="区域修改" />
            <mi:ToolBarButton Text="工具栏定义" Align="Right" Command="ToolbarSetup" />
            
            <mi:ToolBarButton Text="流程定义" Align="Right" Command="GoFlowSetup" />
        </mi:Toolbar>


        <mi:Table runat="server" ID="table1"  StoreID="store1" PagerVisible="false"  Dock="Full">
            <Columns>
                <mi:RowNumberer />
                <mi:BoundField DataField="TABLE_NAME" HeaderText="表内部名称" />
                <mi:BoundField DataField="DISPLAY" HeaderText="表名" Width="200" />
                <mi:CheckColumn DataField="SINGLE_SELECTION" HeaderText="单选" />
                <mi:SelectColumn DataField="UI_TYPE_ID" HeaderText="界面控件" TriggerMode="None">
                    <mi:ListItem Value="" Text="" TextEx="--没有--" />
                    <mi:ListItem Value="TABLE" Text="表格" />
                    <mi:ListItem Value="FORM" Text="表单" />
                    <mi:ListItem Value="SEARCH" Text="查询" />
                </mi:SelectColumn>

                <mi:CheckColumn DataField="SUMMARY_VISIBLE" HeaderText="汇总显示" />
                
                <mi:BoundField DataField="STYLE_JSON_FIELD" HeaderText="样式 JSON 字段" />

                <mi:BoundField DataField="SORT_TEXT" HeaderText="默认排序" />


                <mi:SelectColumn DataField="INSERT_POS" HeaderText="插入记录位置" TriggerMode="None">
                    <mi:ListItem TextEx="默认" />
                    <mi:ListItem Value="LAST" Text="最后面" />
                    <mi:ListItem Value="FIRST" Text="最前面" />
                    <mi:ListItem Value="FocusLast" Text="焦点行后面" />
                    <mi:ListItem Value="FocusFirst" Text="焦点行前面" />
                </mi:SelectColumn>

                <mi:CheckColumn DataField="VALID_MSG_ENABLED" HeaderText="验证显示" />
                <mi:SelectColumn DataField="LOCKED_FIELD" HeaderText="锁行字段" TriggerMode="None" >
                </mi:SelectColumn>
                
                
                <mi:BoundField DataField="LOCKED_RULE" HeaderText="锁行规则 JS" Width="400" />

                <mi:CheckColumn DataField="ATTACH_FILE_VISIBLE" HeaderText="附件显示" Width="60" />
                
            </Columns>
        </mi:Table>

    </mi:Panel>
    <mi:Panel runat="server" ID="buttonPanel" Dock="Full" Region="Center" Scroll="None" Layout="Auto">
        <mi:Toolbar runat="server" ID="toolbar1" Scroll="None">
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton OnClick="ser:store2.MoveUp()" Text="上移" />
            <mi:ToolBarButton OnClick="ser:store2.MoveDown()" Text="下移" />     
            <mi:ToolBarHr />       
            <mi:ToolBarButton Text="重建排序" OnClick="ser:store2.SortReset()" />
            <mi:ToolBarHr />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table2"  StoreID="store2"  Dock="Full" Region="Center" Sortable="false"  PagerVisible="false">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="DB_FIELD" HeaderText="数据字段" EditorMode="None" />
                <mi:BoundField DataField="F_NAME" HeaderText="字段名" EditorMode="None" />
                <mi:BoundField DataField="DISPLAY" HeaderText="显示名" />
                <mi:CheckColumn DataField="IS_VISIBLE" HeaderText="显示" ItemAlign="Center" Width="60" />
                <mi:CheckColumn DataField="IS_LIST_VISIBLE" HeaderText="表格显示" ItemAlign="Center" Width="60" />
                <mi:CheckColumn DataField="IS_SEARCH_VISIBLE" HeaderText="查询显示" ItemAlign="Center" Width="60" />
                <mi:CheckColumn DataField="IS_READONLY" HeaderText="只读" ItemAlign="Center" Width="60"  />
                <mi:BoundField DataField="DEFAULT_VALUE" HeaderText="默认值" />
                
                    <mi:NumColumn DataField="FIELD_SEQ" HeaderText="顺序" />
            </Columns>
        </mi:Table>

    </mi:Panel>


    <mi:WindowFooter runat="server" ID="Panel1" >
        <mi:Button runat="server" ID="OkBtn" Text="上一步" Width="80" Height="26" BeforeAskText="返回上一步将清除当前设置, 确定上一步?" Command="GoPre" Dock="Center" />
        <mi:Button runat="server" ID="Button3" Text="下一步" Width="80" Height="26" Command="GoNext" Dock="Center" />
        
        <mi:Button runat="server" ID="Button2" Text="完成" Width="80" Height="26" Command="GoLast" Dock="Center"  />
        <mi:Button runat="server" ID="Button1" Text="取消" Width="80" Height="26" Dock="Right" OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>


</form>

