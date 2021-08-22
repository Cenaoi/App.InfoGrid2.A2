<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WelcomeEmpty.ascx.cs" Inherits="App.InfoGrid2.View.Explorer.WelcomeEmpty" %>

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
                <img src="/res/login/centerBox3.png" style="margin-left:auto; margin-right:auto;" />
            </div>

        </div>

    </div>

</div>

<script src="/Core/Scripts/bootstrap/3.3.4/js/bootstrap.min.js"></script>


<script>


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


</script>

