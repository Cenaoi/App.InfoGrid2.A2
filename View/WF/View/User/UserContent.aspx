<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserContent.aspx.cs" Inherits="App.InfoGrid2.WF.View.User.UserContent" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>流程审批</title>

    <script src="/WF/Script/common.js"></script>

</head>
<body  style="max-width:640px; margin:auto; font-family: Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">



     <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

             <nav class="bar bar-tab">
              <a class="tab-item external " href="/WF/View/Home.aspx">
                <span class="icon icon-home"></span>
                <span class="tab-label">流程</span>
              </a>
              <a class="tab-item external active" href="#">
                <span class="icon icon-me"></span>
                <span class="tab-label">我的</span>
              </a>
            </nav>


            <div class="content">

                <div style="margin:0px;" class="list-block media-list">
                    <ul>
                        <li>
                            <div style="padding-left:0.5rem;" class="item-content ">
                                <div class="item-media">
                                    <img src="/mobile/res/image/demo/person_center_face.png" style="width: 2.5rem;">
                                </div> 
                                <div class="item-inner">
                                    <div class="item-title-row">
                                        <div class="item-title">{{user_obj.COL_2}}</div>
                                        <div class="item-after"></div>
                                    </div> 
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>

                <div class="list-block media-list">
                    <ul>
                         <li class="list-group-title" style="background-color:white;color:#0894ec;border-bottom:1px solid rgb(207, 207, 210);">企业信息</li>
                        <li>
                            <div class="item-content">
                                <div class="item-inner">
                                    <div class="item-subtitle" style="font-size:0.5rem;color:#a59595;">手机号</div>
                                    <div class="item-text" style="font-weight:bold;color:#0e0e13;height:auto;">{{user_obj.COL_6}}</div>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="item-content">
                                <div class="item-inner">
                                    <div class="item-subtitle" style="font-size:0.5rem;color:#a59595;">所属区域</div>
                                    <div class="item-text" style="font-weight:bold;color:#0e0e13;height:auto;">{{user_obj.COL_15}}</div>
                                </div>
                            </div>
                        </li>
                         <li>
                            <div class="item-content">
                                <div class="item-inner">
                                    <div class="item-subtitle" style="font-size:0.5rem;color:#a59595;">部门</div>
                                    <div class="item-text" style="font-weight:bold;color:#0e0e13;height:auto;">{{user_obj.COL_12}}</div>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="item-content">
                                <div class="item-inner">
                                    <div class="item-subtitle" style="font-size:0.5rem;color:#a59595;">职位</div>
                                    <div class="item-text" style="font-weight:bold;color:#0e0e13;height:auto;">{{user_obj.COL_11}}</div>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>


                <div class="content-block">
                    <div class="row">
                        <div class="col-100"><a href="#" class="button button-big button-fill button-danger">退出系统</a></div>
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

                        var v_id = xyf_util.getQuery(me.url,"v_id",true);

                        //退出系统按钮点击事件
                        el.on("click", ".button-danger", function (e) {

                            $.confirm("注销用户后解除微信号与系统的关联，将无法接收到来之系统审批、抄送等信息！","警告",function(){
                            
                                xyf_util.post("/WF/Handlers/LoginV1.ashx","LOGOUT",{},function(data){
                                
                                    location.href = Mini2.urlAppend("/WF/View/Login.aspx",{v_id:v_id},true);

                                });

                            });
                        });


                        obj.initVue(me);

                    }

                    obj.user_obj = <%= GetUserInfo() %>;

                    //vue对象
                    obj.my_vue = null;

                    //初始化vue相关对象
                    obj.initVue = function (me) {

                        var el = me.el;

                        obj.my_vue = new Vue({

                            el: el.children(".content")[0],
                            data: {
                                user_obj:obj.user_obj
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






