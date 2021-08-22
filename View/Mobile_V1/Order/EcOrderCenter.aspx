<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EcOrderCenter.aspx.cs" Inherits="App.InfoGrid2.Mobile_V1.Order.EcOrderCenter" %>

<%@ Register Src="~/Mobile_V1/SiteFooter.ascx" TagPrefix="uc1" TagName="SiteFooter" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>订单中心</title>

    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />

</head>
<body>


    
    <div class="page-group">

        <div class="page" id="es_user_order">
            <!-- 底部按钮 -->
            <uc1:SiteFooter runat="server" ID="SiteFooter" ActionName="me" />
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back" href="EcCenter.aspx"></a>
                <h1 class='title'>订单列表</h1>
            </header>
            <div class="content" id="user_order_content">

                <div class="list-block media-list" style="margin-top:0rem;">
                    <ul>
                        <li v-for="o in order_list">
                            <a href="EcOrderDetail.aspx?order_id={{o.ES_ORDER_ID}}" data-no-cache="true" class="item-content item-link">
                                <div class="item-inner">
                                    <div class="item-title-row">
                                        <div class="item-title">{{o.ORDER_NUM}}</div>
                                        <div class="item-after">￥{{o.MONEY_TOTAL}}</div>
                                    </div>
                                    <div class="item-text">{{o.BIZ_SID | biz_sid_text}}</div>
                                </div>
                            </a>
                        </li>
                    </ul>
                </div>

            </div>
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

