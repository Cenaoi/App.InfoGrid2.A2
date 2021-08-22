.<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EcSearch.aspx.cs" Inherits="App.InfoGrid2.Mobile_V1.EcSearch" %>

<%@ Register Src="~/Mobile_V1/SiteFooter.ascx" TagPrefix="uc1" TagName="SiteFooter" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>搜索</title>

    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />

    <link href="/Core/Scripts/SUI/swiper-3.4.0.min.css" rel="stylesheet" />

</head>
<body>


      <div class="page-group">


        <div class="page">
            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back" href="EcHome.aspx"></a>
                <h1 class="title">搜索</h1>
            </header>

            <!-- 搜索框 -->
            <div class="bar bar-header-secondary">
                <div class="searchbar">
                    <a class="searchbar-cancel">取消</a>
                    <div class="search-input">
                        <label class="icon icon-search" for="search"></label>
                        <input type="search" id='search' placeholder='输入关键字...' />
                    </div>
                </div>
            </div>

            <!-- 底部按钮 -->
            <uc1:SiteFooter runat="server" ID="SiteFooter" />

            <div class="content">
                <!-- 这里是页面内容区 -->


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




