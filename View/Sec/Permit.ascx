<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Permit.ascx.cs" Inherits="App.InfoGrid2.Sec.Permit" %>
<%@ Register Assembly="EasyClick.BizWeb" Namespace="EasyClick.BizWeb.UI" TagPrefix="biz" %>
<%@ Register Assembly="EasyClick.Web.Mini" Namespace="EasyClick.Web.Mini" TagPrefix="mi" %>


<%@ Register assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="System.Web.UI.HtmlControls" tagprefix="cc1" %>

<form method="post">
<span style="font-family: 微软雅黑, 新宋体; font-size: 30px">请输入通行证.</span>
<br /><br />
<mi:TableLayoutPanel runat="server" ID="table1">
    <Fields>
        <mi:Password runat="server" ID="PermitKeyTB" HeaderText="授权号" style="width:300px; font-size:30px;" />

        <mi:Button runat="server" ID="SubmitBtn" style="width:80px;height:30px;" Command="Submit">确定</mi:Button>
    </Fields>
</mi:TableLayoutPanel>
</form>