 <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlowCC.aspx.cs" Inherits="App.InfoGrid2.WF.View.Flow.FlowCC" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>抄送我的界面</title>

    <script src="/WF/Script/common.js"></script>

</head>
<body style="max-width:640px; margin:auto; font-family: Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">抄送我的</h1>
            </header>

            <div class="content">
                
                <div class="buttons-tab">
                    <a href="#tab1" class="tab-link active button">未读</a>
                    <a href="#tab2" class="tab-link button">已读</a>
                </div>
                
                <div class="content-block" style="margin:0px; padding:0px;">
                    <div class="tabs">
                      <div id="tab1" class="tab active">

                         <my-list-link  :list_data="no_checks"></my-list-link>

                      </div>
                      <div id="tab2" class="tab">
                        <my-list-link  :list_data="yes_checks" v-on:load_more="load_more_tab2" load_more="true"></my-list-link>
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

                       
                        obj.initVue(me);
        

                    }



                    obj.on_checks = <%= GetOnCheckArray() %>;

                    obj.yes_checks = <%= GetYesCheckArray() %>;


                    obj.my_vue = null;

                    //初始化Vue相关对象
                    obj.initVue = function (me) {

                        var el = me.el;


                        obj.my_vue = new Vue({

                            el: el.children(".content")[0],
                            data: {
                                no_checks: obj.on_checks,
                                yes_checks:obj.yes_checks

                            },
                            methods:{

                                load_more_tab2:function(){

                                    var my_vue = this;

                                    console.log("加载更多点击了");
                                   

                                }



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