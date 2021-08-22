<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewPreview.ascx.cs" Inherits="App.InfoGrid2.View.OneView.ViewPreview" %>
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
<mi:Store runat="server" ID="store1" Model="" IdField="" PageSize="20">

</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">
    <mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="10">
        <mi:Label runat="server" ID="headLab" Value="单号" HideLabel="true" Dock="Center" Mode="Transform" />
    </mi:Panel>
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:FormLayout runat="server" ID="searchForm" Dock="Top" Region="North" FlowDirection="TopDown"
            ItemWidth="300" ItemLabelAlign="Right" ItemClass="mi-box-item" Layout="HBox"
            StoreID="store1" FormMode="Filter" Scroll="None">

        </mi:FormLayout>
        <mi:Toolbar ID="Toolbar1" runat="server">
            

            <mi:ToolBarButton Text="新增" OnClick="ser:store1.Insert()" />
            <mi:ToolBarButton Text="保存" OnClick="ser:store1.SaveAll()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:store1.Delete()" />

            <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm.toggle()" />

            <mi:ToolBarButton Text="导出" Icon="/res/file_ico/excel.png" Command="ToExcel" />


            <%--
            <mi:ToolBarButton Text="列定义" Align="Right" Command="StepEdit2" /> 比工作表少了这个按钮
            <mi:ToolBarButton Text="列设置" Align="Right" Command="StepEdit3" />
            <mi:ToolBarButton Text="列高级设置" Align="Right" Command="StepEdit4" />
             --%>
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" >
            <Columns>
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>
<% if ( EC5.SystemBoard.EcContext.Current.User.Roles.Exist(EC5.IG2.Core.IG2Param.Role.BUILDER) )
    { %>
<div id="SwitchPanel" style="width:640px;height:40px;border: 1px solid #C0C0C0;background-color: #FFFFFF;">
    <div style="margin:8px 24px 8px 8px; text-align:right;">
        <mi:Button runat="server" ID="Button4" OnClick="showFlowSetup()" Text="流程设置" />
        <mi:Button runat="server" ID="Button3" Command="ToolbarSetup" Text="工具栏设置" />
        <mi:Button runat="server" ID="Button1" Command="GoValidSetup" Text="验证设置" />
        <mi:Button runat="server" ID="StepEdit3Btn" Command="StepEdit3" Text="列定义" />
        <mi:Button runat="server" ID="StepEdit4Btn" Command="StepEdit4" Text="列高级设置" />
        <mi:Button runat="server" ID="Button2" Command="StepEdit5_DialogMode" Text="模式窗体设置" />

    </div>
</div>
<script type="text/javascript">

    $(document).ready(function () {

        var ps = Mini2.create('Mini2.ui.extend.PullSwitch', {
            panelId: 'SwitchPanel'
        });

        ps.render();
    });
</script>
<% } %>

<%= GetDisplayRule() %>

</form>
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

        var win = Mini2.createTop('Mini2.ui.Window', {
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







    //显示弹出提示框
    function showSelectWinForTable(owner) {

        var me = owner,
            tag = me.tag;

        //var formPanel = me.ownerParent;
        //var store = formPanel.store;

        //var record = store.getCurrent();


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
        //win.record = record;
        win.show();

        win.formClosed(function (e) {

            console.debug("e ", e);

            selectForm_Closed(e, owner);
        });

    }

    function selectForm_Closed(e, triggerBox, record) {

        var me = this;

        if (e.result != 'ok') { return; }

        var map = e.map;

        var row = e.row;

        var newValues = {};

        var dataField = triggerBox.dataField;

        for (var i = 0; i < map.length; i++) {


            var m = map[i];
            var v = row[m.src];

            //注意: 组一行代码特殊过滤, 当前只映射一个字段, 其它作废
            if(m.to != dataField){
                continue;
            }

            try{
                var conList = widget1_I_searchForm.findByDataField(m.to);

                if(conList){
                    for (var j = 0; j < conList.length; j++) {
                        conList[j].setValue( v);
                    }
                   
                }
                else{
                    triggerBox.setValue(v);
                }
            }
            catch(ex){
                console.error(ex);
            }
            //newValues[m.to] = v;
        }

        //record.set(newValues);

    }


    

    //显示窗体
    function form_EditShow(view, cell, recordIndex, cellIndex, e, record, row) {

        var menu_id = $.query.get('menu_id');

        var id = record.getId();
        var formType = '<%= this.FormEditType %>';
        var formEditPageId = <%= this.FormEditPageID %>;
        var altis = '<%= this.FromEditAliasTitle %>';

        
        var url;
        
        if(formType == 'ONE_FORM'){
            url = $.format('/App/InfoGrid2/View/OneForm/FormEditPreview.aspx?row_pk={0}&pageId={1}&menu_id={2}&alias_title={3}', 
                id, formEditPageId, menu_id, altis);
        }
        else if(formType == 'TABLE_FORM'){
            url = $.format('/App/InfoGrid2/View/OneForm/FormOneEditPreview.aspx?row_pk={0}&pageId={1}&menu_id={2}&alias_title={3}', 
               id, formEditPageId, menu_id, altis);
        }

        //var win = Mini2.create('Mini2.ui.Window',{
        //    url : url,
        //    width : 1000,
        //    height: 768,
        //    mode:true
        //});

        //win.show();

        Mini2.EcView.show(url, altis + '-表单编辑');
    }

    function mainStoreInsert() {
        
       widget1.subMethod($('form:first'), { subName: 'store1', subMethod: 'Insert' });

    }


</script>


<script>


    function showFlowSetup(){

        var id = $.query.get('id');


        var urlStr = Mini2.urlAppend("/App/InfoGrid2/View/OneForm/FlowSetup.aspx",{
            'id': id
        });
        
        var win = Mini2.createTop('Mini2.ui.Window', {
            mode: true,
            text: '流程参数设置',
            iframe: true,
            width: 800,
            height: 600,
            url: urlStr
        });

        win.show();


    }

</script>
