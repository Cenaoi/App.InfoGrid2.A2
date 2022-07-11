<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Main_v201608.ascx.cs" Inherits="App.InfoGrid2.View.Explorer.Main_v201608" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<% if (false)
   { %>
<link href="/App_Themes/Blue/common.css" rel="stylesheet" type="text/css" />
<link href="/App_Themes/Vista/table.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>
<link href="/Core/Scripts/ui-lightness/jquery-ui-1.8.6.custom.css" rel="stylesheet"
    type="text/css" />
<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
<script src="/Core/Scripts/validate/jquery.validate-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>
<script src="/Core/Scripts/Mini/_CodeHelp.js" type="text/javascript"></script>

<link href="/Core/Scripts/Mini2/Themes/theme-globel.css" rel="stylesheet" type="text/css" />
<link href="/Core/Scripts/Mini2/Themes/theme-window.css" rel="stylesheet" type="text/css" />
<% } %>
<link href="/App/BizExplorer2007/Menu2013.1.css" rel="stylesheet" type="text/css" />

    <link href="/Core/scripts/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        body {
            background-color: #0f88d0;
            color:#ffffff;
            font-size:12px;
        }

        .navbar-yqic a {
            color: #dae9f2;
        }
                
        .navbar-yqic a:hover {
            color:#ffffff;
            background-color:#6d9eca !important;
        }

    </style>



<script src="/Core/scripts/bootstrap/3.3.4/js/bootstrap.min.js"></script>

<!-- 头部导航条 -->
<nav class="navbar navbar-yqic" style="border-bottom:1px solid #96ceef;">
        <div class="container-fluid" >

            <div class="navbar-header">
                <a class="navbar-brand" onclick="goHone()"><%= GetCompanyName() %></a>
            </div>

            <div class="collapse navbar-collapse">

                <ul class="nav navbar-nav navbar-right " id="user_info" >
                    <li><a href="#" id="timeTb1">T</a></li>
                    <li><a href="#" id="userNameTb1">管理员</a></li>
                    <li><a href="/App/InfoGrid2/Login/Logout.aspx" >退出</a> </li>
                </ul>
            </div>
        </div>
        <div style="height:50px; background-color:#e8f2fc;"></div>
</nav>

<mi:Viewport runat="server" ID="TopViewport" MarginTop="56" Scroll="None" >
    <mi:TabPanel runat="server" ID="layout1" Region="Center" ButtonVisible="false" Plain="true" State="Normal" TabLeft="20" UI="win10">
<%--            
        <mi:Tab runat="server" ID="welcome1" Text=" 欢 迎! " Closable="false" >
            <iframe src="/App/InfoGrid2/View/Explorer/WelcomeKXJ.aspx" dock="full" style="border:none;" >

            </iframe>
        </mi:Tab>
    --%>
    </mi:TabPanel>
    <mi:Panel runat="server" Height="10" Region="South" ID="topFooter">

    </mi:Panel>
</mi:Viewport>


<script type="text/javascript">

    var tabs =  null;
    

    function AddTab(url, label, closable, autoFocus, parentPage) {
        
        if (undefined === closable) {
            closable = true;
        }

        tabs = tabs || window['widget1_I_layout1'];


        var findTab = null;



        for (var i = 0; i < tabs.items.length; i++) {
            var srcTab = tabs.items[i];

            if (srcTab.url == url) {
                findTab = srcTab;
                break;
            }
        }

        if (findTab != null) {

            tabs.setActiveTab(findTab);
        }
        else {
            
            var tab = tabs.add({
                url: url,
                text: label,
                closable: closable,
                iframe: true,
                scroll: 'none',
                parentPage:parentPage
            });

            if (autoFocus == undefined || autoFocus === true) {
                tabs.setActiveTab(tab);
            }
        }


        //var panel = tab.panel;

        //$(panel).attr("srcUrl", url);

    }


    function proMenuData(data, status) {

        $("#HomeMenu").html(data);

        $('#mobanwang_com li').hover(function () {
            $(this).children('ul').stop(true, true).slideDown('slow');
        }, function () {
            $(this).children('ul').stop(true, true).hide();
        });

        $('#mobanwang_com li').hover(function () {
            $(this).children('div').stop(true, true).slideDown('slow');
        }, function () {
            $(this).children('div').stop(true, true).hide();
        });

    }


    var m_TimeX = null;

    $(document).ready(function () {

        
        setTimeout(function () {

            AddTab("/App/InfoGrid2/View/Explorer/Menu_v201608.aspx", "用户桌面", false, false);
            
            <% if(IsBuilder()){ %>
                AddTab("/App/InfoGrid2/View/Explorer/Menu_v201608.aspx?menu_type=builder", "设计师桌面", false,false);
            <% } %>
        }, 500);

        if (m_TimeX == null) {
            m_TimeX = $("#timeTb1");
        }

        setTimeout(getUserInfo, 1000);

        Mini2.setTimer(function () {

            getUserInfo();

        }, 10 * 1000);
    });

    var m_ResetLoginFrm = null;

    var m_Offline = null;

    var isReq = false;

    function getUserInfo() {

        if (isReq) {
            return;
        }

        isReq = true;

        var offline = m_Offline || (m_Offline = Mini2.create('Mini2.ui.Offline', {
            autoShow: false
        }));
        

        $.ajax({
            url:'/app/InfoGrid2/View/Explorer/SysHeartbeat.aspx?tag=USER_INFO', 
            
            error: function () {
                offline.show();
                console.warn('怀疑网络断开.');
                isReq = false;
            },

            success: function (data, state) {
                try {

                    var obj = eval('(' + data + ')');

                    if (obj.loginVaild != "1") {
                        window.location.href = "/app/infogrid2/login/index2.aspx";

                        alert('登录已过期，请重新登录！');
                    }
                    else if (obj.loginState == "1" ) {

                        var panel = $('#user_info');
                        var timeXEl = panel.find('#timeTb1');
                        var uInfoEl = panel.find('#userNameTb1');

                        timeXEl.text(obj.time);
                        uInfoEl.text(obj.loginName + '(' + obj.roleName + ')');

                        if (!obj.loign) {

                            //window.location.href = "/app/infogrid2/login/index2.aspx";
                            if (!m_ResetLoginFrm || (m_ResetLoginFrm && m_ResetLoginFrm.isDispose)) {

                                m_ResetLoginFrm = Mini2.createTop('Mini2.ui.Window', {
                                    url: '/app/InfoGrid2/View/Explorer/ResetLogin.aspx?' + Mini2.Guid.newGuid(),
                                    text: '登录超时,请重新登录',
                                    width: 400,
                                    height: 200,
                                    mode: true
                                });

                                m_ResetLoginFrm.formClosed(function (e) {

                                    if (e.success) {

                                    }
                                    else {

                                        m_ResetLoginFrm = null;

                                        window.location.href = "/app/infogrid2/login/index2.aspx";
                                    }
                                });

                                m_ResetLoginFrm.show();

                            }
                        }
                        else if (m_ResetLoginFrm) {
                            m_ResetLoginFrm.close({ success: true });

                            m_ResetLoginFrm = null;
                        }
                    }
                    else {
                        
                        window.location.href = "/app/infogrid2/login/index2.aspx";

                        alert('账号已在别处登录，请保管好自己的用户密码！');
                    }

                    offline.hide();
                }
                catch (ex) {
                
                    offline.show();

                    console.warn('怀疑网络断开.', ex);

                }

                isReq= false;

            }
        });
    }



    $(document).ready(function () {
        
        $.get('Menu_v2015.aspx', proMenuData);
    });

    window.Mini_IsMainWindow = true;


    /**
     * 自动调用页面的刷新事件
     *
     */
    Mini2.ready(function () {

        var tabPanel = Mini2.find('layout1');

        tabPanel.bind('tabAction', function (e) {

            var tab = e.tab;

            var win = tab.getIFrameWindow();
            
            if (win) {
                var m2 = win.Mini2;

                if (m2 && m2.onwerPage.reload) {
                    m2.onwerPage.reload();
                }
                else if (win.reload) {
                    win.reload();
                }
            }
            
        });

    });

</script>