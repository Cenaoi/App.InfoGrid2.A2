<%@ Page Language="C#" AutoEventWireup="true"  Inherits="System.Web.UI.Page" ValidateRequest="False" EnableViewState="False" EnableTheming="False" EnableEventValidation="False" %>
<%@ Register Assembly="EasyClick.BizWeb" Namespace="EasyClick.BizWeb.UI" TagPrefix="biz" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi2" %>
<!DOCTYPE html>
<html>
<head>
    <title><%= EC5.SystemBoard.SysBoardManager.CurrentApp.AppSettings["ExplorerTitle"] %></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="renderer" content="webkit">
    <style type="text/css">
    *{ font-size:12px;}
    #loading-mask{background-color:white;height:100%;position:absolute;left:0;top:0;width:100%;z-index:20000;}
    #loading{height:auto;position:absolute;left:40%;top:40%;padding:2px;z-index:20001;}
    #loading a {color:#225588;}
    #loading .loading-indicator{background:white;color:#444;font:bold 14px 新宋体;height:auto;margin:0;padding:10px;}
    #loading-msg{font-size: 12px;font-weight: normal;white-space: nowrap;}
    </style>

</head>
<body style="margin:0; padding:0;" leftMargin="0" topMargin="0" marginwidth="0" marginheight="0" >
    <div id="loading-mask" ></div>
    <div id="loading">
        <div class="loading-indicator">
            <img src="/res/extanim32.gif" width="32" height="32" style="margin-right:8px;float:left;vertical-align:top;"/>加载...
            <br /><span id="loading-msg"><input type="text" value="我在努力为您加载数据..." id="loading-text" style="border:none; width:200px;" /> </span>
        </div>
    </div>
    <div runat="server" id="loadAscx" style="display:none;">
    </div>

    <div id="wait" style="position: absolute; top: 350px; left: 0px; width:100%;  padding: 0px; display:none;"><table border="0" width="120" cellspacing="0" cellpadding="0" style="border: 0 solid #aab6ba" valign="top" align="center"><tr><td height="30" align="center" valign="middle">
        <img alt="正在下载,请稍候......" src="/res/image/loading.gif" style="width:197px;height:48px;" /></td></tr></table></div>

    <biz:Widget runat="server" ID="widget1" UriString="Admin/Views/New" Ajax="True" EcReturnFormat="script" EnableViewState="False" EnableTheming="False" EcDelay="False" CreateParentBox="false" />

</body>
<mi2:ScriptManager runat="server" ID="ScriptManager1" />
</html>

<% #if(false) %>
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<% #endif %>
<script type="text/javascript">
    function WaitResize() { $("#wait").width($(window).width()).css("top", $(window).height() / 3 - 24); }

    $(document).ready(function () {

        $('#loading-text').focus();

        $(window).resize(WaitResize);
        $(document).ready(WaitResize);

        $("#loading-mask").fadeOut(200, function () {
            $(this).hide();
        });
        $("#loading").hide();       
    });
</script>

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


    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void Render(HtmlTextWriter writer)
    {


        string jsMode = null;

        if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
        {
            jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
        }
        
        if (jsMode == "InJs")
        {
            loadAscx.Controls.Add(Page.LoadControl("~/Core/Mini/InPageTop.ascx"));
        }
        else if (jsMode == "MInJs")
        {
            loadAscx.Controls.Add(Page.LoadControl("~/Core/Mini/MInPageTop.ascx"));
        }
        else if(jsMode == "Full")
        {
            loadAscx.Controls.Add(Page.LoadControl("~/Core/Mini/MiniPageTop.ascx"));
        }
        else if(jsMode == "MInJs2")
        {
            loadAscx.Controls.Add(Page.LoadControl("~/Core/Mini/MInPageTop2.ascx"));
        }
        else
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            loadAscx.Controls.Add(Page.LoadControl("~/Core/Mini/MInPageTop2.ascx"));
        }

        base.Render(writer);
    }

    protected override void OnPreInit(EventArgs e)
    {
        //System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs";

        if (!this.IsPostBack)
        {

            this.widget1.UriString = Request.QueryString["ViewUri"];
        }

        base.OnPreInit(e);
    }

</script>