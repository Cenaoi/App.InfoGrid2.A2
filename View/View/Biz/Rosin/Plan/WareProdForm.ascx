<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WareProdForm.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.Plan.WareProdForm" %>

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
      

<div runat="server" id="StoreSet" style="">
    <mi:Store runat="server" ID="storeMain1" Model="UT_009" IdField="ROW_IDENTITY_ID">
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
            <mi:QueryStringParam Name="ROW_IDENTITY_ID" QueryStringField="row_id" />
        </FilterParams>
    </mi:Store>

</div>


<mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="0" Padding="0">

<%--    
    <mi:Toolbar runat="server" ID="mainToolbar1">
        <mi:ToolBarButton Text="保存" Command="GoSave" />
        <mi:ToolBarButton Text="刷新" Command="ser:storeMain1.Refresh()" />

    </mi:Toolbar>
--%>

    <mi:Panel runat="server" ID="HeadPanel" Height="50" Scroll="None" PaddingTop="10">
        <mi:Label runat="server" ID="headLab" Value="单据" HideLabel="true" Dock="Center" Mode="Transform"  />
    </mi:Panel>

    <mi:FormLayout runat="server" ID="HanderForm1" ItemWidth="300" PaddingTop="10" StoreID="storeMain1" Scroll="Auto"
        ItemLabelAlign="Right" Region="Center" FlowDirection="LeftToRight" >

        <mi:TextBox runat="server" ID="COL_2_tb" DataField="COL_2" FieldLabel="货物卡号" />


        <mi:TriggerBox runat="server" ID="COL_3_tb" DataField="PROD_CODE" FieldLabel="货物编码" ButtonType="More"  Required="true"
            OnButtonClick="showDialgoForTable_ProdName(this)" />

            
        <mi:TextBox runat="server" ID="TextBox1" DataField="COL_3" FieldLabel="货物品名" />

        <mi:TextBox runat="server" ID="COL_4_tb" DataField="COL_4" FieldLabel="材质" />
        <mi:ComboBox runat="server" ID="COL_5_tb" DataField="COL_5" FieldLabel="规格" TriggerMode="None"
            StringItems="特级;一级;二级;三级;四级;等外（黑香/红香）" >
        </mi:ComboBox>

        <mi:TextBox runat="server" ID="COL_6_tb" DataField="COL_6" FieldLabel="品牌" />
        <mi:ComboBox runat="server" ID="COL_7_cb" DataField="COL_7" FieldLabel="产地" >

        </mi:ComboBox>

        <mi:TextBox runat="server" ID="PACK_TEXT_tb" DataField="PACK_TEXT" FieldLabel="包装" />
        <mi:DatePicker runat="server" ID="DatePicker1" DataField="PROD_DATE" FieldLabel="生产日期" />

        <div class="mi-newline mi-newline-border" >
            
        </div>
            
        <mi:TextBox runat="server" ID="COL_8_tb" DataField="COL_8" FieldLabel="车(箱)号" />
        <mi:DatePicker runat="server" ID="COL_9_dp" DataField="COL_9" FieldLabel="到货时间" />
        <mi:DatePicker runat="server" ID="COL_10_dp" DataField="COL_10" FieldLabel="验收时间" />
        <mi:ComboBox runat="server" ID="COL_11_tb" DataField="COL_11" FieldLabel="验收方式" StringItems="点件;过磅;量体积;其它" TriggerMode="None" />
        <mi:ComboBox runat="server" ID="COL_12_tb" DataField="COL_12" FieldLabel="工人装卸"  StringItems="是;否" TriggerMode="None" />
        <mi:ComboBox runat="server" ID="COL_13_tb" DataField="COL_13" FieldLabel="分拣" TriggerMode="None" StringItems="是;否" />
        <mi:ComboBox runat="server" ID="COL_14_tb" DataField="COL_14" FieldLabel="换袋" TriggerMode="None" StringItems="是;否" />
        <mi:TextBox runat="server" ID="COL_15_tb" DataField="COL_15" FieldLabel="打包材料" />
            
        
        <div class="mi-newline" >
            <div class="mi-newline-text"></div>
        </div>

    <%--       
        <mi:NumberBox runat="server" ID="COL_18_nb" DataField="COL_18" FieldLabel="应收数量" />
        <mi:NumberBox runat="server" ID="COL_19_nb" DataField="COL_19" FieldLabel="应收重量" />
        <mi:TextBox runat="server" ID="COL_20_tb" DataField="COL_20" FieldLabel="应收散件" />--%>

        <table border="0" cellpadding="0" cellspacing="0" >
            <tr>
                <td><mi:NumberBox runat="server" ID="COL_21_nb" DataField="COL_21" FieldLabel="数量" LabelAlign="Right" Width="200" /></td>
                <td><mi:ComboBox runat="server" ID="COL_22_tb" DataField="COL_22" FieldLabel="数量单位名称" StringItems="桶;袋;其它" HideLabel="true" Width="100%" TriggerMode="None" /></td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" >
            <tr>
                <td><mi:NumberBox runat="server" ID="COL_23_nb" DataField="COL_23" FieldLabel="重量" LabelAlign="Right" Width="200"  /></td>
                <td><mi:ComboBox runat="server" ID="COL_24_tb" DataField="COL_24" FieldLabel="重量单位名称" StringItems="吨;公斤;其它" HideLabel="true" Width="100%" TriggerMode="None" /></td>
            </tr>
        </table>
        <mi:TextBox runat="server" ID="COL_25_tb" DataField="COL_25" FieldLabel="散件" />


        <mi:TextBox runat="server" ID="COL_27_tb" DataField="COL_27" FieldLabel="货主" />
        <mi:TextBox runat="server" ID="COL_28_tb" DataField="COL_28" FieldLabel="等级" />
        <mi:TextBox runat="server" ID="COL_29_tb" DataField="COL_29" FieldLabel="厂家" />
            
        <mi:TextBox runat="server" ID="COL_16_tb" DataField="COL_16" FieldLabel="存放货位" />
        <mi:TextBox runat="server" ID="COL_17_tb" DataField="COL_17" FieldLabel="货位备注" />

        <mi:TextBox runat="server" ID="COL_26_tb" DataField="COL_26" FieldLabel="货物备注" MinWidth="606px" />

    </mi:FormLayout>

    <mi:WindowFooter runat="server" ID="windowFooter1">
        <mi:Button runat="server" ID="btn1" Text="关闭" Width="80"  OnClick="ownerWindow.close()" Dock="Center" />
    </mi:WindowFooter>
</mi:Viewport>
</form>
<script>

    "use strict";

    //显示窗体
    function form_EditShow(view, cell, recordIndex, cellIndex, e, record, row) {

        var id = record.getId();
        var formEditPageId = 8;
        var altis = '货物明细';

        var url = $.format('/App/InfoGrid2/View/OneForm/FormEditPreview.aspx?row_pk={0}&pageId={1}&alias_title={2}&form_eiit_pageId={3}', id, formEditPageId, altis,formEditPageId);
        
        var win = Mini2.createTop('Mini2.ui.Window', {
            url : url,
            width : 1000,
            height: 768,
            state:'max',
            mode:true
        });

        win.show();

        //EcView.show(url, '表单编辑');
    }


    function showDialgoForTable_ProdName(owner) {


        showDialgoForTable(owner, "{\"type_id\":\"VIEW\",\"view_id\":39,\"table_name\":\"UT_005\",\"owner_table_id\":4}");
    }

    function showDialgoForTable(owner,tag) {

        var me = owner,
            tag = tag || me.tag;

        var formPanel = me.ownerParent;
        var store = formPanel.store;

        var record = store.getCurrent();


        if (!tag || tag == '') {
            return;
        }

        var ps = eval('(' + tag + ')');

        var urlStr = $.format("/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={0}&viewId={1}",
            ps.type_id, ps.view_id);

        var win = Mini2.createTop('Mini2.ui.Window', {
            mode: true,
            text: '选择',
            iframe: true,
            width: 800,
            height: 600,
            startPosition: 'center_screen',
            url: urlStr
        });

        //win.editor = me;
        win.record = record;
        win.show();

        win.formClosed(function (e) {
            form_Closed(e, owner, record);
        });

    }

    function form_Closed(e, triggerBox, record) {

        var me = this;

        if (e.result != 'ok') { return; }

        var map = e.map;

        var row = e.row;

        var newValues = {};

        for (var i = 0; i < map.length; i++) {


            var m = map[i];
            var v = row[m.src];

            newValues[m.to] = v;
        }

        record.set(newValues);

    }


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

                var win = Mini2.createTop('Mini2.ui.Window', {
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


</script>

