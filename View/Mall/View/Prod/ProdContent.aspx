<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProdContent.aspx.cs" Inherits="App.InfoGrid2.Mall.View.Prod.ProdContent" %>

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

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">产品中心</h1>
            </header>

            <div class="content">
               <!-- 搜索框 -->
               <div class="searchbar row">
                <div class="search-input col-80">
                  <label class="icon icon-search"></label>
                  <input type="search" placeholder='输入关键字...' v-model="s_prod_text"/>
                </div>
                <a class="button button-fill button-primary col-20"  @click="search_btn">搜索</a>
              </div>

                <!-- 这里是页面内容区 -->
                <div class="list-block media-list" style="margin-top: 0rem;" >
                    <ul class="list-image" v-if="prods.length > 0">
                        <li v-for="p in prods">
                            <a :href="'/Mall/View/Prod/ProdDeta.aspx?prod_code='+p.PK_PROD_CODE" data-no-cache="true"  class="item-link item-content" >
                                <div class="item-media">
                                    <div style="width: 4.5rem; height: 4.5rem; padding: 4px; background-image: url(/Mobile/res/image/demo/prod_bg_001.png); background-size: 100% 100%;">
                                        <img :src="p.PROD_THUMB" style="width: 100%;height:100%;">
                                    </div>
                                </div>
                                <div class="item-inner">
                                    <div class="item-title-row">
                                        <div class="item-title">{{p.PROD_TEXT}}</div>
                                    </div>
                                    <div class="item-text" v-html="p.PROD_INTRO"></div>
                                    <div class="item-text">
                                        会员价：<span style="color: red; font-size: 12pt; font-weight: bold;">￥{{p.PRICE_MEMBER}}</span><br />
                                        市场价：<span style="text-decoration: line-through; color: #a7a7a7; margin-right: 15px;">￥{{p.PRICE_MARKET}}</span>
                                    </div>
                                </div>
                            </a>
                        </li>
                    </ul>

                    <div class="text-center" v-if="prods.length === 0">
                        <p>哦噢，没有数据了喔！</p>
                    </div>

                </div>

                <div class="content-block" v-if="prods.length > 0">
                    <p><a href="#" class="button button-round button-fill" @click="loadMore">加载更多</a></p>
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

                        obj.loadMroe(me);

        
                    }

                    //vue对象
                    obj.my_vue = null;


                    //初始化vue相关对象  用一下箭头函数看看
                    obj.initVue = function(me){

                        var el = me.el;

                        obj.my_vue = new Vue({
                            el:el.children(".content")[0],
                            data:{
                                prods: [],
                                page_index: 0,
                                page_size: 10,
                                s_prod_text: ''

                            }, methods: {

                                search_btn: function () {

                                    var my_vue = this;

                                    my_vue.page_index = 0;

                                    my_vue.prods = [];

                                    obj.loadMroe(me);


                                },
                                loadMore: function () {

                                    var my_vue = this;

                                    obj.loadMroe(me);


                                }
                            }
                        });

                    }


                    //加载更多
                    obj.loadMroe = function (me) {

                        var my_vue = obj.my_vue;

                        var cata_code = xyf_util.getQuery(me.url, "cata_code", true);

                        console.log("准备post......");

                        xyf_util.post("/App/InfoGrid2/Mall/Handlers/Prod.ashx", "GET_PRODS_BY_PAGE",
                            {
                                page_index: my_vue.page_index,
                                page_size: my_vue.page_size,
                                s_prod_text: my_vue.s_prod_text,
                                cata_code:cata_code
                            }, function (data) {

                            if (data.length === 0) {

                                $.toast("没有更多数据了");

                                return;
                            }

                            my_vue.prods = my_vue.prods.concat(data);

                            my_vue.page_index++;
                        });

                    }



                    return obj;

                }

            </script>


        </div>

    </div>


</body>
</html>
