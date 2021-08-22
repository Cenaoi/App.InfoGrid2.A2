<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportSample.ascx.cs" Inherits="App.InfoGrid2.View.ReportBuilder.ReportSample" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<form action="" id="form1" method="post">
   
    <mi:Store runat="server" ID="store1" Model="IG2_MAP" IdField="IG2_MAP_ID">

    </mi:Store>

    <mi:Viewport runat="server" ID="viewport1">

        <mi:SearchFormLayout runat="server" ID="search1">

            <mi:SearchButtonGroup runat="server" ID="btnGroup1"></mi:SearchButtonGroup>
        </mi:SearchFormLayout>
        <mi:Toolbar runat="server" ID="toolbar1">
            <mi:ToolBarTitle Text="交叉报表" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" PagerVisible="false" Region="Center">
            <Columns>
                <mi:RowNumberer />
            </Columns>
        </mi:Table>

    </mi:Viewport>
</form>