<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintTemplateTest.ascx.cs" Inherits="App.InfoGrid2.View.PrintTemplate.PrintTemplateTest" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form method="post">
    

    <mi:Store runat="server" ID="store1" Model="BIZ_PRINT" IdField="BIZ_PRINT_ID" SortText="IS_LINE DESC"  >
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" DbType="Int32"  Logic="&gt;=" />
        </FilterParams>
    </mi:Store>
    <mi:Store runat="server" ID="store2" Model="BIZ_PRINT_TEMPLATE" IdField="BIZ_PRINT_TEMPLATE_ID">
        <FilterParams>
            <mi:QueryStringParam Name="PAGE_ID" QueryStringField="id" DbType="Int32" />
            <mi:Param Name="ROW_SID" DefaultValue="0" DbType="Int32"  Logic="&gt;=" />
        </FilterParams>
    </mi:Store>


  <mi:Viewport runat="server" ID="viewport">

    <mi:Panel runat="server" ID="centerPanel"  Region="West" Scroll="None" Width="400" >
        <mi:Toolbar runat="server" ID="toolbar1">
            <mi:ToolBarTitle Text="打印相关" />
            <mi:ToolBarButton Text="打印预览" OnClick="还没写" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="true" StoreID="store1" Dock="Full" ReadOnly="true" 
            AutoRowCheck="true" CheckedMode="Single" PagerVisible="false"  >
            <Columns>
                <mi:RowCheckColumn />
                <mi:SelectColumn DataField="IS_LINE" HeaderText="状态" EditorMode="None" Width="50" ItemAlign="Center">
                    <mi:ListItem Value="False" Text="断线" />
                    <mi:ListItem Value="True" Text="在线" />
                </mi:SelectColumn>
                <mi:BoundField DataField="PRINT_TEXT" HeaderText="打印机名" />
                <mi:BoundField DataField="PRINT_CODE" HeaderText="打印机代码" />
            </Columns>
        </mi:Table>
    </mi:Panel>
    
    <mi:Panel runat="server" ID="Panel1" Dock="Full" Region="Center" Scroll="None" >

        <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarTitle Text="模板相关" />
            <mi:ToolBarButton Text="模板管理" Command="showManage" ID="btn1" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store2.Refresh();"  />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table2" RowLines="true" ColumnLines="true" StoreID="store2" Dock="Full" ReadOnly="true" 
            AutoRowCheck="true" CheckedMode="Single" PagerVisible="false" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="TEMPLATE_NAME" HeaderText="模板名称" Width="240" />
<%--                <mi:BoundField DataField="TEMPLATE_URL" HeaderText="模板路径" />--%>
            </Columns>
        </mi:Table>
    </mi:Panel>
     <mi:WindowFooter runat="server" ID="footer1">
        <mi:Button runat="server" ID="OkBtn" Text="打印" Width="80" Height="26" Command="btnPrint"
            Dock="Center" />
        <mi:Button runat="server" ID="Button2" Text="取消" Width="80" Height="26" Dock="Right"
            OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
  
</mi:Viewport>



</form>

<script type="text/javascript">

      function ShowUrl(urlStr) {

        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '列表框架',
            iframe: true,
            startPosition: 'center_screen',
            url: urlStr
        });

        win.show();


    }

</script>



