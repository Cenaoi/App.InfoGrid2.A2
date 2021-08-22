<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="App.InfoGrid2.Mall.View.Home" %>

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


    .prod_intro img,iframe {
        width:100%;
        height:100%;
    }





    </style>

</head>
<body>
    
    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

                <!-- 首页假的搜索框 -->
                <header class="bar bar-nav" style="background-color: #dd2727;">
                    <a href="#" >
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

            <uc1:Footer runat="server" ID="Footer" ActionName="home" />

            <!-- 内容 -->
            <div class="content">

                <!-- 广告位-->
                <div class="swiper-container" style="width:100%;padding-bottom:0rem;"  >
                    <div class="swiper-wrapper" style="height:100%;">
                        <div class="swiper-slide" v-for="tl in TOP_LINK_LIST ">
                            <img :src="tl.IMAGE_URL" :alt="tl.T_TEXT" style="width: 100%;">
                        </div>
                    </div>
                    <div class="swiper-pagination"></div>
                </div>
                
                <div style="background-color: #FFFFFF; padding-top: 0.5rem;" class="grid-demo">
                    <div class="row  no-gutter">
                        <div style="padding-bottom: 1rem; height: 90px;" class="col-25"><a href="/Mall/View/Prod/ProdContent.aspx" data-no-cache="true">
                            <div style="padding: 0rem 1rem 0rem 1rem;" class="text-center">
                                <img src="/Mobile/res/image/demo/cata_009_256.png" style="width: 100%;" class="prod-catalog">
                                <h5 style="margin: 0; font-weight: normal;font-size:0.5rem;">所有品类</h5>
                            </div>
                        </a></div>
                        <div style="padding-bottom: 1rem; height: 90px;" class="col-25"><a href="/Mall/View/Prod/ProdContent.aspx?cata_code=101" data-no-cache="true">
                            <div style="padding: 0rem 1rem 0rem 1rem;" class="text-center">
                                <img src="/Mobile/res/image/demo/cata_010_256.png" style="width: 100%;" class="prod-catalog">
                                <h5 style="margin: 0; font-weight: normal;font-size:0.5rem;">EUG-HOMME</h5>
                            </div>
                        </a></div>
                        <div style="padding-bottom: 1rem; height: 90px;" class="col-25"><a href="/Mall/View/Prod/ProdContent.aspx?cata_code=102" data-no-cache="true">
                            <div style="padding: 0rem 1rem 0rem 1rem;" class="text-center">
                                <img src="/Mobile/res/image/demo/cata_011_256.png" style="width: 100%;" class="prod-catalog">
                                <h5 style="margin: 0; font-weight: normal;font-size:0.5rem;">THOMASTN</h5>
                            </div>
                        </a></div>
                        <div style="padding-bottom: 1rem; height: 90px;" class="col-25">
                            <a href="/Mall/View/Prod/ProdContent.aspx?cata_code=103" data-no-cache="true">
                            <div style="padding: 0rem 1rem 0rem 1rem;" class="text-center">
                                <img src="/Mobile/res/image/demo/cata_003_256.png" style="width: 100%;" class="prod-catalog">
                                <h5 style="margin: 0; font-weight: normal;font-size:0.5rem;">KELANTAN</h5>
                            </div>
                        </a>

                        </div>
  
                    </div>
                </div>

                <div><a href="#">
                    <img style="width: 100%; display: block;" src="/Mobile/res/image/demo/Banner_004.png" alt="广告1" border="0"></a>
                </div>

                <div class="row no-gutter">
                    <div style="border-bottom: 1px solid #DDDDDD; border-right: 1px solid #DDDDDD;" class="col-50" v-for="p in prods">
                       <a :href="'/Mall/View/Prod/ProdDeta.aspx?prod_code='+p.PK_PROD_CODE" data-no-cache="true">
                          <div class="card demo-card-header-pic">
                            <div class="card-header color-white no-border no-padding">
                              <img class='card-cover'  :src="p.PROD_THUMB" alt="">
                            </div>
                            <div class="card-content">
                              <div class="card-content-inner" style="padding:0 6px;margin:10px 0 0 0;">
                                <p class="color-gray" style="white-space:nowrap;text-overflow: ellipsis;overflow: hidden;">{{p.PROD_TEXT}}</p>
                              </div>
                            </div>
                            <div class="card-footer" style="position: static;">
                              <span class="link" style="color:red;">￥{{p.PRICE_MARKET}}</span>
                            </div>
                         </div>
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

                        //获取可以显示首页的商品信息
                        xyf_util.post("/APP/InfoGrid2/Mall/Handlers/Prod.ashx", "GET_PRODS_BY_HOME", {}, function (data) {

                            obj.my_vue.prods = data;

                            Vue.nextTick(function () {

                                var mySwiper = new Swiper('.swiper-container', {
                                    pagination: '.swiper-pagination',
                                    autoplay: 5000,//可选选项，自动滑动
                                });

                            });

                        });

                        //搜索框点击事件
                        el.on("click", ".search-fake", function (e) {

                            var url = Mini2.urlAppend("/App/InfoGrid2/Mall/View/Prod/ProdContent.aspx", {}, true);

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
                                TOP_LINK_LIST: [

                                    {
                                        IMAGE_URL: 'https://images.pexels.com/photos/374845/pexels-photo-374845.jpeg?w=1260&h=750&auto=compress&cs=tinysrgb'
                                    },
                                      {
                                          IMAGE_URL: 'https://images.pexels.com/photos/211050/pexels-photo-211050.jpeg?w=1260&h=750&auto=compress&cs=tinysrgb'
                                      },
                                        {
                                            IMAGE_URL: 'https://images.pexels.com/photos/450212/pexels-photo-450212.jpeg?w=1260&h=750&auto=compress&cs=tinysrgb'
                                        }


                                ],
                                prods:[]
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
