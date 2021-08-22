<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.UserControl" %>
<%@ Import Namespace="EasyClick.BizWeb" %>



    <%= BizRes.Write("/Core/Scripts/jquery/jquery-3.1.0.js") %>
    <%= BizRes.Write("/Core/Scripts/vue/vue-2.0.1.js") %>
    <%= BizRes.Write("/Core/Scripts/moment/moment-2.9.js") %>
    <%= BizRes.Write("/Core/Scripts/jtemplates/jquery-jtemplates-0.8.4.js") %>

    <!--简单图片预览-->
    <%= BizRes.Write("/Core/Scripts/jquery.fs.boxer/css/jquery.fs.boxer.css") %>
    <%= BizRes.Write("/Core/Scripts/jquery.fs.boxer/jquery.fs.boxer-3.0.min.js") %>

    <!--多功能图片预览-->
    <%= BizRes.Write("/Core/Scripts/Viewer/viewer-0.7.2.min.css") %>
    <%= BizRes.Write("/Core/Scripts/Viewer/viewer-0.7.2.min.js") %>


    <%= BizRes.Write("/Core/Scripts/jquery.metadata.js") %>
    <%= BizRes.Write("/Core/Scripts/jquery.form.min.js") %>
    <%= BizRes.Write("/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js") %>
    <%= BizRes.Write("/Core/Scripts/validate/1.11.1/jquery.validate.min.js") %>
    <%= BizRes.Write("/Core/Scripts/Mini/1.1/MiniHtml.min.js") %>
    <%= BizRes.Write("/Core/Scripts/jquery.ui/ui/jquery.ui.datepicker.js") %>


    <%= BizRes.Write("/Core/Scripts/font-awesome-4.7.0/css/font-awesome.min.css") %>

    <% if (false)
       { %>
    
         <%= BizRes.Write("/Core/Scripts/Mini2/Mini2.min.js") %>
    <% }
       else
       {
           string mapPath = MapPath("~/Core/Scripts/Mini2/Mini2.join.ini");
           string[] jsLines = System.IO.File.ReadAllLines(mapPath);
           
           foreach (string item in jsLines)
           {
                if(string.IsNullOrEmpty(item) || item.Trim().StartsWith("--")){
                    continue;   
                }
                
                Response.Write(BizRes.Write(string.Format("/Core/Scripts/Mini2/dev/{0}",item)));
           }
                
            //Response.WriteFile("~/Core/Scripts/Mini2/Mini2.script.txt");
        } %>

    <%= BizRes.Write("/Core/Scripts/Mini2/extend/ui/TextWindow.js") %>
    <%= BizRes.Write("/Core/Scripts/Mini2/extend/ui/PullSwitch.js") %>

    <%= BizRes.Write("/Core/Scripts/jquery.ui/themes/base/jquery.ui.core.css") %>
    <%= BizRes.Write("/Core/Scripts/jquery.ui/themes/base/jquery.ui.theme.css") %>
    <%= BizRes.Write("/Core/Scripts/jquery.ui/themes/base/jquery.ui.datepicker.css") %>

    <%= BizRes.Write("/Core/Scripts/Mini2/Themes/theme-globel.css") %>
    <%= BizRes.Write("/Core/Scripts/Mini2/Themes/theme-window.css") %>
    <%= BizRes.Write("/Core/Scripts/Mini2/Themes/Win8/theme-win8.css") %>

<%= BizRes.Write("/Core/Scripts/Mini2/Themes/theme-Triton.css") %>
    <%= BizRes.Write("/Core/Scripts/Mini2/Themes/theme-cartoon.css") %>
    <%= BizRes.Write("/Core/Scripts/Mini2/Themes/theme-Triton.Bootstrap3.css") %>

<%--    <%= BizRes.Style("/Core/Scripts/Mini2/Themes/theme-deepen50.css") %>--%>

    <%= BizRes.Write("/Core/Scripts/Base64.js") %>

    <script type="text/javascript">
        $.metadata.setType("attr", "validate");
    </script>

    <% if ( EasyClick.BizWeb2.BizServer.IsTouch())
        { %>
        
        <%= BizRes.Write("/Core/Scripts/Mini2/Themes/theme-Touch.css") %>

        <script type="text/javascript">
            Mini2.SystemInfo.touchMode();
        </script>

    <% }
    else
    {%>

        <script type="text/javascript">
            Mini2.SystemInfo.bootstrap();
        </script>

<%--        <link href="<%= JsHome %>/Core/Scripts/Mini2/Themes/theme-compact.css" rel="stylesheet" />

        <script type="text/javascript">
            Mini2.SystemInfo.compactMode();
        </script>
        
--%>


    <% } %>
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