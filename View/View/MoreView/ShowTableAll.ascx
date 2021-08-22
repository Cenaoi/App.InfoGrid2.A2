<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShowTableAll.ascx.cs" Inherits="App.EC52Demo.View.ViewSetup.ShowTableAll" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" method="post">

<mi:Store runat="server" ID="store1" Model="IG2_TABLE" IdField="IG2_TABLE_ID"  >
    <FilterParams>
       <mi:Param Name="ROW_SID" DefaultValue="-1" Logic=">" DbType="Int32" />
       <mi:Param Name="TABLE_TYPE_ID" DefaultValue="TABLE" />
    </FilterParams>
</mi:Store>

<mi:Viewport runat="server" ID="viewport">
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="false" StoreID="store1" Dock="Full" ReadOnly="true" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="DISPLAY" HeaderText="表名" Width="300" />
                <mi:DateColumn DataField="ROW_DATE_CREATE" HeaderText="创建日期" Format="Y-m-d H:m" Width="140"  />
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