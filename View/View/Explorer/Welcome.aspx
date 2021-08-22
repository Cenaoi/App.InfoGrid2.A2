<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="App.InfoGrid2.View.Explorer.Welcome" %>

<%@ Import Namespace="App.InfoGrid2.Model" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>首页</title>
    <link href="/Core/Scripts/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" />


    <link href="/res/v2/dashboard.css" rel="stylesheet" />
    
    <style type="text/css">
        dd {
            padding-top: 6px;
        }

        .bigger-230 {
            height:160px;
            padding:30px 10px 10px 10px;
            margin: 0px;
        }

        .bigger-230 .text {

            margin-top:10px;
        }

        .mi2-btn-group > a {
            border-style: none;
            text-decoration: none;
        }
    </style>

</head>
<body>
    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-2 col-md-1 sidebar">
                <ul class="nav nav-sidebar">
                    <% foreach (BIZ_C_MENU item in mainMenus)
                       {  %>
                    <li style="text-align: center;">
                        <a href="#" onclick="GetMenu(this)" data-id="<%=item.BIZ_C_MENU_ID %>">
                            <img class="tile-image big-illustration"  alt="" src="<%=item.ICO %>" />
                            <div style="padding:10px;" ><%=item.NAME %></div>
                        </a>
                    </li>
                    <%} %>
                </ul>
            </div>

          <div class="col-sm-10 col-sm-offset-1 col-md-11 col-md-offset-1 main">
                <div class="page-header">
                    欢迎您!
                </div>
                <div class="row">
                    <div class="col-md-7" style="margin-right:30px; padding:40px;">
                        <div class="mi2-btn-group row  center-block;" style="" id="MainPanel">
                           
<%--                            <a href="/app/infogrid2/view/onetable/tableandstruce.aspx?id=122&menu_id=125" 
                                title="销售出库"
                                onclick="return AddTab2(this);"
                                class="btn btn-default bigger-230 col-md-3 col-sm-4 col-xs-6">
                                <img class="tile-image big-illustration" alt="" src="/res/v2/xsd.gif" />
                                <div class="text">销售出库</div>
                            </a>--%>

                        </div>
                    </div>

                    <div class="panel panel-warning col-md-4  " style="margin-right:30px; background-color:#fcf8e3; ">
                        <div class="panel-body row" id="RightPanel">
<%--                            <dl class="col-sm-6 col-sm-6 col-xs-12">
                                <dt>标题1</dt>
                                <dd><a href="/app/infogrid2/view/onetable/tableandstruce.aspx?id=122&menu_id=125" 
                                    title="销售出库" onclick="return AddTab2(this)">进销存</a></dd>
                                <dd><a href="#">进销存</a></dd>
                                <dd><a href="#">进销存</a></dd>
                                <dd><a href="#">进销存</a></dd>
                            </dl>--%>
                        </div>
                    </div>
                </div>


            </div>

        </div>
    </div>

    <script src="/Core/Scripts/jquery/jquery-2.0.3.min.js"></script>
    <script src="/Core/Scripts/bootstrap/3.3.4/js/bootstrap.min.js"></script>

    <script src="/Core/Scripts/Mini/1.1/MiniHtml.min.js"></script>

    <script>
        
        $(document).ready(function () {

            GetMenu2(<%= CurMenuId %>);

        });


        function AddTab2(sender) {

            var url = $(sender).attr("href");
            var label = $(sender).attr("title");

            if (url == "#" || url == "") {
                return false;
            }


            EcView.show(url, label);

            return false;
        }

        //获取下面的子菜单
        function GetMenu(me) {
            //拿到自身的菜单ID
            var id = $(me).attr("data-id");


            GetMenu2(id);
        }

        function GetMenu2(id) {

            //这是中间快捷菜单的路径
            var mainUrl = "/App/InfoGrid2/View/Explorer/WelcomeMenu.aspx";

            //这是右边明细菜单的路径
            var rightUrl = "/App/InfoGrid2/View/Explorer/WelcomeMenu.aspx";


            $.get(mainUrl, { id: id, action: "main", rand: GetRandomNum(1, 10000) }, function (reuslt) {

                $("#MainPanel").html(reuslt);

            });

            $.get(rightUrl, { id: id, action: "right", rand: GetRandomNum(1, 10000) }, function (reuslt) {

                $("#RightPanel").html(reuslt);

            });


        }

        //获取随机数
        function GetRandomNum(Min,Max)
        {   
            var Range = Max - Min;   
            var Rand = Math.random();   
            return(Min + Math.round(Rand * Range));   
        }   
    </script>
</body>
</html>
