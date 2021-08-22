<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WarehouseBeg.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.Transform_V1.WarehouseBeg" %>


<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<!--过户单据-表头-->
<form action="" method="post" >
<mi:Store runat="server" ID="store1" Model="UT_018" IdField="ROW_IDENTITY_ID" PageSize="20" SortText="ROW_DATE_CREATE desc,ROW_IDENTITY_ID desc">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="ROW_IDENTITY_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >

          <mi:SearchFormLayout runat="server" ID="searchForm" StoreID="store1">

         </mi:SearchFormLayout>

        <mi:Toolbar ID="Toolbar1" runat="server">

            <mi:ToolBarTitle ID="tableNameTB1" Text="过户单据-表头" />

            <mi:ToolBarButton Text="新增" OnClick="ser:store1.Insert()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm.toggle()" />
       <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?" OnClick="ser:store1.Delete()" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" CheckedMode="Single" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:ActionColumn HeaderText="功能">
                    <mi:ActionItem Text="修改" Command="GoShowUT_019_2" DisplayMode="Text" />
                    <mi:ActionItem Text="查看" Command="GoShowUT_019"  DisplayMode="Text"/>
                </mi:ActionColumn>
                <mi:SelectColumn  HeaderText="状态" DataField="BIZ_SID" TriggerMode="None" EditorMode="None" Width="80" >
                    <mi:ListItem Value="0" Text="草稿" />
                    <mi:ListItem Value="2" Text="审核中" />
                    <mi:ListItem Value="999" Text="审核完成" />
                </mi:SelectColumn>
                <mi:BoundField HeaderText="客户编码" DataField="CLIENT_CODE" />
                <mi:BoundField HeaderText="客户名称" DataField="CLIENT_TEXT" />
                <mi:BoundField HeaderText="原-客户编码" DataField="SRC_CLIENT_CODE" />
                <mi:BoundField HeaderText="原-客户名称" DataField="SRC_CLIENT_TEXT" />
                
                <mi:NumColumn HeaderText="数量" DataField="F_NUM" />

                <mi:DateTimeColumn HeaderText="转移时间" DataField="ROW_DATE_CREATE"/>
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>

    <mi:Hidden runat="server" ID="H_ID"  />

</form>


