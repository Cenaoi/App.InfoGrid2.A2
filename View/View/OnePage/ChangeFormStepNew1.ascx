﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeFormStepNew1.ascx.cs" Inherits="App.InfoGrid2.View.OnePage.ChangeFormStepNew1" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<span>转换表单</span>
<hr />

<form>

<table align="center" style="width:400px;" border="0" >
    <tr>
        <td style="padding: 20px; border: 1px solid #CCCCCC; background-color: #FFFFFF;">

            <mi:TextBox runat="server" ID="TB1" FieldLabel="复杂表名称" LabelAlign="Right" />


        </td>
    </tr>
    <tr>
        <td align="center">
            <mi:Button runat="server" ID="SubmitBtn" Text="下一步" Width="80" Height="26" Command="GoNext"  />
        </td>
    </tr>
</table>

</form>
