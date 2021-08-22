<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TableList.ascx.cs" Inherits="App.InfoGrid2.View.Manager.TableList"  %>
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

<link href="/Core/Scripts/Mini2/ex_plugins/webui-popover/jquery.webui-popover.css" rel="stylesheet" />
<script src="/Core/Scripts/Mini2/ex_plugins/webui-popover/jquery.webui-popover.js"></script>

<form method="post">

<mi:Store runat="server" ID="store1" Model="IG2_TABLE" IdField="IG2_TABLE_ID" AutoSave="true" DeleteRecycle="true" AuotBind="true"  >
    <StringFields>IG2_TABLE_ID, TABLE_TYPE_ID, DISPLAY,IS_BIG_TITLE_VISIBLE,ROW_DATE_CREATE</StringFields>
    <FilterParams>
        <mi:QueryStringParam Name="IG2_CATALOG_ID" QueryStringField="catalog_id" />
        <mi:QueryStringParam Name="TABLE_TYPE_ID" QueryStringField="tableTypeId" />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="IG2_TABLE_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" /> 
    </DeleteRecycleParams>
</mi:Store>


<mi:Viewport runat="server" ID="viewport">


    <mi:SearchFormLayout runat="server" ID="searchForm1" StoreID="store1" Height="30" ItemWidth="240">
        <mi:TextBox runat="server" ID="idTb" DataField="IG2_TABLE_ID" FieldLabel="ID" ClearText="true" />
        <mi:TextBox runat="server" ID="TextBox1" DataField="TABLE_NAME" FieldLabel="数据表名"  DataLogic="like" />
        <mi:TextBox runat="server" ID="TextBox2" DataField="DISPLAY" FieldLabel="表名" DataLogic="like" />

        <mi:SearchButtonGroup runat="server" ID="btnGroup1"></mi:SearchButtonGroup>
    </mi:SearchFormLayout>

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarButton Text="新增" Command="GoNewTable" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            
            <mi:ToolBarButton Text="移动" Command="GoTableMove" />
            <%--<mi:ToolBarButton Text="移动" Command="GoTableMove" OnClick="TableMove_Click()" />--%>
            
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="验证工作表结构" Command="GoValidStruce" />
            <mi:ToolBarButton Text="验证关联表结构" Command="GoValidViewStruce" />
            
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?" OnClick="ser:store1.Delete()" />

            <mi:ToolBarButton Text="创建"  Command="CreateTable" />

            

            <mi:ToolBarButton Text="批量增删字段" Command="GoOpTableBatch"  />

            <mi:ToolBarButton Text="测试联动v3" Command="Test_Ac3" />
            
<%--            <mi:ToolBarButton Text="删除"  Command="CreateTable222" />

            <mi:ToolBarHr />
            <mi:ToolBarTemplate ID="toobarTemp1" >
                
            </mi:ToolBarTemplate>

            <mi:ToolBarButton Text="气泡" OnClick="AAAAAA(this)" />
            <mi:ToolBarButton Text="关闭 - 气泡" OnClick="AAAAAA2(this)" />--%>
                 

        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" RowLines="true" StoreID="store1" Dock="Full" ColumnLines="false" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:ActionColumn  AutoHide="true" HeaderText="功能">
                    <mi:ActionItem Handler="TablePreview"  Tooltip="预览" Icon="/res/icon/application_view_columns.png" />
                    <mi:ActionItem Handler="TableEdit" Tooltip="修改" Icon="/res/icon/page_white_edit.png" />
                    <mi:ActionItem Handler="TableCopy" Tooltip="拷贝" Icon="/res/icon_sys/copy.png" Text="拷贝" />
                    <mi:ActionItem Handler="TableStruct" Tooltip="拷贝" Icon="/res/icon_sys/Setup_3.png" Text="结构" />
                </mi:ActionColumn>                
                <mi:BoundField DataField="IG2_TABLE_ID" HeaderText="#" Width="60" ItemAlign="Center" EditorMode="None" />
                <mi:BoundField DataField="TABLE_NAME" HeaderText="数据表" Width="160" EditorMode="None" />
                <mi:BoundField DataField="DISPLAY" HeaderText="表名" Width="300"  />
                <mi:CheckColumn DataField="IS_BIG_TITLE_VISIBLE" HeaderText="显示大标题" Width="100"  />
                <mi:BoundField DataField="REMARK" HeaderText="备注" Width="300"  />
                <mi:DateColumn DataField="ROW_DATE_CREATE" HeaderText="创建时间" Format="Y-m-d H:i" Width="140"  EditorMode="None"/>
                
            </Columns>
        </mi:Table>
    </mi:Panel>

</mi:Viewport>


</form>
<script >



    //预览
    function TablePreview(view, cell, recordIndex, cellIndex, e, record, row) {

        var tableId = record.getId();

        var typeId = record.get('TABLE_TYPE_ID');

        var text = record.get('DISPLAY');

        var tSubTypeID = record.get('TABLE_SUB_TYPE_ID');

        if ('PAGE' == typeId) {
            if ('ONE_FORM' == tSubTypeID) {
                EcView.show("/App/InfoGrid2/View/OneForm/FormPreview.aspx?pageId=" + tableId, "预览-表单");
            }
            else {
                EcView.show("/App/InfoGrid2/View/OneBuilder/PreviewPage.aspx?pageId=" + tableId, "预览-复杂表");
            }
        }
        else if ('VIEW' == typeId) {

            if (tSubTypeID == '') {
                EcView.show("/App/InfoGrid2/View/OneSearch/SelectPreview.aspx?viewId=" + tableId, text + "-查询弹出表");
            }
            else {
                EcView.show("/App/InfoGrid2/View/OneView/ViewPreview.aspx?id=" + tableId, text + "-视图表");
            }
        }
        else if ('MORE_VIEW' == typeId) {
            EcView.show("/App/InfoGrid2/View/MoreView/MoreViewPreview.aspx?id=" + tableId, text + "-关联表");
        }
        else if ('CROSS_TABLE' == typeId) {
            EcView.show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id=" + tableId, text + "-交叉报表");
            
        }
        else {
            EcView.show("/App/InfoGrid2/View/OneTable/TablePreview.aspx?id=" + tableId, text + "-工作表");
        }
    }


    function TableStruct(view, cell, recordIndex, cellIndex, e, record, row) {

        var tableId = record.getId();

        var typeId = record.get('TABLE_TYPE_ID');

        var text = record.get('DISPLAY');

        var tSubTypeID = record.get('TABLE_SUB_TYPE_ID');

        if ('PAGE' == typeId) {
            //EcView.show("/App/InfoGrid2/View/OneBuilder/PreviewPage.aspx?pageId=" + tableId, text + "-复杂表");
        }
        else if ('VIEW' == typeId) {

            //if (tSubTypeID == '') {
            //    EcView.show("/App/InfoGrid2/View/OneSearch/SelectPreview.aspx?viewId=" + tableId, text + "-查询弹出表");
            //}
            //else {
            //    EcView.show("/App/InfoGrid2/View/OneView/ViewPreview.aspx?id=" + tableId, text + "-视图表");
            //}
        }
        else if ('MORE_VIEW' == typeId) {
            //EcView.show("/App/InfoGrid2/View/MoreView/MoreViewPreview.aspx?id=" + tableId, text + "-关联表");
        }
        else if ('CROSS_TABLE' == typeId) {
            alert("未实现!");
        }
        else {
            EcView.show("/App/InfoGrid2/View/OneTable/TableAndStruce.aspx?id=" + tableId, text + "-工作表-结构权限");
        }
    }

    //修改
    function TableEdit(view, cell, recordIndex, cellIndex, e, record, row) {

        var tableId = record.getId();

        var typeId = record.get('TABLE_TYPE_ID');
        var tSubTypeID = record.get('TABLE_SUB_TYPE_ID');

        if ('PAGE' == typeId) {
            if ('ONE_FORM' == tSubTypeID) {
                EcView.show("/App/InfoGrid2/View/OneBuilder/PageBuilder.aspx?id=" + tableId, "表单-设计器");
            }
            else {
                EcView.show("/App/InfoGrid2/View/OneBuilder/PageBuilder.aspx?id=" + tableId, "复杂表-设计器");
            }
        }
        else if ('VIEW' == typeId) {
            if (tSubTypeID == '') {
                var ownerID = record.get('VIEW_OWNER_TABLE_ID');
                EcView.show("/App/InfoGrid2/View/OneSearch/StepEdit2.aspx?view_id=" + tableId + "&owner_table_id="+ownerID,  "查询弹出表");
            }
            else {
                EcView.show("/App/InfoGrid2/View/OneView/ViewPreview.aspx?id=" + tableId, "预览-视图表");
            }
        }
        else if ('MORE_VIEW' == typeId) {
            EcView.show("/App/InfoGrid2/View/MoreView/MViewStepInitData.aspx?id=" + tableId, "设置-关联表");
        }
        else if ('CROSS_TABLE' == typeId) {
            EcView.show("/App/InfoGrid2/View/ReportBuilder/ReportCreate.aspx?id=" + tableId, "修改-交叉报表");
            
        }
        else {
            EcView.show("/App/InfoGrid2/View/OneTable/TablePreview.aspx?id=" + tableId, "预览-工作表");
        }
    }

    function TableCopy(view, cell, recordIndex, cellIndex, e, record, row) {

        var tableId = record.getId();

        var typeId = record.get('TABLE_TYPE_ID');
        var tSubTypeID = record.get('TABLE_SUB_TYPE_ID');

        if ('PAGE' == typeId) {
            //EcView.show("/App/InfoGrid2/View/OneBuilder/PreviewPage.aspx?pageId=" + tableId, "预览-复杂表");
            Mini2.toast("未实现!");
        }
        else if ('VIEW' == typeId) {
            //EcView.show("/App/InfoGrid2/View/OneView/ViewPreview.aspx?id=" + tableId, "预览-视图表");
            //Mini2.toast("未实现!");

            EcView.show("/App/InfoGrid2/View/OneView/OneViewCopy.aspx?src_view_id=" + tableId, "拷贝-视图表");
        }
        else if ('MORE_VIEW' == typeId) {
            EcView.show("/App/InfoGrid2/View/MoreView/StepCopy1.aspx?id=" + tableId, "设置-关联表");
            
        }
        else if ('CROSS_TABLE' == typeId) {
            Mini2.toast("未实现!");
        }
        else {
            EcView.show("/App/InfoGrid2/View/OneTable/StepCopy1.aspx?id=" + tableId, "预览-工作表");
        }
    }


    function AAAAAA(sender) {

        $(sender.el).webuiPopover({
            title: '',
            content: '此功能无效，即将爆炸...',
            multi: false,
            closeable:'yes'
        });
    }

    function AAAAAA2(sender) {
        $(sender.el).webuiPopover({
            title: '',
            content: '呵呵哈哈哈...',
            multi: false,
            closeable: 'yes'
        });
    }


</script>

