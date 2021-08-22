<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataImportDialog.ascx.cs" Inherits="App.InfoGrid2.View.MoreView.DataImportDialog" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<form action="" method="post">
<mi:Store runat="server" ID="store1" Model="" IdField="" PageSize="20" EngineName="">
    
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:FormLayout runat="server" ID="searchForm" Dock="Top" Region="North" FlowDirection="TopDown"
            ItemWidth="300" ItemLabelAlign="Right" ItemClass="mi-box-item" Layout="HBox" Visible="false"
            StoreID="store1" FormMode="Filter" Scroll="None">

        </mi:FormLayout>
       
        <mi:Toolbar ID="Toolbar1" runat="server">
            
            <mi:ToolBarTitle ID="tableNameTB1" Text="表名" />

            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />

            <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm.toggle()" />

            <%--<mi:ToolBarButton Text="列定义" Align="Right" Command="StepEdit2" />--%>

        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true">
            <Columns>
            </Columns>
        </mi:Table>
    </mi:Panel>


    <mi:WindowFooter ID="WindowFooter1" runat="server">
        <mi:Button runat="server" ID="GoPreBtn" Width="80" Height="26" Command="GoSubmit" Text="确定" Dock="Center" />
        <mi:Button runat="server" ID="GoNextBtn" Width="80" Height="26" Text="取消" Dock="Center" OnClick="ownerWindow.close()"  />
    </mi:WindowFooter>
</mi:Viewport>

</form>
