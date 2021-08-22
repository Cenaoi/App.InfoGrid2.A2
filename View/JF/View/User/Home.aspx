<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="App.InfoGrid2.JF.View.User.Home" %>

<%@ Register Src="~/JF/View/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>

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

    <title>用户中心</title>
</head>
<body>


      <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">


            <uc1:Footer runat="server" id="Footer" ActionName="me" />

            <div class="content">
                <div style="width: 100%; height: 40%; overflow: hidden; background-size: cover; background-image: url(/mobile/res/image/demo/person_center_bg.jpg)">
                    <div style="height: 3rem;"></div>
                    <div style="width: 4rem; height: 4rem; margin-left: auto; margin-right: auto;">
                        <img :src="user.head_img_url" style="width: 100%; height: 100%;" />
                    </div>
                </div>

                <div class="list-block" style="margin-top: 0rem;">
                    <ul>
                        <li>
                            <a class="item-content item-link" href="EditUser.aspx">
                                <div class="item-inner">
                                    <div class="item-title">修改个人信息</div>
                                </div>
                            </a>
                        </li>
                         <li v-if="user.is_distr">
                            <a class="item-content item-link" href="QRCode.aspx">
                                <div class="item-inner">
                                    <div class="item-title">专属二维码</div>
                                </div>
                            </a>
                        </li>
                        <li v-if="user.is_distr">
                            <a class="item-content item-link" href="AgentContent.aspx">
                                <div class="item-inner">
                                    <div class="item-title">我的分销中心</div>
                                </div>
                            </a>
                        </li>
                    </ul>
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


                    //用户数据
                    obj.user = <%= GetUserObj() %>;

                    //Vue对象
                    obj.my_vue = null;

                    //初始化Vue相关对象
                    obj.initVue = function(me){

                        var el = me.el;

                        obj.my_vue = new Vue({
                            el:el.children(".content")[0],
                            data:{
                                user:obj.user
                            }

                        });




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
