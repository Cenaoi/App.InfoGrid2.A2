<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MoreViewPreview.ascx.cs" Inherits="App.InfoGrid2.View.MoreView.MoreViewPreview" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<style type="text/css">
    .page-head
    {
         font-size:26px;
         font-weight:bold;
    }
</style>

<form action="" method="post">
<mi:Store runat="server" ID="store1" Model="" IdField="" PageSize="20" AutoFocus="false" >


</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">
    <mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="10">
        <mi:Label runat="server" ID="headLab" Value="单号" HideLabel="true" Dock="Center" Mode="Transform" />
    </mi:Panel>
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:FormLayout runat="server" ID="searchForm" Dock="Top" Region="North" FlowDirection="TopDown"
            ItemWidth="300" ItemLabelAlign="Right" ItemClass="mi-box-item" Layout="HBox" Visible="false"
            StoreID="store1" FormMode="Filter" Scroll="None">

        </mi:FormLayout>
       
        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarTitle ID="tableNameTB1" Text="表名" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm.toggle()" />
            <mi:ToolBarButton Text="导出" Icon="/res/file_ico/excel.png" Command="ToExcel" />
            <mi:ToolBarButton Text="打印" Command="btnPrint" />
            <mi:ToolBarButton Text="模板管理" Command="ManageTemplate" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" SummaryVisible="true">
            <Columns>
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>

<% if (EC5.SystemBoard.EcContext.Current.User.Roles.Exist(EC5.IG2.Core.IG2Param.Role.BUILDER))
{ %>
<div id="SwitchPanel" style="width:340px;height:40px;border: 1px solid #C0C0C0;background-color: #FFFFFF;">
    <div style="margin:8px 24px 8px 8px; text-align:right;">

        <mi:Button runat="server" ID="ToolbarSetup1" Command="ToolbarSetup" Text="工具栏定义"   />
        <mi:Button runat="server" ID="EditPageBtn" Command="StepEdit3" Text="关联定义" />
        <mi:Button runat="server" ID="Button2" Command="StepEdit5" Text="列设置" />
        <mi:Button runat="server" ID="Button1" Command="StepEdit4" Text="列高级设置" />
    </div>
</div>
<script type="text/javascript">

    $(document).ready(function () {

        var ps = Mini2.create('Mini2.ui.extend.PullSwitch', {
            panelId: 'SwitchPanel'
        });

        ps.render();
    });
</script>
<% } %>
</form>
