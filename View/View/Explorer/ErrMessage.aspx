<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrMessage.aspx.cs" Inherits="App.InfoGrid2.View.Explorer.ErrMessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="font-size:12pt;">
    <form id="form1">
    <div>
        

        <div style="height:100px;">
        
        
        </div>

        <div style="border: 2px solid #CCCCCC; width:600px; height:200px; padding:20px; margin-right:auto; margin-left:auto; background-color: #FFFFCC;">
            <div runat="server" id="titleDIV" style="text-align:center; padding:10px; font-weight: bold; font-size: 16pt;" >标题</div>
            <hr size="1" />
            <br />
            <div runat="server" id="contentDIV">内容</div>
        </div>

    </div>
    </form>
</body>
</html>
