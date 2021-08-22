<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Jbgxjdb_5.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Report.Jbgxjdb_5" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<script src="/Core/Scripts/echarts/echarts.min.js"></script>

<link href="/Core/Scripts/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" />
<link href="/res/ace/ace.css" rel="stylesheet" />
<link href="/res/ace/font-awesome-4.4.0/css/font-awesome.min.css" rel="stylesheet" />
<link href="/res/ace/mx.css" rel="stylesheet" />




<div class="main-content">

    <div class="main-content-inner">

        <div class="page-content">

            <div class="row">
             <p style="font-size:30px">合同管理</p> 

            </div>
            <div class="row">

                <div class="col-lg-6">

                    <div id="left_chart" style="height: 300px; width: 100%; float: left"></div>


                </div>
                <div class="col-lg-6">


                    <div id="right_chart" style="height: 300px; width: 100%; float: right"></div>

                </div>
            </div>








            <div class="row">

                <div id="zhuont_chart" style="height: 300px; width: 100%; float: right"></div>


            </div>

            <div class="row">


                <div id="zhutwo_chart" style="height: 300px; width: 100%; float: right"></div>



            </div>






        </div>


    </div>

</div>





<%--饼状图--%>
<script>

    $(function () {




        option = {
            title: {
                text: '合同类型统计',

                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            legend: {
                orient: 'vertical',
                left: 'left',

                data:[<%=smlisttitle%>],
  
            },
            label:{
                normal:{
                    formatter: '{b}\n{c} ({d}%)'
                }
            },

            series: [
                {
                    name: '合同类型',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data:<%= smlistji%>,

                    itemStyle: {
                        emphasis: {
                            shadowBlur: 10,
                            shadowOffsetX: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        }
                    }
                }
            ]
        };



        var myChart = echarts.init(document.getElementById('left_chart'));
        myChart.setOption(option);
        myChart.on('click', function (params) {

            EcView.show("/App/InfoGrid2/View/OneView/ViewPreview.aspx?id="+1981);

        });

    });

       

    
</script>

<%--横柱状图--%>
<script>


    option = {
        title: {
            text: '合同收付额统计',
           x:'left'
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            }
        },
        legend: {
            data: ['收付统计']
        },
 

        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: {
            type: 'value',
            boundaryGap: [0, 0.01]
        },
        yAxis: {
            type: 'category',
            data:[<%=hzxname%>]
       
        },
        label:{
            normal:{
                formatter: '{b}\n{c} ({d}%)'
            }
        },
        series: [
            {
                name: '收付统计',
                type: 'bar',
                data:[<%=hzxbiaode%>],
                itemStyle: {
                    //emphasis: {
                    //    shadowBlur: 10,
                    //    shadowOffsetX: 0,
                    //    shadowColor: 'rgba(0, 0, 0, 0.5)'
                    //},
                    normal: {
                        label: {
                            show: true,
                            position: 'right',
                            formatter: '{c}'
                        },
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }

        
            }

  
        ]

    };

    var myChart = echarts.init(document.getElementById('right_chart'));
    myChart.setOption(option);
  
        myChart.on('click', function (params) {

            EcView.show("/App/InfoGrid2/View/OneView/ViewPreview.aspx?id="+1981);

        });
  

</script>
<%--柱状图--%>
<script>


    option = {
        title: {
            text: '逾期未收款统计',
            x: 'center'
        },
       
        tooltip: {
            trigger: 'axis',
            axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },

        xAxis: [
            {
                type: 'category',
                data:[<%=hetongxdf%>],
               

                axisTick: {
                    alignWithLabel: true
                }
            }
        ],
        yAxis: [
            {
                type: 'value'
            }
        ],
        series: [
            {
                name: '欠款金额',
                type: 'bar',
                barWidth: '60%',
                data:[<%=weifuje%>],

                itemStyle: {
                    //emphasis: {
                    //    shadowBlur: 10,
                    //    shadowOffsetX: 0,
                    //    shadowColor: 'rgba(0, 0, 0, 0.5)'
                    //},
                    normal: {
                        label: {
                            show: true,
                            position: 'top',
                            formatter: '{c}'
                        },
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }

           
            }
        ]
    };

    var myChart = echarts.init(document.getElementById('zhuont_chart'));
    myChart.setOption(option);
    myChart.on('click',function(){
        EcView.show("/App/InfoGrid2/View/OneView/ViewPreview.aspx?id="+1983);
    });
</script>
<%--柱状图--%>
<script>

    option = {
        title: {
            text: '逾期未付款统计',
            x: 'center'
        },
     
        tooltip: {
            trigger: 'axis',
            axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: [
            {
                type: 'category',
                data:[<%=hetongfu%>],
               
                axisTick: {
                    alignWithLabel: true
                }
            }
        ],
        yAxis: [
            {
                type: 'value'
            }
        ],
        series: [
            {
                name: '欠款金额',
                type: 'bar',
                barWidth: '60%',
                data:[<%=weifujew%>],
                itemStyle: {
                    //emphasis: {
                    //    shadowBlur: 10,
                    //    shadowOffsetX: 0,
                    //    shadowColor: 'rgba(0, 0, 0, 0.5)'
                    //},
                    normal: {
                        label: {
                            show: true,
                            position: 'top',
                            formatter: '{c}'
                        },
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
           
            }
        ]
    };

    var myChart = echarts.init(document.getElementById('zhutwo_chart'));
    myChart.setOption(option);
    myChart.on('click',function(){
        EcView.show("/App/InfoGrid2/View/OneView/ViewPreview.aspx?id="+1982);
    
    });
</script>
