<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectExcel2.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.ImportExcel.SelectExcel2" %>
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

    <!--引入CSS-->
    <link rel="stylesheet" type="text/css" href="/Core/Scripts/webuploader-0.1.5/webuploader.css" />

    <!--引入JS-->
    <script src="/Core/Scripts/webuploader-0.1.5/webuploader.js"></script>

<form action="">

    <div style="height:20px;"></div>

    <mi:FileUpload runat="server" ID="fieldUpdate1" FieldLabel="文件上传" PluginType="OtherFile"  />

</form>