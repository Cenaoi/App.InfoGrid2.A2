<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProdSpec.aspx.cs" Inherits="App.InfoGrid2.JF.View.Prod.ProdSpec" %>

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

    <title>商品规格</title>

</head>
<body>
    

    <!-- 这是基本结构 -->
    <div class="page-group">

        <!-- 产品规格 -->
        <div class="popup popup-specifications" style="background: none;">

            <div class="content" >
                    <div style="height: 4rem;"></div>
                    <div style="height: 3rem; background-color: white; position: relative;">
                        <div style="width: 4.5rem; height: 4.5rem; position: absolute; left: 1rem; top: -2rem; padding: 4px; background-image: url(/mobile/res/image/demo/prod_bg_001.png); background-size: 100% 100%;">
                            <img src="/mobile/res/image/demo/PROD_001.jpg" style="width: 100%;">
                        </div>
                        
                        <span style="text-align: center; left: 7rem; top: 0.5rem; position: absolute;color:red;font-weight:bold;">￥{{spec_price}}</span>
                    </div>
                    <div style="height: 60%; background-color: white;padding-top:1rem;">
                        <div style="width:100%;"> 
                            <span  :class="{'button-danger': cur_spec === s }"  style="font-size:1.1rem;margin-left:1rem;border: 1px solid;padding: 0px 5px;display:inline-block;margin-top:0.5rem;" v-for="s in spec_list" @click="spec_click(s)">{{s.spec_text}}</span>
                        </div>
                        <hr />


                        <div class="row" style="font-size:1.1rem;">

                            <div class="col-50" >
                                购买数量
                            </div>
                            <div class="col-50">
                                
                                <a href="#" class="button prod-num-button" style="display:inline;font-size:1.1rem;margin-right:10px;" @click="numRem">-</a>

                                <span>{{spec_num}}</span>

                                <a href="#" class="button prod-num-button" style="display:inline;font-size:1.1rem;margin-left:10px;" @click="numAdd">+</a>

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
                    obj.my_vue = null;


                    obj.specs = <%= GetSpecsObj()  %>;

                    //初始化vue相关对象
                    obj.initVue = function(me){

                        var el = me.el;

       
                        var content_node = el.children(".content")[0];

                        console.log(obj.specs);

                        obj.my_vue = new Vue({
                            el:content_node,
                            data: {
                                spec_list: obj.specs,
                                //选中的规格
                                cur_spec:null,
                                //数量
                                spec_num:1,
                                spec_price:0
                            },
                            methods: {
                                //添加规格数量
                                numAdd: function () {

                                    var me = this;

                                    me.spec_num++;

                                    console.log(me.spec_num);

                                },
                                //减少规格数量
                                numRem: function () {
                                    
                                    var me = this;

                                    if(me.spec_num === 1){
                                        return;
                                    }

                                    me.spec_num--;

                                    console.log(me.spec_num);

                                    

                                },
                                //确定按钮点击事件
                                btnSuccess: function () {

                                    var my_vue = this;

                                    if(my_vue.cur_spec === null){

                                        $.toast("请选择规格！");

                                        return;

                                    }


                                    $.post("/JF/Handlers/ProdHandler.ashx", { 
                                        action: "PUSH_IN_CART",  
                                        prod_id :my_vue.cur_spec.id,
                                        num:my_vue.spec_num
                                    }, 

                                    function (result) {

                                        if(!result.success){

                                            $.toast(result.msg);

                                            return;

                                        }


                                        $.toast("加入购物车了！");
                                        
                                        me.close();

                                    },"json");


                                },
                                //规格点击事件
                                spec_click:function(spec){

                                    var me = this;


                                    me.cur_spec = spec;

                                    //规格的价钱
                                    me.spec_price = spec.price;



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


