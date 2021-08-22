<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleTree.ascx.cs" Inherits="App.InfoGrid2.Sec.FunDefine.ModuleTree" %>
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

<script src="/Core/Scripts/jstree_pre1.0_stable/jquery.jstree.js" type="text/javascript"></script>

<form method="post">
    <a href="#" id="TmpButton" style="display:none;">...</a>
    <mi:ToolBar ID="ToolBar1" runat="server">
        <mi:ToolBarTitle Text="系统模块权限定义" />
        <mi:ToolBarButton Text="创建目录" Command="CreateNodeForDir" OnClick="widget1.submit(this);" />
        <mi:ToolBarButton Text="创建模块" Command="CreateNodeForModule" OnClick="widget1.submit(this);" />
        <mi:ToolBarButton Text="重命名节点" Command="RenameNode" OnClick="widget1.submit(this);" />
        <mi:ToolBarHr />
        <mi:ToolBarButton Text="删除节点" Command="DeleteNode" OnClick="widget1.submit(this);" />
    </mi:ToolBar>
    <table style="width:100%;" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td style="width:300px;" valign="top">

                <mi:TreeView runat="server" ID="TreeView1" 
                                DataPathField="SEC_FUN_DEF_ID" DefaultRootID="100" ParentField="PARENT_ID" 
                                TextField="TEXT" ValueField="SEC_FUN_DEF_ID" TypeField="FUN_TYPE_ID" 
                                OnSelectNodeClientScript="Seelct_FunList(event, data);" TextFormatString="{TEXT}({SEC_FUN_DEF_ID})">
                    <Types>
                        <mi:TreeNodeType Name="page" Icon="/res/icon/application_view_columns.png" />
                    </Types>
                </mi:TreeView>
                <mi:HiddenField runat="server" ID="TreeView_CheckedIds" ClientIDMode="Static" />

            </td>
            <td valign="top" 
                style="background-color: #FFFFFF; border-style: groove; border-width: 2px">
                
                <iframe id="SecFunList_Iframe" src="FunList.aspx?moduleId=100" style="width:700px;height:600px;" frameborder="0">
                
                
                </iframe>

            </td>
        </tr>
    </table>


</form>

<script type="text/javascript">

    function Seelct_FunList(event, data) {

        var value = data.rslt.obj.attr("value");

        var frm = window.frames["SecFunList_Iframe"];

        frm.location.href = "FunList.aspx?moduleId=" + value;


        var node = data.rslt.obj, inst = data.inst;
        if (node.hasClass('jstree-closed')) { return inst.open_node(node); }
        if (node.hasClass('jstree-open')) { return inst.close_node(node); }

    }
    

    function Edit_FormClosed(sender, e) {
        if (e.result != 'ok') { return; }

        var obj = e.item;

        widget1.submit("#TmpButton", { action: "ResetNodeText", actionPs: obj.SEC_FUN_DEF_ID });
    }

    function New_FormClosed(sender, e) {

        if (e.result != 'ok') { return; }

        var obj = e.item;
        
        widget1.submit("#TmpButton", { action: "LoadNode", actionPs: obj.SEC_FUN_DEF_ID });
    }

</script>