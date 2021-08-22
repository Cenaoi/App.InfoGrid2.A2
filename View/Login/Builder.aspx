<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Builder.aspx.cs" Inherits="App.InfoGrid2.Login.Builder" %>

<!doctype html>
<html>
<head>
    <title>设计师</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="renderer" content="webkit">
        <!--[if lt IE 9]>
            <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
        <![endif]-->

        <style type="text/css">
            input {
                width: 270px;
                height: 42px;
                margin-top: 25px;
                padding: 0 15px;
                background: #1c1a00; /* browsers that don't support rgba */
                -moz-border-radius: 6px;
                -webkit-border-radius: 6px;
                border-radius: 6px;
                border: 1px solid #767600; /* browsers that don't support rgba */
                -moz-box-shadow: 0 2px 3px 0 rgba(0,0,0,.1) inset;
                -webkit-box-shadow: 0 2px 3px 0 rgba(0,0,0,.1) inset;
                box-shadow: 0 2px 3px 0 rgba(0,0,0,.1) inset;
                font-family: 'PT Sans', Helvetica, Arial, sans-serif;
                font-size: 14px;
                color: #d3c800;
                text-shadow: 0 1px 2px rgba(0,0,0,.1);
                -o-transition: all .2s;
                -moz-transition: all .2s;
                -webkit-transition: all .2s;
                -ms-transition: all .2s;
                outline:none;
    
                -webkit-appearance: none;
                -webkit-rtl-ordering: logical;
                -webkit-user-select: text;
                -webkit-writing-mode: horizontal-tb;
                writing-mode: lr-tb;
            }



            button {
                cursor: pointer;
                width: 300px;
                height: 44px;
                margin-top: 25px;
                padding: 0;
                background: #747300;
                -moz-border-radius: 6px;
                -webkit-border-radius: 6px;
                border-radius: 6px;
                border: 1px solid #fff100;
                -moz-box-shadow:
                    0 15px 30px 0 rgba(255,255,255,.25) inset,
                    0 2px 7px 0 rgba(0,0,0,.2);
                -webkit-box-shadow:
                    0 15px 30px 0 rgba(255,255,255,.25) inset,
                    0 2px 7px 0 rgba(0,0,0,.2);
                box-shadow:
                    0 15px 30px 0 rgba(255,255,255,.25) inset,
                    0 2px 7px 0 rgba(0,0,0,.2);
                font-family: 'PT Sans', Helvetica, Arial, sans-serif;
                font-size: 14px;
                font-weight: 700;
                color: #fff;
                text-shadow: 0 1px 2px rgba(0,0,0,.1);
                -o-transition: all .2s;
                -moz-transition: all .2s;
                -webkit-transition: all .2s;
                -ms-transition: all .2s;
                font-size:18pt;
            }

            button:hover {
                -moz-box-shadow:
                    0 15px 30px 0 rgba(255,255,255,.15) inset,
                    0 2px 7px 0 rgba(0,0,0,.2);
                -webkit-box-shadow:
                    0 15px 30px 0 rgba(255,255,255,.15) inset,
                    0 2px 7px 0 rgba(0,0,0,.2);
                box-shadow:
                    0 15px 30px 0 rgba(255,255,255,.15) inset,
                    0 2px 7px 0 rgba(0,0,0,.2);
            }

            button:active {
                -moz-box-shadow:
                    0 15px 30px 0 rgba(255,255,255,.15) inset,
                    0 2px 7px 0 rgba(0,0,0,.2);
                -webkit-box-shadow:
                    0 15px 30px 0 rgba(255,255,255,.15) inset,
                    0 2px 7px 0 rgba(0,0,0,.2);
                box-shadow:        
                    0 5px 8px 0 rgba(0,0,0,.1) inset,
                    0 1px 4px 0 rgba(0,0,0,.1);

                border: 0px solid #ef4300;
            }
        </style>
</head>
<body style="background-color: #262626; padding:0px; margin:0px;">

    <div id="builderId" style="width:20px;height:20px; margin-left:auto; margin-right:0px; background-color: #616161;">
    </div>

    <form id="form1" method="post" >
    <div>

        <div style="height:100px;"></div>
        <div style="margin-right:auto; margin-left:auto; width:400px; text-align:center;">
            
            
            <input type="text" class="login-name" name="LoginName" value="<%= m_LoginName %>" placeholder="请输入您的用户名！" autocomplete="off" />
            <br />
            <input type="password" class="login-name" name="LoginPass" value="<%= m_LoginPass %>" placeholder="请输入您的密码！" />
            <br />
            <button type="submit" name="Action" value="GoSubmit" >提交</button>

        </div>
    </div>
    </form>
    
        <script src="/Core/Scripts/jquery/jquery-1.8.3.min.js" type="text/javascript"></script>
</body>

    <script type="text/javascript">

        $(document).ready(function () {

            $('#builderId').click(function () {
                window.location.href = "index.aspx";
            });


            if (navigator.userAgent.toLowerCase().indexOf("chrome") >= 0) {
                $(window).load(function () {
                    $('ul input:not(input[type=submit])').each(function () {
                        var outHtml = this.outerHTML;
                        $(this).append(outHtml);
                    });
                });
            }

            setTimeout(function () {
                <%= AlertMsg %>
            }, 100);
        });

    </script>

</html>
