<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="View._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>Insert title here</title>
</head>
<body style="text>
    <form action="login" method="post">
    name :
    <input type="text" name="name" />
    <br />
    password:
    <input type="password" name="pwd" />
    <br />
    <input type="submit" value="submit" />
    </form>

    <asp:DropDownList ID="DropDownList1" runat="server" 
        onselectedindexchanged="DropDownList1_SelectedIndexChanged" 
        style="height: 19px">
    </asp:DropDownList>

    <asp:Label runat="server" ID="ddd" Text="sss发送的飞洒发的啥发">
    </asp:Label>

    <asp:Literal runat="server" ID="sss" 
        Text="&lt;a href=&quot;#&quot;&gt;dsds&lt;/a&gt;"></asp:Literal>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &lt;&gt;=;/</p>
</body>
</html>
