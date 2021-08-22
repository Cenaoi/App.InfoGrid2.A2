﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShowChartPie.ascx.cs" Inherits="App.InfoGrid2.View.ReportBuilder.ShowChartPie" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

    <!-- ECharts单文件引入 -->
    <script src="http://echarts.baidu.com/build/dist/echarts.js"></script>


    <link href="/Core/Scripts/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" />

<form action="ShowChart.aspx" id="form1" method="post">

    <mi:Store runat="server" ID="store1" Model="IG2_MAP" IdField="IG2_MAP_ID" PageSize="0" ReadOnly="true" AutoFocus="false" >
    </mi:Store>

    <table style="width:100%;margin-top:20px;">
        <tbody >

            <tr>
                <td style="width:25%;"><mi:DateRangePicker runat="server" FieldLabel="时间" ID="drpTime" Width="400" LabelAlign="Right" />   </td>
                <td style="width:10%;"> <mi:Button ID="Button1" runat="server" Text="查询" Command="btnSelect" Width="80" /> </td>
                <td style="width:15%;"> <mi:Button ID="Button2" runat="server" Text="本月" Command="btnMonth" Width="120" /></td>
                <td style="width:15%;"> <mi:Button ID="Button3" runat="server" Text="本季" Command="btnQuarter" Width="120" /></td>
                <td > <mi:Button ID="Button4" runat="server" Text="本年" Command="btnYear" Width="120" /></td>
            </tr>
            
        </tbody>

    </table>



    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
    <div id="main" style="height:600px"></div>

    
 
</form>

<script type="text/javascript">



  // 路径配置
    require.config({
        paths: {
            echarts: 'http://echarts.baidu.com/build/dist'
        }
    });

    // 使用
    require(
        [
            'echarts',
            'echarts/chart/pie'
        ],
        function (ec) {
            // 基于准备好的dom，初始化echarts图表
             myChart = ec.init(document.getElementById('main'));



            option = {
                title : {
                    text: '<%= m_title %>',
                    x:'center'
                },
                tooltip : {
                    trigger: 'item',
                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                },
                legend: {
                    orient : 'vertical',
                    x : 'left',
                    data:<%= m_grapTitel %>
                },
                toolbox: {
                    show : true
                   
                },
                calculable : true,
                series : [
                    {
                        name:'库存数量',
                        type:'pie',
                        radius : '65%',
                        center: ['50%', '60%'],
                        data:<%=m_grapData %>
                    }
                ]
            };

            // 为echarts对象加载数据
            myChart.setOption(option);
        }
    );




</script>
