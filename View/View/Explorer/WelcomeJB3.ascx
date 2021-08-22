<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WelcomeJB3.ascx.cs" Inherits="App.InfoGrid2.View.Explorer.WelcomeJB3" %>

<link href="/Core/Scripts/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" />
<link href="/res/ace/ace.css" rel="stylesheet" />
<link href="/res/ace/font-awesome-4.4.0/css/font-awesome.min.css" rel="stylesheet" />
<link href="/res/ace/mx.css" rel="stylesheet" />

<div class="main-container" id="main-container">

    <div id="sidebar" class="sidebar responsive">

        <ul class="nav nav-list">

            <%= GetHtmlCommonMenu() %>

        </ul>
        <div class="sidebar-toggle sidebar-collapse" id="sidebar-collapse">
		    <i class="ace-icon fa fa-angle-double-left" data-icon1="ace-icon fa fa-angle-double-left" data-icon2="ace-icon fa fa-angle-double-right"></i>
		</div>
    </div>

    <div class="main-content">

        <div class="main-content-inner">

            <div class="page-content">

                <div class="row">

                    <div class="col-sm-6 col-md-3">

                        <div class="panel-stat3 no-radius btn-danger" id="cm0101" style="margin-bottom: 5px; margin-top: 5px; padding-top: 15px; padding-bottom: 15px; height: 80px;">

                            <div style="height: 41px;"  onclick="viewTab(this)"  data-text="未出订单"  data-href="/App/InfoGrid2/View/OneView/ViewPreview.aspx?id=1532">
                                <h2 class="m-top-none" id="SUM_UT091" style="margin-bottom: 0px;">0</h2>
                                <h5 style="margin-top: 2px; margin-bottom: 0px;">未出订单</h5>
                            </div>
                            <%--                            
                            <div style="height: 41px; margin-top: 20px;" onclick="addTabByCountMark('0e750604-8c1a-4297-896d-d1ec8de6ad86','1')">

                                <h2 class="m-top-none" id="h2010102" style="margin-bottom: 0px;">1,784,219.86</h2>
                                <h5 style="margin-top: 2px; margin-bottom: 0px;">库存成本</h5>

                            </div>
                            --%>
                            <div class="stat-icon"><i class="fa fa-3x fa-cubes"></i></div>
                            <div class="refresh-button" onclick="execBizMethod('SUM_UT091','SUM_UT091');"><i class="fa fa-refresh"></i></div>
                            <div class="loading-overlay"><i class="loading-icon fa fa-refresh fa-spin fa-lg"></i></div>
                        </div>


                    </div>
                    <div class="col-sm-6 col-md-3">
                        <div class="panel-stat3 no-radius btn-info" id="cm0202" style="margin-bottom: 5px; margin-top: 5px; padding-top: 15px; padding-bottom: 15px; height: 80px;">
                            <div style="height: 41px;" onclick="viewTab(this)"  data-text="车间在产" data-href="/App/InfoGrid2/View/OneView/ViewPreview.aspx?id=1534">
                                <h2 class="m-top-none" id="SUM_UT281" style="margin-bottom: 0px;">0</h2>
                                <h5 style="margin-top: 2px; margin-bottom: 0px;">车间在产</h5>
                            </div>

                            <div class="stat-icon"><i class="fa fa-3x fa-dollar"></i></div>
                            <div class="refresh-button" onclick="execBizMethod('SUM_UT281','SUM_UT281');"><i class="fa fa-refresh"></i></div>
                            <div class="loading-overlay"><i class="loading-icon fa fa-refresh fa-spin fa-lg"></i></div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-md-3">
                        <div class="panel-stat3 no-radius btn-warning" id="cm0402" style="margin-bottom: 5px; margin-top: 5px; padding-top: 15px; padding-bottom: 15px; height: 80px;">
                            <div style="height: 41px;" onclick="viewTab(this)"  data-text="采购在途 "  data-href="/App/InfoGrid2/View/OneTable/TablePreview.aspx?id=1533" >
                                <h2 class="m-top-none" id="SUM_UT133" style="margin-bottom: 0px;">0</h2>
                                <h5 style="margin-top: 2px; margin-bottom: 0px;">采购在途 </h5>
                            </div>
                            <div class="stat-icon"><i class="fa fa-3x fa-cny"></i></div>
                            <div class="refresh-button" onclick="execBizMethod('SUM_UT133','SUM_UT133');"><i class="fa fa-refresh"></i></div>
                            <div class="loading-overlay"><i class="loading-icon fa fa-refresh fa-spin fa-lg"></i></div>
                        </div>
                    </div>

                    <div class="col-sm-6 col-md-3">
                        <div class="panel-stat3 no-radius btn-success" id="cm0403" style="margin-bottom: 5px; margin-top: 5px; padding-top: 15px; padding-bottom: 15px; height: 80px;">
                            <div style="height: 41px;" onclick="viewTab(this)"  data-text="外发未归"  data-href="/App/InfoGrid2/View/OneView/ViewPreview.aspx?id=1535" >
                                <h2 class="m-top-none" id="SUM_UT243" style="margin-bottom: 0px;">0</h2>
                                <h5 style="margin-top: 2px; margin-bottom: 0px;">外发未归</h5>
                            </div>

                            <div class="stat-icon"><i class="fa fa-3x fa-cny"></i></div>
                            <div class="refresh-button" onclick="execBizMethod('SUM_UT243','SUM_UT243');"><i class="fa fa-refresh"></i></div>
                            <div class="loading-overlay"><i class="loading-icon fa fa-refresh fa-spin fa-lg"></i></div>
                        </div>
                    </div>

                </div>

                <div class="hr hr-15 col-xs-12 " style="border-top-color:#ededed"></div>

                <div class="row">
                    <div class="col-sm-6 col-xs-12">
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
                    </div>
                    

                    <div class="col-sm-6 col-xs-12">
                        <div class="widget-box" style="border-color:#ddd">
                                
                            <div class="widget-header widget-header-flat widget-header-small">
                                
                                <i class="ace-icon fa fa-calendar"></i>
                                <h5 class="widget-title">新闻 - 公告</h5>
                            </div>
                            <div class="" style="height:240px;overflow:auto;padding:6px;" id="cmsItems1">
                                <div v-for="i in items"  style=" width:100%; overflow :hidden; margin-bottom:10px;">
                                    <a href="#" @click="viewTab(i)">
                                        <div style="float:left; width:19%;"><img :src="i.C_IMAGE_URL" alt="" style="width:100%" /></div>
                                        <div style="float:left; width:79%; padding-left:10px; overflow:hidden; ">
                                            <h4>{{i.C_TITLE}}</h4>
                                            {{i.C_INTRO}}
                                        </div>
                                    </a>
                                </div>

                            </div>


                        </div>
                    </div>



                </div>
                
                <div class="hr hr-15 col-xs-12 " style="border-top-color:#ededed"></div>
                
                <div class="row">
                    <div class="col-lg-12">
                        <div class="widget-box" style="background-color: white;">
                            <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
                            <div id="main" style="height: 700px;"></div>
                        </div>

                    </div>
                </div>

                <div class="hr hr-15 col-xs-12 " style="border-top-color:#ededed"></div>

                <!--放图片的这一行-->
                <div class="row">
                    <div class="col-lg-6">
                        <div class="widget-box" style="background-color:white;">
                            <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
                            <div id="tempFlow1" style="height:600px;"></div>
                        </div>
                
                    </div>
                    <div class="col-lg-6">
                        <div class="widget-box" style="background-color:white;">
      
                            <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
                            <div id="tempFlow2" style="height:300px;"></div>

                        </div>

                         <div class="widget-box" style="background-color:white;">

                                                      
                            <div class="panel-heading text-center" style="font-weight:bold;font-size:20px;">
                                 物联网控制台
                             </div>
                          
                                <!-- 这里是放图片的  -->
                            <img  style="width:100%;" src="/res/SmartHome.gif" />
                       
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

        </div>

    </div>

</div>

<script src="/Core/Scripts/bootstrap/3.3.4/js/bootstrap.min.js"></script>


<script type="text/javascript">



    function setChart() {


        // 基于准备好的dom，初始化echarts图表
        var myChart = echarts.init(document.getElementById('main'));

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
                    draggable: false,          //是否允许拖动
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
                        draggable: false,          //是否允许拖动
                        markPoint: {
                            data: [
                                { type: 'max', name: '最大值' },
                                { type: 'min', name: '最小值' }
                            ]
                        }
                }
            ]
        };

        // 为echarts对象加载数据
        myChart.setOption(option);

    }


    function changeUrl(url, title) {


        EcView.show(url, title);

    }

</script>


<script>

    $(function () {

        setChart();

        var chart_data1 =<%=chart1_data%>;
          
        setColumnChart1("main_chart_1", chart_data1);

        var chart_data2 = <%=chart2_data%>;
           
        setColumnChart2("main_chart_2", chart_data2);

        var chart_data3 = <%=chart3_data%>;
           
        setColumnChart3("main_chart_3", chart_data3); 

    });

</script>

<script>


    var m_LastActiveEls = null;

    $(function () {
        $('body').addClass('no-skin');

        var itemEls = $('#sidebar').find('.dropdown-toggle');

        itemEls.click(function () {
            var parentEl = $(this).parent();

            var submenuEl = $(this).nextAll('.submenu');

            if (submenuEl.hasClass('nav-hide')) {
                submenuEl.slideDown('normal', function () {
                    submenuEl.removeClass('nav-hide');
                    submenuEl.addClass('nav-show');
                });

                $(parentEl).addClass('open');

            }
            else {
                submenuEl.slideUp('normal', function () {
                    submenuEl.removeClass('nav-show');
                    submenuEl.addClass('nav-hide');
                });

                $(parentEl).removeClass('open');
            }




            var parentEls = $(this).parents('li');


            $(m_LastActiveEls).removeClass('active');

            parentEls.addClass('active');

            m_LastActiveEls = parentEls;

            var href = $(this).attr('href');

            if (href == '#') {
                //var url = '/App/InfoGrid2/View/Explorer/Menu_v201608.aspx?p_id=' + item.ID;

                //Mini2.EcView.show(url, $(this).text());
            }
            else {
                Mini2.EcView.show(href, $(this).text());
            }


            return false;
        });


    });


    function execBizMethod(method_code, renderEl) {

        Mini2.get('/App/InfoGrid2/View/Biz/Core_Method/Action.aspx', {
            method: 'get',
            code: method_code
        },
        function (data) {
            $('#' + renderEl).html(data);
        });

    }


    function viewTab(sender) {

        var href = $(sender).attr('data-href');
        var text = $(sender).attr('data-text');

        console.debug("跳转地址");

        Mini2.EcView.show(href, text);

        return false;
    }

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


    function viewCMS() {

        Mini2.get('/App/InfoGrid2/View/CMS/CmsHandler.aspx?action=get_list', 
        function (data) {

            var my_vue = new Vue({
                el: '#cmsItems1',
                data: {
                    items: data
                },
                methods: {

                    viewTab: function (item) {

                        var url = '/App/InfoGrid2/View/Cms/CmsView.aspx?id=' + item.CMS_ITEM_ID;

                        Mini2.EcView.show(url, '公告');
                        Mini2.EventManager.stopEvent();
                    }
                }

            });


        });
    }


    $(function () {

        execBizMethod('SUM_UT091', 'SUM_UT091');
        execBizMethod('SUM_UT281', 'SUM_UT281');
        execBizMethod('SUM_UT133', 'SUM_UT133');
        execBizMethod('SUM_UT243', 'SUM_UT243');


        viewFlow();

        viewCMS();

    });

</script>



<%--<script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts/echarts-all-3.js"></script>--%>
<script src="/Core/Scripts/echarts/echarts.min.js"></script>

<script>


    var app = {};

    $(function () {

        var option = {
            title: {
                text: '车间生产进度',
                subtext: '--'
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: ['物料消耗', '工序']
            },
            toolbox: {
                show: true,
                feature: {
                    dataView: { readOnly: false },
                    restore: {},
                    saveAsImage: {}
                }
            },
            dataZoom: {
                show: false,
                start: 0,
                end: 100
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function () {
                        var now = new Date();
                        var res = [];
                        var len = 10;
                        while (len--) {
                            res.unshift(now.toLocaleTimeString().replace(/^\D*/, ''));
                            now = new Date(now - 2000);
                        }
                        return res;
                    })()
                },
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function () {
                        var res = [];
                        var len = 10;
                        while (len--) {
                            res.push(len + 1);
                        }
                        return res;
                    })()
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    scale: true,
                    name: '时间',
                    max: 30,
                    min: 0,
                    boundaryGap: [0.2, 0.2]
                },
                {
                    type: 'value',
                    scale: true,
                    name: '物料使用量',
                    max: 1200,
                    min: 0,
                    boundaryGap: [0.2, 0.2]
                }
            ],
            series: [
                {
                    name: '工序',
                    type: 'bar',
                    xAxisIndex: 1,
                    yAxisIndex: 1,
                    data: (function () {
                        var res = [];
                        var len = 10;
                        while (len--) {
                            res.push(Math.round(Math.random() * 1000));
                        }
                        return res;
                    })()
                },
                {
                    name: '物料消耗',
                    type: 'line',
                    data: (function () {
                        var res = [];
                        var len = 0;
                        while (len < 10) {
                            res.push((Math.random() * 10 + 5).toFixed(1) - 0);
                            len++;
                        }
                        return res;
                    })()
                }
            ]
        };


        var dom = document.getElementById("tempFlow1");
        var myChart = echarts.init(dom);


        clearInterval(app.timeTicket);
        app.count = 11;
        app.timeTicket = setInterval(function () {
            axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');

            var data0 = option.series[0].data;
            var data1 = option.series[1].data;
            data0.shift();
            data0.push(Math.round(Math.random() * 1000));
            data1.shift();
            data1.push((Math.random() * 10 + 5).toFixed(1) - 0);

            option.xAxis[0].data.shift();
            option.xAxis[0].data.push(axisData);
            option.xAxis[1].data.shift();
            option.xAxis[1].data.push(app.count++);

            myChart.setOption(option);
        }, 2100);


        //myChart.setOption(option, true);

    });

    $(function () {
        return;

        var option = {
            title: {
                text: '未来一周气温变化',
                subtext: '纯属虚构'
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: ['最高气温', '最低气温']
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
                data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
            },
            yAxis: {
                type: 'value',
                axisLabel: {
                    formatter: '{value} °C'
                }
            },
            series: [
                {
                    name: '最高气温',
                    type: 'line',
                    data: [11, 11, 15, 13, 12, 13, 10],
                    markPoint: {
                        data: [
                            { type: 'max', name: '最大值' },
                            { type: 'min', name: '最小值' }
                        ]
                    },
                    markLine: {
                        data: [
                            { type: 'average', name: '平均值' }
                        ]
                    }
                },
                {
                    name: '最低气温',
                    type: 'line',
                    data: [1, -2, 2, 5, 3, 2, 0],
                    markPoint: {
                        data: [
                            { name: '周最低', value: -2, xAxis: 1, yAxis: -1.5 }
                        ]
                    },
                    markLine: {
                        data: [
                            { type: 'average', name: '平均值' },
                            [{
                                symbol: 'none',
                                x: '90%',
                                yAxis: 'max'
                            }, {
                                symbol: 'circle',
                                label: {
                                    normal: {
                                        position: 'start',
                                        formatter: '最大值'
                                    }
                                },
                                type: 'max',
                                name: '最高点'
                            }]
                        ]
                    }
                }
            ]
        };


        var dom = document.getElementById("tempFlow1");
        var myChart = echarts.init(dom);

        myChart.setOption(option, true);
    });

    $(function () {

        option = {
            title: {
                text: '库存分布图',
                subtext: '----',
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            legend: {
                orient: 'vertical',
                left: 'left',
                data: ['原材料仓', '辅料仓', '五金仓', '备件仓', '成品仓']
            },
            series: [
                {
                    name: '访问来源',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data: [
                        { value: 335, name: '原材料仓' },
                        { value: 310, name: '辅料仓' },
                        { value: 234, name: '五金仓' },
                        { value: 135, name: '备件仓' },
                        { value: 1548, name: '成品仓' }
                    ],
                    itemStyle: {
                        emphasis: {
                            shadowBlur: 10,
                            shadowOffsetX: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        },
                        normal: {
                            label: {
                                show: true,
                                position: 'top',
                                formatter: '{b} {c}'
                            },
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        }
                    }
                }
            ]
        };



        var dom = document.getElementById("tempFlow2");
        var myChart = echarts.init(dom);

        myChart.setOption(option, true);
    });
</script>


<%--饼状图--%>
<script>

    $(function () {

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
            label: {
                normal: {
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

    $(function () {

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
            label: {
                normal: {
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
                text: '销售出货情况'
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            legend: {
                data: ["昨天", "2016年同天", "2015年同天"]
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
                text: '销售退货情况'
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
                text: '采购情况'
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            legend: {
                data: ['最近30天', '昨天']
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


