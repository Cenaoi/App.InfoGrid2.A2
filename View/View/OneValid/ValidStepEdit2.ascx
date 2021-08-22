<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ValidStepEdit2.ascx.cs" Inherits="App.InfoGrid2.View.OneValid.ValidStepEdit2" %>
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
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <UpdateParams>
        <mi:ServerParam Name="ROW_DATE_UPDATE" ServerField="TIME_NOW" />
    </UpdateParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport">
    
    <mi:Panel runat="server" ID="topPanel" Dock="Top" Region="North" Scroll="None">
        <mi:Toolbar runat="server" ID="tooblarX">
            <mi:ToolBarTitle Text="区域修改" />
            <mi:ToolBarButton Text="工具栏定义" Align="Right" Command="ToolbarSetup" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table2"  StoreID="storeTab" Height="80" PagerVisible="false"  Dock="Full" ReadOnly="true">
            <Columns>
                <mi:RowNumberer />
                <mi:BoundField DataField="DISPLAY" HeaderText="表名" Width="200" />
                <mi:CheckColumn DataField="SINGLE_SELECTION" HeaderText="单选" />
                <mi:CheckColumn DataField="SUMMARY_VISIBLE" HeaderText="显示汇总" Width="100" />
            </Columns>
        </mi:Table>
    </mi:Panel>

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Layout="Auto" Scroll="None" >
        <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarButton Text="应用到全部视图" BeforeAskText="将影响到整个系统的界面 , 确定应用到全部视图?" Command="GoApplyToViewAll" />
            
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="true" StoreID="store1" Sortable="false"  Dock="Full">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="F_NAME" HeaderText="字段名" EditorMode="None" />
                <mi:BoundField DataField="DISPLAY" HeaderText="显示名" />

                <mi:SelectColumn DataField="VALID_TYPE_ID" HeaderText="验证类型"  TriggerMode="None">
                    <mi:ListItem Value="" Text="" TextEx="自动" />
                    <mi:ListItem Value="METADATA" Text="元数据" />
                    <mi:ListItem Value="PLUG" Text="插件" />
                </mi:SelectColumn>
                <mi:BoundField DataField="VALID_CRITERIA" HeaderText="验证条件" Width="500" />
                <mi:CheckColumn DataField="VALID_REQUIRED" HeaderText="必填" Width="60" />
                <mi:CheckColumn DataField="IS_BIZ_MANDATORY" HeaderText="业务必填" Width="60" />
                <mi:CheckColumn DataField="VALID_UNIQUE" HeaderText="唯一性" Width="60" />

                <mi:TriggerColumn DataField="VALID_METADATA" HeaderText="元数据" Width="300" ButtonClass="mi-icon-more"  
                   OnButtonClick="showTableDialog(this)" />

                <mi:BoundField DataField="VALID_PLUG" HeaderText="插件" Width="300" />

            </Columns>
        </mi:Table>

    </mi:Panel>

    <mi:WindowFooter ID="WindowFooter1" runat="server">
        <mi:Button runat="server" ID="Button1" Width="80" Height="26" Command="GoLast" Text="返回" Dock="Center" />
    </mi:WindowFooter>

</mi:Viewport>


</form>

<script src="/Core/Scripts/Base64.js" type="text/javascript"></script>

<script type="text/javascript">




    function showTableDialog(owner) {

        var tableId = $.query.get("id");

        var record = owner.record;

        var colId = record.get('DB_FIELD');
        var metaData = record.get('VALID_METADATA');

        var urlStr;

        metaData = BASE64.encoder(metaData);

        metaData = metaData.replace("+", "%2B");


        urlStr = $.format("/App/InfoGrid2/View/OneValid/ValidParamSetup.aspx?rule={0}",
            metaData);




        var win = Mini2.createTop('Mini2.ui.Window', {
            mode: true,
            text: '列表框架',
            iframe: true,
            width: 880,
            height: 660,
            startPosition: 'center_screen',
            url: urlStr
        });

        win.show();

        win.formClosed(function (e) {
            if (e.result != 'ok') { return; }

            try {

                owner.setValue(e.rule);
            }
            catch (ex) {
                alert(ex.Message);
            }


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