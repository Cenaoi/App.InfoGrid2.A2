﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EcBalance.aspx.cs" Inherits="App.InfoGrid2.Mobile_V1.Person.EcBalance" %>

<%@ Register Src="~/Mobile_V1/SiteFooter.ascx" TagPrefix="uc1" TagName="SiteFooter" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>余额列表</title>

    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />

</head>
<body>
    

     <div class="page-group">

        <div class="page">
            <!-- 底部按钮 -->
            <uc1:SiteFooter runat="server" ID="SiteFooter" ActionName="me" />

            <div class="content">
                <header class="bar bar-nav">
                    <a class="icon icon-left pull-left back" href="EcCenter.aspx"></a>
                    <h1 class='title'>充值记录</h1>
                </header>
                <div class="list-block media-list" style="margin-top: 2.2rem;">
                    <ul>
                        <li>
                            <a href="#" class="item-content">
                                <div class="item-inner">
                                    <div class="item-title-row">
                                        <div class="item-title">充值10元</div>
                                        <div class="item-after">2016-01-01</div>
                                    </div>
                                    <div class="item-text">余额：￥500</div>
                                </div>
                            </a>
                        </li>

                        <li>
                            <a href="#" class="item-content">
                                <div class="item-inner">
                                    <div class="item-title-row">
                                        <div class="item-title">充值20元</div>
                                        <div class="item-after">2016-01-02</div>
                                    </div>
                                    <div class="item-text">余额：￥520</div>
                                </div>
                            </a>
                        </li>

                        <li>
                            <a href="#" class="item-content">
                                <div class="item-inner">
                                    <div class="item-title-row">
                                        <div class="item-title">充值30元</div>
                                        <div class="item-after">2016-01-03</div>
                                    </div>
                                    <div class="item-text">余额：￥550</div>
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

