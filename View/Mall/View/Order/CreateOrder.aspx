<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateOrder.aspx.cs" Inherits="App.InfoGrid2.Mall.View.Order.CreateOrder" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>致瑧服饰商城</title>

    <script src="/Core/Scripts/XYF/common.js"></script>

</head>
<body>

    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class=" icon icon-left pull-left back" ></a>
                <h1 class="title">确认订单</h1>
            </header>

            <div class="vue_conter">


                <div class="bar bar-footer">
                  <a class="pull-left">
                        共
                        <span style="color: #DD2727;font-size:1rem;">{{cars.length}}</span>
                        件，合计：
                        <span style="color: #DD2727; font-size: 1rem; text-align: right;">￥{{money}}</span>
                  </a>
                  <a class="button button-fill button-big button-danger btn_sumbit pull-right" style="top: 0;">
                     提交订单
                  </a>
                </div>



                <div class="content" style="top:2.2rem;" >

                    <div class="list-block" style="margin-top:0px;">
                        <ul>
                                <li>
                                    <div class="item-content">
                                        <div class="item-inner">
                                            <div class="item-title label">收货人：</div>
                                            <div class="item-input">
                                                <input type="text" placeholder="填写" v-model="user.COL_23" />
                                            </div>
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div class="item-content">
                                        <div class="item-inner">
                                            <div class="item-title label">手机号：</div>
                                            <div class="item-input">
                                                <input type="text" placeholder="填写" v-model="user.COL_20" />
                                            </div>
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div class="item-content">
                                        <div class="item-inner">
                                            <div class="item-title label">收货地址：</div>
                                            <div class="item-input">
                                                <input type="text" placeholder="填写" v-model="user.COL_27" />
                                            </div>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                    </div>

                    <!-- 这里是页面内容区 -->
                    <div class="list-block media-list" style="margin-top: 0rem;">

                        <ul class="list-image">
                            <li v-for="car in cars">

                                <a href="#" class="item-link item-content">
                                    <div class="item-media">
                                        <img :src="car.PROD_THUMB" style='width: 4rem;height:5rem;'>
                                    </div>
                                    <div class="item-inner">
                                    <div class="item-title-row">
                                        <div class="item-title">{{car.PROD_TEXT}}</div>
                                        <div class="item-after">￥{{car.PROD_PRICE}}</div>
                                    </div>
                                    <div class="item-subtitle">{{car.PROD_INTRO}}</div>
                                    <div class="item-text">X {{car.PROD_NUM}}</div>
                                    </div>
                                </a>
                            </li>
                        </ul>

                    </div>



                </div>

            </div>
            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        obj.initVue(me);

                        //提交订单按钮事件
                        el.on("click",".btn_sumbit",function(){

                            var my_vue = obj.my_vue;

                            if (xyf_util.isNullOrWhiteSpace(my_vue.user.COL_23)) {
                                $.alert("收货人不能为空！");
                                return;
                            }

                            if (xyf_util.isNullOrWhiteSpace(my_vue.user.COL_20)) {
                                $.alert("手机号码不能为空！");
                                return;
                            }


                            if (xyf_util.isNullOrWhiteSpace(my_vue.user.COL_27)) {
                                $.alert("收货地址不能为空！");
                                return;
                            }

                            xyf_util.post("/App/InfoGrid2/Mall/Handlers/Order.ashx", "CREATE_ORDER", { address: my_vue.user.COL_27, text: my_vue.user.COL_23, tel: my_vue.user.COL_20 }, function (data) {


                                $.alert("支付成功");


                                setTimeout(function () {


                                    var url = Mini2.urlAppend("/App/InfoGrid2/Mall/View/Order/OrderContent.aspx", null, true);


                                    $.router.load(url);



                                }, 0.5 * 1000);


                                //WeixinJSBridge.invoke('getBrandWCPayRequest',data, function (res) {

                                //    my_vue.js_api_json = JSON.stringify(res);

                                //    if (res.err_msg == "get_brand_wcpay_request：ok") {
                                //        console.log("哈哈哈，支付成功了耶！");

                                //    }     // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回    ok，但并不保证它绝对可靠。 


                                //});

                            });


         

                            
                            //if (typeof WeixinJSBridge == "undefined") {
                            //    if (document.addEventListener) {
                            //        document.addEventListener('WeixinJSBridgeReady', my_vue.js_api_call, false);
                            //    }
                            //    else if (document.attachEvent) {
                            //        document.attachEvent('WeixinJSBridgeReady', my_vue.js_api_call);
                            //        document.attachEvent('onWeixinJSBridgeReady', my_vue.js_api_call);
                            //    }
                            //} else {
                            //    my_vue.js_api_call();
                            //}

                        });

                        //获取购物车选中的商品
                        xyf_util.post("/App/InfoGrid2/Mall/Handlers/Order.ashx", "GET_CHECKED_CARTS", {}, function (data) {

                            obj.my_vue.cars = data;

                            data.forEach(function (v, i) {

                                obj.my_vue.money += v.PROD_NUM * v.PROD_PRICE;

                            });

                        });

                        xyf_util.post("/App/InfoGrid2/Mall/Handlers/User.ashx", "GET_WX_USER", {}, function (data) {

                            obj.my_vue.user = data;
                        });



                    }


                    //Vue对象
                    obj.my_vue = null;


                    //初始化Vue相关对象
                    obj.initVue = function(me){

                        var el = me.el;

                        obj.my_vue = new Vue({
                            el: el.children(".vue_conter")[0],
                            data:{
                                cars:[],
                                user: {},
                                money:0,
                                js_api_json: '这是调用jsapi对象数据来的',
                                //这是不让重复支付的属性
                                pay_sid:false

                            },
                            methods:{
                                //微信支付
                                js_api_call:function(){

                                    var my_vue = this;

                                    $.alert("111111111111111");

                                    if (my_vue.pay_sid) {
                                        console.log("已经在支付中了");
                                        return;

                                    }


                                    my_vue.pay_sid = true;

                                    xyf_util.post("/App/InfoGrid2/Mall/Handlers/Order.ashx", "CREATE_ORDER", { address: my_vue.user.COL_27, text: my_vue.user.COL_23, tel: my_vue.user.COL_20 }, function (data) {


                                        $.alert("支付成功");


                                        setTimeout(function () {

                                           
                                            var url = Mini2.urlAppend("/App/InfoGrid2/Mall/View/Order/OrderContent.aspx", null, true);

                                         
                                            $.router.load(url);
                                        


                                        }, 0.5 * 1000);


                                        //WeixinJSBridge.invoke('getBrandWCPayRequest',data, function (res) {

                                        //    my_vue.js_api_json = JSON.stringify(res);

                                        //    if (res.err_msg == "get_brand_wcpay_request：ok") {
                                        //        console.log("哈哈哈，支付成功了耶！");

                                        //    }     // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回    ok，但并不保证它绝对可靠。 


                                        //});

                                    });

      
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
