<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="App.InfoGrid2.Login.Index" %>
<html>
<!DOCTYPE html>
<html lang="cn" class="no-js">

    <head>

        <meta charset="utf-8">
        <title><%= GetCompanyName() %></title>
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="renderer" content="webkit">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <meta name="description" content="">
        <meta name="author" content="">

        <!-- CSS -->
        <link rel="stylesheet" href="/app/InfoGrid2/Login/assets/css/reset.css">
        <link rel="stylesheet" href="/app/InfoGrid2/Login/assets/css/supersized.css">
        <link rel="stylesheet" href="/app/InfoGrid2/Login/assets/css/style.css">

        <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
        <!--[if lt IE 9]>
            <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
        <![endif]-->

<%--        <style type="text/css">
            input:-webkit-autofill {   
                background-color: #FAFFBD;
                background-image: none;
                color: #000;
            }   
        </style>--%>
    </head>
   
    <body>
    
        <div id="builderId" style="width:20px;height:20px; margin-left:auto; margin-right:0px;">
            
        </div>
            
        <div class="page-container" style="width:800px;" >
            <form action="" method="post">
                <h1 style="letter-spacing:4px;"><%= GetCompanyName() %></h1>
                <input type="text" name="username" value="<%= m_LoginName %>" class="username" placeholder="请输入您的用户名！" autocomplete="off" style="width:340px;" />
                <br />
                <input type="password" name="password" class="password" placeholder="请输入您的用户密码！" autocomplete="off" style="width:340px;" />
                <br />
                <button type="submit" name="Action" value="Submit" class="submit_button" style="width:340px;">登录</button>

                <div class="error"><span>+</span></div>
            </form>
            <div class="connect">
                <p>easysoft@2014~2015</p>
            </div>
        </div>
		

        <!-- Javascript -->
        <script src="/Core/Scripts/jquery/jquery-1.8.3.min.js" type="text/javascript"></script>
        <script src="/app/InfoGrid2/Login/assets/js/supersized.3.2.7.min.js" ></script>
        <script src="/app/InfoGrid2/Login/assets/js/supersized-init.js" ></script>
        <script src="/app/InfoGrid2/Login/assets/js/scripts.js" ></script>

    </body>

    <script type="text/javascript">

        $(document).ready(function(){

            $('#builderId').click(function(){
                window.location.href= "builder.aspx";
            });


            setTimeout(function(){
                $('input').each(function(){
                    var text = $(this).val();
                    var name = $(this).attr('name');
                    $(this).after(this.outerHTML).remove();
                    $('input[name=' + name + ']').val(text);
                });
            },500);

        });

        <%= AlertMsg %>

    </script>

</html>


