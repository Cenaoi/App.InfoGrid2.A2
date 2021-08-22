<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MViewStepEdit1.ascx.cs" Inherits="App.EC52Demo.View.ViewSetup.MViewStepEdit1" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>



<form action="" id="form2" method="post">

<span>编辑 - 视图表</span>
<hr />


<table align="center" style="width:400px;" border="0" >
    <tr>
        <td style="padding: 20px; border: 1px solid #CCCCCC;">
            <mi:TextBox runat="server" ID="tbxViewName" FieldLabel="视图名称" LabelAlign="Right" />
        </td>
    </tr>
    <tr>
        <td align="center">
            <mi:Button runat="server" ID="SubmitBtn" Text="下一步" Width="80" Height="26" Command="GoNext"  />
        </td>
    </tr>
</table>
<mi:Hidden ID="HID" runat="server" />
</form>
