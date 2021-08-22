<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileUpdateForm.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin2.Plan.FileUpdateForm" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<% if (false)
   { %>
<link href="/App_Themes/Blue/common.css" rel="stylesheet" type="text/css" />
<link href="/App_Themes/Vista/table.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>
<link href="/Core/Scripts/ui-lightness/jquery-ui-1.8.6.custom.css" rel="stylesheet"
    type="text/css" />
<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
<script src="/Core/Scripts/validate/jquery.validate-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>
<script src="/Core/Scripts/Mini/_CodeHelp.js" type="text/javascript"></script>
<% } %>

    <!--引入CSS-->
    <link rel="stylesheet" type="text/css" href="/Core/Scripts/webuploader-0.1.5/webuploader.css" />

    <!--引入JS-->
    <script src="/Core/Scripts/webuploader-0.1.5/webuploader.js"></script>

<form action="" method="post">

    <mi:Viewport runat="server" ID="viewport1">

        <mi:FormLayout runat="server" ID="panel1" Region="Center" Padding="20"   >
           

            <mi:FileUpload runat="server" ID="fieldUpdate1" FieldLabel="文件上传" PluginType="OtherFile" LabelAlign="Right"  />
            <div class="mi-newline"></div>
            <mi:Label runat="server" ID="fileName1"  Value="" FieldLabel="文件名" LabelAlign="Right" />

        </mi:FormLayout>

        <mi:WindowFooter runat="server" ID="footer1" Padding="8">

            <mi:Button runat="server" ID="okBtn" Text="确认" Width="80" Dock="Center" Command="GoOkClick" />
            
            <mi:Button runat="server" ID="cancelBtn" Text="取消" Width="80" Dock="Center" OnClick="ownerWindow.close()" />

        </mi:WindowFooter>

    </mi:Viewport>

    <mi:Hidden runat="server" ID="H_FILE_PATH" />

</form>