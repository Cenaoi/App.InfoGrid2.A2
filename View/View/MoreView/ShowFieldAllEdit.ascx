<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShowFieldAllEdit.ascx.cs"
    Inherits="App.EC52Demo.View.ViewSetup.ShowFieldAllEdit" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<form action="" method="post">
<mi:Store runat="server" ID="store1" Model="IG2_TMP_VIEW_TABLE" IdField="IG2_TMP_VIEW_TABLE_ID"
    PageSize="20">
    <SelectQuery>
        <mi:QueryStringParam Name="IG2_VIEW_ID" QueryStringField="id" DbType="Int32" />
        <mi:QueryStringParam Name="TMP_GUID" QueryStringField="uid" DbType="Guid" />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" DbType="Int32" />
    </SelectQuery>
</mi:Store>
<mi:Store runat="server" ID="store2" Model="IG2_TABLE_COL" IdField="IG2_TABLE_COL_ID" SortField="FIELD_SEQ"
    PageSize="200">
    <FilterParams>
        <mi:StoreCurrentParam Name="IG2_TABLE_ID" ControlID="store1" PropertyName="TABLE_ID" />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" DbType="Int32" />
        <%--<mi:Param Name="SEC_LEVEL" DefaultValue="6" Logic="<" />--%>
    </FilterParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1">

    <mi:Panel runat="server" ID="toolbarX1" Height="24" Dock="Top" Layout="Toolbar" Scroll="None" ItemMarginLeft="10" ItemMarginRight="10" PaddingRight="10">
        <mi:RadioGroup runat="server" ID="radioGroup1" HideLabel="true" Dock="Right" >
            <mi:ListItem Text="列表" />
            <mi:ListItem Text="分类" />
        </mi:RadioGroup>
    </mi:Panel>

    <mi:Table runat="server" ID="table1" StoreID="store1" ReadOnly="true" CheckedMode="Single"
        Sortable="false" Region="West" Dock="Left" Width="260" PagerVisible="false">
        <Columns>
            <mi:RowNumberer />
            <mi:BoundField DataField="TABLE_TEXT" HeaderText="表名" Width="180" />
        </Columns>
    </mi:Table>

    <mi:Panel runat="server" ID="centerPanel" Region="Center" Dock="Full" Scroll="None">
        <mi:Table runat="server" ID="table2" StoreID="store2" ReadOnly="true" Sortable="false"
            Region="Center" Dock="Full" PagerVisible="false">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="DISPLAY" HeaderText="列名" Width="300" />
                <mi:BoundField DataField="DB_FIELD" HeaderText="系统字段名" Width="200" />

            </Columns>
        </mi:Table>
    </mi:Panel>
    <mi:WindowFooter runat="server" ID="footer1">
        <mi:Button runat="server" ID="OkBtn" Text="确定选择" Width="80" Height="26" Command="SelectField"
            Dock="Center" />
        <mi:Button runat="server" ID="Button2" Text="取消" Width="80" Height="26" Dock="Right"
            OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>
</form>
