<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EcEditUser.aspx.cs" Inherits="App.InfoGrid2.Mobile_V1.Person.EcEditUser" %>

<%@ Register Src="~/Mobile_V1/SiteFooter.ascx" TagPrefix="uc1" TagName="SiteFooter" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>编辑用户信息</title>

    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />

</head>
<body>


    <div class="page-group">
        <div class="page">
             <header class="bar bar-nav">
                    <a class="icon icon-left pull-left back"></a>
                    <h1 class='title'>个人信息修改</h1>
             </header>

             <!-- 底部按钮 -->
            <uc1:SiteFooter runat="server" ID="SiteFooter" ActionName="me" />

            <div class="content">
           
                <div class="content">
                    <div class="list-block">
                        <ul>
                            <!-- Text inputs -->
                            <li>
                                <div class="item-content">
                                    <div class="item-media"><i class="icon icon-form-name"></i></div>
                                    <div class="item-inner">
                                        <div class="item-title label">昵称</div>
                                        <div class="item-input">

                                            <input type="text" />

                                        
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div class="item-content">
                                    <div class="item-media"><i class="icon icon-form-email"></i></div>
                                    <div class="item-inner">
                                        <div class="item-title label">邮箱</div>
                                        <div class="item-input">
                                            <input type="email" />
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div class="item-content">
                                    <div class="item-media"><i class="icon icon-form-password"></i></div>
                                    <div class="item-inner">
                                        <div class="item-title label">密码</div>
                                        <div class="item-input">
                                            <input type="password"/>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div class="item-content">
                                    <div class="item-media"><i class="icon icon-form-gender"></i></div>
                                    <div class="item-inner">
                                        <div class="item-title label">性别</div>
                                        <div class="item-input">
                                            <select>
                                                <option>男</option>
                                                <option>女</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <!-- Date -->
                            <li>
                                <div class="item-content">
                                    <div class="item-media"><i class="icon icon-form-calendar"></i></div>
                                    <div class="item-inner">
                                        <div class="item-title label">生日</div>
                                        <div class="item-input">
                                            <input type="date" value="2016-01-01" />
                                        </div>
                                    </div>
                                </div>
                            </li>
                             <li>

                                <a class="item-content item-link external" href="/Mobile/Person/EcAdderssList.aspx">
                                    <div class="item-inner">
                                        <div class="item-title">收货地址</div>
                                    </div>
                                </a>
                            </li>
                      
                        </ul>
                    </div>
                    <div class="content-block">
                        <div class="row">
                            <div class="col-100"><a href="#" class="button button-big button-fill button-success">保存</a></div>
                        </div>
                    </div>
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

