<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectUser.ascx.cs" Inherits="App.InfoGrid2.Sec.User.SelectUser" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>



<form action="" method="post">
<mi:Store runat="server" ID="store1" Model="SEC_LOGIN_ACCOUNT" IdField="SEC_LOGIN_ACCOUNT_ID" PageSize="20">
    <FilterParams>
        <mi:Param Name="ROW_STATUS_ID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">
    <mi:SearchFormLayout runat="server" ID="searchForm" StoreID="store1" Visible="false">

    </mi:SearchFormLayout>
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >

        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />

                <mi:BoundField DataField="BIZ_USER_CODE"  HeaderText="用户编码" EditorMode="None" />
                <mi:BoundField DataField="TRUE_NAME"  HeaderText="姓名"  />
                <mi:BoundField DataField="LOGIN_NAME"  HeaderText="登陆账号"  />

                <mi:BoundField DataField="ARR_ROLE_NAME"  HeaderText="角色"  />

            </Columns>
        </mi:Table>
    </mi:Panel>

    <mi:WindowFooter runat="server" ID="footer1">
            <mi:Button runat="server" ID="Button2" Text="确定" Width="80" Height="26" Dock="Center" Command="GoSuccess" />
        <mi:Button runat="server" ID="Button1" Text="取消" Width="80" Height="26" Dock="Center" OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>

</form>


