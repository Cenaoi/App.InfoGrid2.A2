<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Jbgxjdb_7.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Report.Jbgxjdb_7" %>
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
               <p style="font-size:30px">费用报销报表</p> 

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

                <div id="main_chart3" style="height: 300px; width: 100%; margin-top:70px;"></div>


            </div>








        </div>


    </div>

</div>




<%--饼状图--%>
<script>


    $(function(){


        option = {
            title: {
                text: '部门报销情况',
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} "
            },
            legend: {
                orient: 'vertical',
                left: 'left',

                data:[<%=smlisttitle%>],
  
            },
            label:{
                normal:{
                    formatter: '{b}\n报销金额：{c} '
                }
            },

            series: [
                {
                    name: '报销金额',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data:<%=smlistji%>,

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

            EcView.show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id="+1898);

        });

    });



       

    
</script>

<%--饼状图--%>
<script>

    $(function(){
  
        var myChart = echarts.init(document.getElementById('right_chart')); 


        

        option = {
            title: {
                text: '费用报销情况',
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} "
            },
            legend: {
                orient: 'vertical',
                left: 'left',

                data:[<%=smlisttitle2%>],
  
            },
            label:{
                normal:{
                    formatter: '{b}\n{c} '
                }
            },

            series: [
                {
                    name: '费用类型',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data:<%=smlistji2%>,

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

            EcView.show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id="+1979);

        });
        
    });



</script>



<script>


    $(function(){
    

        var chart_data3 = <%=chart_data3 %>

        setChart3('main_chart3', chart_data3);
    
    
    
    
    
    });








</script>



<script>


    function setChart3(main_columnChart, columnChart_data) {

        console.log("123");
        console.log(columnChart_data);


        // 基于准备好的dom，初始化echarts实例
        var myColumnChart = echarts.init(document.getElementById(main_columnChart));


        var series = columnChart_data.series;

   
        series.forEach(function(v,i){
        
            v["itemStyle"] = {
                normal: {
                    label: {
                        show: true,
                        position: 'top',
                        formatter:  function (v) {

                            var text = "";

                                if(v.data){

                                    text += v.seriesName +":"+v.data[1];
                                }



                            return text;
                        }
                    },
                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                }
            }


            console.log(v["data"]);

            v["data"] = eval("("+v["data"]+")");


            console.log(v["data"]);

        });

        console.log("series=",series);





        var columnOption = {
            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                },
                formatter: 
                    function (params, ticket, callback) {

                        var text = "";

                        params.forEach(function(v,i){
                        
                            console.log("params=",v);

                            if(v.data){

                                text += v.seriesName +":"+v.data[1]+"<br />";
                            }



                        });
                        return text;
                    }
            },
            title: {
                text:'月度差旅费统计表'
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            legend: {
                data: columnChart_data.data6,
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
            series: series
        };


        // 使用刚指定的配置项和数据显示图表。
        myColumnChart.setOption(columnOption);


        //这是柱状图的点击按钮
        myColumnChart.on("click", function (params) {

            EcView.show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id=1980");

        });

    }




</script>


<%--柱状图--%>
<script>


    option = {
        title: {
            text: '逾期未收款',
            subtext: '纯属虚构',
            x: 'center'
        },
        color: ['#3398DB'],
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
                name: '直接访问',
                type: 'bar',
                barWidth: '60%',
                data:[<%=weifuje%>]
           
            }
        ]
    };

    var myChart = echarts.init(document.getElementById('zhuont_chart'));

    myChart.setOption(option);

    myChart.on('click',function(){
        EcView.show("/App/InfoGrid2/View/OneView/ViewPreview.aspx?id="+1168);
    });

</script>



<%--柱状图--%>
<script>

    option = {
        title: {
            text: '逾期未付款',
            subtext: '纯属虚构',
            x: 'center'
        },
        color: ['#3398DB'],
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
                name: '直接访问',
                type: 'bar',
                barWidth: '60%',
                data:[<%=weifujew%>]
           
            }
        ]
    };

    var myChart = echarts.init(document.getElementById('zhutwo_chart'));

    myChart.setOption(option);

    myChart.on('click',function(){

        EcView.show("/App/InfoGrid2/View/OneView/ViewPreview.aspx?id="+1168);
    
    });

</script>
