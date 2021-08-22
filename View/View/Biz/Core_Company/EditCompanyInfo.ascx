<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCompanyInfo.ascx.cs"
    Inherits="App.InfoGrid2.View.Biz.Core_Company.EditCompanyInfo" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>



<form method="post">
<mi:Viewport runat="server" ID="viewport">
    <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="400" ItemLabelAlign="Right" Region="Center" AutoSize="true">
              

        <mi:Label runat="server" ID="compIdLab" FieldLabel="ID" DataField="BIZ_C_COMPANY_ID" />
        <mi:TextBox runat="server" ID="tbxCode" FieldLabel="代  码" DataField="CODE" />
        <mi:TextBox runat="server" ID="tbxShortName" FieldLabel="简 称" DataField="SHORT_NAME" />
        <mi:TextBox runat="server" ID="tbxFullName" FieldLabel="全 称" DataField="FULL_NAME" />
        <mi:TextBox runat="server" ID="tbxAddrsss" FieldLabel="地 址" DataField="ADDRSSS" />
        <mi:TextBox runat="server" ID="tbxPortcode" FieldLabel="邮 编" DataField="PORTCODE" />
        <mi:TextBox runat="server" ID="tbxTel" FieldLabel="电 话" DataField="TEL" />
        <mi:TextBox runat="server" ID="tbxFax" FieldLabel="传 真" DataField="FAX" />
        <mi:TextBox runat="server" ID="tbxWebsite" FieldLabel="Http://" DataField="WEBSITE" />
        <mi:TextBox runat="server" ID="tbxEmail" FieldLabel="Email" DataField="EMAIL" />
        <mi:TextBox runat="server" ID="tbxBankName" FieldLabel="开户银行" DataField="BANK_NAME" />
        <mi:TextBox runat="server" ID="tbxBankAccount" FieldLabel="银行账号" DataField="BANK_ACCOUNT" />
        <mi:TextBox runat="server" ID="tbxCorporationTax" FieldLabel="税 号" DataField="CORPORATION_TAX" />


    </mi:FormLayout>
    <mi:WindowFooter ID="WindowFooter1" runat="server">
        <mi:Button runat="server" ID="SubmitBtn" Width="120" Height="26" Command="btnSave"
            Text="完成" Dock="Center" />


    </mi:WindowFooter>
</mi:Viewport>
</form>

<script>




</script>