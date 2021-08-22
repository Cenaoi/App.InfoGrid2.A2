<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DwgSoundCode.ascx.cs" Inherits="App.InfoGrid2.View.MoreActionBuilder.DwgSoundCode" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<link href="/Core/Scripts/codemirror/codemirror-5.21.0/codemirror.css" rel="stylesheet" />
<script src="/Core/Scripts/codemirror/codemirror-5.21.0/codemirror.js"></script>
<script src="/Core/Scripts/codemirror/codemirror-5.21.0/mode/clike/clike.js"></script>

<form action="" method="post">

<mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="0" Padding="0">

    <mi:Toolbar runat="server" ID="toolbar1">
        <mi:ToolBarTitle />
        <mi:ToolBarButton Text="复制" />
    </mi:Toolbar>

    <mi:FormLayout runat="server" ID="formLayout1" Region="Center">
        
        <mi:CodeEditor runat="server" ID="codeEditor1" Dock="Full" HideLabel="true" />

    </mi:FormLayout>
    <mi:WindowFooter runat="server" ID="footer1">
        <mi:Button runat="server" ID="Button1" Text="关闭" Width="80" Height="26" Dock="Center" OnClick="ownerClose()" />
    </mi:WindowFooter>

</mi:Viewport>
</form>


<script>
    "use strict";

    function ownerClose() {
        if (ownerWindow) {
            ownerWindow.close();
        }
    }

</script>