<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Main_v2016.ascx.cs" Inherits="App.InfoGrid2.View.Explorer.Main_v2016" %>

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

<link href="/Core/Scripts/Mini2/Themes/theme-globel.css" rel="stylesheet" type="text/css" />
<link href="/Core/Scripts/Mini2/Themes/theme-window.css" rel="stylesheet" type="text/css" />
<% } %>
<link href="/Core/Scripts/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" />
<link href="/Core/Scripts/bootstrap/3.3.4/css/bootstrap-theme.min.css" rel="stylesheet" />
<link href="/App/BizExplorer2007/Menu2013.1.css" rel="stylesheet" type="text/css" />
 <style>
     body{
         background-color:#333333;
     }
 </style>

<div id="HomeMenu">
    加载菜单...
</div>

<mi:Viewport runat="server" ID="TopViewport" MarginTop="39" Scroll="None" >
    <mi:TabPanel runat="server" ID="layout1" Region="Center" ButtonVisible="false" Plain="true" State="Normal" TabLeft="6">
        <mi:Tab runat="server" ID="welcome1" Text="欢迎!" Closable="true" >
            <iframe src="" dock="full" style="border:none;" >

            </iframe>
        </mi:Tab>
    </mi:TabPanel>
    <mi:Panel runat="server" Height="6" Region="South" ID="topFooter">        
    </mi:Panel>
</mi:Viewport>


<script type="text/javascript">

    var tabs =  null;

    function AddTab(url, label) {

        tabs = tabs || window['widget1_I_layout1'];

        var tab = tabs.add({
            url: url,
            text: label,
            closable:true,
            iframe: true,
            scroll: 'none'
        });

        tabs.setActiveTab(tab);

        //var panel = tab.panel;

        //$(panel).attr("srcUrl", url);

    }


    function proMenuData(data, status) {

        $("#HomeMenu").html(data);

        $('#mobanwang_com li').hover(function () {
            $(this).children('ul').stop(true, true).slideDown('slow');
        }, function () {
            $(this).children('ul').stop(true, true).hide();
        });

        $('#mobanwang_com li').hover(function () {
            $(this).children('div').stop(true, true).slideDown('slow');
        }, function () {
            $(this).children('div').stop(true, true).hide();
        });

    }

    $(document).ready(function () {
        
        $.get('Menu_v2015.aspx', proMenuData);
    });

    window.Mini_IsMainWindow = true;

</script>