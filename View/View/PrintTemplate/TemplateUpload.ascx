<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplateUpload.ascx.cs" Inherits="App.InfoGrid2.View.PrintTemplate.TemplateUpload" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<script src="/Core/Scripts/webuploader-0.1.5/webuploader.min.js"></script>
<form action="" id="form1" method="post">



    <mi:FileUpload runat="server" ID="FileUpload1" PluginType="OtherFile" FieldLabel="请上传文件"  ></mi:FileUpload>

</form>




