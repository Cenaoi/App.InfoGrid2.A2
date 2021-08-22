<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TablePreview.ascx.cs" Inherits="App.InfoGrid2.View.OneTable.TablePreview" %>
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

<style type="text/css">
    .page-head
    {
         font-size:26px;
         font-weight:bold;
         
    }
</style>

<form action="" method="post">
<mi:Store runat="server" ID="store1" Model="" IdField="" PageSize="20" LockedField="" TranEnabled="true"  >
    

</mi:Store>

<mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="0" >
    <mi:Label runat="server" ID="headLab" Value="单号" HideLabel="true" Mode="Transform" BodyAlign="Center" />
</mi:Panel>

<mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="40">

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >

        <mi:FormLayout runat="server" ID="searchForm" Dock="Top" Region="North" FlowDirection="TopDown"
            ItemWidth="300" ItemLabelAlign="Right" ItemClass="mi-box-item" Layout="HBox"
            StoreID="store1" FormMode="Filter" Scroll="None">


        </mi:FormLayout>

        <mi:Toolbar ID="Toolbar1" runat="server">
            
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full"  >
            <Columns>
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>

<% if (this.IsBuilder())
   { %>
<div id="SwitchPanel" style="width:800px;height:40px;border: 1px solid #C0C0C0;background-color: #FFFFFF;">
    <div style="margin:8px 24px 8px 8px; text-align:right;">

        <mi:Button runat="server"  Text="工具栏定义" Command="ToolbarSetup" />
        <mi:Button runat="server" ID="Button2" Command="GoValidSetup" Text="验证设置" />

        <mi:Button runat="server"  Text="列定义" Command="StepEdit2" />
        <mi:Button runat="server"  Text="列设置"  Command="StepEdit3" />
        <mi:Button runat="server"  Text="列高级设置"  Command="StepEdit4" />
        
        <mi:Button ID="Button1" runat="server"  Text="联动设置"  Command="GoActList" />

        <mi:Button ID="Button5" runat="server"  Text="保存顺序" OnClick="ec5_PostColsSeq()" />
        <mi:Button ID="Button3" runat="server"  Text="保存宽度" OnClick="ec5_PostCols()" />
    </div>
</div>
    <input type="hidden" name="table_cols_data" id="table_cols_data" value="{}" />
<script type="text/javascript">

    //提交尺寸
    function ec5_PostCols() {

        var table1 = window.widget1_I_table1;
        
        var json = {
            cols: []
        };

        for (var i = 0; i < table1.columns.length; i++) {
            var col = table1.columns[i];

            if (!col || !col.dataIndex || col.dataIndex == '') {
                continue;
            }

            json.cols.push({
                index : i,
                field: col.dataIndex,
                width: col.width
            });
        }


        var jsonText = Mini2.Json.toJson(json);

        console.log(jsonText);

        $('#table_cols_data').val(jsonText);

        widget1.submit('form', {
            action: 'PostTableColWidth',
            actionPs: ''
        });
    }

    //提交宽度
    function ec5_PostColsSeq() {

        var table1 = window.widget1_I_table1;
        
        var json = {
            cols: []
        };

        for (var i = 0; i < table1.columns.length; i++) {
            var col = table1.columns[i];

            if (!col || !col.dataIndex || col.dataIndex == '') {
                continue;
            }

            json.cols.push({
                index : i,
                field: col.dataIndex,
                width: col.width
            });
        }


        var jsonText = Mini2.Json.toJson(json);

        console.log(jsonText);

        $('#table_cols_data').val(jsonText);

        widget1.submit('form', {
            action: 'PostTableColSeq',
            actionPs: ''
        });
    }

    $(document).ready(function () {

        var ps = Mini2.create('Mini2.ui.extend.PullSwitch', {
            panelId: 'SwitchPanel'
        });

        ps.render();
    });
</script>
<%} %>
</form>

<%= GetDisplayRule() %>

<script type="text/javascript">


    function showDialog(owner) {


    }

    function TarSearch() {

        var viewport1 = window.widget1_I_viewport1;

        viewport1.toggleRegion('north');

    }

    function form_Closed(e) {
        var me = this,
            record = me.record,
            editor = me.editor;

        if (e.result != 'ok') { return; }

        var row = e.row;
        var map = e.map;


        for (var i = 0; i < map.length; i++) {
            var m = map[i];
            var v = row[m.src];

            record.set(m.to, v);

            if (editor.dataIndex == m.to) {
                editor.setValue(v);
            }
        }

    }

    function showDialgoForTable(owner) {

        var me = owner,
            tag = me.tag;
        var record = me.record;

        if (!tag || tag == '') {
            return;
        }

        var ps = eval('(' + tag + ')');

        var urlStr = $.format("/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={0}&viewId={1}",
            ps.type_id, ps.view_id);

        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '选择',
            iframe: true,
            width: 800,
            height: 600,
            startPosition: 'center_screen',
            url: urlStr
        });

        win.editor = me;
        win.record = record;
        win.show();

        win.formClosed(form_Closed);

    }

</script>



<script>

    //根据输入的文字获取数据   回车按钮触发
    function biz_mapping(owner, type_id, view_id, one_field) {

        var me = owner;
        var record = me.record;

        log.debug(one_field);

        var id = owner.getValue();
        var mapItems = owner.mapItems;  //映射字段的规则

        var base64 = window.BASE64.encoder("[{field:'" + one_field + "',logic:'like',value:'%" + id + "%'}]");//返回编码后的字符 

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

            console.log(model);

            if (model.result != "ok") { return; }

            //如果只有一个就赋值到文本框中
            if (model.count == 1) {
                biz_setMapValues(record, model.data, mapItems);
            }
            else {

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

                win.formClosed(biz_FormClosed);


            }

        });

    }

    //设置映射值
    function biz_setMapValues(record, model, mapItems) {

        var me = this,
            i,
            item,
            srcValue;

        if (mapItems && model) {

            console.log(model);
            console.log(mapItems);

            for (i = 0; i < mapItems.length; i++) {

                item = mapItems[i];

                if ('' == item.srcField || '' == item.targetField) {
                    continue;
                }

                srcValue = model[item.srcField];

                record.set(item.targetField, srcValue);
            }

        }
    }


    function RndNum(n) {
        var rnd = "";
        for (var i = 0; i < n; i++)
            rnd += Math.floor(Math.random() * 10);
        return rnd;
    }

    function biz_FormClosed(e) {
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


    //显示窗体
    function form_EditShow(view, cell, recordIndex, cellIndex, e, record, row) {

<%--        var id = record.getId();
        var formEditPageId = <%= this.FormEditPageID %>;
        var altis = '<%= this.FromEditAliasTitle %>';

        var url = $.format('/App/InfoGrid2/View/OneForm/FormEditPreview.aspx?row_pk={0}&pageId={1}&alias_title={2}', id, formEditPageId, altis);
        --%>
        var menu_id = $.query.get('menu_id');
        var formType = '<%= this.FormEditType %>';
        var id = record.getId();
        var formEditPageId = <%= this.FormEditPageID %>;
        var altis = '<%= this.FromEditAliasTitle %>';

        
        var url;
        
        // = $.format('/App/InfoGrid2/View/OneForm/FormEditPreview.aspx?row_pk={0}&pageId={1}&menu_id={2}&alias_title={3}', 
        //    id, formEditPageId, menu_id, altis);
        
        if(formType == 'ONE_FORM'){
            url = Mini2.urlAppend('/App/InfoGrid2/View/OneForm/FormEditPreview.aspx',{
                row_pk: id,
                pageId: formEditPageId,
                menu_id :menu_id,
                alias_title : altis
            });
        }
        else if(formType == 'TABLE_FORM'){
            url = Mini2.urlAppend('/App/InfoGrid2/View/OneForm/FormOneEditPreview.aspx',{
                row_pk: id,
                pageId: formEditPageId,
                menu_id :menu_id,
                alias_title : altis
            });


        }


        EcView.show(url, '表单编辑');
    }


</script>

