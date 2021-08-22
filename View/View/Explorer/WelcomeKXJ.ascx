<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WelcomeKXJ.ascx.cs" Inherits="App.InfoGrid2.View.Explorer.WelcomeKXJ" %>


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

                    <div class="col-sm-6 col-md-4">

                        <div class="panel-stat3 no-radius btn-danger" id="cm0101" style="margin-bottom: 5px; margin-top: 5px; padding-top: 15px; padding-bottom: 15px; height: 80px;">

                            <div style="height: 41px;"  onclick2="viewTab(this)"  data-text="收录总人数"  data-href="/App/InfoGrid2/View/OneView/ViewPreview.aspx?id=1532">
                                <h2 class="m-top-none" id="SUM_UT005" style="margin-bottom: 0px;">0</h2>
                                <h5 style="margin-top: 2px; margin-bottom: 0px;">收录总人数</h5>
                            </div>                                           
                            
                            <div class="stat-icon"><i class="fa fa-3x fa-cubes"></i></div>
                            <div class="refresh-button" onclick="execBizMethod('SUM_UT005','SUM_UT005');"><i class="fa fa-refresh"></i></div>
                            <div class="loading-overlay"><i class="loading-icon fa fa-refresh fa-spin fa-lg"></i></div>
                        </div>


                    </div>
                    <div class="col-sm-6 col-md-4">
                        <div class="panel-stat3 no-radius btn-info" id="cm0202" style="margin-bottom: 5px; margin-top: 5px; padding-top: 15px; padding-bottom: 15px; height: 80px;">
                            <div style="height: 41px;" onclick2="viewTab(this)"  data-text="高级职称人数" data-href="/App/InfoGrid2/View/OneView/ViewPreview.aspx?id=1534">
                                <h2 class="m-top-none" id="SUM_UT005_GJ" style="margin-bottom: 0px;">0</h2>
                                <h5 style="margin-top: 2px; margin-bottom: 0px;">高级职称人数</h5>
                            </div>

                            <div class="stat-icon"><i class="fa fa-3x fa-dollar"></i></div>
                            <div class="refresh-button" onclick="execBizMethod('SUM_UT005_GJ','SUM_UT005_GJ');"><i class="fa fa-refresh"></i></div>
                            <div class="loading-overlay"><i class="loading-icon fa fa-refresh fa-spin fa-lg"></i></div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-md-4">
                        <div class="panel-stat3 no-radius btn-warning" id="cm0402" style="margin-bottom: 5px; margin-top: 5px; padding-top: 15px; padding-bottom: 15px; height: 80px;">
                            <div style="height: 41px;" onclick2="viewTab(this)"  data-text="信息员人数 "  data-href="/App/InfoGrid2/View/OneTable/TablePreview.aspx?id=1533" >
                                <h2 class="m-top-none" id="SUM_EMPLOYEE" style="margin-bottom: 0px;">0</h2>
                                <h5 style="margin-top: 2px; margin-bottom: 0px;">信息员人数 </h5>
                            </div>
                            <div class="stat-icon"><i class="fa fa-3x fa-cny"></i></div>
                            <div class="refresh-button" onclick="execBizMethod('SUM_EMPLOYEE','SUM_EMPLOYEE');"><i class="fa fa-refresh"></i></div>
                            <div class="loading-overlay"><i class="loading-icon fa fa-refresh fa-spin fa-lg"></i></div>
                        </div>
                    </div>

<%--                    <div class="col-sm-6 col-md-3">
                        <div class="panel-stat3 no-radius btn-success" id="cm0403" style="margin-bottom: 5px; margin-top: 5px; padding-top: 15px; padding-bottom: 15px; height: 80px;">
                            <div style="height: 41px;" onclick="viewTab(this)"  data-text="外发未归"  data-href="/App/InfoGrid2/View/OneView/ViewPreview.aspx?id=1535" >
                                <h2 class="m-top-none" id="SUM_UT243" style="margin-bottom: 0px;">0</h2>
                                <h5 style="margin-top: 2px; margin-bottom: 0px;">外发未归</h5>
                            </div>

                            <div class="stat-icon"><i class="fa fa-3x fa-cny"></i></div>
                            <div class="refresh-button" onclick="execBizMethod('SUM_UT243','SUM_UT243');"><i class="fa fa-refresh"></i></div>
                            <div class="loading-overlay"><i class="loading-icon fa fa-refresh fa-spin fa-lg"></i></div>
                        </div>
                    </div>--%>

                </div>

                <div class="hr hr-15 col-xs-12 " style="border-top-color:#ededed"></div>

                <div class="row">

                    <div class="col-sm-6 col-xs-12">
                        <div class="widget-box" style="border-color:#ddd">
                                
                            <div class="widget-header widget-header-flat widget-header-small">
                                
                                <i class="ace-icon fa fa-calendar"></i>
                                <h5 class="widget-title">产业分类</h5>
                            </div>
                            <div id="tempFlow1" class="" style="height:340px;overflow:auto;">
                                
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-6 col-xs-12">
                        <div class="widget-box" style="border-color:#ddd">
                                
                            <div class="widget-header widget-header-flat widget-header-small">
                                
                                <i class="ace-icon fa fa-signal"></i>
                                <h5 class="widget-title">拥有重要头衔的人数</h5>

<%--                                <h5 class="widget-title" style="float:right;margin-right:20px;">
                                    <a href="#" onclick="Mini2.EcView.show('/app/InfoGrid2/View/OneFlow/UserFlowList.aspx?state=no_check', '流程列表');">更多...</a></h5>--%>
                            </div>
                            <div id="tempFlow2" class="" style="height:340px;overflow:auto;">
                                
                            </div>


                        </div>
                    </div>


                </div>

                <div class="row">

                    <div class="col-sm-6 col-xs-12">
                        <div class="widget-box" style="border-color:#ddd">
                                
                            <div class="widget-header widget-header-flat widget-header-small">
                                
                                <i class="ace-icon fa fa-calendar"></i>
                                <h5 class="widget-title">数据完整性:</h5>
                            </div>
                            <div id="tempFlow3" class="" style="height:340px;overflow:auto;">
                                
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-6 col-xs-12">
                        <div class="widget-box" style="border-color:#ddd">
                                
                            <div class="widget-header widget-header-flat widget-header-small">
                                
                                <i class="ace-icon fa fa-signal"></i>
                                <h5 class="widget-title">任职地域比例</h5>

<%--                                <h5 class="widget-title" style="float:right;margin-right:20px;">
                                    <a href="#" onclick="Mini2.EcView.show('/app/InfoGrid2/View/OneFlow/UserFlowList.aspx?state=no_check', '流程列表');">更多...</a></h5>--%>
                            </div>
                            <div id="tempFlow4" class="" style="height:340px;overflow:auto;">
                                
                            </div>


                        </div>
                    </div>


                </div>
                <div class="hr hr-15 col-xs-12 " style="border-top-color:#ededed"></div>
     

            </div>

        </div>

    </div>

</div>

<script src="/Core/Scripts/bootstrap/3.3.4/js/bootstrap.min.js"></script>


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


        Mini2.EcView.show(href, text);
    }

    function viewFlow() {


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

        execBizMethod('SUM_UT005', 'SUM_UT005');
        execBizMethod('SUM_UT005_GJ', 'SUM_UT005_GJ');
        execBizMethod('SUM_EMPLOYEE', 'SUM_EMPLOYEE');
        //execBizMethod('SUM_UT243', 'SUM_UT243');


        viewFlow();

        viewCMS();

    });

</script>



<%--<script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts/echarts-all-3.js"></script>--%>
<script src="/Core/Scripts/echarts/echarts.min.js"></script>

<script>


    var app = {};

    
    $(function () {
        
        var jd = <%= GetCYFL() %>;

        var option = {
            grid:{                
                bottom: '30%',
                top: '16%',
            },
            title: {
                text: '产业分类',
                subtext: '',
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} 人 ({d}%)"
            },
            legend: {
                orient: 'vertical',
                left: 'left',
                data: jd.title_list
            },
            series: [
                {
                    name: '人数',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data: jd.data,
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



        var dom = document.getElementById("tempFlow1");
        var myChart = echarts.init(dom);

        myChart.setOption(option, true);
    });

    $(function () {

        var jd = <%= GetGroup() %>;

        var option = {
            title: {
                text: '拥有重要头衔的人数',
                subtext: '----',
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c}人 ({d}%)"
            },
            legend: {
                orient: 'vertical',
                left: 'left',
                data: jd.title_list
            },
            series: [
                {
                    name: '人数',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data: jd.data,
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



        var dom = document.getElementById("tempFlow2");
        var myChart = echarts.init(dom);

        myChart.setOption(option, true);
    });



</script>


<script>
    
    $(function () {

        var jd = <%= GetRzdy() %>;

        var option = {
            title: {
                text: '任职地域比例',
                subtext: '----',
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c}人 ({d}%)"
            },
            legend: {
                orient: 'vertical',
                left: 'left',
                data: jd.title_list
            },
            series: [
                {
                    name: '人数',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data: jd.data,
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



        var dom = document.getElementById("tempFlow4");
        var myChart = echarts.init(dom);

        myChart.setOption(option, true);
    });



</script>

<script>
    
    $(function () {

        var jd = <%= GetEmptyAndBl2() %>;
        option = {
            title: {
                text: '数据完整性分析',
                x: 'center'
            },
            tooltip: {
                trigger: 'axis',
                axisPointer: { // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
                }
            },
            legend: {
                right: 10,
                orient: 'vertical',
                left: 'left',
                data: ['已填写','未填写']
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '20%',
                top: '30%',
                containLabel: true,
                y2:100
            },
            xAxis: [{                
                type: 'category',
                data: jd.title_list,
                boundaryGap : true,
                interval:0,
                rotate:45,
                margin:20,
                axisLabel:{
                    rotate:45,
                    margin:2,
                    interval:0 
                }
            }],
            yAxis: [{
                type: 'value',
                name: '数量',
                axisLabel: {
                    formatter: '{value}'
                }
            }],
            dataZoom: [{
                type: 'inside',
                start: 0,
                end: 30
            }, {
                show: true,
                type: 'slider',
                y: '90%',
                start: 30,
                end: 100
            }],
            series: [{
                name: '已填写',
                type: 'bar',
                data: jd.data_1,
                
                itemStyle: {
                    normal: {
                        color:'#61a0a8',
                        label: {
                            show: true,
                            position: 'top',
                            formatter: '{c}'
                        },
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                },
            }, {
                name: '未填写',
                type: 'bar',
                data: jd.data_2,
                itemStyle: {
                    normal: {                        
                        color:'#c23531',
                        label: {
                            show: true,
                            position: 'top',
                            formatter: '{c}'
                        },
                        shadowBlur: 80,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
            }]
        };

        var dom = document.getElementById("tempFlow3");
        var myChart = echarts.init(dom);

        myChart.setOption(option, true);
    });



</script>