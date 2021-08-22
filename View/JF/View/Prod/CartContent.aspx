<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CartContent.aspx.cs" Inherits="App.InfoGrid2.JF.View.Prod.CartContent" %>

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

    <title>购物车</title>

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
                                <input type="checkbox" v-model="is_all_checkbox"  @click="all_checkbox_click($event)"  />
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
                                    <input type="checkbox" v-model="car.is_checked"  @change="oneCheckBox(car,$event)">
                                    <div class="item-media">
                                        <i class="icon icon-form-checkbox"></i>
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

                            $.confirm("删除了不能恢复的喔！",function(){


                                $.post("/JF/Handlers/ProdHandler.ashx",{action:"DELETE_CAR"},function(result){

                                    if(!result.success){

                                        $.toast(result.msg);

                                        return;

                                    }
                                        
                                    //删除过后重新获取新的购物车数据来的
                                    obj.my_vue.cars = result.data;



                                },"json");


                            });

                            
                        });

                        obj.initVue(me);

                    }

                    obj.cars = <%= GetCarsObj() %>;

                    //vue对象
                    obj.my_vue = null;

                    //初始化Vue相关对象
                    obj.initVue = function(me){

                        var el = me.el;

                        //是否全选
                        var is_all_checkbo = true;

                        //所选商品合计金额
                        var prod_total_money = 0;



                        obj.cars.forEach(function(v,i){
                            
                            if(!v.is_checked){

                                is_all_checkbo = false;

                            }

                            prod_total_money += v.price * v.prod_num;


                        });


                        obj.my_vue = new Vue({
                            el: el.children(".vue_conter")[0],
                            data: {
                                cars:obj.cars,
                                prod_total_money:prod_total_money,
                                is_all_checkbox:is_all_checkbo
                            },
                            methods:{

                                oneCheckBox:function(car,e){

                                    console.log("单选按钮事件！");

                                    var my_vue = this;

                                    var is_checked = e.target.checked;
                                    

                                    $.post("/JF/Handlers/ProdHandler.ashx",{action:"checked_car",car_id:car.car_id,is_checked:is_checked},function(result){

                                        if(!result.success){

                                            $.toast(result.msg);

                                            return;


                                        }

                                        console.log("单选回来的数据！");

                                        //删除过后重新获取新的购物车数据来的
                                        my_vue.cars = result.data;


                                    },"json");




                                },
                                //全选点击事件
                                all_checkbox_click:function(e){

                                    console.log("全选按钮事件！");

                                    var my_vue = this;

                                    var is_checked = e.target.checked;

                                    $.post("/JF/Handlers/ProdHandler.ashx",{action:'CHECKED_CAR_ALL',is_checked:is_checked},function(result){

                                        if(!result.success){

                                            $.toast(result.msg);

                                            return;

                                        }

                                        //删除过后重新获取新的购物车数据来的
                                        my_vue.cars = result.data;


                                    },"json");


                                },
                                //结算按钮点击事件
                                btnSuccess:function(){

                                    var my_vue  = this;

                                    var is_checked = false;

                                    my_vue.cars.forEach(function(v,i){
      
                                        if(v.is_checked){
                                            is_checked = true;
                                            return false;
                                        }

                                    });

                                    if(!is_checked){

                                        $.toast("请至少选中一件商品结算！");
                                        return;
                                    }


                                    var url = Mini2.urlAppend("/JF/View/Order/CreateOrder.aspx",{},true);

                                    $.router.load(url);


                                }


                            },
                            //查看属性
                            watch:{
                                //全选值
                                cars:function(){

                                    var my_vue = this;

                                    console.log("watch事件！");

                                    my_vue.prod_total_money = 0;

                                    my_vue.cars.forEach(function(v,i){

                                        if(v.is_checked){

                                            my_vue.prod_total_money += v.price * v.prod_num;

                                        }

                                    });


                                    if(my_vue.cars.length === 0){

                                        return false;
                                    }

                                    var value = true;

                                    my_vue.cars.forEach(function(v,i){

                                        if(!v.is_checked){

                                            value = false;

                                            return false;

                                        }

                                    });

                                    my_vue.is_all_checkbox = value;

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

