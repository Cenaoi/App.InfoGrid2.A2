<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderContent.aspx.cs" Inherits="App.InfoGrid2.Mall.View.Order.OrderContent" %>

<%@ Register Src="~/Mall/View/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>

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

        <uc1:Footer runat="server" id="Footer" ActionName="order" />

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <h1 class="title">订单中心</h1>
            </header>

            <uc1:Footer runat="server" id="Footer1" ActionName="order" />

            <div class="content">

                  <div class="buttons-tab">
                    <a href="#tab1" class="tab-link active button" @click="show_filter_order('all','')"  >所有订单</a>
                    <a href="#tab2" class="tab-link button" @click="show_filter_order('101','生产中')" >生产中</a>
                    <a href="#tab3" class="tab-link button" @click="show_filter_order('102','已发货')" >已发货</a>
                    <a href="#tab4" class="tab-link button" @click="show_filter_order('103','已完成')" >已完成</a>
                  </div>

                  <div class="content-block">
                      <div class="tabs">

                          <div id="tab1" class="tab active">
                                     <div class="card" v-for="(o,index) in orders"  >
                                        <div class="card-header" style="font-size:0.5rem;">

                                            订单号：
                                            <span style="font-size:1rem;">{{o.ORDER_NO}}</span>
                                            <span class="pull-right" style="right:0.5rem;color:red;">{{biz_sid_text(o)}}</span>
                                        </div>
                                        <div class="card-content">
                                          <div class="card-content-inner">
                                              <div v-html="o.ORDER_INTRO">
                              
                                              </div>
                                               <hr />
                                              <span>
                                                  合计：数量：
                                                  <span style="color:#808080;margin-right:0.4rem;">{{o.GOODS_NUM}}</span>
                                                  金额：<span style="color:red;">￥{{o.MONEY_TOTAL}}</span>
                                              </span>        
                                          </div>
                                        </div>
                                        <div class="card-footer" style="justify-content:flex-end;">
<%--                                            <button class="button button-light" @click="show_express(o)" style="margin-right:0.4rem;">发货信息</button>--%>
<%--                                            <button class="button button-light" @click="delete_order(index,o)" style="margin-right:0.4rem;">删除订单</button>--%>
                                            <button class="button button-danger" @click="pay_order(o)" v-if="o.is_pay_btn" style="margin-right:0.4rem;">付款</button>
                                        </div>
                                     </div>
                          </div>

                          <div id="tab2" class="tab">
                                     <div class="card" v-for="(o,index) in orders"  >
                                        <div class="card-header" style="font-size:0.5rem;">

                                            订单号：
                                            <span style="font-size:1rem;">{{o.ORDER_NO}}</span>
                                            <span class="pull-right" style="right:0.5rem;color:red;">{{biz_sid_text(o)}}</span>
                                        </div>
                                        <div class="card-content">
                                          <div class="card-content-inner">
                                              <div v-html="o.ORDER_INTRO">
                              
                                              </div>
                                               <hr />
                                              <span>
                                                  合计：数量：
                                                  <span style="color:#808080;margin-right:0.4rem;">{{o.GOODS_NUM}}</span>
                                                  金额：<span style="color:red;">￥{{o.MONEY_TOTAL}}</span>
                                              </span>        
                                          </div>
                                        </div>
                                        <div class="card-footer" style="justify-content:flex-end;">
<%--                                            <button class="button button-light" @click="show_express(o)" style="margin-right:0.4rem;">发货信息</button>--%>
<%--                                            <button class="button button-light" @click="delete_order(index,o)" style="margin-right:0.4rem;">删除订单</button>--%>
                                            <button class="button button-danger" @click="pay_order(o)" v-if="o.is_pay_btn" style="margin-right:0.4rem;">付款</button>
                                        </div>
                                     </div>
                          </div>

                          <div id="tab3" class="tab">

                                     <div class="card" v-for="(o,index) in orders"  >
                                        <div class="card-header" style="font-size:0.5rem;">

                                            订单号：
                                            <span style="font-size:1rem;">{{o.ORDER_NO}}</span>
                                            <span class="pull-right" style="right:0.5rem;color:red;">{{biz_sid_text(o)}}</span>
                                        </div>
                                        <div class="card-content">
                                          <div class="card-content-inner">
                                              <div v-html="o.ORDER_INTRO">
                              
                                              </div>
                                               <hr />
                                              <span>
                                                  合计：数量：
                                                  <span style="color:#808080;margin-right:0.4rem;">{{o.GOODS_NUM}}</span>
                                                  金额：<span style="color:red;">￥{{o.MONEY_TOTAL}}</span>
                                              </span>        
                                          </div>
                                        </div>
                                        <div class="card-footer" style="justify-content:flex-end;">
<%--                                            <button class="button button-light" @click="show_express(o)" style="margin-right:0.4rem;">发货信息</button>--%>
<%--                                            <button class="button button-light" @click="delete_order(index,o)" style="margin-right:0.4rem;">删除订单</button>--%>
                                            <button class="button button-danger" @click="pay_order(o)" v-if="o.is_pay_btn" style="margin-right:0.4rem;">付款</button>
                                        </div>
                                     </div>
                          </div>

                          <div id="tab4" class="tab">
                                     <div class="card" v-for="(o,index) in orders"  >
                                        <div class="card-header" style="font-size:0.5rem;">

                                            订单号：
                                            <span style="font-size:1rem;">{{o.ORDER_NO}}</span>
                                            <span class="pull-right" style="right:0.5rem;color:red;">{{biz_sid_text(o)}}</span>
                                        </div>
                                        <div class="card-content">
                                          <div class="card-content-inner">
                                              <div v-html="o.ORDER_INTRO">
                              
                                              </div>
                                               <hr />
                                              <span>
                                                  合计：数量：
                                                  <span style="color:#808080;margin-right:0.4rem;">{{o.GOODS_NUM}}</span>
                                                  金额：<span style="color:red;">￥{{o.MONEY_TOTAL}}</span>
                                              </span>        
                                          </div>
                                        </div>
                                        <div class="card-footer" style="justify-content:flex-end;">
<%--                                            <button class="button button-light" @click="show_express(o)" style="margin-right:0.4rem;">发货信息</button>--%>
<%--                                            <button class="button button-light" @click="delete_order(index,o)" style="margin-right:0.4rem;">删除订单</button>--%>
                                            <button class="button button-danger" @click="pay_order(o)" v-if="o.is_pay_btn" style="margin-right:0.4rem;">付款</button>
                                        </div>
                                     </div>




                          </div>

                      </div>

                  </div>



                <!-- 这是没有数据时显示的 -->
                <div class="text-center" v-if="orders.length === 0">
                    <h3>您还没有挑选商品</h3>
                </div>

            <div class="content-block" v-if="orders.length > 0">
                <p><a href="#" class="button button-round button-fill" @click="show_filter_order2(tab_code,tab_name)">加载更多</a></p>
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

                        obj.my_vue.load_more();

                    }

                    //Vue对象
                    obj.my_vue = null;

                    //初始化Vue相关对象
                    obj.initVue = function (me) {

                        var el = me.el;

                        obj.my_vue = new Vue({
                            el: el.children(".content")[0],
                            data: {
                                orders: [],
                                page_index: 0,
                                page_size: 10,
                                tab_name: '',
                                tab_code:'all'
                            },
                            methods: {
                                //删除订单按钮事件
                                delete_order: function (index, order) {

                                    var my_vue = this;


                                    $.confirm("是否真的要删除订单，删除不能恢复的，请慎重！", function () {

                                        xyf_util.post("/App/InfoGrid2/Mall/Handlers/Order.ashx", "DELETE_ORDER", { order_id: order.MALL_ORDER_ID }, function (data) {

                                            my_vue.orders.splice(index, 1);

                                            console.log(my_vue.orders);


                                            $.toast("删除订单成功！");

                                        });

                                    });


                                },
                                //根据订单状态来显示相应的文字
                                biz_sid_text: function (order) {

                                    var my_vue = this;

                                    //未付款
                                    if (order.BIZ_SID === 0) {

                                        //付款按钮要显示出来
                                        order.is_pay_btn = true;

                                        return "等待付款";

                                    } else {

                                        return "已确定";

                                    }


                                },
                                //重新付款
                                pay_order: function (order) {

                                    var my_vue = this;


                                    if (order.BIZ_SID !== 0) {

                                        $.toast("只有等待付款才能重新付款");

                                        return;

                                    }


                                    xyf_util.post("/App/InfoGrid2/Mall/Handlers/Order.ashx", "PAY_ORDER", { order_id: order.order_id }, function (data) {

                                        WeixinJSBridge.invoke('getBrandWCPayRequest', data, function (res) {

                                            //location.href = "/JF/View/Order/OrderContent.aspx";

                                            if (res.err_msg == "get_brand_wcpay_request：ok") {
                                                console.log("哈哈哈，支付成功了耶！");

                                            }     // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回    ok，但并不保证它绝对可靠。 

                                        });

                                    });




                                },
                                //加载更多
                                load_more: function () {

                                    var my_vue = this;


                                    xyf_util.post("/App/InfoGrid2/Mall/Handlers/Order.ashx", "GET_ORDERS", { page_index: my_vue.page_index, page_size: my_vue.page_size }, function (data) {

                                        if (data.length === 0) {

                                            $.toast("没有更多数据了");

                                            return;
                                        }



                                        my_vue.orders = my_vue.orders.concat(data);

                                        my_vue.page_index++;

                                    });
                                },
                                //显示发货信息
                                show_express: function (T) {

                                    var my_vue = this;


                                    var url = Mini2.urlAppend("/App/InfoGrid2/Mall/View/Order/OrderExpress.aspx", { order_id: T.MALL_ORDER_ID }, true);


                                    $.router.load(url);

                                },
                                //获取订单数据(订单类型过滤)
                                get_order_byfilter: function (filter_param,param_name,page_index, page_size) {

                                    var my_vue = this;

                                    xyf_util.post("/App/InfoGrid2/Mall/Handlers/Order.ashx", "GET_ORDER_BY_FILTER", { filter_param: filter_param, page_index: page_index, page_size: page_size }, function (data) {


                                        if (data.length === 0) {

                                            $.toast("没有更多" + param_name + "的订单");

                                            return;
                                        }


                                        my_vue.orders = my_vue.orders.concat(data);

                                        my_vue.page_index++;


                                    });

                                },
                                //点击订单类型
                                show_filter_order: function (filter_param, param_name) {

                                    console.log("12222222222222");

                                    var my_vue = this;

                                    my_vue.page_index = 0;

                                    my_vue.orders = [];

                                    my_vue.tab_name = param_name;

                                    my_vue.tab_code = filter_param;

                                    my_vue.get_order_byfilter(filter_param,param_name,my_vue.page_index, my_vue.page_size);

                                },
                                //点击加载更多(订单类型过滤)
                                show_filter_order2: function () {

                                    console.log("12222222222222");

                                    var my_vue = this;

                                    console.log(my_vue.tab_code, "类型编码");

                                    my_vue.get_order_byfilter(my_vue.tab_code, my_vue.tab_name,my_vue.page_index, my_vue.page_size);

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
