﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OneViewCopy.ascx.cs" Inherits="App.InfoGrid2.View.OneView.OneViewCopy" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" id="form1" method="post">


    
<div class="mi-newline">
    <div class="mi-newline-text">创建 - 工作表</div>
</div>

<table align="center" style="width:400px;" border="0" >
    <tr>
        <td style="padding: 20px; border: 1px solid #CCCCCC; background-color: #FFFFFF;">
            <mi:FormLayout runat="server" ID="formLayout1" Dock="None" AutoSize="true" ItemLabelAlign="Right">
                <mi:TextBox runat="server" ID="TB1" FieldLabel="工作表名称" LabelAlign="Right" />
            </mi:FormLayout>
        </td>
    </tr>

    <tr>
        <td align="center">
            
                    <mi:Button runat="server" ID="SubmitBtn" Text="下一步" Width="80" Height="26" Command="GoNext"  />
        </td>
    </tr>
</table>

</form>