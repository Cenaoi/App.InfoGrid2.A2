<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectPreview.ascx.cs" Inherits="App.InfoGrid2.View.OneSearch.SelectPreview" %>

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
<mi:Store runat="server" ID="store1" Model="" IdField="" PageSize="20">

</mi:Store>
<mi:Viewport runat="server" ID="viewport1">       

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None"  >
        <mi:SearchFormLayout runat="server" ID="searchForm" StoreID="store1" Visible="False">

        </mi:SearchFormLayout>

        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm.toggle()" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" AutoRowCheck="true" >
            <Columns>
            </Columns>
        </mi:Table>
    </mi:Panel>
    <mi:WindowFooter runat="server" ID="footer1">
        <mi:Button runat="server" ID="OkBtn" Text="确定" Width="80" Height="26" Command="GoSubmit" Dock="Center" />
        <mi:Button runat="server" ID="Button1" Text="取消" Width="80" Height="26" Dock="Center" OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>

<script type="text/javascript">

    $(document).ready(function () {


        setTimeout(function () {


            var table1 = window.widget1_I_table1;

            table1.setKeydownEvent(function (ea) {

                var keyCode = ea.keyCode;



                if (13 == keyCode) {
                    
                    widget1.submit('form', {
                        action: 'GoSubmit'
                    });

                    Mini2.EventManager.stopEvent(ea);
                }

            });




            table1.setFocus();

        }, 500);
    });

</script>

<% if (this.IsBuilder())
   { %>
<div id="SwitchPanel" style="width:100px;height:40px;border: 1px solid #C0C0C0;background-color: #FFFFFF;">
    <div style="margin:8px 24px 8px 8px; text-align:right;">
        <mi:Button ID="Button4" runat="server"  Text="设置" Command="StepEdit" />
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
<%} %>

    
<%= GetDisplayRule() %>
</form>