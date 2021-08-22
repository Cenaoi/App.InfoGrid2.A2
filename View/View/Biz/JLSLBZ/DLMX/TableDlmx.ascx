<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TableDlmx.ascx.cs" Inherits="App.InfoGrid2.View.Biz.JLSLBZ.DLMX.TableDlmx" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<style type="text/css">
    .page-head
    {
         font-size:26px;
         font-weight:bold;
    }
</style>
<form action="" method="post">

<!--UT_102 吸塑指令单-订单明细-->
<mi:Store runat="server" ID="StoreUT102" Model="UT_102" IdField="ROW_IDENTITY_ID"  ReadOnly="true">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="2" Logic="&gt;=" />
    </FilterParams>

</mi:Store>

<!--	UT_103	吸塑指令单-主材料	-->
<mi:Store runat="server" ID="StoreUT103" Model="UT_103" IdField="ROW_IDENTITY_ID"  ReadOnly="true">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="2" Logic="&gt;=" />
    </FilterParams>

</mi:Store>
    
<mi:Viewport runat="server" ID="viewport1">
    <mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="10" Region="North">
        <mi:Label runat="server" ID="headLab" Value="<span class='page-head' >待领料明细查询</span>" HideLabel="true" Dock="Center" Mode="Transform" />
    </mi:Panel>

    <mi:SearchFormLayout ID="SearchFormLayout1" runat="server" StoreID="StoreUT103">
        
        <mi:TextBox ID="TextBox1" runat="server" FieldLabel="成品类型" />
        <mi:TextBox ID="TextBox2" runat="server" FieldLabel="材料名称" />
        <mi:TextBox ID="TextBox3" runat="server" FieldLabel="材料编码" />
        <mi:TextBox ID="TextBox4" runat="server" FieldLabel="生产指令单" />
        <mi:DateRangePicker ID="DateRangePicker1" runat="server" FieldLabel="生产指令单时间" />
        <mi:SearchButtonGroup ID="SearchButtonGroup1" runat="server">
        </mi:SearchButtonGroup>
    </mi:SearchFormLayout>

    <mi:Panel runat="server" ID="panelXX1" Dock="Full" Region="Center" Scroll="None" Height="200">
        <mi:Toolbar runat="server" ID="Toolbar1" >
            <mi:ToolBarTitle Text="主材料" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT103.Refresh();"  />
        </mi:Toolbar>

        <mi:Table runat="server" ID="Table103" Dock="Full" StoreID="StoreUT103" PagerVisible="false" SummaryVisible="true" Height="160">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:TriggerColumn HeaderText="材料编号" DataField="COL_1" OnButtonClick="showDialgoForTable_103(this)" ButtonClass="mi-icon-more" Tag='{"type_id":"VIEW","view_id":241,"table_name":"UT_083","owner_table_id":214}' />
                <mi:BoundField HeaderText="材料名称" DataField="COL_2" />
                <mi:BoundField HeaderText="材料规格" DataField="COL_3" />
                <mi:BoundField HeaderText="宽度" DataField="COL_4" />
                <mi:BoundField HeaderText="密度" DataField="COL_5" />
                <mi:BoundField HeaderText="厚度" DataField="COL_17" />
                <mi:BoundField HeaderText="板长" DataField="COL_6" />
                <mi:BoundField HeaderText="拉片张数-计划张数" DataField="COL_7"  SummaryType="SUM" />
                <mi:BoundField HeaderText="拉片张数-实需张数" DataField="COL_8"  SummaryType="SUM" />
                <mi:BoundField HeaderText="基本用料-计量单位" DataField="COL_9"   />
                <mi:BoundField HeaderText="基本用料-计划用料" DataField="COL_10" />
                <mi:BoundField HeaderText="基本用料-损耗" DataField="COL_11"   />
                <mi:BoundField HeaderText="基本用料-实需用料" DataField="COL_12"   />
                <mi:BoundField HeaderText="是否需要植绒" DataField="COL_13" />
                <mi:BoundField HeaderText="备注" DataField="COL_14" />
            </Columns>
        </mi:Table>
    </mi:Panel>

    <mi:Panel runat="server" ID="Panel1" Region="South" Scroll="None" Height="200" >
        <mi:Toolbar runat="server" ID="Toolbar2" >
            <mi:ToolBarTitle Text="产品组明细" />
        </mi:Toolbar>

        <mi:Table runat="server" ID="mainClientTable" Dock="Full" StoreID="StoreUT102"  PagerVisible="false" SummaryVisible="true">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />        
                <mi:BoundField HeaderText="客户名称" DataField="COL_1" EditorMode="None" />
                <mi:BoundField HeaderText="订单号" DataField="COL_2"  EditorMode="None"/>
                <mi:BoundField HeaderText="产品编号" DataField="COL_3" EditorMode="None" />
                <mi:BoundField HeaderText="产品名称" DataField="COL_4" EditorMode="None" />
                <mi:BoundField HeaderText="规格" DataField="COL_5" EditorMode="None" />
                <mi:BoundField HeaderText="计量单位" DataField="COL_6" EditorMode="None" />
                <mi:BoundField HeaderText="订单数量" DataField="COL_7"  EditorMode="None"  />
                <mi:BoundField HeaderText="排摸个数" DataField="COL_8" />
                <mi:BoundField HeaderText="生产数量" DataField="COL_9" SummaryType="SUM" />
                <mi:BoundField HeaderText="当前库存数量" DataField="COL_10" SummaryType="SUM" />
                <mi:DateColumn HeaderText="计划完工时间" DataField="COL_11" />
                <mi:BoundField HeaderText="用料说明" DataField="COL_12" />
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>



</form>