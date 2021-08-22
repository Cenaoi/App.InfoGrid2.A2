<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MapStepNew1.ascx.cs" Inherits="App.InfoGrid2.View.OneMap.MapStepNew1" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form  id="form1" method="post">




<mi:Viewport runat="server" ID="viewport1">
       <mi:Panel runat="server" ID="Panel1" Dock="Full" Region="Center" Scroll="None" >

           <table align="center" style="width:100%;" border="0" >
                <tr>
                    <td style=" " align="center">

                        <table border="0" style="padding: 20px;width:500px;border: 1px solid #CCCCCC; background-color: #FFFFFF;" cellspacing="6">
                            <tr>
                                <td>
                                    <mi:TextBox runat="server" ID="tbxLTalbe" FieldLabel="目标表" Width="300"  LabelAlign="Right" />
                                </td>
                                <td>
                                    <mi:TextBox runat="server" ID="tbxLDisplay" FieldLabel="目标表名" Width="300" LabelAlign="Right" />
                                </td>
                                <td>
                                    <mi:Button runat="server" ID="btnSelectLTable" Text="选择目标表" OnClick="SelectLTabel()"  />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <mi:TextBox runat="server" ID="tbxRTable" FieldLabel="数据表" Width="300" LabelAlign="Right" />
                                </td>
                                <td>
                                    <mi:TextBox runat="server" ID="tbxRDisplay" FieldLabel="数据表名" Width="300"  LabelAlign="Right" />
                                </td>
                                <td>
                                    <mi:Button runat="server" ID="btnSelectRTable" Text="选择数据表" OnClick="SelectRTabel()"  />
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td align="center" style="padding: 20px;">
                        <mi:Button runat="server" ID="SubmitBtn" Text="下一步" Width="80" Height="26" Command="GoNext"  />
                    </td>
                </tr>
            </table>

       </mi:Panel>
      <mi:WindowFooter runat="server" ID="footer1">
        <mi:Button runat="server" ID="OkBtn" Text="下一步" Width="80" Height="26" Command="GoNext"
            Dock="Center" />
        <mi:Button runat="server" ID="Button2" Text="取消" Width="80" Height="26" Dock="Right"
            OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>
</form>
<script >
    function SelectLTabel() {
        var urlStr = "ShowTableAll.aspx";
        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '选择数据表',
            iframe: true,
            width: 800,
            height: 600,
            startPosition: 'center_screen',
            url: urlStr
        });

        win.show();

        win.formClosed(function (e) {
            if (e.result != 'ok') { return; }
            widget1.submit('form:first', {
                action: 'SetLTable',
                actionPs: e.id
            });


        });
    }


    function SelectRTabel() {
        var urlStr = "ShowTableAll.aspx";
        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '选择数据表',
            iframe: true,
            width: 800,
            height: 600,
            startPosition: 'center_screen',
            url: urlStr
        });

        win.show();

        win.formClosed(function (e) {
            if (e.result != 'ok') { return; }
            widget1.submit('form:first', {
                action: 'SetRTable',
                actionPs: e.id
            });


        });
    }

    
</script>
