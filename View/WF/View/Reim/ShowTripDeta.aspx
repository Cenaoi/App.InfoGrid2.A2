<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowTripDeta.aspx.cs" Inherits="App.InfoGrid2.WF.View.Reim.ShowTripDeta" %>

<!DOCTYPE html>

<html>
<head>
    

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <script src="/WF/Script/common.js"></script>

    <title>出差明细</title>


</head>
<body style="max-width:640px; margin:auto; font-family: Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">
    


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">出差记录</h1>
                <a class="icon icon-home pull-right expand" href="/WF/View/Home.aspx"></a>
            </header>
            
            <div class="content">


                  <template v-for="(t,t_index) in trip_detas">
                 
                    <div class="row" style="padding:10px 20px;">

                        <div class="col-50">出差记录({{ t_index + 1 }})</div>
                    </div>

                    <div class="list-block" style="margin:0px;">
                        <ul>
                          <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">出差日期</div>
                                <div class="item-input">
                                  <input type="date" v-model="t.COL_8" readonly="readonly">
                                </div>
                              </div>
                            </div>
                          </li>
                           <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">记录内容</div>
                                <div class="item-input">
                                  <textarea  v-model="t.COL_27" readonly="readonly"></textarea>
                                </div>
                              </div>
                            </div>
                          </li>
                        </ul>
                    </div>

                    <img-list :imgs="t.imgs"></img-list>
                    
                    <annex-list :annexs="t.annexs"></annex-list>

                     
                </template>

                <div v-if="trip_detas.length===0" style="text-align:center;padding:1rem;color:#808080;font-size:0.7rem;">没有相关记录</div>

            </div>

            <script type="text/javascript" data-main="true">
        
                function main() {
            
                    
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        me.initVue();

                        me.initData();
                    }

                    obj.pageRefresh = function () {

                        var me = this,
                            el = me.el;

                        me.initData();
                    }

                    //vue对象
                    obj.my_vue = null;

                    //初始化vue相关对象
                    obj.initVue = function () {

                        var me = this;

                        var el = me.el;

                        var row_id = xyf_util.getQuery(me.url,"id",true);

                        var time = null;

                        me.my_vue = new Vue({
                            el: el.children(".content")[0],
                            data: {
                                trip_detas: [],
                            },
                            methods: {

                            }
                        });

                    }

                    //加载数据
                    obj.initData = function () {

                        var me = this;

                        me.my_vue.trip_detas = <%= GetTripDetasObj() %>;

                    }


                    return obj;

                }

            </script>

        </div>

    </div>


</body>
</html>
