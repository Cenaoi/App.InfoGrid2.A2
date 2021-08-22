<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reimbur.aspx.cs" Inherits="App.InfoGrid2.WF.View.Reim.Reimbur" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />


    <title>报销界面</title>

        <script src="/WF/Script/common.js"></script>

</head>

<body style="max-width:640px; margin:auto; font-family: Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">月度报销</h1>
            </header>

            <nav class="bar bar-tab" style="background-color:#4cd964;">
              <a class="tab-item btn_success"  href="#">
                <span class="tab-label" style="color:white;" >提交</span>
              </a>
       
            </nav>

            <div class="content">

                <div class="list-block" style="margin:0px;">
                    <ul>
                      <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">报销月份</div>
                            <div class="item-input">
                              <input type="month"  v-model="reim_obj.COL_1" @change="save_reim_deta(reim_obj,'COL_1')" style="box-sizing: border-box;border: none;background: none; border-radius: 0 0 0 0;box-shadow: none;display: block;padding: 0 0 0.25rem;margin: 0;width: 100%;height: 2.15rem;color: #3d4145;font-size: 0.85rem;font-family: inherit;" >
                            </div>
                          </div>
                        </div>
                      </li>
                    </ul>
                </div>

                <div class="list-block" style="margin-top:0.5rem;margin-bottom:0px;">
                  <ul>
                    <li class="item-content item-link reim_deta">
                        <div class="item-inner">
                            <div class="item-title">报销明细</div>                        
                        </div>
                    </li>
                    <li class="item-content item-link trip_deta">
                        <div class="item-inner">
                            <div class="item-title">出差记录</div>
                        </div>
                    </li>
                    <li class="item-content item-link work_report">
                        <div class="item-inner">
                            <div class="item-title">工作报告</div>
                        </div>
                    </li>
                   </ul>
                </div>

                <!-- 这两个要先隐藏 -->
    <%--            <img-list :imgs="imgs"></img-list>

                <annex-list :annexs="annexs"></annex-list>--%>



                <div class="content-block">
                    <div class="row">
                        <div class="col-100"><a href="#" class="button button-fill button-big button-success  btn_show_history">查看历史状态</a></div>
                    </div>
                </div>

                 <template v-if="show_old_flow_insts">

                    <!-- 历史流程信息 -->
                    <div class="detail-comment-timeline" v-for="o in old_flow_insts" > 
                        <h3>{{o.inst_code}}</h3>
                        <ul class="comment-timeline-list" style="background-color:#eae9e9;padding-left:1.5rem;">
                            <li class="timelineunit" v-for="i in o.data" >
                                <div class="send" style="padding:0px;">
                                    <h4 class="line-time" v-show="i.step_sid === 2" style="float:right;margin: 4px;">{{ flow_data_format(i.date_start)}}</h4>
                                    <h4 class="line-time" v-show="i.step_sid === 4" style="float:right;margin: 4px;">{{ flow_data_format(i.date_end)}}</h4>
                                    <p class="line-content" style="    margin: 4px;"> {{i.op_check_desc}}  {{i.from_line_text}}</p>
                                    <p class="line-content" style="    margin: 4px;">[{{i.biz_sid_text}}] {{i.def_node_text}} {{i.op_check_comments}}</p>
                                    <div class="arrow"></div>
                                </div>
                                <div class="line-dot item-rainbow-2" style="top: 12px;"></div>
                            </li>
                        </ul>
                    </div>

                </template>

                <hr />

                <!-- 流程信息 -->
                <div class="detail-comment-timeline" style="background-color:#efeff4;" v-show="flow_insts">
                     <h3>当前</h3>
                    <ul class="comment-timeline-list "  style="background-color:#efeff4;padding-left:1.5rem;">
                        <li class="timelineunit" v-for="i in flow_insts" >
                            <div class="send" style="padding:0px;">
                                <h4 class="line-time" v-show="i.step_sid === 2" style="float:right;margin: 4px;">{{ flow_data_format(i.date_start)}}</h4>
                                <h4 class="line-time" v-show="i.step_sid === 4" style="float:right;margin: 4px;">{{ flow_data_format(i.date_end)}}</h4>
                                <p class="line-content" style="    margin: 4px;"> {{i.op_check_desc}}  {{i.from_line_text}}</p>
                                <p class="line-content" style="    margin: 4px;">[{{i.biz_sid_text}}] {{i.def_node_text}} {{i.op_check_comments}}</p>
                                <div class="arrow"></div>
                            </div>
                            <div class="line-dot item-rainbow-2" style="top: 12px;"></div>
                        <li>
                    </ul>
                </div>


                
            </div>

            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //页面加载完成事件
                    obj.onLoad = function () {

                    }

                    obj.my_vue = null;

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        console.log("onInit......");

                        me.initVue();

                        me.getData();

                        var row_id = xyf_util.getQuery(me.url, "id", true);

                        //报销明细点击事件
                        el.on("click", ".reim_deta", function (e) {

                            var url = Mini2.urlAppend("ReimburDeta.aspx", { id: row_id }, true);

                            $.router.load(url);

                        });


                        //出差记录点击事件
                        el.on("click",".trip_deta",function(e){
                        

                            var url = Mini2.urlAppend("TripDeta.aspx",{id:row_id},true);

                            $.router.load(url);
                        });


                        //工作报告点击事件
                        el.on("click",".work_report",function(e){
                            
                            var url = Mini2.urlAppend("WorkReport.aspx",{id:row_id},true);

                            $.router.load(url);
                            
                        });


                        //提交
                        el.on("click", '.btn_success', function (e) {


                            $.post("/WF/Handlers/ReimHandler.ashx", { action: 'SUBMIT_REIM', id: row_id}, function (result) {

                                console.log("result：", result);

                                if (!result.success) {

                                    $.toast(result.error_msg);

                                    return;

                                } else {

                                    var url = Mini2.urlAppend("/WF/View/Success.aspx", { tiem: 3, text: '提交成功' }, true);

                                    $.router.load(url);

                                }                               
                                
                            },"json");


                        });


                        //显示历史流程按钮点击事件
                        el.on("click",".btn_show_history",function(e){
                        
                            console.log("显示历史流程按钮点击了");

                            me.my_vue.show_old_flow_insts = !me.my_vue.show_old_flow_insts;

                        });

                        var time = null;


                    }


                    //刷新
                    obj.pageRefresh = function () {

                        var me = this;

                        me.getData();
                    }


                    //初始化Vue相关对象
                    obj.initVue = function () {

                        var me = this;

                        var el = me.el;

                        var row_id = xyf_util.getQuery(me.url, "id", true);

                        me.my_vue = new Vue({
                            el: el.children(".content")[0],
                            data: {
                                imgs: {
                                    data: [],
                                    server_url: '/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table=UT_346&tag_code=reim_img&item_id=' + row_id,
                                    delete_img_url: '/WF/Handlers/UploaderFileHandle.ashx',
                                    row_id: row_id,
                                    tag_code: 'reim_img',
                                    table_name: 'UT_346',
                                    btn_id: 'uploader_img',
                                    field_text: 'COL_49'

                                },
                                annexs: {

                                    data: [],
                                    server_url: '/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table=UT_346&tag_code=reim_annex&item_id=' + row_id,
                                    delete_annex_url: '/WF/Handlers/UploaderFileHandle.ashx',
                                    row_id: row_id,
                                    tag_code: 'reim_annex',
                                    table_name: 'UT_346',
                                    btn_id: 'uploader_annex',
                                    field_text: 'COL_50'
                                },
                                reim_obj: {},
                                cur_row: null,
                                change_fields: [],
                                flow_insts: [],
                                old_flow_insts: [],
                                show_old_flow_insts: false

                            },
                            methods: {

                                //只要变更一个就保存整个对象
                                save_reim_deta: function (r, field_text) {

                                    var my_vue = this;

                                    my_vue.cur_row = r;

                                    Mini2.Array.include(my_vue.change_fields, field_text);

                                    console.log("准备保存对象：", JSON.stringify(r));

                                    if (time) {
                                        time.resetStart();
                                    }
                                    else {
                                        time = Mini2.setTimer(function () {
                                            $.post("/WF/Handlers/ReimHandler.ashx", {
                                                action: 'SAVE_REIM_DETA',
                                                reim_deta_json_str: JSON.stringify(my_vue.cur_row),
                                                change_files_str: JSON.stringify(my_vue.change_fields),
                                                table_name: 'UT_346'
                                            }, function (result) {

                                                if (!result.success) {

                                                    console.error(result);

                                                    return;
                                                }

                                            }, "json");

                                        }, { interval: 500, limit: 1 });
                                    }


                                },
                                //流程时间的格式化
                                flow_data_format: function (item) {

                                    var my_vue = this;

                                    //return Mini2.Date.format(new Date(item), 'm月d日 (l) H:i');

                                    var week = my_vue.weekFormat(item);

                                    return moment(item).format('MM') + "月" + moment(item).format('DD') + "日" + week + moment(item).format('HH:mm');

                                },
                                //日期Week格式
                                weekFormat: function (item) {

                                    var week = moment(item).format('d');

                                    var week_text = "";

                                    if (week == 1) {
                                        week_text = "星期一";
                                    }
                                    if (week == 2) {
                                        week_text = "星期二";
                                    }
                                    if (week == 3) {
                                        week_text = "星期三";
                                    }
                                    if (week == 4) {
                                        week_text = "星期四";
                                    }
                                    if (week == 5) {
                                        week_text = "星期五";
                                    }
                                    if (week == 6) {
                                        week_text = "星期六";
                                    }
                                    if (week == 7) {
                                        week_text = "星期日";
                                    }

                                    return week_text;
                                }

                            }

                        });

                    }


                    ///初始化数据
                    obj.getData = function () {

                        var me = this;
                        var el = me.el;

                        var r_id = xyf_util.getQuery(me.url, "id", true);

                        $.post("/WF/Handlers/ReimHandler.ashx", { action: 'GET_REIMBUR_DATA', id: r_id }, function (result) {

                            console.log("result", result);

                            var data = result.data;

                            me.my_vue.imgs.data = data.img_data;
                            me.my_vue.annexs.data = data.annex_data;
                            me.my_vue.reim_obj = data.reim_obj;
                            me.my_vue.flow_insts = data.flow_insts;
                            me.my_vue.old_flow_insts = data.old_flow_insts;

                        }, "json");

                    }


                    return obj;

                }

            </script>


        </div>

    </div>


    
</body>
</html>


