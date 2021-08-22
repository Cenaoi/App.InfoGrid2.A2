<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForeverQrcodeApply.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.User.ForeverQrcodeApply" %>

<!DOCTYPE html>

<html>
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>微信红包</title>

    <script src="/Wxhb/Script/common.js?v=201702016001"></script>

</head>
<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">

    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">永久二维码申请</h1>
            </header>


            <div class="content">

                <div class="list-block" style="margin-top:0px;">
                    <ul>
                      <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">姓名</div>
                            <div class="item-input">
                              <input type="text" placeholder="输入姓名" v-model="forever_qrcode_obj.USER_TEXT" @keyup="save_trip_deta('USER_TEXT')">
                            </div>
                          </div>
                        </div>
                      </li>
                      <li class="align-top">
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">住址</div>
                            <div class="item-input">
                               <input type="text" placeholder="请输入详细地址" v-model="forever_qrcode_obj.USER_ADDRESS" @keyup="save_trip_deta('USER_ADDRESS')" />
                            </div>
                          </div>
                        </div>
                      </li>
                      <li class="align-top">
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">电话</div>
                            <div class="item-input">
                               <input type="text" placeholder="请输入联系电话"  v-model="forever_qrcode_obj.USER_TEL" @keyup="save_trip_deta('USER_TEL')"  />
                            </div>
                          </div>
                        </div>
                      </li>
                   </ul>
                </div>
                
                <wx-img-upload :imgs="forever_qrcode_obj.imgs" left_text="身份证"></wx-img-upload>

                 <div class="list-block" style="margin-top:0px;">
                    <ul>
                      <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">工作单位及岗位</div>
                          </div>
                        </div>
                      </li>
                      <li class="align-top">
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-input">
                               <input type="text" placeholder="请输入工作单位及岗位" v-model="forever_qrcode_obj.WORK_UNIT" @keyup="save_trip_deta('WORK_UNIT')" />
                            </div>
                          </div>
                        </div>
                      </li>
                      <li class="align-top">
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">紧急联系人及联系电话</div>
                          </div>
                        </div>
                      </li>
                      <li class="align-top">
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-input">
                               <input type="text" placeholder="紧急联系人及联系电话" v-model="forever_qrcode_obj.EMERGENCY_CONTACT" @keyup="save_trip_deta('EMERGENCY_CONTACT')"  />
                            </div>
                          </div>
                        </div>
                      </li>
                   </ul>
                </div>

                 <div class="content-block">
                     <p><a href="#" class="button button-fill button-big">提交 </a></p>
                  </div>
            </div>

            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        var id = xyf_util.getQuery(me.url, "id", true);
                        
                        $.post("/Wxhb/Handlers/UserHandler.ashx", { action: 'GET_FOREVER_QRCODE_APPLY', id: id }, function (result) {

                            if (!result.success) {

                                $.toast(result.error_msg);

                                return;
                            }


                            var time = null;

                            obj.my_vue = new Vue({
                                el: el.children(".content")[0],
                                data: {
                                    forever_qrcode_obj: result.data,
                                    change_fields: []
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
                                                        table_name: 'UT_006'
                                                    }, function (result) {

                                                        if (!result.success) {

                                                            console.error(result);

                                                            return;
                                                        }

                                                    }, "json");


                                            }, { interval: 500, limit: 1 });
                                        }


                                    }
                                }


                            });


                        }, "json");


                        el.on("click", ".button-big", function () {

                            $.post("/Wxhb/Handlers/UserHandler.ashx", { action: "SUBMIT_FOREVER_QRCODE_APPLY",id:id}, function (result) {

                                if (!result.success) {
                                    $.toast(result.error_msg);
                                    return;
                                }

                                $.toast(result.msg);

                            }, "json");


                        });

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
