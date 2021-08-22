<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WithdrawalsContent.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.User.WithdrawalsContent" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>微信红包</title>

    <script src="/Wxhb/Script/common.js?v=20170209001"></script>

</head>
<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">
    
    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">提现列表</h1>
                <a class="icon icon-edit pull-right"></a>
            </header>


            <div class="content">

                    
                <my-list :list_data="withds"></my-list>


            </div>


            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;


                        $.post("/Wxhb/Handlers/UserHandler.ashx", { action: 'GET_WITHDS' }, function (result) {

                            if (!result.success) {
                                
                                $.alert(result.error_msg);

                                return;

                            }

                            obj.my_vue = new Vue({
                                el: el.children(".content")[0],
                                data: {
                                    withds:result.data
                                }
                            });

                        }, "json");

                        //右上角的新建按钮
                        el.on("click", ".pull-right", function (e) {

                            var url = Mini2.urlAppend("SubmitWithdrawals.aspx", {}, true);

                            $.router.load(url);

                        });

                    }




                    //页面恢复的事件
                    obj.pageRevert = function () {

                        console.log("其它页面返回来的");


                        $.post("/Wxhb/Handlers/UserHandler.ashx", { action: 'GET_WITHDS' }, function (result) {

                            if (!result.success) {

                                $.alert(result.error_msg);

                                return;

                            }

                  
                                
                            obj.my_vue.withds = result.data;


                        }, "json");

                      
                    }

                    //Vue对象
                    obj.my_vue = null;

                    return obj;

                }

            </script>


        </div>

    </div>

</body>
</html>
