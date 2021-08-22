<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProdDeta.aspx.cs" Inherits="App.InfoGrid2.Mall.View.Prod.ProdDeta" %>

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

            <!-- 底部按钮 -->
            <nav class="bar bar-tab">
                <a class="tab-item external" href="/Mall/View/Home.aspx">
                    <span class="icon icon-home"></span>
                    <span class="tab-label">首页</span>
                </a>
                <a class="tab-item push_cart" href="#" style="background-color: #FF9100; color: #ffffff; " >
                    <span class="tab-label">加入购物车</span>
                </a>
            </nav>

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                  <h1 class="title">产品详情</h1>
                <a class="icon icon-cart pull-right external" style="font-size:1.5rem;" href="/App/InfoGrid2/Mall/View/Prod/CartContent.aspx">
                    <span class="badge badge_cart" style="display:none;position: absolute;top: .1rem;left: 50%;z-index: 100; height: .8rem;min-width: .8rem;padding: 0 .3rem;font-size: .6rem;line-height: .8rem;color: white;vertical-align: top;background: red;border-radius: .5rem;margin-left: .1rem;">
                        
                    </span>
                </a>
            </header>


            <!-- 内容 -->
            <div class="content">

                <!-- 商品详情-->
                <div class="swiper-container" style="padding-top: 4px;">
                    <div class="swiper-wrapper">
                        <div class="swiper-slide" v-for="i in prod_obj.imgs">
                            <img :src="i.url" alt="" style="width: 100%;">
                        </div>
                    </div>
                    <div class="swiper-pagination"></div>
                </div>

                <div class="" style="background-color: white; padding: 1rem;">
                    <div style="margin-top: 0; margin-bottom: 0.5rem; padding: 0px; font-size: 1rem;">
                        {{prod_obj.PROD_TEXT}}
                    </div>
                    <div style="color: #DD2727;">
                        <span style="color: #999999;">会员价&nbsp;</span>
                        <span style="font-size: 1rem;">¥</span>
                        <span style="font-size: 1.5rem;">{{prod_obj.PRICE_MEMBER}}</span>
                    </div>
                    <div style="color: #999999; font-size: 1rem;">
                        <span style="font-size: 12pt;">市场价&nbsp;</span>
                        <span style="text-decoration: line-through;"> ¥{{prod_obj.PRICE_MARKET}}</span>
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

                    <div class="prod_intro" v-html="prod_obj.PROD_INTRO"></div>
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

                        var code = xyf_util.getQuery(me.url, "prod_code", true);


                        xyf_util.post("/App/InfoGrid2/Mall/Handlers/Prod.ashx", "GET_PROD_BY_CODE", { code: code }, function (data) {

                            obj.my_vue.prod_obj = data;

                            Vue.nextTick(function () {

                                var mySwiper = new Swiper('.swiper-container', {
                                    pagination: '.swiper-pagination',
                                    autoplay: 5000,//可选选项，自动滑动
                                });

                            });

                        });

                        //获取购物车商品
                        xyf_util.post("/App/InfoGrid2/Mall/Handlers/Prod.ashx", "GET_CART_NUM", {}, function (data) {

                            var num = parseFloat(data.num);

                            if (num > 0) {

                                var badgeCart = $(".badge_cart");

                                badgeCart.show();

                                badgeCart.text(num);
                            }


                        });


                        //加入购物车 没有其它规格就默认添加自身
                        el.on("click", ".push_cart", function (e) {

                            var url = Mini2.urlAppend("/Mall/View/Prod/ProdSpec.aspx", { code: code }, true);

                            $.router.load(url);

                        });

                        //加入购物车点击事件
                        el.on("click", ".open_spec_modal", function (e) {

                            var url = Mini2.urlAppend("/Mall/View/Prod/ProdSpec.aspx", { code: code }, true);

                            $.router.load(url);

                        });


                    }


                    //vue对象
                    obj.my_vue = null;

                    //初始化vue对象
                    obj.initVue = function (me) {

                        var el = me.el;

                        obj.my_vue = new Vue({
                            el: el.children(".content")[0],
                            data: {
                                prod_obj: {
                                    imgs:[]
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
