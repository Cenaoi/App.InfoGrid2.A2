<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Gwcz.aspx.cs" Inherits="App.InfoGrid2.GQT.View.Gwcz" %>

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
<body style="max-width:480px;margin:auto;font-family:Microsoft YaHei,Helvitica,Verdana,Tohoma,Arial,san-serif;">
    

    <!-- 这是基本结构 -->
    <div class="page-group">

        <div class="page">


            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a class="icon icon-left pull-left back"></a>
                <h1 class="title def_title">岗位查找</h1>
                <a class="icon icon-home pull-right expand" href="/GQT/View/Home.aspx"></a>
            </header>
                        
            <nav class="bar bar-tab btn_nav">
              <a class="tab-item btn_agree"  href="#" style="background-color:#4cd964;">
                <span class="tab-label" style="color:white;" >我要实习</span>
              </a>
            </nav>

            <div class="content">


                  <div class="searchbar row">
                    <div class="col-25">
                        <select>
                            <option>岗位</option>
                            <option>学历</option>
                            <option>专业</option>
                        </select>
                    </div>
                    <div class="search-input col-60">
                      <input type="search" id='search' placeholder='输入关键字...'/>
                    </div>
                    <a class="button button-fill button-primary col-15"><span class="icon icon-search"></span></a>
                </div>

                
                  <div class="list-block media-list" style="margin-top:0px;">
                    <ul>
                      <li>
                        <a href="#" class="item-link item-content">
                          <div class="item-inner">
                            <div class="item-title-row">
                              <div class="item-title">会计</div>
                            </div>
                            <div class="item-text">某某公司需要一名能吃苦耐劳的会计</div>
                          </div>
                        </a>
                      </li>
                      <li>
                        <a href="#" class="item-link item-content">
                          <div class="item-inner">
                            <div class="item-title-row">
                              <div class="item-title">有经验的出纳</div>
                            </div>
                            <div class="item-text">我们公司需要一名有经验的出纳了！</div>
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

                        


                            
                    }


                    return obj;

                }

            </script>


        </div>

    </div>


</body>
</html>
