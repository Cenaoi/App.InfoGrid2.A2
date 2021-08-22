<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Zxzx.aspx.cs" Inherits="App.InfoGrid2.GQT.View.Zxzx" %>

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
                <h1 class="title def_title">在线咨询</h1>
                <a class="icon icon-home pull-right expand" href="/GQT/View/Home.aspx"></a>
            </header>

            <!-- 工具栏 -->
            <nav class="bar bar-tab">
               
                <input type="text"  style="box-sizing: border-box;border: none;border-radius: 0 0 0 0;box-shadow: none;display:inline;padding: 0 0 0.25rem;margin: 0;width: 80%;height: 2.5rem;color: #3d4145;font-size: 0.85rem;font-family: inherit;" />
                <button class="button button-success button-fill" style="display:inline;height: 100%;margin: 0px;width: 3.3rem;top:0px;">发送</button>
            </nav>


            <div class="content">


                    <div style="margin-top:1rem;">
                        <div  style="height:1.4rem;margin-left:0.9rem;font-size:0.6rem;">
                            <span>在线客服</span>
                            <span style="margin-left:0.5rem;">星期一  15：08</span>
                        </div>
                        <span style="background-color:white;color:black;padding:0.5rem;border-radius:0.5rem;margin-left:0.5rem;">
                               你好，有什么要问的吗？
                        </span>
                    </div>

                    <div style="margin-top:1rem;text-align:right;">
                        <div  style="height:1.4rem;margin-left:0.9rem;font-size:0.6rem;">
                            <span>星期一  15：09</span>
                            <span style="margin-left:0.5rem;">自己</span>
                        </div>
                        <span style="background-color:white;color:black;padding:0.5rem;border-radius:0.5rem;margin-left:0.5rem;">
                               额，我只是心情不好，想找个人聊天！
                        </span>
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
