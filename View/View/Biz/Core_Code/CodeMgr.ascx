<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CodeMgr.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Core_Code.CodeMgr" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<!--编码管理-->
<form action="" method="post" >
<mi:Store runat="server" ID="store1" Model="BIZ_CODE" IdField="BIZ_CODE_ID" PageSize="20" DeleteRecycle="true">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <InsertParams>
        <mi:Param Name="MODE_ID" DefaultValue="Day" />
        <mi:Param Name="NUM_ADD" DefaultValue="1" />
        <mi:Param Name="NUM_END" DefaultValue="99999999" />
    </InsertParams>
    <DeleteQuery>
        <mi:ControlParam Name="BIZ_CODE_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarButton Text="新增" OnClick="ser:store1.Insert()" />
            <mi:ToolBarButton Text="保存" OnClick="ser:store1.SaveAll()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="重新应用" Command="GoApplyReset" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm.toggle()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:store1.Delete()" />

            
            <mi:ToolBarButton Text="测试" Command="GoTest" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />

                <mi:BoundField HeaderText="组代码" DataField="T_CODE"  Tooltip=""/>
                <mi:BoundField HeaderText="组名称" DataField="GROUP_NAME" Tooltip="例：进货单，出货单。。" />
                <mi:SelectColumn HeaderText="模式" DataField="MODE_ID" TriggerMode="None">
                    <mi:ListItem Value="Auto" Text="自动" />
                    <mi:ListItem Value="Year" Text="年份" />
                    <mi:ListItem Value="Month" Text="月份" />
                    <mi:ListItem Value="Day" Text="日期" />
                </mi:SelectColumn>
                <mi:BoundField HeaderText="编码前缀" DataField="CODE_PRDFIX" />
                <mi:BoundField HeaderText="格式化显示" DataField="T_FORMAT" Width="200" />
                <mi:BoundField HeaderText="编码后缀" DataField="CODE_SUFFIX" />
                <mi:NumColumn HeaderText="起始值" DataField="NUM_START" />
                <mi:NumColumn HeaderText="终止值" DataField="NUM_END" />
                <mi:NumColumn HeaderText="当前值" DataField="NUM_CUR" />
                <mi:NumColumn HeaderText="增量" DataField="NUM_ADD" />
                <mi:BoundField HeaderText="标记" DataField="CATALOG_TEXT" />
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>

</form>

