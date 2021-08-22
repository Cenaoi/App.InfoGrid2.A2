<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SecFunDefTree.ascx.cs" Inherits="App.InfoGrid2.Sec.SecFunDefTree" %>
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

    <mi:ToolBar runat="server">
        <mi:ToolBarTitle Text="系统模块权限定义" />
        <mi:ToolBarButton Text="创建节点" />
    </mi:ToolBar>
    
    <table style="width:100%;" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td style="width:300px;" valign="top">

    <mi:TreeView runat="server" ID="TreeView1" ShowCheckBox="true" 
                    DataPathField="SEC_FUN_DEF_ID" DefaultRootID="100" ParentField="PARENT_ID" 
                    TextField="TEXT" ValueField="SEC_FUN_DEF_ID" TypeField="FUN_TYPE_ID" 
                    CheckValueField="ENABLE"
                    OnSelectNodeClientScript="selectNode(event, data);" >
        <Types>
            <mi:TreeNodeType Name="page" Icon="/res/icon/application_view_columns.png" />
        </Types>
    </mi:TreeView>
    <mi:HiddenField runat="server" ID="TreeView_CheckedIds" ClientIDMode="Static" />

            </td>
            <td valign="top" 
                style="background-color: #FFFFFF; border-style: groove; border-width: 2px">
                
                <iframe id="SecFunList_Iframe" src="SecFunDefList.aspx" style="width:600px;height:300px;" frameborder="0">
                
                
                </iframe>

            </td>
        </tr>
    </table>

    <button type="button" onclick="getMenuIds()">获取被选中的</button>
</form>

<script type="text/javascript">

    //取得选中的菜单id  
    function getMenuIds() {

        //取得所有选中的节点，返回节点对象的集合  
        var checkeds = $("#widget1_I_TreeView1").find(".jstree-checked");

        var undes = $("").find(".jstree-undetermined");
        
        //得到节点的id，拼接成字符串   
        var ids = "";

        for (var i = 0; i < checkeds.size(); i++) {
            ids += checkeds[i].value + ",";
        }

        ids += "-----";

        for (var i = 0; i < undes.size(); i++) {
            ids += undes[i].value + ",";
        }

        //写回表单   
        $('#TreeView_CheckedIds').val(ids);

        alert(ids);
    }

    function selectNode(event, data) {
        var value = data.rslt.obj.attr("value");


        var frm = window.frames["SecFunList_Iframe"];

        frm.location.href = "SecFunDefList.aspx?moduleId=" + value;


    }

</script>