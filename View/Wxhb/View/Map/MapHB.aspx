<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapHB.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.Map.MapHB" %>

<!DOCTYPE html>

<html>
<head>

    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>地图红包</title>

    <script src="/Wxhb/Script/common.js"></script>

</head>
<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">

     <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">


            <div class="content" style="display:none;">

                <!-- 地图节点 -->
                <div id="div_map" style="height:100%;width:100%;">


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

                        
                        $.post("/Wxhb/Handlers/LoginHandler.ashx", { action: 'AUTO_LOGIN', v_id: v_id }, function () {

                        
                            me.getGeolocation(me);


                        },"json");



                    }

                    //获取位置信息的
                    obj.getGeolocation = function (me) {

                        var el = me.el;



                            obj.map = new AMap.Map('div_map', {
                                resizeEnable: true,
                                zoom: 16,
                                center: [116.397428, 39.90923]
                            });

                            //这是添加缩放工具条
                            AMap.plugin(['AMap.ToolBar'],
                                function () {
                                    obj.map.addControl(new AMap.ToolBar());
                                });

                            AMap.plugin('AMap.Geolocation', function () {

                                obj.geolocation = new AMap.Geolocation({
                                    enableHighAccuracy: true,//是否使用高精度定位，默认:true
                                    timeout: 10000,          //超过10秒后停止定位，默认：无穷大
                                    maximumAge: 0,           //定位结果缓存0毫秒，默认：0
                                    convert: true,           //自动偏移坐标，偏移后的坐标为高德坐标，默认：true
                                    showButton: true,        //显示定位按钮，默认：true
                                    buttonPosition: 'LB',    //定位按钮停靠位置，默认：'LB'，左下角
                                    buttonOffset: new AMap.Pixel(10, 20),//定位按钮与设置的停靠位置的偏移量，默认：Pixel(10, 20)
                                    showMarker: true,        //定位成功后在定位到的位置显示点标记，默认：true
                                    showCircle: true,        //定位成功后用圆圈表示定位精度范围，默认：true
                                    panToLocation: true,     //定位成功后将定位到的位置作为地图中心点，默认：true
                                    zoomToAccuracy: true      //定位成功后调整地图视野范围使定位位置及精度范围视野内可见，默认：false
                                });


                                obj.map.addControl(obj.geolocation);

                            });

                           
                            $.post("/Wxhb/Handlers/UserHandler.ashx", { action: "GET_USER"}, function (result) {


                                if (!result.success) {


                                    $.toast(result.error_msg);

                                    return;

                                }


                                var user_obj = result.data;
  
                                obj.map.setCenter([user_obj.longitude, user_obj.latitude]);

                                el.children(".content").show();

                                //var icon = new AMap.Icon({
                                //    image: '/Wxhb/res/HB_001.png',//24px*24px
                                //    //icon可缺省，缺省时为默认的蓝色水滴图标，
                                //    imageSize: new AMap.Size(36, 36)
                                //});

                                //var marker = new AMap.Marker({
                                //    icon: icon,
                                //    position: [113.40658665, 23.16912513],
                                //    map: map
                                //});

                                //var marker2 = new AMap.Marker({
                                //    icon: icon2,
                                //    position: [113.41319561, 23.16269395],
                                //    map: map
                                //});

                                //var marker3 = new AMap.Marker({
                                //    icon: icon3,
                                //    position: [113.41032028, 23.16600821],
                                //    map: map
                                //});


                                ////这是绑定标记的点击事件
                                //AMap.event.addListener(marker, 'click', function () {

                                //    $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'NEW_ADVE' }, function (result) {

                                //        var url = Mini2.urlAppend("/Wxhb/View/Bus/ShowAdve.aspx", { adve_id: result.data.adve_id }, true);

                                //        $.router.load(url);

                                //    }, "json");

                                //});

                                ////这是绑定标记的点击事件
                                //AMap.event.addListener(marker2, 'click', function () {

                                //    $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'NEW_ADVE' }, function (result) {

                                //        var url = Mini2.urlAppend("/Wxhb/View/Bus/ShowAdve.aspx", { adve_id: result.data.adve_id }, true);

                                //        $.router.load(url);

                                //    }, "json");

                                //});

                                ////这是绑定标记的点击事件
                                //AMap.event.addListener(marker3, 'click', function () {

                                //    $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'NEW_ADVE' }, function (result) {

                                //        var url = Mini2.urlAppend("/Wxhb/View/Bus/ShowAdve.aspx", { adve_id: result.data.adve_id }, true);

                                //        $.router.load(url);

                                //    }, "json");

                                //});

                            }, "json");


                            $.post("/Wxhb/Handlers/MapHandler.ashx", { action: 'GET_ALL_HB_ADDRESS' }, function (result) {


                                if (!result.success) {


                                    $.toast(result.error_msg);

                                    return;

                                }

                                var icon = new AMap.Icon({
                                    image: '/Wxhb/res/HB_001.png',//24px*24px
                                    //icon可缺省，缺省时为默认的蓝色水滴图标，
                                    imageSize: new AMap.Size(36, 36)
                                });

                               result.data.forEach(function (v, i) {

                                    var marker = new AMap.Marker({
                                        icon: icon,
                                        position: [v.HB_LONGITUDE, v.HB_LATITUDE],
                                        map: obj.map
                                    });



                                   //这是绑定标记的点击事件
                                   AMap.event.addListener(marker, 'click', function () {

                                       $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'GET_ADVE_BY_CODE', adve_code: v.FK_ADVE_CODE }, function (result) {

                                           if (!result.success) {
                                               $.alert(result.error_msg);
                                               return;
                                           }

                                           var url = Mini2.urlAppend("/Wxhb/View/Bus/ShowAdve.aspx", { adve_code: result.data.ROW_IDENTITY_ID }, true);

                                           $.router.load(url);


                                       }, "json");


                                   });

                                });


                            }, "json");

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


                    //地图对象
                    obj.map = null;


                    obj.geolocation = null;

                    return obj;

                }

            </script>


        </div>

    </div>

</body>
</html>
