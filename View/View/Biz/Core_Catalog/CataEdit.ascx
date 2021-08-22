<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CataEdit.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Core_Catalog.CataEdit" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form method="post">
<mi:Store runat="server" ID="store1" Model="BIZ_CATALOG" IdField="BIZ_CATALOG_ID" >
    <FilterParams>
        <mi:QueryStringParam Name="BIZ_CATALOG_ID" QueryStringField="id" DbType="Int32" />
    </FilterParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport">
    <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="300" ItemLabelAlign="Right"
        Dock="Full" Region="Center" StoreID="store1" SaveEnabled="true">
        <mi:Label runat="server" ID="Label1" FieldLabel="ID" DataField="BIZ_CATALOG_ID" />
        <mi:Label runat="server" ID="cataTextLAB" FieldLabel="名称" DataField="CATA_TEXT" />
        <mi:ComboBox runat="server" ID="comboBox1" FieldLabel="类型" DataField="CATA_TYPE_CODE" TriggerMode="None">

        </mi:ComboBox>
        <mi:TextBox runat="server" ID="TextBox1" FieldLabel="代码" DataField="CATA_CODE" />
        <mi:TextBox runat="server" ID="TextBox12" FieldLabel="唯一值" DataField="CATA_IDENTITY" />
        <mi:CheckBox runat="server" ID="visibleCB" FieldLabel="显示" DataField="VISIBLE" />
        <mi:ComboBox runat="server" ID="secLevelCB" FieldLabel="等级" DataField="SEC_LEVEL" TriggerMode="None">
            <mi:ListItem Value="0" Text="0-用户自定义" />
            <mi:ListItem Value="6" Text="6-管理员" />
            <mi:ListItem Value="8" Text="8-系统设计师" />
            <mi:ListItem Value="99" Text="99-系统内部" />
        </mi:ComboBox>
        <mi:TextBox runat="server" ID="TextBox3" FieldLabel="备注" DataField="REMARK" />
         <mi:TriggerBox runat="server" ID="TextBox4" FieldLabel="权限结构代码" DataField="SEC_STRUCT_CODE"  OnButtonClick="ShowTree(this)"  ButtonType="More" />


        <mi:ComboBox runat="server" ID="ComboBox2" FieldLabel="是否要扩展" DefaultValue="NONE" DataField="EX_TYPE_CODE" TriggerMode="None">
            <mi:ListItem Value="NONE" Text="NONE" />
            <mi:ListItem Value="EX_TABLE" Text="EX_TABLE" />
        </mi:ComboBox>
         <mi:TextBox runat="server" ID="TextBox5" FieldLabel="扩展表名" DataField="EX_TABLE" />

    </mi:FormLayout>
    <mi:WindowFooter ID="WindowFooter1" runat="server">
        <mi:Button runat="server" ID="SubmitBtn" Width="120" Height="26" Command="btnSave" OnClick="ser:store1.SaveAll()"
            Text="保存" Dock="Center"  />
    </mi:WindowFooter>
</mi:Viewport>
</form>

<script type="text/javascript">

    function ShowTree(owner)
    {


        var urlStr = "/App/InfoGrid2/Sec/StructDefine/TreeStruceDialog.aspx";

        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '列表框架',
            iframe: true,
            width: 800,
            height: 600,
            startPosition: 'center_screen',
            url: urlStr
        });

        win.show();

        win.formClosed(function (e) {
            if (e.result != 'ok') { return; }

            owner.setValue(e.row.STRUCE_CODE);

            //record.set('ACTION_TABLE_ITEMS', 'TABLE,' + e.sviewId);

        });


    }
</script>