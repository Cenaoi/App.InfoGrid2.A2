<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReimburDeta.aspx.cs" Inherits="App.InfoGrid2.WF.View.Reim.ReimburDeta" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />


    <title>报销明细界面</title>

        <script src="/WF/Script/common.js"></script>

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
            <nav class="bar bar-tab" style="background-color:#4cd964;">
              <a class="tab-item btn_success saveAndRed"  href="#">

                <span class="tab-label" style="color:white;">保存并返回</span>
              </a>
       
            </nav>
            <div class="content">
               

                <template v-for="(r,r_index) in reim_detas">
                 
                    <div class="row" style="padding:10px 20px;">

                        <div class="col-50">报销明细({{ r_index + 1 }})</div>
                        <div class="col-50" v-show="r_index > 0"  style="text-align:right;">
                            <a href="#" @click="delete_reim(r)" >删除</a>
                        </div>
                    </div>

                    <div class="list-block" style="margin:0px;">
                        <ul>
                          <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">费用日期</div>
                                <div class="item-input">
                                  <input type="date" v-model="r.COL_8" @change="save_reim_deta(r,'COL_8')">
                                </div>
                              </div>
                            </div>
                          </li>
                           <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">报销金额(元)</div>
                                <div class="item-input">
                                  <input type="number" v-model="r.COL_17" @keyup="save_reim_deta(r,'COL_17')" @change="save_reim_deta(r,'COL_17')">
                                </div>
                              </div>
                            </div>
                          </li>
                           <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">相关人数</div>
                                <div class="item-input">
                                  <input type="number" v-model="r.COL_40" @keyup="save_reim_deta(r,'COL_40')" @change="save_reim_deta(r,'COL_17')">
                                </div>
                              </div>
                            </div>
                          </li>
                           <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">报销类型</div>
                                <div class="item-input">
                                  <select v-model="r.COL_13"  @change="select_reim_type(r,'COL_13')">
                                      <option :value="r_type.COL_1" v-for="r_type in reim_types">{{r_type.COL_2}}</option>
                                  </select>
                                </div>
                              </div>
                            </div>
                          </li>
                           <li>
                            <div class="item-content">
                              <div class="item-inner">
                                <div class="item-title label">费用明细</div>
                                <div class="item-input">
                                  <textarea  placeholder="请输入费用明细描述" v-model="r.COL_27" @keyup="save_reim_deta(r,'COL_27')" @change="save_reim_deta(r,'COL_17')"></textarea>
                                </div>
                              </div>
                            </div>
                          </li>
                        </ul>
                    </div>

                    <img-list :imgs="r.imgs"></img-list>
                    
                    <annex-list :annexs="r.annexs" ></annex-list> 


                </template>

                <div class="content-block" style="margin-top:0.5rem;">
                    <p><a href="#" class="button button-success button-fill button-big">增加报销明细</a></p>
                </div>
                <div>
                    报销合计：{{money_total}}
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

                        el.on("click", ".saveAndRed", function () {
                             
                            console.log("ddddddddddddd");
                       
                            var my_vue = me.my_vue;

                            console.log(me);


                            var smlist = my_vue.reim_detas;

                            var smlistjson = JSON.stringify(smlist);

                            $.post("/WF/Handlers/ReimHandler.ashx", { action: "SAVEALL", smlistjson: smlistjson, tablename: 'UT_347' }, function (result) {

                                if (result.success == true) {

                                    $.alert("保存成功");

                                    $.router.back();

                                } else {

                                    $.alert("保存失败");
                                }
                                                         
                            }, "json");
                          
                  
                        });

                        //增加报销明细 按钮点击事件
                        el.on("click", ".button-success", function () {
                           
                            $.post("/WF/Handlers/ReimHandler.ashx",{action:"NEW_REIM_DETA",id:row_id},function(result){
                            
                                if(!result.success){
                                    $.toast(result.error_msg);
                                    return;
                                }

                                
                                var v = result.data;

                                console.log("1111111111111111111");

                                console.log(v);

                                me.addImgAndAnnex(v);
                          
                                me.my_vue.reim_detas.push(v);

                            },"json");
        
                        });

                    }

                    //vue对象
                    obj.my_vue = null;

                    //初始化vue相关对象
                    obj.initVue = function () {

                        var me = this;

                        console.log(me);

                        var row_id = xyf_util.getQuery(me.url,"id",true);
                        
                        var el = me.el;

                        //定时器
                        var time = null;

                        me.my_vue = new Vue({
                            el: el.children(".content")[0],
                            data: {
                                reim_detas: obj.reim_detas,
                                //改变字段
                                change_fields: [],
                                reim_types: obj.reim_types,
                                cur_row: null,
                                money_total: 0
                            },
                            computed: {
                                //报销金额合计
                                reim_money_total: function () {

                                    var my_vue = this;

                                    var money_total = 0;

                                    if (!my_vue.reim_detas) {

                                        return;
                                    }

                                    my_vue.reim_detas.forEach(function (v, i) {

                                        money_total += v.COL_17;
                                    });
                                    
                                    return money_total;
                                }
                            },
                            methods: {
                                delete_reim: function (r) {

                                    var my_vue = this;

                                    $.confirm("你确定要删除数据吗？", "", function () {

                                        $.post("/WF/Handlers/ReimHandler.ashx",{action:"DELETE_REIM_DETA",id:r.ROW_IDENTITY_ID,table_name:'UT_347'},function(result){
                                        
                                            if(!result.success){

                                                $.toast(result.error_msg);

                                                return;

                                            }
                                            
                                            Mini2.Array.remove(my_vue.reim_detas,r);

                                            $.toast("删除成功了！");


                                        },"json");


                                    });
                                },

                                //只要变更一个就保存整个对象
                                save_reim_deta:function(r,field_text){

                                    var my_vue = this;

                                    my_vue.cur_row = r;

                                    Mini2.Array.include(my_vue.change_fields,field_text);

                                    if(time){
                                        time.resetStart();
                                    }
                                    else{
                                        time =  Mini2.setTimer(function(){
                                                $.post("/WF/Handlers/ReimHandler.ashx",{
                                                    action:'SAVE_REIM_DETA',
                                                    id:row_id,
                                                    reim_deta_json_str:JSON.stringify(my_vue.cur_row),
                                                    change_files_str:JSON.stringify(my_vue.change_fields),
                                                    table_name:'UT_347'},function(result){
                                    
                                                        if(!result.success){

                                                            console.error(result);

                                                            return;
                                                        }

                                                    },"json");
                                            
                                        },{ interval: 500,limit: 1});
                                    }


                                },
                                //费用类型改变事件
                                select_reim_type:function(r,field_text){

                                    var my_vue = this;
                                    var val = r[field_text];
                                    var reim_type = null;

                                    if(xyf_util.isNullOrWhiteSpace(val)){
                                        console.log("费用类型为空！");
                                        return;
                                    }

                                    my_vue.reim_types.forEach(function(v,i){
                                    
                                        if(v.COL_1 === val){
                                            reim_type = v;
                                        }

                                    });

                                    if(!reim_type){
                                        console.error("费用类型选择出错了！");
                                        return;
                                    }

                                    r.COL_12 = reim_type.COL_2;
                                    r.COL_13 = reim_type.COL_1;
                                    r.COL_14 = reim_type.COL_3;
                                    r.COL_15 = reim_type.COL_5;
                                    r.COL_28 = reim_type.COL_4;
                                    r.COL_29 = reim_type.COL_6;
                                    
                                    Mini2.Array.include(my_vue.change_fields,'COL_12');
                                    Mini2.Array.include(my_vue.change_fields,'COL_13');
                                    Mini2.Array.include(my_vue.change_fields,'COL_14');
                                    Mini2.Array.include(my_vue.change_fields,'COL_15');
                                    Mini2.Array.include(my_vue.change_fields,'COL_28');
                                    Mini2.Array.include(my_vue.change_fields,'COL_29');

                                    //保存一下
                                    my_vue.save_reim_deta(r,field_text);


                                }
                            }
                        });

                    }

                    //添加图片组件和附件数据给报销明细中
                    obj.addImgAndAnnex = function(reim_deta){

                        var imgs = {
                            data:[],
                            server_url:"/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_347&tag_code=reim_deta_img&row_id="+reim_deta.ROW_IDENTITY_ID,
                            delete_img_url: '/WF/Handlers/UploaderFileHandle.ashx',
                            btn_id:'uploader_img_'+reim_deta.ROW_IDENTITY_ID,
                            row_id:reim_deta.ROW_IDENTITY_ID,
                            table_name:'UT_347',
                            tag_code:'reim_deta_img',
                            field_text:'COL_22'
                        }

                        var annexs ={

                            data: [],
                            server_url: '/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_347&tag_code=reim_deta_annex&row_id=' + reim_deta.ROW_IDENTITY_ID,
                            delete_annex_url: '/WF/Handlers/UploaderFileHandle.ashx',
                            btn_id:'uploader_annex_'+reim_deta.ROW_IDENTITY_ID,
                            row_id:reim_deta.ROW_IDENTITY_ID,
                            table_name:"UT_347",
                            tag_code:"reim_deta_annex",
                            field_text:"COL_23"

                        }

                        reim_deta.imgs = imgs;

                        reim_deta.annexs = annexs;


                    }


                    ///初始化数据
                    obj.getData = function () {

                        var me = this;
                        var el = me.el;

                        var r_id = xyf_util.getQuery(me.url, "id", true);

                        $.post("/WF/Handlers/ReimburDetaHandle.ashx", { action: 'INIT_DATA_REIMBUR_DETA', r_id: r_id }, function (result) {

                            var data = result.data;

                            console.log("初始化数据结果: ", result);

                            me.my_vue.money_total = data.money_total;
                            me.my_vue.reim_detas = data.reim_detas;
                            me.my_vue.reim_types = data.reim_types;


                        }, "json");

                    }

                    return obj;

                }

            </script>


        </div>

    </div>



</body>

</html>
