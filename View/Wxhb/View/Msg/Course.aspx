<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Course.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.Msg.Course" %>

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
                <h1 class="title def_title">使用说明</h1>
            </header>


            <div class="content">

                <h2>1、如何抢身边的红包</h2>
                <p>打开红包地图，点击红色的红包就可以抢了。</p>
                <img  src="/Wxhb/res/如何在红包地图抢红包.gif" style="width:100%;" />

                <p>显示为灰色的红包，是不能抢的。原因可能是红包已经派完，或者是商家设置了红包的发放范围，您必须要在红包附近才能抢。</p>


            </div>


            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        
                        

        

                    }


                    return obj;

                }

            </script>


        </div>

    </div>



</body>
</html>
