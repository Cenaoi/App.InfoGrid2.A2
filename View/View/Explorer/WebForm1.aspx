<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="App.InfoGrid2.View.Explorer.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="/Core/Scripts/echarts/echarts.min.js"></script>
    <script src="/Core/Scripts/jquery/jquery-2.0.3.min.js"></script>
    <link href="/Core/Scripts/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" />
    <script src="/Core/Scripts/Mini/1.1/MiniHtml.min.js"></script>
    <title>报表首页</title>

</head>
<body style="background-color: #ecf0f5;">

    <div class="container-fluid" style="padding: 20px;">

        <div class="row" style="height: 80px;">
            <div class="col-lg-12" style="height: 100%;">

                <div class="panel panel-default">
                    <div class="panel-body text-center">
                        <p style="font-size: 20px; font-weight: bolder;">欢迎光临</p>
                    </div>
                </div>
            </div>
        </div>


        <div class="row" style="color: white;">

            <div class="col-lg-3">
                <div class="panel " style="background-color: #00c0ef;">
                    <div class="panel-body" style="padding-top: 0px; position: static;">
                        <p style="font-size: 38px; font-weight: bolder;"><%= m_wcddNum %></p>
                        <h3 style="font-weight: bolder;">未出订单</h3>
                        <%--  <span class="glyphicon glyphicon-chevron-right"  style="font-size:70px; position:absolute; right:20px; bottom:20px;" aria-hidden="true"></span>--%>
                    </div>
                    <div class="panel-footer text-center" style="background: rgba(0,0,0,0.1); border-top: 0px;">
                        <a href="#" style="color: rgba(255,255,255,0.8); text-decoration: none;" onclick="changeUrl('/app/infogrid2/view/moreview/moreviewpreview.aspx?id=879','未出订单')">详细信息
                        <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                        </a>
                    </div>
                </div>

            </div>

            <div class="col-lg-3">
                <div class="panel " style="background-color: #00a65a;">
                    <div class="panel-body" style="padding-top: 0px;">
                        <p style="font-size: 38px; font-weight: bolder;"><%= m_cjscNum %></p>
                        <h3 style="font-weight: bolder;">车间在产</h3>
                    </div>
                    <div class="panel-footer text-center" style="background: rgba(0,0,0,0.1); border-top: 0px;">
                        <a href="#" style="color: rgba(255,255,255,0.8); text-decoration: none;" onclick="changeUrl('/app/infogrid2/view/moreview/MoreViewPreview.aspx?id=880','车间在产')">详细信息
                        <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                        </a>
                    </div>
                </div>

            </div>


            <div class="col-lg-3">
                <div class="panel " style="background-color: #f39c12;">
                    <div class="panel-body" style="padding-top: 0px;">
                        <p style="font-size: 38px; font-weight: bolder;"><%= m_cgztNum %></p>
                        <h3 style="font-weight: bolder;">采购在途</h3>
                    </div>
                    <div class="panel-footer text-center" style="background: rgba(0,0,0,0.1); border-top: 0px;">
                        <a href="#" style="color: rgba(255,255,255,0.8); text-decoration: none;" onclick="changeUrl('/app/infogrid2/view/moreview/MoreViewPreview.aspx?id=882','采购在途')">详细信息
                        <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                        </a>
                    </div>
                </div>

            </div>


            <div class="col-lg-3">
                <div class="panel " style="background-color: #dd4b39;">
                    <div class="panel-body" style="padding-top: 0px;">
                        <p style="font-size: 38px; font-weight: bolder;"><%= m_fwwgNum %></p>
                        <h3 style="font-weight: bolder;">发外未归</h3>
                    </div>
                    <div class="panel-footer text-center" style="background: rgba(0,0,0,0.1); border-top: 0px;">
                        <a href="#" style="color: rgba(255,255,255,0.8); text-decoration: none;" onclick="changeUrl('/app/infogrid2/view/oneview/viewpreview.aspx?id=883','发外未归')">详细信息
                        <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                        </a>
                    </div>
                </div>

            </div>

        </div>

        <!--放图片的这一行-->
        <div class="row">
            <div class="col-lg-7">
                <div class="panel" style="background-color: white;">
                    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
                    <div id="main" style="height: 700px;"></div>
                </div>

            </div>
            <div class="col-lg-5">

                <div class="panel" style="background-color: white;">

                    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
                    <div id="pie1" style="height: 300px;"></div>


                </div>

                <div class="panel" style="background-color: white;">

                    <div class="panel-heading text-center" style="font-weight: bold; font-size: 20px;">
                        智能物联网
                    </div>
                    <div class="panel-body">
                        <!-- 这里是放图片的  -->
                        <img style="height: 300px; width: 100%;" src="/res/SmartHome.gif" />
                    </div>
                </div>




            </div>

        </div>

        <div class="row">
            <div class="col-lg-7">
                <div class="panel" style="background-color: white;">

                    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->

                    <div id="main_chart_1" style="height: 300px; width: 100%; margin-top: 30px;" class="mi-newline"></div>
                </div>
                <div class="panel" style="background-color: white;">

                    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->

                    <div id="main_chart_2" style="height: 300px; width: 100%; margin-top: 30px;" class="mi-newline"></div>
                </div>
            </div>
            <div class="col-lg-5">

                <div class="panel" style="background-color: white;">

                    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->

                    <div id="pie_chart1" style="height: 300px; width: 100%; margin-top: 30px;" class="mi-newline"></div>

                </div>

                <div class="panel" style="background-color: white;">

                    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->

                    <div id="pie_chart2" style="height: 300px; width: 100%; margin-top: 30px;" class="mi-newline"></div>

                </div>

            </div>

        </div>

        <div class="row">

            <div class="col-lg-12">
                <div class="panel" style="background-color: white;">

                    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->

                    <div id="main_chart_3" style="height: 300px; width: 100%; margin-top: 30px;" class="mi-newline"></div>

                </div>
            </div>

        </div>



    </div>




</body>
</html>

<script type="text/javascript">
    


    function setChart() {


        // 基于准备好的dom，初始化echarts图表
        var myChart = echarts.init(document.getElementById('main'));

        var myPie1 = echarts.init(document.getElementById('pie1'));


        var option = {
            title: {
                text: '生产压力预测图'
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: ['数量', '金额']
            },
            dataZoom: {
                show: true,
                start: 0
            },
            toolbox: {
                show: true
            },
            calculable: true,
            xAxis: [
                {
                    type: 'category',
                    data: <%=m_dateList %>,
                    }
                ],
                yAxis: [
                    {
                        type: 'value'
                    }
                ],
                series: [
                    {
                        name: '数量',
                        type: 'line',
                        smooth: true,              //平滑线条
                        data: <%= m_munberList%>,
                        draggable:false,          //是否允许拖动
                        markPoint: {
                            data: [
                                { type: 'max', name: '最大值' },
                                { type: 'min', name: '最小值' }
                            ]
                        }

                    },
                    {
                        name: '金额',
                        type: 'line',
                        smooth: true,            //平滑线条
                        data: <%= m_moneyList%>,
                        draggable:false,          //是否允许拖动
                        markPoint: {
                            data: [
                                { type: 'max', name: '最大值' },
                                { type: 'min', name: '最小值' }
                            ]
                        }
                    }
                ]
            };


                var optionPie1 = {
                    title : {
                        text: '库存分布图',
                        x:'center'
                    },
                    tooltip : {
                        trigger: 'item',
                        formatter: "{a} <br/>{b} : {c} ({d}%)"
                    },
                    legend: {
                        orient : 'vertical',
                        x : 'left',
                        data:<%= m_grapTitel %>,
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
                        data:<%=m_grapData %>,
                    }
                ]
            };




        // 为echarts对象加载数据
                myChart.setOption(option);
        // 为echarts对象加载数据
                myPie1.setOption(optionPie1);

            }


            function changeUrl(url,title){


                EcView.show(url,title);

            }

</script>


<script>


    $(function(){
    
        setChart();

        var chart_data1 =<%=chart1_data%>;
           

        setColumnChart1("main_chart_1",chart_data1);


        var chart_data2 = <%=chart2_data%>;
           
        setColumnChart2("main_chart_2",chart_data2);    

        var chart_data3 = <%=chart3_data%>;
           
        setColumnChart3("main_chart_3",chart_data3); 



    });

</script>

<script>


    function setColumnChart1(main_columnChart, chart_data) {



        //app.title = '坐标轴刻度与标签对齐';

        // 基于准备好的dom，初始化echarts实例
        var myColumnChart = echarts.init(document.getElementById(main_columnChart));

        console.log(chart_data);

        var columnOption = {
            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                },
                //formatter: '{a} <br/>{b} : {c}% '
            },
            title: {
                text:'销售出货情况'
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            legend: {
                data:["昨天","2016年同天","2015年同天"]
            },
            xAxis: [
                {
                    type: 'category',
                    data: chart_data.title,
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
                    name: '昨天',
                    type: 'bar',
                    data: chart_data.data1,
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
                },
                {
                    name: '2016年同天',
                    type: 'bar',
                    data: chart_data.data2,
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
                },
                {
                    name: '2015年同天',
                    type: 'bar',
                    data: chart_data.data3,
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
                },
            ]
        };


        // 使用刚指定的配置项和数据显示图表。
        myColumnChart.setOption(columnOption);


        //这是柱状图的点击按钮
        myColumnChart.on("click", function (params) {

            EcView.show("/App/InfoGrid2/View/MoreView/MoreViewPreview.aspx?id=206");

        });

    }


    function setColumnChart2(main_columnChart, chart_data) {



        //app.title = '坐标轴刻度与标签对齐';

        // 基于准备好的dom，初始化echarts实例
        var myColumnChart = echarts.init(document.getElementById(main_columnChart));

        console.log(chart_data);

        var columnOption = {
            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                },
                //formatter: '{a} <br/>{b} : {c}% '
            },
            title: {
                text:'销售退货情况'
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            legend: {
                data: chart_data.data_legend,
            },
            xAxis: [
                {
                    type: 'category',
                    data: chart_data.title,
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
                    name: '昨天',
                    type: 'bar',
                    data: chart_data.data1,
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
                },
                {
                    name: '最近30天',
                    type: 'bar',
                    data: chart_data.data2,
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
                },

            ]
        };


        // 使用刚指定的配置项和数据显示图表。
        myColumnChart.setOption(columnOption);


        //这是柱状图的点击按钮
        myColumnChart.on("click", function (params) {

            EcView.show("/App/InfoGrid2/View/MoreView/MoreViewPreview.aspx?id=207");

        });

    }



    function setColumnChart3(main_columnChart, chart_data) {



        //app.title = '坐标轴刻度与标签对齐';

        // 基于准备好的dom，初始化echarts实例
        var myColumnChart = echarts.init(document.getElementById(main_columnChart));

        console.log(chart_data);

        var columnOption = {
            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                },
                //formatter: '{a} <br/>{b} : {c}% '
            },
            title: {
                text:'采购情况'
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            legend: {
                data: ['最近30天','昨天']
            },
            xAxis: [
                {
                    type: 'category',
                    data: chart_data.title,
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
                    name: '最近30天',
                    type: 'bar',
                    data: chart_data.data1,
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
                },
                {
                    name: '昨天',
                    type: 'bar',
                    data: chart_data.data2,
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
                },

            ]
        };


        // 使用刚指定的配置项和数据显示图表。
        myColumnChart.setOption(columnOption);


        //这是柱状图的点击按钮
        myColumnChart.on("click", function (params) {

            EcView.show("/App/InfoGrid2/View/MoreView/MoreViewPreview.aspx?id=537");

        });

    }



</script>



<%--饼状图--%>
<script>

    $(function(){
  
        var myChart = echarts.init(document.getElementById('pie_chart1')); 


        

        option = {
            title: {
                text: '昨天销售出货情况',
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} "
            },
            legend: {
                orient: 'vertical',
                left: 'left',
                data:<%=pie_chart1_title%>,
  
            },
            label:{
                normal:{
                    formatter: '{b}\n{c} '
                }
            },

            series: [
                {
                    name: '数量',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data:<%=pie_chart1_data%>,
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

        myChart.setOption(option);

        myChart.on('click', function (params) {

            EcView.show("/App/InfoGrid2/View/MoreView/MoreViewPreview.aspx?id=206");

        });
        
    });



</script>


<script>

    $(function(){
  
        var myChart = echarts.init(document.getElementById('pie_chart2')); 


        

        option = {
            title: {
                text: '未生产销售订单占比率',
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} "
            },
            legend: {
                orient: 'vertical',
                left: 'left',

                data:<%=pie_chart2_title%>,
  
            },
            label:{
                normal:{
                    formatter: '{b}\n{c} '
                }
            },

            series: [
                {
                    name: '数量',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data:<%=pie_chart2_data%>,
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

        myChart.setOption(option);

        myChart.on('click', function (params) {

            EcView.show("/App/InfoGrid2/View/MoreView/MoreViewPreview.aspx?id=879");

        });
        
    });



</script>
