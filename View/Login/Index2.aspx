<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index2.aspx.cs" Inherits="App.InfoGrid2.Login.Index2" %>


<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title><%= GetCompanyName() %></title>
    <meta name="author" content="DeathGhost" />
    
    <link href="/res/login2/css/style.css" rel="stylesheet" />
    <style>
        body {
            height: 100%;
            background: #16a085;
            overflow: hidden;
        }

        canvas {
            z-index: -1;
            position: absolute;
        }

        .admin_login dt {
            color: #C4ECE5;
        }
    </style>

    
</head>
<body>
    <div id="builderId" style="width:20px;height:20px; margin-left:auto; margin-right:0px;">
            
    </div>

    <dl class="admin_login">
        <dt>
            <strong><%= GetCompanyName() %></strong>
            <em>Management System</em>
        </dt>

        <dd class="user_icon">
            <input type="text" placeholder="账号"  v-model="login_name"  class="login_txtbx" />
        </dd>
        <dd class="pwd_icon">
            <input type="password" placeholder="密码"  v-model="login_pass"  class="login_txtbx" />
        </dd>
        <dd>
            <input type="button" value="立即登陆" class="submit_btn" @click="login_btn" />
        </dd>
        <dd>
            <p>适用浏览器：360、FireFox、Chrome、Safari、Opera、傲游、搜狗、世界之窗. 不支持IE8及以下浏览器。</p>
        </dd>
    </dl>

    <script src="/Core/Scripts/jquery/jquery-3.1.0.min.js"></script>
    <script src="/Core/Scripts/vue/vue.js"></script>

    <script src="/res/login2/js/verificationNumbers.js"></script>
    <script src="/res/login2/js/Particleground.js"></script>

    <script src="/Core/Scripts/Mini2/Mini2.min.js"></script>

</body>
</html>

<script>
    $(document).ready(function () {

        $('#builderId').click(function () {
            window.location.href = "builder2.aspx";
        });


        //粒子背景特效
        $('body').particleground({
            dotColor: '#5cbdaa',
            lineColor: '#5cbdaa'
        });


        var login_vue = new Vue({
            el: '.admin_login',
            data: {
                login_name: '',
                login_pass:'',
                login_domain:''
            },
            methods: {
                login_btn: function () {

                    var me = this;


                    if (me.login_name.length == 0) {
                        alert('请填写账号信息！');
                        return;
                    }

                    if (me.login_pass.length == 0) {

                        alert('请填写账号密码！');
                        return;
                    }

                    Mini2.post("/App/InfoGrid2/Login/LoginHandle.aspx", {
                        action: "Login",
                        LoginName: me.login_name,
                        LoginPass: me.login_pass

                    }, function (data) {
                        location.replace(data);
                    }, function (err) { alert(err.msg) });
                }


            }

        });

        $(window).keydown(function (e) {

            if (e && e.keyCode == 13) {

                login_vue.login_btn();
            }

        });

      
    });
</script>

