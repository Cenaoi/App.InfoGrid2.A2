<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleList.ascx.cs" Inherits="App.InfoGrid2.Sec.Role.RoleList" %>
<%@ Register Assembly="EasyClick.BizWeb" Namespace="EasyClick.BizWeb.UI" TagPrefix="biz" %>
<%@ Register Assembly="EasyClick.Web.Mini" Namespace="EasyClick.Web.Mini" TagPrefix="mi" %>


<%@ Register assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="System.Web.UI.HtmlControls" tagprefix="cc1" %>
<% if (false)
   { %>
<link href="/App_Themes/Blue/common.css" rel="stylesheet" type="text/css" />
<link href="/App_Themes/Vista/table.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>
<link href="/Core/Scripts/ui-lightness/jquery-ui-1.8.6.custom.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
<script src="/Core/Scripts/validate/jquery.validate-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>
<script src="/Core/Scripts/jquery.form.js" type="text/javascript"></script>
<% } %>
<form method="post" action="" >
   
<mi:ToolBar runat="server" ID="toolbar1">
    <mi:ToolBarHr />
    <mi:ToolBarButton Text="新增行" Command="NewBtn_Click" OnClick="widget1.submit(this)" Icon="/res/icon/add.png" />
    <mi:ToolBarButton Text="保存" Command="Save_Click" OnClick="widget1.submit(this)" Icon="$save" />
    <mi:ToolBarButton Text="刷新" Command="ResetBtn_Click" OnClick="widget1.submit(this)" Icon="/res/icon/recur.png" />
    <mi:ToolBarHr />
    <mi:ToolBarButton Text="删除行" ID="DeleteBtn" Command="DeleteItems" OnClick="widget1.submit(this)" Icon="/res/icon/del.png" />

</mi:ToolBar>
<mi:DataGridView ID="DataGridView1" runat="server" class="table1" 
    cellspacing="0" rules="all" DataKeys="SEC_ROLE_ID" 
    FilterField="False"
    ScrollBars="Horizontal" ViewStateMode="Inherit" AllowSorting="true" >
    <Columns>
        <mi:RowHeadersField />
        <mi:CheckBoxField DataField="SEC_ROLE_ID" Name="Check_SEC_ROLE_ID" />
        <mi:ButtonField Text="分配权限" ButtonType="Link" ItemAlign="Center" Width="60" CommandName="SetupSec" CommandParam="{$T.SEC_ROLE_ID}" />
        <mi:EditorTextCell DataField="ROLE_CODE" HeaderText="代码" />
        <mi:EditorTextCell DataField="TEXT" HeaderText="角色名称" />
        <mi:EditorTextCell DataField="DESCRIPTION" HeaderText="备注" Width="260" />
        <mi:EmptyField />
    </Columns>
    <EmptyDataText>没有找到记录! 请您重新输入查询条件,进行查询!</EmptyDataText>
    <Pagination IsAjax="True" Command="GoPage" ClassName="flickr" RowCount="20" 
        UrlFormat="PageIndex={0}" ButtonCount="10" ID="DataGridView1_Page"></Pagination>
</mi:DataGridView>
</form>