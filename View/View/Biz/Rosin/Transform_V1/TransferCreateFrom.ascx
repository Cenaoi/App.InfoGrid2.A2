<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransferCreateFrom.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.Transform_V1.TransferCreateFrom" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>



<mi:Panel runat="server" ID="panel1"  Height="500">

    
    <mi:FormLayout runat="server" ID="formLayout1" Caption="过户" BorderWidth="1"  Width="600" ItemWidth="300" FlowDirection="LeftToRight" Dock="Center" ItemLabelAlign="Right" AutoSize="true">

        <mi:TriggerBox runat="server" ID="fromClientCode" FieldLabel="从" ButtonType="More"></mi:TriggerBox>
        <mi:TextBox runat="server" ID="formClientText" HideLabel="true" Width="200px" />

        <div class="mi-newline"></div>

        <mi:TriggerBox runat="server" ID="toCleintCode" FieldLabel="过户到" ButtonType="More"></mi:TriggerBox>

    </mi:FormLayout>


</mi:Panel>