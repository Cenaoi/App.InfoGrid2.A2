<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewAdve.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.Bus.NewAdve" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>微信红包</title>

    <script src="/Wxhb/Script/common.js?v=20170210001"></script>

</head>
<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">


            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">添加广告</h1>
            </header>

            <div class="content">

                
                <div class="list-block" style="margin-top:0px;margin-bottom:0.5rem;">
                    <ul>
                      <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">广告标题</div>
                            <div class="item-input">
                              <input type="text" v-model="adve_obj.COL_1" @keyup="save_trip_deta('COL_1')">
                            </div>
                          </div>
                        </div>
                      </li>
                      <li class="align-top">
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">内容正文</div>
                            <div class="item-input">
                               <textarea v-model="adve_obj.COL_2" @keyup="save_trip_deta('COL_2')"></textarea>
                            </div>
                          </div>
                        </div>
                      </li>
                   </ul>
                </div>

                <wx-img-upload :imgs="adve_obj.imgs" left_text="图片"></wx-img-upload>


                <div class="list-block" style="margin:0.5rem 0;">
                    <ul>
                    <li>
                        <div class="item-content">
                            <div class="item-inner">
                                <div class="item-title">店铺位置</div>
                            </div>
                        </div>
                      </li>
                     <li>
                        <div class="item-content item-link" @click="edit_shop_address">
                            <div class="item-inner">
                                <div class="item-title" style="font-size:0.7rem;white-space:inherit;">{{adve_obj.COL_6}}</div>
                            </div>
                        </div>
                      </li>
                    </ul>
                  </div>
                
                
                <div class="list-block" style="margin:0.5rem 0;">
                    <ul>
                      <li>
                        <div class="item-content">
                            <div class="item-inner">
                                <div class="item-title">红包地址投放位置</div>
                                <div class="item-after icon icon-edit" @click="new_hb_addres"></div>
                            </div>
                        </div>
                      </li>
                      <li v-for="h in  adve_obj.hb_addr_list">
                          <div class="item-content item-link"  >
                              <div class="item-inner">
                                  <div class="item-title" style="font-size:0.7rem;white-space:inherit;" @click="edit_hb_address(h)">{{h.HB_ADDRESS}}</div>
                                  <div class="item-after" style="color:red;" @click.stop="delete_hb_address(h)">删除</div>
                              </div>
                          </div>
                      </li>
                   </ul>
                </div>     

                <div class="list-block" style="margin:0.5rem 0;">
                        <ul>
                      <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">详细地址</div>
                            <div class="item-input">
                              <input type="text" v-model="adve_obj.COL_7" @keyup="save_trip_deta('COL_7')">
                            </div>
                          </div>
                        </div>
                      </li>

                      <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">联系方式</div>
                            <div class="item-input">
                              <input type="text" v-model="adve_obj.COL_8" @keyup="save_trip_deta('COL_8')" >
                            </div>
                          </div>
                        </div>
                      </li>
                      <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">广告覆盖区域</div>
                            <div class="item-input">
                               <select v-model="adve_obj.COL_9" @change="save_trip_deta('COL_9')">
                                   <option value="2">2公里以内</option>
                                   <option value="10">10公里以内</option>
                                   <option value="40">40公里以内</option>
                                   <option value="200">200公里以内</option>
                                   <option value="1000">1000公里以内</option>
                                   <option value="0">不限（红包墙）</option>
                               </select>
                            </div>
                          </div>
                        </div>
                      </li>

                      <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">预算金额</div>
                            <div class="item-input">
                               <input type="number" v-model="re_money" />
                            </div>
                          </div>
                        </div>
                      </li>

                       <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">红包数量</div>
                            <div class="item-input">
                               <input type="number" v-model="adve_obj.COL_10"  @change="save_trip_deta('COL_10')"/>
                            </div>
                          </div>
                        </div>
                      </li>

                       <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">红包最高金额</div>
                            <div class="item-input">
                               <input type="number" v-model="adve_obj.COL_11"  @change="save_trip_deta('COL_11')"/>
                            </div>
                          </div>
                        </div>
                      </li>
                              
                   </ul>
                </div>

                
                <div class="content-block" v-show="show_type !== 'edit'">
                    <p><a href="#" class="button button-round button-fill button-big" @click="btn_submit">提交审核</a></p>
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

                        obj.getAdveObj(me);

                        
                    }
                    
                    //初始化vue相关的
                    obj.initVue = function (me) {

                        var el = me.el;

                        var time = null;

                        var adve_id = xyf_util.getQuery(me.url, "adve_id", true);

                        var show_type = xyf_util.getQuery(me.url, "show_type", true);

                        obj.my_vue = new Vue({
                            el: el.children(".content")[0],
                            data: {
                                adve_obj: {
                                    'COL_11': 0,
                                    'COL_10': 0,
                                    're_money': 0,
                                    'COL_9': 0,
                                    'COL_8': '',
                                    'COL_7': '',
                                    "COL_6": '',
                                    'COL_1': '',
                                    'COL_2': '',
                                    'imgs': {

                                    }

                                },
                                change_fields: [],
                                //广告充值金额
                                re_money: 0,
                                show_type: show_type

                            },
                            methods: {
                                //只要变更一个就保存整个对象
                                save_trip_deta: function (field_text) {

                                    var my_vue = this;

                                    Mini2.Array.include(my_vue.change_fields, field_text);

                                    if (time) {
                                        time.resetStart();
                                    }
                                    else {
                                        time = Mini2.setTimer(function () {


                                            $.post("/Wxhb/Handlers/BusHandler.ashx",
                                                {
                                                    action: 'SAVE_ADVE',
                                                    reim_deta_json_str: JSON.stringify(my_vue.adve_obj),
                                                    change_files_str: JSON.stringify(my_vue.change_fields),
                                                    table_name: 'UT_002'
                                                }, function (result) {

                                                    if (!result.success) {

                                                        console.error(result);

                                                        return;
                                                    }

                                                }, "json");


                                        }, { interval: 500, limit: 1 });
                                    }


                                },
                                //提交按钮事件
                                btn_submit: function () {

                                    var my_vue = this;

                                    $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'ADVE_SUBMIT', adve_id: adve_id, re_money: my_vue.re_money }, function (result) {

                                        if (!result.success) {

                                            $.alert(result.error_msg);
                                            return;
                                        }

                                        var url = Mini2.urlAppend("/Wxhb/View/Bus/BusContent.aspx", {}, true);

                                        location.href = url;


                                    }, "json");

                                },
                                //编辑店铺地址
                                edit_shop_address: function () {

                                    var my_vue = this;

                                    var lat = my_vue.adve_obj.COL_5;
                                    var lon = my_vue.adve_obj.COL_4;
                                    var addr = my_vue.adve_obj.COL_6;

                                    var url = Mini2.urlAppend("/Wxhb/View/Map/MapAddress.aspx", { lat: lat, lon: lon, addr: addr, action: 'edit_adve_shop_addres', row_id: my_vue.adve_obj.ROW_IDENTITY_ID}, true);

                                    $.router.load(url);
                                },
                                //红包地址删除按钮事件
                                delete_hb_address: function (h) {

                                    var my_vue = this;

                                    $.confirm("确定要删除红包地址吗？", "", function () {

                                        $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'DELETE_HB_ADDRESS', id: h.ROW_IDENTITY_ID, adve_code: my_vue.adve_obj.PK_ADVE_CODE }, function (result) {

                                            if (!result.success) {
                                                $.alert(result.error_msg);
                                                return;
                                            }


                                            xyf_util.arrayRemove(my_vue.adve_obj.hb_addr_list, h);

                                            $.toast(result.msg);

                                        }, "json");

                                    });

                                   

                                },
                                //编辑红包地址按钮事件
                                edit_hb_address: function (h) {

                                    var my_vue = this;

                                    var lat = h.HB_LATITUDE;
                                    var lon = h.HB_LONGITUDE;
                                    var addr = h.HB_ADDRESS;

                                    var url = Mini2.urlAppend("/Wxhb/View/Map/MapAddress.aspx", { lat: lat, lon: lon, addr: addr, action: 'edit_adve_hb_addres', row_id: h.ROW_IDENTITY_ID }, true);

                                    $.router.load(url);

                                },
                                //新增红包地址按钮事件
                                new_hb_addres: function () {

                                    var my_vue = this;

                                    var lat = my_vue.adve_obj.COL_5;
                                    var lon = my_vue.adve_obj.COL_4;
                                    var addr = my_vue.adve_obj.COL_6;

                                    var url = Mini2.urlAppend("/Wxhb/View/Map/MapAddress.aspx", { lat: lat, lon: lon, addr: addr, action: 'new_adve_hb_addres', row_id: my_vue.adve_obj.ROW_IDENTITY_ID }, true);

                                    $.router.load(url);

                                },
                                

                            }

                        });


                    }

                    //页面加载成功事件
                    obj.onLoad = function () {

                        //获取微信js配置对象
                        $.post("/Wxhb/Handlers/UserHandler.ashx", { action: 'GET_WX_JS_CONFIG' }, function (result) {

                            if (!result.success) {

                                $.alert(result.error_msg);
                                return;

                            }

                            var config = result.data;

                            var config_str = JSON.stringify(config);

                            wx.config(config);

                            wx.ready(function () {

                                //这是隐藏微信自带的右上角菜单  所有非基础按钮接口
                                wx.hideAllNonBaseMenuItem();

                            });



                        }, "json");

                    }

                    //获取广告对象数据
                    obj.getAdveObj = function (me) {




                        var adve_id = xyf_util.getQuery(me.url, "adve_id", true);



                        $.post("/Wxhb/Handlers/BusHandler.ashx", { action: 'GET_ADVE', adve_id: adve_id }, function (result) {

                            if (!result.success) {

                                $.alert(result.error_msg);

                                return;

                            }

                            obj.my_vue.adve_obj = result.data;



                        }, "json");

                    }


                    //页面恢复的事件
                    obj.pageRevert = function () {
                        var me = this,
                            el = me.el;

                        obj.getAdveObj(me);


                    }
                    
                    //vue对象
                    obj.my_vue = null;

                    return obj;

                }

            </script>


        </div>

    </div>
    
</body>
</html>



