﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="App.InfoGrid2.Mall.View.Welcome" %>

<!DOCTYPE html>

<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>欢迎界面</title>
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <script src="/Core/Scripts/Zepto/zepto.1.2.min.js"></script>
    <script src="/Core/Scripts/XYF/xyfUtil.js"></script>
    <style type="text/css">
       .spinner {
  margin: 100px auto 0;
  width: 150px;
  text-align: center;
}
 
.spinner > div {
  width: 30px;
  height: 30px;
  background-color: #FFF;
 
  border-radius: 100%;
  display: inline-block;
  -webkit-animation: bouncedelay 1.4s infinite ease-in-out;
  animation: bouncedelay 1.4s infinite ease-in-out;
  /* Prevent first frame from flickering when animation starts */
  -webkit-animation-fill-mode: both;
  animation-fill-mode: both;
}
 
.spinner .bounce1 {
  -webkit-animation-delay: -0.32s;
  animation-delay: -0.32s;
}
 
.spinner .bounce2 {
  -webkit-animation-delay: -0.16s;
  animation-delay: -0.16s;
}
 
@-webkit-keyframes bouncedelay {
  0%, 80%, 100% { -webkit-transform: scale(0.0) }
  40% { -webkit-transform: scale(1.0) }
}
 
@keyframes bouncedelay {
  0%, 80%, 100% {
    transform: scale(0.0);
    -webkit-transform: scale(0.0);
  } 40% {
    transform: scale(1.0);
    -webkit-transform: scale(1.0);
  }
}        

    </style>

</head>
<body style="background-color:#1bbc9b; color:#FFF;">

    <div style="text-align:center; margin-top:20%;">

        <h2>微信商城</h2>
        <h3>加载...</h3>
        <div class="spinner">
          <div class="bounce1"></div>
          <div class="bounce2"></div>
          <div class="bounce3"></div>
        </div>
    </div>

</body>

</html>


<script>

    $(document).ready(function () {

        var v_id = xyf_util.getQuery(location.href, "v_id", true);


        xyf_util.post("/App/InfoGrid2/Mall/Handlers/Login.ashx", "WELCOME", { v_id: v_id }, function (data) {

            location.replace("/App/InfoGrid2/Mall/View/Home.aspx?" + Math.random());

        }, function (result) {

            var data = result.data;

            var url = "/App/InfoGrid2/Mall/View/Login.aspx?id=" + data.user_id + "&" + Math.random();

            location.replace(url);

        });

    });

</script>





