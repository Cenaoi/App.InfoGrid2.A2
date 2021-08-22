<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArticlePage.aspx.cs" Inherits="App.InfoGrid2.DJK.ArticlePage" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title><%= GetMenuTextStr() %></title>

    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />

</head>
<body>


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <div class="content">

                <div class="card" v-for="c in  catas">
                <div class="card-header" style="background-color:black;color:white;">{{c.CATA_TEXT}}</div>
                <div class="card-content">
                  <div class="card-content-inner" v-show="c.sub_data.length > 0">
                        <div class="list-block">
                        <ul>
                          <li class="item-content item-link" v-for="d in c.sub_data">
                            <div class="item-inner">
                              <div class="item-title">{{d.TITLE}}</div>
                            </div>
                          </li>
                        </ul>
                      </div>
                  </div>
                </div>
              </div>
                <aaa ddd="333></aaa>
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

                    //vue对象
                    obj.my_vue = null;

                    obj.catas = <%= GetCatasObj()  %>;
                    
                    //初始化vue相关的对象
                    obj.initVue = function (me) {

                        var el = me.el;


                        obj.my_vue = new Vue({
                        
                            el:el.children(".content")[0],
                            data:{
                                catas:obj.catas
                            }


                        });







                    }





                    return obj;

                }

            </script>


        </div>

    </div>
    <script src="/Core/Scripts/m5/M5.min.js"></script>

    <script src="/Core/Scripts/Hammer/hammer.min.js"></script>

    <script src="/Core/Scripts/vue.2.0/vue.js"></script>

    <script src="/Core/Scripts/m5/M5.min.js"></script>

    <script src="/Core/Scripts/vue/vue-2.0.1.js"></script>

</body>
</html>

<script>



    $(function () {

        //FastClick.attach(document.body);

        $("body").height($(window).height());

        
        $("body").width($(window).width());

        $.router = Mini2.create('Mini2.ui.PageRoute', {});

    });


</script>










