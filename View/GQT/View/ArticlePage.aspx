<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArticlePage.aspx.cs" Inherits="App.InfoGrid2.GQT.View.ArticlePage" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>广州市南沙区青年联合会</title>
    <script src="/Script/common.js"></script>

</head>
<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">
    
        <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title def_title">文章列表</h1>
                <a class="icon icon-home pull-right expand" href="/GQT/View/Home.aspx"></a>
            </header>


            <div class="content">

                <div class="list-block" style="margin:0px;">
                    <ul>
                        <li class="item-content item-link" v-for="i in items" @click="articleItemClick(i)">
                            <div class="item-media" >
                                <div style="width: 4.5rem; height: 4.5rem; padding: 4px; background-image: url(/Mobile/res/image/demo/prod_bg_001.png); background-size: 100% 100%;">
                                    <img :src="i.IMG_URL" style="width: 100%;height:100%;">
                                </div>
                            </div>
                            <div class="item-inner" style="position:initial;height:100px;">
                                <div class="item-title">{{i.TITLE}}</div>
                            </div>
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

                        
                        var menu_id = xyf_util.getQuery(me.url, "menu_id", true);

                        var cata_id = xyf_util.getQuery(me.url, "cata_id");

                        $.post("/GQT/Handlers/CmsHandler.ashx", { action: "GET_ITEMS", menu_id: menu_id, cata_id: cata_id }, function (result) {

                            if (!result.success) {
                                $.toast(result.error_msg)
                                return;
                            }


                            var my_vue = new Vue({
                                el: el.children(".content")[0],
                                data: {
                                    items:result.data
                                },
                                methods: {
                                    //文章项点击事件
                                    articleItemClick: function (obj) {

                                        var my_vue = this;


                                        var url = Mini2.urlAppend("ArticleDeta.aspx", { menu_id: menu_id, item_id: obj.ROW_IDENTITY_ID }, true);

                                        $.router.load(url);


                                    }
                                }
                            })




                        }, "json");

                        $.post("/GQT/Handlers/CmsHandler.ashx", { action: "GET_CATALOG", menu_id: menu_id, cata_id: cata_id }, function (result) {

                            if (!result.success) {

                                $.toast(result.error_msg);
                                return;

                            }

                            el.find(".def_title").text(result.data.CATA_TEXT);



                        }, "json");

                    }


                    return obj;

                }

            </script>


        </div>

    </div>

</body>
</html>
