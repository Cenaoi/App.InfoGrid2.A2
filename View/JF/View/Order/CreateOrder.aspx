<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateOrder.aspx.cs" Inherits="App.InfoGrid2.JF.View.Order.CreateOrder" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />

    <link href="/Core/Scripts/SUI/swiper-3.4.0.min.css" rel="stylesheet" />

    <title>创建订单界面</title>
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

            <div class="bar bar-footer-secondary" style="background-color: #f2f2f2; border-bottom: 1px solid #999999; padding-bottom: 0; padding-right: 0;bottom:0rem;">
                <div class="row no-gutter" style="padding: 0; margin: 0;">
                    <div class="col-66" style="padding-top: 0.5rem;">
                        
                        共
                        <span style="color: #DD2727;font-size:1rem;"><%= Num %></span>
                        件，合计：
                        <span style="color: #DD2727; font-size: 1rem; text-align: right;">￥<%= Total.ToString("0.00") %></span>
                    </div>
                    <div class="col-33" style="padding: 0px; margin: 0;">
                        <a class="button button-fill button-big button-danger btn_sumbit" style="float: right; width: 100%; margin: 0; padding: 0; top: 0; right: 0; height: 2rem; border-radius: 0px 0px;">提交订单</a>
                    </div>
                </div>
            </div>


            <div class="content" >

                <div class="list-block" style="margin-top:0px;">
                    <ul>
                            <li>
                                <div class="item-content">
                                    <div class="item-media"><i class="icon icon-form-name"></i></div>
                                    <div class="item-inner">
                                        <div class="item-title label">收货人：</div>
                                        <div class="item-input">
                                            <input type="text" placeholder="填写" v-model="user.contacter_name" />
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div class="item-content">
                                    <div class="item-media"><i class="icon icon-form-name"></i></div>
                                    <div class="item-inner">
                                        <div class="item-title label">手机号：</div>
                                        <div class="item-input">
                                            <input type="text" placeholder="填写" v-model="user.contacter_tel" />
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div class="item-content">
                                    <div class="item-media"><i class="icon icon-form-name"></i></div>
                                    <div class="item-inner">
                                        <div class="item-title label">收货地址：</div>
                                        <div class="item-input">
                                            <input type="text" placeholder="填写" v-model="user.address_t2" />
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
                            <label class="label-checkbox item-content">
                                <div class="item-media">
                                    <img :src="car.prod_thumb" width="80">
                                </div>
                                <div class="item-inner">
                                    <div class="item-title-row">
                                        <div class="item-title">{{car.prod_text}}</div>
                                        <div class="item-after" style="color: red;">￥{{car.price}}</div>
                                    </div>
                                    <div class="item-text">{{car.prod_intro}}</div>
                                    <div class="item-text">X {{car.prod_num}}</div>
                                </div>
                            </label>
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

                       
                        obj.initVue(me);

                        //提交订单按钮事件
                        el.on("click",".btn_sumbit",function(){

                            var my_vue = obj.my_vue;

                            if(my_vue.user.address_t2 === undefined || my_vue.user.address_t2 === null  || my_vue.user.address_t2.length == 0){
                                $.alert("收货地址不能为空！");
                                return;
                            }

                            if(my_vue.user.contacter_name === undefined || my_vue.user.contacter_name === null || my_vue.user.contacter_name.length == 0){
                                $.alert("收货人不能为空！");
                                return;
                            }

                            if(my_vue.user.contacter_tel === undefined || my_vue.user.contacter_tel === null || my_vue.user.contacter_tel.length == 0){
                                $.alert("手机号码不能为空！");
                                return;
                            }

                            if (typeof WeixinJSBridge == "undefined") {
                                if (document.addEventListener) {
                                    document.addEventListener('WeixinJSBridgeReady', my_vue.js_api_call, false);
                                }
                                else if (document.attachEvent) {
                                    document.attachEvent('WeixinJSBridgeReady', my_vue.js_api_call);
                                    document.attachEvent('onWeixinJSBridgeReady', my_vue.js_api_call);
                                }
                            } else {
                                my_vue.js_api_call();
                            }




                        });


                    }

                    obj.cars = <%= CarsObj %>;

                    obj.user = <%= GetUserObj() %>;


                    //Vue对象
                    obj.my_vue = null;


                    //初始化Vue相关对象
                    obj.initVue = function(me){

                        var el = me.el;


                        obj.my_vue = new Vue({
                            el:$(el).children(".content")[0],
                            data:{
                                cars:obj.cars,
                                user:obj.user,
                                js_api_json:'这是调用jsapi对象数据来的'

                            },
                            methods:{
                                //微信支付
                                js_api_call:function(){

                                    var my_vue = this;

                                    $.post("/JF/Handlers/OrderHandler.ashx",{action:'CREATE_ORDER',address:my_vue.user.address_t2,text:my_vue.user.contacter_name,tel:my_vue.user.contacter_tel},function(result){
                                        
                                        my_vue.js_api_json = JSON.stringify(result);

                                        if(!result.success){

                                            $.toast(result.msg);

                                            return;

                                        }


                                        WeixinJSBridge.invoke('getBrandWCPayRequest', result.data, function (res) {

                                            //location.href = "/JF/View/Order/OrderContent.aspx";


                                            if (res.err_msg == "get_brand_wcpay_request：ok") {
                                                console.log("哈哈哈，支付成功了耶！");

                                            }     // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回    ok，但并不保证它绝对可靠。 
                                             

                                        });



                                    },"json");

                                }

                            }
                        
                        });
                    }

                    return obj;

                }

            </script>


        </div>

    </div>


        
    <script src="/Core/Scripts/m5/M5.min.js"></script>

    <script src="/Core/Scripts/vue/vue-2.0.1.js"></script>

    <script src="/Core/Scripts/SUI/swiper-3.4.0.min.js"></script>
</body>
</html>


<script>



    $(function () {

        //FastClick.attach(document.body);

        $("body").height($(window).height());

        $.router = Mini2.create('Mini2.ui.PageRoute', {});

    });


</script>
