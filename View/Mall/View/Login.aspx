<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="App.InfoGrid2.Mall.View.Login" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>登录界面</title>

    <script src="/Core/Scripts/XYF/common.js"></script>

    <style>

    .port_bc{

            background-color:#ffc107;
            color:white;

        }

    </style>

</head>
<body>
     <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <h1 class="title def_title">登录界面</h1>
            </header>

            <div class="content">

                <div class="list-block">
                    <ul>
                      <!-- Text inputs -->
                      <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">账号</div>
                            <div class="item-input">
                              <input type="text" v-model="user_obj.login_name" placeholder="请输入你的账号">
                            </div>
                          </div>
                        </div>
                      </li>
                      <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">密码</div>
                            <div class="item-input">
                              <input type="password" placeholder="Password" v-model="user_obj.login_pass" >
                            </div>
                          </div>
                        </div>
                      </li>
                    </ul>
                </div>


                <div class="content-block" >
                    <p><a href="#" class="button button-round button-fill button-big" @click="login_btn">登录</a></p>
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



                        obj.initVue(me);
                        

                        
                      

                    }

                    obj.my_vue = null;

                    //初始化vue相关对象
                    obj.initVue = function (me) {

                        var el = me.el;

                        
                        var id = xyf_util.getQuery(me.url, "id", true);




                        obj.my_vue = new Vue({
                            el: el.find(".content")[0],
                            data: {
                                user_obj: {
                                    login_name: '',
                                    login_pass: '',
                                }
                            },
                            methods: {
                                //登录按钮点击事件
                                login_btn: function () {

                                    var my_vue = this;

                                    if ( xyf_util.isNullOrWhiteSpace(my_vue.user_obj.login_name)) {


                                        $.alert("登录账号不能為空！");

                                        return;

                                    }


                                    if (xyf_util.isNullOrWhiteSpace(my_vue.user_obj.login_pass)) {

                                        $.alert("登录密码不能為空！");
                                        return;

                                    }

                                    xyf_util.post("/App/InfoGrid2/Mall/Handlers/Login.ashx", "LOGIN_BTN", {
                                        login_name: my_vue.user_obj.login_name,
                                        login_pass: my_vue.user_obj.login_pass,
                                        id: id
                                    }, function (data,result) {

                                        $.toast(result.msg);

                                        setTimeout(function () {

                                            var url = Mini2.urlAppend("/App/InfoGrid2/Mall/View/Home.aspx", {}, true);

                                            location.replace(url);

                                        }, 0.5 * 1000);

                                    });

                                }
                            }
                        });
                    }



                    //页面恢复的事件
                    obj.pageRevert = function () {
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
