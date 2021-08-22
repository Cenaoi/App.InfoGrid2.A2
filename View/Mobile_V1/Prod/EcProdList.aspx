<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EcProdList.aspx.cs" Inherits="App.InfoGrid2.Mobile_V1.Prod.EcProdList" %>

<%@ Register Src="~/Mobile_V1/SiteFooter.ascx" TagPrefix="uc1" TagName="SiteFooter" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>

     <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>产品列表</title>

    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />

    <link href="/Core/Scripts/SUI/swiper-3.4.0.min.css" rel="stylesheet" />

</head>
<body>


        <div class="page-group">

            <div class="page" id="page_ec_prod_list">


                <style>
                    .list-block .item-inner:after {
                        background: none;
                    }

                    .list-image li {
                        border-bottom: 1px solid #DDDDDD;
                    }
                </style>

                <!-- 标题栏 -->
                <header class="bar bar-nav">
                    <a class="icon icon-left pull-left back" href="EcHome.aspx"></a>
                    <h1 class="title"><%=GetCataTextStr() %></h1>
                </header>

                <!-- 底部按钮 -->
                <uc1:SiteFooter runat="server" ID="SiteFooter" ActionName="cart" />

                <div class="content">

                    <div class="row" style="height: 1.5rem;">

                        <div class="col-40">
                            <button class="button  button-light" style="width: 100%;">综合</button>
                        </div>

                        <div class="col-40">
                            <button class="button  button-light" style="width: 100%;">销量</button>
                        </div>
                    </div>

                    <!-- 这里是页面内容区 -->
                    <div class="list-block media-list" style="margin-top: 0rem;">
                        <ul class="list-image">
                            <li v-for="ecp in ECP_LIST">
                                <a href="EcProd.aspx?prod_id={{ecp.ES_COMMON_PROD_ID}}" data-no-cache="true"  class="item-link item-content">
                                    <div class="item-media">
                                        <div style="width: 4.5rem; height: 4.5rem; padding: 4px; background-image: url(res/image/demo/prod_bg_001.png); background-size: 100% 100%;">
                                            <img src="res/image/demo/PROD_001.jpg" style="width: 100%;">
                                        </div>
                                    </div>
                                    <div class="item-inner">
                                        <div class="item-title-row">
                                            <div class="item-title">{{ecp.PROD_NAME}}</div>
                                        </div>
                                        <div class="item-text">{{ecp.PROD_INTRO}}</div>
                                        <div class="item-text">
                                            会员价：<span style="color: red; font-size: 12pt; font-weight: bold;">￥{{ecp.PRICE_MEMBER}}</span><br />
                                            市场价：<span style="text-decoration: line-through; color: #a7a7a7; margin-right: 15px;">￥{{ecp.PRICE_MARKET}}</span>

                                        </div>
                                    </div>
                                </a>
                            </li>
                        </ul>
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

                            //初始化Vue相关的对象
                            obj.initVue = function (me) {

                                var el = me.el;

                                var prode_data = <%= GetEcpsObj() %>;

                                var prodVue = new Vue({
                                    el: $(el).children(".content")[0],
                                    data:prode_data
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

