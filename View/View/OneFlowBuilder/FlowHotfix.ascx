<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlowHotfix.ascx.cs" Inherits="App.InfoGrid2.View.OneFlowBuilder.FlowHotfix" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<!--流程修补-->

<form method="post">

    <mi:Button runat="server" ID="hotfixBtn" Text="修补空白单号" Command="GoFullBlankCode" />

</form>