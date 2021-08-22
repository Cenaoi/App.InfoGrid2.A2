<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusContent.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.Bus.BusContent" %>

<!DOCTYPE html>

<html>

<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>微信红包</title>

    <script src="/Wxhb/Script/common.js?v=20170210003"></script>

</head>

<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <h1 class="title def_title">商家</h1>
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
                                        <div class="item-title">{{user_obj.col_23}}</div>
                                        <div class="item-after"></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>

                <div class="list-block" style="margin-top:0px;margin-bottom:0px;">
                    <ul>
                        <li class="item-content">
                            <div class="item-inner">
                                <div class="item-title">电话：{{user_obj.col_20}}</div>
                            </div>
                        </li>
                        <li class="item-content item-link bus_recharge">
                            <div class="item-inner">
                                <div class="item-title">账户余额：{{user_obj.col_21}} 元</div>
                            </div>
                        </li>


                        <li class="block-title" style="padding-left: 10px;">店铺位置<li>
                        <li class="item-content item-link" @click="shop_address_edit">
                            <div class="item-inner">
                                <div class="item-title" style="font-size:0.7rem;white-space:inherit;">{{user_obj.col_27}} </div>
                            </div>
                        </li>
                    </ul>
                </div>

                <div class="content-block">
                    <p class="buttons-row"><a href="#" class="button button-big active new_adve">新增广告</a></p>
                </div>

                <div class="list-block media-list">
                    <ul>
                      <li v-for="a in adves">
                        <div class="item-content">
                          <div class="item-inner">
                            <div>{{a.COL_1}}</div>
                            <div>地址：{{a.COL_6}}</div>
                            <div>累计点击：{{a.BROWSE_NUM}}</div>
                            <div>消费金额：{{a.MONEY_CONSUME}}</div>
                            <div>已领红包数：{{a.HB_CONSUME}}</div>
                            <div>状态：上线</div>
                            <div class="content-block" style="margin:0px;">
                                <p class="buttons-row">
                                  <a href="#" class="button" @click="adve_edit(a)">内容编辑</a>
                                  <a href="#" class="button" @click="adve_customer(a)">客户管理</a>
                                  <a href="#" class="button" @click="adve_delete(a)">删除广告</a>
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
                          

                            obj.getUser(me);

                            $.post("/Wxhb/Handlers/BusHandler.ashx", { action: "GET_ADVES_BY_USER_ID" }, function (result) {


                                if (!result.success) {

                                    $.alert(result.error_msg);

                                    return;

                                }

                                var data = result.data;

                                obj.my_vue.adves = data

                            }, "json");
                            
                        }, "json");
                        



                        //充值点击事件
                        el.on("click", ".bus_recharge", function (e) {

                            var url = Mini2.urlAppend("/Wxhb/View/Bus/Recharge.aspx", {}, true);

                            $.router.load(url);

                        });

                        //新增广告按钮点击事件
                        el.on("click", ".new_adve", function (e) {

                            $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'NEW_ADVE' }, function (result) {


                                //if (!result.success) {

                                //    $.toast(result.error_msg);
                                //    return;

                                //}


                               // $.toast(result.msg);




                                var url = Mini2.urlAppend("/Wxhb/View/Bus/NewAdve.aspx", { adve_id: result.data.adve_id}, true);

                                $.router.load(url);

                            }, "json");

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


                    //初始化vue相关对象
                    obj.initVue = function (me) {

                        var el = me.el;

                        obj.my_vue = new Vue({
                            el: el.children(".content")[0],
                            data: {
                                user_obj: {},
                                adves: [],

                            },
                            methods: {
                                //删除广告按钮点击事件
                                adve_delete: function (adve) {

                                    var my_vue = this;


                                    $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'DELETE_ADVE', adve_id: adve.ROW_IDENTITY_ID}, function (result) {

                                        if (!result.success) {

                                            $.alert(result.error_msg);

                                            return;

                                        }

                                        $.toast(result.msg);

                                    }, "json");


                                },
                                //编辑广告按钮点击事件
                                adve_edit: function (adve) {

                                    var my_vue = this;

                                    var url = Mini2.urlAppend("/Wxhb/View/Bus/NewAdve.aspx", { adve_id: adve.ROW_IDENTITY_ID,show_type:'edit' }, true);

                                    $.router.load(url);

                                },
                                //广告客户管理
                                adve_customer: function () {

                                    var my_vue = this;

                                    $.toast("这个不知道干嘛的！");

                                },
                                //商铺位置的修改事件
                                shop_address_edit: function () {

                                    var my_vue = this;

                                    var lat = my_vue.user_obj.col_25;
                                    var lon = my_vue.user_obj.col_24;
                                    var addr = my_vue.user_obj.col_27;

                                    var url = Mini2.urlAppend("/Wxhb/View/Map/MapAddress.aspx", { lat: lat, lon: lon, addr: addr, action: 'edit_shop_addres'}, true);


                                    $.router.load(url);

                                }
                            }

                        });




                    }


                    //Vue对象
                    obj.my_vue = null;

                    //获取用户数据
                    obj.getUser = function (me) {

                        var el = me.el;

                        $.post("/Wxhb/Handlers/UserHandler.ashx", { action: "GET_USER" }, function (result) {

                            if (!result.success) {

                                $.alert(result.error_msg);

                                return;

                            }

                            //不是商家就跳到商家注册界面
                            if (!result.data.col_19) {


                                var url = Mini2.urlAppend("/Wxhb/View/Bus/Register.aspx", { v_id: v_id }, true);

                                $.router.load(url);

                                return;

                            }

                            obj.my_vue.user_obj = result.data;


                            el.children(".content").show();

                        }, "json");


                    }

                    //页面恢复的事件
                    obj.pageRevert = function () {

                        var me = this,
                            el = me.el;


                        console.log("页面恢复事件！");

                        obj.getUser(me);


                    }

                    return obj;

                }

            </script>

        </div>

    </div>

</body>

</html>
