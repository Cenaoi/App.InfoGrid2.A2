<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentContent.aspx.cs" Inherits="App.InfoGrid2.JF.View.User.AgentContent" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />

    <link href="/Core/Scripts/SUI/swiper-3.4.0.min.css" rel="stylesheet" />

    <title>代理中心</title>

</head>
<body>
    


      <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">


            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">我的分销中心</h1>
            </header>


            <div class="content">

                <div class="list-block" style="margin-top: 0rem;" v-show="agent_users.length !== 0">
                    <ul>
                        <li class="list-group-title" style="color:black;">我的分销员</li>
                        <li v-for="a in agent_users">
                            <a class="item-content">
                                <div class="item-inner">
                                    <div class="item-title">{{a.name}}</div>
                                </div>
                            </a>
                        </li>
                    </ul>
                </div>

                <!-- 这是没有数据时显示的 -->
                <div class="text-center" v-show="agent_users.length === 0">
                    <h3>您还没有代理用户</h3>
                </div>

            </div>

            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        
                        
                        obj.initVue(me);
        

                    }

                    obj.agent_users = <%= GetAgentUsersObj() %>;

                    obj.my_vue = null;

                    //初始化Vue相关对象
                    obj.initVue = function(me){

                        var el = me.el;

                        obj.my_vue = new Vue({
                            el:el.children(".content")[0],
                            data:{
                                agent_users:obj.agent_users
                            }

                        })
                        
                    }


                    return obj;

                }

            </script>



        </div>

    </div>


    <script src="/Core/Scripts/m5/M5.min.js"></script>

    <script src="/Core/Scripts/vue/vue-2.0.1.js"></script>

    <script src="/Core/Scripts/SUI/swiper-3.4.0.min.js"></script>

</body>
</html>

<script>



    $(function () {

        //FastClick.attach(document.body);

        $("body").height($(window).height());

        $.router = Mini2.create('Mini2.ui.PageRoute', {});

    });


</script>

