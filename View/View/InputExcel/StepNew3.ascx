<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StepNew3.ascx.cs" Inherits="App.InfoGrid2.View.InputExcel.StepNew3" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" id="form2" method="post">


<mi:Store runat="server" ID="store1" Model="IG2_IMPORT_RULE" IdField="IG2_IMPORT_RULE_ID" >
    <FilterParams>
        <mi:QueryStringParam Name="IG2_IMPORT_RULE_ID" QueryStringField="id" DbType="Int32" />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
</mi:Store>
<mi:Store runat="server" ID="store2" Model="IG2_IMPORT_RULE_MAP" IdField="IG2_IMPORT_RULE_MAP_ID" PageSize="0">
    <FilterParams>
        <mi:QueryStringParam Name="IG2_IMPORT_RULE_ID" QueryStringField="id" DbType="Int32" />
    </FilterParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport">
    
    <mi:Panel runat="server" ID="topPanel" Dock="Top" Region="North" Scroll="None">
        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarButton Text="重新上传" Command="ShowInputFile" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table2"  StoreID="store1" Height="80" PagerVisible="false"  Dock="Full" ReadOnly="true">
            <Columns>
                <mi:BoundField DataField="TARGET_TABLE" HeaderText="目标表名" Width="200" />
                <mi:BoundField DataField="TARGET_TABLE_TEXT" HeaderText="描述"   />
                <mi:BoundField DataField="RULE_NAME" HeaderText="规则名"  />

                <mi:TriggerColumn DataField="SRC_FILE" HeaderText="Excel 文件"  Width="400" OnButtonClick="" ButtonClass="mi-icon-more"></mi:TriggerColumn>

            </Columns>
        </mi:Table>
    </mi:Panel>

   <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Layout="Auto" Scroll="None" >
        <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="true" StoreID="store2" Sortable="false"  Dock="Full" PagerVisible="false">
            <Columns>
                <mi:BoundField DataField="TARGET_FIELD" HeaderText="目标字段"  Width="200" EditorMode="None"  />
                <mi:BoundField DataField="TARGET_FIELD_TEXT" HeaderText="描述"  EditorMode="None"  />
                <mi:BoundField  DataField="EQUAL" EditorMode="None" Resizable="false" Width="60" ItemAlign="Center" />

                <mi:SelectColumn DataField="SRC_FIELD_INDEX" DisplayField="SRC_FIELD"  HeaderText="Excel字段" TriggerMode="None" NotDisplayValue="-1">
                      
                    <MapItems>
                        <mi:MapItem SrcField="text" TargetField="SRC_FIELD" />
                    </MapItems>
                </mi:SelectColumn>
                
                <mi:CheckColumn DataField="VALID_REQUIRED" HeaderText="必填" Width="60" />
                <mi:CheckColumn DataField="VALID_UNIQUE" HeaderText="唯一" Width="60" />

                <mi:CheckColumn DataField="VALID_AUTO_SKIP" HeaderText="相同跳过(配合唯一)" Width="100" />
                <mi:CheckColumn DataField="VALID_BLANK_SKIP" HeaderText="空白,跳过这一行" Width="120" />
                
                <mi:CheckColumn DataField="FOREIGN_ENABLED" HeaderText="激活外链接" Width="100" />
                <mi:BoundField DataField="FOREIGN_TABLE" HeaderText="表名" />
                <mi:BoundField  DataField="FOREIGN_RE_FIELD"  HeaderText="返回的字段名"  />
                
                <mi:SelectColumn DataField="DEFAULT_VALUE" HeaderText="默认值">
                    <mi:ListItem Value="(GetDate())" Text="当前时间" />
                    <mi:ListItem Value="(NewId())" Text="随机 Guid" />
                </mi:SelectColumn>

                <mi:BoundField   DataField="FOREIGN_FILTER" HeaderText="条件过滤" Width="500" />
                
            </Columns>
        </mi:Table>
    </mi:Panel>

    <mi:WindowFooter ID="WindowFooter1" runat="server">
        <mi:Button runat="server" ID="Button1" Width="80" Height="26" Command="GoLast" Text="开始导入" Dock="Center" />
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

    
    ///显示上传界面
    function ShowInputFile(url) {
        var frm = Mini2.create('Mini2.ui.Window', {
            url: url,
            mode: true

        });
        frm.show();

        frm.formClosed(function (e) {
            if (e.result != 'ok') { return; };
            var id = e.id;

            widget1.subMethod($('form:first'), { subName: 'store1', subMethod: 'Refresh' });

        });
     }

</script>
