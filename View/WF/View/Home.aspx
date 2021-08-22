<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="App.InfoGrid2.WF.View.Home" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>流程审批</title>
    <script src="/WF/Script/common.js?v=20170523"></script>

    <style>
        .mu-icon-box {
            padding: 30px 0px 0.5rem 0px;
            position: relative;
        }

        .mu-icon {
            font-family: '微软雅黑';
            height: 72px;
            width: 72px;
            border-radius: 4px;
            overflow: hidden;
            color: white;
            font-size: 30px;
            margin-left: auto;
            margin-right: auto;
            cursor: pointer;
            text-align: center;
        }

        .mu-icon-text {
            font-family: '微软雅黑';
            color: white;
            font-size: 30px;
            cursor: pointer;
            pointer-events: none;
        }

        .mi-icon-label {
            overflow: hidden;
            color: #9e9e9e;
            margin-top: 5px;
            font-weight: normal;
            margin-left: auto;
            margin-right: auto;
            text-align: center;
            font-size:15px;
        }

        .mi-icon-tag {
            right: 36px;
            top: 32px;
            position: absolute;
            width: 16px;
            height: 16px;
            background-image: url(/res/icon/application_view_columns.png);
        }


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

            .arrow {
                width:0;
                height:0;
                font-size:0;
                border:solid 10px #000;
                }

             .send {

                background: #ffffff;
                    border-radius: 5px;
                    padding: 0.5rem;
                    border: 1px solid #9E9E9E;
                }

                .send .arrow {
                    position: absolute;
                    top: 10px;
                    left: 5px;
                    width: 0;
                    height: 0;
                    font-size: 0;
                    border: solid 8px;
                    border-color: rgba(248, 195, 1, 0) rgb(158, 158, 158) rgba(248, 195, 1, 0) rgba(248, 195, 1, 0);
                }



    </style>

</head>

<body style="max-width:640px; margin:auto; font-family: Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;" >


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <nav class="bar bar-tab">
              <a class="tab-item external active" href="#">
                <span class="icon icon-home"></span>
                <span class="tab-label">流程</span>
              </a>
              <a class="tab-item tab_me">
                <span class="icon icon-me"></span>
                <span class="tab-label">我的</span>
              </a>
            </nav>

            <div class="content" style="background-color: rgba(158, 158, 158, 0.19);">

                <div class="row" style="padding:0 0.5rem 0.5rem 0.5rem;">

                    <div class="col-33 flow_eaa" style="margin-top: 10px;">

                        <div class="menu_box mu-icon-box" style="border-radius:10px;background-color: #FFF;" >
                            <div class="text-center" style="border-radius: 8px; padding-top:0px;">
                                <img src="/WF/res/25.png" class=" mu-icon" />
                            </div>
                            <div v-show="eaa_num > 0" style="height:20px; width:36px; position:absolute;cursor:pointer;right:0.2rem;top:0.2rem;background-color:red;border-radius:8px; color:white;text-align:center; font-weight:bold;font-size:0.7rem;">{{eaa_num}}</div>
                            <div class="text-center mi-icon-label">
                                待我审批
                            </div>
                        </div>

                    </div>

                    <div class="col-33 flow_start" style="margin-top: 10px; ">

                        <div class="menu_box mu-icon-box" style="border-radius:10px;background-color: #FFF;" >
                            <div class="text-center mu-icon" style="border-radius: 8px; padding-top:0px;">
                                <img src="/WF/res/h9.png" class="mu-icon" />
                                 
                            </div>
                            <div v-show="draft_num > 0" style="height:20px; width:36px; position:absolute;cursor:pointer;right:0.2rem;top:0.2rem;background-color:red;border-radius:8px; color:white;text-align:center; font-weight:bold;font-size:0.7rem;">{{draft_num}}</div>
                            <div class="text-center mi-icon-label">
                                我发起的
                            </div>
                        </div>

                    </div>

                    <div class="col-33 flow_cc" style="margin-top: 10px;">

                        <div class="menu_box mu-icon-box" style="border-radius:10px;background-color: #FFF;" >
                            <div class="text-center" style="padding-top:0px; border-radius: 8px;">
                                <img src="/WF/res/129.png" class="mu-icon"  />
                            </div>
                            <div v-show="cc_num > 0" style="height:20px; width:36px; position:absolute;cursor:pointer;right:0.2rem;top:0.2rem;background-color:red;border-radius:8px; color:white;text-align:center; font-weight:bold;font-size:0.7rem;">{{cc_num}}</div>
                            <div class="text-center mi-icon-label">
                               抄送我的
                            </div>
                        </div>

                    </div>

                </div>

                <div class="row  no-gutter" style="background-color:white;padding-bottom:25px;">

                    <div class="row  no-gutter">
                        <div style="height:2rem;line-height:2.5rem;">

                            <div  style="padding-left:0.55rem;float:left;">
                                {{login_name}}
                            </div>
                            <div  style="text-align:right;padding-right:0.7rem;float:right;">
                                部门：{{user_obj.COL_12}}
                            </div>

                        </div>
                    </div>
                    <div class="row  no-gutter" >

                        <div class="col-33 month_reim" >
                            <div class="menu_box mu-icon-box">
                                <div class="text-center mu-icon" style="border-radius: 8px;padding-top:0px;">
                                    <img src="/WF/res/s6.png" style="width:100%;" />
                                </div>
                                <div class="text-center mi-icon-label" >
                                    月度报销
                                </div>
                            </div>
                        </div>

                        <div class="col-33">
                            <div class="menu_box mu-icon-box">
                                <div class="text-center mu-icon" style="border-radius: 8px;padding-top:0px;">
                                    <img src="/WF/res/home_001.png" style="width:100%;" />
                                </div>
                                <div class="text-center mi-icon-label">
                                    签到打卡
                                </div>
                            </div>
                        </div>

                        <div class="col-33 week_report">
                            <div class="menu_box mu-icon-box">
                                <div class="text-center mu-icon" style="border-radius: 8px;padding-top:0px;">
                                     <img src="/WF/res/home_002.png" style="width:100%;" />
                                </div>
                                <div class="text-center mi-icon-label">
                                    周报告
                                </div>
                            </div>
                        </div>

                        <div class="col-33">
                            <div class="menu_box mu-icon-box">
                                <div class="text-center mu-icon" style="border-radius: 8px;padding-top:0px;">
                                     <img src="/WF/res/home_003.png" style="width:100%;" />
                                </div>
                                <div class="text-center mi-icon-label">
                                    请假
                                </div>
                            </div>
                        </div>

                        <div class="col-33">
                            <div class="menu_box mu-icon-box">
                                <div class="text-center mu-icon" style="border-radius: 8px;padding-top:0px;">
                                     <img src="/WF/res/home_004.png" style="width:100%;" />
                                </div>
                                <div class="text-center mi-icon-label">
                                    合同审批
                                </div>
                            </div>
                        </div>

                        <div class="col-33">
                            <div class="menu_box mu-icon-box">
                                <div class="text-center mu-icon" style="border-radius: 8px;padding-top:0px;">
                                     <img src="/WF/res/home_005.png" style="width:100%;" />
                                </div>
                                <div class="text-center mi-icon-label">
                                    客户订单
                                </div>
                            </div>
                        </div>

                        <div class="col-33">
                            <div class="menu_box mu-icon-box">
                                <div class="text-center mu-icon" style="border-radius: 8px;padding-top:0px;">
                                     <img src="/WF/res/home_006.png" style="width:100%;" />
                                </div>
                                <div class="text-center mi-icon-label">
                                    用车申请
                                </div>
                            </div>
                        </div>

                        <div class="col-33">
                            <div class="menu_box mu-icon-box">
                                <div class="text-center mu-icon" style="border-radius: 8px;padding-top:0px;">
                                     <img src="/WF/res/home_007.png" style="width:100%;" />
                                </div>
                                <div class="text-center mi-icon-label">
                                    物品申购
                                </div>
                            </div>
                        </div>

                        <div class="col-33">
                            <div class="menu_box mu-icon-box">
                                <div class="text-center mu-icon" style="border-radius: 8px;padding-top:0px;">
                                     <img src="/WF/res/home_008.png" style="width:100%;" />
                                </div>
                                <div class="text-center mi-icon-label">
                                    小组会议
                                </div>
                            </div>
                        </div>

                    </div>

                </div>

            </div>


            <script type="text/javascript" data-main="true">
        
                function main() {


                    var obj = {};

                    obj.pageRefresh = function () {

                        var me = this;

                        me.initData();

                        console.log("刷新重新加载页面数据！");
                    }

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        console.log("首页加载了！");

                        //月度报销点击按钮
                        el.on("click", ".month_reim", function (e) {


                            $.post("/WF/Handlers/HomeHandler.ashx", { action: 'NEW_REIM' }, function (result) {

                                if (!result.success) {

                                    $.alert(result.error_msg);

                                    return;
                                }

                                var data = result.data;

                                var url = Mini2.urlAppend("Reim/Reimbur.aspx", { id: data.ROW_IDENTITY_ID}, true);

                                $.router.load(url);


                            }, "json");



                        });

                        el.on("click",".week_report",function(e){
                        
                        
                            var url =  Mini2.urlAppend("Report/ReimList.aspx", {}, true);

                            $.router.load(url);


                        });


                        //待我审批点击事件
                        el.on("click", ".flow_eaa", function (e) {

                            $.router.load("Flow/FlowEaa.aspx", true)

                        });

                        //我发起的点击事件
                        el.on("click", ".flow_start", function (e) {

                            $.router.load("Flow/FlowStart.aspx", true);
                        });

                        //抄送我的点击事件
                        el.on("click", ".flow_cc", function (e) {

                            $.router.load("Flow/FlowCC.aspx", true);

                        });

                       
                        //点击用户中心
                        el.on("click",".tab_me",function(e){
                            
                            var v_id = xyf_util.getQuery(me.url,"v_id",true);

                            location.href =  Mini2.urlAppend("/WF/View/User/UserContent.aspx", {v_id:v_id}, true);

                        });

                        me.initVue();
                    }                    

                    //vue对象
                    obj.my_vue = null;

                    //初始化vue相关对象
                    obj.initVue = function () {

                        var me = this;
                        var el = me.el;

                        me.my_vue = new Vue({
                            el: el.children(".content")[0],
                            data: {
                                //待审核记录
                                eaa_num: null,
                                //抄送未读记录
                                cc_num: null,
                                //用户对象
                                user_obj: {},
                                //制单中的数据
                                draft_num: null,
                                //登录名称
                                login_name: ''
                            }
                        });
                
                    }


                    ///初始化数据
                    obj.initData = function () {

                        var me = this;
                        var el = me.el;

                        $.post("/WF/Handlers/HomeHandler.ashx", { action: 'INIT_DATA_HOME' }, function (result) {

                            var data = result.data;

                            me.my_vue.eaa_num = data.eaa_num;
                            me.my_vue.cc_num = data.cc_num;
                            me.my_vue.user_obj = data.user_obj;
                            me.my_vue.draft_num = data.draft_num;
                            me.my_vue.login_name = data.login_name;

                        }, "json");

                    }

                    return obj;

                }

            </script>


        </div>

    </div>


</body>
</html>














