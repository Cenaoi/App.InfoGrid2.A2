<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkReport.aspx.cs" Inherits="App.InfoGrid2.WF.View.Reim.WorkReport" %>

<!DOCTYPE html>

<html>
<head>
    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <script src="/WF/Script/common.js"></script>

    <title>工作报告</title>
</head>
<body style="max-width:640px; margin:auto; font-family: Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">工作报告</h1>
                <a class="icon icon-home pull-right expand" href="/WF/View/Home.aspx"></a>
            </header>
            <nav class="bar bar-tab" style="background-color:#4cd964;">
                <a class="tab-item btn_success saveAndRed">
                     <span class="tab-label " style="color:white;">保存并返回</span>
                </a>     
            </nav>
            <div class="content">

                  <template v-for="(t,t_index) in trip_detas">
                 
                    <div class="row" style="padding:10px 20px;">

                        <div class="col-50">工作报告({{ t_index + 1 }})</div>
                        <div class="col-50" v-show="t_index > 0"  style="text-align:right;">
                            <a href="#" @click="delete_reim(t)" >删除</a>
                        </div>
                    </div>

                    <div class="list-block" style="margin:0px;">
                        <ul>
                          <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">报告主题</div>
                                <div class="item-input">
                                  <input type="text" v-model="t.COL_26" @keyup="save_trip_deta(t,'COL_26')">
                                </div>
                              </div>
                            </div>
                          </li>
                           <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">报告内容</div>
                                <div class="item-input">
                                  <textarea  placeholder="请输入报告内容" v-model="t.COL_27" @keyup="save_trip_deta(t,'COL_27')"></textarea>
                                </div>
                              </div>
                            </div>
                          </li>
                        </ul>
                    </div>

                    <img-list :imgs="t.imgs"></img-list>
                    
                    <annex-list :annexs="t.annexs"></annex-list> 


                </template>

                <div class="content-block" style="margin-top:0.5rem;">
                    <p><a href="#" class="button button-success button-fill button-big">增加工作报告</a></p>
                </div>




            </div>

            <script type="text/javascript" data-main="true">
        
                function main() {
            
                    
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        var row_id = xyf_util.getQuery(me.url,"id",true);

                        me.initVue();
                        
                        //增加报销明细 按钮点击事件
                        el.on("click", ".button-success", function () {
                           
                            $.post("/WF/Handlers/ReimHandler.ashx",{action:"NEW_WORK_REPORT",id:row_id},function(result){
                            
                                if(!result.success){
                                    $.toast(result.error_msg);
                                    return;
                                }

                                var v = result.data;

                                me.addImgAndAnnex(v);

                                me.my_vue.trip_detas.push(v);

                            },"json");
        
                        });
                        el.on("click", ".saveAndRed", function () {

                            var json = me.my_vue.trip_detas;
                            var json_to_str = JSON.stringify(json);
                            console.log("修改的数据" + json_to_str);
                            $.post("/WF/Handlers/ReimHandler.ashx", { action: "WORKREPOSRT_SAVE_ALL", json_to_str: json_to_str, table_name: "UT_351" }, function (data) {
                               

                                console.log("返回的数据", data);
                                if (data.success == true) {
                                 
                                    $.toast("已完成");
                                    $.router.back();
                                } else {
                                    $.toast("保存失败");
                                    $.router.back();
                                }
                                
                            },"json");
                            
                        })
        

                    }

                    //刷新加载数据
                    obj.pageRefresh = function () {

                        var me = this;
                   
                        me.my_vue.trip_detas =<%= GetWorkReportsArray() %>;
                    }

                    obj.trip_detas = <%= GetWorkReportsArray() %>;;

                    //vue对象
                    obj.my_vue = null;

                    //初始化vue相关对象
                    obj.initVue = function () {

                        var me = this;

                        var el = me.el;

                        var row_id = xyf_util.getQuery(me.url,"id",true);

                        var time = null;

                        me.my_vue = new Vue({
                            el: el.children(".content")[0],
                            data: {
                                trip_detas: me.trip_detas,
                                //改变字段
                                change_fields: [],
                                cur_row:null
                            },
                            methods: {
                                delete_reim: function (r) {

                                    var my_vue = this;

                                    $.confirm("你确定要删除数据吗？", "", function () {

                                        $.post("/WF/Handlers/ReimHandler.ashx", { action: "DELETE_REIM_DETA", id: r.ROW_IDENTITY_ID,table_name:'UT_351'}, function (result) {

                                            if (!result.success) {

                                                $.toast(result.error_msg);

                                                return;

                                            }

                                            Mini2.Array.remove(my_vue.trip_detas, r);

                                            $.toast("删除成功了！");


                                        }, "json");


                                    });
                                },
                                //只要变更一个就保存整个对象
                                save_trip_deta: function (r, field_text) {

                                    var my_vue = this;

                                    my_vue.cur_row = r;

                                    Mini2.Array.include(my_vue.change_fields, field_text);

                                    if (time) {
                                        time.resetStart();
                                    }
                                    else {
                                        time = Mini2.setTimer(function () {


                                                $.post("/WF/Handlers/ReimHandler.ashx", { 
                                                    action: 'SAVE_REIM_DETA', 
                                                    id: row_id, 
                                                    reim_deta_json_str: JSON.stringify(my_vue.cur_row), 
                                                    change_files_str: JSON.stringify(my_vue.change_fields),
                                                    table_name:'UT_351'}, function (result) {

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

                    //添加图片组件和附件数据给出差记录中
                    obj.addImgAndAnnex = function (reim_deta) {

                        var imgs = {
                            data: [],
                            server_url:"/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_351&tag_code=work_report_img&row_id="+reim_deta.ROW_IDENTITY_ID,
                            delete_img_url: '/WF/Handlers/UploaderFileHandle.ashx',
                            btn_id:'uploader_img_'+reim_deta.ROW_IDENTITY_ID,
                            row_id:reim_deta.ROW_IDENTITY_ID,
                            table_name:'UT_351',
                            tag_code:'work_report_img',
                            field_text:'COL_22'
                        }

                        var annexs = {
                            data: [],
                            server_url: '/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_351&tag_code=work_report_annex&row_id=' + reim_deta.ROW_IDENTITY_ID,
                            delete_annex_url: '/WF/Handlers/UploaderFileHandle.ashx',
                            btn_id:'uploader_annex_'+reim_deta.ROW_IDENTITY_ID,
                            row_id:reim_deta.ROW_IDENTITY_ID,
                            table_name:"UT_351",
                            tag_code:"work_report_annex",
                            field_text:"COL_23"
                        }

                        reim_deta.imgs = imgs;

                        reim_deta.annexs = annexs;


                    }

                    return obj;

                }

            </script>


        </div>

    </div>


</body>

</html>


