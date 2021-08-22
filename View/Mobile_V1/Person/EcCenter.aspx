<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EcCenter.aspx.cs" Inherits="App.InfoGrid2.Mobile_V1.Person.EcCenter" %>

<%@ Register Src="~/Mobile_V1/SiteFooter.ascx" TagPrefix="uc1" TagName="SiteFooter" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>用户中心</title>

    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />


</head>
<body>


    <div class="page-group">

        <div class="page">


                        <!-- 底部按钮 -->
            <uc1:SiteFooter runat="server" ID="SiteFooter" ActionName="me" />

            <div class="content">

                <div style="width: 100%; height: 40%; overflow: hidden; background-size: cover; background-image: url(/mobile/res/image/demo/person_center_bg.jpg)">
                    <div style="height: 3rem;"></div>
                    <div style="width: 4rem; height: 4rem; margin-left: auto; margin-right: auto;">
                        <img src="/mobile/res/image/demo/person_center_face.png" style="width: 100%; height: 100%;" />
                    </div>
                </div>

                <div class="list-block" style="margin-top: 0rem;">
                    <ul>
                        <li>
                            <a class="item-content item-link" href="EcEditUser.aspx">
                                <div class="item-inner">
                                    <div class="item-title">修改个人信息</div>
                                </div>
                            </a>

                        </li>
                        <li>
                            <a class="item-content item-link" href="EcIntegralList.aspx">
                                <div class="item-inner">
                                    <div class="item-title">积分中心</div>
                                </div>
                            </a>

                        </li>
                        <li>
                            <a class="item-content item-link" href="EcBalance.aspx">
                                <div class="item-inner">
                                    <div class="item-title">余额</div>
                                </div>
                            </a>

                        </li>
                        <li>
                            <a class="item-content item-link" href="EcCoupon.aspx">
                                <div class="item-inner">
                                    <div class="item-title">电子券中心</div>
                                </div>
                            </a>
                        </li>

                        <li>
                            <a class="item-content item-link" href="Mobile_V1/Order/EcOrderCenter.aspx" data-no-cache="true">
                                <div class="item-inner">
                                    <div class="item-title">订单中心</div>
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


                        var user_obj = <%= GetUserObj() %>;



        

                    }


                    return obj;

                }

            </script>



        </div>

   </div>


    <script src="/Core/Scripts/m5/M5.min.js"></script>

    <script src="/Core/Scripts/vue/vue-2.0.1.js"></script>

</body>
</html>


<script>



    $(function () {

        //FastClick.attach(document.body);

        $("body").height($(window).height());

        $.router = Mini2.create('Mini2.ui.PageRoute', {});

    });


</script>

