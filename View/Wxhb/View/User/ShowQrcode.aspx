<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowQrcode.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.User.ShowQrcode" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>微信红包</title>

    <script src="/Wxhb/Script/common.js"></script>
</head>
<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

             <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title def_title">专属二维码</h1>
            </header>

            <div class="content" style="display:none;">

                 <div class="card demo-card-header-pic" style="width: 70%;margin: auto;margin-top: 3rem;">
                    <div  class="card-header no-border">
                       关注下方二维码，可以领取身边商家派发的现金红包，您也可以发红包给身边的顾客朋友。
                    </div>
                     <div class="card-content">
                      <img class='card-cover' :src="user_obj.col_17" alt=""  style="background-color:aqua;width:100%;height:100%;">
                      <div class="card-content-inner" v-show="show_type === 'temp'">
                         <p class="color-gray" style="text-align:center;">二维码过期时间</p>
                         <p style="text-align:center;">{{user_obj.col_18}}</p>
                      </div>
                    </div>
                 </div> 

                  <div class="content-block">
                     <p><a href="#" class="button button-fill button-big">申请永久二维码 </a></p>
                  </div>


            </div>


            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;



                        var show_type = xyf_util.getQuery(me.url, "show_type", true);


                        $.post("/Wxhb/Handlers/UserHandler.ashx", { action: 'GET_QRCODE' }, function (result) {

                            if (!result.success) {
                                $.toast(result.error_msg);
                                return;
                            }



                            obj.my_vue = new Vue({
                                el: el.children(".content")[0],
                                data: {
                                    user_obj: result.data,
                                    show_type:show_type
                                }

                            });

                            el.children(".content").show();

                        },"json")

                        //申请永久二维码 按钮点击事件
                        el.on("click", ".button-big", function (e) {

                            $.post("/Wxhb/Handlers/UserHandler.ashx", { action: 'NEW_FOREVER_QRCODE_APPLY' }, function (result) {

                                if (!result.success) {

                                    $.toast(result.error_msg);

                                    return;

                                }

                                var url = Mini2.urlAppend("ForeverQrcodeApply.aspx", { id: result.data.id }, true);

                                $.router.load(url);

                            }, "json");

                        });

                    }


                    //vue对象
                    obj.my_vue = null;

                    return obj;

                }

            </script>


        </div>

    </div>

</body>
</html>
