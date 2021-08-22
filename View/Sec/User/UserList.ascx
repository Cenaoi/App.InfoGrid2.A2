<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserList.ascx.cs" Inherits="App.InfoGrid2.Sec.User.UserList" %>
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

<script src="/Core/Scripts/jstree_pre1.0_stable/jquery.jstree.js" type="text/javascript"></script>

<form method="post" action="" >
   <a href="" style="display:none;" id="TmpButton">....</a>
<mi:ToolBar runat="server" ID="toolbar1">
    <mi:ToolBarHr />
    <mi:ToolBarButton Text="创建账号" Command="NewBtn_Click" OnClick="widget1.submit(this)" Icon="/res/icon/add.png" />
    <mi:ToolBarButton Text="保存" Command="Save_Click" OnClick="widget1.submit(this)" Icon="$save" />
    <mi:ToolBarButton Text="刷新" Command="ResetBtn_Click" OnClick="widget1.submit(this)" Icon="/res/icon/recur.png" />
    <mi:ToolBarHr />
    <mi:ToolBarButton Text="删除行" ID="DeleteBtn" Command="DeleteItems" OnClick="widget1.submit(this)" Icon="/res/icon/del.png" />

<%--    <mi:ToolBarHr />
    <mi:ToolBarButton Text="改角色" Command="ChangeRole"  OnClick="widget1.submit(this)" />
    <mi:ToolBarHr />
    <mi:ToolBarButton Text="配置自定义权限" Command="CreateUserSec" OnClick="widget1.submit(this)"  />
    <mi:ToolBarButton Text="删除自定义权限" Command="DeleteUserSec"  OnClick="widget1.submit(this)" />--%>
</mi:ToolBar>
<mi:DataGridView ID="DataGridView1" runat="server" class="table1" 
    cellspacing="0" rules="all" DataKeys="SEC_LOGIN_ACCOUNT_ID" 
    FilterField="False"
    ScrollBars="Horizontal" ViewStateMode="Inherit" AllowSorting="true" >
    <Columns>
        <mi:RowHeadersField />
        <mi:CheckBoxField DataField="SEC_LOGIN_ACCOUNT_ID" Name="Check_SEC_LOGIN_ACCOUNT_ID" />
        <mi:ButtonField Visible="false" Text="分配权限" ButtonType="Link" ItemAlign="Center" Width="60" CommandName="SetupSec" CommandParam="{$T.SEC_LOGIN_ACCOUNT_ID}" />
        <mi:ButtonField Text="初始化密码" ButtonType="Link" ItemAlign="Center" Width="60" CommandName="InitPass" CommandParam="{$T.SEC_LOGIN_ACCOUNT_ID}" />

        <%--<mi:ButtonField Text="自定义权限" ButtonType="Link" ItemAlign="Center" Width="60" CommandName="CreateUserSec" CommandParam="{$T.SEC_LOGIN_ACCOUNT_ID}" />--%>

        <mi:EditorTextCell DataField="BIZ_USER_CODE" HeaderText="用户编号" />
        <mi:EditorTextCell DataField="TRUE_NAME" HeaderText="真实名称" />
        <mi:EditorTextCell DataField="LOGIN_NAME" HeaderText="登录账号" ReadOnly="true" />
        <mi:EditorSelectCell DataField="SEC_MODE_ID" HeaderText="权限模式" DefaultValue="2" >
            <mi:ListItem Value="0" Text="0-禁止登陆" />
            <%--<mi:ListItem Value="1" Text="1-按角色配置" />--%>
            <mi:ListItem Value="2" Text="2-按自定义配置" />
        </mi:EditorSelectCell>


        <mi:EditorTextCell DataField="REF_ARR_USER_CODE" HeaderText="可管理的下级人员编码" Width="300" />
        <mi:EditorTextCell DataField="REF_ARR_ROLE_CODE" HeaderText="可管理的角色编码" Width="300" />

        
        <mi:EditorTextCell DataField="REMARK" HeaderText="备注" Width="200" />
<%--        <mi:EditorTextCell DataField="ARR_ROLE_NAME" HeaderText="角色名称" Width="200" ReadOnly="true" />--%>

        <mi:EmptyField />
    </Columns>
    <EmptyDataText>没有找到记录! 请您重新输入查询条件,进行查询!</EmptyDataText>
    <Pagination IsAjax="True" Command="GoPage" ClassName="flickr" RowCount="20" 
        UrlFormat="PageIndex={0}" ButtonCount="10" ID="DataGridView1_Page"></Pagination>
</mi:DataGridView>
</form>

<script type="text/javascript">

    function AutoRefresh() {
        /// <summary>自动刷新</summary>

        widget1.submit("#TmpButton", { command: "ResetBtn_Click" });
    }

    function refresh(sender, e) {
        
        if (e.result != 'ok') { return; }

        AutoRefresh();

    }

</script>