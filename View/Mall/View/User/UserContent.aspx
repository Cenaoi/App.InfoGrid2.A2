<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserContent.aspx.cs" Inherits="App.InfoGrid2.Mall.View.User.UserContent" %>

<%@ Register Src="~/Mall/View/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <script src="/Core/Scripts/XYF/common.js"></script>

    <title>致瑧服饰商城</title>

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
                        <img :src="user.HEAD_IMG_URL" style="width: 100%; height: 100%;" />
                    </div>
                </div>

                <div class="list-block" style="margin-top: 0rem;">
                    <ul>
                        <li>
                            <a class="item-content item-link">
                                <div class="item-inner">
                                    <div class="item-title">修改个人信息</div>
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

                        xyf_util.post("/App/InfoGrid2/Mall/Handlers/User.ashx", "GET_WX_USER", {}, function (data) {

                            obj.my_vue.user = data;

                        });

                    }
                    


                    //Vue对象
                    obj.my_vue = null;

                    //初始化Vue相关对象
                    obj.initVue = function(me){

                        var el = me.el;

                        obj.my_vue = new Vue({
                            el:el.children(".content")[0],
                            data:{
                                user: {}
                            }

                        });




                    }

                    return obj;

                }

            </script>

        </div>

    </div>

</body>
</html>
