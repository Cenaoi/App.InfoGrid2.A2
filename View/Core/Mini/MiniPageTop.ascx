<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.UserControl" %>
<link href="/App_Themes/Mini/mini-1.0.css" rel="stylesheet" type="text/css" />
<%--
        
    警告：JQuery 8.0 无法支持某些插件。所以禁止升级 8.0
        
        
--%>
<!--普通模式-->
<% if(IsIE6789()) { %>
    <style type="text/css">@import '/Core/Scripts/fauxconsole/fauxconsole.css';</style>
    <script src="/Core/Scripts/fauxconsole/fauxconsole.js" type="text/javascript"></script>
<% } %>

<% if(IsMSIE6_7()) { %>
    <% #if(true) %>
    <script src="/Core/Scripts/jquery/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <% #else %>
    <%--<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript">            !window.jQuery && document.write('<script src="/Core/Scripts/jquery/jquery-1.7.2.min.js"><\/script>');</script>
    <%--<script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.18/jquery-ui.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript">            !window.jQuery && document.write('<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js"><\/script>');</script>
    <% #endif %>
<% } else { %>
    <% #if(true) %>
    <script src="/Core/Scripts/jquery/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="/Core/Scripts/ui-lightness/jquery-ui-1.9.2.custom.js" type="text/javascript"></script>
    <% #else %>
    <%--<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript">            !window.jQuery && document.write('<script src="/Core/Scripts/jquery/jquery-1.8.3.min.js"><\/script>');</script>
    <%--<script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.2/jquery-ui.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript">            !window.jQuery && document.write('<script src="/Core/Scripts/ui-lightness/jquery-ui-1.9.2.custom.min.js"><\/script>');</script>
    <% #endif %>

    <link href="/Core/Scripts/ui-lightness/css/smoothness/jquery-ui-1.9.2.custom.css" rel="stylesheet" type="text/css" />
<% } %>
    
    <script src="/Core/Scripts/Tongji/Tongji-0.1.js" type="text/javascript"></script>

    <script src="/Core/Scripts/jquery.metadata.js" type="text/javascript"></script>
    <script src="/Core/Scripts/jquery.form.js" type="text/javascript"></script>

    <%--<script src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.10.0/jquery.validate.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript">            $(window).validate || document.write('<script src="/Core/Scripts/validate/jquery.validate.min-1.10.0.js"><\/script>');</script>
    <script src="/Core/Scripts/validate/lang/zh-cn.js" type="text/javascript"></script>

    <script src="/Core/Scripts/JQuery.Tools/jquery.tools.min.js" type="text/javascript"></script>

    <link href="/Core/Scripts/ui-lightness/redmond/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Core/Scripts/ui-lightness/jquery.ui.datepicker-zh-CN.js" type="text/javascript"></script>

    <script src="/Core/Scripts/jtemplates/jquery-jtemplates.js" type="text/javascript"></script>
    <script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>
    <script src="/Core/Scripts/jquery.powerFloat/jquery-powerFloat-min.js" type="text/javascript"></script>

    <% #if(true) %>
    
    <script src="/Core/Scripts/Mini/_Default.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/DateHelper.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/AutoCompleteBox.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/Template.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/Pagination.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/SWFUpload.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/Fileupdate.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/TreeView.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/Widget.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/Window.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/Tooltip.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/DropDownText.js" type="text/javascript"></script>

    <%--单元格编辑控件--%>
    <script src="/Core/Scripts/Mini/SelectItemField.js" type="text/javascript"></script>

    <script src="/Core/Scripts/Mini/DataGridView.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/EditorTextCell.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/EditorSelectCell.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/EditorSelect2Cell.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/EditorCheckBoxCell.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/EditorDateCell.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/EditorTextButtonCell.js" type="text/javascript"></script>

    <script src="/Core/Scripts/Mini/DataGrid.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/EcView.js" type="text/javascript"></script>
    <% #else %>
    <script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>
    <% #endif %>




    <link href="/Core/Scripts/sticky/sticky.css" rel="stylesheet" type="text/css" />
    <script src="/Core/Scripts/sticky/sticky.js" type="text/javascript"></script>
    



    <script src="/Core/Scripts/imgPreview/imgpreview.full.jquery.js" type="text/javascript"></script>

    <script src="/Core/Scripts/SWFUpload/swfupload.js" type="text/javascript"></script>
    <script src="/Core/Scripts/SWFUpload/fileprogress.js" type="text/javascript"></script>
    <script src="/Core/Scripts/SWFUpload/handlers.js" type="text/javascript"></script>
    <link href="/Core/Scripts/SWFUpload/default.css" rel="stylesheet" type="text/css" />


    <script src="/Core/Scripts/jstree_pre1.0_stable/_lib/jquery.cookie.js" type="text/javascript"></script>
    <script src="/Core/Scripts/jstree_pre1.0_stable/jquery.jstree.js" type="text/javascript"></script>


    <script type="text/javascript">
        $.metadata.setType("attr", "validate");
        Mini.globel.loadingBoxId = "wait";
    </script>

    <style type="text/css">

    .ui-tabs .ui-tabs-panel { display: block; border-width: 0; padding: 2px 0px 0px 0px ; background: none; } 
    
    .ui-datepicker .ui-datepicker-title select {font-size:1em; margin:1px 0; }    
    .ui-datepicker select.ui-datepicker-month-year {width: 100%;}    
    .ui-datepicker select.ui-datepicker-month{margin-left:4px;}
    .ui-datepicker select.ui-datepicker-month{ width: 40%;vertical-align:top;}
    .ui-datepicker select.ui-datepicker-year { width: 43%;vertical-align:top;}

    .ui-datepicker .ui-datepicker-title select.ui-datepicker-year { float: left; }    
    ui-datepicker-title{vertical-align:middle;}

    </style>


<script runat="server">

    public bool IsMSIE6_7()
    {
        //return true;

        System.Web.HttpBrowserCapabilities browser = this.Request.Browser;

        if ("IE".Equals(browser.Browser.ToUpper(), StringComparison.Ordinal))
        {
            Version v = new Version(browser.Version);

            if (v.Major < 8)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsIE6789()
    {
        //return true;

        System.Web.HttpBrowserCapabilities browser = this.Request.Browser;

        if ("IE".Equals(browser.Browser.ToUpper(), StringComparison.Ordinal))
        {
            Version v = new Version(browser.Version);

            if (v.Major < 10)
            {
                return true;
            }
        }

        return false;
    }

</script>