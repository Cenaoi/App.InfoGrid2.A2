<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActList.ascx.cs" Inherits="App.InfoGrid2.View.OneAction.ActList" %>
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

<mi:Store runat="server" ID="store1" Model="IG2_ACTION" IdField="IG2_ACTION_ID"  DeleteRecycle="true" >
    <StringFields></StringFields>
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="IG2_ACTION_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
    <InsertParams>
        <mi:Param Name="LINK_TYPE_ID" DefaultValue="=" />
        <mi:Param Name="LISTEN_AO" DefaultValue="OR" />
    </InsertParams>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" /> 
    </DeleteRecycleParams>
</mi:Store>


<mi:Viewport runat="server" ID="viewport">
    

    <mi:SearchFormLayout runat="server" ID="filterForm2" StoreID="store1"  Height="26">

        <mi:ComboBox runat="server" ID="lActCode" TriggerMode="None" DataField="L_ACT_CODE" FieldLabel="左-动作"  >
            <mi:ListItem Value="" Text="" TextEx="--N/A--" />
            <mi:ListItem Value="DELETE" Text="删除" />
            <mi:ListItem Value="INSERT" Text="新建" />
            <mi:ListItem Value="UPDATE" Text="更新" />
        </mi:ComboBox>
        <mi:TextBox runat="server" ID="lTable" DataField="L_TABLE" DataLogic="like" FieldLabel="左-表名" />

        <mi:ComboBox runat="server" ID="ComboBox1" TriggerMode="None" DataField="R_ACT_CODE" FieldLabel="右-动作" >
            <mi:ListItem Value="" Text="" TextEx="--N/A--" />
            <mi:ListItem Value="ALL" Text="全部" />
            <mi:ListItem Value="DELETE" Text="删除" />
            <mi:ListItem Value="INSERT" Text="新建" />
            <mi:ListItem Value="UPDATE" Text="更新" />
        </mi:ComboBox>
        <mi:TextBox runat="server" ID="TextBox1" DataField="R_TABLE" DataLogic="like" FieldLabel="右-表名" />

        <mi:SearchButtonGroup runat="server" ID="searchBtns"></mi:SearchButtonGroup>
    </mi:SearchFormLayout>
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarButton Text="新增" OnClick="ser:store1.Insert()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />            
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="保存" OnClick="ser:store1.SaveAll()" />   
                   
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="确定删除联动规则?" OnClick="ser:store1.Delete()" />
            <mi:ToolBarButton Text="复制" Command="CopyData" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="应用到系统" Command="GoApply" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" RowLines="true" StoreID="store1" Dock="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                
                <mi:BoundField DataField="IG2_ACTION_ID" HeaderText="ID" Width="50" EditorMode="None" />
                
                <mi:GroupColumn HeaderText="左表 (事件源)">
                    <mi:SelectColumn DataField="L_ACT_CODE" HeaderText="动作-L" TriggerMode="None" Width="60">
                        <mi:ListItem Value="DELETE" Text="删除" />
                        <mi:ListItem Value="INSERT" Text="新建" />
                        <mi:ListItem Value="UPDATE" Text="更新" />
                    </mi:SelectColumn>

                    <mi:BoundField DataField="L_TABLE" HeaderText="内部表名" Width="60"  EditorMode="None" />
                    <mi:BoundField DataField="L_TABLE_TEXT" HeaderText="表名" Width="200" />
                </mi:GroupColumn>
                <mi:BoundField DataField="LINK_TYPE_ID" HeaderText="" ItemAlign="Center" Width="60" Resizable="false" EditorMode="None" />
                
                
                <mi:GroupColumn HeaderText="右表 (联动执行)">
                    <mi:SelectColumn DataField="R_ACT_CODE" HeaderText="动作-R" TriggerMode="None" Width="60">
                        <mi:ListItem Value="ALL" Text="全部" />
                        <mi:ListItem Value="DELETE" Text="删除" />
                        <mi:ListItem Value="INSERT" Text="新建" />
                        <mi:ListItem Value="UPDATE" Text="更新" />
                    </mi:SelectColumn>
                    <mi:BoundField DataField="R_TABLE" HeaderText="内部表名" Width="60"  EditorMode="None" />
                    <mi:BoundField DataField="R_TABLE_TEXT" HeaderText="表名"  Width="200"  />
                </mi:GroupColumn>
                <mi:NumColumn DataField="EXEC_SEQ" HeaderText="排序" Width="60" NotDisplayValue="0" />

                <mi:BoundField DataField="REMARK" HeaderText="备注" Width="300"  />
                
                <mi:CheckColumn DataField="ENABLED" HeaderText="生效" />

                <mi:DateTimeColumn DataField="ROW_DATE_CREATE" HeaderText="创建时间" EditorMode="None"  />
                

                <mi:ActionColumn AutoHide="true">
                    <%--<mi:ActionItem Handler="TablePreview" Tooltip="预览" Icon="/res/icon/application_view_columns.png" />--%>
                    <mi:ActionItem Handler="TablePreview" Tooltip="修改" Icon="/res/icon/page_white_edit.png" />
                    <mi:ActionItem Handler="RTableShow" Tooltip="显示数结构" Icon="/res/icon/lorry.png" />
                </mi:ActionColumn>

            </Columns>
        </mi:Table>
    </mi:Panel>


    <mi:WindowFooter ID="WindowFooter1" runat="server" Visible="false">
<%--        <mi:Button runat="server" ID="SubmitBtn" Width="80" Height="26" Command="GoNext" Text="下一步" Dock="Center" />
--%>
        <mi:Button runat="server" ID="Button1" Width="80" Height="26" Command="GoLast" Text="完成" Dock="Center" />
    </mi:WindowFooter>


</mi:Viewport>


</form>

<script >


    function TablePreview(view, cell, recordIndex, cellIndex, e, record, row) {

        var tableId = record.getId();



        EcView.show("ActionStepEdit2.aspx?id=" + tableId, "预览-联动设置");

    }

    function RTableShow(view, cell, recordIndex, cellIndex, e, record, row) {

        var tableId = record.getId();

        var rTable = record.get('R_TABLE');

        EcView.show('ActionTreeList.aspx?table_right=' + rTable);

    }

</script>