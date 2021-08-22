<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AreaStepEdit3.ascx.cs" Inherits="App.InfoGrid2.View.OnePage.AreaStepEdit3" %>
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
<mi:Store runat="server" ID="store2" Model="IG2_TABLE" IdField="IG2_TABLE_ID" >
    <SelectQuery>
        <mi:QueryStringParam Name="IG2_TABLE_ID" QueryStringField="view_Id" DbType="Int32" />
    </SelectQuery>
</mi:Store>
<mi:Store runat="server" ID="store1" Model="IG2_TABLE_COL" IdField="IG2_TABLE_COL_ID" SortField="FIELD_SEQ" PageSize="200" >
    <StringFields></StringFields>
    <FilterParams>
        <mi:QueryStringParam Name="IG2_TABLE_ID" QueryStringField="view_Id" DbType="Int32" />
        <mi:Param Name="SEC_LEVEL" DefaultValue="6" Logic="<=" />
        <mi:Param Name="ROW_SID" DefaultValue="-1" Logic=">" />
    </FilterParams>
    <UpdateParams>
        <mi:ServerParam Name="ROW_DATE_UPDATE" ServerField="TIME_NOW" />
    </UpdateParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport">
    <mi:Panel runat="server" ID="Panel2" Dock="Full" Region="North"  Scroll="None" Layout="Auto" Height="114">
        <mi:Toolbar runat="server" ID="tooblarX">
            <mi:ToolBarTitle Text="区域修改" />
            <mi:ToolBarButton Text="工具栏定义" Align="Right" Command="ToolbarSetup" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table2"  StoreID="store2" PagerVisible="false"  Dock="Full">
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


                <mi:BoundField DataField="STYLE_JSON_FIELD" HeaderText="样式 JSON 字段" />
                
                <mi:BoundField DataField="SORT_TEXT" HeaderText="默认排序" />
                

                <mi:SelectColumn DataField="INSERT_POS" HeaderText="插入记录位置" TriggerMode="None">
                    <mi:ListItem TextEx="默认" />
                    <mi:ListItem Value="LAST" Text="最后面" />
                    <mi:ListItem Value="FIRST" Text="最前面" />
                </mi:SelectColumn>

                <mi:CheckColumn DataField="VALID_MSG_ENABLED" HeaderText="验证显示" />
                <mi:SelectColumn DataField="LOCKED_FIELD" HeaderText="锁行字段" TriggerMode="None" >
                </mi:SelectColumn>

                <mi:BoundField DataField="LOCKED_RULE" HeaderText="锁行规则 JS" Width="400" />
                
                <mi:CheckColumn DataField="VISIBLE_BTN_EDIT" HeaderText="编辑按钮" Width="70"  />
                <mi:SelectColumn DataField="FORM_NEW_TYPE" HeaderText="新建表单类型" Width="100" TriggerMode="None" >
                    <mi:ListItem Value="" Text="--空--" />
                    <mi:ListItem Value="ONE_FORM" Text="表单" />
                </mi:SelectColumn>
                <mi:NumColumn DataField="FORM_NEW_PAGEID" HeaderText="新建页ID" />
                <mi:BoundField DataField="FORM_NEW_ALIAS_TITLE" HeaderText="新建表单别名" />

                <mi:SelectColumn DataField="FORM_EDIT_TYPE" HeaderText="编辑表单类型" Width="100" TriggerMode="None" >
                    <mi:ListItem Value="" Text="--空--" />
                    <mi:ListItem Value="ONE_FORM" Text="表单" />
                </mi:SelectColumn>
                <mi:NumColumn DataField="FORM_EDIT_PAGEID" HeaderText="编辑页ID" />
                <mi:BoundField DataField="FORM_EDIT_ALIAS_TITLE" HeaderText="编辑表单别名" />
            </Columns>
        </mi:Table>
    </mi:Panel>
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Layout="Auto" Scroll="None" >
        <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarButton Text="保存" OnClick="ser:store1.SaveAll()"  />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="true" StoreID="store1" Sortable="false"  Dock="Full" PagerVisible="false">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="DB_FIELD" HeaderText="内部列名" EditorMode="None" />
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
                <mi:NumColumn DataField="DB_LEN" HeaderText="字段长度" Width="80"  NotDisplayValue="0" />
                <mi:NumColumn DataField="DB_DOT" HeaderText="保留小数位" Width="80"  NotDisplayValue="0" />
                
                <mi:GroupColumn HeaderText="表单属性">
                    <mi:CheckColumn DataField="FORM_IS_NEWLINE" HeaderText="新行" />
                    <mi:BoundField DataField="FORM_PLACEHOLER" HeaderText="水印" />
                </mi:GroupColumn>

                <mi:SelectColumn DataField="SUMMARY_TYPE" HeaderText="汇总" TriggerMode="None">
                    <mi:ListItem Value="" Text="" TextEx="----空----" />
                    <mi:ListItem Value="SUM" Text="求和" />
                    <mi:ListItem Value="COUNT" Text="计数" />
                    <mi:ListItem Value="AVG" Text="平均值" />
                    <mi:ListItem Value="MIN" Text="最小值" />
                    <mi:ListItem Value="MAX" Text="最大值" />
                    <mi:ListItem Value="OTHER" Text="其它" />
                </mi:SelectColumn>

                
                <mi:BoundField DataField="SUMMARY_FILTER" HeaderText="汇总过滤 T-SQL 子句 Where" Width="200" />

                <mi:GroupColumn HeaderText="下拉框或弹出框">
                    <mi:SelectColumn DataField="ACT_MODE" HeaderText="选择模式" TriggerMode="None">
                        <mi:ListItem Value="NONE" Text="没有" />
                        <mi:ListItem Value="FIXED" Text="固定值模式" />
                        <mi:ListItem Value="TABLE" Text="工作表模式" />
                    </mi:SelectColumn>

                    <mi:TriggerColumn DataField="ACT_FIXED_ITEMS" HeaderText="固定选择项目" OnButtonClick="showTextDialog(this)" ButtonClass="mi-icon-more" TriggerMode="None" />
                    <mi:TriggerColumn DataField="ACT_TABLE_ITEMS" HeaderText="工作表项目" OnButtonClick="showTableDialog(this)" ButtonClass="mi-icon-more" TriggerMode="None" />
                </mi:GroupColumn>
                <mi:BoundField DataField="L_CODE" HeaderText="简单表达式" Width="300" />
                <mi:NumColumn DataField="FIELD_SEQ" HeaderText="顺序" />

                <mi:GroupColumn HeaderText="结构过滤">
                    <mi:BoundField DataField="FILTER_CATA_TABLE" HeaderText="结构过滤-表名" />
                    <mi:BoundField DataField="FILTER_CATA_FIELD" HeaderText="结构过滤-字段名" />
                    <mi:BoundField DataField="FILTER_CATA_IDFIELD" HeaderText="结构过滤-主键" />
                </mi:GroupColumn>

            </Columns>
        </mi:Table>

    </mi:Panel>



    <mi:WindowFooter runat="server" ID="Panel1" >
        <mi:Button runat="server" ID="OkBtn" Text="上一步" Width="80" Height="26" Command="GoPre" Dock="Center" />
        <mi:Button runat="server" ID="Button2" Text="完成" Width="80" Height="26" Command="GoLast" Dock="Center"  />
        <mi:Button runat="server" ID="Button1" Text="取消" Width="80" Height="26" Dock="Right" OnClick="ownerWindow.close()" />
    </mi:WindowFooter>

</mi:Viewport>


</form>


<script type="text/javascript">

    function showTableDialog(owner) {

        var tableId = $.query.get("view_id");

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

            urlStr = Mini2.urlAppend('/App/InfoGrid2/view/OneSearch/StepEdit2.aspx',{
                'owner_table_id': tableId,
                'owner_col_id': colId,
                'view_id': cs.view_id,
                'table_name':cs.table_name
            });
            
        }
        else {

            //owner_type_id : 当前表类型[TABLE | VIEW | PAGE]
            //owner_table_id : 当前表的 ID ,主键值 IG2_TABLE_ID
            //owner_col_id : 当前是由那个字段管理的. 字段名
            urlStr = Mini2.urlAppend("/App/InfoGrid2/view/OneSearch/StepNew1.aspx", {
                'owner_type_id': 'TABLE',
                'owner_table_id': tableId,
                'owner_col_id' : colId
            });
        }




        var win = Mini2.createTop('Mini2.ui.Window', {
            mode: true,
            text: '列表框架',
            iframe: true,
            width: 1000,
            height: 600,
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


            owner.setValue(json);

            //record.set('ACTION_TABLE_ITEMS', 'TABLE,' + e.sviewId);

        });


    }


    function showTextDialog(owner) {

        var win = Mini2.createTop('Mini2.ui.extend.TextWindow');

        win.show();

        win.formClosed(function (e) {
            if (e.result != 'ok') { return; }

            var txt = this.getValue();

            console.log(owner);
            owner.setValue(txt);
        });

        var ownerText = owner.getValue();
        win.setValue(ownerText);

    }


</script>