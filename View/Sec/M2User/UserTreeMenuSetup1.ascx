<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserTreeMenuSetup1.ascx.cs" Inherits="App.InfoGrid2.Sec.M2User.UserTreeMenuSetup1" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<% if (false)
   { %>
<link href="/App_Themes/Blue/common.css" rel="stylesheet" type="text/css" />
<link href="/App_Themes/Vista/table.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>
<link href="/Core/Scripts/ui-lightness/jquery-ui-1.8.6.custom.css" rel="stylesheet"
    type="text/css" />
<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
<script src="/Core/Scripts/validate/jquery.validate-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>
<script src="/Core/Scripts/Mini/_CodeHelp.js" type="text/javascript"></script>
<% } %>
<link rel="stylesheet" type="text/css" href="/Core/Scripts/jstree/3.0.2/themes/default/style.css" />
<script src="/Core/Scripts/jstree/3.0.2/jstree.js" type="text/javascript"></script>

<form action="" method="post">


<mi:Viewport runat="server" ID="viewport1">

    <mi:Panel runat="server" ID="LeftPanel" Region="West" Width="300" BackColor="White" Scroll="None">
        <mi:Toolbar runat="server" ID="toolbar1" Dock="Top">
            <mi:ToolBarButton Tooltip="保存" Command="GoSaveTreeChecked" Icon="/res/icon_sys/Save.png" />
        </mi:Toolbar>
        <mi:Panel runat="server" ID="Panel2" Region="West" Width="300" BackColor="White" Dock="Full">
            <mi:TreePanel runat="server" ID="TreePanel1" Dock="Full" Width="300" Region="Center"
                ShowCheckBox="true" >
                <Types>
                    <mi:TreeNodeType Name="default" Icon="/res/icon/application_view_columns.png" />
                    <mi:TreeNodeType Name="table" Icon="/res/icon/table.png" />
                    <mi:TreeNodeType Name="view" Icon="/res/icon/view.png" />
                </Types>
            </mi:TreePanel>
        </mi:Panel>
    </mi:Panel>
    <mi:Panel ID="Panel1" runat="server" Dock="Full" Region="Center" Scroll="None">
        <iframe dock="full" region="center" id="iform1_2" frameborder="0">
    
                

        </iframe>
    </mi:Panel>
</mi:Viewport>


</form>

<script>

    function GetCheckedIds() {

        //取得所有选中的节点，返回节点对象的集合  
        var checkeds = $("#widget1_I_TreeView1").find(".jstree-checked");

        //得到节点的id，拼接成字符串   
        var ids = "";

        for (var i = 0; i < checkeds.size() ; i++) {
            ids += checkeds[i].value + ",";
        }

        return ids;
    }

    function GetUndeterminedIds() {

        //取得所有选中的节点，返回节点对象的集合  
        var checkeds = $("#widget1_I_TreeView1").find(".jstree-undetermined");

        //得到节点的id，拼接成字符串   
        var ids = "";

        for (var i = 0; i < checkeds.size() ; i++) {
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
