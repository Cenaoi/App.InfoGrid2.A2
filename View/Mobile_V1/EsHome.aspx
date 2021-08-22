<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EsHome.aspx.cs" Inherits="App.InfoGrid2.Mobile_V1.EsHome" %>

<%@ Register Src="~/Mobile_V1/SiteFooter.ascx" TagPrefix="uc1" TagName="SiteFooter" %>

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

</head>
<body>



    <div class="page-group">

             <div class="page">
                <!-- 首页假的搜索框 -->
                <header class="bar bar-nav" style="background-color: #dd2727;">
                    <a href="EcSearch.aspx">
                        <div class="search-fake" style="padding: 0.3rem;">
                            <div style=" position:static; height:1.6rem;background-color: white; -moz-border-radius: 5px; /* gecko browsers */
                                        -webkit-border-radius: 5px; /* webkit browsers */
                                        border-radius: 5px; /* w3c syntax */">
                                <label class="icon icon-search" style="top:0px; position:absolute;"></label>
                                <span style="color: #808080;top:10px;left:1rem; position:absolute; margin-left:1rem;">输入关键字</span>
                            </div>
                        </div>
                    </a>
                </header>

                <!-- 底部按钮 -->
                <uc1:SiteFooter runat="server" ID="SiteFooter" ActionName="home" />

                <div class="content">
                    <!-- 这里是页面内容区 -->

                    <!-- 广告位-->
                    <div class="swiper-container" style="width:100%;padding-bottom:0rem;">
                        <div class="swiper-wrapper" style="height:100%;">
                            <div class="swiper-slide" v-for="tl in TOP_LINK_LIST ">
                                <img :src="tl.IMAGE_URL" :alt="tl.T_TEXT" style="width: 100%;">
                            </div>
                        </div>
                        <div class="swiper-pagination"></div>
                    </div>

                    <!--商品大分类-->
                    <div class="grid-demo" style="background-color:#FFFFFF;padding-top:0.5rem;">
                        <div class="row  no-gutter">
                            <div class="col-25" style="padding-bottom: 1rem; height:90px;" v-for="pc in PROD_CATA_LIST">
                                <a :href="pc.T_HREF" data-no-cache="true">
                                    <div class="text-center" style="padding: 0rem 1rem 0rem 1rem;">
                                        <img :src="pc.IMAGE_URL" class="prod-catalog"  style="width:100%;"/>
                                        <h5 style="margin: 0; font-weight:normal;">{{pc.T_TEXT}}</h5>
                                    </div>
                                </a>
                            </div>

                        </div>
                    </div>
            
                    <div >
                    <!-- 论坛信息 -->
                        <a href="#"  v-for="adv in ADV_LIST" >
                            <img style="width: 100%;display: block;" :src="adv.IMAGE_URL" :alt="adv.T_TEXT" border="0" />
                        </a>
                    </div>

                    <!--推广商品-->
                    <div class="row no-gutter">
                        <div class="col-50" style="border-bottom:1px solid #DDDDDD; border-right:1px solid #DDDDDD;" v-for="pl in PROD_LIST">
                            <a :href="pl.T_HREF" data-no-cache="true">
                                <img :src="pl.IMAGE_URL" :alt="pl.T_TEXT" style="width: 100%;display: block;" border="0" />
                            </a>
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

                            var mySwiper = new Swiper('.swiper-container', {
                                pagination: '.swiper-pagination',
                                autoplay: 5000,//可选选项，自动滑动
                            });


                            


                        }


                        //Vue对象
                        obj.myVue = null;


                        //初始化跟Vue相关的东西
                        obj.initVue = function (me) {

                            var el = me.el;

                            var link_list = <%= GetRedirectLink()  %>;

                            //初始化vue对象
                            obj.myVue = new Vue({
                                el:el.children(".content")[0],
                                data:link_list
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



