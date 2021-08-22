<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormPrint2.aspx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.Prints.FormPrint2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>

    <script src="/Core/Scripts/jquery/jquery-2.0.3.min.js"></script>
    <script src="/Core/Scripts/jquery.printArea/2.4.0/jquery.PrintArea.js"></script>

<input type="button" id="btnPrint" value="打印" style="width:80px;height:30px;" />

<style>
    table tr td{
        font-size:30px;
    }
</style>

    <div style="width:100%; height:600px;">

        <div id="printContent" style="width:400px;height:200px;  margin-left:auto;margin-right:auto; ">

            <table style="border:none; width:400px;" cellspacing="0">
                <tr>
                    <td style="border:solid 1px #000000; text-align:center;">货物品名</td>
                </tr>
                <tr>
                    <td style="border-left:solid 1px #000000;border-right:solid 1px #000000;border-bottom:solid 1px #000000;  text-align:center;">单据条码</td>
                </tr>
                <tr>
                    <td style="border-left:solid 1px #000000;border-right:solid 1px #000000;border-bottom:solid 1px #000000; text-align:center;">单据条码</td>
                </tr>
            </table>

        </div>

    </div>

    <script type="text/javascript">

    $(function(){
        $("#btnPrint").click(function () {
            $("printContent").printArea({
                mode:false,
                popClose: true,
                retainAttr: 'printContent'
            });
        });

    });
    </script>
</body>
</html>
