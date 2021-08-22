<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsMenu.ascx.cs" Inherits="App.InfoGrid2.View.Biz.CMS.CmsMenu" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>



<!--菜单表-->
<form action="" method="post" >
<mi:Store runat="server" ID="store1" Model="CMS_MENU" IdField="CMS_MENU_ID" PageSize="20">
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

        <mi:Toolbar ID="Toolbar1" runat="server">

            <mi:ToolBarTitle ID="tableNameTB1" Text="菜单表" />
            <mi:ToolBarButton Text="新增" OnClick="ser:store1.Insert()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm.toggle()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:store1.Delete()" />
            <mi:ToolBarButton Text="创建数据表"  Command="GoCreateTable" />
            
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField HeaderText="自定义主键编码" DataField="PK_MENU_CODE" EditorMode="None" />
                <mi:BoundField HeaderText="数据库表名" DataField="TABLE_NAME" EditorMode="None" />
                <mi:SelectColumn HeaderText="菜单类型" DataField="MENU_TYPE" TriggerMode="None">
                    <mi:ListItem Text="文章" Value="ARTICLE" />
                    <mi:ListItem Text="单页" Value="TILE" />
                </mi:SelectColumn>
                <mi:BoundField HeaderText="上级菜单自定义主键编码" DataField="PARENT_MENU_CODE" />
                <mi:BoundField HeaderText="菜单名称" DataField="MENU_TEXT" />
                <mi:CheckColumn HeaderText="激活" DataField="ENABLED" />
                <mi:BoundField HeaderText="标识符" DataField="M_IDENTIFIER" />
                <mi:BoundField HeaderText="界面类型" DataField="PAGE_TYPE" />
                <mi:BoundField HeaderText="菜单排序" DataField="MENU_SEQ" />
                <mi:BoundField HeaderText="界面模板" DataField="PAGE_TEMPLATE" />
                <mi:BoundField HeaderText="界面目录模板" DataField="PAGE_CATA_TEMPLATE" />
                <mi:BoundField HeaderText="页面分类模板" DataField="PAGE_CATE_TEMPLATE" />
                <mi:BoundField HeaderText="图标路径" DataField="ICON_URL" />
                <mi:CheckColumn HeaderText="目录激活" DataField="CATALOG_ENABLED" />
                <mi:CheckColumn HeaderText="分类激活" DataField="CATEGORY_ENABLED" />
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>

</form>





