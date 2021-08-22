<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MoreFileDialog.ascx.cs" Inherits="App.InfoGrid2.View.OneForm.Dialogs.MoreFileDialog" %>
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

<link href="/Core/Scripts/webuploader-0.1.5/webuploader.css" rel="stylesheet" />
<script src="/Core/Scripts/webuploader-0.1.5/webuploader.js"></script>

<form action="" method="post">

    <mi:Viewport runat="server" ID="viewport1">

        <mi:FormLayout runat="server" ID="formLayout1" Region="Center">

            <mi:MoreFileUpload runat="server" ID="moreFileUpload1" HideLabel="true" Height="500" />


        </mi:FormLayout>

        <mi:WindowFooter runat="server" ID="footer1">

            <mi:Button runat="server" ID="okBtn" Text="确认" Dock="Center" Command="GoSubmit" />

            <mi:Button  runat="server" ID="cancelBtn" Text="取消" Dock="Center" OnClick="ownerWindow.close()" />

        </mi:WindowFooter>
    </mi:Viewport>


</form>