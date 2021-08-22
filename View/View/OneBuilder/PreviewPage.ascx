<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PreviewPage.ascx.cs" Inherits="App.InfoGrid2.View.OneBuilder.PreviewPage" %>
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

<form action="" id="form1" method="post" >

<div runat="server" id="StoreSet">

</div>
<mi:Panel runat="server" ID="HeadPanel" Height="40" Dock="Top" Scroll="None" PaddingTop="0" >
    <mi:Label runat="server" ID="headLab" Value="单号" HideLabel="true"  Mode="Transform" BodyAlign="Center" />
</mi:Panel>
<mi:Viewport runat="server" ID="viewport1" MarginTop="40">

</mi:Viewport>

<% if (this.IsBuilder())
   { %>
<div id="SwitchPanel" style="width:200px;height:40px;border: 1px solid #C0C0C0;background-color: #FFFFFF;">
    <div style="margin:8px 24px 8px 8px; text-align:right;">
        <mi:Button runat="server" ID="Button1" OnClick="change_pageToForm()" Text="转换为表单" />

        <mi:Button runat="server" ID="EditPageBtn" OnClick="EditTemplate()" Text="设置" />
    </div>
</div>
<script type="text/javascript">

    $(document).ready(function () {

        var ps = Mini2.create('Mini2.ui.extend.PullSwitch', {
            panelId: 'SwitchPanel'
        });

        ps.render();
    });

    function EditTemplate() {
    
        var id = $.query.get('pageid');
        window.location.href = 'PageBuilder.aspx?id=' + id;
        //EcView.show('PageBuilder.aspx?id=' + id, '修改');
    }

    function change_pageToForm() {
        var id = $.query.get('pageid');

        var url = '/App/InfoGrid2/View/OnePage/ChangeFormStepNew1.aspx?id=' + id;

        EcView.show(url,'复杂表转表单');
    }

</script>
<%} %>
</form>
<%= GetDisplayRule() %>

<script type="text/javascript">

    $(document).ready(function () {


        $(window).resize(function () {

            var w = $(this).width();

            var toolbar = window.widget1_I_AdminTools;

            if (toolbar && toolbar.setWidth) {
                toolbar.setWidth(w);
            }

        });

        setTimeout(function () {

            var w = $(this).width();

            var toolbar = window.widget1_I_AdminTools;

            if (toolbar && toolbar.setWidth) {
                toolbar.setWidth(w);
            }


        }, 500);

    });


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
            tag = me.tag,
            curPage = window.curPage,
            mainStore= curPage.mainStore;

        var record = me.record;

        var menuID = $.query.get('menu_id');
        var pageid = $.query.get('pageid');

        //获取主表的信息
        var mainRow = mainStore.getCurrent();

        //如果主表没有数据，则不弹出选择窗口
        if (!mainRow) {
            return;
        }


        


        if (!tag || tag == '') {
            return;
        }

        var ps = eval('(' + tag + ')');

        var urlStr = Mini2.urlAppend("/App/InfoGrid2/view/OneSearch/SelectPreview.aspx", {
            'type': ps.type_id,
            'viewId': ps.view_id,
            'menu_Id': menuID,
            'dialog_col': me.dataIndex,
            'ui_Type_id': 'PAGE',
            'owner_page_id': pageid,
            'main_table': curPage.mainModel,
            'main_row_id': mainRow.getId(),
            'owner_table': record.name,
            'owner_row_id': record.getId()
        });

        console.debug("record = ", record);

            //$.format("type={0}&viewId={1}&menu_Id={2}&dialog_col={3}&ui_Type_id=PAGE&owner_page_id={4}",
            //ps.type_id, ps.view_id, menuID, me.dataIndex, pageid);

        
        //urlStr += $.format("&main_table={0}&main_row_id={1}", curPage.mainModel, mainRow.getId());

        

        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '选择',
            iframe: true,
            width: 800,
            state:'max',
            height: 600,
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

        base64 = base64.replace('+', '%2B');

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

        if (e.is_multi) {

            var row = e.rows[0];
            var map = e.map;

            setRecordValues(record, editor, editorMaps, row, map);

            var idsStr = '';

            for (var i = 0; i < e.new_ids.length; i++) {
                idsStr += e.new_ids[i] + ",";
            }

            widget1.submit($('form:first'), {
                action: 'GoQueryInsert',
                actionPs: {
                    storeId: record.store.id,
                    curId: record.getId(),
                    selectTable: e.select_table, //选择的表名
                    selectIds: idsStr,
                    mapId: e.map_id

                }
            });

            //widget1.subMethod($('form:first'), { subName: 'Store_UT_091', subMethod: 'Insert' });
        }
        else {

            var row = e.row;
            var map = e.map;

            setRecordValues(record, editor, editorMaps, row, map);
        }


        log.end();
    }


    function setRecordValues(record, editor, editorMaps, row, map) {

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
    }

    function form_EditShow() {

    }

    
</script>

