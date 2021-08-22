<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormPrint.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.Prints.FormPrint" %>
<%@ Import Namespace="HWQ.Entity.LightModels" %>

<%--<script src="/Core/Scripts/jquery.printArea/1.0/printArea.js"></script>--%>

<script src="/Core/Scripts/jquery.printArea/2.4.0/jquery.PrintArea.js"></script>

<style >
    table tr td{
        font-size:30px;
    }

    body{
        background-color:white;
    }
</style>

<div style="width:100%; height:600px;">


    <%

        //LModel ut1 = this.GetBill();

        LModelList<LModel> prods = this.GetProds();

        

        int n = 0;
    %>

    <div id="printContent" style="width:400px;height:200px;  margin-left:auto;margin-right:auto; ">
        <style media="print">
            table tr td{
                font-size:30px;
            }

            body{
                background-color:white;
            }
            
            .PageNext{page-break-after: always;} 
        </style>
        <%
            foreach (LModel T in prods)
            {
                n++;

                %>
        
        <table style="border:none; width:400px; padding-bottom:20px;" cellspacing="0">
            <tr>
                <td style="border:solid 1px #000000; text-align:center;" title="货物品名"><%= T["S_PROD_TEXT"] %>&nbsp;</td>
            </tr>
            <tr>
                <td style="border-left:solid 1px #000000;border-right:solid 1px #000000;border-bottom:solid 1px #000000;  text-align:center;" title="单号">单号：<%= T["IN_BILL_NO"] %></td>
            </tr>
            <tr>
                <td style="border-left:solid 1px #000000;border-right:solid 1px #000000;border-bottom:solid 1px #000000; text-align:center; padding:10px;">
                    <img src="<%= this.GetCode1(T["PROD_CODE"].ToString(),T) %>" />
                </td>
            </tr>
        </table>

                <%

                if (n % 4 == 0)
                {
                    %><div class="PageNext"></div><%
                }


            }
        %>
    </div>

</div>

<script type="text/javascript">

$(function(){

    setTimeout(function () {

        $("#printContent").printArea();

    },1000);
    
    //setTimeout(function () {

    //    ownerWindow.close();

    //}, 1000);


});
</script>