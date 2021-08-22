<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArticleDeta.aspx.cs" Inherits="App.InfoGrid2.GQT.View.ArticleDeta" %>

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
                <h1 class="title def_title"></h1>
                <a class="icon icon-home pull-right expand" href="/GQT/View/Home.aspx"></a>
            </header>

            <!--内容容器-->
            <div class="content" style="padding:0.5rem;">

                <div class="div_content">


                </div>

                <div>
                    阅读 10000+      点赞 300  
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

                        var item_id = xyf_util.getQuery(me.url,"item_id");

                        $.post("/GQT/Handlers/CmsHandler.ashx", { action: 'GET_ITEM', menu_id: menu_id,item_id:item_id }, function (result) {

                            if (!result.success) {

                                $.toast(result.error_msg);

                                return;
                            }

                            el.find(".def_title").text(result.data.CATALOG_TEXT);

                            var itemContentEl = $("<div>"+result.data.ITEM_CONTENT+"</div>");
                          

                            el.find(".div_content").append(itemContentEl);


                            //要过一会才能拿到img的宽度
                            setTimeout(function () {

                                var imgs = itemContentEl.find("img");

                                var body_width = $(document.body).width();

                                //设置img的宽度不能超过body的宽度
                                imgs.forEach(function (v, i) {


                                    var img_el = $(v);

                                    console.log(img_el[0].width);

                                    if (v.width > body_width) {

                                        img_el.width(body_width - 20);

                                    }


                                });

                                
                            }, 0.5 * 1000);



                        },"json");

                    }

                    return obj;
                }

            </script>


        </div>

    </div>

</body>
</html>
