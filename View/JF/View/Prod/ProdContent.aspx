<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProdContent.aspx.cs" Inherits="App.InfoGrid2.JF.View.Prod.ProdContent" %>



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

    <title>商品中心</title>

</head>
<body>



     <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title"><%=GetCataTextStr() %></h1>
            </header>


            <div class="content">

            <!-- 这里是页面内容区 -->
            <div class="list-block media-list" style="margin-top: 0rem;">
                <ul class="list-image">
                    <li v-for="p in prods">
                        <a :href="'ProdDeta.aspx?prod_id='+p.id" data-no-cache="true"  class="item-link item-content">
                            <div class="item-media">
                                <div style="width: 4.5rem; height: 4.5rem; padding: 4px; background-image: url(/Mobile/res/image/demo/prod_bg_001.png); background-size: 100% 100%;">
                                    <img :src="p.prod_thumb" style="width: 100%;height:100%;">
                                </div>
                            </div>
                            <div class="item-inner">
                                <div class="item-title-row">
                                    <div class="item-title">{{p.prod_name}}</div>
                                </div>
                                <div class="item-text">{{p.prod_intro}}</div>
                                <div class="item-text">
                                    会员价：<span style="color: red; font-size: 12pt; font-weight: bold;">￥{{p.price}}</span><br />
                                    市场价：<span style="text-decoration: line-through; color: #a7a7a7; margin-right: 15px;">￥{{p.price_market}}</span>
                                </div>
                            </div>
                        </a>
                    </li>
                </ul>

                <div class="text-center" v-if="prods.length === 0">
                    <p>哦噢，没有数据了喔！</p>
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
        

                    }

                    obj.prods = <%= GetProdsObj() %>;

                    //vue对象
                    obj.my_vue = null;


                    //初始化vue相关对象  用一下箭头函数看看
                    obj.initVue = function(me){

                        var el = me.el;

                        obj.my_vue = new Vue({
                            el:$(el).children(".content")[0],
                            data:{
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

        $.router = Mini2.create('Mini2.ui.PageRoute', {});



    });


</script>

