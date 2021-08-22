<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="App.InfoGrid2.JF.View.User.EditUser" %>

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

    <title>修改用户信息</title>

</head>
<body>


     <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">


            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">编辑用户</h1>
            </header>


            <div class="content" >

                <div class="list-block" style="margin-top:0px;">
                    <ul>
                            <li>
                                <div class="item-content">
                                    <div class="item-media"><i class="icon icon-form-name"></i></div>
                                    <div class="item-inner">
                                        <div class="item-title label">昵称：</div>
                                        <div class="item-input">
                                            <input type="text" v-model="user.w_nickname" />
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div class="item-content">
                                    <div class="item-media"><i class="icon icon-form-name"></i></div>
                                    <div class="item-inner">
                                        <div class="item-title label">性别：</div>
                                        <div class="item-input">
                                            <select v-model="user.sex" >
                                                <option>男</option>
                                                <option>女</option>
                                            </select> 
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div class="item-content">
                                    <div class="item-media"><i class="icon icon-form-name"></i></div>
                                    <div class="item-inner">
                                        <div class="item-title label">联系人：</div>
                                        <div class="item-input">
                                             <input type="text" v-model="user.contacter_name" />
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div class="item-content">
                                    <div class="item-media"><i class="icon icon-form-name"></i></div>
                                    <div class="item-inner">
                                        <div class="item-title label">联系电话：</div>
                                        <div class="item-input">
                                                <input type="text" v-model="user.contacter_tel" />
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div class="item-content">
                                    <div class="item-media"><i class="icon icon-form-name"></i></div>
                                    <div class="item-inner">
                                        <div class="item-title label">详细地址：</div>
                                        <div class="item-input">
                                            <input type="text" v-model="user.address_t2" />
                                        </div>
                                    </div>
                                </div>
                            </li>
                            
                        </ul>
                </div>

                <div class="content-block">
                    <div class="row">
                        <div class="col-100"><a href="#" class="button button-big button-fill button-success" @click="edit_user">保存</a></div>
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

                        obj.initVue(me);

       
                    }


                    obj.user = <%= GetUserObj() %>;

                    //Vue对象
                    obj.my_vue = null;

                    //初始化Vue相关对象
                    obj.initVue = function (me) {

                        var el = me.el;


                        obj.my_vue = new Vue({
                            el:el.children(".content")[0],
                            data:{
                                user:obj.user
                            },
                            methods:{
                                //保存按钮事件
                                edit_user:function(){

                                    var my_vue = this;


                                    $.post("/JF/Handlers/UserHandler.ashx",{action:'EDIT_USER',json_data:JSON.stringify(my_vue.user)},function(result){
                                    

                                        if(!result.success){

                                            $.toast(result.msg);
                                            return;

                                        }


                                        $.toast("保存成功了！");


                                        
                                        $.router.back();

                                        console.log("执行了返回界面");

                                    
                                    },"json");




                                }

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


