<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recharge.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.Bus.Recharge" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>微信红包</title>

    <script src="/Wxhb/Script/common.js"></script>

</head>
<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title def_title">充值界面</h1>
            </header>

            <div class="content">

                <div class="list-block">
                    <ul>
                      <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">充值金额：</div>
                            <div class="item-input">
                              <input type="number" class="recharge_money">
                            </div>
                          </div>
                        </div>
                      </li>
                    </ul>
                </div>


                 <div class="row" style="margin:0px;padding:0px;">

                     <div class="col-33" style="padding-left:5px; padding-right:5px;">
                        <button class="button button-success button-fill" data-recharge="10" style="height:5.5rem;margin-bottom:1rem;width:100%;">
                            <span style="font-size:1.5rem;" data-recharge="10">10元</span>
                        </button>
                    </div>
                     <div class="col-33" style="padding-left:5px; padding-right:5px;">
                        <button class="button button-success button-fill" data-recharge="30" style="height:5.5rem;margin-bottom:1rem;width:100%;">
                            <span style="font-size:1.5rem;" data-recharge="30">30元</span>
                        </button>
                    </div>
                     <div class="col-33" style="padding-left:5px; padding-right:5px;">
                        <button class="button button-success button-fill" data-recharge="50" style="height:5.5rem;margin-bottom:1rem;width:100%;">
                            <span style="font-size:1.5rem;" data-recharge="50">50元</span>
                        </button>
                    </div>
                     <div class="col-33" style="padding-left:5px; padding-right:5px;">
                        <button class="button button-success button-fill" data-recharge="100" style="height:5.5rem;margin-bottom:1rem;width:100%;">
                            <span style="font-size:1.5rem;" data-recharge="100">100元</span>
                        </button>
                    </div>
                     <div class="col-33" style="padding-left:5px; padding-right:5px;">
                        <button class="button button-success button-fill" data-recharge="200" style="height:5.5rem;margin-bottom:1rem;width:100%;">
                            <span style="font-size:1.5rem;" data-recharge="200">200元</span>
                        </button>
                    </div>
                     <div class="col-33" style="padding-left:5px; padding-right:5px;">
                        <button class="button button-success button-fill" data-recharge="300" style="height:5.5rem;margin-bottom:1rem;width:100%;">
                            <span style="font-size:1.5rem;" data-recharge="300">300元</span>
                        </button>
                    </div>
                 </div>


                <div class="content-block">
                    <p><a href="#" class="button button-round button-fill button-big btn_recharge">充值</a></p>
                </div>

                
            </div>



            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        el.on("click", ".btn_recharge", function (e) {

                            var re_money = $(".recharge_money").val();

                            if (re_money < 1) {

                                $.toast("充值金额不能小于1元");
                                return;

                            }


                            $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'BUS_RECHARGE', bus_recharge: re_money }, function (result) {


                                if (!result.success) {

                                    $.toast(result.error_msg);

                                    return;


                                }

                                var data = result.data;

                                //这是用微信js sdk来调用的，测试来的
                                wx.chooseWXPay({
                                    timestamp: data.timeStamp, // 支付签名时间戳，注意微信jssdk中的所有使用timestamp字段均为小写。但最新版的支付后台生成签名使用的timeStamp字段名需大写其中的S字符
                                    nonceStr: data.nonceStr, // 支付签名随机串，不长于 32 位
                                    package: data.package, // 统一支付接口返回的prepay_id参数值，提交格式如：prepay_id=***）
                                    signType: 'MD5', // 签名方式，默认为'SHA1'，使用新版支付需传入'MD5'
                                    paySign: data.paySign, // 支付签名
                                    success: function (res) {

                                        alert(JSON.stringify(res));

                                        // 支付成功后的回调函数
                                    }
                                });

                            }, "json");



                            //if (typeof WeixinJSBridge == "undefined") {
                            //    if (document.addEventListener) {
                            //        document.addEventListener('WeixinJSBridgeReady', obj.jsApiCall, false);
                            //    }
                            //    else if (document.attachEvent) {
                            //        document.attachEvent('WeixinJSBridgeReady', obj.jsApiCall);
                            //        document.attachEvent('onWeixinJSBridgeReady', obj.jsApiCall);
                            //    }
                            //} else {
                            //    obj.jsApiCall();
                            //}
                        });

                       
                        el.on("click", ".col-33", function (e) {


                            var target = e.target;


                            var recharge = $(target).attr("data-recharge");


                            $(".recharge_money").val(recharge);

                        });


                    }


                    //页面加载成功事件
                    obj.onLoad = function () {

                        //获取微信js配置对象
                        $.post("/Wxhb/Handlers/UserHandler.ashx", { action: 'GET_WX_JS_CONFIG' }, function (result) {

                            if (!result.success) {

                                $.toast(result.error_msg);
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

                    //支付函数来的
                    obj.jsApiCall = function () {


                        var re_money = $(".recharge_money").val();

                        $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'BUS_RECHARGE', bus_recharge: re_money }, function (result) {


                            if (!result.success) {

                                $.toast(result.error_msg);

                                return;


                            }


                            WeixinJSBridge.invoke('getBrandWCPayRequest', json.data, function (res) {


                                var res_str = JSON.stringify(res);

                                $.alert(res_str);

                            });


                        }, "json");

                    }


                    return obj;

                }

            </script>


        </div>

    </div>


</body>
</html>
