<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cxcy.aspx.cs" Inherits="App.InfoGrid2.GQT.View.Cxcy" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>广州市南沙区青年联合会</title>

    <script src="/GQT/Script/common.js"></script>

</head>
<body style="max-width:480px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page" >

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title def_title">创新创业</h1>
                <a class="icon icon-home pull-right expand" href="/GQT/View/Home.aspx"></a>
            </header>

            <div class="content" >

                <!-- 广告位-->
                <div class="swiper-container" style="width:100%;padding-bottom:0rem;margin-bottom:10px;">
                    <div class="swiper-wrapper" style="height:200px;">
                        <div class="swiper-slide" >
                            <img src="//cache.house.sina.com.cn/citylifehouse/citylife/41/0e/20090727_65001_1.jpg"  style="width: 100%;height:100%;">
                        </div>
                        <div class="swiper-slide">
                            <img src="//img04.tooopen.com/images/20131102/sy_45272029654.jpg" style="width: 100%;height:100%;">
                        </div>
                        <div class="swiper-slide">
                            <img src="//www.sinaimg.cn/dy/slidenews/9_img/2009_34/438_426_216145.jpg" style="width: 100%;height:100%;">
                        </div>
                    </div>
                    <div class="swiper-pagination"></div>
                </div>


                <div class="row" style="padding:0.5rem;">
                    <div class="col-50 cms_menu" style="margin-top:10px;" data-url="SinglePage.aspx?menu_id=21" >

                        <div class="text-center" style="background-color:#2e6da4; border-radius:10px; height:6rem; cursor:pointer; overflow:hidden;padding-top:2.4rem;color:white;font-size:0.9rem;margin-left:auto;margin-right:auto; ">
                            关于“创汇谷”
                        </div>

                    </div>
                    <div class="col-50 cms_menu" style="margin-top:10px;" data-url="ArticlePage.aspx?menu_id=22&cata_id=1" >

                        <div class="text-center" style="background-color:#5cb85c;border-radius:10px; height:6rem; cursor:pointer; overflow:hidden;padding-top:2.4rem;color:white;font-size:0.9rem;margin-left:auto;margin-right:auto; ">
                            扶持政策
                        </div>

                    </div>
                    <div class="col-50 cms_menu" style="margin-top:10px;" data-url="Wyrz.aspx">

                        <div class="text-center" style="background-color:#5bc0de;border-radius:10px; height:6rem; cursor:pointer; overflow:hidden;padding-top:2.4rem;color:white;font-size:0.9rem;margin-left:auto;margin-right:auto; ">
                            入驻申请
                        </div>

                    </div>
                    <div class="col-50 cms_menu" style="margin-top:10px;" data-url="ArticlePage.aspx?menu_id=25&cata_id=1" >

                        <div class="text-center" style="background-color:#f0ad4e;border-radius:10px; height:6rem; cursor:pointer; overflow:hidden;padding-top:2.4rem;color:white;font-size:0.9rem;margin-left:auto;margin-right:auto; ">
                            增值服务
                        </div>

                    </div>
                    
                    <div class="col-50 cms_menu" style="margin-top:10px;" data-url="Hdsq.aspx">

                        <div class="text-center" style="background-color:#c9302c;border-radius:10px; height:6rem; cursor:pointer; overflow:hidden;padding-top:2.4rem;color:white;font-size:0.9rem;margin-left:auto;margin-right:auto; ">
                            活动社区
                        </div>

                    </div>
                    <div class="col-50 cms_menu" style="margin-top:10px;" data-url="Zxzx.aspx" >

                        <div class="text-center" style="background-color:#9f5edd;border-radius:10px; height:6rem; cursor:pointer; overflow:hidden;padding-top:2.4rem;color:white;font-size:0.9rem;margin-left:auto;margin-right:auto; ">
                            在线咨询
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

   
                        setTimeout(function () {

                            var mySwiper = new Swiper('.swiper-container', {
                                pagination: '.swiper-pagination',
                                autoplay: 5000,//可选选项，自动滑动
                                paginationType: 'bullets',
                            });


                        }, 0.5 * 1000);


                        //cms菜单点击事件
                        el.on("click", ".cms_menu", function (e) {

                            console.log("点击了！");

                            var target_node = e.currentTarget;

                            var url = $(target_node).attr("data-url");

                            url = Mini2.urlAppend(url, {}, true);

                            console.log("url:", url);

                            $.router.load(url);


                        });



                    }

                    return obj;

                }

                </script>


        </div>

    </div>


</body>
</html>
