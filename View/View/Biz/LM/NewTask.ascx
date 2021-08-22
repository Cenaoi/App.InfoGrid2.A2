<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewTask.ascx.cs" Inherits="App.InfoGrid2.View.Biz.LM.NewTask" %>


<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<!--MESystem_Front-->
<form action="" method="post" >
<mi:Store runat="server" ID="store1" Model="UT_425_MESYSTEM_FRONT" IdField="ROW_IDENTITY_ID" PageSize="20">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="ROW_IDENTITY_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>
</mi:Store>

<mi:Store runat="server" ID="store2" Model="UT_428_MESYSTEM_DEPT" IdField="ROW_IDENTITY_ID" PageSize="20">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:Param Name="COL_16" DefaultValue="True" DbType="Boolean" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="ROW_IDENTITY_ID" ControlID="table2" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>
</mi:Store>

<mi:Viewport runat="server" ID="viewport1" Main="true">

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="North" Height="300" Scroll="None" >
        <mi:SearchFormLayout runat="server" ID="searchForm" StoreID="store1">

        </mi:SearchFormLayout>
        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarTitle ID="tableNameTB1" Text="任务" />

            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />


        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:NumColumn HeaderText="业务状态" DataField="BIZ_SID" />
                <mi:BoundField HeaderText="单据类型编号" DataField="COL_2" />
                <mi:BoundField HeaderText="单据类型" DataField="COL_3" />
                <mi:BoundField HeaderText="单据号" DataField="COL_4" />
                <mi:BoundField HeaderText="时间" DataField="COL_5" />
                <mi:BoundField HeaderText="制单员编号" DataField="COL_6" />
                <mi:BoundField HeaderText="制单员" DataField="COL_7" />
                <mi:BoundField HeaderText="部门编号" DataField="COL_8" />
                <mi:BoundField HeaderText="部门" DataField="COL_9" />
                <mi:NumColumn HeaderText="排程顺序" DataField="COL_19" />
                <mi:BoundField HeaderText="单体工作量" DataField="COL_20" />
                <mi:BoundField HeaderText="工作量基数" DataField="COL_21" />
                <mi:BoundField HeaderText="工作量" DataField="COL_22" />
                <mi:NumColumn HeaderText="主体ID" DataField="COL_10" />
                <mi:BoundField HeaderText="主体编号" DataField="COL_11" />
                <mi:BoundField HeaderText="主体名称" DataField="COL_12" />
                <mi:BoundField HeaderText="主体概述" DataField="COL_17" />
                <mi:NumColumn HeaderText="事件ID" DataField="COL_13" />
                <mi:BoundField HeaderText="事件编号" DataField="COL_14" />
                <mi:BoundField HeaderText="事件名称" DataField="COL_15" />
                <mi:BoundField HeaderText="事件概述" DataField="COL_18" />
                <mi:BoundField HeaderText="概述" DataField="COL_16" />
                <mi:BoundField HeaderText="排程A级归类编号" DataField="COL_23" />
                <mi:BoundField HeaderText="排程A级" DataField="COL_24" />
                <mi:BoundField HeaderText="排程B级归类编号" DataField="COL_25" />
                <mi:BoundField HeaderText="排程B级" DataField="COL_26" />
                <mi:BoundField HeaderText="排程C级归类编号" DataField="COL_27" />
                <mi:BoundField HeaderText="排程C级" DataField="COL_28" />
                <mi:BoundField HeaderText="排程状态编号" DataField="COL_29" />
                <mi:BoundField HeaderText="排程状态" DataField="COL_30" />
            </Columns>
        </mi:Table>
    </mi:Panel>

    <mi:Panel runat="server" ID="Panel1" Dock="Full" Region="Center" Scroll="None" >

        <mi:Toolbar ID="Toolbar2" runat="server">
            <mi:ToolBarTitle ID="tableNameTB2" Text="工作组" />

            <mi:ToolBarButton Text="刷新" OnClick="ser:store2.Refresh()" />

            <mi:ToolBarButton Text="生成排程任务" Command="GoCreateTask" />
            <mi:ToolBarButton Text="显示日历" Command="GoShowCalendar" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table2" StoreID="store2" Dock="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField HeaderText="业务所属部门编码" DataField="BIZ_ORG_CODE" />
                <mi:BoundField HeaderText="业务所属人编码" DataField="BIZ_USER_CODE" />
                <mi:BoundField HeaderText="业务所结构编码" DataField="BIZ_CATA_CODE" />
                <mi:BoundField HeaderText="业务更新人编码" DataField="BIZ_UPDATE_USER_CODE" />
                <mi:BoundField HeaderText="业务删除人编码" DataField="BIZ_DELETE_USER_CODE" />
                <mi:NumColumn HeaderText="业务状态" DataField="BIZ_SID" />
                <mi:BoundField HeaderText="分类名称" DataField="COL_1" />
                <mi:BoundField HeaderText="分类" DataField="COL_2" />
                <mi:BoundField HeaderText="编号" DataField="COL_3" />
                <mi:BoundField HeaderText="名称" DataField="COL_4" />
                <mi:BoundField HeaderText="责任人" DataField="COL_21" />
                <mi:BoundField HeaderText="产能" DataField="COL_14" />
                <mi:BoundField HeaderText="人数" DataField="COL_15" />
                <mi:NumColumn HeaderText="排程顺序" DataField="COL_17" />
                <mi:CheckColumn HeaderText="参与排程" DataField="COL_16" />
                <mi:BoundField HeaderText="排程A级归类编号" DataField="COL_6" />
                <mi:BoundField HeaderText="排程A级" DataField="COL_7" />
                <mi:BoundField HeaderText="排程B级归类编号" DataField="COL_8" />
                <mi:BoundField HeaderText="排程B级" DataField="COL_9" />
                <mi:BoundField HeaderText="排程C级归类编号" DataField="COL_10" />
                <mi:BoundField HeaderText="排程C级" DataField="COL_11" />
                <mi:BoundField HeaderText="工作负荷类型" DataField="COL_12" />
                <mi:BoundField HeaderText="工作负荷类型编号" DataField="COL_13" />
                <mi:BoundField HeaderText="备注" DataField="COL_5" />
                <mi:BoundField HeaderText="最近排程操作时间" DataField="COL_18" />
                <mi:BoundField HeaderText="工作代码" DataField="COL_23" />
                <mi:BoundField HeaderText="排程时间代码" DataField="COL_24" />
                <mi:NumColumn HeaderText="排程记录位置ID" DataField="COL_19" />
                <mi:NumColumn HeaderText="排程记录位置分数" DataField="COL_29" />
                <mi:BoundField HeaderText="排程方式代码" DataField="COL_30" />
                <mi:BoundField HeaderText="最新排程时间节点" DataField="COL_22" />
                <mi:NumColumn HeaderText="数据源ID" DataField="COL_20" />
                <mi:BoundField HeaderText="排程数据源ID代码" DataField="COL_25" />
                <mi:BoundField HeaderText="占用情况" DataField="COL_26" />
                <mi:BoundField HeaderText="占用情况代码" DataField="COL_27" />
                <mi:BoundField HeaderText="排程计算说明" DataField="COL_28" />
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>

</form>





