<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainFrm.ascx.cs" Inherits="App.InfoGrid2.View.Biz.JLSLBZ.Scjhxqzx.MainFrm" %>
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
<link rel="stylesheet" type="text/css" href="/Core/Scripts/jstree/3.0.2/themes/default/style.css" />
<script src="/Core/Scripts/jstree/3.0.2/jstree.js" type="text/javascript"></script>
<style type="text/css">
    .page-head
    {
         font-size:26px;
         font-weight:bold;
    }
</style>
<form action="" method="post">

<mi:Store ID="Store1" runat="server" Model="UT_071" IdField="ROW_IDENTITY_ID" SortField="COL_14" PageSize="50" ReadOnly="true">
    <StringFields>ROW_IDENTITY_ID, COL_1, COL_2</StringFields>
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
</mi:Store>

<mi:Viewport runat="server" ID="viewport1">
    <mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="10" Region="North">

        <mi:Label runat="server" ID="headLab" Value="生产计划需求分析中心" HideLabel="true" Dock="Center" Mode="Transform" />
    </mi:Panel>
    
    <mi:Panel runat="server" ID="Panel2" Width="200" PaddingTop="0" Region="West" Scroll="None">
        <mi:Panel runat="server" Height="35">
            
            <table>
                <tr>
                    <td><mi:TextBox runat="server" ID="tbxName"  FieldLabel="客户名称"  HideLabel="true" /></td>
                    <td><mi:Button  runat="server" ID="btnton1" Text="查询" Command="SelectUT071"/></td>
                    <td><mi:Button  runat="server" ID="Button2" Text="全部" Command="GoShowAll"/></td>
                </tr>

            </table>

        </mi:Panel>
        <mi:Table runat="server" ID="table1" StoreID="Store1" Dock="Full" Region="West" PagerVisible="false" >
            <Columns>
                <mi:RowNumberer />
                <mi:BoundField HeaderText="客户名称" DataField="COL_2" Width="140" />
                <mi:BoundField HeaderText="客户编码" DataField="COL_1" Width="80" />
            </Columns>
        </mi:Table>
    </mi:Panel>

    <mi:Panel ID="Panel1" runat="server" Dock="Full" Region="Center" Scroll="None">
        <iframe dock="full" region="center" id="iform1" frameborder="0"
            src="">
    
            

        </iframe>
    </mi:Panel>
</mi:Viewport>


</form>