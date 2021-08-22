<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditMenuInfo.ascx.cs"
    Inherits="App.InfoGrid2.View.Biz.Core_Menu.EditMenuInfo" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<form method="post">
<mi:Store runat="server" ID="store1" Model="BIZ_C_MENU" IdField="BIZ_C_MENU_ID"  >
    <FilterParams>
        <mi:QueryStringParam Name="BIZ_C_MENU_ID" QueryStringField="id" DbType="Int32" />
    </FilterParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport">
    <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="300" ItemLabelAlign="Right" FlowDirection="TopDown"
        Dock="Full" Region="Center" SaveEnabled="true" StoreID="store1" AutoSize="true" Width="600">
        <mi:Label runat="server" ID="labId" FieldLabel="ID" DataField="BIZ_C_MENU_ID" />
        <mi:TextBox runat="server" ID="tbxUri" FieldLabel="页面地址" ColSpan="2" DataField="URI" />
        <mi:ComboBox runat="server" FieldLabel="语言" ID="tbxDisplayLanguage" TriggerMode="None" DataField="DISPLAY_LANGUAGE">
            <mi:ListItem Value="CN" Text="中文" />
            <mi:ListItem Value="EN" Text="英文" />
        </mi:ComboBox>
        <mi:TextBox runat="server" ID="tbxMenuTypeID" FieldLabel="菜单类型" DataField="MENU_TYPE_ID" />
        <mi:TextBox runat="server" ID="tbxMenuIdentifier" FieldLabel="唯一值" DataField="MENU_IDENTIFIER" />
        <mi:TextBox runat="server" ID="tbxDescription" FieldLabel="提示信息" DataField="DESCRIPTION" />
        <mi:NumberBox runat="server" ID="nbSecFunId" FieldLabel="权限ID" DataField="SEC_FUN_ID" />
        
        <mi:TextBox runat="server" ID="tbxParams" FieldLabel="特殊参数" DataField="PARAMS" />
        <mi:NumberBox runat="server" ID="nbMaxCount" FieldLabel="打开窗口数量" DataField="MAX_COUNT" />
        
        <mi:CheckBox runat="server" ID="menuEnabledCB" DataField="MENU_ENABLED" FieldLabel="激活" />

                
        <div style="border: 1px solid #c0c0c0; padding: 4px; background-color: #f5f5f5; font-weight: bold; color: #666666;" ColSpan="4">图标</div>

        
        <mi:TextBox runat="server" ID="tbxICO" FieldLabel="图标地址" ColSpan="2"  DataField="ICO" />
        
        <mi:TextBox runat="server" ID="TextBox1" FieldLabel="图标文字" ColSpan="2"  DataField="ICON_CHAT" />
        <mi:TextBox runat="server" ID="TextBox2" FieldLabel="文字颜色" ColSpan="2"  DataField="ICON_COLOR" />
        <mi:TextBox runat="server" ID="TextBox3" FieldLabel="背景颜色" ColSpan="2"  DataField="ICON_BG_COLOR" />

        <div style="border: 1px solid #c0c0c0; padding: 4px; background-color: #f5f5f5; font-weight: bold; color: #666666;" ColSpan="4">权限项目</div>


        <mi:ComboBox runat="server" ID="tbxPageTypeID" DataField="SEC_PAGE_TYPE_ID" FieldLabel="页面类型" TriggerMode="None">
            <mi:ListItem TextEx="--空--" />
            <mi:ListItem Value="TABLE" Text="工作表类型" />
            <mi:ListItem Value="PAGE" Text="复杂表类型" />
            <mi:ListItem Value="FIXED_PAGE" Text="自定义UI" />
        </mi:ComboBox>
        <mi:NumberBox runat="server" ID="secPageIdNum" DataField="SEC_PAGE_ID" FieldLabel="页面ID" />

        <mi:TextBox runat="server" ID="tbxSecTag" DataField="SEC_PAGE_TAG" FieldLabel="页面标识" />

        <mi:TextBox runat="server" ID="tbxAliasTitle" DataField="ALIAS_TITLE" FieldLabel="标题别名" />
        
        <mi:TextBox runat="server" ID="XmlExTB" DataField="EXPAND_CFG" FieldLabel="扩展配置" ColSpan="2" />

        <mi:TextBox runat="server" ID="tbxOwnerMenuId" DataField="OWNER_MENU_ID" FieldLabel="所属菜单ID" />

    </mi:FormLayout>
    <mi:WindowFooter ID="WindowFooter1" runat="server">
        <mi:Button runat="server" ID="SubmitBtn" Width="120" Height="26" Command="btnSave"
            Text="保存" Dock="Center" />
    </mi:WindowFooter>
</mi:Viewport>
</form>

