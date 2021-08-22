<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WelcomeJB2.ascx.cs" Inherits="App.InfoGrid2.View.WelcomeJB2" %>

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

                <div class="row"  >

                    <div class="col-sm-12 col-xs-12" >
                        <div class="widget-box" style="border-color:#ddd">
                                
                            <div class="widget-header widget-header-flat widget-header-small">
                                
                                <i class="ace-icon fa fa-signal"></i>
                                <h5 class="widget-title">待审核单据</h5>

                                <h5 class="widget-title" style="float:right;margin-right:20px;">
                                    <a href="#" onclick="Mini2.EcView.show('/app/InfoGrid2/View/OneFlow/UserFlowList.aspx?state=no_check', '流程列表');">更多...</a></h5>
                            </div>
                            <div   style="overflow:auto;">
                                     <iframe src="/App/InfoGrid2/View/OneView/ViewPreview.aspx?id=2045"  style="border:none;width:100%;height:500px;"  id="viewpreview2045" >

                                     </iframe>                               
                            </div>


                        </div>
                    </div>


                </div>


            <div class="row" style="margin-top:50px;">

                <div class="col-lg-6">

                    <div id="left_chart" style="height: 300px; width: 100%; float: left"></div>


                </div>
                <div class="col-lg-6">


                    <div id="right_chart" style="height: 300px; width: 100%; float: right"></div>

                </div>
            </div>



            </div>

        </div>

    </div>

</div>

<script src="/Core/Scripts/bootstrap/3.3.4/js/bootstrap.min.js"></script>

<script src="/Core/Scripts/jquery/jquery-3.1.0.min.js"></script>

<script>




</script>

<script>

    $(function () {

        var w_height = $(window).height() - 50;

        $("#viewpreview2045").css("height", w_height);

    });


    var m_LastActiveEls = null;

    $(function () {
        $('body').addClass('no-skin');

        var itemEls = $('#sidebar').find('.dropdown-toggle');

        console.log("---------------------------------------");

        console.log("itemEls.length",itemEls.length);

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