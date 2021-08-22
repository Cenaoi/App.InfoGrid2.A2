<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArrLoginList.ascx.cs" Inherits="App.InfoGrid2.Sec.ArrLogin.ArrLoginList" %>
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
<mi:Store runat="server" ID="store1" Model="SEC_ARR_ACCOUNT" IdField="SEC_ARR_ACCOUNT_ID" DeleteRecycle="true" PageSize="20">
    <FilterParams>
        <mi:QueryStringParam Name="SEC_LOGIN_CODE" QueryStringField="SEC_LOGIN_CODE" DefaultValue="007" />
    </FilterParams>
    <InsertParams>
        <mi:QueryStringParam Name="SEC_LOGIN_CODE" QueryStringField="SEC_LOGIN_CODE" DefaultValue="007" />
    </InsertParams>
    <DeleteQuery>
        <mi:ControlParam Name="SEC_ARR_ACCOUNT_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
    </DeleteRecycleParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">
    <mi:SearchFormLayout runat="server" ID="searchForm" StoreID="store1" Visible="false" Height="100">
        
    </mi:SearchFormLayout>
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:Toolbar ID="Toolbar1" runat="server">
            
            <mi:ToolBarTitle ID="tableNameTB1" Text="可管理的下级人员" />
                
                
                <mi:ToolBarButton Text="选择人员" OnClick="" />

                <mi:ToolBarHr />

                <mi:ToolBarButton Text="新增" OnClick="ser:store1.Insert()" Icon="/res/icon_sys/Insert.png" />
                <mi:ToolBarButton Text="保存" OnClick="ser:store1.SaveAll()" Icon="/res/icon_sys/Save.png" />
                <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" Icon="/res/icon_sys/Refresh.png" />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm.toggle()" Icon="/res/icon_sys/Search.png" />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:store1.Delete()" Icon="/res/icon_sys/Delete.png" />


        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                
                <mi:SelectColumn  DataField="REF_CODE" HeaderText="用户代码" >
                    <MapItems>
                        <mi:MapItem SrcField="text" TargetField="REF_TEXT" />
                    </MapItems>
                </mi:SelectColumn>

                <mi:BoundField DataField="REF_TEXT" HeaderText="用户名称" />
                <mi:CheckColumn DataField="ENABLED" HeaderText="激活" />


            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>

</form>