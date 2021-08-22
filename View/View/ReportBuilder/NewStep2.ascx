<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewStep2.ascx.cs" Inherits="App.InfoGrid2.View.ReportBuilder.NewStep2" %>


<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" id="form1" method="post">


<table align="center" style="width:400px;" border="0" >
 
    <tr>
      <td>
             <mi:TextBox runat="server" ID="tbxTableID" FieldLabel="ID" ReadOnly="true" />
        </td>
   </tr>
   <tr>
       <td>
            <mi:TextBox runat="server" ID="tbxTableName" FieldLabel="表名" ReadOnly="true" />
            
        </td>
   </tr>
    <tr>
        <td>
            <mi:TextBox runat="server" ID="TextBox1" FieldLabel="描述" ReadOnly="true" />
        </td>
    </tr>
    <tr >
        <td align="center">
            <mi:Button runat="server" ID="btnShowTable" Text="选择表" Width="80" Height="26" OnClick="ShowTable()" />
        </td>
    </tr>
    <tr>
        <td>
            <mi:Button runat="server" ID="Button1" Text="上一步" Width="80" Height="26" Command="GoPre"  />
        </td>
        <td>
            <mi:Button runat="server" ID="SubmitBtn" Text="下一步" Width="80" Height="26" Command="GoNext"  />
        </td>
    </tr>
</table>

</form>

<script>

    ///显示所有表名界面
    function ShowTable() {
        
        var frm = Mini2.create('Mini2.ui.Window', {
            url: '/App/InfoGrid2/View/OneMap/ShowTableAll.aspx',
            mode: true,
            state:'max'

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
    function ShowEditCol(url) {
        var frm = Mini2.create('Mini2.ui.Window', {
            url: url,
            mode: true,
            state: 'max'

        });
        frm.show();
     }


</script>   
