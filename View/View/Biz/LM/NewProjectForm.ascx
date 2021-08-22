<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewProjectForm.ascx.cs" Inherits="App.InfoGrid2.View.Biz.LM.NewProjectForm" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form method="post">

    <!--UT_101	吸塑指令单-单头-->
    <mi:Store runat="server" ID="StoreUT090" Model="UT_090" IdField="ROW_IDENTITY_ID" SortText="ROW_DATE_UPDATE desc" LockedRule="(me.get(\'BIZ_SID\') > 0)">
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:QueryStringParam Name="ROW_IDENTITY_ID" QueryStringField="id" />
        </FilterParams>
    </mi:Store>
    <!--	UT_102	吸塑指令单-订单明细-->
    <mi:Store runat="server" ID="StoreUT091_1" Model="UT_091" IdField="ROW_IDENTITY_ID" LockedRule="(me.get(\'BIZ_SID\') > 0)">
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT090" Name="COL_12" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
    </mi:Store>
    <!--	UT_196	生产管理BOM配方表-构件明细-->
    <mi:Store runat="server" ID="StoreUT091_2" Model="UT_091" IdField="ROW_IDENTITY_ID" LockedRule="(me.get(\'BIZ_SID\') > 0)">
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT090" Name="COL_12" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:TSqlWhereParam Where="COL_166 in ('901','101')" />
        </FilterParams>
        <InsertParams>
            <mi:Param Name="COL_166" DefaultValue="101" />
            <mi:Param Name="COL_167" DefaultValue="木制品" />
        </InsertParams>
    </mi:Store>
    <!--UT_196	生产管理BOM配方表-构件明细    领料需求-->
    <mi:Store runat="server" ID="StoreUT091_3" Model="UT_091" IdField="ROW_IDENTITY_ID" LockedRule="(me.get(\'BIZ_SID\') > 0)">
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT090" Name="COL_12" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:TSqlWhereParam Where="COL_166 in ('901','102')" />
        </FilterParams>
        <InsertParams>
            <mi:Param Name="COL_166" DefaultValue="102" />
            <mi:Param Name="COL_167" DefaultValue="软装水电" />
        </InsertParams>
    </mi:Store>
    <!--UT_196	生产管理BOM配方表-构件明细    外发加工需求-->
    <mi:Store runat="server" ID="StoreUT091_4" Model="UT_091" IdField="ROW_IDENTITY_ID" LockedRule="(me.get(\'BIZ_SID\') > 0)">
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT090" Name="COL_12" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:TSqlWhereParam Where="COL_166 in ('901','103')" />
        </FilterParams>
        <InsertParams>
            <mi:Param Name="COL_166" DefaultValue="103" />
            <mi:Param Name="COL_167" DefaultValue="杂项" />
        </InsertParams>
    </mi:Store>

    <mi:Store runat="server" ID="StoreUT091_5" Model="UT_091" IdField="ROW_IDENTITY_ID" LockedRule="(me.get(\'BIZ_SID\') > 0)">
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT090" Name="COL_12" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:TSqlWhereParam Where="COL_166 in ('901','104')" />
        </FilterParams>
        <InsertParams>
            <mi:Param Name="COL_166" DefaultValue="104" />
            <mi:Param Name="COL_167" DefaultValue="植配套餐" />
        </InsertParams>
    </mi:Store>

    <mi:Store runat="server" ID="StoreUT091_6" Model="UT_091" IdField="ROW_IDENTITY_ID" LockedRule="(me.get(\'BIZ_SID\') > 0)">
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT090" Name="COL_12" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:TSqlWhereParam Where="COL_166 in ('901','105')" />
        </FilterParams>
        <InsertParams>
            <mi:Param Name="COL_166" DefaultValue="105" />
            <mi:Param Name="COL_167" DefaultValue="服务费" />
        </InsertParams>
    </mi:Store>


    <mi:Viewport runat="server" ID="viewport1" Padding="4" ItemMarginRight="6">


        <mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="10">
                <mi:Label runat="server" ID="headLab" Value="工程项目正式订单" HideLabel="true" Dock="Center" Mode="Transform"  />
         </mi:Panel>


        <mi:SearchFormLayout runat="server" ID="searchFormLayout1" StoreID="StoreUT102" Region="North" Visible="false">

            <mi:TextBox runat="server" ID="textBox8" FieldLabel="生产单号" DataField="COL_1" DataLogic="like" />
            <mi:TextBox runat="server" ID="textBox9" FieldLabel="生产模式" DataField="COL_3" DataLogic="like" />
            <mi:TextBox runat="server" ID="textBox10" FieldLabel="是否生产" DataField="COL_10" DataLogic="like" />
            <mi:DateRangePicker runat="server" ID="data1" FieldLabel="时间" DataField="COL_4" />

            <mi:SearchButtonGroup runat="server" ID="btnGroups"></mi:SearchButtonGroup>

        </mi:SearchFormLayout>


        <mi:Panel runat="server" ID="Panel1" Region="Center" Scroll="None">

            <!--	UT_196	生产管理BOM配方表-构件明细-->
            <mi:Toolbar runat="server" ID="Toolbar0">
                <mi:ToolBarButton Text="查询" OnClick="widget1_I_searchFormLayout1.toggle();$(window).resize();" />
                <mi:ToolBarButton Text="提交" Command="GoChangeBizSid0_2" />
                <mi:ToolBarButton Text="审核" Command="GoChangeBizSid2_4" />
                <mi:ToolBarButton Text="撤销提交" Command="GoChangeBizSid2_0" />
                <mi:ToolBarButton Text="撤销审核" Command="GoChangeBizSid4_2" />
                <mi:ToolBarButton Text="打印" Command="GoPrint" />
            </mi:Toolbar>

            <!--UT_090	销售订单单头-单头-->
            <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="300" StoreID="StoreUT090" ItemLabelAlign="Right"
                Height="200" Dock="Top" Region="Center" SaveEnabled="true"
                FlowDirection="LeftToRight" ItemClass="mi-box-item" Layout="HBox">


                <mi:TextBox runat="server" ID="COL_142TB" FieldLabel="系统编号" DataField="COL_142" />
                <mi:TextBox runat="server" ID="COL_1TB" FieldLabel="订单号" DataField="COL_1" />
                <mi:TextBox runat="server" ID="COL_2TB" FieldLabel="时间" DataField="COL_2" />

                <mi:TextBox runat="server" ID="COL_70TB" FieldLabel="项目编号" DataField="COL_70" />
                <mi:TextBox runat="server" ID="COL_71TB" FieldLabel="项目名称" DataField="COL_71" />
                <mi:TextBox runat="server" ID="COL_3TB" FieldLabel="客户名称" DataField="COL_3" />
                <mi:TextBox runat="server" ID="COL_4TB" FieldLabel="联系人" DataField="COL_4" />
                <mi:TextBox runat="server" ID="COL_5TB" FieldLabel="电话" DataField="COL_5" />

                <mi:TextBox runat="server" ID="COL_84TB" FieldLabel="门店" DataField="COL_84" />
                <mi:TextBox runat="server" ID="COL_7TB" FieldLabel="地址" DataField="COL_7" />

                <mi:TextBox runat="server" ID="COL_13TB" FieldLabel="合计数量" DataField="COL_13" />
                <mi:TextBox runat="server" ID="COL_10TB" FieldLabel="备注" DataField="COL_10" />

                <mi:TextBox runat="server" ID="COL_72TB" FieldLabel="设计师" DataField="COL_72" />
                <mi:TextBox runat="server" ID="COL_73TB" FieldLabel="设计师电话" DataField="COL_73" />
                <mi:TextBox runat="server" ID="COL_74TB" FieldLabel="导购" DataField="COL_74" />


            </mi:FormLayout>


            <!-- 下面的 tab 标签  -->
            <mi:TabPanel runat="server" ID="tabPanel1" Region="South" Dock="Full" Height="300">

                <mi:Tab runat="server" ID="tabUT_196" Text="打印明细">

                    <!--	UT_196	生产管理BOM配方表-构件明细-->
                    <mi:Toolbar runat="server" ID="Toolbar3">
                                <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT091_1.Refresh();" />
                    </mi:Toolbar>
                    <mi:Table runat="server" ID="UT_196Table" Dock="Full" StoreID="StoreUT091_1" PagerVisible="true" ReadOnly="true">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:BoundField HeaderText="模块名称" DataField="COL_167" />
                            <mi:BoundField HeaderText="工程大类" DataField="COL_149" />
                            <mi:BoundField HeaderText="项目名称" DataField="COL_112" />
                            <mi:BoundField HeaderText="客户方名称" DataField="COL_25" />
                            <mi:BoundField HeaderText="规格" DataField="COL_4" />
                            <mi:BoundField HeaderText="长" DataField="COL_102" />
                            <mi:BoundField HeaderText="宽" DataField="COL_103" />
                            <mi:BoundField HeaderText="高" DataField="COL_104" />
                            <mi:BoundField HeaderText="计量单位" DataField="COL_5" />
                            <mi:BoundField HeaderText="数量" DataField="COL_7" />
                            <mi:BoundField HeaderText="参考价" DataField="COL_66" />
                            <mi:BoundField HeaderText="金额" DataField="COL_8" />
                            <mi:BoundField HeaderText="包装单位" DataField="COL_47" />
                            <mi:BoundField HeaderText="包装系数" DataField="COL_48" />
                            <mi:BoundField HeaderText="件数" DataField="COL_49" />
                            <mi:BoundField HeaderText="备注" DataField="COL_11" />
                        </Columns>
                    </mi:Table>

                </mi:Tab>

                <mi:Tab runat="server" ID="tab1" Text="木制品">

                    <!--	UT_196	生产管理BOM配方表-构件明细-->
                    <mi:Toolbar runat="server" ID="Toolbar1">
                        <mi:ToolBarButton Text="新增" OnClick="ser:StoreUT091_2.Insert();" />
                        <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT091_2.Refresh();" />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT091_2.Delete();" />
                    </mi:Toolbar>
                    <mi:Table runat="server" ID="Table1" Dock="Full" StoreID="StoreUT091_2" PagerVisible="true" ReadOnly="true">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:BoundField HeaderText="模块名称" DataField="COL_167" />
                            <mi:BoundField HeaderText="工程大类" DataField="COL_149" />
                            <mi:BoundField HeaderText="客户方名称" DataField="COL_25" />
                            <mi:BoundField HeaderText="规格" DataField="COL_4" />
                            <mi:BoundField HeaderText="长" DataField="COL_102" />
                            <mi:BoundField HeaderText="宽" DataField="COL_103" />
                            <mi:BoundField HeaderText="高" DataField="COL_104" />
                            <mi:BoundField HeaderText="计量单位" DataField="COL_5" />
                            <mi:BoundField HeaderText="数量" DataField="COL_7" />
                            <mi:BoundField HeaderText="参考价" DataField="COL_66" />
                            <mi:BoundField HeaderText="金额" DataField="COL_8" />
                            <mi:BoundField HeaderText="包装单位" DataField="COL_47" />
                            <mi:BoundField HeaderText="包装系数" DataField="COL_48" />
                            <mi:BoundField HeaderText="件数" DataField="COL_49" />
                            <mi:BoundField HeaderText="备注" DataField="COL_11" />
                        </Columns>
                    </mi:Table>

                </mi:Tab>

                <mi:Tab runat="server" ID="tab2" Text="软装水电">

                    <!--	UT_196	生产管理BOM配方表-构件明细-->
                    <mi:Toolbar runat="server" ID="Toolbar2">
                        <mi:ToolBarButton Text="新增" OnClick="ser:StoreUT091_3.Insert();" />
                        <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT091_3.Refresh();" />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT091_3.Delete();" />
                    </mi:Toolbar>
                    <mi:Table runat="server" ID="Table2" Dock="Full" StoreID="StoreUT091_3" PagerVisible="true" ReadOnly="true">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:BoundField HeaderText="模块名称" DataField="COL_167" />
                            <mi:BoundField HeaderText="工程大类" DataField="COL_149" />
                            <mi:BoundField HeaderText="客户方名称" DataField="COL_25" />
                            <mi:BoundField HeaderText="规格" DataField="COL_4" />
                            <mi:BoundField HeaderText="长" DataField="COL_102" />
                            <mi:BoundField HeaderText="宽" DataField="COL_103" />
                            <mi:BoundField HeaderText="高" DataField="COL_104" />
                            <mi:BoundField HeaderText="计量单位" DataField="COL_5" />
                            <mi:BoundField HeaderText="数量" DataField="COL_7" />
                            <mi:BoundField HeaderText="参考价" DataField="COL_66" />
                            <mi:BoundField HeaderText="金额" DataField="COL_8" />
                            <mi:BoundField HeaderText="包装单位" DataField="COL_47" />
                            <mi:BoundField HeaderText="包装系数" DataField="COL_48" />
                            <mi:BoundField HeaderText="件数" DataField="COL_49" />
                            <mi:BoundField HeaderText="备注" DataField="COL_11" />
                        </Columns>
                    </mi:Table>

                </mi:Tab>


                <mi:Tab runat="server" ID="tab3" Text="杂项">

                    <!--	UT_196	生产管理BOM配方表-构件明细-->
                    <mi:Toolbar runat="server" ID="Toolbar4">
                                                <mi:ToolBarButton Text="新增" OnClick="ser:StoreUT091_4.Insert();" />
                        <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT091_4.Refresh();" />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT091_4.Delete();" />
                    </mi:Toolbar>
                    <mi:Table runat="server" ID="Table3" Dock="Full" StoreID="StoreUT091_4" PagerVisible="true" ReadOnly="true">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:BoundField HeaderText="模块名称" DataField="COL_167" />
                            <mi:BoundField HeaderText="工程大类" DataField="COL_149" />
                            <mi:BoundField HeaderText="客户方名称" DataField="COL_25" />
                            <mi:BoundField HeaderText="规格" DataField="COL_4" />
                            <mi:BoundField HeaderText="长" DataField="COL_102" />
                            <mi:BoundField HeaderText="宽" DataField="COL_103" />
                            <mi:BoundField HeaderText="高" DataField="COL_104" />
                            <mi:BoundField HeaderText="计量单位" DataField="COL_5" />
                            <mi:BoundField HeaderText="数量" DataField="COL_7" />
                            <mi:BoundField HeaderText="参考价" DataField="COL_66" />
                            <mi:BoundField HeaderText="金额" DataField="COL_8" />
                            <mi:BoundField HeaderText="包装单位" DataField="COL_47" />
                            <mi:BoundField HeaderText="包装系数" DataField="COL_48" />
                            <mi:BoundField HeaderText="件数" DataField="COL_49" />
                            <mi:BoundField HeaderText="备注" DataField="COL_11" />
                        </Columns>
                    </mi:Table>

                </mi:Tab>


                <mi:Tab runat="server" ID="tab4" Text="值配套餐">

                    <!--	UT_196	生产管理BOM配方表-构件明细-->
                    <mi:Toolbar runat="server" ID="Toolbar5">
                        <mi:ToolBarButton Text="新增" OnClick="ser:StoreUT091_5.Insert();" />
                        <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT091_5.Refresh();" />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT091_5.Delete();" />
                    </mi:Toolbar>
                    <mi:Table runat="server" ID="Table4" Dock="Full" StoreID="StoreUT091_5" PagerVisible="true" ReadOnly="true">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:BoundField HeaderText="模块名称" DataField="COL_167" />
                            <mi:BoundField HeaderText="工程大类" DataField="COL_149" />
                            <mi:BoundField HeaderText="客户方名称" DataField="COL_25" />
                            <mi:BoundField HeaderText="规格" DataField="COL_4" />
                            <mi:BoundField HeaderText="长" DataField="COL_102" />
                            <mi:BoundField HeaderText="宽" DataField="COL_103" />
                            <mi:BoundField HeaderText="高" DataField="COL_104" />
                            <mi:BoundField HeaderText="计量单位" DataField="COL_5" />
                            <mi:BoundField HeaderText="数量" DataField="COL_7" />
                            <mi:BoundField HeaderText="参考价" DataField="COL_66" />
                            <mi:BoundField HeaderText="金额" DataField="COL_8" />
                            <mi:BoundField HeaderText="包装单位" DataField="COL_47" />
                            <mi:BoundField HeaderText="包装系数" DataField="COL_48" />
                            <mi:BoundField HeaderText="件数" DataField="COL_49" />
                            <mi:BoundField HeaderText="备注" DataField="COL_11" />
                        </Columns>
                    </mi:Table>

                </mi:Tab>

                <mi:Tab runat="server" ID="tab5" Text="服务费">

                    <!--	UT_196	生产管理BOM配方表-构件明细-->
                    <mi:Toolbar runat="server" ID="Toolbar6">
                                                <mi:ToolBarButton Text="新增" OnClick="ser:StoreUT091_6.Insert();" />
                        <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT091_6.Refresh();" />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT091_6.Delete();" />
                    </mi:Toolbar>
                    <mi:Table runat="server" ID="Table5" Dock="Full" StoreID="StoreUT091_6" PagerVisible="true" ReadOnly="true">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:BoundField HeaderText="模块名称" DataField="COL_167" />
                            <mi:BoundField HeaderText="工程大类" DataField="COL_149" />
                            <mi:BoundField HeaderText="客户方名称" DataField="COL_25" />
                            <mi:BoundField HeaderText="规格" DataField="COL_4" />
                            <mi:BoundField HeaderText="长" DataField="COL_102" />
                            <mi:BoundField HeaderText="宽" DataField="COL_103" />
                            <mi:BoundField HeaderText="高" DataField="COL_104" />
                            <mi:BoundField HeaderText="计量单位" DataField="COL_5" />
                            <mi:BoundField HeaderText="数量" DataField="COL_7" />
                            <mi:BoundField HeaderText="参考价" DataField="COL_66" />
                            <mi:BoundField HeaderText="金额" DataField="COL_8" />
                            <mi:BoundField HeaderText="包装单位" DataField="COL_47" />
                            <mi:BoundField HeaderText="包装系数" DataField="COL_48" />
                            <mi:BoundField HeaderText="件数" DataField="COL_49" />
                            <mi:BoundField HeaderText="备注" DataField="COL_11" />
                        </Columns>
                    </mi:Table>

                </mi:Tab>

            </mi:TabPanel>

        </mi:Panel>

    </mi:Viewport>
</form>

