<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleNew.ascx.cs" Inherits="App.InfoGrid2.Sec.FunDefine.ModuleNew" %>
<%@ Register Assembly="EasyClick.BizWeb" Namespace="EasyClick.BizWeb.UI" TagPrefix="biz" %>
<%@ Register Assembly="EasyClick.Web.Mini" Namespace="EasyClick.Web.Mini" TagPrefix="mi" %>
<%@ Register assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="System.Web.UI.HtmlControls" tagprefix="cc1" %>
<!--出口舱单-->

<% if (false)
   { %>
<link href="/App_Themes/Mini/mini-1.0.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>
<link href="/Core/Scripts/ui-lightness/jquery-ui-1.8.6.custom.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
<script src="/Core/Scripts/validate/jquery.validate-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>
<% } %>

<form method="post">

    <mi:TableLayoutPanel runat="server" ID="TableLayoutPanel1">
        <Fields>
            
            <mi:TextBox runat="server" ID="CODE_TB" HeaderText="代码" />
            <mi:TextBox runat="server" ID="TEXT_TB" HeaderText="名称" />        


        </Fields>
    </mi:TableLayoutPanel>

    <div class="WinButtons">
        <mi:Button runat="server" ID="SaveBtn" Command="Save" class="Mini-Button">保存</mi:Button>
        <mi:Button runat="server" ID="Button1" onclick="EcView.close()" class="Mini-Button">取消</mi:Button>
    </div>
</form>