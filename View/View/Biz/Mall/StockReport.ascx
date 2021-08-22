<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StockReport.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Mall.StockReport" %>



<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<script src="/Core/Scripts/echarts/echarts.min.js"></script>

<link href="/Core/Scripts/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" />



<form action="" method="post">

<mi:Store runat="server" ID="store1" Model="IG2_MAP" ReadOnly="true" PageSize="100" SortText="ROW_DATE_CREATE asc">

</mi:Store>

<mi:Store runat="server" ID="store2" Model="IG2_MAP" ReadOnly="true" PageSize="100" SortText="ROW_DATE_CREATE asc" >

</mi:Store>

<mi:Store runat="server" ID="store3" Model="IG2_MAP" ReadOnly="true" PageSize="100" SortText="ROW_DATE_CREATE asc" >

</mi:Store>

    
<mi:Store runat="server" ID="store4" Model="IG2_MAP" ReadOnly="true" PageSize="100"  SortText="ROW_DATE_CREATE asc">

</mi:Store>


    <div class="main-content">

    <div class="main-content-inner">

        <div class="page-content">


            <div class="row">

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



    <mi:Panel runat="server" ID="centerPanel" Dock="None"  Region="North" Scroll="None" Height="250" >


        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarTitle ID="tableNameTB1" Text="昨日产品进出统计表" />
            <mi:ToolBarButton Text="查询更多" OnClick="" Command="showReport1"/>
        </mi:Toolbar>


        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" PagerVisible="false">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />

            </Columns>
        </mi:Table>
    </mi:Panel>

    

    <mi:Panel runat="server" ID="Panel1" Dock="None"  Region="North" Scroll="None" Height="250" >

        <mi:Toolbar ID="Toolbar2" runat="server">
            <mi:ToolBarTitle ID="tableNameTB2" Text="昨日产品小类统计表" />
            <mi:ToolBarButton Text="查询更多" OnClick="" Command="showReport2"/>
        </mi:Toolbar>
        <mi:Table runat="server" ID="table2" StoreID="store2" Dock="Full" ReadOnly="true" PagerVisible="false" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
            </Columns>
        </mi:Table>
    </mi:Panel>

    <mi:Panel runat="server" ID="Panel2" Dock="None"  Region="North" Scroll="None" Height="250" >

        <mi:Toolbar ID="Toolbar3" runat="server">
            <mi:ToolBarTitle ID="tableNameTB3" Text="昨日产品品牌统计表" />
            <mi:ToolBarButton Text="查询更多" OnClick="" Command="showReport3"/>
        </mi:Toolbar>
        <mi:Table runat="server" ID="table3" StoreID="store3" Dock="Full" ReadOnly="true" PagerVisible="false" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
            </Columns>
        </mi:Table>
    </mi:Panel>

    <mi:Panel runat="server" ID="Panel3" Dock="None"  Region="North" Scroll="None" Height="250" >

        <mi:Toolbar ID="Toolbar4" runat="server">
            <mi:ToolBarTitle ID="tableNameTB4" Text="昨日产品大类统计表" />
            <mi:ToolBarButton Text="查询更多" OnClick="" Command="showReport4"/>
        </mi:Toolbar>
        <mi:Table runat="server" ID="table4" StoreID="store4" Dock="Full" ReadOnly="true" PagerVisible="false" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
            </Columns>
        </mi:Table>
    </mi:Panel>


    <iframe src="/App/InfoGrid2/View/OneBuilder/PreviewPage.aspx?pageId=2046&alias_title=库存情况表&menu_id=829" style="width:100%;height:900px;"></iframe>
   

    <mi:Hidden ID="hTableName1" runat="server"  />
    <mi:Hidden ID="hTableName2" runat="server" />
    <mi:Hidden ID="hTableName3" runat="server" />
    <mi:Hidden ID="hTableName4" runat="server" />

</form>


<script>


    $(function () {


        Mini2.LoaderManager.resize({
            resize: function () {

                var w = $(window).width();

                Mini2.find('centerPanel').setWidth(w);

                Mini2.find('Panel1').setWidth(w);
                Mini2.find('Panel2').setWidth(w);
                Mini2.find('Panel3').setWidth(w);

            }
        });


        console.debug("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
    });

    


</script>


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

                data: [<%=smlisttitle%>]

            },
            label: {
                normal: {
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

            EcView.show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id=" + 1912);

        });

    });




</script>


<script>

    $(function () {

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
                data: [<%=yingsk%>]

        },
        label: {
            normal: {
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

            EcView.show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id=" + 1913);

        });



    });

    



</script>

