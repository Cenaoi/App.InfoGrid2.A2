<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StepManyNew2.ascx.cs" Inherits="App.InfoGrid2.View.InputExcel.StepManyNew2" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" id="form1" method="post">


<table align="center" style="width:400px;" border="0" >
    <tr>
        <td>
            <mi:TextBox runat="server" ID="tbxMainName" FieldLabel="主表名" ReadOnly="true" />
            <mi:TextBox runat="server" ID="tbxMainDsiplay" FieldLabel="描述" ReadOnly="true" />
        </td>
        <td>
            <mi:Button runat="server" ID="btnShowMainTable" Text="选择表" OnClick="ShowMainTable()" />
        </td>
    </tr>
    <tr>
        <td>
            <mi:TextBox runat="server" ID="tbxTableName" FieldLabel="子表名" ReadOnly="true" />
            <mi:TextBox runat="server" ID="tbxDisplay" FieldLabel="描述" ReadOnly="true" />
        </td>
        <td>
            <mi:Button runat="server" ID="btnShowTable" Text="选择表" OnClick="ShowTable()" />
        </td>
    </tr>
    <tr>
        <td>
            <mi:TextBox runat="server" ID="tbxExcelName" FieldLabel="Excel文件" ReadOnly="true"  />
        </td>
        <td>
            <mi:Button runat="server" ID="btnInputFile" Text="上传" Command="ShowInputFile" />
        </td>
    </tr>
    <tr>
        <td align="center" colspan="2">
            <mi:Button runat="server" ID="SubmitBtn" Text="下一步" Width="80" Height="26" Command="GoNext"  />
        </td>
    </tr>
</table>

</form>

<script>
    

    

     ///显示所有表名界面
    function ShowMainTable() {
        
        var frm = Mini2.create('Mini2.ui.Window', {
            url: '/App/InfoGrid2/View/OneMap/ShowTableAll.aspx',
            mode: true

        });


        frm.show();

        frm.formClosed(function (e) {
            if (e.result != 'ok') { return; };
            var id = e.id;
            widget1.submit('form:first', {
                action: 'SetMainTableName',
                actionPs: id
            });

        });
    }

    ///显示所有表名界面
    function ShowTable() {
        
        var frm = Mini2.create('Mini2.ui.Window', {
            url: '/App/InfoGrid2/View/OneMap/ShowTableAll.aspx',
            mode: true

        });


        frm.show();

        frm.formClosed(function (e) {
            if (e.result != 'ok') { return; };
            var id = e.id;
            widget1.submit('form:first', {
                action: 'SetTableName',
                actionPs: id
            });

        });
    }

    ///显示上传界面
    function ShowInputFile(url) {
        var frm = Mini2.create('Mini2.ui.Window', {
            url: url,
            mode: true

        });
        frm.show();

        frm.formClosed(function (e) {
            if (e.result != 'ok') { return; };
            var id = e.id;
            widget1.submit('form:first', {
                action: 'Refresh',
            });

        });
     }





</script>   



