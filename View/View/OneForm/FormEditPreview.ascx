<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormEditPreview.ascx.cs" Inherits="App.InfoGrid2.View.OneForm.FormEditPreview" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<link href="/Core/Scripts/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" />
<link href="/Core/Scripts/bootstrap/3.3.4/css/bootstrap-theme.min.css" rel="stylesheet" />


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

<script src="/Core/Scripts/webuploader-0.1.5/webuploader.js"></script>
<script src="/Core/Scripts/UEditor/1.2.6.0/ueditor.config.js"></script>
<script src="/Core/Scripts/UEditor/1.2.6.0/ueditor.all.min.js"></script>

<style type="text/css">
    .page-head
    {
         font-size:26px;
         font-weight:bold;
    }
       
    .enclosure-item{
        width: 300px; float: left; height: 60px;margin-right:10px; margin-bottom:10px; padding:8px;
    }
        
    .webuploader-container {
        margin-left:20px;
    }
    .webuploader-container .webuploader-pick{
        color:black;
    }
</style>


<script>

    /*设计模式, 正常模式切换*/
    $(function(){
        var design_mode = $.query.get("design_mode");

        console.info("design_mode = ", design_mode);

        if(design_mode == "true"){

            Mini2.designMode(true);

        }
    });

</script>

<form action="" method="post">


<div runat="server" id="StoreSet">

</div>

<mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="0" Padding="0">
    
    <mi:Toolbar runat="server" ID="mainToolbar1">
<%--        <mi:ToolBarButton Text="保存" />
        <mi:ToolBarButton Text="刷新" Command="GoRefresh" />
        <mi:ToolBarHr />
        <mi:ToolBarButton Text="提交" Command="GoBizSID_0_2" />--%>
    </mi:Toolbar>

    <mi:Panel runat="server" ID="HeadPanel" Height="60" Scroll="None" PaddingTop="10">
        <mi:Label runat="server" ID="headLab" Value="提运单" HideLabel="true" Dock="Center" Mode="Transform"  />
    </mi:Panel>
    <mi:ClipImage runat="server" ID="bizSID_ci" DataField="BIZ_SID" HideLabel="true" Value="0" DataSource="storeMain1" Position="Relative"
         Left="10px" Top="40px" Width="129" Height="47">
        <mi:ListItem Value="-3" Text="/res/signet_sid/SID_F3.png" /> 
        <mi:ListItem Value="0" Text="/res/signet_sid/SID_0.png" />            
        <mi:ListItem Value="2" Text="/res/signet_sid/SID_2.png" />
        <mi:ListItem Value="4" Text="/res/signet_sid/SID_4.png" />
    </mi:ClipImage>

    <mi:FormLayout runat="server" ID="FormLayoutTop" ItemWidth="200" ItemLabelWidth="60" PaddingTop="10" StoreID="storeMain1"  Dock="Right" Top="40px"
                ItemLabelAlign="Right" Width="380px"  FlowDirection="LeftToRight" AutoSize="true" Position="Relative" Right="0px" Left="auto" >   
             
        <mi:TextBox runat="server" ID="textBox4" DataField="BIZ_CREATE_USER_TEXT" FieldLabel="开单人" ReadOnly="true" Width="160px" Validate="{}" />
        <mi:TextBox runat="server" ID="textBox13" DataField="BIZ_CREATE_DATE" FieldLabel="开单时间" ReadOnly="true" />
                
        <div class="mi-newline" style="width:96%;height:1px;" ></div>

        <mi:TextBox runat="server" ID="textBox14" DataField="BIZ_CHECK_USER_TEXT" FieldLabel="审核人" ReadOnly="true" Width="160px" />
        <mi:TextBox runat="server" ID="textBox15" DataField="BIZ_CHECK_DATE" FieldLabel="审核时间" ReadOnly="true" />
        
    </mi:FormLayout>

    <mi:FormLayout runat="server" ID="HanderForm1" ItemWidth="300" PaddingTop="10" ItemLabelAlign="Right" Region="North" FlowDirection="LeftToRight" AutoSize="true">

    </mi:FormLayout>

    <mi:Panel runat="server" ID="panel1" Padding="8"  Region="Center" Scroll="None" MinHeight="600" >
        <mi:TabPanel runat="server" ID="detailTabPanel" Height="400" Region="Center" Dock="Full" ButtonVisible="false" Plain="true" UI="win10" >

        </mi:TabPanel>
    </mi:Panel>


    <mi:FormLayout runat="server" ID="footerForm1" ItemWidth="300" PaddingTop="8" PaddingBottom="8" ItemLabelAlign="Right" Region="South" FlowDirection="LeftToRight" AutoSize="true">

    </mi:FormLayout>


        <mi:Panel runat="server" ID="Enclosure1" Region="South" Height="140px" Padding="4" Scroll="None">

            <div style="height: 30px;background-color:#e6eaf0;padding-left:10px;padding-top:4px;" dock="top">
                <div style="font-weight:bold; float:left; padding-top:4px;">附件</div>
                <div style="float:left; padding-top:4px;"></div>
                <button id="batchUploadBtn" style="height:22px;float:left;" >添加附件</button>
            </div>
            <div id="Enclosure_list" dock="full" style="border:2px solid #e6eaf0;padding:6px; overflow:auto;">
                <div class="enclosure-item" v-for="i in items">
                    <div style="height:46px;width:46px; float:left;overflow:hidden;"><img :src="i.EX_PATH" style="width:46px;height:46px;" /></div>
                    <div style="height:46px; width:230px; float:left; overflow:hidden; padding:0px 0px 0px 6px;">
                        <div style="float:left; max-width:160px;overflow:hidden;height:20px;" :title="i.FILE_NAME">{{i.FILE_NAME}}</div>
                        <div style="float:left;margin-left:6px; color:#808080;height:20px;">({{i.FILE_SIZE_STR}})</div>
                        <div style="padding:6px;float: left; width:100px; height:23px;">
                            <a :href="'/View/Biz/Handle/DownloadFile.ashx?biz_file_id='+i.BIZ_FILE_ID" target="_blank">下载</a>
                            <a href="#" @click="delete_item(i)">删除</a>
                        </div>
                    </div>
                </div>
            </div>
        </mi:Panel>


</mi:Viewport>
    

    <!--流程的简要信息-->
    <div id="flow_menupanel" class="mi-panel mi-layer mi-panel-default mi-border-box mi-menu-panel" 
        style="display:none;height: auto; right: auto;left: 480px; top: 40px; width:auto;z-index: 19001" >

        <div  id="flow_list" class="mi-axis" style="display:none;">

            <div class="mi-axis-list-wrapper"  v-for="flow in flow_list" >
                <p class="mi-axis-title" style="font-weight:bold;">流程编码: {{flow.def_code}} ({{flow.state}}) <span style="color:#c8c8c8;font-style:normal;font-weight:100;float:right;"> {{flow.inst_code}}</span> </p>
                <ul class="mi-axis-list">
                    <li v-for="step in flow.step_items"  >
                        {{ fromNow(step.date_end) }}
                        <span v-bind:style="{ color: (step.is_back_operate?'red':'') }" >[{{step.state}}]</span>
                        <span >{{step.text}}</span>
                        <span>{{step.comments}}</span>
                        <span style="padding-left:20px;float: right;">{{ formatDate(step.date_end) }}</span>
                    </li>
                </ul>
            </div>

        </div>     

    </div>

    <!--选择流程的界面-->
    <div id="flowSelectPanel" class="mi-panel mi-layer mi-panel-default mi-border-box mi-tooltip" 
        style=" display:none;height: auto; right: auto;left: 480px; top: 40px; width:auto;z-index: 19001" >

        <span>选择流程</span>

        <div  id="flowSelectPanels" class="mi-panel" style="">

            <button type="button" v-for="btn in btns" onclick="">流程</button>

        </div>     

    </div>
    
    <input type="hidden" name="table_cols_data" id="table_cols_data" value="{}" />
    
<% if (this.IsBuilder())
   { %>
<div id="SwitchPanel" style="width:600px;height:40px;border: 1px solid #C0C0C0;background-color: #FFFFFF;">
    <div style="margin:8px 24px 8px 8px; text-align:right;">
        
        <mi:Button runat="server" ID="EditPageBtn" OnClick="EditTemplate()" Text="设置" />
        
        <mi:Button runat="server" ID="Button3" Text="流程设置" Command="GoFlowSetup" />
        <mi:Button runat="server" ID="Button4" Text="流程节点权限设置" Command="GoFlowFormSetup" />

        <mi:Button runat="server" ID="Button1" OnClick="ChangeDesignMode()" Text="切换设计模式" />
        
        <mi:Button runat="server" ID="Button5" OnClick="SvaeLocalItemsForHander()" Text="保存表头位置" />
        <mi:Button runat="server" ID="Button2" OnClick="SvaeLocalItemsForFooter()" Text="保存表尾位置" />
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

        EcView.show('/App/InfoGrid2/View/OneBuilder/PageBuilder.aspx?id=' + id, '表单复杂表');

    }


</script>
<%} %>
</form>
<script>


    //显示窗体
    function form_EditShow(view, cell, recordIndex, cellIndex, e, record, row) {

        var id = record.getId();
        var formEditPageId = 8;
        var altis = '货物明细';

        var url = $.format('/App/InfoGrid2/View/OneForm/FormEditPreview.aspx?row_pk={0}&pageId={1}&alias_title={2}&form_eiit_pageId={3}', id, formEditPageId, altis, formEditPageId);

        var win = Mini2.createTop('Mini2.ui.Window', {
            url: url,
            width: 1000,
            height: 768,
            state: 'max',
            mode: true
        });

        win.show();

        //EcView.show(url, '表单编辑');
    }



    function showDialgoForTable(owner) {
        var me = owner,
            tag = me.tag,
            store,
            record,                
            curPage = window.curPage,
            mainStore= curPage.mainStore;


        
        //获取主表的信息
        var mainRow = mainStore.getCurrent();

        //如果主表没有数据，则不弹出选择窗口
        if (!mainRow) {
            return;
        }


        if (me.record) {
            record = me.record;
        }
        else {
            var formPanel = me.ownerParent;
            store = formPanel.store;
            record = store.getCurrent();
        }

        var menuId = $.query.get('menu_Id');
        var pageId = $.query.get('pageId');

        var ownerDialogCol = owner.dataIndex || owner.dataField;

        if (!tag || tag == '') {
            return;
        }

        var ps = eval('(' + tag + ')');


        var urlStr = Mini2.urlAppend('/App/InfoGrid2/view/OneSearch/SelectPreview.aspx',{            
            type: ps.type_id,
            viewId: ps.view_id,
            menu_id: menuId,

            owner_page_id : pageId,
            dialog_col : ownerDialogCol,
            ui_Type_id:'PAGE',

            main_table: curPage.mainModel,
            main_row_id: mainRow.getId(),

            owner_table : record.name,
            owner_row_id: record.getId()
        },true);

        //console.debug("record",record);
        //console.debug('urlStr = ', urlStr);

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

        win.formClosed(function(e){
            form_Closed( e, owner, record);
        });

    }

    function form_Closed(e, triggerBox ,record) {

        var me = this;

        if (e.result != 'ok') { return; }



        if (e.is_multi) {

            var row = e.rows[0];
            var map = e.map;

            
            try{
                var newValues = {};

                for (var i = 0; i < map.length; i++) {
                    var m = map[i];
                    var v = row[m.src];
                    newValues[m.to] = v;
                }

                record.set(newValues);

                var tDataIndex = triggerBox.dataIndex;
                var curValue = newValues[tDataIndex];

                triggerBox.setValue(curValue);
            }
            catch(ex){
                console.error("setRecordValues(...) 设置记录值错误.",ex);
            }

            
            var idsStr = '';

            for (var i = 0; i < e.new_ids.length; i++) {
                idsStr += e.new_ids[i] + ",";
            }

            
            var actPs =  {
                storeId: record.store.id,
                curId: record.getId(),
                selectTable: e.select_table, //选择的表名
                selectIds: idsStr,
                mapId: e.map_id

            };

            console.debug("actPs = ",actPs);

            widget1.submit($('form:first'), {
                action: 'GoQueryInsert',
                actionPs:actPs
            });

            //widget1.subMethod($('form:first'), { subName: 'Store_UT_091', subMethod: 'Insert' });
        }
        else {

            var row = e.row;
            var map = e.map;

            var newValues = {};

            for (var i = 0; i < map.length; i++) {


                var m = map[i];
                var v = row[m.src];

                newValues[m.to] = v;
            }

            record.set(newValues);

            var tDataIndex = triggerBox.dataIndex;
            var curValue = newValues[tDataIndex];

            triggerBox.setValue(curValue);
        }
    }


    //根据输入的文字获取数据   回车按钮触发
    function biz_mapping(owner, type_id, view_id, one_field) {

        var me = owner;
        var record = me.record;

        if(!record){        
            record = window.curPage.mainStore.getCurrent();
            console.info('没有找到 record, 可能是表单, 从表单获取', record);
        }

        console.debug("开始映射数据-------------------------------------------------------------------.");

        var id = owner.getValue();
        var mapItems = owner.mapItems;  //映射字段的规则

        var base64 = window.BASE64.encoder("[{field:'" + one_field + "',logic:'like',value:'%" + id + "%'}]");//返回编码后的字符 
        

        $.get("/App/InfoGrid2/view/MoreView/OneMapAction.aspx", {
            type: type_id,
            viewId: view_id,
            id: id,
            one_field: '',
            filter2: base64,
            _rum: Mini2.Guid.newGuid()
        },
        function (responseText, textStatus) {

            var model = eval('(' + responseText + ')');

            console.log('model = ',model);

            if (model.result != "ok") { return; }

            //如果只有一个就赋值到文本框中
            if (model.count == 1) {

                biz_setMapValues(record, model.data, mapItems);
            }
            else {

                var urlStr = Mini2.urlAppend("/App/InfoGrid2/view/OneSearch/SelectPreview.aspx",{
                    'type': type_id,
                    'viewId': view_id,
                    'filter2': base64
                });

                var win = Mini2.createTop('Mini2.ui.Window', {
                    mode: true,
                    text: '选择',
                    iframe: true,
                    width: 800,
                    height: 600,
                    state: 'max',
                    url: urlStr
                });

                win.editor = me;
                win.record = record;
                win.show();


                win.formClosed(function(e){
                    try{
                        biz_FormClosed.call(win, e);
                    }
                    catch(ex){
                        console.error('提交数据错误',ex);
                    }
                });

            }

        });

    }

    
    function biz_FormClosed(e) {
        var me = this,
           record = me.record,
           editor = me.editor,
           editorMaps = editor.mapItems;

        console.debug("xxxxxxxxxxxxxxx",e);

        if (e.result != 'ok') { return; }

        var row = e.row;
        var map = e.map;

        log.begin("form_Closed102(...)", e);

        if (e.is_multi) {

            var row = e.rows[0];
            var map = e.map;

            
            try{
                setRecordValues(record, editor, editorMaps, row, map);
            }
            catch(ex){
                console.error("setRecordValues(...) 设置记录值错误.",ex);
            }

            
            var idsStr = '';

            for (var i = 0; i < e.new_ids.length; i++) {
                idsStr += e.new_ids[i] + ",";
            }

            
            var actPs =  {
                storeId: record.store.id,
                curId: record.getId(),
                selectTable: e.select_table, //选择的表名
                selectIds: idsStr,
                mapId: e.map_id

            };

            console.debug("actPs = ",actPs);

            widget1.submit($('form:first'), {
                action: 'GoQueryInsert',
                actionPs:actPs
            });

            //widget1.subMethod($('form:first'), { subName: 'Store_UT_091', subMethod: 'Insert' });
        }
        else {

            var row = e.row;
            var map = e.map;
            
            console.debug("0000000000000 1");
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

                //if (editor.dataIndex == m.targetField) {
                //    editor.setValue(v);
                //}
            }

            record.endEdit();
        }
        else {

            var values = {};

            for (var i = 0; i < map.length; i++) {
                var m = map[i];
                var v = row[m.src];

                values[m.to] = v;
                                
                if (editor.dataIndex == m.to) {
                    editor.setValue(v);
                }
            }

            record.set(values);

        }
    }

    /**
     * 设置映射值
     * @param record
     * @param model
     * @param mapItems
     */
    function biz_setMapValues(record, model, mapItems) {

        var me = this,
            i,
            item,
            srcValue;

        if (mapItems && model) {

            var values = getMapValues(model, mapItems);

            console.debug('映射后的对象: ', values);

            record.set(values);            
        }
    }

    /**
     * 获取新的映射对象
     * @param model 实体
     * @param mapItems 映射对象
     */
    function getMapValues(model, mapItems) {

        var newValues = {};

        for (i = 0; i < mapItems.length; i++) {

            item = mapItems[i];

            if ( Mini2.String.isBlank(item.srcField) || Mini2.String.isBlank(item.targetField)) {
                continue;
            }

            srcValue = model[item.srcField];

            newValues[item.targetField] = srcValue;
        }

        return newValues;
    }


</script>


<script >

    /**
    * 流程的脚本
    *
    */


    var m_FlowBtns = [];

    //流程按钮变化
    function flow_BtnChagned() {

        var flowEnabled = <%= FlowEnabled %>;
        
        if(!flowEnabled){
            return;
        }

        var toolbar = Mini2.find('mainToolbar1');
        
        
        var page_id = <%= MainPageId %>;
        var table_id = <%= MainTableId %>;
        var row_id = <%= MainRowId %>;
        var menu_id = <%= MenuId %>;
        var doc_text = '<%= GetAliasTitle() %>';
        var doc_url = '<%= DocUrl %>';



        for (var i = 0; i < m_FlowBtns.length; i++) {
            var btn = m_FlowBtns[i];

            toolbar.remove(btn);
        }

        m_FlowBtns.length = 0;

        var url = '/App/InfoGrid2/view/OneForm/FlowHandle.aspx?action=get_buttons';

        Mini2.post(url, {
            page_id : page_id,
            table_id:table_id,
            row_id: row_id,
            menu_id : menu_id,
            doc_text : doc_text,
            doc_url : doc_url
        },
        function(data){

            flow_ButtonCreate(data);

        });


    }


    function flow_viewBtnCretae(viewCfg,userData){

        var btn = Mini2.create('Mini2.ui.toolbar.Button',{
            text : '查看流程',
            icon : '/res/icon/chart_organisation.png',

            tooltipContentEl : '#flow_menupanel',

            inst_code : viewCfg.inst_code,

            click:function(){
                var me = this,
                    inst_code =me.inst_code;
                
                if(Mini2.String.isBlank(inst_code)){
                    Mini2.toast('还没提交,无法查看流程.');
                    return;
                }
                
                var url = Mini2.urlAppend('/App/InfoGrid2/View/OneForm/FlowInstPreview.aspx',{
                    inst_code:inst_code
                });

                EcView.show(url,"查看流程");
            }
        });

        //提示显示前触发的事件
        btn.bind('tooltipShowing',function(){
            var vm = window.vm;
            
            if(!vm){                
                vm = new Vue({
                    el: '#flow_list',
                    data: {
                        flow_list: viewCfg.flow_list
                    },
                    methods:{
                        fromNow: function (item) {
                            if(!item){ return ''; }
                            return moment(item).fromNow();
                        },

                        formatDate: function (item) {
                            if(!item){ return ''; }
                            return Mini2.Date.format(new Date(item), 'm月d日 (l) H:i');
                        }
                    }
                });
                
                window.vm = vm;

                $('#flow_list').show();
            }
            else{
                vm.flow_list = viewCfg.flow_list;
            }
        });

        return btn;
    }


    function flow_ButtonCreate(btnConfig){

        var toolbar = Mini2.find('mainToolbar1');
                
        var userData = {
            
            page_id : <%= MainPageId %>,
            table_id : <%= MainTableId %>,
            row_id : <%= MainRowId %>,
            menu_id : <%= MenuId %>,
            doc_text : '<%= GetAliasTitle() %>',
            doc_url : '<%= DocUrl %>'
        };

        m_FlowBtns.push(toolbar.add('-'));
        
        var viewCfg  = btnConfig[0];

        
        var btn = flow_viewBtnCretae(viewCfg,userData);

        toolbar.add(btn);

        m_FlowBtns.push(btn);


        for (var i = 1; i < btnConfig.length; i++) {
            
            var cfg = btnConfig[i];

            console.debug('cfg = ', cfg);

            if('select-flow' == cfg.role){
                
                //for (var i = 1; i <= 4; i++) {
                //    if(cfg['ext_col_' + i]){
                //        userData["ext_col_" + i] = cfg["ext_col_" + i];
                //    }
                //}

                console.debug("--------------- cfg ",cfg);
                console.debug("--------------- data ", userData);

                var btn = createFlowSelect(cfg,userData);

                toolbar.add(btn);
                m_FlowBtns.push(btn);
            }
            else {
                userData.bill_type = cfg.bill_type;
                userData.bill_code_field = cfg.bill_code_field;

                
                console.debug("xxxxxxx cfg",cfg);
                console.debug("xxxxxxxxx",userData);

                var btn = createFlowBtn(cfg,userData);

                toolbar.add(btn);
            
                m_FlowBtns.push(btn);
            }
        }

        
        toolbar.updateLayout();

    }

    /**
    * 创建选择的按钮
    *
    */
    function createFlowSelect(cfg,userData){

        
        var btn = Mini2.create('Mini2.ui.toolbar.Button',{
            text : cfg.text,


            flow_items : cfg.flow_items,

            icon : '/res/icon/Move_Up.png',


            click:function(){
                var me = this,
                    flow_items = me.flow_items;
                
                var str = '<h4>请选择一个流程进行提交:</h4>';

                for (var i = 0; i < flow_items.length; i++) {
    
                    var item = flow_items[i];

                    str += Mini2.join([
                        '<p class="text-center">',
                            '<button class="btn btn-success" data-flow="',item.code,'" ',
                            
                                'data-node-code="',item.cur_node_code,'" ',
                                'data-line-code="',item.cur_line_code,'" ',
                                'data-bill-code-field="',item.bill_code_field,'" ',
                                'data-bill-type="',item.bill_type,'" ',

                                'data-ext-col-1="', item.ext_col_1 ,'" ',
                                'data-ext-col-2="', item.ext_col_2 ,'" ',
                                'data-ext-col-3="', item.ext_col_3 ,'" ',
                                'data-ext-col-4="', item.ext_col_4 ,'" ',
                                
                                'data-ext-col-text-1="', item.ext_col_text_1 ,'" ',
                                'data-ext-col-text-2="', item.ext_col_text_2 ,'" ',
                                'data-ext-col-text-3="', item.ext_col_text_3 ,'" ',
                                'data-ext-col-text-4="', item.ext_col_text_4 ,'" ',
                            
                            '>', item.text  ,'<br/>', item.code ,'</button>',
                        '</p>'
                    ]);
                                        
                }

                var win = Mini2.createTop('Mini2.ui.Window',{
                    text: '选择流程',
                    iframe:false,
                    width: 600,
                    height: 400,
                    
                    userData : userData,

                    contentTpl:[
                        '<div dock="full" class="container-fluid" style="overflow: auto;">',            
                            
                            str,

                        '</div>'
                    ],

                    mode:true,
                    
                    render: function () {
                        "use strict";
                        var me = this,
                            contentEl;

                        me.renderBase();

                        contentEl = me.contentEl;

                        var btns = $(contentEl).find('.btn');

                        $(btns).click(function(){
                            var itemEl = $(this);

                            var flowCode = itemEl.attr('data-flow');
                            var curNodeCode = itemEl.attr('data-node-code');
                            var curLineCode = itemEl.attr('data-line-code');
                            var billCodeField = itemEl.attr('data-bill-code-field');
                            var billType  = itemEl.attr('data-bill-type');

                            var extData = {};

                            for (var i = 1; i <= 4; i++) {
                                extData['ext_col_' + i] = itemEl.attr('data-ext-col-' + i);
                                extData['ext_col_text_' + i] = itemEl.attr('data-ext-col-text-' + i);
                            }

                            me.close();

                            flow_showStep('submit', me.userData, flowCode,curNodeCode, 
                                curLineCode, billCodeField, billType,extData );
                        });
                    }
                });

                win.show();


            }
        });
        

        return btn;
    }

    function flow_showStep(action_type, ud, cur_flow_code, cur_node_code,
        line_code, billCodeField, billType, extData){
        
        console.debug('---------------- extData = ',extData);

        var urlParam = {
            'action_type':  action_type,

            'page_id':      ud.page_id,
            'table_id':     ud.table_id,
            'row_id':       ud.row_id,
            'menu_id':      ud.menu_id,
            'doc_text' :    ud.doc_text,
            'doc_url' :     ud.doc_url,

            'bill_type' :   billType || ud.bill_type,
            'bill_code_field': billCodeField || ud.bill_code_field,

            'cur_flow_code': cur_flow_code,
            'cur_node_code' : cur_node_code,
            'line_code':    line_code
        };

        if(extData){
            for (var i = 1; i <= 4; i++) {
            
                if(extData["ext_col_" + i]){
                    urlParam["ext_col_" + i] = extData["ext_col_" + i];
                }

                if(extData["ext_col_text_" + i]){
                    urlParam["ext_col_text_" + i] = extData["ext_col_text" + i];
                }
            }
        }

        console.debug("--------------------- extData",extData);

        console.debug("ud = ",ud);
        console.debug("urlParams = ", urlParam);

        var url = Mini2.urlAppend('/App/InfoGrid2/View/OneForm/FlowStep.aspx',urlParam,true);


        var frm = Mini2.createTop('Mini2.ui.Window',{
            mode : true,
            width: 600,
            height:600,
            url: url
        });

        frm.show();

        frm.formClosing(function(e){
            flow_BtnChagned();

            widget1.submit($('form'), {
                command: 'GoRefresh'
            });
        });
    }


    function createFlowBtn(cfg, userData){


        console.debug("cfg = ",cfg);
        console.debug("userData = ",userData);

        var btn = Mini2.create('Mini2.ui.toolbar.Button',{
            text : cfg.text,

            userData:userData,                
            action_type: cfg.action_type || 'submit',

            line_code: cfg.code,
            cur_flow_code : cfg.cur_flow_code,
            cur_node_code: cfg.cur_node_code,

            bill_type: cfg.bill_type,
            bill_code_field:cfg.bill_code_field,

            ext_col_1 : cfg.ext_col_1,
            ext_col_2 : cfg.ext_col_2,
            ext_col_3 : cfg.ext_col_3,
            ext_col_4 : cfg.ext_col_4,

            ext_col_text_1 : cfg.ext_col_text_1,
            ext_col_text_2 : cfg.ext_col_text_2,
            ext_col_text_3 : cfg.ext_col_text_3,
            ext_col_text_4 : cfg.ext_col_text_4,

            click: function(){
                var me = this,
                    ud = me.userData;
                
                var itemEl = $(this);

                var extData = {};

                for (var i = 1; i <= 4; i++) {
                    extData['ext_col_' + i] = me['ext_col_' + i];
                    extData['ext_col_text_' + i] = me['ext_col_text_' + i];
                }

                flow_showStep(me.action_type, ud, me.cur_flow_code, me.cur_node_code, 
                    me.line_code, me.bill_code_field, me.bill_type, extData);

            }
        });

        return btn;
    }


    $(document).ready(function(){

        flow_BtnChagned();

    });



    




</script>

<!-- 附件的脚本 -->
<script>


    //这是批量上传文章按钮的事件
    $(document).ready(function () {


        var m_biz_file = <%= GetFileList() %>;

        var m_table_name = '<%= m_table_naem_json %>';
        var m_table_id = <%= m_table_id_json %>;
        var m_row_id = <%=m_row_id_json %>;



        var uploader = WebUploader.create({

            // swf文件路径
            swf: '/Core/scripts/webuploader-0.1.5/Uploader.swf',

            // 文件接收服务端。
            server: '/View/Biz/Handle/UploaderFileHandle.ashx?action=BATCH_UPLOAD&table_type=table&table_name='+m_table_name+"&table_id="+m_table_id+"&row_id="+m_row_id,

            // 选择文件的按钮。可选。
            // 内部根据当前运行是创建，可能是input元素，也可能是flash.
            pick: {
                id: '#batchUploadBtn',
                multiple: true

            },
            // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
            resize: false,
            auto: true,
            //是否可以上传相同文件
            duplicate: true
        });


        var my_vue = new Vue({
            el: '#Enclosure_list',
            data: {
                items:m_biz_file
            },
            methods:{
                //删除附件
                delete_item:function(obj){

                    var me = this;


                    Mini2.Msg.confirm("","是否要删除，删除不能恢复了！",function(){
                    

                        $.get("/View/Biz/Handle/UploaderFileHandle.ashx",{
                            action:'DELETE_ITEM',
                            biz_file_id:obj.BIZ_FILE_ID,
                            'table_name':m_table_name,
                            'row_id':m_row_id
                        },
                        function(result){
                        
                            var json = JSON.parse(result);

                            if(json.result != "ok"){

                                Mini2.toast(json.msg);

                                return;

                            }



                                          
                            var index = me.items.indexOf(obj);

                            me.items.splice(index,1);


                            Mini2.toast("删除附件成功了！");


                        });

                    });

                    

                }
            }
            

        });


        // 文件上传失败，显示上传出错。
        uploader.on('uploadError', function (file, reason) {

            //alert('上传失败了！');

            Mini2.toast('上传失败了！');
        });

        // 文件上传成功，给item添加成功class, 用样式标记上传成功。
        uploader.on('uploadSuccess', function (file, response) {

            if (response.result != 'ok') {
                Mini2.toast(response.msg);
                return;
            }

            my_vue.items.push(response.data);

        });

        setTimeout(function () {
            //$('#batchTxtUploadBtn').find('.webuploader-element-invisible').hide();
        }, 100);


    });



</script>

    <script src="/Core/Scripts/jquery.ui/ui/custom/jquery-ui.js"></script>

    <style type="text/css">
        .ui-sortable-handle{
            cursor:move;
        }

        .ui-sortable-handle .mi-form-item-label{
            cursor:move
        }

        .ui-sortable-helper > table {
            background-color:#FFF;
            border:2px solid #6a9f1c; 
        }

        .ui-sortable-highlight { 
            height: 1.5em; 
            line-height: 1.2em;
            width: 300px;
            border:1px dashed #fc0;
            background:#fff;
            
            float: left;

        }
    </style>

<script>
    
        
    //保存控件的位置(表单头)
    function SvaeLocalItemsForHander(){

        var targetEl = $( "#widget1_I_HanderForm1>.mi-box-inner>.mi-box-target");

        var items = targetEl.children('.mi-form-item');

        
        var json = {
            cols: []
        };


        for (var i = 0; i < items.length; i++) {
            var item = items[i];

            var col = $(item).data('me');

            json.cols.push({
                index : i,
                field: col.dataField
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

            
    //保存控件的位置(表单头)
    function SvaeLocalItemsForFooter(){

        var targetEl = $( "#widget1_I_footerForm1>.mi-box-inner>.mi-box-target");

        var items = targetEl.children('.mi-form-item');

        
        var json = {
            cols: []
        };


        for (var i = 0; i < items.length; i++) {
            var item = items[i];

            var col = $(item).data('me');

            json.cols.push({
                index : i,
                field: col.dataField
            });
            
        }

        var jsonText = Mini2.Json.toJson(json);

        console.log(jsonText);
        
        $('#table_cols_data').val(jsonText);

        widget1.submit('form', {
            action: 'PostTableColSeqFooter',
            actionPs: ''
        });
    }

    //切换设计模式
    function ChangeDesignMode(){



        var design_mode = $.query.get("design_mode");

        if(design_mode == "true"){
            design_mode = false;
        }
        else{
            design_mode = true;
        }

        var url = Mini2.urlAppend('/App/InfoGrid2/View/OneForm/FormEditPreview.aspx',{
            row_pk: $.query.get("row_pk"),
            pageId: $.query.get("pageId"),
            menu_id: $.query.get("menu_id"),
            alias_title: $.query.get("alias_title"),
            design_mode: design_mode
        });
                        
        window.location.href = url; 
    }

    $(function() {

        window.setTimeout(function(){
            //$('#footerForm1').sortable({
            //    connectWith: ".mi-form-item"
            //}).disableSelection();

            if(Mini2.designMode()){

                //顶部主表单
                var targetEl = $( "#widget1_I_HanderForm1>.mi-box-inner>.mi-box-target");
                                
                targetEl.sortable({
                    cursor: "move",
                    placeholder : 'ui-sortable-highlight',
                    revert: true,
                    opacity: 0.7
                });
                targetEl.disableSelection();


                //底部表单
                var footerEl = $( "#widget1_I_footerForm1>.mi-box-inner>.mi-box-target");
                                
                footerEl.sortable({
                    cursor: "move",
                    placeholder : 'ui-sortable-highlight',
                    revert: true,
                    opacity: 0.7
                });
                footerEl.disableSelection();


            }

        },500);

    });

</script>