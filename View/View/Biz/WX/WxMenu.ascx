<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WxMenu.ascx.cs" Inherits="App.InfoGrid2.View.Biz.WX.WxMenu" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<!--微信自定义菜单主表-->
<form action="" method="post" >
<mi:Store runat="server" ID="store1" Model="WX_MENU" IdField="WX_MENU_ID" PageSize="100">
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


<mi:Store runat="server" ID="store2" Model="WX_MENU_LA" IdField="WX_MENU_LA_ID" PageSize="100">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:StoreCurrentParam Name="FK_MENU_CODE" ControlID="store1" PropertyName="PK_MENU_CODE" />
    </FilterParams>
    <InsertParams>
        <mi:StoreCurrentParam Name="FK_MENU_CODE" ControlID="store1" PropertyName="PK_MENU_CODE" />
    </InsertParams>
    <DeleteQuery>
        <mi:ControlParam Name="ROW_IDENTITY_ID" ControlID="table2" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>
</mi:Store>


<mi:Viewport runat="server" ID="viewport1" Main="true">

    <mi:Panel runat="server" ID="centerPanel" Dock="Center" Region="North" Scroll="None"  Height="400">

        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarTitle ID="tableNameTB1" Text="微信自定义菜单主表" />

            <mi:ToolBarButton Text="新增" OnClick="ser:store1.Insert()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:store1.Delete()" />

            <mi:ToolBarButton Text="更新自定义菜单" Command="GoUpdataWxMenu" />


        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:NumColumn HeaderText="ID" DataField="WX_MENU_ID" />
                <mi:BoundField HeaderText="自定义主键编码" DataField="PK_MENU_CODE" />
                <mi:BoundField HeaderText="菜单文字" DataField="MENU_TEXT" />
                <mi:SelectColumn HeaderText="菜单类型" DataField="MENU_TYPE" TriggerMode="None">
                    <mi:ListItem Value="有子菜单" Text="有子菜单" />
                </mi:SelectColumn>
                <mi:BoundField HeaderText="菜单地址" DataField="MENU_URL" />
                <mi:SelectColumn  HeaderText="点击类型" DataField="CLICK_TYPE" TriggerMode="None">
                    <mi:ListItem Value="click" Text="点击"  />
                    <mi:ListItem Value="url" Text="地址" />
                </mi:SelectColumn>
                <mi:BoundField HeaderText="菜单关键字" DataField="MENU_KEY" />
            </Columns>
        </mi:Table>
    </mi:Panel>
    
     <mi:Panel runat="server" ID="Panel1" Dock="Full" Region="Center" Scroll="None" >

        <mi:Toolbar ID="Toolbar2" runat="server">
            <mi:ToolBarTitle ID="tableNameTB2" Text="微信自定义菜单子表" />

            <mi:ToolBarButton Text="新增" OnClick="ser:store2.Insert()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store2.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:store2.Delete()" />


        </mi:Toolbar>
        <mi:Table runat="server" ID="table2" StoreID="store2" Dock="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:NumColumn HeaderText="ID" DataField="WX_MENU_LA_ID" />
                <mi:BoundField HeaderText="自定义主键编码" DataField="PK_MENU_LA_CODE" />
                <mi:BoundField HeaderText="外键  主表的自定义主键编码" DataField="FK_MENU_CODE" />
                <mi:BoundField HeaderText="菜单文字" DataField="MENU_TEXT" />
                <mi:BoundField HeaderText="菜单地址" DataField="MENU_URL" />
                <mi:SelectColumn  HeaderText="点击类型" DataField="CLICK_TYPE" TriggerMode="None">
                    <mi:ListItem Value="click" Text="点击"  />
                    <mi:ListItem Value="url" Text="地址" />
                </mi:SelectColumn>
                <mi:BoundField HeaderText="菜单关键字" DataField="MENU_KEY" />
            </Columns>
        </mi:Table>
    </mi:Panel>



</mi:Viewport>

</form>



