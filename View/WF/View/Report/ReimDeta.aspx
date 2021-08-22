<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReimDeta.aspx.cs" Inherits="App.InfoGrid2.WF.View.Report.ReimDeta" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />


    <title>周报告明细</title>

    <script src="/WF/Script/common.js"></script>
        
</head>
<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">


             <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">周报告明细</h1>
            </header>

            <nav class="bar bar-tab" >
              <a v-if="show_type!=show" class="tab-item btn_success" style="background-color:#4cd964;"  href="#">
                <span class="tab-label" style="color:white;" >提交</span>
              </a>
              <a class="tab-item " style="background-color:#808080;"  href="#">
                <span class="tab-label" style="color:white;" >已提交</span>
              </a>
            </nav>


            <div class="content">


                    <div class="list-block" style="margin:0px;">
                        <ul>
                          <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">时间</div>
                                <div class="item-input">
                                  <input v-show="show_type == 'edit'"  type="date" v-model="report_obj.COL_1" @change="save_trip_deta('COL_1')" >
                                  <input v-show="show_type == 'show'"  type="text" :value="report_obj.COL_1" >
                                </div>
                              </div>
                            </div>
                          </li>
                          <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">第 {{report_obj.COL_52}} 周</div>
                              </div>
                            </div>
                          </li>
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
                                  <textarea v-show="show_type == 'edit'"   v-model="report_obj.COL_26" @keyup="save_trip_deta('COL_26')"  @change="save_trip_deta('COL_26')"></textarea>
                                  <textarea  v-show="show_type == 'show'"  readonly="readonly">{{report_obj.COL_26}}</textarea>
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
                                  <textarea v-show="show_type == 'edit'"  style="height:10rem;"  v-model="report_obj.COL_53" @keyup="save_trip_deta('COL_53')" @change="save_trip_deta('COL_53')"></textarea>
                                  <textarea  v-show="show_type == 'show'" style="height:10rem;" readonly="readonly">{{report_obj.COL_54}}</textarea>
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
                                  <textarea v-show="show_type == 'edit'" style="height:10rem;"  v-model="report_obj.COL_54" @keyup="save_trip_deta('COL_54')" @change="save_trip_deta('COL_54')"> ></textarea>
                                  <textarea v-show="show_type == 'show'" style="height:10rem;" readonly="readonly">{{report_obj.COL_54}}</textarea>
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
                                  <textarea v-if="show_type == 'edit'" style="height:10rem;" v-model="report_obj.COL_55" @keyup="save_trip_deta('COL_55')"  @change="save_trip_deta('COL_55')">></textarea>
                                  <textarea  v-show="show_type == 'show'" style="height:10rem;">{{report_obj.COL_55}}</textarea>
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
                                  <textarea v-if="show_type == 'edit'" style="height:10rem;" v-model="report_obj.COL_56" @keyup="save_trip_deta('COL_56')" @change="save_trip_deta('COL_56')">></textarea>
                                  <textarea  v-show="show_type == 'show'" style="height:10rem;">{{report_obj.COL_56}}</textarea>
                                </div>
                              </div>
                            </div>
                          </li>
                        </ul>
                    </div>

                    <img-list :imgs="report_obj.imgs" ></img-list>
                    
                    <annex-list :annexs="report_obj.annexs" ></annex-list>



            </div>


            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        var show_type = xyf_util.getQuery(me.url,"show_type",true);

                        var id = xyf_util.getQuery(me.url,"id",true);

                        //隐藏提交按钮
                        if(show_type === "show"){
                            el.find(".btn_success").hide();
                        }

                        obj.initVue(me);

                        //提交按钮点击事件
                        el.on("click",".btn_success",function(e){
                        
                            $.post("/WF/Handlers/ReportHandler.ashx",{action:'SUBMIT_REPORT',id:id},function(result){
                            
                                if(!result.success){

                                    $.toast(result.error_msg);

                                    return;
                                }

                                var url = Mini2.urlAppend("/WF/View/Success.aspx",{tiem:3,text:'提交成功'},true);

                                $.router.load(url);

                            },"json");

                        });


                    }

                    obj.report_obj = <%= GetReportObj() %>;


                    //vue对象
                    obj.my_vue = null;


                    //初始化vue相关对象
                    obj.initVue = function (me) {

                        var el = me.el;

                        var time = null;

                        var show_type = xyf_util.getQuery(me.url,"show_type",true);


                        obj.my_vue = new Vue({
                            el: el.children(".content")[0],
                            data: {
                                report_obj: obj.report_obj,
                                change_fields:[],
                                show_type:show_type
                               
                            },
                            methods: {
                                //只要变更一个就保存整个对象
                                save_trip_deta: function (field_text) {

                                    var my_vue = this;

                                    //时间特殊处理，要转成周数
                                    if(field_text === "COL_1"){

                                        my_vue.report_obj.COL_52 = moment(my_vue.report_obj.COL_1).week(); 
                             
                                        Mini2.Array.include(my_vue.change_fields, "COL_52");
                                    }

                                  
                                    Mini2.Array.include(my_vue.change_fields, field_text);

                                    if (time) {
                                        time.resetStart();
                                    }
                                    else {
                                        time = Mini2.setTimer(function () {


                                            console.log(JSON.stringify(my_vue.change_fields));

                                            $.post("/WF/Handlers/ReimHandler.ashx", 
                                                { 
                                                    action: 'SAVE_REIM_DETA',
                                                    id: my_vue.report_obj.ROW_IDENTITY_ID, 
                                                    reim_deta_json_str: JSON.stringify(my_vue.report_obj),
                                                    change_files_str: JSON.stringify(my_vue.change_fields),
                                                    table_name:'UT_371'
                                                }, function (result) {

                                                    if (!result.success) {

                                                        console.error(result);

                                                        return;
                                                    }

                                                }, "json");
                                            

                                        }, { interval: 500, limit: 1});
                                    }

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
