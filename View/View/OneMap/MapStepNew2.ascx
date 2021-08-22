<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MapStepNew2.ascx.cs" Inherits="App.InfoGrid2.View.OneMap.MapStepNew2" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" id="form1" method="post">


<mi:Store runat="server" ID="store2" Model="IG2_MAP" IdField="IG2_MAP_ID"  >
    <SelectQuery>
        <mi:QueryStringParam Name="IG2_MAP_ID" QueryStringField="id" DbType="Int32"  />
    </SelectQuery>
</mi:Store>


<mi:Store runat="server" ID="store1" Model="IG2_MAP_COL" IdField="IG2_MAP_COL_ID" PageSize="0" >
    <SelectQuery>
        <mi:QueryStringParam Name="IG2_MAP_ID" QueryStringField="id" DbType="Int32"  />
    </SelectQuery>
</mi:Store>

<mi:Viewport runat="server" ID="viewport">
    
    <mi:Panel runat="server" ID="Panel1" Dock="Full" Region="North" Scroll="None" AutoSize="true"  >
        <mi:Table runat="server" ID="table2" RowLines="true" ColumnLines="true" StoreID="store2" 
            Dock="Full" CheckedMode="Single" 
            PagerVisible="false" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="L_TABLE" HeaderText="目标表"  EditorMode="None" />
                <mi:BoundField DataField="L_DISPLAY" HeaderText="目标表名" EditorMode="None" Width="300" />
                <mi:BoundField DataField="R_TABLE" HeaderText="数据源表"  EditorMode="None" />
                <mi:BoundField DataField="R_DISPLAY" HeaderText="数据源表名" EditorMode="None" Width="300" />
            </Columns>
        </mi:Table>
    </mi:Panel>

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:Toolbar runat="server" ID="toolbar1">
            <mi:ToolBarButton Text="重置目标列" Command="GoResetTargetFields" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="true" StoreID="store1" Dock="Full" PagerVisible="false" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="L_COL_TEXT" HeaderText="目标列描述" EditorMode="None" />
                <mi:BoundField DataField="L_COL" HeaderText="目标列名" EditorMode="None" />
                <mi:BoundField  DataField="LONIN" ItemAlign="Center"  EditorMode="None" Width="60" Resizable="false"  />
                <mi:SelectColumn  DataField="R_COL" HeaderText="数据列" TriggerMode="None" >
                    <MapItems>
                        <mi:MapItem SrcField="text" TargetField="R_COL_TEXT" />
                    </MapItems>
                </mi:SelectColumn>
                <mi:BoundField DataField="R_COL" HeaderText="数据列名" />
                 <mi:SelectColumn DataField="VALUE_MODE" HeaderText="值类型" TriggerMode="None">
                    <mi:ListItem Value="FIXED" Text="固定值" />
                    <mi:ListItem Value="FUN" Text="函数值" />
                    <mi:ListItem Value="TABLE" Text="数据表" />
                </mi:SelectColumn>
                <mi:BoundField DataField="R_FIXED" HeaderText="固定值"  />
                <mi:BoundField DataField="R_FUN" HeaderText="函数"  />
            </Columns>
        </mi:Table>
    </mi:Panel>
    <mi:WindowFooter runat="server" ID="footer1">
        <mi:Button runat="server" ID="OkBtn" Text="完成" Width="80" Height="26" Command="Complete"
            Dock="Center" />
        <mi:Button runat="server" ID="Button2" Text="取消" Width="80" Height="26" Dock="Right"
            OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>

</form>

