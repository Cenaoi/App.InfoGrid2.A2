<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderExpress.aspx.cs" Inherits="App.InfoGrid2.Mall.View.Order.OrderExpress" %>

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
                <h1 class="title">发货信息</h1>
            </header>

            <div class="content">
<%--                 <div class="card">
                    <div class="card-header">未发货明细</div>
                    <div class="card-content">
                      <div class="card-content-inner">
                          <div>A产品 红色-L  数量：120</div>
                          <div>A产品 红色-M  数量：150</div>
                      </div>
                    </div>
                    <div class="card-footer">合计：<span style="color:red;">270</span></div>
                 </div>--%>


                <div class="card" v-if="ut_091s.length > 0">
                    <div class="card-header">未发货明细</div>
                    <div class="card-content">
                      <div class="card-content-inner">
                          <div v-for="T in ut_091s">{{T.prod_text}} {{T.color_text}}-{{T.size_text}}  数量：{{T.size_num}}</div>
                      </div>
                    </div>
                    <div class="card-footer">合计：<span style="color:red;">{{ut_091_total}}</span></div>
                 </div>


                 <div class="content-block-title" v-if="ut_097s.length > 0">发货记录</div>

                 <div class="card" v-for="T in ut_097s">
                    <div class="card-header">
                        <div>物流单号：{{T.express_no}}</div>
                    </div>
                    <div class="card-content">
                         <div class="card-content-inner">
                            <div>时间：{{T.express_date}}</div>
                            <div>物流公司：{{T.express_text}}</div>
                        </div>
                      <div class="card-content-inner">
                          <div v-for="ST in T.items">{{T.prod_text}} {{T.color_text}}-{{T.size_text}}  数量：{{T.size_num}}</div>
                      </div>
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

                        
                        var order_id = xyf_util.getQuery(me.url, "order_id", true);

                        //初始化vue
                        obj.initVue(me);

                        xyf_util.post("/App/InfoGrid2/Mall/Handlers/Order.aspx", "GET_ORDER_EXPRESS", { order_id: order_id }, function (data) {

                            obj.my_vue.ut_091s = data.ut_091s;
                            obj.my_vue.ut_097s = data.ut_097s;

                        });

                    }


                    //Vue对象
                    obj.my_vue = null;

                    //初始化Vue相关对象
                    obj.initVue = function (me) {

                        var el = me.el;

                        obj.my_vue = new Vue({
                            el: el.children(".content")[0],
                            data: {
                                ut_091s: [],
                                ut_097s:[]
                            },
                            methods: {

                            },
                            //计算属性
                            computed: {
                                //未发数量合计
                                ut_091_total: function () {

                                    var my_vue = this;

                                    var total = 0;

                                    my_vue.ut_091s.forEach(function (v, i) {

                                        total += v.size_num;
                                        
                                    });

                                    
                                    return total;

                                }
                            }

                        });

                    }



                    //页面恢复的事件
                    obj.pageRevert = function () {
		                        var me = this,
                            	    el = me.el;
                    }

                    return obj;

                }

            </script>

        </div>

    </div>
</body>
</html>
