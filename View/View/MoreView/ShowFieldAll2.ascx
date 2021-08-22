<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShowFieldAll2.ascx.cs" Inherits="App.EC52Demo.View.ViewSetup.ShowFieldAll2" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" method="post">
<mi:Store runat="server" ID="store1" Model="IG2_TMP_VIEW_TABLE" IdField="IG2_TMP_VIEW_TABLE_ID" PageSize="20">
    <SelectQuery>
        <mi:QueryStringParam Name="TMP_GUID" QueryStringField="uid" DbType="Guid"  />
    </SelectQuery>
</mi:Store>
<mi:Store runat="server" ID="store2" Model="IG2_TABLE_COL" IdField="IG2_TABLE_COL_ID" PageSize="100">
    <FilterParams>
        <mi:StoreCurrentParam Name="IG2_TABLE_ID" ControlID="store1" PropertyName="TABLE_ID" />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" DbType="Int32" />
        <mi:Param Name="SEC_LEVEL" DefaultValue="6" Logic="<=" />
    </FilterParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Padding="4" ItemMarginRight="6">
    <mi:Table runat="server" ID="table1" StoreID="store1" ReadOnly="true" CheckedMode="Single" Sortable="false"
        AutoRowCheck="true" Region="West" Dock="Left" Width="260" PagerVisible="false">
        <Columns>
            <mi:RowNumberer />
            <mi:BoundField DataField="TABLE_TEXT" HeaderText="工作表" Width="200" />
        </Columns>
    </mi:Table>
    <mi:Table runat="server" ID="table2" StoreID="store2" ReadOnly="true" CheckedMode="Multi" Sortable="false"
        ColumnLines="false" Region="Center" Dock="Full" PagerVisible="false">
        <Columns>
            <mi:RowNumberer />
            <mi:RowCheckColumn />
            <mi:BoundField DataField="DISPLAY" HeaderText="列名" Width="300" />
            <mi:DateColumn DataField="ROW_DATE_CREATE" HeaderText="创建时间" />
        </Columns>
    </mi:Table>
    <mi:WindowFooter runat="server" ID="footer1">
        <mi:Button runat="server" ID="OkBtn" Text="确定选择" Width="80" Height="26" Command="SelectField"
            Dock="Center" />
        <mi:Button runat="server" ID="Button2" Text="取消" Width="80" Height="26" Dock="Right"
            OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>
</form>

