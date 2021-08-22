<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdveHistoryMsg.aspx.cs" Inherits="App.InfoGrid2.Wxhb.View.Msg.AdveHistoryMsg" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>地图红包</title>

    <script src="/Wxhb/Script/common.js"></script>

</head>
<body  style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">

    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            
            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title def_title">历史消息</h1>
            </header>

            <div class="content">

                <div class="list-block" style="margin:0.5rem 0;">
                    <ul>
                      <li>
                          <div class="item-content"  >
                              <div class="item-inner">
                                  <div class="item-title" style="font-size:0.7rem;white-space:inherit;" >你好，我是小渔夫真的不知道要写什么东西了，你能给我显示换行吗</div>
                                  <div class="item-after" style="color:red;">2017-02-01</div>
                              </div>
                          </div>
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

                        var adve_code = xyf_util.getQuery(me.url, "adve", true);


                        $.post("/App/InfoGrid2/Wxhb/Handlers/MsgHandler.ashx", { action: 'GET_ADVE_HISTORY_MSGS' }, function (result) {

                            if (!result.success) {

                                $.alert(result.error_msg);

                                return;

                            }





                        }, "json");


                        
                        

        

                    }

                    //vue对象
                    obj.my_vue = null;


                    //初始化Vue对象
                    obj.initVue = function () {





                    }


                    //页面恢复的事件
                    obj.pageRevert = function () {
		                var me = this,
                            el = me.el;
                    }

                    return obj;

                }

            </script>

        </div>

    </div>
    
</body>
</html>
