<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HBContent.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.Bus.HBContent" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <title>微信红包</title>
    <script src="/Wxhb/Script/common.js?v=20170210003"></script>
</head>
<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">
    
    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">


            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <h1 class="title def_title">红包列表</h1>
            </header>

            <div class="content">


                <my-list-link :list_data="list_data"></my-list-link>




            </div>

            <script type="text/javascript" data-main="true">
        
                function main() {
            



                    var obj = {};

                    //vue对象
                    obj.my_vue = null;

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;


                        var v_id = xyf_util.getQuery(me.url, "v_id", true);

                        $.post("/Wxhb/Handlers/LoginHandler.ashx", { action: 'AUTO_LOGIN', v_id: v_id }, function () {


                            $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'GET_ALL_ADVES' }, function (result) {


                                if (!result.success) {


                                    $.toast(result.error_msg);

                                    return;

                                }

                                result.data.forEach(function (v, i) {

                                    var o = {};


                                    o.img_src = "/Wxhb/res/HB_001.png";
                                    o.title = v.COL_1;
                                    o.text = "地址：" + v.COL_6;
                                    o.sub_text = "距您：30千米.";

                                    o.url = Mini2.urlAppend("/Wxhb/View/Bus/ShowAdve.aspx", { adve_id: v.ROW_IDENTITY_ID }, true);

                                    obj.adves.push(o);
                                    
                                });

                                obj.initVue(me);
                                



                            }, "json");






                        }, "json");


                    }



                    //广告数组
                    obj.adves = [];

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

                    

                    //初始化Vue相关对象
                    obj.initVue = function (me) {

                        var el = me.el;

                        obj.my_vue = new Vue({
                            el: el.find(".content")[0],
                            data: {
                                list_data: obj.adves
                            }
                        });




                    }


                    return obj;

                }

            </script>


        </div>

    </div>


</body>
</html>
