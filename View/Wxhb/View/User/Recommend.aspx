<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recommend.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.User.Recommend" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>微信红包</title>

    <script src="/Wxhb/Script/common.js?v=20170209001"></script>

</head>
<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">

    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">


            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">推荐用户列表</h1>
            </header>

            <div class="content">

                 <div style="margin:0px;" class="list-block media-list">
                    <ul>
                        <li v-for="r in recommend_users">
                            <div style="padding-left:0.5rem;" class="item-content ">
                                <div class="item-media">
                                    <img :src="r.HEAD_IMG_URL" style="width: 2.5rem;">
                                </div> 
                                <div class="item-inner">
                                    <div class="item-title-row">
                                        <div class="item-title">{{r.W_NICKNAME}}</div>
                                        <div class="item-after"></div>
                                    </div> 
                                </div>
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

                        
                        $.post("/Wxhb/Handlers/UserHandler.ashx", { action: "GET_RECOMMEND_USERS" }, function (result) {

                            
                            if (!result.success) {

                                $.toast(result.error_msg);
                                return;
                            }


                            obj.my_vue = new Vue({

                                el: el.children(".content")[0],
                                data: {
                                    recommend_users:result.data
                                }

                            });





                        }, "json");


                    }


                    //vue对象
                    obj.my_vue = null;


                   

                    //页面加载成功事件
                    obj.onLoad = function () {

                        //获取微信js配置对象
                        $.post("/Wxhb/Handlers/UserHandler.ashx", { action: 'GET_WX_JS_CONFIG' }, function (result) {

                            if (!result.success) {

                                $.toast(result.error_msg);
                                return;

                            }

                            var config = result.data;

                            var config_str = JSON.stringify(config);

                            wx.config(config);

                            wx.ready(function () {

                                //这是隐藏微信自带的右上角菜单  所有非基础按钮接口
                                wx.hideAllNonBaseMenuItem();

                            });



                        }, "json");

                    }



                    return obj;

                }

            </script>


        </div>

    </div>

</body>
</html>
