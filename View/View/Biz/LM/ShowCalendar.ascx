<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShowCalendar.ascx.cs" Inherits="App.InfoGrid2.View.Biz.LM.ShowCalendar" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<style type="text/css">

    .us-box-selected{
        display:none;
    }

    .mi-dataview-item-selected .us-box-selected{
        border:1px solid #f39c12;
        position:absolute;
        width:100%;
        height:100%;
        top:0px;
        display:block;
    }

    
    .mi-dataview-item-selected {
        background:none;
    }

    .mi-dataview-triton-big .mi-dataview-item-selected{
        background: #FFF;
    }

    .us_button {
        position: relative;
        background: #fff;
        box-shadow: none;
        font-family: 'Montserrat', sans-serif;
        border: 1px solid #D3D9DC;
        font-size: 13px;
        padding: 5px 20px;
        height: auto;
        margin:0px;
    }


    .us_c_title{
        clear: both;
    }
    
</style>
<script src="/Core/Scripts/XYF/xyfUtil.js"></script>
<!--MESystem_Front-->
<form action="" method="post" >

<mi:Store runat="server" ID="store1" Model="UT_440" IdField="ROW_IDENTITY_ID" PageSize="20">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="ROW_IDENTITY_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>
</mi:Store>


<mi:Viewport runat="server" ID="viewport1" Main="true">

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="North" Height="400" >
        
             <div style="width:100%;margin-left:0.2rem;" class="us_calendar">

                 <div style="width:100%;padding:0.5rem 0;" >
                    <div style="float:left;width:20%;padding-top:10px;">

                     <button class="us_button" type="button" @click="changeMonth('back')">
                         <i class="fa fa-chevron-left"></i>
                         上个月</button>
                     <button class="us_button" type="button" @click="changeMonth('next')">
                         下个月
                         <i class="fa fa-chevron-right"></i>
                     </button>

                    </div>
                     <div style="float:left;width:60%; text-align:center;">
                           <p style="font-size:1.2rem !important;font-weight:bold;">{{year}}年{{month}}月</p> 
                     </div>
                  
                 </div>

                 <div class="us_c_title">

                     <div  style="width:14.2%;float:left;border:1px solid #ddd;font-weight:bold;text-align:center;padding:0.5rem 0;" v-for="T in weeks">
                         {{T}}
                     </div>
                
                 </div>

                 <div class="us_c_body" style="border-left:1px solid #ddd;height:500px;">

                     <div style="width:14.2%;height:6.5rem;float:left;border-right:1px solid #ddd;border-bottom:1px solid #ddd;" v-for="T in days" @click="dayClick(T)">
                        <div style="text-align:right;margin-top:0.3rem;margin-right:0.1rem;font-weight:bold;font-size:0.9rem;" v-if="!T.next_m">{{T.day}}</div> 
                        <div style="text-align:right;margin-top:0.3rem;margin-right:0.1rem;font-weight:bold;font-size:0.9rem;opacity: 0.3;" v-else>{{T.day}}</div> 
                         <div  style="background-color:#9A80B9;text-align:center; margin:0.3rem;" v-for="(ST,SI) in T.texts" v-show="SI === 0"> 
                            <span style="font-size:0.8rem;color:white;white-space:nowrap;overflow:hidden;margin-left:0.3rem;" >{{ST}}</span> 
                         </div>
                         <div v-show="T.num >= 1" style="color:#399bff;font-size:0.8rem;margin-left:0.3rem;">({{T.num}})个任务</div>

                     </div>

                 </div>


             </div>


    </mi:Panel>

    <mi:Panel runat="server" ID="Panel1" Dock="Full" Region="Center" Scroll="None" >

         <mi:Toolbar ID="Toolbar1" runat="server">

            <mi:ToolBarTitle ID="tableNameTB1" Text="排程任务明细" />

            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:NumColumn HeaderText="业务状态" DataField="BIZ_SID" />
                <mi:DateColumn HeaderText="排程日期" DataField="T_DATE" />
                <mi:BoundField HeaderText="任务名称" DataField="T_TEXT" />
                <mi:BoundField HeaderText="工作组名称" DataField="D_TEXT" />
            </Columns>
        </mi:Table>

    </mi:Panel>

    <mi:Hidden runat="server"  ID="hidDay"></mi:Hidden>

</mi:Viewport>

</form>


<script>


    Mini2.ready(function () {


        var vm = new Vue({
            el: ".us_calendar",
            data: {
                weeks: ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
                days: [],
                e_text: '',
                //年份
                year: moment().years(),
                //月份
                month:moment().months()+1

            },
            methods: {
                /**
                 * 改变月份事件
                 * @param {String} op next -- 下个月  back -- 上个月
                 */
                changeMonth: function (op) {

                    var vm = this;

                    var now = moment([vm.year, vm.month-1,1]);

                    if (op === 'next') {
                        now = now.add("M", 1);
                    } else if (op === 'back') {
                        now = now.subtract("M", 1);
                    }

                    vm.year = now.years();
                    vm.month = now.months() +1;

                    //当前页开始那一天
                    var beg_date = now.startOf('month');
              
                    //整个日历表数据
                    var days = [];

                    for (var i = 0; i < beg_date._d.getDay(); i++) {

                        days.push({ texts: [] });

                    }

                    //当前月结束那一天
                    var end_date = now.endOf('month');

                    console.log(end_date,"结束天");

                    for (var j = 0; j < end_date._d.getDate(); j++) {

                        var time = new Date(vm.year, vm.month -1, j + 1);
                        //周末
                        var holiday = false;

                        if (time.getDay() === 0 || time.getDay() === 6) {
                            holiday = true;
                        }

                        days.push({
                            day: j + 1,
                            next_m: holiday,
                            num:0,
                            texts: []
                        });
                    }

                    vm.days = days;

                },
                /**
                 * 根据月份来获取数据
                 * */
                getDataByMonth: function () {


                    var vm = this;

                    var urlStr = xyf_util.appUrl("/InfoGrid2/View/Biz/LM/ShowCalendar", "GET_DATA_BY_MONTH");

                    Mini2.post(urlStr, { year: vm.year,month:vm.month}, function (data) {

                        data.forEach(function (v) {

                            var time = new Date(v.T_DATE);

                            var day = time.getDate();

                            vm.days.forEach(function (s_v) {

                                if (s_v.day === day) {
                                    s_v.num++;
                                }
                            });

                        });


                    });

                },
                /**
                 * 日期点击事件
                 * @param {Object} T 日期对象
                 * */
                dayClick: function (T) {

                    var vm = this;

                    if (!T.day) {
                        return;
                    }

                    var hidEl = Mini2.find("hidDay");

                    hidEl.setValue(vm.year + "-" + vm.month + "-" + T.day);

                    console.log(hidEl);


                    widget1.subMethod($('form:first'), {subName: 'store1', subMethod: 'Refresh'});


                }
            },
            //初始化完成事件
            mounted: function () {

                var vm = this;

                vm.getDataByMonth();

                vm.changeMonth();

            }

        });

    });


</script>




