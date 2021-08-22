<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormPreview.ascx.cs" Inherits="App.InfoGrid2.View.OneForm.FormPreview" %>
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


<div runat="server" id="StoreSet">

</div>

<mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="0" Padding="0">

    <mi:Toolbar runat="server" ID="mainToolbar1">
        <mi:ToolBarButton Text="保存" />
        <mi:ToolBarButton Text="刷新" Command="GoRefresh" />
        <mi:ToolBarHr />
        <mi:ToolBarButton Text="提交" />
    </mi:Toolbar>

    <mi:Panel runat="server" ID="HeadPanel" Height="60" Scroll="None" PaddingTop="10">
        <mi:Label runat="server" ID="headLab" Value="提运单" HideLabel="true" Dock="Center" Mode="Transform"  />
    </mi:Panel>

    <mi:FormLayout runat="server" ID="HanderForm1" ItemWidth="300" PaddingTop="10" ItemLabelAlign="Right" Region="North" Height="200" FlowDirection="LeftToRight" AutoSize="true">

    </mi:FormLayout>

    <mi:Panel runat="server" ID="panel1" Padding="8"  Region="Center" Scroll="None">
        <mi:TabPanel runat="server" ID="detailTabPanel" Height="400" Region="Center" Dock="Full" ButtonVisible="false" Plain="true" >

        </mi:TabPanel>
    </mi:Panel>


    <mi:FormLayout runat="server" ID="footerForm1" ItemWidth="300" PaddingTop="8" PaddingBottom="8" ItemLabelAlign="Right" Region="South" FlowDirection="LeftToRight" AutoSize="true">

    </mi:FormLayout>

</mi:Viewport>

    
<% if (this.IsBuilder())
   { %>
<div id="SwitchPanel" style="width:200px;height:40px;border: 1px solid #C0C0C0;background-color: #FFFFFF;">
    <div style="margin:8px 24px 8px 8px; text-align:right;">

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

        EcView.show('/App/InfoGrid2/View/OneBuilder/PageBuilder.aspx?id=' + id, '表单复杂表');

    }


</script>
<%} %>
</form>

<script>
    function form_EditShow() {

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