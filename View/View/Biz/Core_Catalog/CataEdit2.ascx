<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CataEdit2.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Core_Catalog.CataEdit2" %>

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
        <mi:Label runat="server" ID="TextBox1" FieldLabel="代码" DataField="CATA_CODE" />
        <mi:CheckBox runat="server" ID="visibleCB" FieldLabel="显示" DataField="VISIBLE" />
      
        <mi:TextBox runat="server" ID="TextBox3" FieldLabel="备注" DataField="REMARK" />
        <mi:TriggerBox runat="server" ID="TextBox4" FieldLabel="权限结构代码" DataField="SEC_STRUCT_CODE"  OnButtonClick="ShowTree(this)"  ButtonType="More" />
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
        var struct_code = $.query.get("struct_code");

        console.log(struct_code);
        console.log($.query.toString());

        var urlStr = "/App/InfoGrid2/Sec/StructDefine/TreeStruceDialog.aspx?struct_code=" + struct_code;

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

