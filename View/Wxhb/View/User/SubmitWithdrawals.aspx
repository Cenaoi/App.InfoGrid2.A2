<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubmitWithdrawals.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.User.SubmitWithdrawals" %>

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
<body  style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">
    
    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            
            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">提现</h1>
            </header>

            <div class="content">

                <div class="list-block">
                    <ul>
                      <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">提现金额</div>
                            <div class="item-input">
                              <input type="text" id="withd_money" placeholder="请输入要提现的金额">
                            </div>
                          </div>
                        </div>
                      </li>
                      <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">当前金额</div>
                            <div class="item-input">
                              <input type="text" readonly="readonly" value="100">
                            </div>
                          </div>
                        </div>
                      </li>
                    </ul>
                </div>

                <div class="content-block">
                     <p><a href="#" class="button button-fill button-big">提交 </a></p>
                </div>

            </div>

            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        
                        el.on("click", ".button-big", function (e) {

                            var withd_money = $("#withd_money").val();

                            if (withd_money < 1) {

                                $.alert("提现金额不能小于1元");

                                return;
                            }

                            $.post("/Wxhb/Handlers/UserHandler.ashx", { action: 'SUBMIT_WITHDRAWALS', withd_money: withd_money }, function (result) {


                                if (!result.success) {

                                    $.alert(result.error_msg);

                                    return;
                                }

                                $.toast(result.msg);

                                setTimeout(function () {

                                    $.router.back();

                                }, 0.5 * 1000);


                            }, "json");

                        });

                    }

                    return obj;

                }

            </script>

        </div>

    </div>

</body>
</html>
