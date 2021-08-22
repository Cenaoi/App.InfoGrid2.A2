<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WelcomeGBuy.ascx.cs" Inherits="App.InfoGrid2.View.Explorer.WelcomeGBuy" %>


<!DOCTYPE html>

<link href="/Core/Scripts/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" />
<link href="/res/ace/ace.css" rel="stylesheet" />
<link href="/res/ace/font-awesome-4.4.0/css/font-awesome.min.css" rel="stylesheet" />
<link href="/res/ace/mx.css" rel="stylesheet" />


<div class="main-container" id="main-container">

    <div id="sidebar" class="sidebar responsive">

        <ul class="nav nav-list">

            <%= GetHtmlCommonMenu() %>

        </ul>
        <div class="sidebar-toggle sidebar-collapse" id="sidebar-collapse">
		    <i class="ace-icon fa fa-angle-double-left" data-icon1="ace-icon fa fa-angle-double-left" data-icon2="ace-icon fa fa-angle-double-right"></i>
		</div>
    </div>

    <div class="main-content">

        <div class="main-content-inner">

            <div class="page-content">


                <div style="text-align:center;">

                      <img src="https://images.pexels.com/photos/239918/pexels-photo-239918.jpeg?w=1260&h=750&auto=compress&cs=tinysrgb"/>

                </div>

             
            </div>

        </div>

    </div>

</div>

<script src="/Core/Scripts/bootstrap/3.3.4/js/bootstrap.min.js"></script>

<script src="/Core/Scripts/jquery/jquery-3.1.0.min.js"></script>



<script>

    $(function () {

        var w_height = $(window).height() - 50;

        $("#viewpreview2045").css("height", w_height);

    });


    var m_LastActiveEls = null;

    $(function () {
        $('body').addClass('no-skin');

        var itemEls = $('#sidebar').find('.dropdown-toggle');

        itemEls.click(function () {
            var parentEl = $(this).parent();

            var submenuEl = $(this).nextAll('.submenu');

            if (submenuEl.hasClass('nav-hide')) {
                submenuEl.slideDown('normal', function () {
                    submenuEl.removeClass('nav-hide');
                    submenuEl.addClass('nav-show');
                });

                $(parentEl).addClass('open');

            }
            else {
                submenuEl.slideUp('normal', function () {
                    submenuEl.removeClass('nav-show');
                    submenuEl.addClass('nav-hide');
                });

                $(parentEl).removeClass('open');
            }




            var parentEls = $(this).parents('li');


            $(m_LastActiveEls).removeClass('active');

            parentEls.addClass('active');

            m_LastActiveEls = parentEls;

            var href = $(this).attr('href');

            if (href == '#') {
                //var url = '/App/InfoGrid2/View/Explorer/Menu_v201608.aspx?p_id=' + item.ID;

                //Mini2.EcView.show(url, $(this).text());
            }
            else {
                Mini2.EcView.show(href, $(this).text());
            }


            return false;
        });


    });


    function execBizMethod(method_code, renderEl) {

        Mini2.get('/App/InfoGrid2/View/Biz/Core_Method/Action.aspx', {
            method: 'get',
            code: method_code
        },
        function (data) {
            $('#' + renderEl).html(data);
        });

    }


    function viewTab(sender) {

        var href = $(sender).attr('data-href');
        var text = $(sender).attr('data-text');

        console.debug("跳转地址");

        Mini2.EcView.show(href, text);

        return false;
    }



    function viewCMS() {

        Mini2.get('/App/InfoGrid2/View/CMS/CmsHandler.aspx?action=get_list', 
        function (data) {

            var my_vue = new Vue({
                el: '#cmsItems1',
                data: {
                    items: data
                },
                methods: {

                    viewTab: function (item) {

                        var url = '/App/InfoGrid2/View/Cms/CmsView.aspx?id=' + item.CMS_ITEM_ID;

                        Mini2.EcView.show(url, '公告');
                        Mini2.EventManager.stopEvent();
                    }
                }

            });


        });
    }


</script>



<%--<script type="text/javascript" src="http://echarts.baidu.com/gallery/vendors/echarts/echarts-all-3.js"></script>--%>
<script src="/Core/Scripts/echarts/echarts.min.js"></script>



