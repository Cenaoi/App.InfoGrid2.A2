<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EcProdSpec.aspx.cs" Inherits="App.InfoGrid2.Mobile_V1.Prod.EcProdSpec" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>产品规格界面</title>

    <link href="/Core/Scripts/SUI/sm.css" rel="stylesheet" />

</head>
<body>


    <div class="page-group">
            
        <!-- 产品规格 -->
        <div class="popup popup-specifications" style="background: none;">

             <div class="content"  style="overflow:hidden;">
                    <div style="height: 4rem;"></div>
                    <div style="height: 3rem; background-color: white; position: relative;">
                        <div style="width: 4.5rem; height: 4.5rem; position: absolute; left: 1rem; top: -2rem; padding: 4px; background-image: url(/mobile/res/image/demo/prod_bg_001.png); background-size: 100% 100%;">
                            <img src="/mobile/res/image/demo/PROD_001.jpg" style="width: 100%;">
                        </div>
                        <span style="text-align: center; left: 7rem; top: 0.5rem; position: absolute;">产品规格</span>
                    </div>
                    <div style="height: 60%; overflow: auto; background-color: white;">
                        <div class="list-block contacts-block" style="margin: 0 0;">
                            <div class="list-group" v-for="spec in spec_list">
                                <ul>
                                    <li class="list-group-title" style="color: black; font-size: 1rem;">{{spec.text}}</li>
                                     <li class="list-item" v-for="d in spec.datas">
                                        <div class="item-content">
                                            <div class="item-inner">
                                                <div class="item-title" style="margin-left: 1rem;">{{d.SPEC_NAME}}</div>
                                                <table border="0" class="prod-num-box">
                                                    <tr>
                                                        <td><a href="#" class="button prod-num-button" @click="numRem(d)">-</a></td>
                                                        <td>
                                                        <input type="tel" class="prod-num" v-model="d.cus_num" value="0" /></td>
                                                        <td><a href="#" class="button prod-num-button" @click="numAdd(d)">+</a></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </div>

                        </div>
                    </div>
                    <div style="background-color: white; padding: 1rem 1rem 0.5rem 1rem; overflow: hidden;">
                        <div class="row">
                            <div class="col-50">
                                <a href="#" class="close-popup button button-big button-fill button-warning" style="text-align: center;">关闭</a>
                            </div>
                            <div class="col-50">
                                <a href="#" class="button button-big button-fill button-danger" @click="btnSuccess" style="text-align: center;">确定</a>
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

                        el.on("click",".close-popup",function(){

                            me.close();

                        });


                    }

                    //Vue对象
                    obj.myVue = null;

                    //初始化vue相关对象
                    obj.initVue = function (me) {

                        var el = me.el;

                        
                        var specs = <%= GetSpecsObj() %>;

                     
                        specs.spec_list.forEach(function(v,i){


                            v.datas.forEach(function(s_v,s_i){
                            
                                s_v.cus_num = 0;

                            
                            });



                        });



                        var content_node = el.children(".content")[0];

                        obj.specVuew = new Vue({
                            el:content_node,
                            data: {
                                spec_list: specs.spec_list,
                                post_data_obj: {}
                            },
                            methods: {
                                //添加规格数量
                                numAdd: function (d) {


                                    var num = d.cus_num || 0;

                                    var me = this;

                                    d.cus_num = num + 1;
                                    

                                    //添加数据进提交数组里面
                                    me.post_data_obj["S_"+d.ES_PRODUCT_DATA_ID] = d;


                                },
                                //减少规格数量
                                numRem: function (d) {
                                    
                                    var me = this;

                                    if (d.cus_num == 0) {
                                        return;
                                    }

                                    d.cus_num--;

                                    //添加数据进提交数组里面
                                    me.post_data_obj["S_"+d.ES_PRODUCT_DATA_ID] = d;

                                },
                                //确定按钮点击事件
                                btnSuccess: function () {

                                    var me = this;

                                    var post_data_list = [];

                                    console.log("11111111111111111");

                                    for(var key in me.post_data_obj){

                                        var data = me.post_data_obj[key];


                                        var obj = {
                                            "spec_id": data.ES_PRODUCT_DATA_ID,
                                            "prod_id": data.PROD_ID,
                                            "spec_num": data.cus_num
                                        };

                                        post_data_list.push(obj);

                                    }


                                    console.log("2222222222222222222");


                                    $.post("/Handlers/EcCartHandler.ashx", { action: "PUSH_IN_CART", data: JSON.stringify(post_data_list) }, function (result) {

                                        
                                        console.log("333333333333333333333333333");

                                         var json = JSON.parse(result);


                                         if(!json.success){

                                             $.toast(json.msg);

                                             return;

                                         }

                                         location.href = "/Mobile_V1/Order/EcCart.aspx?"+ Math.random();
                                        
                                         me.close();

                                    });


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



