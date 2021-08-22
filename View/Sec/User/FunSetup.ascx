<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FunSetup.ascx.cs" Inherits="App.InfoGrid2.Sec.User.FunSetup" %>
<%@ Register Assembly="EasyClick.BizWeb" Namespace="EasyClick.BizWeb.UI" TagPrefix="biz" %>
<%@ Register Assembly="EasyClick.Web.Mini" Namespace="EasyClick.Web.Mini" TagPrefix="mi" %>
<%@ Register assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="System.Web.UI.HtmlControls" tagprefix="cc1" %>
<!--出口舱单-->

<% if (false)
   { %>
<link href="/App_Themes/Mini/mini-1.0.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>
<link href="/Core/Scripts/ui-lightness/jquery-ui-1.8.6.custom.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
<script src="/Core/Scripts/validate/jquery.validate-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>
<% } %>

<form method="post">
    <a href="#" id="TmpButton" style="display:none;">...</a>
    <div>
        <mi:Label runat="server" ID="TitleLab"  style="font-size: 18px; font-weight:bold; color: #666666"  >船期表维护</mi:Label>
    </div>

    <br />
        &nbsp;&nbsp;&nbsp<a href="#" onclick="checkAll();return false;">全选</a>
        &nbsp;&nbsp;<a href="#" onclick="clearAll();return false;">全不选</a>
        &nbsp;&nbsp;<a href="#"  onclick="uncheck();return false;">反选</a>
    <br /><br />
                
    <mi:CheckBoxList runat="server" ID="CheckBoxList1" RepeatColumn="1" >
    </mi:CheckBoxList>

</form>

<script type="text/javascript">

    $(document).ready(function () {

        var list = $("#widget1_I_CheckBoxList1").find(":checkbox");

        list.change(Post_Save);
    });

    function Post_Save() {

        widget1.submit("#TmpButton", { action: "Submit" });
    }

    function checkAll() {
        /// <summary>全选</summary>

        var items = $("#widget1_I_CheckBoxList1").find(":checkbox");

        items.attr("checked", "checked");

        Post_Save();
    }

    function clearAll() {
        /// <summary>全不选</summary>

        var items = $("#widget1_I_CheckBoxList1").find(":checkbox");

        items.removeAttr("checked");

        Post_Save();
    }

    function uncheck() {
        /// <summary>反选</summary>

        var items = $("#widget1_I_CheckBoxList1").find(":checkbox");

        $(items).each(function () {

            if ($(this).attr("checked") == "checked") {
                $(this).removeAttr("checked");
            }
            else {
                $(this).attr("checked", "checked");
            }

        });


        Post_Save();
    }

</script>