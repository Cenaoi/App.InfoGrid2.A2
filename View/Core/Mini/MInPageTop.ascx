<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.UserControl" %>
<link href="/App_Themes/Mini/mini-1.0.css" rel="stylesheet" type="text/css" />
<!--MIn模式-->
<%
    bool isRemove = false;

    if (isRemove)
    {
        this.JsHome = "http://www.aoocom.com";
    }    
%>
<% if(IsIE6789()) { %>
    <style type="text/css">@import '<%=JsHome %>/Core/Scripts/fauxconsole/fauxconsole.css';</style>
    <script src="<%=JsHome %>/Core/Scripts/fauxconsole/fauxconsole.js" type="text/javascript"></script>
<% } %>
<%--    <script src="http://libs.baidu.com/jquery/1.8.3/jquery.min.js"></script>
    <script src="http://libs.baidu.com/jqueryui/1.9.2/jquery-ui.min.js "></script>
--%>
    <script src="<%=JsHome %>/Core/Scripts/jquery/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="<%=JsHome %>/Core/Scripts/jquery.ui/ui/jquery.ui.datepicker.js" type="text/javascript"></script>

    <link href="<%=JsHome %>/Core/Scripts/ui-lightness/css/smoothness/jquery-ui-1.9.2.custom.css" rel="stylesheet" type="text/css" />

    
    <script src="<%=JsHome %>/Core/Scripts/Tongji/Tongji-0.1.js" type="text/javascript"></script>

    <script src="<%=JsHome %>/Core/Scripts/jquery.metadata.js" type="text/javascript"></script>
    <script src="<%=JsHome %>/Core/Scripts/jquery.form.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(window).validate || document.write('<script src="<%=JsHome %>/Core/Scripts/validate/jquery.validate.min-1.10.0.js"><\/script>');
    </script>
    <script src="<%=JsHome %>/Core/Scripts/validate/lang/zh-cn.js" type="text/javascript"></script>

    <script src="<%=JsHome %>/Core/Scripts/JQuery.Tools/jquery.tools.min.js" type="text/javascript"></script>

    <link href="<%=JsHome %>/Core/Scripts/ui-lightness/redmond/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <script src="<%=JsHome %>/Core/Scripts/ui-lightness/jquery.ui.datepicker-zh-CN.js" type="text/javascript"></script>

    <script src="<%=JsHome %>/Core/Scripts/jtemplates/jquery-jtemplates.js" type="text/javascript"></script>
    <script src="<%=JsHome %>/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>
    <script src="<%=JsHome %>/Core/Scripts/jquery.powerFloat/jquery-powerFloat-min.js" type="text/javascript"></script>

    <script src="<%=JsHome %>/Core/Scripts/Mini/1.1/MiniHtml.min.js" type="text/javascript"></script>
    <script src="<%=JsHome %>/Core/Scripts/Mini/TableTemplate.js" type="text/javascript"></script>

    <link href="<%=JsHome %>/Core/Scripts/sticky/sticky.css" rel="stylesheet" type="text/css" />
    <script src="<%=JsHome %>/Core/Scripts/sticky/sticky.js" type="text/javascript"></script>
    
    <script src="<%=JsHome %>/Core/Scripts/In/in-min.js" autoload="true" core="<%=JsHome %>/Core/Scripts/Mini2/In_M2_dev.Config.js" ></script>


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

    public string JsHome = string.Empty;

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