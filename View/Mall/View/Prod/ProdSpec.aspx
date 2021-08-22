<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProdSpec.aspx.cs" Inherits="App.InfoGrid2.Mall.View.Prod.ProdSpec" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>致瑧服饰商城</title>

    <script src="/Core/Scripts/XYF/common.js"></script>

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
                            <img :src="prod_obj.PROD_THUMB" style="width: 100%;height:100%;">
                        </div>
                        
                        <span style="text-align: center; left: 7rem; top: 0.5rem; position: absolute;color:red;font-weight:bold;">￥{{spec_price}}</span>
                    </div>
                    <div style="background-color: white;padding-top:1rem;">
                        <div style="width:100%;"> 
                            <span  :class="{'port_bc': cur_spec === s }"  style="font-size:1.1rem;margin-left:1rem;padding: 0px 5px;display:inline-block;margin-top:0.5rem;" v-for="s in spec_list_1" @click="spec_click(s)">{{s.SPEC_TEXT}}</span>
                        </div>
                        <hr />


                        <div class="row" style="font-size:1.1rem;">
                            <template v-for="T in spec_list_2">
                                <div class="col-40" style="padding-left:1rem;">
                                {{T.SPEC_TEXT}}
                                </div>
                                <div class="col-60">
                                
                                    <a href="#" class="button prod-num-button" style="display:inline;font-size:1.1rem;margin-right:10px;" @click="numRem(T)">-</a>

                                    <input type="text" v-model="T.spec_num" style="width:30px;border-style:none;" @change="spec_num_change(T)"/>
       
                                    <a href="#" class="button prod-num-button" style="display:inline;font-size:1.1rem;margin-left:10px;" @click="numAdd(T)">+</a>

                                </div>
                            </template>

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

                        var code = xyf_util.getQuery(me.url,"code",true);

                        xyf_util.post("/App/InfoGrid2/Mall/Handlers/Prod.ashx","GET_PROD_SPECS",{code:code},function(data){
                        
                            obj.my_vue.prod_obj = data.prod_obj;

                            //放默认商品价格
                            if (data.prod_obj) {

                                obj.my_vue.spec_price = data.prod_obj.PRICE_MEMBER;

                            }

                            obj.my_vue.spec_list_1 = data.specs1;
                            
                            //默认选中第一个颜色规格
                            if (data.specs1.length > 0) {

                                obj.my_vue.cur_spec = data.specs1[0];
                            }
                                

                            obj.my_vue.spec_list_2 = data.specs2;

                        });



                        el.on("click",".close-popup",function(){

                            me.close();

                        });


                    }

                    //Vue对象
                    obj.my_vue = null;

                    //初始化vue相关对象
                    obj.initVue = function(me){

                        var code = xyf_util.getQuery(me.url, "code", true);

                        var el = me.el;

                        obj.my_vue = new Vue({
                            el:el.children(".content")[0],
                            data: {
                                //颜色规格
                                spec_list_1: [],
                                //尺寸规格
                                spec_list_2: [],

                                prod_obj: {},
                                //选中的规格
                                cur_spec: null,
                                spec_price: 0,
                                check_specs: []
                            },
                            methods: {
                                //添加规格数量
                                numAdd: function (T) {

                                    var me = this;

                                    T.spec_num++;

                                    me.update_checked_spec(T);

                                },
                                //减少规格数量
                                numRem: function (T) {
                                    
                                    var me = this;

                                    if(T.spec_num < 1){
                                        return;
                                    }

                                    T.spec_num--;

                                    me.update_checked_spec(T);

                                },
                                //直接改变规格数量
                                spec_num_change: function (T) {
                                    var me = this;

                                    me.update_checked_spec(T);
                                },
                                //确定按钮点击事件
                                btnSuccess: function () {

                                    var my_vue = this;

                                    if (!my_vue.cur_spec) {

                                        $.alert("请选择颜色规格！");

                                        return;

                                    }


                                    xyf_util.post("/App/InfoGrid2/Mall/Handlers/Prod.ashx",
                                        "PUSH_CAR_BTN",
                                        { prod_code: code, spec_json: JSON.stringify(my_vue.check_specs) },
                                        function (data,result) {

                                            $.toast(result.msg);

                                            me.close();

                                    });


                       
                                     


                                },
                                //规格点击事件
                                spec_click:function(spec){

                                    var me = this;

                                    me.cur_spec = spec;

                                    //换一种颜色规格都要尺寸数量给清零
                                    me.spec_list_2.forEach(function (v,i) {

                                        v.spec_num = 0;

                                    });

                                    me.check_specs.forEach(function (v, i) {
        
                                        if (v.spec_code_1 === spec.PK_SPEC_CODE) {

                                            //这是尺寸规格编码
                                            var spec_code = v.spec_code_2;

                                            me.spec_list_2.forEach(function (s_v,s_i) {

                                                if (s_v.PK_SPEC_CODE === spec_code) {

                                                    s_v.spec_num = v.spec_num;
                                                }
                       

                                            });



                                        }


                                    });



                                },
                                //更新选中规格数量 参数1--尺寸规格对象
                                update_checked_spec: function (T) {

                                    var me = this;
                                    //这是颜色主键编码
                                    var spec_code_1 = me.cur_spec.PK_SPEC_CODE;
                                    //这是尺寸主键编码
                                    var spec_code_2 = T.PK_SPEC_CODE;

                                    var flag = false;

                                    me.check_specs.forEach(function (v, i) {

                                        if (v.spec_code_1 === spec_code_1 && v.spec_code_2 === spec_code_2) {
                                            v.spec_num = T.spec_num;
                                            flag = true;
                                            return;

                                        }

                                    });

                                    if (flag) {

                                        return;

                                    }

                                    me.check_specs.push({
                                        spec_code_1: spec_code_1,
                                        spec_text_1: me.cur_spec.SPEC_TEXT,
                                        spec_no_1:me.cur_spec.SPEC_NO,
                                        spec_code_2: spec_code_2,
                                        spec_text_2: T.SPEC_TEXT,
                                        spec_no_2:T.COL_6,
                                        spec_num: T.spec_num
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
