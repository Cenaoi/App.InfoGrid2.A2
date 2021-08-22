<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FunList.ascx.cs" Inherits="App.InfoGrid2.Sec.FunDefine.FunList" %>
<%@ Register Assembly="EasyClick.BizWeb" Namespace="EasyClick.BizWeb.UI" TagPrefix="biz" %>
<%@ Register Assembly="EasyClick.Web.Mini" Namespace="EasyClick.Web.Mini" TagPrefix="mi" %>
<%@ Register assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="System.Web.UI.HtmlControls" tagprefix="cc1" %>
<!--出口舱单-->

<% if (false)
   { %>
<link href="/App_Themes/Mini/mini-1.0.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>
<link href="/Core/Scripts/ui-lightness/jquery-ui-1.8.6.custom.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
<script src="/Core/Scripts/validate/jquery.validate-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>
<% } %>

<form method="post">
    <a href="#" id="TmpButton" style="display:none; background-color:transparent;">...</a>
    <mi:ToolBar ID="ToolBar1" runat="server">
        <mi:ToolBarTitle Text="方法权限" />
        <mi:ToolBarButton Text="创建" Command="CreateFun" OnClick="widget1.submit(this);" />
        <mi:ToolBarButton Text="创建常用功能" Command="CreateCommonFun" OnClick="widget1.submit(this);" />
       
        <mi:ToolBarButton Text="保存" Command="SaveFun" OnClick="widget1.submit(this);" />
        <mi:ToolBarButton Text="刷新" Command="RefreshFun" OnClick="widget1.submit(this);" />
        <mi:ToolBarHr />
        <mi:ToolBarButton Text="删除" Command="DeleteFun" OnClick="widget1.submit(this);" />
        <mi:ToolBarHr />
        <mi:ToolBarButton Text="移上" Command="MoveUp" OnClick="widget1.submit(this);" />
        <mi:ToolBarButton Text="移下" Command="MoveDown" OnClick="widget1.submit(this);" />
        <mi:ToolBarButton Text="初始化索引" Command="InitSeq" OnClick="widget1.submit(this);" Align="Right" />
    </mi:ToolBar>

    <mi:DataGridView runat="server" ID="DataGridView1" DataKeys="SEC_FUN_DEF_ID" class="table1" 
        cellspacing="0" rules="all" ScrollBars="Horizontal" PagerVisible="False" SortExpression="SEQ ASC,SEC_FUN_DEF_ID ASC">
        <Columns>
            <mi:RowHeadersField />
            <mi:CheckBoxField DataField="SEC_FUN_DEF_ID" Name="Check_SEC_FUN_DEF_ID" />
            <mi:BoundField DataField="SEC_FUN_DEF_ID" HeaderText="ID" Width="60" ItemAlign="Center" /> 
            <mi:EditorSelect2Cell DataField="CODE" HeaderText="代码" Width="200">                

            </mi:EditorSelect2Cell>
            <mi:EditorTextCell DataField="TEXT" HeaderText="描述" />  

            <mi:EmptyField />
        </Columns>
    
    </mi:DataGridView>


</form>