<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Jbgxjdb_2.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Report.Jbgxjdb_2" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<% if (false)
   { %>
<link href="/App_Themes/Blue/common.css" rel="stylesheet" type="text/css" />
<link href="/App_Themes/Vista/table.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>
<link href="/Core/Scripts/ui-lightness/jquery-ui-1.8.6.custom.css" rel="stylesheet"
    type="text/css" />
<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
<script src="/Core/Scripts/validate/jquery.validate-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>
<script src="/Core/Scripts/Mini/_CodeHelp.js" type="text/javascript"></script>
<% } %>

<style type="text/css">
    .page-head
    {
         font-size:26px;
         font-weight:bold;
    }
</style>



<script src="/Core/Scripts/echarts/echarts.min.js"></script>



<form action="" method="post">
<mi:Store runat="server" ID="store1" Model="" IdField="" PageSize="20" ReadOnly="true">

</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">
    <mi:Panel runat="server" ID="HeadPanel" Height="60" Scroll="None" PaddingTop="10" Align="Center">
        <mi:Label runat="server" ID="headLab" Value="单号" HideLabel="true" Mode="Transform" />
    </mi:Panel>


    <mi:Panel runat="server" Region="North" Height="600" >


    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
    <div id="main_chart_1" style="height:300px;width:33%;float:left;" class="mi-newline"></div>
        
    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
    <div id="main_chart_2" style="height:300px;width:33%;float:left;" class="mi-newline"></div>

    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
    <div id="main_chart_3" style="height:300px;width:33%;float:left;" class="mi-newline"></div>

    <div id="main_chart_4" style="height:300px;width:100%;" class="mi-newline"></div>

    </mi:Panel>

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None"  MinHeight="700"  >
        <mi:FormLayout runat="server" ID="searchForm" Dock="Top" Region="North" FlowDirection="TopDown"
            ItemWidth="300" ItemLabelAlign="Right" ItemClass="mi-box-item" Layout="HBox"
            StoreID="store1" FormMode="Filter" Scroll="None">

        </mi:FormLayout>
        <mi:Toolbar ID="Toolbar1" runat="server">
            

            <mi:ToolBarButton Text="新增" OnClick="ser:store1.Insert()" />
            <mi:ToolBarButton Text="保存" OnClick="ser:store1.SaveAll()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:store1.Delete()" />

            <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm.toggle()" />

            <mi:ToolBarButton Text="导出" Icon="/res/file_ico/excel.png" Command="ToExcel" />


            <%--
            <mi:ToolBarButton Text="列定义" Align="Right" Command="StepEdit2" /> 比工作表少了这个按钮
            <mi:ToolBarButton Text="列设置" Align="Right" Command="StepEdit3" />
            <mi:ToolBarButton Text="列高级设置" Align="Right" Command="StepEdit4" />
             --%>
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" >
            <Columns>
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>

    <% if (EC5.SystemBoard.EcContext.Current.User.Roles.Exist(EC5.IG2.Core.IG2Param.Role.BUILDER))
        { %>
<div id="SwitchPanel" style="width:640px;height:40px;border: 1px solid #C0C0C0;background-color: #FFFFFF;">
    <div style="margin:8px 24px 8px 8px; text-align:right;">
        <mi:Button runat="server" ID="Button4" OnClick="showFlowSetup()" Text="流程设置" />
        <mi:Button runat="server" ID="Button3" Command="ToolbarSetup" Text="工具栏设置" />
        <mi:Button runat="server" ID="Button1" Command="GoValidSetup" Text="验证设置" />
        <mi:Button runat="server" ID="StepEdit3Btn" Command="StepEdit3" Text="列定义" />
        <mi:Button runat="server" ID="StepEdit4Btn" Command="StepEdit4" Text="列高级设置" />
        <mi:Button runat="server" ID="Button2" Command="StepEdit5_DialogMode" Text="模式窗体设置" />

    </div>
</div>
<script type="text/javascript">

    $(document).ready(function () {

        var ps = Mini2.create('Mini2.ui.extend.PullSwitch', {
            panelId: 'SwitchPanel'
        });

        ps.render();
    });
</script>

    <% } %>

<%= GetDisplayRule() %>

</form>
<script type="text/javascript">


    function showDialog(owner) {


    }

    function TarSearch() {

        var viewport1 = window.widget1_I_viewport1;

        viewport1.toggleRegion('north');

    }

    function form_Closed(e) {
        var me = this,
            record = me.record,
            editor = me.editor;

        if (e.result != 'ok') { return; }

        var row = e.row;
        var map = e.map;


        for (var i = 0; i < map.length; i++) {
            var m = map[i];
            var v = row[m.src];

            record.set(m.to, v);

            if (editor.dataIndex == m.to) {
                editor.setValue(v);
            }
        }

    }

    function showDialgoForTable(owner) {

        var me = owner,
            tag = me.tag;
        var record = me.record;

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

        win.editor = me;
        win.record = record;
        win.show();

        win.formClosed(form_Closed);

    }







    //显示弹出提示框
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

            selectForm_Closed(e, owner);
        });

    }

    function selectForm_Closed(e, triggerBox, record) {

        var me = this;

        if (e.result != 'ok') { return; }

        var map = e.map;

        var row = e.row;

        var newValues = {};

        for (var i = 0; i < map.length; i++) {


            var m = map[i];
            var v = row[m.src];

            try{
                var conList = widget1_I_searchForm.findByDataField(m.to);

                if(conList){
                    for (var j = 0; j < conList.length; j++) {
                        conList[j].setValue( v);
                    }
                   
                }
                else{
                    triggerBox.setValue(v);
                }
            }
            catch(ex){
                console.error(ex);
            }
            //newValues[m.to] = v;
        }

        //record.set(newValues);

    }


    

    //显示窗体
    function form_EditShow(view, cell, recordIndex, cellIndex, e, record, row) {

        var menu_id = $.query.get('menu_id');

        var id = record.getId();
        var formType = '<%= this.FormEditType %>';
        var formEditPageId = <%= this.FormEditPageID %>;
        var altis = '<%= this.FromEditAliasTitle %>';

        
        var url;
        
        if(formType == 'ONE_FORM'){
            url = $.format('/App/InfoGrid2/View/OneForm/FormEditPreview.aspx?row_pk={0}&pageId={1}&menu_id={2}&alias_title={3}', 
                id, formEditPageId, menu_id, altis);
        }
        else if(formType == 'TABLE_FORM'){
            url = $.format('/App/InfoGrid2/View/OneForm/FormOneEditPreview.aspx?row_pk={0}&pageId={1}&menu_id={2}&alias_title={3}', 
               id, formEditPageId, menu_id, altis);
        }

        //var win = Mini2.create('Mini2.ui.Window',{
        //    url : url,
        //    width : 1000,
        //    height: 768,
        //    mode:true
        //});

        //win.show();

        EcView.show(url, altis + '-表单编辑');
    }


</script>


<script>


    function showFlowSetup(){

        var id = $.query.get('id');


        var urlStr = Mini2.urlAppend("/App/InfoGrid2/View/OneForm/FlowSetup.aspx",{
            'id': id
        });
        
        var win = Mini2.createTop('Mini2.ui.Window', {
            mode: true,
            text: '流程参数设置',
            iframe: true,
            width: 800,
            height: 600,
            url: urlStr
        });

        win.show();


    }

</script>


<script>



    
    $(document).ready(function(){
    

        setTimeout(function(){
        
        
            // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('main_chart_1'));


        option = {
            title : {
                text: '机台一个月内生产数',
                x:'center'
            },
            tooltip : {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            legend:
             <%= m_pie_legends_1 %>,
            series : [
                {
                    name: '机台名称',
                    type: 'pie',
                    radius : '55%',
                    center: ['50%', '60%'],
                    data:<%= m_pie_series_1 %>,
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

  
        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);


        // 处理点击事件并且跳转到相应的百度搜索页面
        myChart.on('click', function (params) {

            EcView.show("/App/InfoGrid2/View/OneView/ViewPreview.aspx?id="+1873);


        });



        setTimeout(function(){
        
        
              
        // 基于准备好的dom，初始化echarts实例
        var myChart_2 = echarts.init(document.getElementById('main_chart_2'));


        option_2 = {
            title : {
                text: '一个月内材料消耗表',
                x:'center'
            },
            tooltip : {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            legend:
             <%= m_pie_legends_2 %>,
            series : [
                {
                    name: '机台名称',
                    type: 'pie',
                    radius : '55%',
                    center: ['50%', '60%'],
                    data:<%= m_pie_series_2 %>,
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

  
        // 使用刚指定的配置项和数据显示图表。
        myChart_2.setOption(option_2);


        // 处理点击事件并且跳转到相应的百度搜索页面
        myChart_2.on('click', function (params) {
            EcView.show("/App/InfoGrid2/View/OneView/ViewPreview.aspx?id="+1875);


        });


        setTimeout(function(){
        
        
        
            
         // 基于准备好的dom，初始化echarts实例
        var myChart_3 = echarts.init(document.getElementById('main_chart_3'));


        option_3 = {
            title : {
                text: '10天内生产入库',
                x:'center'
            },
            tooltip : {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            legend:
             <%= m_pie_legends_3 %>,
            series : [
                {
                    name: '机台名称',
                    type: 'pie',
                    radius : '55%',
                    center: ['50%', '60%'],
                    data:<%= m_pie_series_3 %>,
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

  
        // 使用刚指定的配置项和数据显示图表。
        myChart_3.setOption(option_3);

        // 处理点击事件并且跳转到相应的百度搜索页面
        myChart_3.on('click', function (params) {
            EcView.show("/App/InfoGrid2/View/OneView/ViewPreview.aspx?id="+1874);


        });



        setTimeout(function(){
        

            var data_4 = <%= m_bar_data %>;




        
            // 基于准备好的dom，初始化echarts实例
            var myChart_4 = echarts.init(document.getElementById('main_chart_4'));


          var option_4 = {
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        type: 'cross',
                        crossStyle: {
                            color: '#999'
                        }
                    }
                },
                legend: {
                    data:['合格数','总生产数','合格率']
                },
                xAxis: [
                    {
                        type: 'category',
                        data: data_4.x_datas,
                        axisPointer: {
                            type: 'shadow'
                        }
                    }
                ],
                yAxis: [
                    {
                        type: 'value',
                        name: '总生产数',
                        min: 0,
                        axisLabel: {
                            formatter: '{value}'
                        }
                    },
                    {
                    type: 'value',
                    name: '合格率',
                    min: 0,
                    max: 100
                }
                ],
                series: [
                    {
                        name:'合格数',
                        type:'bar',
                        data:data_4.series_1s
                    },
                    {
                        name:'总生产数',
                        type:'bar',
                        data:data_4.series_2s
                    },
                    {
                        name:'合格率',
                        type:'line',
                        data:data_4.series_3s
                    }
                ]
            };


          myChart_4.setOption(option_4);


         //这是柱状图的点击按钮
          myChart_4.on("click",function(params){
          
              EcView.show("/App/InfoGrid2/View/OneBuilder/PreviewPage.aspx?pageId=1946");

          });




        },1 * 1000);



        },1 * 1000);
         


        },1 * 1000);



        },5 * 1000);


        

    });

        

</script>



