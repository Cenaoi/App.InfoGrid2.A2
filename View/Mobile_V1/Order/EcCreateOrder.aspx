<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EcCreateOrder.aspx.cs" Inherits="App.InfoGrid2.Mobile_V1.Order.EcCreateOrder" %>


<%@ Register Src="~/Mobile_V1/SiteFooter.ascx" TagPrefix="uc1" TagName="SiteFooter" %>



<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>确定订单界面</title>

    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />

    <style>
        .list-block .item-inner:after {
            background: none;
        }

        .list-image li {
            border-bottom: 1px solid #DDDDDD;
        }
    </style>

</head>
<body>


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">


                        <!-- 底部按钮 -->
            <uc1:SiteFooter runat="server" ID="SiteFooter" ActionName="cart" />

            <div class="vue_conter">
                
                <div class="bar bar-footer-secondary" style="background-color: #f2f2f2; border-bottom: 1px solid #999999; padding-bottom: 0; padding-right: 0;">
                    <div class="row no-gutter" style="padding: 0; margin: 0;">
                        <div class="col-33" style="padding-top: 0.5rem;">
                        </div>
                        <div class="col-33">
                            <span style="color: #DD2727; font-size: 10pt; text-align: right;">合计：￥{{order_total_money}}</span>
                        </div>
                        <div class="col-33" style="padding: 0px; margin: 0;">
                            <a class="button button-fill button-big button-danger" style="float: right; width: 100%; margin: 0; padding: 0; top: 0; right: 0; height: 2rem; border-radius: 0px 0px;" @click="btnSuccess()">确定</a>
                        </div>
                    </div>
                </div>


                <div class="content" >

                     <div class="list-block">
                            <ul>
                                <!-- Text inputs -->
                                <li>
                                    <div class="item-content">
                                        <div class="item-media"><i class="icon icon-form-name"></i></div>
                                        <div class="item-inner">
                                            <div class="item-title label">收货人：</div>
                                            <div class="item-input">
                                                <input type="text" placeholder="填写" v-model="CONTACTER_NAME" />
                                            </div>
                                        </div>
                                    </div>
                                </li>
                                <!-- Text inputs -->
                                <li>
                                    <div class="item-content">
                                        <div class="item-media"><i class="icon icon-form-name"></i></div>
                                        <div class="item-inner">
                                            <div class="item-title label">收货地址：</div>
                                            <div class="item-input">
                                                <input type="text" placeholder="填写" v-model="ADDRESS_T2" />
                                            </div>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </div>

                    <!-- 这里是页面内容区 -->
                    <div class="list-block media-list" style="margin-top: 0rem;">

                        <ul class="list-image">
                            <li v-for="item in order_item_list">
                                <label class="label-checkbox item-content">
                                    <div class="item-media">
                                        <img src="res/image/demo/PROD_001.jpg" width="80">
                                    </div>
                                    <div class="item-inner">
                                        <div class="item-title-row">
                                            <div class="item-title">{{item.PROD_NAME}}</div>
                                            <div class="item-after" style="color: red;">￥{{item.SUB_TOTAL_MONEY}}</div>
                                        </div>
                                        <div class="item-text">{{item.PROD_INTRO}}</div>
                                        <div class="item-text">X {{item.PROD_NUM}}</div>
                                    </div>
                                </label>
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
        

                    }

                    //Vue对象
                    obj.myVue = null;


                    //初始化vue相关的对象
                    obj.initVue = function (me) {

                        var el = me.el;



                        obj.myVue = new Vue({
                            el: el.children(".vue_conter")[0],


                        });


                    }


                    return obj;

                }

            </script>


        </div>

    </div>


    <script src="/Core/Scripts/m5/M5.min.js"></script>

    <script src="/Core/Scripts/vue/vue-2.0.1.js"></script>


</body>
</html>


<script>



    $(function () {

        //FastClick.attach(document.body);

        $("body").height($(window).height());

        $.router = Mini2.create('Mini2.ui.PageRoute', {});

    });


</script>
