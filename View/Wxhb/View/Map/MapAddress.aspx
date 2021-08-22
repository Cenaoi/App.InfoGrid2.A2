<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapAddress.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.Map.MapAddress" %>

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
<body>

     <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title def_title">地址</h1>
            </header>

            <div class="content">

                 <!-- 地图节点 -->
                <div id="div_map" style="height:75%;width:100%;">


                </div>

                <div id="div_address" style="margin-top:0.5rem;">

                </div>

                <div class="content-block" style="margin-top:0.5rem;">
                    <p><a href="#" class="button button-round button-fill button-big">确定</a></p>
                </div>

            </div>

            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;
                        
                        //纬度
                        var lat = xyf_util.getQuery(me.url, "lat", true);
                        //经度
                        var lon = xyf_util.getQuery(me.url, "lon", true);
                        //真是地址
                        var address = xyf_util.getQuery(me.url, "addr", true);
                        //到底要执行那个函数
                        var action = xyf_util.getQuery(me.url, "action", true);

                        var row_id = xyf_util.getQuery(me.url, "row_id", true);

                        var div_address_el = el.find("#div_address");

                        div_address_el.text(address);

                        obj.map = new AMap.Map('div_map', {
                            resizeEnable: true,
                            zoom: 16,
                            center: [lon, lat]
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

                        AMap.service('AMap.Geocoder', function () {//回调函数
                            //实例化Geocoder
                            obj.geocder = new AMap.Geocoder({
                                city: "010"//城市，默认：“全国”
                            });
                        });

                        var marker = new AMap.Marker({
                            position: [lon, lat],
                            map: obj.map,
                        });

                        //移动中事件
                        obj.map.on("touchmove", function () {

                            marker.setPosition(obj.map.getCenter());
                        });

                        //移动结束事件
                        obj.map.on("touchend", function () {

                            var aa = obj.map.getCenter();
                            marker.setPosition(aa);

                            //这里是获取真实地址的，不是经纬度了
                            obj.geocder.getAddress(aa, function (status, result) {


                                div_address_el.text(result.regeocode.formattedAddress);

                                lon = aa.lng;
                                lat = aa.lat;
                                address = result.regeocode.formattedAddress;
                            });


               

                        });

                        //确定按钮点击事件
                        el.on("click", ".button-big", function (e) {


                            console.log("点击了确定按钮！");

                            console.log("lat:", lat);
                            console.log("lon:", lon);
                            console.log("address:", address);


                            $.post("/Wxhb/Handlers/MapHandler.ashx", { action: action, lat: lat, lon: lon, address: address, row_id: row_id }, function (result) {

                                if (!result.success) {

                                    $.alert(result.error_msg);

                                    return;
                                }

                                $.toast(result.msg);

                                setTimeout(function () {

                                    $.router.back();

                                }, 0.5 * 1000);


                            }, "json");
                                    

                          
                        });

                    }

                    //地图对象
                    obj.map = null;

                    //地理位置对象
                    obj.geolocation = null;

                    //获取真实地址用的
                    obj.geocder = null;


                    return obj;

                }

            </script>


        </div>

    </div>

</body>
</html>
