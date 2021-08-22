<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlowEaa.aspx.cs" Inherits="App.InfoGrid2.WF.View.Flow.FlowEaa" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>待审核界面</title>

    <script src="/WF/Script/common.js"></script>

</head>
<body style="max-width:640px; margin:auto; font-family: Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back" data-no-cache="true"></a>
                <h1 class="title">我审核的</h1>
            </header>

            <div class="content">
                
                <div class="buttons-tab">
                    <a href="#tab1" class="tab-link active button">待我审批的</a>
                    <a href="#tab2" class="tab-link button">我已审批的</a>
                </div>
                
                <div class="content-block" style="margin:0px; padding:0px;">
                    <div class="tabs">
                      <div id="tab1" class="tab active">

                          <div class="list-block media-list" style="margin:0px;" >
                                <ul>
                                <li v-for="l in no_checks">
                                    <div class="item-content item-link" @click="item_click(l)" style="padding-left:0.5rem;">
                                        <div class="item-media" v-if="l.img_src">
                                            <img :src="l.img_src" style="width: 2.5rem;" />
                                        </div>
                                        <div class="item-inner">
                                            <div class="item-title-row">
                                                <div class="item-title">{{l.title}}</div>
                                                <div class="item-after">{{date_format(l.create_date)}}</div>
                                            </div>
                                            <div class="item-subtitle">{{l.sub_text}}</div>
                                            <div class="item-subtitle">{{l.text}}</div>
                                        </div>
                                    </div>
                                </li>
                                </ul>
                            </div>


                      </div>
                      <div id="tab2" class="tab">


                        <div class="list-block media-list" style="margin:0px;" >
                                <ul>
                                <li v-for="(l,i) in yes_checks">
                                    <div class="item-content item-link" @click="item_click(l,i)" style="padding-left:0.5rem;">
                                        <div class="item-media" v-if="l.img_src">
                                            <img :src="l.img_src" style="width: 2.5rem;" />
                                        </div>
                                        <div class="item-inner">
                                            <div class="item-title-row">
                                                <div class="item-title">{{l.title}}</div>
                                                <div class="item-after">{{date_format(l.create_date)}}</div>
                                            </div>
                                            <div class="item-subtitle" style="white-space:normal;">{{l.sub_text}}</div>
                                            <div class="item-subtitle">{{l.text}}</div>
                                        </div>
                                    </div>
                                </li>
                                </ul>
                            </div>

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
                                item_click: function (T){

                                    var my_vue = this;

                                    var url = Mini2.urlAppend(T.url,{flow_id:T.flow_id,biz_sid:T.biz_sid},true);

                                    $.router.load(url);
                                },
                                date_format:function(date_text){

                                    return moment(date_text).fromNow();
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




