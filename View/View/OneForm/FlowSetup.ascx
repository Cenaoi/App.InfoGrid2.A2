<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlowSetup.ascx.cs" Inherits="App.InfoGrid2.View.OneForm.FlowSetup" %>
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


<form action="" method="post">


<mi:Store runat="server" ID="store1" Model="IG2_TABLE" IdField="IG2_TABLE_ID"  >
    <StringFields>FLOW_ENABLED,FLOW_PARAMS</StringFields>
    <FilterParams>
        <mi:QueryStringParam Name="IG2_TABLE_ID" QueryStringField="id" DbType="Int32" />
    </FilterParams>
       
</mi:Store>


<mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="0" Padding="0">

    <mi:FormLayout runat="server" ID="formLayout1" StoreID="store1" Region="Center" ItemLabelAlign="Right" Padding="8">
        
        <mi:CheckBox runat="server" ID="checkbox1" FieldLabel="流程" TrueText="激活" FalseText="无效" DataField="FLOW_ENABLED" />

        <mi:Textarea runat="server" ID="textarea1" FieldLabel="流程参数" Height="480" DataField="FLOW_PARAMS" />

    </mi:FormLayout>

    

</mi:Viewport>

</form>
<script>




</script>