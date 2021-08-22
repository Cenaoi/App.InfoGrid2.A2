<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Jbgxjdb_6.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Report.Jbgxjdb_6" %>


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
             <p style="font-size:30px">销售报表</p> 

            </div>

            <div class="row">

                <div class="col-lg-6">

                    <div id="left_chart" style="height: 300px; width: 100%; float: left"></div>


                </div>
                <div class="col-lg-6">


                    <div id="right_chart" style="height: 300px; width: 100%; float: right"></div>

                </div>
            </div>






            <div class="row"  style="width:100%">

                  <div id="main_chart1" style="height: 200px; width: 100%;"></div>

             </div>
           



            <div class="row">

                 <div id="main_chart2" style="height: 350px; width: 100%;"></div>

              </div>






        </div>


    </div>

</div>





<%--饼状图--%>
<script>

    $(function () {




        option = {
            title: {
                text: '客户销售额分析',
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            legend: {
                orient: 'vertical',
                left: 'left',

               data:[<%=smlisttitle%>]
               //data: ['直接访问','邮件营销','联盟广告','视频广告','搜索引擎']
  
            },
            label:{
                normal:{
                    formatter: '{b}\n{c}({d}%)'
                }
            },

            series: [
                {
                    name: '合同类型',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data:<%=smlistji%>,
            //         data:[
            //    {value:335, name:'直接访问'},
            //    {value:310, name:'邮件营销'},
            //    {value:234, name:'联盟广告'},
            //    {value:135, name:'视频广告'},
            //    {value:1548, name:'搜索引擎'}
            //],

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

            EcView.show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id="+1912);

        });

    });

       

    
</script>


<script>


           option = {
            title: {
                text: '产品销售分析',
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            legend: {
                orient: 'vertical',
                left: 'left',
                data:[<%=yingsk%>]
               //data: ['直接访问','邮件营销','联盟广告','视频广告','搜索引擎']
  
            },
            label:{
                normal:{
                    formatter: '{b}\n{c} ({d}%)'
                }
            },

            series: [
                {
                    name: '产品类型',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data:<%=yingsklist%>,
            //         data:[
            //    {value:335, name:'直接访问'},
            //    {value:310, name:'邮件营销'},
            //    {value:234, name:'联盟广告'},
            //    {value:135, name:'视频广告'},
            //    {value:1548, name:'搜索引擎'}
            //],

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



        var myChart = echarts.init(document.getElementById('right_chart'));
        myChart.setOption(option);
        myChart.on('click', function (params) {

            EcView.show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id="+1913);

        });



</script>



<script>

    $(document).ready(function () {





        //柱状图数据
        var columnChart_data1 = <%=columnChart_data1 %>

        setColumnChart1('main_chart1', columnChart_data1);

        var columnChart_data2 = <%=columnChart_data2 %>

        setColumnChart2('main_chart2', columnChart_data2);



        //波浪图数据
        //var waveChart_data = {

        //    title: ['周一', '周二', '周三', '周四', '周五', '周六', '周日'],
        //    data: [11, 11, 15, 13, 12, 13, 10]

        //}

        //setWaveChart('main_chart3', waveChart_data);

    });


</script>

<script>
    function setColumnChart1(main_columnChart, columnChart_data) {


        //app.title = '坐标轴刻度与标签对齐';

        // 基于准备好的dom，初始化echarts实例
        var myColumnChart = echarts.init(document.getElementById(main_columnChart));

        console.log(myColumnChart);

        var columnOption = {
            title: {
                text: '月度销售计划执行情况表',
                x: 'left'
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
            legend: {
                data: ['金额', '订单金额']
            },
            xAxis: [
                {
                    type: 'category',
                    data: columnChart_data.title,
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
                    name: '金额',
                    type: 'bar',
                    data: columnChart_data.data1,
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
                    name: '订单金额',
                    type: 'bar',
                    data: columnChart_data.data2,
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


        // 使用刚指定的配置项和数据显示图表。
        myColumnChart.setOption(columnOption);


        //这是柱状图的点击按钮
        myColumnChart.on("click", function (params) {

            EcView.show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id=1911");

        });

    }


    function setColumnChart2(main_columnChart, columnChart_data) {

        console.log(columnChart_data);


        // 基于准备好的dom，初始化echarts实例
        var myColumnChart = echarts.init(document.getElementById(main_columnChart));

        var columnOption = {
            title: {
                text: '年度销售计划执行情况表',
                x: 'left'
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
            legend: {
                data: ['金额', '订单金额']
            },
            xAxis: [
                {
                    type: 'category',
                    data: columnChart_data.title,
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
                    name: '金额',
                    type: 'bar',
                    data: columnChart_data.data1,
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
                    name: '订单金额',
                    type: 'bar',
                    data: columnChart_data.data2,
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


        // 使用刚指定的配置项和数据显示图表。
        myColumnChart.setOption(columnOption);


        //这是柱状图的点击按钮
        myColumnChart.on("click", function (params) {

            EcView.show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id=1911");

        });

    }
</script>