<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.UserControl" %>
<link href="/App_Themes/Mini/mini-1.0.css" rel="stylesheet" type="text/css" />
<% if(IsIE6789()) { %>
    <style type="text/css">@import '/Core/Scripts/fauxconsole/fauxconsole.css';</style>
    <script src="/Core/Scripts/fauxconsole/fauxconsole.js" type="text/javascript"></script>
<% } %>
<%--警告：JQuery 1.8.0 无法支持某些插件。所以禁止升级 1.8.0--%>
<!--In 模式-->

<% #if(false) %>
<script src="/Core/Scripts/jquery/jquery-1.7.2.js" type="text/javascript"></script>
<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.18.min.js" type="text/javascript"></script>
<% #else %>
<%--<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js" type="text/javascript"></script>--%>
<script type="text/javascript"> !window.jQuery && document.write('<script src="/Core/Scripts/jquery/jquery-1.7.2.min.js"><\/script>');</script>

<%--<script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.18/jquery-ui.min.js" type="text/javascript"></script>--%>
<script type="text/javascript"> window.jQuery && !window.jQuery.ui && document.write('<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.18.min.js"><\/script>');</script>
<% #endif %>

<link href="/Core/Scripts/ui-lightness/css/smoothness/jquery-ui-1.9.2.custom.css" rel="stylesheet" type="text/css" />
<link href="/Core/Scripts/ui-lightness/redmond/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">if (!window.console) { window.console = null; }</script>
<script src="/Core/Scripts/In/in-min.js" type="text/javascript" autoload="true" core="/Core/Scripts/In_Config.js?v=1.3" ></script>
<script src="/Core/Scripts/Tongji/Tongji-0.1.js" type="text/javascript"></script>
<script type="text/javascript">

    In.ready("mi._Default", "mi.Widget", "jq.form", "jq.metadata", "mi.EcView", function () {
        $.metadata.setType("attr", "validate");
        Mini.globel.loadingBoxId = "wait";
    });

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