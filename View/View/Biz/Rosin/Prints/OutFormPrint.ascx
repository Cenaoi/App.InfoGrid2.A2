<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutFormPrint.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.Prints.OutFormPrint" %>
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

        LModel ut1 = this.GetBill();

        LModelList<LModel> prods = this.GetProds();
                

        int n = 0;
    %>

    <div id="printContent" style="  margin-left:auto;margin-right:auto; ">
        <style media="print">
            table tr th{
                font-size:40px;
            }
                        
            table tr td{
                font-size:30px;
            }
            body{
                background-color:white;
            }

            
            
            .PageNext{page-break-after: always;} 
        </style>

        <div style="text-align:center;width:180mm; font-size:40px;">出库单</div>

        <table style="width:180mm; font-size:14px; border:1px solid #000000; ">
            <tr>
                <td style="width:50%;">
                    打印时间: <%= DateTime.Now.ToString() %>
                </td>
                <td>
                    数量: <%= prods.Count %>
                </td>
            </tr>
            <tr>
                <td>
                    车牌号: <%= ut1["O_CAR_NO"]  %>
                </td>
                <td>                   
                    装车时间: <%= ut1["O_CAR_DATE"]  %>
                </td>
            </tr>
            <tr>
                <td style="padding:6px;" colspan="2">
                    <hr style="width:100%;" />
        <%
            foreach (LModel T in prods)
            {
                n++;

                %>
                  <%= T["PROD_CODE"] %> ;   
                <%
            }
        %>


                </td>
            </tr>
        </table>


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