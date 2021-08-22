<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StepManyNew1.ascx.cs" Inherits="App.InfoGrid2.View.InputExcel.StepManyNew1" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<form action="" id="form1" method="post">

<span>创建 - 导入规则</span>
<hr />


<table align="center" style="width:400px;" border="0" >
    <tr>
        <td style="padding: 20px; border: 1px solid #CCCCCC; background-color: #FFFFFF;">
            <mi:TextBox runat="server" ID="tbxName" FieldLabel="规则名" LabelAlign="Right" />
        </td>
    </tr>
    <tr>
        <td align="center">
            <mi:Button runat="server" ID="SubmitBtn" Text="下一步" Width="80" Height="26" Command="GoNext"  />
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


