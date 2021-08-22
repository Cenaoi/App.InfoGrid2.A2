<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResetLogin.ascx.cs" Inherits="App.InfoGrid2.View.Explorer.ResetLogin" %>

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

<link href="/Core/Scripts/Mini2/ex_plugins/webui-popover/jquery.webui-popover.css" rel="stylesheet" />
<script src="/Core/Scripts/Mini2/ex_plugins/webui-popover/jquery.webui-popover.js"></script>

<form method="post">
<mi:Viewport runat="server" ID="viewport1">

    <mi:FormLayout runat="server" ID="panel1" Region="Center" ItemWidth="300">

       <mi:TextBox runat="server" ID="loginNameTB" LabelAlign="Right" FieldLabel="账号" />

       <mi:TextBox runat="server" ID="passTB" LabelAlign="Right" FieldLabel="密码" Type="Password" />

    </mi:FormLayout>

    <mi:WindowFooter runat="server" ID="footer1">

        <mi:Button runat="server" ID="okBtn" Command="GoSubmit" Text="提交" Scale="Medium" Width="100" Dock="Center" />

        <mi:Button runat="server" ID="cancelBtn" Command="GoCancel" Text="取消" Scale="Medium" Width="100" Dock="Center" />
    </mi:WindowFooter>

</mi:Viewport>
 
</form>
