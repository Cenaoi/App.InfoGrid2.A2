<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowAdve.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.Bus.ShowAdve" %>

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
                <h1 class="title def_title">5秒后显示红包</h1>
            </header>
            

            <div class="content" style="padding:0.5rem;display:none;" >

                 
                <div  class="text-center" style="font-size:1rem;font-weight:bold;">{{adve_obj.COL_1}}</div>

                <hr />

                <div style="text-indent:1rem;">
                    
                    
                    {{adve_obj.COL_2}}    


                </div>

                <div style="width:100%;">

                    <img  :src="i.url" style="width:100%;" v-for="i in adve_obj.imgs.data" />

                </div>

                <div>
                    所在地址：{{adve_obj.COL_6}}
                </div>

                <div>
                    联系方式：{{adve_obj.COL_8}}
                </div>

                <div id="div_hb" style="background-color:#EAC94E;height:4rem;padding: 1.5rem 0rem; text-align:center;margin-top:1rem;display:none;">

                    点击领取红包

                </div>


                <div id="div_messgae" style="background-color:#EAC94E;height:4rem;padding: 1.5rem 0rem; text-align:center;margin-top:1rem;display:none;">

                    商家今天的红包已派完，请明天再来领取。

                </div>

                
            </div>


            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        var adve_id = xyf_util.getQuery(me.url, "adve_id", true);

                        $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'GET_ADVE', adve_id: adve_id }, function (result) {

                            if (!result.success) {

                                $.alert(result.error_msg);

                                return;

                            }


                            var data = result.data;


                            obj.my_vue = new Vue({
                                el:el.children(".content")[0],
                                data:{
                                    adve_obj:data
                                }

                            });


                            el.children(".content").show();

                            //5秒显示抢红包节点
                            setTimeout(function () {

                                $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'GET_HB', adve_id: adve_id }, function (result) {

                                    if (!result.success) {

                                        
                                        el.find("#div_messgae").show();

                                        console.log(result);

                                        return;
                                    }

                                    var hb_el = el.find("#div_hb");

                                    hb_el.attr("data-id", result.data.id);

                                    hb_el.show();
                                    
                                }, "json");

                            }, 5 * 1000);
                            
                        }, "json");

                        $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'BROWSE_ADVE',adve_id:adve_id}, function (result) {


                            console.log(result);

                        }, "json");


                        //红包点击事件
                        el.on("click", "#div_hb", function (e) {

                            var hb_id = el.find("#div_hb").attr("data-id");



                            $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'RECEIVE_HB', adve_id: adve_id, hb_id: hb_id }, function (result) {

                                if (!result.success) {
                                    $.alert(result.error_msg);
                                    return;
                                }

                                var data = result.data;

                                $.alert("恭喜你获得了 "+data.HB_MONEY+" 元红包");


                            }, "json");

                        });

                    }

                    //页面加载成功事件
                    obj.onLoad = function () {

                        //获取微信js配置对象
                        $.post("/Wxhb/Handlers/UserHandler.ashx", { action: 'GET_WX_JS_CONFIG' }, function (result) {

                            if (!result.success) {

                                $.alert(result.error_msg);
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
