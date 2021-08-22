<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserListM2v3.ascx.cs" Inherits="App.InfoGrid2.Sec.User.UserListM2v3" %>
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


<%--懒猫木阳台装饰项目的（系统管理-公司用户管理）页面--%>
<form action="" method="post">
<mi:Store runat="server" ID="store1" Model="SEC_LOGIN_ACCOUNT" IdField="SEC_LOGIN_ACCOUNT_ID" PageSize="20">
    <FilterParams>
        <mi:Param Name="ROW_STATUS_ID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="SEC_LOGIN_ACCOUNT_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_STATUS_ID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
    </DeleteRecycleParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">
    <mi:SearchFormLayout runat="server" ID="searchForm" StoreID="store1" Visible="false">

    </mi:SearchFormLayout>
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >

        <mi:Toolbar ID="Toolbar1" runat="server">

            <mi:ToolBarButton Text="新增" OnClick="ser:store1.Insert()" />
            <mi:ToolBarButton Text="保存" OnClick="ser:store1.SaveAll()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:store1.Delete()" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />

                <mi:BoundField DataField="BIZ_USER_CODE"  HeaderText="用户编码" EditorMode="None" />
                <mi:BoundField DataField="TRUE_NAME"  HeaderText="姓名"  />
                <mi:BoundField DataField="LOGIN_NAME"  HeaderText="登陆账号"  />
                <mi:PasswordColumn DataField="LOGIN_PASS"  HeaderText="登陆密码" />
                
                <mi:BoundField DataField="ARR_ROLE_NAME"  HeaderText="角色"  />

                <mi:SelectColumn DataField="SEC_MODE_ID" HeaderText="权限类型" TriggerMode="None">
                    <mi:ListItem Value="0" Text="0-禁止登陆" />
                    <mi:ListItem Value="1" Text="1-角色权限" />
                    <mi:ListItem Value="2" Text="2-自定义权限" />
                </mi:SelectColumn>
                <mi:BoundField DataField="REF_ARR_USER_CODE"  HeaderText="权限范围（可管理的人员）" Width="200" />
                <mi:BoundField DataField="REF_ARR_ROLE_CODE"  HeaderText="权限范围（可管理的角色）" Width="200" />

                <mi:ActionColumn AutoHide="true" Visible="false">
                    <mi:ActionItem Handler="showStruceDialog" Tooltip="业务结构权限" Icon="/res/icon_sys/Setup_3.png" Text="业务结构权限" />
                </mi:ActionColumn>

                <mi:BoundField DataField="ARR_COMP_CODE" HeaderText="客户ID" />
                <mi:BoundField DataField="REMARK" HeaderText="备注" />
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>

</form>

<script>


    function showStruceDialog(view, cell, recordIndex, cellIndex, e, record, row) {

        var id = record.getId();

        console.log()

        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '选择 - 结构权限',
            iframe: true,
            width: 400,
            height: 600,
            //state: 'max',
            startPosition: 'center_screen',
            url: "/App/InfoGrid2/Sec/StructDefine/TreeStruceCheckDialog.aspx?id="+id
        });



        win.show();

        win.formClosed(function (e) {
            if (e.result != 'ok') { return; }
            alert("保存成功了！");
        });
    }

</script>