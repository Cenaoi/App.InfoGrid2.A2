<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowReimbur.aspx.cs" Inherits="App.InfoGrid2.WF.View.Reim.ShowReimbur" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />

    <title>流程中报销主界面</title>

    <script src="/WF/Script/common.js"></script>


    <style>


          .detail-comment-timeline {
            padding-top:20px;
        }

            .detail-comment-timeline .comment-timeline-list {
                list-style: none;
                padding-top: 45px;
                padding-left: 80px;
                padding-right: 20px;
                padding-bottom: 30px;
                margin-top: 10px;
                background-color: #fff;
            }

            .detail-comment-timeline .comment-timeline-list .timelineunit {
                border-left: 1px solid #e0e0e0;
                padding-left: 20px;
                padding-bottom: 20px;
                position: relative;
                margin: 0px;
                display: list-item;
            }


            .detail-comment-timeline .comment-timeline-list .timelinehand {
                padding-bottom: 45px;
            }


            .detail-comment-timeline .comment-timeline-list .timelineunit .line-time {
                font-size: 12px;
                color: #b0b0b0;
                line-height: 20px;
                font-weight: normal;
            }

            .detail-comment-timeline .comment-timeline-list .timelineunit .line-content {
                margin-top: 10px;
                font-size: 14px;
                line-height: 22px;
                overflow: hidden;
            }

            .detail-comment-timeline .comment-timeline-list .timelineunit .line-foot {
                margin-top: 20px;
                padding-bottom: 15px;
                border-bottom: 1px solid #e0e0e0;
            }

            .detail-comment-timeline .comment-timeline-list .timelineunit .line-dot {
                border-radius: 50%;
                position: absolute;
                top: 0px;
                left: -6px;
                width: 11px;
                height: 11px;
            }

            .detail-comment-timeline .comment-timeline-list .timelineunit .item-rainbow-1 {
                background: #2196f3;
            }

            .detail-comment-timeline .comment-timeline-list .timelineunit .item-rainbow-2 {
                background: #83c44e;
            }

            .detail-comment-timeline .comment-timeline-list .timelineunit .item-rainbow-3 {
                background: #e53935;
            }

            .detail-comment-timeline .comment-timeline-list .timelineunit .item-rainbow-4 {
                background: #00c0a5;
            }

            .detail-comment-timeline .comment-timeline-list .timelineunit .item-rainbow-5 {
                background: #ffac13;
            }

            .detail-comment-timeline .comment-timeline-list .timelinehand .line-time {
                position: absolute;
                font-size: 18px;
                color: #808080;
                line-height: 20px;
                font-weight: normal;
                left: -60px;
            }

            .detail-comment-timeline .comment-timeline-list .timelinehand .line-dot {
                border-radius: 50%;
                position: absolute;
                top: -2px;
                left: -10px;
                width: 20px;
                height: 20px;
                background: #FFFFFF;
                border: 2px solid #e0e0e0;
            }


    </style>


</head>
<body style="max-width:640px; margin:auto; font-family: Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">月度报销审核</h1>
                <button class="button button-link button-nav pull-right">
                    下一步
                </button>
            </header>

            <nav class="bar bar-tab btn_nav" style="display:none;">
              <a class="tab-item btn_agree"  href="#" style="background-color:#4cd964;">
                <span class="tab-label" style="color:white;" >同意</span>
              </a>
              <a class="tab-item btn_back"  href="#" style="background-color:#f6383a;">
                <span class="tab-label " style="color:white;" >拒绝</span>
              </a>
              <a class="tab-item btn_back_start" href="#" style="background-color:bisque;display:none;">
                  <span class="tab-label " style="color:white;" >退单</span>
              </a>
            </nav>

            <nav class="bar bar-tab btn_nav_end" style="display:none;">
              <a class="tab-item"  href="#" style="background-color:#b1aeae;">
                <span class="tab-label" style="color:white;" >已提交</span>
              </a>
            </nav>

            <div class="content">

                <div style="margin:0px;" class="list-block media-list">
                    <ul><!----> 
                        <li>
                            <div style="padding-left:0.5rem;" class="item-content ">
                                <div class="item-media">
                                    <img src="/mobile/res/image/demo/person_center_face.png" style="width: 2.5rem;">
                                </div> 
                                <div class="item-inner">
                                    <div class="item-title-row">
                                        <div class="item-title">{{reim_obj.col_5}}</div> 
                                        <div class="item-after"></div>
                                    </div> 
                                    <div class="item-subtitle" style="color: rgba(76, 175, 80, 0.79);">{{reim_obj.col_20}}</div>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>

                <div style="margin:10px 15px;">

                    <span>审批编号：</span> <span>{{reim_obj.biz_flow_inst_code}}</span>
                    <br />
                    <span>所在部门：</span><span>{{reim_obj.col_6}}.{{reim_obj.col_9}}.{{reim_obj.col_10}}</span>
                    <br />
                    <span>单据编号：</span><span>{{reim_obj.col_37}}</span>

                </div>

                <div class="list-block" style="margin:0px;">
                    <ul>
                        <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label" style="font-size:1rem;">报销月份</div>
                                <div class="item-input">
                                  <input type="text" style="font-size:1rem;" readonly="readonly" :value="reim_obj.col_1" >
                                </div>
                              </div>
                            </div>
                        </li>
                    </ul>
                </div>


                <div class="row" style="background-color:white;">

                    <div class="col-50">

                        <div class="menu_box mu-icon-box"  style="background-color:white;padding:0px;margin:5px;padding-top:20px;border-radius:10px;" >
                            <div class="text-center mu-icon"  style=" border-radius: 8px;padding-top:0px;">
                                      <img src="/WF/res/home_009.png" class="reim_deta_by_fee_code" data-fee-code="10103"  style="width:100%;" />
                            </div>
                            <div class="text-center mi-icon-label">
                                电话费：{{reim_obj.col_12}}
                            </div>
                        </div>

                    </div>

                    <div class="col-50">
                        <div class="menu_box mu-icon-box"   style="background-color:white;padding:0px;margin:5px;padding-top:20px;border-radius:10px;">
                            <div class="text-center mu-icon" style=" border-radius: 8px;padding-top:0px;">
                                  <img src="/WF/res/home_010.png" class="reim_deta_by_fee_code" data-fee-code="10106"  style="width:100%;" />
                            </div>
                            <div class="text-center mi-icon-label">
                                差旅费：{{reim_obj.col_13}}
                            </div>
                        </div>
                    </div>

                    <div class="col-50">
                        <div class="menu_box mu-icon-box"  style="background-color:white;padding:0px;margin:5px;padding-top:20px;border-radius:10px;" >
                            <div class="text-center mu-icon "  style=" border-radius: 8px;padding-top:0px;">
                            <img src="/WF/res/home_011.png" class="reim_deta_by_fee_code" data-fee-code="10104" style="width:100%;" />
                            </div>
                            <div class="text-center mi-icon-label">
                                汽车费：{{reim_obj.col_14}}
                            </div>
                        </div>
                    </div>

                    <div class="col-50">
                        <div class="menu_box mu-icon-box " style="background-color:white;padding:0px;margin:5px;padding-top:20px;border-radius:10px;">
                            <div class="text-center mu-icon "  style=" border-radius: 8px;padding-top:0px;">
                               <img src="/WF/res/home_012.png" class="reim_deta_by_fee_code" style="width:100%;" data-fee-code="10105"  />
                            </div>
                            <div class="text-center mi-icon-label">
                                招待费：{{reim_obj.col_15}}
                            </div>
                        </div>
                    </div>

                    <div class="col-50">
                        <div class="menu_box mu-icon-box " style="background-color:white;padding:0px;margin:5px;padding-top:20px;border-radius:10px;">
                            <div class="text-center mu-icon "  style=" border-radius: 8px;padding-top:0px;">
                               <img src="/WF/res/home_015.png" class="reim_deta_by_fee_code" style="width:100%;" data-fee-code="10107"  />
                            </div>
                            <div class="text-center mi-icon-label">
                                推广费：{{reim_obj.col_52}}
                            </div>
                        </div>
                    </div>

                    <div class="col-50">
                        <div class="menu_box mu-icon-box " style="background-color:white;padding:0px;margin:5px;padding-top:20px;border-radius:10px;">
                            <div class="text-center mu-icon "  style=" border-radius: 8px;padding-top:0px;">
                               <img src="/WF/res/home_014.png" class="reim_deta_by_fee_code" style="width:100%;" data-fee-code="10199"  />
                            </div>
                            <div class="text-center mi-icon-label">
                                其它费用：{{reim_obj.col_53}}
                            </div>
                        </div>
                    </div>



                </div>

                <div class="list-block" style="margin:0px;">
                    <ul>
                      <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">虚拟用户代码</div>
                            <div class="item-input">
                              <input type="text" v-model="reim_obj.input_text">
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
                        <div class="item-title">报销明细（总金额：{{reim_obj.col_23}}）</div>
                        
                    </div>
                    </li>
                    <li class="item-content item-link trip_deta">
                        <div class="item-inner">
                            <div class="item-title">出差记录</div>
                        </div>
                    </li>
                    <li class="item-content item-link work_report">
                        <div class="item-inner">
                            <div class="item-title">工作报表</div>
                        </div>
                    </li>
                   </ul>
                </div>

<%--                <img-list :imgs="imgs"></img-list>

                <annex-list :annexs="annexs"></annex-list>--%>


                
                <div class="content-block">
                    <div class="row">
                        <div class="col-100"><a href="#" class="button button-fill button-big button-success  btn_show_history">查看历史状态</a></div>
                    </div>
                </div>

                <template v-if="show_old_flow_insts">

                    <!-- 历史流程信息 -->
                    <div class="detail-comment-timeline" v-for="o in old_flow_insts"  > 
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
                <div class="detail-comment-timeline" style="background-color:#efeff4;">
                    <h3 style="padding-left:1rem;">当前</h3>
                    <ul class="comment-timeline-list "  style="background-color:#efeff4;padding-left:1.5rem;padding-top: 0px;">
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


                <div class="content-block" style="display:none;">
                    <div class="row">
                        <div class="col-50 "><a href="#" class="button button-big button-fill button-success  btn_agree">模拟同意</a></div>
                        <div class="col-50 "><a href="#" class="button button-big button-fill button-danger  btn_back">模拟拒绝</a></div>
                    </div>
                </div>
                
               


            </div>

            <script type="text/javascript" data-main="true">
        
                function main() {
                       
                    var obj = {};

                    obj.pageRefresh = function () {

                        var me = this;

                        me.getData();
                    }

                    obj.btn_enabled = true;

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        me.initVue();

                        me.getData();

                        var row_id = xyf_util.getQuery(me.url,"reim_id",true);

                        //报销明细点击事件
                        el.on("click", ".reim_deta", function (e) {

                            var url = Mini2.urlAppend("ShowReimDeta.aspx", { id: row_id }, true);

                            $.router.load(url);

                        });

                        //出差记录点击事件
                        el.on("click",".trip_deta",function(e){
                        

                            var url = Mini2.urlAppend("ShowTripDeta.aspx",{id:row_id},true);

                            $.router.load(url);
                        });
                        //工作报告点击事件
                        el.on("click",".work_report",function(e){
                            
                            var url = Mini2.urlAppend("ShowWorkReport.aspx",{id:row_id},true);

                            $.router.load(url);
                            
                        });

                        //同意按钮点击事件
                        el.on("click",".btn_agree",function(e){

                            var my_vue = me.my_vue;
                            
                            var frm = Mini2.create('Mini2.ui.PromptWindow', {

                                contentHtml: '<div class="modal-text">审批意见</div><textarea  style="height:8rem;" class="modal-text-input">同意</textarea>',

                                title: '',

                                callback_ok: function(){

                                    $.post("/WF/Handlers/FlowHandler.ashx",{action:'SETP_AGREE',id:row_id,input_text:my_vue.reim_obj.input_text,comment:this.getValue()},function(result){
                                        if(!result.success){
                                            $.toast(result.error_msg);
                                            return;
                                        }


                                        //提交成功了就隐藏按钮
                                        el.find(".btn_nav_end").show();
                                        el.find(".btn_nav").hide();

                                        //var url = Mini2.urlAppend("/WF/View/Success.aspx",{tiem:3,text:result.msg,url:encodeURIComponent(me.url)},true);

                                        //$.router.load(url);

                                        $.alert("提交成功");

                                        $.router.back();

                            
                                    },"json");

                                
                                },

                            });


                            frm.render();     
                           

                        });

                        //拒绝按钮点击事件
                        el.on("click",".btn_back",function(e){

                            var my_vue = me.my_vue;

                            var frm = Mini2.create('Mini2.ui.PromptWindow', {

                                contentHtml: '<div class="modal-text">审批意见</div><textarea  style="height:8rem;" class="modal-text-input">拒绝</textarea>',

                                title: '',

                                callback_ok: function(){

                                    $.post("/WF/Handlers/FlowHandler.ashx",{action:'SETP_BACK_STRAT',id:row_id,input_text:my_vue.reim_obj.input_text,comment:this.getValue()},function(result){
                                        if(!result.success){
                                            $.toast(result.error_msg);
                                            return;
                                        }

                                        console.log("拒绝按钮点击事件resutl:", result);

                                        //var url = Mini2.urlAppend("/WF/View/Success.aspx",{tiem:3,text:result.msg,url:encodeURIComponent(me.url)},true);

                                        //$.router.load(url);

                                        $.alert("提交成功");

                                        $.router.back();


                                    },"json");


                                },

                            });


                            frm.render();

                           

                        });

                        //退单按钮点击事件
                        el.on("click",".btn_back_start",function(e){
                        
                            var my_vue = me.my_vue;

                            var frm = Mini2.create('Mini2.ui.PromptWindow', {

                                contentHtml: '<div class="modal-text">审批意见</div><textarea  style="height:8rem;" class="modal-text-input">退回首节点</textarea>',

                                title: '',

                                callback_ok: function(){

                                    $.post("/WF/Handlers/FlowHandler.ashx",{action:'SETP_BACK_STRAT',id:row_id,input_text:my_vue.reim_obj.input_text,comment:this.getValue()},function(result){
                                        if(!result.success){
                                            $.toast(result.error_msg);
                                            return;
                                        }

                                        //var url = Mini2.urlAppend("/WF/View/Success.aspx",{tiem:3,text:result.msg,url:encodeURIComponent(me.url)},true);

                                        //$.router.load(url);

                                        $.alert("提交成功");

                                        $.router.back();

                                    },"json");

                                },

                            });


                            frm.render();

                        
                        });


                        //显示历史流程按钮点击事件
                        el.on("click",".btn_show_history",function(e){
                        
                            console.log("显示历史流程按钮点击了");

                            me.my_vue.show_old_flow_insts = !me.my_vue.show_old_flow_insts;

                        });


                        //交通费  用车费 电话费 招待费 的点击事件
                        el.on("click",".reim_deta_by_fee_code",function(e){
                        

                            var target = e.target;
                            var fee_code = $(target).attr("data-fee-code");

                            console.log(target);

                            var url = Mini2.urlAppend("ShowReimDeta.aspx",{id:row_id,fee_code:fee_code},true);
                            $.router.load(url);

                        });

                        //按钮是否隐藏
                        if(me.btn_enabled){

                            el.find(".btn_nav").show();


                        }else{

                            el.find(".btn_nav_end").show();

                        }


                        //下一步按钮点击事件
                        el.on("click",".pull-right",function(){
                            
                            var flow_id = xyf_util.getQuery(me.url,"flow_id",true);
                            var biz_sid = xyf_util.getQuery(me.url,"biz_sid",true);

                            xyf_util.post("/App/InfoGrid2/WF/Handlers/ReportHandler.ashx","NEXT_REPORT",{flow_id:flow_id,biz_sid:biz_sid},function(data){
                                
                                var url = Mini2.urlAppend(data.url,{flow_id:data.flow_id,biz_sid:data.biz_sid},true);

                                $.router.load(url);

                            });
                        });


               
                    }

                    obj.my_vue = null;

                    //初始化vue相关对象
                    obj.initVue = function () {

                        var me = this;

                        var el = me.el;

                        var row_id = xyf_util.getQuery(me.url, "reim_id", true);

                        me.my_vue = new Vue({
                            el: el.children(".content")[0],
                            data: {
                                imgs: {
                                    data: null,
                                    server_url: '/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table=UT_346&tag_code=reim_img&item_id=' + row_id,
                                    delete_img_url: '/WF/Handlers/UploaderFileHandle.ashx',
                                    row_id: row_id,
                                    tag_code: 'reim_img',
                                    table_name: 'UT_346',
                                    btn_id: 'uploader_img',
                                    field_text: 'COL_49',
                                    read_only: true

                                },
                                annexs: {

                                    data: null,
                                    server_url: '/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table=UT_346&tag_code=reim_annex&item_id=' + row_id,
                                    delete_annex_url: '/WF/Handlers/UploaderFileHandle.ashx',
                                    row_id: row_id,
                                    tag_code: 'reim_annex',
                                    table_name: 'UT_346',
                                    btn_id: 'uploader_annex',
                                    field_text: 'COL_50',
                                    read_only: true

                                },
                                flow_insts: null,
                                reim_obj: null,
                                old_flow_insts: null,
                                show_old_flow_insts: false
                            },
                            methods: {
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
                            },
                            mounted: function () {

                                me.getData();
                            }
                        });

                    }

                    ///初始化数据
                    obj.getData = function () {

                        var me = this;

                        console.log("me.my_vue", me.my_vue);

                        var r_id = xyf_util.getQuery(me.url, "reim_id", true);

                        $.post("/WF/Handlers/ShowReimburHandle.ashx", { action: 'INIT_DATA_SHOW_REIMBUR', r_id: r_id }, function (result) {

                            var data = result.data;

                            console.log("data", result);

                            me.my_vue.imgs.data = data.img_data;
                            me.my_vue.annexs.data = data.annex_data;
                            me.my_vue.flow_insts = data.flow_insts;
                            me.my_vue.reim_obj = data.reim_obj;
                            me.my_vue.old_flow_insts = data.old_flow_insts;
                            me.btn_enabled = data.btn_enabled;

                            console.log("me.my_vue.old_flow_insts", me.my_vue.old_flow_insts);

                        }, "json");

                    }

                    return obj;

                }

            </script>


        </div>

    </div>


</body>

</html>




