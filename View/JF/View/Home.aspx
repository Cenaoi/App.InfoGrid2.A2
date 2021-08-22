<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="App.InfoGrid2.JF.View.Home1" %>

<%@ Register Src="~/JF/View/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>贝汗商城</title>

    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />

    <link href="/Core/Scripts/SUI/swiper-3.4.0.min.css" rel="stylesheet" />


    <style>

        img{

            width:100%;


        }

    </style>


</head>
<body>

    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <uc1:Footer runat="server" ID="Footer" ActionName="home" />


            <div class="content">

                <!-- 广告位-->
                <div class="swiper-container" style="width:100%;padding-bottom:0rem;" >
                    <div class="swiper-wrapper" style="height:100%;">
                        <div class="swiper-slide" v-for="tl in TOP_LINK_LIST ">
                            <img :src="tl.IMAGE_URL" :alt="tl.T_TEXT" style="width: 100%;">
                        </div>
                    </div>
                    <div class="swiper-pagination"></div>
                </div>

                <div style="background-color: #FFFFFF; padding-top: 0.5rem;" class="grid-demo">
                    <div class="row  no-gutter">
                        <div style="padding-bottom: 1rem; height: 90px;" class="col-25"><a href="/JF/View/Prod/ProdContent.aspx?cata_code=001" data-no-cache="true">
                            <div style="padding: 0rem 1rem 0rem 1rem;" class="text-center">
                                <img src="/Mobile/res/image/demo/cata_009_256.png" style="width: 100%;" class="prod-catalog">
                                <h5 style="margin: 0; font-weight: normal;font-size:0.5rem;">特产</h5>
                            </div>
                        </a></div>
                        <div style="padding-bottom: 1rem; height: 90px;" class="col-25"><a href="/JF/View/Prod/ProdContent.aspx?cata_code=002" data-no-cache="true">
                            <div style="padding: 0rem 1rem 0rem 1rem;" class="text-center">
                                <img src="/Mobile/res/image/demo/cata_010_256.png" style="width: 100%;" class="prod-catalog">
                                <h5 style="margin: 0; font-weight: normal;font-size:0.5rem;">玩家</h5>
                            </div>
                        </a></div>
                        <div style="padding-bottom: 1rem; height: 90px;" class="col-25"><a href="/JF/View/Prod/ProdContent.aspx?cata_code=003" data-no-cache="true">
                            <div style="padding: 0rem 1rem 0rem 1rem;" class="text-center">
                                <img src="/Mobile/res/image/demo/cata_011_256.png" style="width: 100%;" class="prod-catalog">
                                <h5 style="margin: 0; font-weight: normal;font-size:0.5rem;">通信</h5>
                            </div>
                        </a></div>
                        <div style="padding-bottom: 1rem; height: 90px;" class="col-25">
                            <a href="/JF/View/Prod/ProdContent.aspx?cata_code=004" data-no-cache="true">
                            <div style="padding: 0rem 1rem 0rem 1rem;" class="text-center">
                                <img src="/Mobile/res/image/demo/cata_003_256.png" style="width: 100%;" class="prod-catalog">
                                <h5 style="margin: 0; font-weight: normal;font-size:0.5rem;">品牌代理</h5>
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
                       <a :href="'/JF/View/Prod/ProdDeta.aspx?prod_id='+p.id" data-no-cache="true">
                          <div class="card demo-card-header-pic">
                            <div class="card-header color-white no-border no-padding">
                              <img class='card-cover'  :src="p.prod_thumb" alt="">
                            </div>
                            <div class="card-content">
                              <div class="card-content-inner" style="padding:0 6px;margin:10px 0 0 0;">
                                <p class="color-gray" style="white-space:nowrap;text-overflow: ellipsis;overflow: hidden;">{{p.prod_name}}</p>
                              </div>
                            </div>
                            <div class="card-footer" style="position: static;">
                              <span class="link" style="color:red;">￥{{p.price}}</span>
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


                        setTimeout(function () {

                            var mySwiper = new Swiper('.swiper-container', {
                                pagination: '.swiper-pagination',
                                autoplay: 5000,//可选选项，自动滑动
                            });


                        },0.5 * 1000);




                    }


                    obj.prods = <%= GetProdsObj() %>;

                    
                    //Vue对象
                    obj.myVue = null;


                    //初始化跟Vue相关的东西
                    obj.initVue = function (me) {

                        var el = me.el;

                        //初始化vue对象
                        obj.myVue = new Vue({
                            el:el.children(".content")[0],
                            data: {
                                TOP_LINK_LIST:[

                                    {
                                        IMAGE_URL: '/Mobile/res/image/demo/Banner_001_1080x356.jpg'
                                    },
                                      {
                                          IMAGE_URL: '/Mobile/res/image/demo/Banner_002_1080x356.jpg'
                                      },
                                        {
                                            IMAGE_URL: '/Mobile/res/image/demo/Banner_003_1080x356.jpg'
                                        }
                                   

                                ],
                                prods:obj.prods
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

