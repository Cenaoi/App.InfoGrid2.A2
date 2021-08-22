<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsItemList.ascx.cs" Inherits="App.InfoGrid2.View.CMS.CmsItemList" %>


<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form action="" method="post">
    <mi:Store runat="server" ID="store1" Model="CMS_ITEM" IdField="CMS_ITEM_ID" AutoFocus="false" 
         PageSize="20" SortText="CMS_ITEM_ID desc"  >
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
        <DeleteQuery>
            <mi:ControlParam Name="CMS_ITEM_ID" ControlID="table1" PropertyName="CheckedRows" />
        </DeleteQuery>
        <DeleteRecycleParams>
            <mi:Param Name="ROW_SID" DefaultValue="-3" />
            <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
        </DeleteRecycleParams>
    </mi:Store>
    <mi:Viewport runat="server" ID="viewport1">

        <mi:Toolbar runat="server" ID="toolbar1">
            <mi:ToolBarTitle ID="tableNameTB1" Text="内容管理器" />

            
            <mi:ToolBarButton Text="新增" OnClick="ser:store1.Insert()" />
            <mi:ToolBarButton Text="保存" OnClick="ser:store1.SaveAll()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:store1.Delete()" />
            
        </mi:Toolbar>

        <mi:Table runat="server" ID="table1" StoreID="store1" ReadOnly="true" Region="Center" ColumnLines="false">
            <Columns>

                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:ActionColumn Width="60">
                    <mi:ActionItem Command="GoEdit" DisplayMode="Text" Text="修改" />
                </mi:ActionColumn>
                <mi:BoundField HeaderText="目录名称" DataField="CATA_TEXT" />
                <mi:BoundField HeaderText="标题" DataField="C_TITLE" />
                <mi:BoundField HeaderText="创建时间" DataField="ROW_DATE_CREATE" />

            </Columns>
        </mi:Table>

    </mi:Viewport>
</form>

