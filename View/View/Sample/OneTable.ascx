<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OneTable.ascx.cs" Inherits="App.InfoGrid2.View.Sample.OneTable" %>
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



<form action="" method="post">
<mi:Store runat="server" ID="store1" Model="BIZ_TEST" IdField="BIZ_TEST_ID" PageSize="20">

</mi:Store>
    
<!--UT_102 吸塑指令单-订单明细-->
<mi:Store runat="server" ID="StoreUT102" Model="UT_102" IdField="ROW_IDENTITY_ID" DeleteRecycle="true">
    <FilterParams>
        <mi:QueryStringParam Name="COL_13" QueryStringField="id" />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None"  Visible="false">

        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarTitle ID="tableNameTB1" Text="表名" />

            <mi:ToolBarButton Text="新增" OnClick="ser:store1.Insert()" />
            <mi:ToolBarButton Text="保存" OnClick="ser:store1.SaveAll()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm.toggle()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:store1.Delete()" />


        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:NumColumn DataField="NUM_1" HeaderText="数值" Format="0" Width="200" NotDisplayValue="0" />
                <mi:DateColumn DataField="DATE_1" HeaderText="日期" />
                <mi:NumColumn DataField="STR_1" HeaderText="字符串" />

            </Columns>
        </mi:Table>
    </mi:Panel>

    
    <mi:Table runat="server" ID="mainClientTable" Dock="Full" StoreID="StoreUT102"  PagerVisible="false" SummaryVisible="true" Region="South">
        <Columns>
            <mi:RowNumberer />
            <mi:RowCheckColumn />
            <mi:TriggerColumn HeaderText="客户编号" DataField="COL_22"  ButtonClass="mi-icon-more"
                OnButtonClick="showDialgoForTable_102(this,'VIEW',275)" 
                OnMaping="mapping_103(this,'VIEW',275,'COL_1')" 
                Tag='{"type_id":"VIEW","view_id":275, "one_field":"COL_1", "table_name":"UT_071","owner_table_id":213}' Width="200" >
                <MapItems>
                    <mi:MapItem SrcField="COL_2" TargetField="COL_1" Remark="客户名称" />
                    <mi:MapItem SrcField="COL_1" TargetField="COL_22" Remark="客户编号" />
                </MapItems>
            </mi:TriggerColumn>
            <mi:TriggerColumn HeaderText="客户名称" DataField="COL_1"  ButtonClass="mi-icon-more" 
                OnButtonClick="showDialgoForTable_102(this,'VIEW',275)" 
                OnMaping="mapping_103(this,'VIEW',275,'COL_2')" 
                Tag='{"type_id":"VIEW","view_id":275, "one_field":"COL_1", "table_name":"UT_071","owner_table_id":213}' Width="200" >
                <MapItems>
                    <mi:MapItem SrcField="COL_2" TargetField="COL_1" Remark="客户名称" />
                    <mi:MapItem SrcField="COL_1" TargetField="COL_22" Remark="客户编号" />
                </MapItems>
                
            </mi:TriggerColumn>

        </Columns>
    </mi:Table>

</mi:Viewport>

</form>

<script>



    function form_Closed102(e) {
        var me = this,
           record = me.record,
           editor = me.editor,
           editorMaps = editor.mapItems;

        if (e.result != 'ok') { return; }

        var row = e.row;
        var map = e.map;

        log.begin("form_Closed102(...)");

        if (editorMaps && editorMaps.length) {

            map = editorMaps;

            console.log("映射...");
            console.log(map);

            record.beginEdit();

            for (var i = 0; i < map.length; i++) {
                var m = map[i];
                var v = row[m.srcField];


                record.set(m.targetField, v);

                if (editor.dataIndex == m.targetField) {
                    editor.setValue(v);
                }
            }

            record.endEdit();
        }
        else {
            for (var i = 0; i < map.length; i++) {
                var m = map[i];
                var v = row[m.src];

                record.set(m.to, v);

                if (editor.dataIndex == m.to) {
                    editor.setValue(v);
                }
            }
        }

        log.end();
    }





    //根据输入的文字获取数据   回车按钮触发
    function mapping_103(owner, type_id, view_id, one_field) {

        var me = owner;
        var record = me.record;


        log.debug("---record.id = " + record.id);

        var id = owner.getValue();
        var mapItems = owner.mapItems;  //映射字段的规则

        var base64 = window.BASE64.encoder("[{field:'" + one_field + "',logic:'like',value:'%" + id + "%'}]");//返回编码后的字符 

        base64 = base64.replace("+", "%2B");


        var urlStr = $.format("/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={0}&viewId={1}&filter2={2}",
            type_id, view_id, base64);


        $.get("/App/InfoGrid2/view/MoreView/OneMapAction.aspx", {
            type: type_id,
            viewId: view_id,
            id: id,
            one_field: '',
            filter2: base64,
            _rum: RndNum(8)
        },
        function (responseText, textStatus) {

            var model = eval('(' + responseText + ')');

            if (model.result != "ok") { return; }

            //如果只有一个就赋值到文本框中
            if (model.count == 1) {
                setMapValues(record, model.data, mapItems);
            }
            else {
                //找不到数据就把过滤条件清除掉
                if (model.count == 0) {
                    base64 = "";
                }

                var urlStr = $.format("/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={0}&viewId={1}&filter2={2}",
                    type_id, view_id, base64);

                var win = Mini2.create('Mini2.ui.Window', {
                    mode: true,
                    text: '选择',
                    iframe: true,
                    width: 800,
                    height: 600,
                    state: 'max',
                    startPosition: 'center_screen',
                    url: urlStr
                });


                win.editor = me;
                win.record = record;
                win.show();

                win.formClosed(form_Closed102);

            }

        });

    }



    ///这是弹窗按钮事件
    function showDialgoForTable_102(owner, type_id, view_id) {

        var me = owner,
            tag = me.tag;
        var record = me.record;


        if (!type_id || !view_id) {
            console.log("弹窗事件没有必要的参数！，type_id  和 view_id");
            return;
        }


        var urlStr = $.format("/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={0}&viewId={1}",
            type_id, view_id);


        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '选择',
            iframe: true,
            width: 800,
            height: 600,
            state: 'max',
            startPosition: 'center_screen',
            url: urlStr
        });

        win.editor = me;
        win.record = record;
        win.show();

        win.formClosed(form_Closed102);

    }


    function RndNum(n) {
        var rnd = "";
        for (var i = 0; i < n; i++)
            rnd += Math.floor(Math.random() * 10);
        return rnd;
    }

</script>