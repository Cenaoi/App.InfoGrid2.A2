<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReimList.aspx.cs" Inherits="App.InfoGrid2.WF.View.Report.ReimList" %>

<!DOCTYPE html>

<html>
<head >
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />


    <title>报销列表</title>

    <script src="/WF/Script/common.js"></script>

</head>
<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">
    

    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">



            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">周报告</h1>
                <a class="icon icon-edit pull-right" style="cursor:pointer;"></a>
            </header>
            

            <div class="content">

                <div class="list-block media-list" style="margin:0px;" >
                    <ul>
                      <li v-for="r in reports">
                        <a href="#" class="item-link item-content" @click="item_click(r)" >
                          <div class="item-inner">
                            <div class="item-title-row">
                              <div class="item-title">{{r.COl_1}}  第{{r.COL_52}}周  {{r.COL_5}}  </div>
                              <div class="item-after">{{biz_sid_text(r.BIZ_SID)}}</div>
                            </div>
                            <div class="item-subtitle">主题: {{r.COL_26}}</div>
                          </div>
                        </a>
                      </li>
                    </ul>
                  </div>

            </div>

            <script type="text/javascript" data-main="true">
        
                function main() {
            
            
                    var obj = {};

                    //初始化事件
                    obj.onInit = function () {

                        var me = this,
                            el = me.el;

                        el.on("click",".icon-edit",function(e){
                        

                            $.post("/WF/Handlers/ReportHandler.ashx",{action:'GET_REPORT_ID'},function(result){
                            
                                if(!result.success){

                                    $.toast(result.error_msg);
                                    return;

                                }

                                var url = Mini2.urlAppend("ReimDeta.aspx",{id:result.data.ROW_IDENTITY_ID,show_type:"edit"},true);


                                $.router.load(url);

                            },"json");


                        });

                        
                        obj.initVue(me);

        

                    }


                    obj.reports = <%= GetReports() %>;

                    //vue对象
                    obj.my_vue = null;



                    //初始化vue相关对象
                    obj.initVue = function (me) {

                        var el = me.el;


                        obj.my_vue = new Vue({
                            el:el.children(".content")[0],
                            data:{
                                reports:obj.reports

                            },
                            methods:{
                                //周报告点击事件
                                item_click:function(r){

                                    var my_vue = this;


                                    //显示类型
                                    var show_type = 'edit';

                                    if(r.BIZ_SID > 0){

                                        show_type = "show";

                                    }


                                    var url = Mini2.urlAppend("ReimDeta.aspx",{id:r.ROW_IDENTITY_ID,show_type:show_type},true);


                                    $.router.load(url);


                                },
                                biz_sid_text:function(biz_sid){

                                    var me_vue = this;

                                    if(biz_sid > 0){

                                        return "已提交";

                                    }else{

                                        return "未提交";

                                    }

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
