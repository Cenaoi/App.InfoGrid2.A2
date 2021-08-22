<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginPassEdit.ascx.cs" Inherits="App.InfoGrid2.Sec.User.LoginPassEdit" %>


<%@ Register Assembly="EasyClick.BizWeb" Namespace="EasyClick.BizWeb.UI" TagPrefix="biz" %>
<%@ Register Assembly="EasyClick.Web.Mini" Namespace="EasyClick.Web.Mini" TagPrefix="mi" %>


<%@ Register assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="System.Web.UI.HtmlControls" tagprefix="cc1" %>
<% if (false)
   { %>
   <link href="/App_Themes/Mini/mini-1.0.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>
<link href="/Core/Scripts/ui-lightness/jquery-ui-1.8.6.custom.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
<script src="/Core/Scripts/validate/jquery.validate-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>
<script src="/Core/Scripts/jquery.form.js" type="text/javascript"></script>
<% } %>
<form method="post" action="" >
   
   <mi:TableLayoutPanel runat="server" ID="TableLayoutPanel1">
    <Fields>
        <mi:Password runat="server" ID="OldPass1" HeaderText="旧密码" DBField="LOGIN_PASS" Value="" />
        <mi:Password runat="server" ID="NewPass1" HeaderText="新密码" DBField="LOGIN_PASS" Value="" />
        <mi:Password runat="server" ID="NewPass2" HeaderText="再输入一次" DBField="LOGIN_PASS" Value="" />

        <mi:Button runat="server" ID="SubmitBtn" Command="Submit" class="Mini-Button">提交</mi:Button>
    </Fields>    
   </mi:TableLayoutPanel>

</form>
