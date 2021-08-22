<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditTableCol.ascx.cs" Inherits="App.InfoGrid2.View.ReportBuilder.EditTableCol" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form method="post">
    <!--UT_101	吸塑指令单-单头-->
    <mi:Store runat="server" ID="StoreTableCol" Model="IG2_TMP_TABLECOL" IdField="IG2_TMP_TABLECOL_ID" PageSize="0" >
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:QueryStringParam Name="TMP_GUID" QueryStringField="id" DbType="Guid" />
        </FilterParams>
    </mi:Store>

   <mi:Viewport runat="server" ID="viewport1" Padding="4" ItemMarginRight="6">

        <mi:Table runat="server" ID="UT_101leftTable" Dock="Full" Region="Center" StoreID="StoreTableCol"  PagerVisible="false" >
            <Columns>
                <mi:RowNumberer />
                <mi:BoundField HeaderText="显示名" DataField="DISPLAY"   />
                <mi:BoundField HeaderText="字段名" DataField="DB_FIELD"   />
    <%--                <mi:BoundField HeaderText="过滤模式" DataField="FILTER_MODE"   />
                <mi:BoundField HeaderText="过滤逻辑" DataField="FILTER_LOGIC"   />
                <mi:BoundField HeaderText="过滤值" DataField="FILTER_VALUE" />--%>
                <mi:BoundField HeaderText="自定义SQL 的 Where 语句" DataField="FILTER_TSQL_WHERE" Width="300"  />
                <mi:CheckColumn HeaderText="查询可视" DataField="IS_SEARCH_VISIBLE"  Width="50"  />

               
                <mi:SelectColumn DataField="V_LIST_MODE_ID" HeaderText="显示型式" TriggerMode="None" Width="80">
                    <mi:ListItem Value="DEFAULT" Text="默认" />
                    <mi:ListItem Value="BoundField" Text="文本框" />
                    <mi:ListItem Value="SelectColumn" Text="下拉框" />
                    <mi:ListItem Value="TriggerColumn" Text="弹出框" />
                </mi:SelectColumn>
                
                <mi:GroupColumn HeaderText="下拉框或弹出窗体">

                    <mi:SelectColumn DataField="ACT_MODE" HeaderText="选择模式" TriggerMode="None">
                        <mi:ListItem Value="NONE" Text="没有" />
                        <mi:ListItem Value="FIXED" Text="固定值模式" />
                        <mi:ListItem Value="TABLE" Text="工作表模式" />
                    </mi:SelectColumn>

                    <mi:TriggerColumn DataField="ACT_FIXED_ITEMS" HeaderText="固定选择项目" OnButtonClick="showTextDialog(this)" ButtonType="More" TriggerMode="None" />
                    <mi:TriggerColumn DataField="ACT_TABLE_ITEMS" HeaderText="工作表项目" OnButtonClick="showTableDialog(this)" ButtonType="More" TriggerMode="UserInput" />
                    
                </mi:GroupColumn>

            </Columns>
        </mi:Table>

        <mi:WindowFooter runat="server" ID="footer1">
            <mi:Button runat="server" ID="Button1" Text="关闭" Width="80" Height="26" Dock="Center" OnClick="ownerWindow.close()" />
        </mi:WindowFooter>

    </mi:Viewport>

</form>




<script>



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




        var win = Mini2.createTop('Mini2.ui.Window', {
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

        var win = Mini2.createTop('Mini2.ui.extend.TextWindow');

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