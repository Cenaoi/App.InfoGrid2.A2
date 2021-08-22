<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportPreview.ascx.cs" Inherits="App.InfoGrid2.View.ReportBuilder.ReportPreview" %>
<%@ Register Assembly="EasyClick.Web.ReportForms.V2" Namespace="EasyClick.Web.ReportForms.UI" TagPrefix="rpt" %>

<form method="post" id="form1">
    <rpt:CrossReport runat="server" ID="CrossReport1">

    </rpt:CrossReport>
</form>
