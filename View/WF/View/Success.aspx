<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="App.InfoGrid2.WF.View.Success" %>

<!DOCTYPE html>

<html>
<head>
     <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <title>成功界面</title>
    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />
</head>
<body style="max-width:640px; margin:auto; font-family: Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;"> 

    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">



            <div class="content">

                <div style="width:8rem;height:8rem;margin:auto;margin-top:10rem;">
                    <img src="/WF/res/3151.png"  style="width:100%;height:100%;"/>
                </div>
                <div style="margin:auto;font-size:2rem;text-align:center;color: green;" id="div_content">
                    
                </div>


                <div class="content-block" style="margin-top:0.5rem;">
                    <p><a href="#" class="button button-success button-fill button-big">确定</a></p>
                </div>


            </div>

            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        //跳转地址
                        var url = xyf_util.getQuery(me.url, "url", true) || encodeURIComponent("/WF/View/Home.aspx?123");
                        //显示内容
                        var text = xyf_util.getQuery(me.url, "text") || "提交成功";
                        //几秒才自动跳 默认三秒
                        var time = xyf_util.getQuery(me.url, "time") || 3;

                        //解析base64编码的url
                        url = decodeURIComponent(url);
                       
                        console.info("url:" + url);
                        console.info("text:" + text);
                        console.info("time:" + time);


                        el.find("#div_content").text(text);

                        //确定按钮点击事件
                        el.on("click", ".button-fill", function () {

                            location.href = url;

                        });

                        //延时跳转
                        setTimeout(function () {

                            location.href = url;

                        }, time * 1000);

                    }

                    return obj;

                }

            </script>


        </div>

    </div>

    <script src="/WF/Script/common.js"></script>

</body>
</html>

