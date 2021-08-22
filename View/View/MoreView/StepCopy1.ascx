<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StepCopy1.ascx.cs" Inherits="App.InfoGrid2.View.MoreView.StepCopy1" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" id="form1" method="post">

<span>拷贝 - 工作表</span>
<hr />


<table align="center" style="width:400px;" border="0" >
    <tr>
        <td style="padding: 20px; border: 1px solid #CCCCCC; background-color: #FFFFFF;">
            <mi:TextBox runat="server" ID="TB1" FieldLabel="工作表名称" LabelAlign="Right" />
        </td>
    </tr>
    <tr>
        <td align="center">
            <mi:Button runat="server" ID="SubmitBtn" Text="确定拷贝" Width="80" Height="26" Command="GoNext"  />
        </td>
    </tr>
</table>

</form>

<script>

    function wSubmit(sender) {
        var me = sender;

        widget1.submit(me.el, {
            command: me.command,
            commandParams: me.commandParams
        });
    }

</script>   
