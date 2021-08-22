<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowReimDeta.aspx.cs" Inherits="App.InfoGrid2.WF.View.Reim.ShowReimDeta" %>


<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <script src="/WF/Script/common.js"></script>

    <title>报销明细界面</title>
</head>
<body style="max-width:640px; margin:auto; font-family: Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">


            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">月度报销明细</h1>
                <a class="icon icon-home pull-right expand" href="/WF/View/Home.aspx"></a>
            </header>
            
            <div class="content">
               

                <template v-for="(r,r_index) in reim_detas">
                 
                    <div class="row" style="padding:10px 20px;">

                        <div class="col-50">报销明细({{ r_index + 1 }})</div>

                    </div>

                    <div class="list-block" style="margin:0px;">
                        <ul>
                          <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">报销日期</div>
                                <div class="item-input">
                                  <input type="date" v-model="r.COL_8" readonly="readonly"  >
                                </div>
                              </div>
                            </div>
                          </li>
                           <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">报销金额(元)</div>
                                <div class="item-input">
                                  <input type="number" v-model="r.COL_17" readonly="readonly"  >
                                </div>
                              </div>
                            </div>
                          </li>
                           <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">相关人数</div>
                                <div class="item-input">
                                  <input type="number" v-model="r.COL_40" readonly="readonly"  >
                                </div>
                              </div>
                            </div>
                          </li>
                           <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">报销类型</div>
                                <div class="item-input">
                                  <input type="text" v-model="r.COL_12"  readonly="readonly"  />
                                </div>
                              </div>
                            </div>
                          </li>
                           <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">费用明细</div>
                                <div class="item-input">
                                  <textarea  placeholder="请输入费用明细描述" v-model="r.COL_27"  readonly="readonly"></textarea>
                                </div>
                              </div>
                            </div>
                          </li>
                        </ul>
                    </div>

                    <img-list :imgs="r.imgs"></img-list>
                    
                    <annex-list :annexs="r.annexs" ></annex-list> 


                </template>


                <div class="row" style="padding:10px 20px;">

                    <div class="col-50">报销合计：{{money_total}}</div>

                </div>
                
            </div>


            <script type="text/javascript" data-main="true">
        
                function main() {
            
                    var obj = {};

                    obj.pageRefresh = function () {

                        var me = this;

                        me.getData();
                    }

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        var row_id = xyf_util.getQuery(me.url,"id",true);

                        me.initVue();

                        me.getData();
                    }


                    //vue对象
                    obj.my_vue = null;

                    //初始化vue相关对象
                    obj.initVue = function () {
                          
                        var me = this,
                            el = me.el;

                        var row_id = xyf_util.getQuery(me.url,"id",true);

                        //定时器
                        var time = null;

                        me.my_vue = new Vue({
                            el: el.children(".content")[0],
                            data: {
                                reim_detas: null,
                                money_total: 0
                            },
                            computed: {
                                //报销金额合计
                                reim_money_total: function () {

                                    var my_vue = this;

                                    if (!my_vue.reim_detas) {

                                        return;
                                    }

                                    var money_total = 0;

                                    console.log("reim_datas", my_vue.reim_detas);

                                    my_vue.reim_detas.forEach(function (v, i) {

                                        money_total += v.COL_17;
                                    });

                                    return money_total;
                                }
                            }
                        });

                    }


                    ///初始化数据
                    obj.getData = function () {

                        var me = this;
                        var el = me.el;

                        var fee_code = xyf_util.getQuery(me.url, "fee_code", true);

                        var id = xyf_util.getQuery(me.url, "id", true);

                        $.post("/WF/Handlers/ShowReimDetaHandle.ashx", { action: 'INIT_DATA', id: id, fee_code: fee_code }, function (result) {

                            var data = result.data;

                            console.log("data", data);

                            me.my_vue.reim_detas = data.reim_detas;
                            me.my_vue.money_total = data.money_total;

                            console.log("me.my_vue.reim_detas", me.my_vue.reim_detas);

                            console.log("money_total",me.my_vue.money_total);

                        }, "json");

                    }
                    
                    return obj;

                }

            </script>


        </div>

    </div>




</body>

</html>

