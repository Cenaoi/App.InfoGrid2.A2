<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditInputData.ascx.cs"
    Inherits="App.InfoGrid2.View.Biz.Input_Data.EditInputData" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<form method="post">
<mi:Viewport runat="server" ID="viewport">
    <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="400" ItemLabelAlign="Right"
        Height="600" Dock="Full" Region="Center" Scroll="None">
        <mi:TriggerBox runat="server" ID="tbxDialogID" FieldLabel="导入窗口ID" ButtonClass="mi-icon-more"
            OnButtonClick="ShowTableAll('/App/InfoGrid2/View/OneMap/ShowTableAll.aspx')" />
        <mi:TextBox runat="server" ID="tbxDialogText" FieldLabel="对应表名" ReadOnly="true" />
        <mi:TriggerBox runat="server" ID="tbxMapID" FieldLabel="映射表ID" ButtonClass="mi-icon-more"
            OnButtonClick="ShowMapAll('SelectMapAll.aspx')" />
        <mi:TextBox runat="server" ID="tbxMapText" FieldLabel="映射表名" ReadOnly="true" />
        <mi:ComboBox runat="server" ID="cbxParentID" FieldLabel="父表字段" TriggerMode="None">
        </mi:ComboBox>
        <mi:TextBox runat="server" ID="tbxLTable" FieldLabel="左表" ReadOnly="true" />
        <mi:TextBox runat="server" ID="tbxLTableDisplay" FieldLabel="左表显示名" ReadOnly="true" />
        <mi:TextBox runat="server" ID="tbxRTable" FieldLabel="右表" ReadOnly="true" />
        <mi:TextBox runat="server" ID="tbxRTableDisplay" FieldLabel="右表显示名" ReadOnly="true" />
    </mi:FormLayout>
    <mi:WindowFooter ID="WindowFooter1" runat="server">
        <mi:Button runat="server" ID="SubmitBtn" Width="120" Height="26" Command="btnSave"
            Text="完成" Dock="Center" />
        <mi:Button runat="server" ID="Button2" Width="80" Height="26" OnClick="ownerWindow.close()"
            Text="取消" Dock="Center" />
    </mi:WindowFooter>
</mi:Viewport>
</form>
<script>
    function ShowTableAll(url) {
        var frm = Mini2.create('Mini2.ui.Window', {
            url: url,
            mode: true,
            state: 'max'
        });


        frm.show();

        frm.formClosed(function (e) {
            if (e.result != 'ok') { return; };
            var id = e.id;
            widget1.submit('form:first', {
                action: 'SetDialogID',
                actionPs: id
            });

        });
    }

    function ShowMapAll(url) {
        var frm = Mini2.create('Mini2.ui.Window', {
            url: url,
            mode: true

        });

        frm.show();

        frm.formClosed(function (e) {
            if (e.result != 'ok') { return; };
            var id = e.id;
            widget1.submit('form:first', {
                action: 'SetMapID',
                actionPs: id
            });

        });
    }

</script>
