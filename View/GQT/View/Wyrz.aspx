<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Wyrz.aspx.cs" Inherits="App.InfoGrid2.GQT.View.Wyrz" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <meta http-equiv="pragma" content="no-cach" />

    <meta http-equiv="cache-control" content="no-cache" />

    <meta http-equiv="expires" content="0" />

    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    <title>广州市南沙区青年联合会</title>

    <script src="/GQT/Script/common.js"></script>
</head>
<body style="max-width:520px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">


     <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">

            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title">我要入驻</h1>
                <a class="icon icon-home pull-right expand" href="/GQT/View/Home.aspx"></a>
            </header>

            <div class="content">

               <div class="list-block" style="margin-top:0px;">
                    <ul>
                      <!-- Text inputs -->
                      <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">姓名</div>
                            <div class="item-input">
                              <input type="text" placeholder="你的名字">
                            </div>
                          </div>
                        </div>
                      </li>
                       <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">地区</div>
                            <div class="item-input">
                                   <input type="text" placeholder="自己手写" >
                            </div>
                          </div>
                        </div>
                      </li>
                       <li>
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">联系方式</div>
                            <div class="item-input">
                                <input type="text"  placeholder="你的call机号码啦"/>
                            </div>
                          </div>
                        </div>
                      </li>
                     <li class="align-top">
                        <div class="item-content">
                          <div class="item-inner">
                            <div class="item-title label">申报项目</div>
                            <div class="item-input">
                              <textarea placeholder="反正写很厉害就是啦！"></textarea>
                            </div>
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

                        
                        

        

                    }


                    return obj;

                }

            </script>

        </div>

    </div>


</body>
</html>
