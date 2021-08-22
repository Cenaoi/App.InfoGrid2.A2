<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HbParam.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.Bus.HbParam" %>

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
<body>
    
    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">


            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title def_title">生成红包参数</h1>
            </header>


            <div class="content" style="display:none;">


                <my-list :title_text="hb_obj.title_text" :list_data="hb_obj.list_data" ></my-list>


                <div class="content-block">
                    <p><a href="#" class="button button-round button-fill button-big btn_success">确定</a></p>
                </div>

            </div>

            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        var re_money = xyf_util.getQuery(me.url, "re_money", true);
                        var adve_id = xyf_util.getQuery(me.url, "adve_id", true);

                        $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'GET_HB_PARAM', re_money: re_money, adve_id: adve_id }, function (result) {
                            
                            if (!result.success) {

                                $.toast(result.error_msg);
                                return;

                            }
                            
                            var data = result.data;

                            obj.my_vue = new Vue({
                                el: el.children(".content")[0],
                                data:{
                                    hb_obj:data,
                                    re_money:data.re_money
                                }
                            });
                            
                            el.children(".content").show();
                            
                        }, "json");



                        el.on("click", ".btn_success", function (e) {
                            
                            $.confirm("是否确定新增广告！", "", function () {


                                $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'ADVE_SUBMIT', adve_id: adve_id, re_money: obj.my_vue.re_money }, function (result)
                                {

                                    if (!result.success) {

                                        $.toast(result.error_msg);
                                        return;
                                    }

                                    
                                    var url = Mini2.urlAppend("/Wxhb/View/Bus/BusContent.aspx", {}, true);

                                    location.href = url;

                                }, "json");
                                
                            });

                        });
                    }


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



                    //vue对象
                    obj.my_vue = null;

                    return obj;

                }

            </script>


        </div>

    </div>

</body>
</html>
