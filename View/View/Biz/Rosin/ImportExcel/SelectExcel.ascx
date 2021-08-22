<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectExcel.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.ImportExcel.SelectExcel" %>



<%@ Register Assembly="EasyClick.BizWeb" Namespace="EasyClick.BizWeb.UI" TagPrefix="biz" %>
<%@ Register Assembly="EasyClick.Web.Mini" Namespace="EasyClick.Web.Mini" TagPrefix="mi" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="System.Web.UI.HtmlControls" TagPrefix="cc1" %>
 <script src="/Core/Scripts/SWFUpload/swfupload.js" type="text/javascript"></script>
<script src="/Core/Scripts/SWFUpload/handlers.js" type="text/javascript"></script>
<script src="/Core/Scripts/SWFUpload/fileprogress.js" type="text/javascript"></script>
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

<% } %>
<form action="" id="form1" method="post">

    <mi:SWFUpload runat="server" ID="suTest" OnUploader="suTest_Uploader" />


</form>

