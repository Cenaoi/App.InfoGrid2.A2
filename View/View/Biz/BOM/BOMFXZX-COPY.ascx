<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BOMFXZX-COPY.ascx.cs" Inherits="App.InfoGrid2.View.Biz.BOM.BOMFXZX_COPY" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form method="post" >

    <!--UT_101	吸塑指令单-单头-->
    <mi:Store runat="server" ID="StoreUT101" Model="UT_101" IdField="ROW_IDENTITY_ID" SortText="ROW_DATE_UPDATE desc">
        <FilterParams>
            
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:Param Name="BIZ_SID" DefaultValue="2"  />
            <mi:TSqlWhereParam Name="dd" Where="COL_1 is not null and COL_1 <> ''" />
            <mi:Param Name="COL_10" DefaultValue="是" DbType="String"/>
        </FilterParams>
    </mi:Store>
    <!--	UT_102	吸塑指令单-订单明细-->
    <mi:Store runat="server" ID="StoreUT102" Model="UT_102" IdField="ROW_IDENTITY_ID">
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_13" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
    </mi:Store>
    <!--	UT_196	生产管理BOM配方表-构件明细-->
    <mi:Store runat="server" ID="StoreUT196" Model="UT_196" IdField="ROW_IDENTITY_ID">
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_20" PropertyName="ROW_IDENTITY_ID" />
            <mi:StoreCurrentParam ControlID="StoreUT102" Name="UT_102_ID" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
    </mi:Store>
    <!--UT_196	生产管理BOM配方表-构件明细    领料需求-->
    <mi:Store runat="server" ID="StoreUT225" Model="UT_225" IdField="ROW_IDENTITY_ID">
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_20" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="COL_13" DefaultValue="自制" DbType="String" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
    </mi:Store>
    <!--UT_196	生产管理BOM配方表-构件明细    外发加工需求-->
    <mi:Store runat="server" ID="StoreUT224" Model="UT_224" IdField="ROW_IDENTITY_ID">
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_20" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="COL_13" DefaultValue="外发加工" DbType="String" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
    </mi:Store>
    <!--	UT_216	生产指令单-采购分析-->
    <mi:Store runat="server" ID="StoreUT216" Model="UT_216" IdField="ROW_IDENTITY_ID">
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_15" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
    </mi:Store>


    <mi:Viewport runat="server" ID="viewport1" Padding="4" ItemMarginRight="6">

        <mi:SearchFormLayout runat="server" ID="searchFormLayout1" StoreID="StoreUT102" Region="North" Visible="false" >

            <mi:TextBox runat="server" ID="textBox8" FieldLabel="生产单号" DataField="COL_1" DataLogic="like" />
            <mi:TextBox runat="server" ID="textBox9" FieldLabel="生产模式" DataField="COL_3" DataLogic="like" />
            <mi:TextBox runat="server" ID="textBox10" FieldLabel="是否生产" DataField="COL_10" DataLogic="like" />
            <mi:DateRangePicker runat="server" ID="data1" FieldLabel="时间" DataField="COL_4" />

            <mi:SearchButtonGroup runat="server" ID="btnGroups"></mi:SearchButtonGroup>

        </mi:SearchFormLayout>


         <!--UT_101	吸塑指令单-单头-->
        <mi:Panel runat="server" ID="LeftPanel" Region="West" Scroll="None" Width="200" >
            <mi:Toolbar runat="server" ID="Toolbar2" >
                <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchFormLayout1.toggle();$(window).resize();" />
            </mi:Toolbar>
            <mi:Table runat="server" ID="UT_101leftTable" Dock="Full" StoreID="StoreUT101"  PagerVisible="true" ReadOnly="true">
            <Columns>
                <mi:RowNumberer />
       
                <mi:BoundField HeaderText="分析状态" DataField="COL_13"  Width="150" />
                <mi:BoundField HeaderText="单据类型" DataField="COL_5"  Width="150" />
                <mi:BoundField HeaderText="指令单号" DataField="COL_1" Width="90" />
                <mi:BoundField HeaderText="制单时间" DataField="COL_4"  Width="150" />
                <mi:BoundField HeaderText="制单员" DataField="COL_2" Width="90" />
                <mi:BoundField HeaderText="是否生产" DataField="COL_10"  Width="150" />               

            </Columns>
            </mi:Table>
        </mi:Panel>

        
        <mi:Panel runat="server" ID="Panel1" Region="Center" Scroll="None"  >

        
            <!--UT_101	吸塑指令单-单头-->
            <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="400" ItemLabelAlign="Right" 
                Height="200" Dock="Top" Region="Center" SaveEnabled="true" 
                FlowDirection="LeftToRight" ItemClass="mi-box-item" Layout="HBox" >
                
                        

                <mi:TextBox runat="server" ID="COL_7TB" FieldLabel="状态" DataField="BIZ_SID" />
                        
                <mi:TextBox runat="server" ID="COL_8TB" FieldLabel="单据类型" DataField="COL_5" />
                <mi:TextBox runat="server" ID="COL_9TB" FieldLabel="指令单号" DataField="COL_1" />
                    <mi:TextBox runat="server" ID="COL_11TB" FieldLabel="制单时间" DataField="COL_4" />
                <mi:TextBox runat="server" ID="COL_12TB" FieldLabel="制单员" DataField="COL_2" />
                <mi:TextBox runat="server" ID="COL_17TB" FieldLabel="是否生产" DataField="COL_10" />
                <mi:TextBox runat="server" ID="COL_18TB" FieldLabel="分析状态" DataField="COL_13" />
                <mi:TextBox runat="server" ID="TextBox1" FieldLabel="分析时间" DataField="COL_14" />
                <mi:TextBox runat="server" ID="TextBox2" FieldLabel="分析值" DataField="COL_15" />
            </mi:FormLayout>

            <!--	UT_102	吸塑指令单-订单明细-->
            <mi:Panel runat="server" ID="UT_102Panel" Dock="Top" Scroll="None"  Height="200">
                <mi:Toolbar runat="server" ID="Toolbar1" >
                    <mi:ToolBarButton Text="获取BOM配方" Command="GetBomFormula" />
                    <mi:ToolBarButton Text="重新获取配方"  Command="AgainFormula" />
                </mi:Toolbar>
                <mi:Table runat="server" ID="UT_102Table" Dock="Full" StoreID="StoreUT102"  PagerVisible="true" ReadOnly="true">
                    <Columns>
                        <mi:RowNumberer />
                        <mi:NumColumn HeaderText="[单头ID]" DataField="COL_13" />
                        <mi:BoundField HeaderText="出货单号" DataField="COL_15" />
                        <mi:BoundField HeaderText="产品类型" DataField="COL_16" />
                        <mi:BoundField HeaderText="生产指令单号" DataField="COL_17" />
                        <mi:BoundField HeaderText="指令单类型" DataField="COL_18" />
                        <mi:BoundField HeaderText="单据类型" DataField="COL_19" />
                        <mi:BoundField HeaderText="生产模式" DataField="COL_20" />
                        <mi:BoundField HeaderText="时间" DataField="COL_21" />
                        <mi:BoundField HeaderText="客户编码" DataField="COL_22" />
                        <mi:BoundField HeaderText="客户名称" DataField="COL_1" />
                        <mi:BoundField HeaderText="订单号" DataField="COL_2" />
                        <mi:BoundField HeaderText="产品编号" DataField="COL_3" />
                        <mi:BoundField HeaderText="产品名称" DataField="COL_4" />
                        <mi:BoundField HeaderText="规格" DataField="COL_5" />
                        <mi:BoundField HeaderText="计量单位" DataField="COL_6" />
                        <mi:BoundField HeaderText="订单数量" DataField="COL_7" />
                        <mi:BoundField HeaderText="排摸个数" DataField="COL_8" />
                        <mi:BoundField HeaderText="生产数量" DataField="COL_9" />
                        <mi:BoundField HeaderText="当前库存数量" DataField="COL_10" />
                        <mi:BoundField HeaderText="计划完工时间" DataField="COL_11" />
                        <mi:BoundField HeaderText="用料说明" DataField="COL_12" />
                        <mi:BoundField HeaderText="仓库编码" DataField="COL_23" />
                        <mi:BoundField HeaderText="仓库名称" DataField="COL_24" />
                        <mi:BoundField HeaderText="备注" DataField="COL_25" />
                        <mi:BoundField HeaderText="送货地址" DataField="COL_26" />
                        <mi:BoundField HeaderText="客户方编号" DataField="COL_27" />
                        <mi:BoundField HeaderText="材料代码" DataField="COL_28" />
                        <mi:BoundField HeaderText="产品类型编号" DataField="COL_29" />
                        <mi:BoundField HeaderText="包装方式" DataField="COL_30" />
                        <mi:BoundField HeaderText="50kg生产数量" DataField="COL_31" />
                        <mi:BoundField HeaderText="单价" DataField="COL_32" />
                        <mi:BoundField HeaderText="【所属结构编号】" DataField="BIZ_CATA_CODE" />
                        <mi:BoundField HeaderText="【所属组织编号】" DataField="BIZ_ORG_CODE" />
                        <mi:BoundField HeaderText="【所属用户编号】" DataField="BIZ_USER_CODE" />
                        <mi:NumColumn HeaderText="业务状态" DataField="BIZ_SID" />
                        <mi:BoundField HeaderText="完成数量" DataField="COL_33" />
                        <mi:BoundField HeaderText="完成金额" DataField="COL_34" />
                        <mi:BoundField HeaderText="未完成数量" DataField="COL_35" />
                        <mi:BoundField HeaderText="未完成金额" DataField="COL_36" />
                        <mi:BoundField HeaderText="完成状态" DataField="COL_37" />
                        <mi:BoundField HeaderText="返工数量" DataField="COL_38" />
                        <mi:BoundField HeaderText="返工金额" DataField="COL_39" />
                        <mi:BoundField HeaderText="返工状态" DataField="COL_40" />
                        <mi:BoundField HeaderText="生产金额" DataField="COL_41" />
                        <mi:BoundField HeaderText="产品ID" DataField="COL_42" />
                        <mi:BoundField HeaderText="客户ID" DataField="COL_43" />
                        <mi:BoundField HeaderText="销售订单ID" DataField="COL_44" />
                        <mi:BoundField HeaderText="板长" DataField="COL_45" />
                        <mi:BoundField HeaderText="是否生产" DataField="COL_46" />
                        <mi:BoundField HeaderText="修正-材料名称" DataField="COL_47" />
                        <mi:BoundField HeaderText="修正-材料代码" DataField="COL_48" />
                        <mi:BoundField HeaderText="密度" DataField="COL_49" />
                        <mi:BoundField HeaderText="宽度" DataField="COL_50" />
                        <mi:BoundField HeaderText="长度" DataField="COL_51" />
                        <mi:BoundField HeaderText="厚度" DataField="COL_52" />
                        <mi:BoundField HeaderText="材料计量单位" DataField="COL_53" />
                        <mi:BoundField HeaderText="联系人" DataField="COL_54" />
                        <mi:BoundField HeaderText="电话" DataField="COL_55" />
                        <mi:BoundField HeaderText="混合排版" DataField="COL_56" />

                     </Columns>
                 </mi:Table>
            </mi:Panel>

            <!-- 下面的 tab 标签  -->
            <mi:TabPanel runat="server" ID="tabPanel1" Region="South" Dock="Full" Plain="True" Height="300" >

                <mi:Tab runat="server" ID="tabUT_196" Text="用量明细"  FlowDirection="TopDown" ItemClass="mi-box-item" 
                    Layout="HBox" Scroll="Vertical"  >
                
                    <!--	UT_196	生产管理BOM配方表-构件明细-->
                    <mi:Toolbar runat="server" ID="Toolbar3" >
                        <mi:ToolBarButton Text="计算生成数量" Command="NumFormula" />
                    </mi:Toolbar>
                    <mi:Table runat="server" ID="UT_196Table" Dock="Full" StoreID="StoreUT196" Height="500"  PagerVisible="true" ReadOnly="true">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:BoundField HeaderText="业务所属部门编码" DataField="BIZ_ORG_CODE" />
                            <mi:BoundField HeaderText="业务所属人编码" DataField="BIZ_USER_CODE" />
                            <mi:BoundField HeaderText="业务所结构编码" DataField="BIZ_CATA_CODE" />
                            <mi:NumColumn HeaderText="业务状态" DataField="BIZ_SID" />
                            <mi:BoundField HeaderText="主表关系" DataField="COL_19" />
                            <mi:NumColumn HeaderText="主表ID" DataField="COL_20" />
                            <mi:NumColumn HeaderText="生产单号ID" DataField="COL_21" />
                            <mi:BoundField HeaderText="生产单号" DataField="COL_22" />
                            <mi:NumColumn HeaderText="订单ID" DataField="COL_23" />
                            <mi:BoundField HeaderText="订单号" DataField="COL_24" />
                            <mi:NumColumn HeaderText="生产产品ID" DataField="COL_39" />
                            <mi:BoundField HeaderText="生产产品编号" DataField="COL_40" />
                            <mi:BoundField HeaderText="生产产品名称" DataField="COL_41" />
                            <mi:BoundField HeaderText="生产产品规格" DataField="COL_42" />
                            <mi:BoundField HeaderText="生产产品单位" DataField="COL_43" />
                            <mi:BoundField HeaderText="生产产品数量" DataField="COL_44" />
                            <mi:NumColumn HeaderText="成品ID" DataField="COL_1" />
                            <mi:BoundField HeaderText="成品编号" DataField="COL_2" />
                            <mi:BoundField HeaderText="成品名称" DataField="COL_3" />
                            <mi:BoundField HeaderText="成品规格" DataField="COL_4" />
                            <mi:BoundField HeaderText="成品计划生产数量" DataField="COL_25" />
                            <mi:BoundField HeaderText="成品库存数量" DataField="COL_26" />
                            <mi:BoundField HeaderText="成品采购在途数量" DataField="COL_27" />
                            <mi:BoundField HeaderText="成品采购在途可用数量" DataField="COL_45" />
                            <mi:BoundField HeaderText="成品外发加在途工数量" DataField="COL_28" />
                            <mi:BoundField HeaderText="成品待领数量" DataField="COL_29" />
                            <mi:BoundField HeaderText="成品车间在制数量" DataField="COL_46" />
                            <mi:BoundField HeaderText="成品可用数量" DataField="COL_30" />
                            <mi:BoundField HeaderText="成品实际生产数量" DataField="COL_31" />
                            <mi:NumColumn HeaderText="物料ID" DataField="COL_6" />
                            <mi:BoundField HeaderText="物料编号" DataField="COL_7" />
                            <mi:BoundField HeaderText="物料名称" DataField="COL_8" />
                            <mi:BoundField HeaderText="物料规格" DataField="COL_9" />
                            <mi:BoundField HeaderText="用料单位" DataField="COL_10" />
                            <mi:BoundField HeaderText="数量" DataField="COL_11" />
                            <mi:BoundField HeaderText="损耗率%" DataField="COL_12" />
                            <mi:BoundField HeaderText="成本价" DataField="COL_17" />
                            <mi:BoundField HeaderText="成本额" DataField="COL_18" />
                            <mi:BoundField HeaderText="单位" DataField="COL_5" />
                            <mi:BoundField HeaderText="物料计划生产数量" DataField="COL_32" />
                            <mi:BoundField HeaderText="物料库存数量" DataField="COL_33" />
                            <mi:BoundField HeaderText="物料采购在途数量" DataField="COL_34" />
                            <mi:BoundField HeaderText="物料采购在途可用数量" DataField="COL_47" />
                            <mi:BoundField HeaderText="物料外发在途数量" DataField="COL_35" />
                            <mi:BoundField HeaderText="物料待领数量" DataField="COL_36" />
                            <mi:BoundField HeaderText="物料车间在制数量" DataField="COL_48" />
                            <mi:BoundField HeaderText="物料可用数量" DataField="COL_37" />
                            <mi:BoundField HeaderText="物料实际需求量" DataField="COL_38" />
                            <mi:BoundField HeaderText="工时" DataField="COL_14" />
                            <mi:BoundField HeaderText="物料来源" DataField="COL_13" />
                            <mi:BoundField HeaderText="所属部门" DataField="COL_15" />
                            <mi:BoundField HeaderText="备注" DataField="COL_16" />
                            </Columns>
                    </mi:Table>

                </mi:Tab>

                <mi:Tab runat="server" ID="tab1" Text="领料需求"  FlowDirection="TopDown" ItemClass="mi-box-item" Layout="HBox" 
                    Scroll="Vertical" IsDelayRender="true">
                     <!--UT_196	生产管理BOM配方表-构件明细    领料需求-->
                    <mi:Table runat="server" ID="UT_196_ATable" Dock="Full" StoreID="StoreUT225"  
                        PagerVisible="true" ReadOnly="true"  IsDelayRender="true">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:BoundField HeaderText="业务所属部门编码" DataField="BIZ_ORG_CODE" />
                            <mi:BoundField HeaderText="业务所属人编码" DataField="BIZ_USER_CODE" />
                            <mi:BoundField HeaderText="业务所结构编码" DataField="BIZ_CATA_CODE" />
                            <mi:NumColumn HeaderText="业务状态" DataField="BIZ_SID" />
                            <mi:BoundField HeaderText="主表关系" DataField="COL_19" />
                            <mi:NumColumn HeaderText="主表ID" DataField="COL_20" />
                            <mi:NumColumn HeaderText="生产单号ID" DataField="COL_21" />
                            <mi:BoundField HeaderText="生产单号" DataField="COL_22" />
                            <mi:NumColumn HeaderText="订单ID" DataField="COL_23" />
                            <mi:BoundField HeaderText="订单号" DataField="COL_24" />
                            <mi:NumColumn HeaderText="生产产品ID" DataField="COL_39" />
                            <mi:BoundField HeaderText="生产产品编号" DataField="COL_40" />
                            <mi:BoundField HeaderText="生产产品名称" DataField="COL_41" />
                            <mi:BoundField HeaderText="生产产品规格" DataField="COL_42" />
                            <mi:BoundField HeaderText="生产产品单位" DataField="COL_43" />
                            <mi:BoundField HeaderText="生产产品数量" DataField="COL_44" />
                            <mi:NumColumn HeaderText="成品ID" DataField="COL_1" />
                            <mi:BoundField HeaderText="成品编号" DataField="COL_2" />
                            <mi:BoundField HeaderText="成品名称" DataField="COL_3" />
                            <mi:BoundField HeaderText="成品规格" DataField="COL_4" />
                            <mi:BoundField HeaderText="成品计划生产数量" DataField="COL_25" />
                            <mi:BoundField HeaderText="成品库存数量" DataField="COL_26" />
                            <mi:BoundField HeaderText="成品采购在途数量" DataField="COL_27" />
                            <mi:BoundField HeaderText="成品采购在途可用数量" DataField="COL_45" />
                            <mi:BoundField HeaderText="成品外发加在途工数量" DataField="COL_28" />
                            <mi:BoundField HeaderText="成品待领数量" DataField="COL_29" />
                            <mi:BoundField HeaderText="成品车间在制数量" DataField="COL_46" />
                            <mi:BoundField HeaderText="成品可用数量" DataField="COL_30" />
                            <mi:BoundField HeaderText="成品实际生产数量" DataField="COL_31" />
                            <mi:NumColumn HeaderText="物料ID" DataField="COL_6" />
                            <mi:BoundField HeaderText="物料编号" DataField="COL_7" />
                            <mi:BoundField HeaderText="物料名称" DataField="COL_8" />
                            <mi:BoundField HeaderText="物料规格" DataField="COL_9" />
                            <mi:BoundField HeaderText="用料单位" DataField="COL_10" />
                            <mi:BoundField HeaderText="数量" DataField="COL_11" />
                            <mi:BoundField HeaderText="损耗率%" DataField="COL_12" />
                            <mi:BoundField HeaderText="成本价" DataField="COL_17" />
                            <mi:BoundField HeaderText="成本额" DataField="COL_18" />
                            <mi:BoundField HeaderText="单位" DataField="COL_5" />
                            <mi:BoundField HeaderText="物料计划生产数量" DataField="COL_32" />
                            <mi:BoundField HeaderText="物料库存数量" DataField="COL_33" />
                            <mi:BoundField HeaderText="物料采购在途数量" DataField="COL_34" />
                            <mi:BoundField HeaderText="物料采购在途可用数量" DataField="COL_47" />
                            <mi:BoundField HeaderText="物料外发在途数量" DataField="COL_35" />
                            <mi:BoundField HeaderText="物料待领数量" DataField="COL_36" />
                            <mi:BoundField HeaderText="物料车间在制数量" DataField="COL_48" />
                            <mi:BoundField HeaderText="物料可用数量" DataField="COL_37" />
                            <mi:BoundField HeaderText="物料实际需求量" DataField="COL_38" />
                            <mi:BoundField HeaderText="工时" DataField="COL_14" />
                            <mi:BoundField HeaderText="物料来源" DataField="COL_13" />
                            <mi:BoundField HeaderText="所属部门" DataField="COL_15" />
                            <mi:BoundField HeaderText="备注" DataField="COL_16" />
                            </Columns>
                    </mi:Table>
                </mi:Tab>

                <mi:Tab runat="server" ID="tab2" Text="外发加工需求"  FlowDirection="TopDown" ItemClass="mi-box-item" Layout="HBox" Scroll="Vertical" >
                     <!--UT_196	生产管理BOM配方表-构件明细    外发加工需求-->

                    <mi:Table runat="server" ID="UT_196_BTable" Dock="Full" StoreID="StoreUT224"  PagerVisible="true" ReadOnly="true"  IsDelayRender="true">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:BoundField HeaderText="业务所属部门编码" DataField="BIZ_ORG_CODE" />
                            <mi:BoundField HeaderText="业务所属人编码" DataField="BIZ_USER_CODE" />
                            <mi:BoundField HeaderText="业务所结构编码" DataField="BIZ_CATA_CODE" />
                            <mi:NumColumn HeaderText="业务状态" DataField="BIZ_SID" />
                            <mi:BoundField HeaderText="主表关系" DataField="COL_19" />
                            <mi:NumColumn HeaderText="主表ID" DataField="COL_20" />
                            <mi:NumColumn HeaderText="生产单号ID" DataField="COL_21" />
                            <mi:BoundField HeaderText="生产单号" DataField="COL_22" />
                            <mi:NumColumn HeaderText="订单ID" DataField="COL_23" />
                            <mi:BoundField HeaderText="订单号" DataField="COL_24" />
                            <mi:NumColumn HeaderText="生产产品ID" DataField="COL_39" />
                            <mi:BoundField HeaderText="生产产品编号" DataField="COL_40" />
                            <mi:BoundField HeaderText="生产产品名称" DataField="COL_41" />
                            <mi:BoundField HeaderText="生产产品规格" DataField="COL_42" />
                            <mi:BoundField HeaderText="生产产品单位" DataField="COL_43" />
                            <mi:BoundField HeaderText="生产产品数量" DataField="COL_44" />
                            <mi:NumColumn HeaderText="成品ID" DataField="COL_1" />
                            <mi:BoundField HeaderText="成品编号" DataField="COL_2" />
                            <mi:BoundField HeaderText="成品名称" DataField="COL_3" />
                            <mi:BoundField HeaderText="成品规格" DataField="COL_4" />
                            <mi:BoundField HeaderText="成品计划生产数量" DataField="COL_25" />
                            <mi:BoundField HeaderText="成品库存数量" DataField="COL_26" />
                            <mi:BoundField HeaderText="成品采购在途数量" DataField="COL_27" />
                            <mi:BoundField HeaderText="成品采购在途可用数量" DataField="COL_45" />
                            <mi:BoundField HeaderText="成品外发加在途工数量" DataField="COL_28" />
                            <mi:BoundField HeaderText="成品待领数量" DataField="COL_29" />
                            <mi:BoundField HeaderText="成品车间在制数量" DataField="COL_46" />
                            <mi:BoundField HeaderText="成品可用数量" DataField="COL_30" />
                            <mi:BoundField HeaderText="成品实际生产数量" DataField="COL_31" />
                            <mi:NumColumn HeaderText="物料ID" DataField="COL_6" />
                            <mi:BoundField HeaderText="物料编号" DataField="COL_7" />
                            <mi:BoundField HeaderText="物料名称" DataField="COL_8" />
                            <mi:BoundField HeaderText="物料规格" DataField="COL_9" />
                            <mi:BoundField HeaderText="用料单位" DataField="COL_10" />
                            <mi:BoundField HeaderText="数量" DataField="COL_11" />
                            <mi:BoundField HeaderText="损耗率%" DataField="COL_12" />
                            <mi:BoundField HeaderText="成本价" DataField="COL_17" />
                            <mi:BoundField HeaderText="成本额" DataField="COL_18" />
                            <mi:BoundField HeaderText="单位" DataField="COL_5" />
                            <mi:BoundField HeaderText="物料计划生产数量" DataField="COL_32" />
                            <mi:BoundField HeaderText="物料库存数量" DataField="COL_33" />
                            <mi:BoundField HeaderText="物料采购在途数量" DataField="COL_34" />
                            <mi:BoundField HeaderText="物料采购在途可用数量" DataField="COL_47" />
                            <mi:BoundField HeaderText="物料外发在途数量" DataField="COL_35" />
                            <mi:BoundField HeaderText="物料待领数量" DataField="COL_36" />
                            <mi:BoundField HeaderText="物料车间在制数量" DataField="COL_48" />
                            <mi:BoundField HeaderText="物料可用数量" DataField="COL_37" />
                            <mi:BoundField HeaderText="物料实际需求量" DataField="COL_38" />
                            <mi:BoundField HeaderText="工时" DataField="COL_14" />
                            <mi:BoundField HeaderText="物料来源" DataField="COL_13" />
                            <mi:BoundField HeaderText="所属部门" DataField="COL_15" />
                            <mi:BoundField HeaderText="备注" DataField="COL_16" />
                            </Columns>
                    </mi:Table>

                </mi:Tab>


                <mi:Tab runat="server" ID="tab3" Text="采购需求"  FlowDirection="TopDown" ItemClass="mi-box-item" Layout="HBox" Scroll="Vertical" >
                        <!--	UT_216	生产指令单-采购分析-->

                    <mi:Table runat="server" ID="UT_216Table" Dock="Full" StoreID="StoreUT216"  PagerVisible="true" ReadOnly="true"  IsDelayRender="true">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:NumColumn HeaderText="[单头ID]" DataField="COL_15" />
                            <mi:BoundField HeaderText="生产指令单号" DataField="COL_16" />
                            <mi:BoundField HeaderText="材料编号" DataField="COL_1" />
                            <mi:BoundField HeaderText="材料名称" DataField="COL_2" />
                            <mi:BoundField HeaderText="是否需要植绒" DataField="COL_13" />
                            <mi:BoundField HeaderText="材料规格" DataField="COL_3" />
                            <mi:BoundField HeaderText="厚度" DataField="COL_17" />
                            <mi:BoundField HeaderText="密度" DataField="COL_5" />
                            <mi:BoundField HeaderText="长度" DataField="COL_19" />
                            <mi:BoundField HeaderText="宽度" DataField="COL_4" />
                            <mi:BoundField HeaderText="板长" DataField="COL_6" />
                            <mi:BoundField HeaderText="计量单位" DataField="COL_9" />
                            <mi:BoundField HeaderText="数量" DataField="COL_35" />
                            <mi:BoundField HeaderText="500KG生产个数" DataField="COL_18" />
                            <mi:BoundField HeaderText="基本用料-实需用料" DataField="COL_12" />
                            <mi:BoundField HeaderText="已采购数量" DataField="COL_41" />
                            <mi:BoundField HeaderText="未采购数量" DataField="COL_42" />
                            <mi:BoundField HeaderText="采购单状态" DataField="COL_30" />
                            <mi:BoundField HeaderText="默认仓库" DataField="COL_38" />
                            <mi:BoundField HeaderText="默认仓库编码" DataField="COL_37" />
                            <mi:BoundField HeaderText="备注" DataField="COL_14" />
                            <mi:BoundField HeaderText="变量单位" DataField="COL_39" />
                            <mi:BoundField HeaderText="变量数量" DataField="COL_40" />
                            <mi:NumColumn HeaderText="拉片张数-实需张数" DataField="COL_8" />
                            <mi:BoundField HeaderText="成品类型" DataField="COL_43" />
                            <mi:BoundField HeaderText="成品类型编码" DataField="COL_44" />
                            <mi:BoundField HeaderText="下采购单数量" DataField="COL_45" />
                            <mi:BoundField HeaderText="未下采购单数量" DataField="COL_46" />
                            <mi:BoundField HeaderText="是否采购" DataField="COL_47" />
                            <mi:BoundField HeaderText="库存数量" DataField="COL_48" />
                            <mi:BoundField HeaderText="可用数量" DataField="COL_49" />
                            <mi:BoundField HeaderText="采购数量" DataField="COL_50" />
                            <mi:BoundField HeaderText="建议采购量" DataField="COL_51" />
                            <mi:BoundField HeaderText="材料ID" DataField="COL_52" />
                            <mi:BoundField HeaderText="【所属结构编号】" DataField="BIZ_CATA_CODE" />
                            <mi:BoundField HeaderText="【所属组织编号】" DataField="BIZ_ORG_CODE" />
                            <mi:BoundField HeaderText="【所属用户编号】" DataField="BIZ_USER_CODE" />
                            <mi:NumColumn HeaderText="业务状态" DataField="BIZ_SID" />
                            <mi:BoundField HeaderText="单价" DataField="COL_53" />
                            <mi:BoundField HeaderText="金额" DataField="COL_54" />
                            <mi:BoundField HeaderText="材料类型编码" DataField="COL_55" />
                            <mi:BoundField HeaderText="默认仓库类型编码" DataField="COL_56" />
                            <mi:BoundField HeaderText="排摸个数" DataField="COL_64" />
                            <mi:NumColumn HeaderText="生产总个数" DataField="COL_65" />
                            <mi:BoundField HeaderText="分析状态" DataField="COL_66" />
                            <mi:NumColumn HeaderText="分析值" DataField="COL_67" />
                            <mi:BoundField HeaderText="分析时间" DataField="COL_68" />
                            <mi:NumColumn HeaderText="分析记录时间" DataField="COL_69" />
                            <mi:BoundField HeaderText="分析-采购在途" DataField="COL_70" />
                            <mi:BoundField HeaderText="分析-待领料" DataField="COL_71" />
                            <mi:BoundField HeaderText="预拉板数" DataField="COL_72" />
                            </Columns>
                    </mi:Table>

                </mi:Tab>

            </mi:TabPanel>

         </mi:Panel>

    </mi:Viewport>
</form>

