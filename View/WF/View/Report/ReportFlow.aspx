<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportFlow.aspx.cs" Inherits="App.InfoGrid2.WF.View.Report.ReportFlow" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />


    <title>流程中周报告明细</title>

    <script src="/WF/Script/common.js"></script>

</head>
<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">
    
    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">


             <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">周报告明细审核</h1>
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
                                        <div class="item-title">{{report_obj.COL_5}}</div> 
                                        <div class="item-after"></div>
                                    </div> 
                                    <div class="item-subtitle" style="color: rgba(76, 175, 80, 0.79);">{{report_obj.COL_20}}</div>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>

                <div style="margin:10px 0px;">
                    <span>审批编号：</span> <span>{{report_obj.BIZ_FLOW_INST_CODE}}</span>
                    <br />
                    <span>所在部门：</span><span>{{report_obj.COL_6}}.{{report_obj.COL_9}}.{{report_obj.COL_10}}</span>
                    <br />
                    <span>单据编号：</span><span>{{report_obj.COL_37}}</span>

                </div>

                <div class="list-block" style="margin:0px;">
                    <ul>
                        <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title" style="font-size:1rem;">报告时间  {{report_obj.COL_1}}  第 {{report_obj.COL_52}} 周 </div>
                              </div>
                            </div>
                        </li>
                    </ul>
                </div>

                <div class="list-block" style="margin:0px;">
                    <ul>
                        <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">人名</div>
                                <div class="item-input">
                                  <input type="text" readonly="readonly" :value="report_obj.COL_5" />
                                </div>
                              </div>
                            </div>
                          </li>
                          <li>
                            <div class="item-content">
                              <div class="item-inner">
                                 <div class="item-title label">报告主题</div>
                              </div>
                            </div>
                          </li>
                            <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-input">
                                  <textarea  readonly="readonly">{{report_obj.COL_26}}</textarea>
                                </div>
                              </div>
                            </div>
                          </li>
                          <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">销售情况</div>
                              </div>
                            </div>
                          </li>
                           <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-input">
                                  <textarea  style="height:10rem;" readonly="readonly">{{report_obj.COL_54}}</textarea>
                                </div>
                              </div>
                            </div>
                          </li>
                          <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">主要工作</div>
                              </div>
                            </div>
                          </li>
                           <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-input">
                                  <textarea  style="height:10rem;" readonly="readonly">{{report_obj.COL_54}}</textarea>
                                </div>
                              </div>
                            </div>
                          </li>
                          <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">问题及建议</div>
                              </div>
                            </div>
                          </li>
                             <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-input">
                                  <textarea style="height:10rem;">{{report_obj.COL_55}}</textarea>
                                </div>
                              </div>
                            </div>
                          </li>
                          <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">下周重点</div>
                              </div>
                            </div>
                          </li>
                           <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-input">
                                  <textarea  style="height:10rem;">{{report_obj.COL_56}}</textarea>
                                </div>
                              </div>
                            </div>
                          </li>
                    </ul>
                </div>

                <img-list :imgs="report_obj.imgs" ></img-list>
                    
                <annex-list :annexs="report_obj.annexs" ></annex-list>



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




            </div>


            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;


                        var id = xyf_util.getQuery(me.url,"report_id",true);

                        //同意按钮点击事件
                        el.on("click",".btn_agree",function(e){
    


                            var frm = Mini2.create('Mini2.ui.PromptWindow', {

                                contentHtml: '<div class="modal-text">审批意见</div><textarea  style="height:8rem;" class="modal-text-input">同意</textarea>',

                                title: '',

                                callback_ok: function(){

                                    $.post("/WF/Handlers/FlowHandler.ashx",{action:'SETP_AGREE_BY_REPORT',id:id,comment:this.getValue()},function(result){
                                        if(!result.success){
                                            $.alert(result.error_msg);
                                            return;
                                        }

                                        //提交成功了就隐藏按钮
                                        el.find(".btn_nav").hide();

                                        var url = Mini2.urlAppend("/WF/View/Success.aspx",{tiem:3,text:result.msg,url:encodeURIComponent(me.url)},true);

                                        $.router.load(url);

                            
                                    },"json");

                                
                                },

                            });

                            frm.render();  

                        });

                        //拒绝按钮点击事件
                        el.on("click",".btn_back",function(e){
                        

                            var frm = Mini2.create('Mini2.ui.PromptWindow', {

                                contentHtml: '<div class="modal-text">审批意见</div><textarea  style="height:8rem;" class="modal-text-input">拒绝</textarea>',

                                title: '',

                                callback_ok: function(){

                                    $.post("/WF/Handlers/FlowHandler.ashx",{action:'SETP_BACK_BY_REPORT',id:id,comment:this.getValue()},function(result){
                                        if(!result.success){
                                            $.alert(result.error_msg);
                                            return;
                                        }

                                        var url = Mini2.urlAppend("/WF/View/Success.aspx",{tiem:3,text:result.msg,url:encodeURIComponent(me.url)},true);

                                        $.router.load(url);

                                    },"json");

                                },

                            });


                            frm.render();

                        });

                        //退单按钮点击事件
                        el.on("click",".btn_back_start",function(e){
                        

                            var frm = Mini2.create('Mini2.ui.PromptWindow', {

                                contentHtml: '<div class="modal-text">审批意见</div><textarea  style="height:8rem;" class="modal-text-input">退回首节点</textarea>',

                                title: '',

                                callback_ok: function(){

                                    $.post("/WF/Handlers/FlowHandler.ashx",{action:'SETP_BACK_STRAT_BY_REPORT',id:id,comment:this.getValue()},function(result){
                                        if(!result.success){
                                            $.alert(result.error_msg);
                                            return;
                                        }

                                        var url = Mini2.urlAppend("/WF/View/Success.aspx",{tiem:3,text:result.msg,url:encodeURIComponent(me.url)},true);

                                        $.router.load(url);

                                    },"json");

                                },

                            });

                            frm.render();
                        });


                        //按钮是否隐藏
                        if(obj.btn_enabled){

                            el.find(".btn_nav").show();


                        }

                        //显示历史流程按钮点击事件
                        el.on("click",".btn_show_history",function(e){
                        
                            console.log("显示历史流程按钮点击了");
                            
                            if(my_vue.show_old_flow_insts){
                                my_vue.show_old_flow_insts = false;
                            }else{
                                my_vue.show_old_flow_insts = true;
                            }
                        });

                        //下一步按钮点击事件
                        el.on("click",".pull-right",function(){
                            
                            var flow_id = xyf_util.getQuery(me.url,"flow_id",true);
                            var biz_sid = xyf_util.getQuery(me.url,"biz_sid",true);

                            xyf_util.post("/App/InfoGrid2/WF/Handlers/ReportHandler.ashx","NEXT_REPORT",{flow_id:flow_id,biz_sid:biz_sid},function(data){
                                
                                var url = Mini2.urlAppend(data.url,{flow_id:data.flow_id,biz_sid:data.biz_sid},true);

                                $.router.load(url);

                            });

                                


                        });




                        obj.initVue(me);


                    }

                    obj.report_obj = <%= GetReportObj() %>;

                    obj.btn_enabled =<%= GetBtnEnabledBool()%>;

                    obj.old_flow_insts = <%= GetOldFlowInstArray() %>;

                    obj.flow_insts = <%= GetFlowInstArray() %>;


                    //vue对象
                    obj.my_vue = null;


                    //初始化vue相关对象
                    obj.initVue = function (me) {

                        var el = me.el;

                        var time = null;

                        obj.my_vue = new Vue({
                            el: el.children(".content")[0],
                            data: {
                                report_obj: obj.report_obj,
                                show_old_flow_insts:false,
                                old_flow_insts:obj.old_flow_insts,
                                flow_insts:obj.flow_insts
                            },
                            methods:{
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



                    return obj;

                }

            </script>


        </div>

    </div>


</body>
</html>
