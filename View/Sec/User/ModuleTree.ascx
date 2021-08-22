<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleTree.ascx.cs" Inherits="App.InfoGrid2.Sec.User.ModuleTree" %>
<%@ Register Assembly="EasyClick.BizWeb" Namespace="EasyClick.BizWeb.UI" TagPrefix="biz" %>
<%@ Register Assembly="EasyClick.Web.Mini" Namespace="EasyClick.Web.Mini" TagPrefix="mi" %>
<%@ Register assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="System.Web.UI.HtmlControls" tagprefix="cc1" %>


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

<script src="/Core/Scripts/jstree_pre1.0_stable/jquery.jstree.js" type="text/javascript"></script>

<form method="post">
    <a href="#" id="TmpButton" style="display:none;">...</a>
    <mi:ToolBar ID="ToolBar1" runat="server">
        <mi:ToolBarTitle Text="角色权限设定" />
        <mi:ToolBarButton Text="保存" Command="SaveTree" OnClick="SaveTree()" />
    </mi:ToolBar>
    <table style="width:100%;" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td style="width:300px;" valign="top">

                <mi:TreeView runat="server" ID="TreeView1" 
                                DataPathField="SEC_FUN_DEF_ID" DefaultRootID="100" ParentField="PARENT_ID" 
                                TextField="TEXT" ValueField="SEC_FUN_DEF_ID" TypeField="FUN_TYPE_ID"
                                OnSelectNodeClientScript="Seelct_FunList(event, data);" 
                                ShowCheckBox="true" CheckValueField="VISIBLE">
                    <Types>
                        <mi:TreeNodeType Name="page" Icon="/res/icon/application_view_columns.png" />
                    </Types>
                </mi:TreeView>

                <mi:HiddenField runat="server" ID="TreeView_CheckedIds" ClientIDMode="Static" />
                <mi:HiddenField runat="server" ID="TreeView_StateUndeterminedIds" ClientIDMode="Static" />

            </td>
            <td valign="top" 
                style="background-color: #FFFFFF; border-style: groove; border-width: 2px">
                
                <iframe id="SecFunList_Iframe" src="FunList.aspx?moduleId=100" style="width:700px;height:500px;" frameborder="0">
                                    
                </iframe>

            </td>
        </tr>
    </table>


</form>


<script type="text/javascript">

    function Seelct_FunList(event, data) {

        var value = data.rslt.obj.attr("value");

        var frm = window.frames["SecFunList_Iframe"];

        frm.location.href = "FunSetup.aspx?moduleId=" + value + "&userId=" + $.query.get("userid");


        var node = data.rslt.obj, inst = data.inst;
        if (node.hasClass('jstree-closed')) { return inst.open_node(node); }
        //if (node.hasClass('jstree-open')) { return inst.close_node(node); }

    }
    //jstree-undetermined jstree-closed

    function GetCheckedIds() {

        //取得所有选中的节点，返回节点对象的集合  
        var checkeds = $("#widget1_I_TreeView1").find(".jstree-checked");

        //得到节点的id，拼接成字符串   
        var ids = "";

        for (var i = 0; i < checkeds.size(); i++) {
            ids += checkeds[i].value + ",";
        }

        return ids;
    }

    function GetUndeterminedIds() {

        //取得所有选中的节点，返回节点对象的集合  
        var checkeds = $("#widget1_I_TreeView1").find(".jstree-undetermined");

        //得到节点的id，拼接成字符串   
        var ids = "";

        for (var i = 0; i < checkeds.size(); i++) {
            ids += checkeds[i].value + ",";
        }

        return ids;
    }


    //取得选中的菜单id  
    function SaveTree() {

        var checkIds = GetCheckedIds();

        var undetIds = GetUndeterminedIds();

        //写回表单   
        $('#TreeView_CheckedIds').val(checkIds);

        $('#TreeView_StateUndeterminedIds').val(undetIds);

        widget1.submit("#TmpButton", { action: "Submit" });
    }

</script>