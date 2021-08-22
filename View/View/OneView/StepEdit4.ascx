﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StepEdit4.ascx.cs" Inherits="App.InfoGrid2.View.OneView.StepEdit4" %>
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

<form action="" id="form2" method="post">

<mi:Store runat="server" ID="storeTab" Model="IG2_TABLE" IdField="IG2_TABLE_ID">
    <FilterParams>
        <mi:QueryStringParam Name="IG2_TABLE_ID" QueryStringField="id" DbType="Int32" />
    </FilterParams>
</mi:Store>

<mi:Store runat="server" ID="store1" Model="IG2_TABLE_COL" IdField="IG2_TABLE_COL_ID" SortField="FIELD_SEQ" >
    <StringFields></StringFields>
    <FilterParams>
        <mi:QueryStringParam Name="IG2_TABLE_ID" QueryStringField="id" DbType="Int32" />
<%--        <mi:Param Name="SEC_LEVEL" DefaultValue="6" Logic="<" />--%>
        <mi:Param Name="ROW_SID" DefaultValue="-1" Logic=">" />
    </FilterParams>
    <UpdateParams>
        <mi:ServerParam Name="ROW_DATE_UPDATE" ServerField="TIME_NOW" />
    </UpdateParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport">

    
    <mi:Panel runat="server" ID="topPanel" Dock="Top" Region="North" Scroll="None" Height="150">
        <mi:Toolbar runat="server" ID="tooblarX">
            <mi:ToolBarTitle Text="区域修改" />
            <mi:ToolBarButton Text="工具栏定义" Align="Right" Command="ToolbarSetup" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table2"  StoreID="storeTab"  PagerVisible="false"  Dock="Full">
            <Columns>
                <mi:RowNumberer />
                <mi:BoundField DataField="DISPLAY" HeaderText="表名" Width="200" />
                <mi:CheckColumn DataField="SINGLE_SELECTION" HeaderText="单选" />
<%--                <mi:SelectColumn DataField="UI_TYPE_ID" HeaderText="界面控件" TriggerMode="None">
                    <mi:ListItem Value="" Text="" TextEx="--没有--" />
                    <mi:ListItem Value="TABLE" Text="表格" />
                    <mi:ListItem Value="FORM" Text="表单" />
                    <mi:ListItem Value="SEARCH" Text="查询" />
                </mi:SelectColumn>--%>
                <mi:CheckColumn DataField="VISIBLE_BTN_EDIT" HeaderText="编辑按钮" Width="70"  />
                <mi:CheckColumn DataField="SUMMARY_VISIBLE" HeaderText="显示汇总" Width="70" />
                <mi:BoundField DataField="STYLE_JSON_FIELD" HeaderText="样式 JSON 字段"   />

                <mi:BoundField DataField="SORT_TEXT" HeaderText="默认排序"  Width="300"  />
                
                <mi:SelectColumn DataField="INSERT_POS" HeaderText="插入记录位置" TriggerMode="None">
                    <mi:ListItem TextEx="默认" />
                    <mi:ListItem Value="LAST" Text="最后面" />
                    <mi:ListItem Value="FIRST" Text="最前面" />
                </mi:SelectColumn>

                <mi:CheckColumn DataField="VALID_MSG_ENABLED" HeaderText="验证显示" />

                <mi:SelectColumn DataField="LOCKED_FIELD" HeaderText="锁行字段" TriggerMode="None" >
                </mi:SelectColumn>

                <mi:BoundField DataField="SERVER_CLASS" HeaderText="扩展类全称" Width="300" />

                <mi:CheckColumn DataField="SEARCH_PANEL_EXPAND" HeaderText="查询版面展开" />

                <mi:CheckColumn DataField="SEC_STRUCT_ENABLED" HeaderText="权限结构过滤" />
                <mi:CheckColumn DataField="BIZ_CATALOG_ENABLED" HeaderText="目录树" />
                <mi:BoundField DataField="BIZ_CATALOG_IDENTITY" HeaderText="目录树代码" />

                <mi:NumColumn DataField="DELETE_BY_BIZ_SID" HeaderText="大于 BIZ_SID 才可删" Width="180" ItemAlign="Center" />
                
                <mi:GroupColumn HeaderText="表单参数">
                    <mi:CheckColumn DataField="VISIBLE_BTN_EDIT" HeaderText="编辑按钮" Width="70"  />
                    <mi:BoundField DataField="BTN_EDIT_COMMAND" HeaderText="编辑按钮命令" Width="100"  />

                    <mi:SelectColumn DataField="FORM_NEW_TYPE" HeaderText="新建表单类型" Width="100" TriggerMode="None" >
                        <mi:ListItem Value="" Text="--空--" />
                        <mi:ListItem Value="ONE_FORM" Text="表单" />
                        <mi:ListItem Value="TABLE_FORM" Text="单表-表单" />
                    </mi:SelectColumn>
                    <mi:NumColumn DataField="FORM_NEW_PAGEID" HeaderText="新建页ID" />
                    <mi:BoundField DataField="FORM_NEW_ALIAS_TITLE" HeaderText="新建表单别名" />

                    <mi:SelectColumn DataField="FORM_EDIT_TYPE" HeaderText="编辑表单类型" Width="100" TriggerMode="None" >
                        <mi:ListItem Value="" Text="--空--" />
                        <mi:ListItem Value="ONE_FORM" Text="表单" />
                        <mi:ListItem Value="TABLE_FORM" Text="单表-表单" />
                    </mi:SelectColumn>
                    <mi:NumColumn DataField="FORM_EDIT_PAGEID" HeaderText="编辑页ID" />
                    <mi:BoundField DataField="FORM_EDIT_ALIAS_TITLE" HeaderText="编辑表单别名" />
                    <mi:CheckColumn DataField="ATTACH_FILE_VISIBLE" HeaderText="附件上传" Width="100" />
                </mi:GroupColumn>
                
            </Columns>
        </mi:Table>
    </mi:Panel>


    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Layout="Auto" Scroll="None" >
        <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarButton Text="保存" OnClick="ser:store1.SaveAll()"  />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="true" StoreID="store1" Sortable="false"  Dock="Full">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="DB_FIELD" HeaderText="内部字段名" EditorMode="None" />
                <mi:BoundField DataField="F_NAME" HeaderText="字段名" EditorMode="None" />
                <mi:BoundField DataField="DISPLAY" HeaderText="显示名" />

                <mi:SelectColumn DataField="DISPLAY_TYPE" HeaderText="显示型式" TriggerMode="None" Width="80">
                    <mi:ListItem Value="AUTO" Text="自动" />
                    <mi:ListItem Value="Textarea" Text="多行文本框" />

                     <mi:ListItem Value="html_edit" Text="富文本框" />
                     <mi:ListItem Value="file_upload" Text="文件上传" />
                     <mi:ListItem Value="more_file_upload" Text="多文件上传" />
                     <mi:ListItem Value="more_image_upload" Text="多图片文件上传" />
                </mi:SelectColumn>
                <mi:NumColumn DataField="DISPLAY_LEN" HeaderText="显示长度" Width="80" Resizable="false" NotDisplayValue="0" />
                
                <mi:NumColumn DataField="DISPLAY_HEIGHT" HeaderText="显示高度" Width="80" Resizable="false" NotDisplayValue="0" />

                <mi:BoundField DataField="FORMAT" HeaderText="显示格式" />
                <mi:BoundField DataField="GROUPS" HeaderText="分组" />
<%--                <mi:BoundField DataField="V_SEARCH_MODE_ID" HeaderText="查询显示格式" />
                <mi:BoundField DataField="V_EDIT_MODE_ID" HeaderText="编辑显示格式" />
                <mi:BoundField DataField="V_LIST_MODE_ID" HeaderText="列表显示格式" />--%>
                <mi:SelectColumn DataField="V_LIST_MODE_ID" HeaderText="显示型式" TriggerMode="None" Width="80">
                    <mi:ListItem Value="DEFAULT" Text="默认" />
                    <mi:ListItem Value="BoundField" Text="文本框" />
                    <mi:ListItem Value="Textarea" Text="多行文本框" />
                    <mi:ListItem Value="SelectColumn" Text="下拉框" />
                    <mi:ListItem Value="TriggerColumn" Text="弹出框" />
                </mi:SelectColumn>
                <mi:SelectColumn DataField="V_TRIGGER_MODE" HeaderText="填写模式" TriggerMode="None" >
                    <mi:ListItem Value="NONE" Text="禁止填写" />
                    <mi:ListItem Value="USERINPUT" Text="可填写" />
                </mi:SelectColumn>
                <mi:BoundField DataField="TOOLTIP" HeaderText="帮助提示" />
                <mi:SelectColumn DataField="ANGLE" HeaderText="对齐方式" TriggerMode="None">
                    <mi:ListItem  Value="Center" Text="居中"/>
                    <mi:ListItem  Value="Left" Text="左对齐"/>
                    <mi:ListItem  Value="Right" Text="右对齐"/>
                </mi:SelectColumn>

                <mi:NumColumn DataField="COL_LEN" HeaderText="字段长度" Width="80"  NotDisplayValue="0" />
                <mi:NumColumn DataField="COL_DOT" HeaderText="保留小数位" Width="80"  NotDisplayValue="0" />
                
                <mi:GroupColumn HeaderText="表单属性">
                    <mi:CheckColumn DataField="FORM_IS_NEWLINE" HeaderText="新行" />
                    <mi:BoundField DataField="FORM_PLACEHOLER" HeaderText="水印" />
                </mi:GroupColumn>

                <mi:SelectColumn DataField="ACT_MODE" HeaderText="选择模式" TriggerMode="None">
                    <mi:ListItem Value="NONE" Text="没有" />
                    <mi:ListItem Value="FIXED" Text="固定值模式" />
                    <mi:ListItem Value="TABLE" Text="工作表模式" />
                </mi:SelectColumn>

                <mi:TriggerColumn DataField="ACT_FIXED_ITEMS" HeaderText="固定选择项目" OnButtonClick="showTextDialog(this)" ButtonType="More" TriggerMode="None" />
                <mi:TriggerColumn DataField="ACT_TABLE_ITEMS" HeaderText="工作表项目" OnButtonClick="showTableDialog(this)" ButtonType="More" TriggerMode="UserInput" />

                
                <mi:BoundField DataField="L_CODE" HeaderText="简单表达式" Width="300" />
                
                <mi:BoundField DataField="VIEW_FIELD_CONFIG" HeaderText="视图字段配置参数" Width="600" />

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

    function showTableDialog(owner) {

        var tableId = $.query.get("id");

        var record = owner.record;

        var colId = record.get('DB_FIELD');
        var aTableItems = record.get('ACT_TABLE_ITEMS');

        var urlStr;

        if (aTableItems && aTableItems != '') {
            var srcParams = eval("(" + aTableItems + ")");

            var cs = Mini2.apply({
                view_id: '',
                type_id: 'VIEW'
            }, srcParams);

            urlStr = $.format('/App/InfoGrid2/view/OneSearch/StepEdit2.aspx?owner_table_id={0}&owner_col_id={1}&view_id={2}&table_name={3}', tableId, colId, cs.view_id, cs.table_name);
        }
        else {

            //owner_type_id : 当前表类型[TABLE | VIEW | PAGE]
            //owner_table_id : 当前表的 ID ,主键值 IG2_TABLE_ID
            //owner_col_id : 当前是由那个字段管理的. 字段名
            urlStr = $.format("/App/InfoGrid2/view/OneSearch/StepNew1.aspx?owner_type_id={0}&owner_table_id={1}&owner_col_id={2}",
                "TABLE", tableId, colId);

        }




        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '列表框架',
            iframe: true,
            width: 800,
            height: 600,
            startPosition: 'center_screen',
            url: urlStr
        });

        win.show();

        win.formClosed(function (e) {
            if (e.result != 'ok') { return; }

            var cs = {
                type_id: 'VIEW',
                view_id: e.view_id,
                table_name: e.table_name,
                owner_table_id: tableId
            };

            var json = Mini2.Json.toJson(cs);

            //            alert(json);

            owner.setValue(json);

            //record.set('ACTION_TABLE_ITEMS', 'TABLE,' + e.sviewId);

        });


    }


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