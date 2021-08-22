<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CartContent.aspx.cs" Inherits="App.InfoGrid2.Mall.View.Prod.CartContent" %>

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

    <style>

    .port_bc{

            background-color:#ffc107;
            color:white;

        }

    </style>


</head>

<body>



      <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <h1 class="title">购物车</h1>
                <a class=" button button-danger pull-right btn_delete" >删除</a>
            </header>

            <uc1:Footer runat="server" id="Footer" ActionName="cart" />


            <div class="vue_conter">

              

                
                <div class="bar bar-footer-secondary" style="background-color:#f2f2f2; border-bottom:1px solid #999999;padding-bottom:0;padding-right:0;">
                    <div class="row no-gutter" style="padding:0;margin:0;">
                        <div class="col-33" style="padding-top:0.5rem;">
                            <label class="label-checkbox item-content" >
                                <input type="checkbox" v-model="is_all_checkbox"  @change="all_checkbox_click"  />
                                <div class="item-media"><i class="icon icon-form-checkbox"></i>全选</div>
                            </label>
                        </div>
                        <div class="col-33">
                            <span style="color:#DD2727;font-size:10pt; text-align:right;">合计：￥{{prod_total_money}}</span>
                        </div>
                        <div class="col-33" style="padding:0px;margin:0; ">
                            <a class="button button-fill button-big button-danger"  style="float:right; width:100%;margin:0;padding:0;top:0;right:0;height:2rem;border-radius:0px 0px;" @click="btnSuccess" >结算</a>
                        </div>
                    </div>
                </div>


                <div class="content" style="top:2.2rem;">
       
                    <!-- 这里是页面内容区 -->
                    <div class="list-block media-list" v-if="cars.length !== 0" style="margin-top:0px;">
                        <ul class="list-image">
                            <li v-for="car in cars">
                                <label class="label-checkbox item-content"  >
                                    <input type="checkbox" v-model="car.IS_CHECKED"  @change="oneCheckBox(car)">
                                    <div class="item-media">
                                        <i class="icon icon-form-checkbox"></i>
                                        <img :src="car.PROD_THUMB" style="width:4rem;height:5rem;">
                                    </div>
                                    <div class="item-inner">
                                        <div class="item-title-row">
                                            <div class="item-title">{{car.PROD_TEXT}}</div>
                                            <div class="item-after" style="color: red;">￥{{car.PROD_PRICE}}</div>
                                        </div>
                                        <div class="item-text">{{car.PROD_INTRO}}</div>
                                        <div class="item-text">X {{car.PROD_NUM}}</div>
                                    </div>
                                </label>
                            </li>
                        </ul>
                    </div>
                    <!-- 这是没有数据时显示的 -->
                     <div class="text-center" v-if="cars.length === 0">
                            <h3>您还没有挑选商品</h3>
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

                        //删除按钮点击事件
                        el.on("click",".btn_delete",function(){

                            $.confirm("打钩的都会删除，删除了不能恢复的喔！",function(){


                                var is_checked = false;

                                obj.my_vue.cars.forEach(function (v, i) {

                                    if (v.IS_CHECKED) {
                                        is_checked = true;
                                        return false;
                                    }

                                });

                                if (!is_checked) {

                                    $.toast("请至少选中一件商品删除！");
                                    return;
                                }


                                xyf_util.post("/App/InfoGrid2/Mall/Handlers/Prod.ashx", "DELETE_CART_ITEM", {}, function (data) {

                                    obj.my_vue.cars = data;

                                    obj.my_vue.change_all_check();

                                });


                            });

                            
                        });

                        obj.initVue(me);


                        xyf_util.post("/App/InfoGrid2/Mall/Handlers/Prod.ashx", "GET_CARTS", {}, function (data) {

                            obj.my_vue.cars = data;

                            obj.my_vue.change_all_check();

                        });


                    }

                    //vue对象
                    obj.my_vue = null;

                    //初始化Vue相关对象
                    obj.initVue = function(me){

                        var el = me.el;

                        obj.my_vue = new Vue({
                            el: el.children(".vue_conter")[0],
                            data: {
                                cars:[],
                                prod_total_money:0,
                                is_all_checkbox:true
                            },
                            methods:{

                                oneCheckBox:function(car){

                                    var my_vue = this;

                                    //单选事件
                                    xyf_util.post("/App/InfoGrid2/Mall/Handlers/Prod.ashx", "CHECKED_CART_ITEM", { cart_id: car.MALL_S_CART_ID, is_checked: car.IS_CHECKED }, function (data) {

                                        my_vue.cars = data;

                                        my_vue.change_all_check();


                                    });



                                },
                                //全选点击事件
                                all_checkbox_click:function(e){
                                    var my_vue = this;

                                    var is_checked = my_vue.is_all_checkbox;

                                    xyf_util.post("/App/InfoGrid2/Mall/Handlers/Prod.ashx", "CHECKED_CART_ALL", { is_checked: is_checked }, function (data) {

                                        my_vue.cars = data;

                                        my_vue.change_all_check();

                                    });


                                },
                                //结算按钮点击事件
                                btnSuccess:function(){

                                    var my_vue  = this;

                                    var is_checked = false;

                                    my_vue.cars.forEach(function(v,i){
      
                                        if (v.IS_CHECKED) {
                                            is_checked = true;
                                            return false;
                                        }

                                    });

                                    if(!is_checked){

                                        $.toast("请至少选中一件商品结算！");
                                        return;
                                    }


                                    var url = Mini2.urlAppend("/App/InfoGrid2/Mall/View/Order/CreateOrder.aspx", {}, true);

                                    $.router.load(url);    

                                },
                                //改变全选按钮值
                                change_all_check: function () {

                                    var me = this;

                                    var money = 0;

                                    me.is_all_checkbox = true;

                                    me.cars.forEach(function (v, i) {
                                        //只要有一个没中选，全选按钮就为false
                                        if (!v.IS_CHECKED) {
                                            me.is_all_checkbox = false;

                                        } else {

                                            money += v.PROD_NUM * v.PROD_PRICE;

                                        }

                                    });

                                    me.prod_total_money = money;

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

