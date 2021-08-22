<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Jbgxjdb_1.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Report.Jbgxjdb_1" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<style type="text/css">
    .page-head
    {
         font-size:26px;
         font-weight:bold;
    }
</style>
<script src="/Core/Scripts/echarts/echarts.min.js"></script>


<form action="" id="form1" method="post">
<mi:Store runat="server" ID="store1" Model="IG2_MAP" IdField="IG2_MAP_ID" PageSize="20" ReadOnly="true" AutoFocus="false" >

</mi:Store>
<mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="10">
    <mi:Label runat="server" ID="headLab" Value="单号" HideLabel="true" Mode="Transform" BodyAlign="Center" />
</mi:Panel>


<mi:Viewport runat="server" ID="viewport1" MarginTop="40" Scroll="Auto">


    <mi:Panel runat="server" Region="North" Height="300" MinHeight="300" >


    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
    <div id="main_chart" style="height:300px;width:100%;"></div>
        


    </mi:Panel>

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" MinHeight="600" >
        <mi:SearchFormLayout runat="server"  ID="searchForm" StoreID="store1" Region="North" >

        </mi:SearchFormLayout>
        <mi:Toolbar runat="server" ID="toolbar1">
            <mi:ToolBarTitle Text="交叉报表" />
            <mi:ToolBarButton Text="导出" Command="GoToExcel" Icon="" />
            <mi:ToolBarButton Text="查询" OnClick="widget1_I_searchForm.toggle()" Icon="/res/icon_sys/Search.png"  />
            <mi:ToolBarButton ID="ShowChartBtn" Text="图形报表" Command="ShowChart" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" PagerVisible="false" Region="Center" MinHeight="400" >
            <Columns>
                <mi:RowNumberer />
            </Columns>
        </mi:Table>
    </mi:Panel>
</mi:Viewport>
   <mi:Hidden runat="server" ID="bufferTableHid" Value="" />
</form>

<script>


    function showSelectWinForTable(owner) {

        var me = owner,
            tag = me.tag;

        //var formPanel = me.ownerParent;
        //var store = formPanel.store;

        //var record = store.getCurrent();


        if (!tag || tag == '') {
            return;
        }

        var ps = eval('(' + tag + ')');

        var urlStr = $.format("/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={0}&viewId={1}",
            ps.type_id, ps.view_id);

        var win = Mini2.createTop('Mini2.ui.Window', {
            mode: true,
            text: '选择',
            iframe: true,
            width: 800,
            height: 600,
            startPosition: 'center_screen',
            url: urlStr
        });

        //win.editor = me;
        //win.record = record;
        win.show();

        win.formClosed(function (e) {

            console.debug("e ", e);

            form_Closed(e, owner);
        });

    }

    function form_Closed(e, triggerBox, record) {

        var me = this;

        if (e.result != 'ok') { return; }

        var map = e.map;

        var row = e.row;

        var newValues = {};

        for (var i = 0; i < map.length; i++) {


            var m = map[i];
            var v = row[m.src];



            try {
                var conList = widget1_I_searchForm.findByDataField(m.to);

                if (conList) {
                    for (var j = 0; j < conList.length; j++) {
                        conList[j].setValue(v);
                    }

                }
                else {
                    triggerBox.setValue(v);
                }
            }
            catch (ex) {
                console.error(ex);
            }
        }

        //record.set(newValues);

    }






</script>

<script>


    function setChart(date_list,title_list,series_list){

        console.log("date_list:", date_list);

        console.log("title_list:", title_list);

        console.log("series_list:", series_list);


        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('main_chart'));

        // 指定图表的配置项和数据
        var option = {
            title: {
                text: '工序上报统计表'
            },
            tooltip: {},
            legend: {
                data: title_list
            },
            xAxis: [
                    {
                        type: 'category',
                        data: date_list
                    }
            ],
            yAxis: [
                    {
                        type: 'value'
                    }
            ],
            series: series_list
        };

        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);

    }




</script>




