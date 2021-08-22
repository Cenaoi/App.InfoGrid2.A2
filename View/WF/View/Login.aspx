<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="App.InfoGrid2.WF.View.Login" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <script src="/WF/Script/common.js?v=20170710"></script>

    <title>登录界面</title>

    <style>
        .mu-icon-box {
            padding: 30px 20px 30px 20px;
            position: relative;
        }

        .mu-icon {
            font-family: '微软雅黑';
            height: 72px;
            width: 72px;
            border-radius: 4px;
            overflow: hidden;
            padding-top: 16px;
            color: white;
            font-size: 30px;
            margin-left: auto;
            margin-right: auto;
            cursor: pointer;
            text-align: center;
            
        }

        .mu-icon-text {
            font-family: '微软雅黑';
            color: white;
            font-size: 30px;
            cursor: pointer;
            pointer-events: none;
        }

        .mi-icon-label {
            overflow: hidden;
            color: #08c;
            margin-top: 5px;
            font-weight: normal;
            margin-left: auto;
            margin-right: auto;
            text-align: center;
        }

        .mi-icon-tag {
            right: 36px;
            top: 32px;
            position: absolute;
            width: 16px;
            height: 16px;
            background-image: url(/res/icon/application_view_columns.png);
        }
    </style>

</head>
<body  style="max-width:640px; margin:auto; font-family: Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


    <div class="page-group">

        <div class="page">
            <header class="bar bar-nav">
                <h1 class="title">登录</h1>
            </header>
            <div class="content" style="display:none;">


                <div style="width:100%;" >

                    <img src="/WF/res/home_013.png" style="width:100%;"  />

                </div>


                <!-- 这里是页面内容区 -->
                <div class="list-block">
                    <ul>
                        <!-- Text inputs -->
                        <li>
                            <div class="item-content">
                                <div class="item-inner">
                                    <div class="item-input ">
                                        <input type="text" id="login_name" placeholder="账号" />
                                    </div>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="item-content">
                                <div class="item-inner">
                                    <div class="item-title label">密码</div>
                                    <div class="item-input">
                                        <input type="password" id="login_pass" placeholder="Password" class="" />
                                    </div>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>

                <div class="content-block">
                    <div class="row">
                        <div class="col-50"><a href="#" class="button button-big button-fill button-danger">取消</a></div>
                        <div class="col-50"><a href="#" class="button button-big button-fill button-success">登录</a></div>
                    </div>
                </div>
            </div>

            <script type="text/javascript" data-main="true">

                function main() {


                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        var v_id = xyf_util.getQuery(me.url,"v_id",true);


                        xyf_util.post("/App/InfoGrid2/WF/Handlers/LoginV1.ashx", "AUTO_LOGIN", {v_id:v_id}, function (data) {

                            location.href = Mini2.urlAppend("/App/InfoGrid2/WF/View/Home.aspx", { v_id: v_id }, true);


                        }, function (result) {

                            el.children(".content").show();


                        });



     
                        //点击时打开图片浏览器
                        el.on('click', '.button-danger', function (e) {

                            wx.ready(function () {

                                wx.closeWindow();
                            });

                        });

                      
                        el.on("click", ".button-success", function (e) {


                            var login_name = el.find("#login_name").val();

                            var login_pass = el.find("#login_pass").val();

                            if (login_name.length === 0) {
                                $.alert("账号不能为空！");
                                return;
                            }

                            if (login_pass.length === 0) {

                                $.alert("密码不能为空！");

                                return;
                            }

                            //v_id = "72c50f86-99e4-4444-8c0c-d2ac234f76de&c4c81997-7373-99ca-6461-c9cd0695950b=";  测试用的

                            xyf_util.post("/App/InfoGrid2/WF/Handlers/LoginV1.ashx", "LOGIN", { login_name: login_name, login_pass: login_pass, v_id: v_id }, function (data) {

                                location.href =  Mini2.urlAppend("Home.aspx", {v_id:v_id}, true);

                            });


                        });

                        el.keydown(function (e) {

                            //回车键触发提交按钮事件
                            if (e.keyCode == 13) {
                                
                                el.find(".button-success").click();

                            }


                        });



                    }


                    //页面加载成功事件
                    obj.onLoad = function () {


                        xyf_util.GetWxJsConfig(false);


                    }


                    return obj;

                }

            </script>

        </div>

    </div>



</body>
</html>

