<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleSelectList.ascx.cs" Inherits="App.InfoGrid2.Sec.User.RoleSelectList" %>
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
   <a href="" style="display:none;" id="TmpButton">....</a>
<mi:ToolBar runat="server" ID="toolbar1">
    <mi:ToolBarHr />
    <mi:ToolBarButton Text="确定选择" Command="OkBtn" OnClick="widget1.submit(this)" Icon="/res/icon/ok.png" />
    <mi:ToolBarButton Text="取消" Command="ClearBtn" OnClick="widget1.submit(this)"  />
</mi:ToolBar>
<mi:DataGridView ID="DataGridView1" runat="server" cellspacing="0" class="table1"
    DataKeys="SEC_ROLE_ID" FilterField="False" rules="all"
    ScrollBars="Horizontal" SecFunCode="SAVE_ROWS">
    <Columns>
        <mi:RowHeadersField HeaderText="&nbsp;" />
        <mi:CheckBoxField DataField="SEC_ROLE_ID" Name="Check_SEC_ROLE_ID" />
        
        <mi:BoundField DataField="SEC_ROLE_ID" HeaderText="ID" Width="40"/>
        <mi:BoundField DataField="TEXT" HeaderText="角色名称"/>
        
        <mi:EmptyField />
    </Columns>
    <EmptyDataText>没有找到记录! 请您重新输入查询条件,进行查询!</EmptyDataText>

<Pagination IsAjax="True" Command="GoPage" ClassName="flickr" RowCount="10" 
        UrlFormat="PageIndex={0}" ButtonCount="10" ID="DataGridView1_Page"></Pagination>
</mi:DataGridView>
   
</form>