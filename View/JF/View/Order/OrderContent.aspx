<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderContent.aspx.cs" Inherits="App.InfoGrid2.JF.View.Order.OrderContent" %>


<%@ Register Src="~/JF/View/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>

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

    <title>订单中心</title>

</head>
<body>


      <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <h1 class="title">订单中心</h1>
            </header>

            <uc1:Footer runat="server" id="Footer1" ActionName="order" />

            <div class="content">

                 <div class="card" v-for="(o,index) in orders">
                    <div class="card-header" style="font-size:0.5rem;">

                        订单号：
                        <span style="font-size:1rem;">{{o.order_code}}</span>
                        <span class="pull-right" style="right:0.5rem;color:red;">{{biz_sid_text(o)}}</span>
                    </div>
                    <div class="card-content">
                      <div class="card-content-inner">
                          <div>
                              {{o.order_intro}}
                          </div>
                           <hr />
                          合计：<span style="color:red;">￥{{o.money_total}}</span>
                      </div>
                    </div>
                    <div class="card-footer" style="justify-content:flex-end;">
                        <button class="button button-light" @click="delete_order(index,o)" style="margin-right:0.4rem;">删除订单</button>
                        <button class="button button-danger" @click="pay_order(o)" v-if="o.is_pay_btn" style="margin-right:0.4rem;">付款</button>
                    </div>
                  </div>

                <!-- 这是没有数据时显示的 -->
                <div class="text-center" v-if="orders.length === 0">
                    <h3>您还没有挑选商品</h3>
                </div>

            </div>


            <uc1:Footer runat="server" id="Footer" ActionName="order" />

            <script type="text/javascript" data-main="true">
        
                function main() {
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        obj.initVue(me);

                    }

                    obj.orders = <%= GetOrdersObj() %>;

                    //Vue对象
                    obj.my_vue = null;

                    //初始化Vue相关对象
                    obj.initVue = function(me) {

                        var el = me.el;

                        obj.my_vue = new Vue({
                            el:el.children(".content")[0],
                            data:{
                                orders:obj.orders
                            },
                            methods:{
                                //删除订单按钮事件
                                delete_order:function(index,order){

                                    var my_vue = this;


                                    $.confirm("是否真的要删除订单，删除不能恢复的，请慎重！",function(){

                                        $.post("/JF/Handlers/OrderHandler.ashx",{action:"DELETE_ORDER",order_id:order.order_id},function(result){

                                            if(!result.success){

                                                $.toast(result.msg);

                                                return;

                                            }

                                            my_vue.orders.splice(index,1);

                                            $.toast("删除订单成功！");



                                        },"json");


                                       




                                    });





                                },
                                //根据订单状态来显示相应的文字
                                biz_sid_text :function(order){

                                    console.log("进入biz_sid_text 函数这里面了！");

                                    console.log(order);


                                    var my_vue = this;

                                    //未付款
                                    if(order.pay_sid === 0){

                                        //付款按钮要显示出来
                                        order.is_pay_btn = true;

                                        return "等待付款";

                                    }

                                    //待发货
                                    if(order.del_sid === 0){
                                        return "已付款";
                                    }

                                    //卖家已发货
                                    if(order.rec_sid === 0){

                                        return "卖家已发货";

                                    }

                                    //订单完成
                                    if(order.biz_sid === 999){
                                        return "交易成功";
                                    }
                                  
                                  

                                },
                                //重新付款
                                pay_order:function(order){

                                    var my_vue = this;


                                    $.post("/JF/Handlers/OrderHandler.ashx",{action:'PAY_ORDER',order_id:order.order_id},function(result){


                                        if(!result.success){

                                            $.toast(result.msg);

                                            return;

                                        }


                                        WeixinJSBridge.invoke('getBrandWCPayRequest', result.data, function (res) {

                                            //location.href = "/JF/View/Order/OrderContent.aspx";

                                            if (res.err_msg == "get_brand_wcpay_request：ok") {
                                                console.log("哈哈哈，支付成功了耶！");

                                            }     // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回    ok，但并不保证它绝对可靠。 
                                            
                                            alert(JSON.stringify(res));

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
