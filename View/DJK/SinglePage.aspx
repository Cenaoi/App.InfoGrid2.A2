<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SinglePage.aspx.cs" Inherits="App.InfoGrid2.DJK.SinglePage" %>

<!DOCTYPE html>

<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title></title>

    <style>

        .plug-menu img.open {-webkit-transform: rotate(135deg);-webkit-transition: -webkit-transform 200ms;}

        .plug-menu img.close {-webkit-transform: rotate(0deg);-webkit-transition: -webkit-transform 200ms;}


    </style>

</head>
<body >
    
 

    <div style="background-color:red;width:40px;height:40px;border-radius:20px;" class="plug-menu" >

        <img id="img_test" src="http://2040.wangzhan31.com/App/Tpl/Wap/Default/Public/images/system/plugmenu.png" style="width:100%;height:100%;" />

    </div>



    <script src="/Core/Scripts/m5/M5.min.js"></script>

</body>
</html>

<script>


    $(document).ready(function () {

        var imgs = $("img");

        var body_width = $(document.body).width();

        //设置img的宽度不能超过body的宽度
        imgs.forEach(function (v, i) {


            var img_el = $(v);


            if (img_el.width() > body_width) {

                img_el.width(body_width);

            }


        });


    });





</script>

