<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProdDeta.aspx.cs" Inherits="App.InfoGrid2.JF.View.Prod.ProdDeta" %>

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

    <title>商品详情</title>

</head>
<body>


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <a class="icon icon-cart pull-right external" style="font-size:1.5rem;" href="/JF/View/Prod/CartContent.aspx">
                    <span class="badge" style="display: <%= CarBadgeShow %>; position: absolute;top: .1rem;left: 50%;z-index: 100; height: .8rem;min-width: .8rem;padding: 0 .3rem;font-size: .6rem;line-height: .8rem;color: white;vertical-align: top;background: red;border-radius: .5rem;margin-left: .1rem;">
                        <%=CarBadgeText %>
                    </span>
                </a>
            </header>

            <!-- 底部按钮 -->
            <nav class="bar bar-tab">
                <a class="tab-item external" href="/JF/View/Home.aspx">
                    <span class="icon icon-home"></span>
                    <span class="tab-label">首页</span>
                </a>
                <a class="tab-item push_cart" href="#" style="background-color: #FF9100; color: #ffffff; " >
                    <span class="tab-label">加入购物车</span>
                </a>
            </nav>

            <div class="content">

                <!-- 商品详情-->
                <div class="swiper-container" style="padding-top: 4px;">
                    <div class="swiper-wrapper">
                        <div class="swiper-slide" v-for="i in prod.imgs">
                            <img :src="i.url" alt="" style="width: 100%;">
                        </div>
                    </div>
                    <div class="swiper-pagination"></div>
                </div>
                <div class="" style="background-color: white; padding: 1rem;">
                    <div style="margin-top: 0; margin-bottom: 0.5rem; padding: 0px; font-size: 1rem;">
                        {{prod.prod_name}}
                    </div>
                    <div style="color: #DD2727;">
                        <span style="color: #999999;">会员价&nbsp;</span>
                        <span style="font-size: 1rem;">¥</span>
                        <span style="font-size: 1.5rem;">{{prod.price}}</span>
                    </div>
                    <div style="color: #999999; font-size: 1rem;">
                        <span style="font-size: 12pt;">市场价&nbsp;</span>
                        <span style="text-decoration: line-through;"> ¥{{prod.price_market}}</span>
                    </div>
                </div>
                <div class="list-block" style="margin: 0.8rem 0;">
                    <ul>
                        <li class="item-content item-link open_spec_modal">
                            <div class="item-inner">
                                <div class="item-title">选择规格</div>
                            </div>
                        </li>
                    </ul>
                </div>
                    
                <div class="content-block" style="width:100%;padding:0px;">

                    <div class="text-center">
                        <h4>商品详情</h4>
                    </div>

                    <div v-html="prod.prod_explain"></div>
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


                        //加入购物车点击事件
                        el.on("click",".open_spec_modal",function(e){

                            var url = Mini2.urlAppend("/JF/View/Prod/ProdSpec.aspx",{prod_id:obj.prod_obj.id},true);

                            $.router.load(url);

                        });

                        //加入购物车 没有其它规格就默认添加自身
                        el.on("click",".push_cart",function(e){

                            $.post("/JF/Handlers/ProdHandler.ashx",{action:"PUSH_CAR_BTN",prod_id:obj.prod_obj.id,num:1},function(result){
                            
                                if(!result.success){

                                    $.toast(result.msg);

                                    return;

                                }


                                //直接加入购物车成功
                                if(result.data.is_ok){
                                    
                                    $.toast("加入购物成功！");

                                    return ;

                                }

                                //要显示选择规格界面

                                var url = Mini2.urlAppend("/JF/View/Prod/ProdSpec.aspx",{prod_id:obj.prod_obj.id},true);

                                $.router.load(url);



                            
                            },"json");



                        })



                        setTimeout(function () {

                            var mySwiper = new Swiper('.swiper-container', {
                                pagination: '.swiper-pagination',
                                autoplay: 5000,//可选选项，自动滑动
                            });


                        },0.5 * 1000);


                    }

                    //后台获取的商品对象
                    obj.prod_obj =<%= ProdObj %>;

                    //vue对象
                    obj.my_vue = null;


                    //初始化Vue相关的对象
                    obj.initVue = function(me){

                        var el = me.el;


                        obj.my_vue = new Vue({
                            el:el.children(".content")[0],
                            data:{
                                prod:obj.prod_obj
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


        $("body").width($(window).width());


        $.router = Mini2.create('Mini2.ui.PageRoute', {});

    });


</script>

