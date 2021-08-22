<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintSetup.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.Prints.PrintSetup" %>
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

<style type="text/css">
    .page-head
    {
         font-size:26px;
         font-weight:bold;
    }
</style>

<form action="" method="post"> 

    <mi:FormLayout runat="server" ID="HanderForm1" Dock="Top" MinHeight="300" ItemWidth="300" PaddingTop="10"  
                ItemLabelAlign="Right" Region="North" Height="600" FlowDirection="LeftToRight" AutoSize="true" >
        
        <mi:RadioGroup runat="server" ID="modeRG1" FieldLabel="打印模式" DefaultValue="all" >
            <mi:ListItem Text="全部" Value="all" />
            <mi:ListItem Text="部分" Value="area" />
        </mi:RadioGroup>

        <mi:NumRangeBox runat="server" ID="range1" StartValue="1" EndValue="10" FieldLabel="打印范围" />

        <div class="mi-newline"><div class="mi-newline-border1"></div></div>

        <div style="padding-left:100px;">
            <mi:Button runat="server" ID="printBtn" Text="打印" Width="80" Command="GoPrint" Dock="Center" />
        </div>
    </mi:FormLayout>

</form>