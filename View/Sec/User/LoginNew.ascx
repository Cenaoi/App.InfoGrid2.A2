<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginNew.ascx.cs" Inherits="App.InfoGrid2.Sec.User.LoginNew" %>

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
        <mi:DropDownList runat="server" ID="ModeID_DDL" HeaderText="权限模式" DBField="SEC_MODE_ID" Value="2">
            <mi:ListItem Value="0" Text="0-禁止登陆" />
            <mi:ListItem Value="1" Text="1-按角色配置" />
            <mi:ListItem Value="2" Text="2-按自定义配置" />
        </mi:DropDownList>
        <mi:TextBox runat="server" ID="TextBox1" HeaderText="真实姓名" DBField="TRUE_NAME" />
        <mi:TextBox runat="server" ID="TextBox2" HeaderText="登陆账号" DBField="LOGIN_NAME" />
        <mi:TextBox runat="server" ID="TextBox3" HeaderText="登陆密码" DBField="LOGIN_PASS" Value="123456" />

        <mi:Button runat="server" ID="SubmitBtn" Command="Submit" class="Mini-Button">提交</mi:Button>
    </Fields>    
   </mi:TableLayoutPanel>

</form>