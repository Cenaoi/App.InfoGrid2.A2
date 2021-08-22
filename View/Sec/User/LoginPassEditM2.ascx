<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginPassEditM2.ascx.cs" Inherits="App.InfoGrid2.Sec.User.LoginPassEditM2" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<form method="post">
<mi:Viewport runat="server" ID="viewport">
    <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="400" ItemLabelAlign="Right"
        Height="600">
        
        <mi:TextBox runat="server" ID="tbxPassOld" FieldLabel="旧密码" Type="Password" />
        <mi:TextBox runat="server" ID="tbxPassNew1" FieldLabel="新密码" Type="Password" />
        <mi:TextBox runat="server" ID="tbxpassNew2" FieldLabel="再输一次新密码"  Type="Password"  />
        
    </mi:FormLayout>
    <mi:WindowFooter ID="WindowFooter1" runat="server">
        <mi:Button runat="server" ID="SubmitBtn" Width="120" Height="26" Command="btnSave"
            Text="完成" Dock="Center" />
        <mi:Button runat="server" ID="Button2" Width="80" Height="26" OnClick="ownerWindow.close()"
            Text="取消" Dock="Center" />
    </mi:WindowFooter>
</mi:Viewport>
</form>

