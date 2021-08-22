<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormSample.ascx.cs" Inherits="App.InfoGrid2.View.OneForm.FormSample" %>
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

<style type="text/css">
    .page-head
    {
         font-size:26px;
         font-weight:bold;
    }
</style>

<form action="" method="post">

    <mi:Store runat="server" ID="store1" >
        
        <StringFields>value,text</StringFields>
        <CustomData>
            [{ value:1,text:"等待"},{value:2},{value:2},{value:2},{value:2},{value:2},{value:2},{value:2},{value:2},{value:2},{value:2},{value:2}]
        </CustomData>
    </mi:Store>


<mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="0" Padding="0">

    <mi:Toolbar runat="server" ID="mainToolbar1">
        <mi:ToolBarButton Text="保存" />
        <mi:ToolBarHr />
        <mi:ToolBarButton Text="提交" />
    </mi:Toolbar>
    <mi:Panel runat="server" ID="HeaderPanel" Height="60" Scroll="None" PaddingTop="10">
        <mi:Label runat="server" ID="headLab" Value="提运单" HideLabel="true" Dock="Center" Mode="Transform"  />
    </mi:Panel>
    <mi:FormLayout runat="server" ID="HanderFL1" ItemWidth="300" PaddingTop="10" ItemLabelAlign="Right" Region="North" Height="200" FlowDirection="LeftToRight" AutoSize="true">
        <mi:TextBox runat="server" ID="tb1" FieldLabel="提单号" />
        <mi:TextBox runat="server" ID="TextBox1" FieldLabel="提单号" />
        <mi:TextBox runat="server" ID="TextBox2" FieldLabel="货主" />
        <mi:TextBox runat="server" ID="TextBox4" FieldLabel="联系电话" />
        <mi:TextBox runat="server" ID="TextBox3" FieldLabel="地址" MinWidth="604px" />
    </mi:FormLayout>

    <mi:Panel runat="server" ID="panel1" Padding="8"  Region="Center" Scroll="None">
        <mi:TabPanel runat="server" ID="subTabPanel1" Height="400" Region="Center" Dock="Full" ButtonVisible="false" Plain="true" >
            <mi:Tab runat="server" ID="subTab1" Text="货物明细" Scroll="None">
                <div Dock="Top">呵呵</div>
                <mi:Table runat="server" ID="Table2" Dock="Full" StoreID="store1" PagerVisible="true">
                    <Columns>
                        <mi:RowNumberer />
                        <mi:RowCheckColumn />
                        <mi:BoundField HeaderText="商品名称" DataField="text" />
                        <mi:BoundField HeaderText="规格" DataField="value" />
                        <mi:BoundField HeaderText="重量(KG)" />
                        <mi:BoundField HeaderText="数量" />
                    </Columns>
                </mi:Table>
            </mi:Tab>
            <mi:Tab runat="server" ID="Tab1" Text="费用清单" Scroll="None">
                <mi:Toolbar runat="server" ID="toolbar2">
                    <mi:ToolBarButton Text="新建" />
                    <mi:ToolBarButton Text="删除" />
                    <mi:ToolBarHr />
                    <mi:ToolBarButton Text="导入" />
                </mi:Toolbar>
                <mi:Table runat="server" ID="Table1" Dock="Full" StoreID="store1" PagerVisible="false">
                    <Columns>
                        <mi:BoundField HeaderText="商品名称" DataField="text" />
                        <mi:BoundField HeaderText="规格" DataField="value" />
                        <mi:BoundField HeaderText="重量(KG)" />
                        <mi:BoundField HeaderText="数量" />
                    </Columns>
                </mi:Table>
            </mi:Tab>
        </mi:TabPanel>
    </mi:Panel>


    <mi:FormLayout runat="server" D="footerLF" ItemWidth="300" PaddingTop="8" PaddingBottom="8" ItemLabelAlign="Right" Region="South" FlowDirection="LeftToRight" AutoSize="true">
        <mi:TextBox runat="server" ID="TextBox5" FieldLabel="制单员" />
        <mi:DatePicker runat="server" ID="TextBox6" FieldLabel="制单日期" />
    </mi:FormLayout>

</mi:Viewport>
</form>