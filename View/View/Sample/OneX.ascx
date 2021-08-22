<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OneX.ascx.cs" Inherits="App.InfoGrid2.View.Sample.OneX" %>
<%@ Import Namespace="EasyClick.Web.Mini" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" id="form1" method="post">

    <mi:Store runat="server" ID="store1" Model="IG2_MAP" IdField="IG2_MAP_ID" DeleteRecycle="true">
        <FilterParams>
            <mi:TSqlWhereParam Where="ROW_SID >= 0  " />

        </FilterParams>
        <DeleteQuery>
            <mi:ControlParam Name="IG2_MAP_ID" ControlID="table1" PropertyName="CheckedRows" />
        </DeleteQuery>
        <DeleteRecycleParams>
            <mi:Param Name="ROW_SID" DefaultValue="-3" />
            <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
        </DeleteRecycleParams>
    </mi:Store>

    <mi:Viewport runat="server" ID="viewport">

        <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None">
            <mi:Toolbar runat="server" ID="toolbar2">
                <mi:ToolBarTitle Text="数据表映射规则" />
                <mi:ToolBarButton Text="新增" Command="NewRow" />
                <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="删除" OnClick="ser:store1.Delete()" BeforeAskText="确定删除映射表？" />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="应用全部规则" Command="GoApplyRuleAll" />
            </mi:Toolbar>
            <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="true" StoreID="store1" Dock="Full" ReadOnly="true">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField DataField="IG2_MAP_ID" HeaderText="ID" Width="80" EditorMode="None" />
                    <mi:GroupColumn HeaderText="目标" HeaderAlign="Center">
                        <Columns>
                            <mi:BoundField DataField="L_TABLE" HeaderText="内部表名" />
                            <mi:BoundField DataField="L_DISPLAY" HeaderText="表名" Width="300" />
                        </Columns>
                    </mi:GroupColumn>
                    <mi:GroupColumn HeaderText="数据源" HeaderAlign="Center">
                        <Columns>
                            <mi:BoundField DataField="R_TABLE" HeaderText="内部表名" /> 
                            <mi:BoundField DataField="R_DISPLAY" HeaderText="表名" Width="300" />
                        </Columns>
                    </mi:GroupColumn>
                    <mi:ActionColumn>
                        <mi:ActionItem Handler="TableEdit" Tooltip="修改" Icon="/res/icon/page_white_edit.png" />
                    </mi:ActionColumn>

                </Columns>
            </mi:Table>
        </mi:Panel>
    </mi:Viewport>

</form>
<script>

    function TableEdit(view, cell, recordIndex, cellIndex, e, record, row) {

        var tableId = record.getId();

        var typeId = record.get('TABLE_TYPE_ID');

        //MiniHelper.Redirect("/App/EC52Demo/View/CopyData/CopyDataNew2.aspx?id=" + tableId);
        EcView.show("/App/InfoGrid2/View/OneMap/MapStepNew2.aspx?id=" + tableId, "修改数据表映射", 800, 600);

    }

</script>
