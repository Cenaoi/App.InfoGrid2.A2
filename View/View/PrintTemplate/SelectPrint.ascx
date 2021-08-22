<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectPrint.ascx.cs" Inherits="App.InfoGrid2.View.PrintTemplate.SelectPrint" %>



<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form method="post">
    

    <mi:Store runat="server" ID="store1" Model="BIZ_PRINT" IdField="BIZ_PRINT_ID" SortText="IS_LINE DESC"  >
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" DbType="Int32"  Logic="&gt;=" />
        </FilterParams>
    </mi:Store>

    <mi:Store runat="server" ID="store2" Model="BIZ_PRINT_TEMPLATE" IdField="BIZ_PRINT_TEMPLATE_ID">
        <FilterParams>
            <mi:QueryStringParam Name="PAGE_ID" QueryStringField="pageid" DbType="Int32" />
            <mi:Param Name="ROW_SID" DefaultValue="0" DbType="Int32"  Logic="&gt;=" />
        </FilterParams>
    </mi:Store>

 
  <mi:Viewport runat="server" ID="viewport">

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="West" Scroll="None" Width="400" >

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
