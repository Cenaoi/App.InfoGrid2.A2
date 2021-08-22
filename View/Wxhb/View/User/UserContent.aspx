<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserContent.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.User.UserContent" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>微信红包</title>

    <script src="/Wxhb/Script/common.js?v=201702210001"></script>

</head>
<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <h1 class="title def_title">用户中心</h1>
            </header>

            <div class="content" style="display:none;">
                  
               <div style="margin:0px;" class="list-block media-list">
                    <ul>
                        <li>
                            <div style="padding-left:0.5rem;" class="item-content ">
                                <div class="item-media">
                                    <img :src="user_obj.head_img_url" style="width: 2.5rem;">
                                </div> 
                                <div class="item-inner">
                                    <div class="item-title-row">
                                        <div class="item-title">{{user_obj.user_text}}</div>
                                        <div class="item-after"></div>
                                    </div> 
                                     <div class="item-subtitle">累计总收入：{{user_obj.col_13}} 元</div>
                                     <div class="item-text">已提现金额：{{user_obj.col_15}} 元</div>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>

               <div class="list-block" style="margin-top:0px;">
                    <ul>
                        <li class="item-content item-link me_qrcode">
                            <div class="item-inner">
                                <div class="item-title">专属二维码</div>
                            </div>
                        </li>
                        <li class="item-content">
                            <div class="item-inner">
                                <div class="item-title">领取红包总数：{{user_obj.col_1}} 个</div>
                            </div>
                        </li>
                        <li class="item-content">
                            <div class="item-inner">
                                <div class="item-title">红包收入：{{user_obj.col_2}} 元</div>
                            </div>
                        </li>
                        <li class="item-content item-link user_col_16">
                            <div class="item-inner">
                                <div class="item-title">未提现金额：{{user_obj.col_16}} 元</div>
                            </div>
                        </li>
                        <li class="item-content item-link user_col_4">
                            <div class="item-inner">
                                <div class="item-title">推荐用户收入：{{user_obj.col_4}} 元</div>
                            </div>
                        </li>
                    </ul>
                </div>

                <div class="list-block media-list" v-if="borwses.length > 0">
                    <ul>
                      <li v-for="(b,i) in borwses">
                        <div class="item-content">
                          <div class="item-inner">
                           <div>{{ i + 1}}.{{b.ADVE_TITLE}}</div>
                            <div class="content-block" style="margin:0px;">
                                <p class="buttons-row">
                                  <a href="#" class="button active" v-if="b.IS_TOP"  @click="b_collection_btn(b)" >收藏</a>
                                   <a href="#" class="button" v-else  @click="b_collection_btn(b)" >收藏</a>
                                  <a href="#" class="button" @click="b_msg_btn(b)">消息</a>
                                  <a href="#" class="button" @click="b_delete_btn(b)" >删除</a>
                                </p>
                            </div>
                          </div>
                        </div>
                      </li>
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

                        var v_id = xyf_util.getQuery(me.url, "v_id", true);


                        $.post("/Wxhb/Handlers/LoginHandler.ashx", { action: 'AUTO_LOGIN', v_id: v_id }, function () {

                            obj.initVue(me);

                           

                            $.post("/Wxhb/Handlers/UserHandler.ashx", { action: "GET_USER" }, function (result) {


                                if (!result.success) {


                                    $.alert(result.error_msg);

                                    return;

                                }

 
                                obj.my_vue.user_obj = result.data;

                                
                                el.children(".content").show();

                            }, "json");

                            

                            obj.getBrowses();

 
                        }, "json");


                        //专属二维码点击事件
                        el.on("click", ".me_qrcode", function (e) {

                            
                            var url = Mini2.urlAppend("ShowQrcode.aspx", {show_type:'temp'}, true);

                            $.router.load(url);                            

                        });

                        //未提现金额列表点击事件
                        el.on("click", ".user_col_16", function (e) {

                            var url = Mini2.urlAppend("WithdrawalsContent.aspx", {}, true);

                            $.router.load(url);

                        });

                        //推荐用户点击事件
                        el.on("click", ".user_col_4", function (e) {

                            var url = Mini2.urlAppend("Recommend.aspx", {}, true);

                            $.router.load(url);

                        });

                    }

                    //页面加载成功事件
                    obj.onLoad = function () {

                        //获取微信js配置对象
                        $.post("/Wxhb/Handlers/UserHandler.ashx", { action: 'GET_WX_JS_CONFIG' }, function (result) {

                            if (!result.success) {

                                $.alert(result.error_msg);
                                return;

                            }

                            var config = result.data;

                            var config_str = JSON.stringify(config);

                            wx.config(config);

                            wx.ready(function () {

                                //这是隐藏微信自带的右上角菜单  所有非基础按钮接口
                                wx.hideAllNonBaseMenuItem();

                            });



                        }, "json");

                    }

                    //初始化vue对象
                    obj.initVue = function (me) {

                        var el = me.el;

                        obj.my_vue = new Vue({

                            el: el.children(".content")[0],
                            data: {
                                user_obj: {},
                                borwses: []

                            },
                            methods: {

                                //浏览历史的消息按钮事件
                                b_msg_btn: function () {

                                    var my_vue = this;

                                    $.toast("消息按钮点击了！");



                                },
                                //浏览历史的删除按钮事件
                                b_delete_btn: function (b) {

                                    var my_vue = this;

                                    $.confirm("是否要删除浏览历史", "", function (e) {

                                        $.post("/Wxhb/Handlers/UserHandler.ashx", { action: 'BROWSE_DELETE', id: b.ROW_IDENTITY_ID }, function (result) {

                                            if (!result.success) {

                                                $.alert(result.error_msg);

                                                return;
                                            }


                                            xyf_util.arrayRemove(my_vue.borwses, b);


                                        }, "json");

                                    });

                                },
                                //浏览历史的收藏按钮事件
                                b_collection_btn: function (b) {

                                    var my_vue = this;




                                    $.post("/Wxhb/Handlers/UserHandler.ashx", { action: 'BROWSE_COLLECTION', id: b.ROW_IDENTITY_ID }, function (result) {

                                        if (!result.success) {

                                            $.alert(result.error_msg);

                                            return;
                                        }

                                        obj.getBrowses();



                                    }, "json");


                                },
                                //浏览历史的消息按钮点击事件
                                b_msg_btn: function (b) {

                                    var my_vue = this;

                                    var url = Mini2.urlAppend("/Wxhb/View/Msg/AdveHistoryMsg.aspx", { adve_code:  b.FK_ADVE_CODE }, true);

                                    $.router.load(url);
                                }

                            }

                        });

                    }


                    //获取浏览历史数据集合
                    obj.getBrowses = function () {

                        $.post("/Wxhb/Handlers/UserHandler.ashx", { action: 'GET_BROWSES' }, function (result) {

                            if (!result.success) {
                                $.alert(result.error_msg);
                                return;
                            }


                            obj.my_vue.borwses = result.data;


                        }, "json");

                    }






                    //Vue对象
                    obj.my_vue = null;

                    return obj;

                }

            </script>

        </div>

    </div>

</body>
</html>
