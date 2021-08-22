<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EcProd.aspx.cs" Inherits="App.InfoGrid2.Mobile_V1.Prod.EcProd" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>首页</title>

    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />

    <link href="/Core/Scripts/SUI/swiper-3.4.0.min.css" rel="stylesheet" />

      <style>
            .spec-expand {
                border-bottom: 1px solid #DDDDDD;
            }

     </style>

</head>
<body>
    

    <div class="page-group">

        <div class="page" >

                <!-- 标题栏 -->
                <header class="bar bar-nav">
                    <a class="icon icon-left pull-left back" href="EcHome.aspx"></a>
                    <h1 class="title"><%= GetProdNameStr() %></h1>
                </header>


                <!-- 底部按钮 -->
                <nav class="bar bar-tab">
                    <a class="tab-item external" href="/Mobile_V1/EsHome.aspx">
                        <span class="icon icon-home"></span>
                        <span class="tab-label">首页</span>
                    </a>
                    <a class="tab-item external" href="/Mobile_V1/Order/EcCart.aspx">
                        <span class="icon icon-cart"></span>
                        <span class="tab-label">购物车</span>
                    </a>
                    <a class="tab-item open_spec_modal" href="#" style="background-color: #FF9100; color: #ffffff; " >
                        <span class="tab-label">加入购物车</span>
                    </a>
                </nav>

                <div class="content">
                    <!-- 这里是页面内容区 -->

                    <div class="buttons-tab">
                        <a href="#tab1" class="tab-link active button">商品</a>
                        <a href="#tab2" class="tab-link button">详情</a>
                        <a href="#tab3" class="tab-link button">证书</a>
                    </div>

                    <div class="content-block" style="padding: 0px; margin-top: 0;">
                        <div class="tabs">
                            <div id="tab1" class="tab active">
                                <!-- 商品详情-->
                                <div class="swiper-container" data-space-between="10" style="padding-top: 4px;">
                                    <div class="swiper-wrapper">
                                        <div class="swiper-slide">
                                            <img src="/Mobile/res/image/demo/PROD_Thumb_003.jpg" alt="" style="width: 100%;">
                                        </div>
                                        <div class="swiper-slide">
                                            <img src="/Mobile/res/image/demo/PROD_Thumb_001.jpg" alt="" style="width: 100%;">
                                        </div>
                                        <div class="swiper-slide">
                                            <img src="/Mobile/res/image/demo/PROD_Thumb_002.jpg" alt="" style="width: 100%;">
                                        </div>
                                        <div class="swiper-slide">
                                            <img src="/Mobile/res/image/demo/PROD_Thumb_004.jpg" alt="" style="width: 100%;">
                                        </div>
                                        <div class="swiper-slide">
                                            <img src="/Mobile/res/image/demo/PROD_Thumb_005.jpg" alt="" style="width: 100%;">
                                        </div>
                                    </div>
                                    <div class="swiper-pagination"></div>
                                </div>
                                <div class="" style="background-color: white; padding: 1rem;">
                                    <div style="margin-top: 0; margin-bottom: 0.5rem; padding: 0px; font-size: 1rem;">
                                        {{ECP.PROD_NAME}}
                                    </div>
                                    <div style="color: #DD2727;">
                                        <span style="color: #999999;">会员价&nbsp;</span>
                                        <span style="font-size: 1rem;">¥</span>
                                        <span style="font-size: 1.5rem;">{{ECP.PRICE_MEMBER}}</span>
                                    </div>
                                    <div style="color: #999999; font-size: 1rem;">
                                        <span style="font-size: 12pt;">市场价&nbsp;</span>
                                        <span style="text-decoration: line-through;"> ¥{{ECP.PRICE_MARKET}}</span>
                                    </div>
                                </div>
                                <div class="list-block" style="margin: 0.8rem 0;">
                                    <ul>
                                        <li class="item-content item-link" onclick="openSpecModal()">
                                            <div class="item-inner">
                                                <div class="item-title">选择规格</div>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div id="tab2" class="tab">
                                <div class="content-block">
                                    <iframe height=290 width=330 src="http://player.youku.com/embed/XMTQzNDQ0MTAxNg=="></iframe>
                                </div>
                            </div>
                            <div id="tab3" class="tab">
                                <div class="content-block">
                                    <p>This is tab 2 content</p>
                                </div>
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

                            obj.initVue(me);

                            el.on("click",".open_spec_modal",function(e){


                                var my_vue = obj.myVue;
                                
                                var url = Mini2.urlAppend("EcProdSpec.aspx",{prod_id:my_vue.ECP.ES_COMMON_PROD_ID},true);


                                $.router.load(url,true);

                            });


        

                        }


                        //Vue对象
                        obj.myVue = null;


                        //初始化vue相关对象
                        obj.initVue = function (me) {

                            var el = me.el;


                            var prod_obj = <%= GetProdObj() %>;


                            obj.myVue = new Vue({
                                el: el.children(".content")[0],
                                data: prod_obj
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

