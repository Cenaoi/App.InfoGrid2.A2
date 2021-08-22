<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectMapAll.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Input_Data.SelectMapAll" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" method="post">

<mi:Store runat="server" ID="store1" Model="IG2_MAP" IdField="IG2_MAP_ID"  >
    <FilterParams>
        <mi:Param Name="ROW_SID" Logic="&gt;=" DefaultValue="0"  />
    </FilterParams>
</mi:Store>

<mi:Viewport runat="server" ID="viewport">
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="false" StoreID="store1" Dock="Full" ReadOnly="true" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="L_TABLE" HeaderText="左表" />
                <mi:BoundField DataField="L_DISPLAY" HeaderText="左表显示名" />
                <mi:BoundField DataField="R_TABLE" HeaderText="右表" />
                <mi:BoundField DataField="R_DISPLAY" HeaderText="右表显示名" />
                
            </Columns>
        </mi:Table>
    </mi:Panel>
    <mi:WindowFooter runat="server" ID="footer1">
        <mi:Button runat="server" ID="OkBtn" Text="确定选择" Width="80" Height="26" Command="SelectTable"
            Dock="Center" />
        <mi:Button runat="server" ID="Button2" Text="取消" Width="80" Height="26" Dock="Right"
            OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>

</form>



