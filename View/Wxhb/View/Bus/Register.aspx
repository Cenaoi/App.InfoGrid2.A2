<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.Bus.Register" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>微信红包</title>

    <script src="/Wxhb/Script/common.js?v=20170213"></script>

</head>
<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">


            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <h1 class="title def_title">注册界面</h1>
            </header>

            <div class="content" style="display:none;">

                <div class="list-block" style="margin-top:0px;">
                    <ul>
                        <li>
                        <div class="item-content">
                            <div class="item-inner">
                            <div class="item-title label">姓名</div>
                            <div class="item-input">
                                <input type="text" v-model="user_obj.col_23" placeholder="你的名字">
                            </div>
                            </div>
                        </div>
                        </li>
                        <li>
                        <div class="item-content">
                            <div class="item-inner">
                            <div class="item-title label">手机号码</div>
                            <div class="item-input">
                                <input type="text" v-model="user_obj.col_20" placeholder="手机号码">
                            </div>
                            </div>
                        </div>
                        </li>
                    </ul>
                </div>

                <div id="div_map_1" style="width:100%;height:15rem;" >

                </div>

                <div id="div_address" style="margin-top:0.5rem;">

                </div>

                <div class="content-block">
                    <p><a href="#" class="button button-round button-fill button-big btn_register">提交</a></p>
                </div>
                


            </div>

            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        var v_id = xyf_util.getQuery(me.url, "v_id", true);


                        $.post("/Wxhb/Handlers/UserHandler.ashx", { action: "GET_USER" }, function (result) {


                                if (!result.success) {


                                    $.toast(result.error_msg);

                                    return;

                                }

     
                                var data = result.data;

                                obj.my_vue = new Vue({
                                    el: el.find(".list-block")[0],
                                    data: {
                                        user_obj: {
                                            //经度
                                            col_24: data.longitude,
                                            //纬度
                                            col_25: data.latitude,
                                            //商家姓名
                                            col_23: '',
                                            //商家电话
                                            col_20: '',
                                            col_27: ''


                                        }
                                    }
                                });

                                //获取实际地址文字的
                                var geocder;

                                var map = new AMap.Map('div_map_1', {
                                    resizeEnable: true,
                                    zoom: 11,
                                    center: [data.longitude, data.latitude]
                                });



                                AMap.service('AMap.Geocoder', function () {//回调函数
                                    //实例化Geocoder
                                    geocoder = new AMap.Geocoder({
                                        city: "010"//城市，默认：“全国”
                                    });
                                    //TODO: 使用geocoder 对象完成相关功能
                                });

                                //这里是获取真实地址的，不是经纬度了
                                geocoder.getAddress([data.longitude, data.latitude], function (status, result) {
                                    $("#div_address").text(result.regeocode.formattedAddress);

                                    obj.my_vue.user_obj.col_27 = result.regeocode.formattedAddress;

                                });


                               var marker = new AMap.Marker({
                                    position: [data.longitude, data.latitude],
                                    map: map,
                               });

                                //这是添加缩放工具条
                               AMap.plugin(['AMap.ToolBar'],
                                   function () {
                                       map.addControl(new AMap.ToolBar());
                                   });

                               //移动中事件
                               map.on("touchmove", function () {

                                   marker.setPosition(map.getCenter());
                               });
                               
                               //移动结束事件
                               map.on("touchend", function () {

                                   var aa = map.getCenter();
                                   marker.setPosition(aa);

                                   //这里是获取真实地址的，不是经纬度了
                                   geocoder.getAddress(aa, function (status, result) {
                                       $("#div_address").text(result.regeocode.formattedAddress);

                                       obj.my_vue.user_obj.col_24 = aa.lng;
                                       obj.my_vue.user_obj.col_25 = aa.lat;
                                       obj.my_vue.user_obj.col_27 = result.regeocode.formattedAddress;

                                   });


            


                               });

                               el.children(".content").show();

            
                            }, "json");

                        el.on("click", ".btn_register", function (e) {


                            $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'BUS_REGISTER', data: JSON.stringify(obj.my_vue.user_obj) }, function (result) {

                                if (!result.success) {

                                    $.toast(result.error_msg);

                                    return;

                                }


                    
                                var url = Mini2.urlAppend("/Wxhb/View/Bus/BusContent.aspx", { v_id: v_id }, true);

                                $.router.load(url);



                            }, "json");

                          


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
