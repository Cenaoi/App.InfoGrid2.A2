<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CataTypeList.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Core_Catalog.CataTypeList" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<!--业务-目录类型-->
<form action="" method="post" >
<mi:Store runat="server" ID="store1" Model="BIZ_CATALOG_TYPE" IdField="BIZ_CATALOG_TYPE_ID" PageSize="20" DeleteRecycle="true">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="BIZ_CATALOG_TYPE_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" >
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarTitle ID="tableNameTB1" Text="业务-目录类型" />

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
                <mi:BoundField HeaderText="类型代码" DataField="CATA_TYPE_CODE" />
                <mi:BoundField HeaderText="类型名称" DataField="CATA_TYPE_TEXT" />
                <mi:BoundField HeaderText="备注" DataField="REMARK" />
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>

</form>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.store1.DataBind();
        }
    }
</script>