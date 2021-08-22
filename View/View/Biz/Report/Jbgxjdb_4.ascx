<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Jbgxjdb_4.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Report.Jbgxjdb_4" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<script src="/Core/Scripts/echarts/echarts.min.js"></script>

<link href="/Core/Scripts/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" />
<link href="/res/ace/ace.css" rel="stylesheet" />
<link href="/res/ace/font-awesome-4.4.0/css/font-awesome.min.css" rel="stylesheet" />
<link href="/res/ace/mx.css" rel="stylesheet" />

<form action="" id="form1" method="post">

<mi:Store runat="server" ID="store1" Model="" IdField="" PageSize="20" ReadOnly="true">

</mi:Store>

    <div class="main-content">

        <div class="main-content-inner">

            <div class="page-content">


                <div class="row">
                     <p style="font-size:30px">采购管理分析报表</p> 

                </div>

                <div class="row">

                    <div class="col-sm-12 col-xs-12">

                           <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
                           <div id="main_chart1" style="height: 260px; width: 100%;margin-top:20px;"></div>

                    </div>


<%--                    <div class="col-sm-6 col-xs-12">
                        <div class="widget-box" style="border-color:#ddd">
                                
                            <div class="widget-header widget-header-flat widget-header-small">
                                
                                <i class="ace-icon fa fa-signal"></i>
                                <h5 class="widget-title">待审核单据</h5>

                                <h5 class="widget-title" style="float:right;margin-right:20px;">
                                    <a href="#" onclick="Mini2.EcView.show('/app/InfoGrid2/View/OneFlow/UserFlowList.aspx?state=no_check', '流程列表');">更多...</a></h5>
                            </div>
                            <div class="" style="height:240px;overflow:auto;">
                                <table class="table table-hover ">
                                    <thead>
                                        <tr>                                            
                                            <th>单据类型</th> 
                                            <th>单号</th>
                                            <th>发起人</th>
                                            <th>其它信息</th>
                                            <th colspan="2">时间</th>
                                        </tr>
                                    </thead>
                                    <tbody id="flowData1" style="display:none;">
                                        <tr v-for="t in items">
                                            <td><a href="t.EXTEND_DOC_URL" @click="return viewTab(t)">{{t.EXTEND_DOC_TEXT}}</a></td>
                                            <td>
                                                <a :href="t.EXTEND_DOC_URL" @click="return viewTab(t)">{{t.EXTEND_BILL_CODE}}</a>
                                            </td>
                                            <td data-header="发起人">{{t.START_USER_TEXT}}</td>
                                            <td data-header="其它信息">
                                                {{t.CUR_NODE_TEXT}}<br />
                                                {{t.EXT_COL_VALUE_1}}<br />
                                                {{t.EXT_COL_VALUE_2}}
                                            </td>
                                            <td>{{ fromNow(t.ROW_DATE_CREATE)  }}</td>
                                            <td>{{ formatDate(t.ROW_DATE_CREATE) }}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>


                        </div>
                    </div>--%>

                </div>

                <div class="widget-box" style="border-color:#ddd;margin-top:20px;"></div>

                <div class="row" >

                    <div class="col-sm-12 col-md-12">

                        <div class="panel-stat3 " id="cm0101" style="margin-bottom: 5px; margin-top: 5px; padding-top: 15px; padding-bottom: 15px; height: 400px;">

                            <div id="main_chart2" style="height: 350px; width: 100%;"></div>

                        </div>


                    </div>

                </div>

                <div class="widget-box" style="border-color:#ddd;margin-top:20px;margin-bottom:50px;"></div>

                <div class="row" >

                    <div class="col-sm-12 col-md-12">

                        <div class="panel-stat3 " id="cm0102" style="margin-bottom: 5px; margin-top: 5px; padding-top: 15px; padding-bottom: 15px; height: 400px;">

                            <div id="main_chart3" style="height: 350px; width: 100%;"></div>

                        </div>


                    </div>

                </div>

                <div class="widget-box" style="border-color:#ddd;margin:20px 0px;"></div>

                <div class="row" >

                    <div class="col-sm-12 col-md-12">

                        <div class="panel-stat3 " id="cm0104" style="margin-bottom: 5px; margin-top: 5px; padding-top: 15px; padding-bottom: 15px; height: 400px;">

                            <div id="main_chart4" style="height: 350px; width: 100%;"></div>

                        </div>


                    </div>

                </div>


                <div class="widget-box" style="border-color:#ddd;margin:20px 0px;"></div>



                <div class="row" >

                    <div class="col-sm-12 col-md-12">

                        <div class="panel-stat3 " id="cm0105" style="margin-bottom: 5px; margin-top: 5px; padding-top: 15px; padding-bottom: 15px; height: 400px;">

                            <div id="main_chart5" style="height: 350px; width: 100%;"></div>

                        </div>


                    </div>

                </div>

              

            </div>

        </div>
    </div>




</form>

    <script src="/Core/Scripts/jquery/jquery-3.1.0.min.js"></script>
    <script src="/Core/Scripts/Mini2/Mini2.js"></script>

    <script src="/Core/Scripts/XYF/xyfUtil.js"></script>


<script>

    $(document).ready(function () {






        //柱状图数据
        var columnChart_data1 = <%=columnChart_data1 %>;

        setColumnChart1('main_chart1', columnChart_data1);

        var columnChart_data2 = <%=columnChart_data2 %>;


        setColumnChart2('main_chart2', columnChart_data2);



        //波浪图数据
        var waveChart_data = <%=columnChart_data3 %>

        setWaveChart('main_chart3', waveChart_data);


        var columnChart_data4 = <%=columnChart_data4%>

        setColumnChart4('main_chart4', columnChart_data4);



        var columnChart_data5 = <%=columnChart_data5 %>

        setColumnChart5('main_chart5', columnChart_data5);

        viewFlow();

    });


</script>


<script>

    function viewFlow() {

        var code = '<%= this.IsBuilder()?"FLOW_DOC_ALL":"FLOW_DOC_ME" %>'; 

        Mini2.get('/App/InfoGrid2/View/Biz/Core_Method/Action.aspx', {
            method: 'get',
            tag:'flow',
            code: code
        },
        function (data) {
            
            var my_vue = new Vue({
                el: '#flowData1',
                data: {
                    items: data
                },
                methods: {
                    viewTab: function (item) {
                        Mini2.EcView.show(item.EXTEND_DOC_URL, '流程审核');
                        Mini2.EventManager.stopEvent();

                        return false;
                    },

                    fromNow: function (item) {
                        return moment(item).fromNow();
                    },

                    formatDate: function (item) {
                        return Mini2.Date.format(new Date(item), 'm月d日 (l) H:i');
                    }
                },


            });

            $('#flowData1').show();

        });
    }




</script>





<script>



    function setColumnChart1(main_columnChart, columnChart_data) {


        console.log("setColumnChart1");
        console.log(columnChart_data);

        //app.title = '坐标轴刻度与标签对齐';

        // 基于准备好的dom，初始化echarts实例
        var myColumnChart = echarts.init(document.getElementById(main_columnChart));

        console.log(myColumnChart);

        var columnOption = {
            title: {
                text: '月度采购计划执行情况',

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
                data: ['数量', '下单数量']
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
                    name: '数量',
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
                    name: '下单数量',
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

            ],
            dataZoom: [
                {
                    type: 'slider',
                    show: true

                }
            ]
        };


        // 使用刚指定的配置项和数据显示图表。
        myColumnChart.setOption(columnOption);


        //这是柱状图的点击按钮
        myColumnChart.on("click", function (params) {

            EcView.show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id=1914");

        });

    }


    function setColumnChart2(main_columnChart, columnChart_data) {

        console.log(columnChart_data);


        // 基于准备好的dom，初始化echarts实例
        var myColumnChart = echarts.init(document.getElementById(main_columnChart));

        var data_complete =[];

        var columnOption = {
            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                },formatter:function(params, ticket, callback){


                    var text = "";

                    var data1 = 0;

                    var data2 = 0;

                    var text = "";

                    params.forEach(function(v,i){
                        
                        console.log("params=",v);

                        if(v.data){

                            text += v.seriesName +":"+v.data+"<br />";
                        }



                    });

                    var text_complete ="";


                    params.forEach(function(v,i){
                        
                        console.log("v=",v);

                        console.log("i=",i);

                        if(v.data){

                            if(v.seriesName=="数量"){

                                data1 = v.data;

                            }else{
                                data2 = v.data;
                            }

                            var complete = (data2 / data1 * 100).toFixed(2);   //计算机完成率,并保留两位小数

                            data_complete.push(complete);

                            text_complete = "完成率:" + complete + "%<br />";

                        }

                    });
                    

                    return text + text_complete;              
                }


                
            },
            title: {
                text: '年度采购计划执行情况',
                x: 'left'
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            legend: {
                data: ['数量','下单数量']
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
                    name: '数量',
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
                                formatter:'{c}',
                            }
                        }
                    }
                },
                {
                    name: '下单数量',
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

            EcView.show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id=1984");

        });

    }


    function setWaveChart(main_chart, chart_data) {


        console.log("setWaveChart");
        console.log(chart_data);


        var myChart = echarts.init(document.getElementById(main_chart));




        option = {
            title: {
                text: '价格波动报表（原材料类）'

            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: ['单价']
            },
            toolbox: {
                show: true,
                feature: {
                    dataZoom: {
                        yAxisIndex: 'none'
                    },
                    dataView: { readOnly: false },
                    magicType: { type: ['line', 'bar'] },
                    restore: {},
                    saveAsImage: {}
                }
            },
            xAxis: {
                type: 'category',
                boundaryGap: false,
                data: chart_data.title
            },
            yAxis: {
                type: 'value',
                axisLabel: {
                    formatter: '{value}'
                }
            },
            series: [
                {
                    name: '单价',
                    type: 'line',
                    data: chart_data.data1
                }
            ],
            dataZoom: [
                {
                    type: 'slider',
                    show: true

                }
            ]
        };


        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);

        myChart.on("click", function (params) {

            EcView.show("/App/InfoGrid2/View/Biz/Report/Jbgxjdb_9.aspx?id=1978");

        });


    }


    function setColumnChart4(main_columnChart, columnChart_data) {

        console.log(columnChart_data);


        // 基于准备好的dom，初始化echarts实例
        var myColumnChart = echarts.init(document.getElementById(main_columnChart));

        var columnOption = {
            title: {
                text: '最低库存预警',

                x: 'left'
            },
            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                }
            },
            //title: {
            //    text: '某站点用户访问来源'
            //},
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            legend: {
                data: ['库存', "最低库存"]

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
                    name: '库存',
                    type: 'bar',
                    data: columnChart_data.data,
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
                     name: '最低库存',
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
                 }

            ]
        };


        // 使用刚指定的配置项和数据显示图表。
        myColumnChart.setOption(columnOption);


        //这是柱状图的点击按钮
        myColumnChart.on("click", function (params) {

            EcView.show("/App/InfoGrid2/View/OneView/ViewPreview.aspx?id=1937");

        });

    }


    function setColumnChart5(main_columnChart, columnChart_data) {

        console.log(columnChart_data);


        // 基于准备好的dom，初始化echarts实例
        var myColumnChart = echarts.init(document.getElementById(main_columnChart));

        var columnOption = {
            title: {
                text: '最高库存预警',

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
                data: ['库存', '最高库存']
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
                    name: '库存',
                    type: 'bar',
                    data: columnChart_data.data,
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
                    name: '最高库存',
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
                }

            ]
        };


        // 使用刚指定的配置项和数据显示图表。
        myColumnChart.setOption(columnOption);


        //这是柱状图的点击按钮
        myColumnChart.on("click", function (params) {

            EcView.show("/App/InfoGrid2/View/OneTable/TablePreview.aspx?id=1938");

        });

    }


</script>