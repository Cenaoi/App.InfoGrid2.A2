<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadTemplate.ascx.cs" Inherits="App.InfoGrid2.View.PrintTemplate.DownloadTemplate" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<form action="" id="form1" method="post">

<table align="center" style="width:400px;" border="0" >
    <tr>
        <td style="padding: 20px; border: 1px solid #CCCCCC; background-color: #FFFFFF;">
            <mi:ComboBox runat="server" ID="cbxTemplate" FieldLabel="模板" TriggerMode="None" >
            </mi:ComboBox>
        </td>
    </tr>
    <tr>
        <td align="center">
            <mi:Button runat="server" ID="SubmitBtn" Text="导出" Width="80" Height="26" Command="InputOut"  />
            <mi:Button runat="server" ID="Button1" Text="管理模板" Width="80" Height="26" Command="GoEdit"  />
        </td>
    </tr>
</table>

</form>


