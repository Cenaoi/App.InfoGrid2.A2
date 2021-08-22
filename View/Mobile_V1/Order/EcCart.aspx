<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EcCart.aspx.cs" Inherits="App.InfoGrid2.Mobile_V1.Order.EcCart" %>

<%@ Register Src="~/Mobile_V1/SiteFooter.ascx" TagPrefix="uc1" TagName="SiteFooter" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>购物车</title>

    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />

    <style>

         .list-block .item-inner:after{
                background:none;
            }

            .list-image li{
                border-bottom:1px solid #DDDDDD;
            }

    </style>

    
</head>
<body>



     <div class="page-group">


        <div class="page" >

            <!-- 底部按钮 -->
            <uc1:SiteFooter runat="server" ID="SiteFooter" ActionName="cart" />

              <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class='title'>购物车</h1>
                <a class=" button button-danger pull-right" >删除</a>
            </header>

            <div class="vue_conter">

              

                
                <div class="bar bar-footer-secondary" style="background-color:#f2f2f2; border-bottom:1px solid #999999;padding-bottom:0;padding-right:0;">
                    <div class="row no-gutter" style="padding:0;margin:0;">
                        <div class="col-33" style="padding-top:0.5rem;">
                            <label class="label-checkbox item-content" >
                                <input type="checkbox" name="my-radio" checked v-model="is_all_checkbox"  @change="all_checkbox_click" />
                                <div class="item-media"><i class="icon icon-form-checkbox"></i>全选</div>
                            </label>
                        </div>
                        <div class="col-33" >
                            <span style="color:#DD2727;font-size:10pt; text-align:right;">合计：￥{{prod_total_money}}</span>
                        </div>
                        <div class="col-33" style="padding:0px;margin:0; ">
                            <a class="button button-fill button-big button-danger" style="float:right; width:100%;margin:0;padding:0;top:0;right:0;height:2rem;border-radius:0px 0px;"  @click="btnSuccess" >结算</a>
                        </div>
                    </div>
                </div>


                <div class="content">
       
                    <!-- 这里是页面内容区 -->
                    <div class="list-block media-list">

                        <ul class="list-image">
                            <li v-for="esc in esc_list">
                                <label class="label-checkbox item-content"  >
                                    <input type="checkbox" name="my-radio" v-model="esc.is_checkbox"  @change="oneCheckBox(esc,$event)">
                                    <div class="item-media">
                                        <i class="icon icon-form-checkbox"></i>
                                        <img src="/mobile/res/image/demo/PROD_001.jpg" width="80">
                                    </div>
                                    <div class="item-inner">
                                        <div class="item-title-row">
                                            <div class="item-title">{{esc.PROD_NAME}}</div>
                                            <div class="item-after" style="color: red;">￥{{esc.SUB_TOTAL_MONEY}}</div>
                                        </div>
                                        <div class="item-text">{{esc.PROD_INTRO}}</div>
                                        <div class="item-text">X {{esc.PROD_NUM}}</div>
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

                        
                        el.on("click",".button-danger",function(e){

                            var my_vue = obj.myVue;

                            var cart_id_list= [];

                            my_vue.esc_list.forEach(function(v,i){

                                if(v.is_checkbox){
                                    cart_id_list.push(v.ES_SHOPPING_CART_ID);
                                }



                            });

                            if(cart_id_list.length === 0){

                                $.toast("没有要删除的商品！");

                                return;

                            }

                            $.post("/Handlers/EcCartHandler.ashx",{action:"DELETE_CARTS",cart_ids:cart_ids},function(result){
                            

                                var json = JSON.parse(result);

                                
                                if(!json.success){

                                    $.toast(json.msg);

                                    return;

                                }


                                my_vue.esc_list = json.data.esc_list;

                                my_vue.is_all_checkbox = false;

                                //选中的商品的总计金额
                                my_vue.prod_total_money = 0;


                            });



                        });

                        
                        obj.initVue(me);
        

                    }


                    //Vue对象
                    obj.myVue = null;


                    //初始化vue相关的对象
                    obj.initVue = function (me) {

                        var el = me.el;

                         var cart_list = <%= GetShoppingCartObj() %>;

                        cart_list.is_all_checkbox = false;

                        //选中的商品的总计金额
                        cart_list.prod_total_money = 0;


                        obj.myVue = new Vue({
                            el: $(el).children(".vue_conter")[0],
                            data:cart_list,
                            methods:{
                                //单选按钮的点击事件 
                                oneCheckBox:function(esc,event){
                        
                                    var me = this;

                                    var checkboxEl = event.target;

                                    var flag = checkboxEl.checked;

                                    esc.is_checkbox = flag;


                                    if(flag){

                                        for(var i in me.esc_list){

                                            var item = me.esc_list[i];

                                            if(item.is_checkbox){
                                                 continue;
                                            }

                                            flag = false;
                                            break;
                                        }

                                    }

                                   me.computedTotalMoney();

                                    me.is_all_checkbox = flag;

                                },
                                //把选中的商品放入新订单中
                                btnSuccess:function(){
                        
                                    var me = this;

                                    var cart_id_list= [];

                                    for(var i in me.esc_list){
                                        var data = me.esc_list[i];
                                        if(data.is_checkbox){
                                            cart_id_list.push(data.ES_SHOPPING_CART_ID);
                                        }
                                    }

                                    if(cart_id_list.length ==0){
                                        $.alert("请选择商品！");
                                        return;
                                    }

                                    console.log(cart_id_list);

                                    var cart_ids = cart_id_list.join(",");

                                    console.log(cart_ids);

                                    $.post("/Handlers/EcCartHandler.ashx",{action:"CREATE_ORDER",cart_ids:cart_ids},function(result){
                            
                                        var json = null;

                                            json = JSON.parse(result);

                                      
                                        if(!json.success){
                                            $.alert(json.msg);
                                            return;
                                        }

                                        $.toast("下单完成了！");


                                        //$.router.load("/Mobile_V1/Order/EcCreateOrder.aspx?cart_ids="+cart_ids+"&order_id="+json.data.order_id,true);

                                    });
                                },
                                //全选按钮点击事件
                                all_checkbox_click:function(event){
                        
                                    var checkboxEl =event.target;

                                    console.log("全选按钮");

                                    console.log(checkboxEl);


                                    if(checkboxEl === undefined){
                                        return;
                                    }

                                    var flag = checkboxEl.checked;

                                    for(var i in this.esc_list){

                                        var esc = this.esc_list[i];

                                        esc.is_checkbox = flag;

                                    }

                                    me.computedTotalMoney();

                                },

                                //计算选中的商品的合计金额
                                computedTotalMoney:function(){

                                    var me = this;

                                    me.prod_total_money = 0;

                                    for(var i in me.esc_list){
                
                                        var item = me.esc_list[i];

                                        if(item.is_checkbox){
                    
                                            me.prod_total_money += item.SUB_TOTAL_MONEY;
                                        }

                                    }



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


</body>
</html>


<script>



    $(function () {

        //FastClick.attach(document.body);

        $("body").height($(window).height());

        $.router = Mini2.create('Mini2.ui.PageRoute', {});

    });


</script>


