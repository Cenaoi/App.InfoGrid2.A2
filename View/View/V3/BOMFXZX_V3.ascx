<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BOMFXZX_V3.ascx.cs" Inherits="App.InfoGrid2.View.V3.BOMFXZX_V3" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form method="post" >

    <!--UT_101	吸塑指令单-单头-->
    <mi:Store runat="server" ID="StoreUT101" Model="UT_280" IdField="ROW_IDENTITY_ID" SortText="ROW_DATE_UPDATE desc">
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:Param Name="BIZ_SID" DefaultValue="2"  />
            <mi:TSqlWhereParam Name="dd" Where="COL_1 is not null and COL_1 <> ''" />
            <mi:Param Name="COL_10" DefaultValue="是" DbType="String"/>
        </FilterParams>
    </mi:Store>
    <!--	UT_102	吸塑指令单-订单明细-->
    <mi:Store runat="server" ID="StoreUT102" Model="UT_281" IdField="ROW_IDENTITY_ID">
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

    <!--	UT_196	生产管理BOM配方表-构件明细 这是根据101显示的-->
    <mi:Store runat="server" ID="StoreUT196B" Model="UT_196" IdField="ROW_IDENTITY_ID">
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_20" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
    </mi:Store>

    <!--UT_196	生产管理BOM配方表-构件明细    领料需求-->
    <mi:Store runat="server" ID="StoreUT238" Model="UT_238" IdField="ROW_IDENTITY_ID">
        <InsertParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_74" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="COL_89" DefaultValue="自制" />
        </InsertParams>
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_74" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
    </mi:Store>
    <!--UT_196	生产管理BOM配方表-构件明细    外发加工需求-->
    <mi:Store runat="server" ID="StoreUT239" Model="UT_239" IdField="ROW_IDENTITY_ID">
        <InsertParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_74" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="COL_89" DefaultValue="外发加工" />
        </InsertParams>
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_74" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
    </mi:Store>
    <!--	UT_216	生产指令单-采购分析-->
    <mi:Store runat="server" ID="StoreUT216" Model="UT_216" IdField="ROW_IDENTITY_ID">
        <InsertParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_79" PropertyName="ROW_IDENTITY_ID" />
        </InsertParams>
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_79" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
    </mi:Store>


    <!--	UT_226	吸塑指令单-生产明细分析 -->
    <mi:Store runat="server" ID="StoreUT196A" Model="UT_196" IdField="ROW_IDENTITY_ID">
        <InsertParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_20" PropertyName="ROW_IDENTITY_ID" />
        </InsertParams>
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_20" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:Param Name="COL_13" DefaultValue="自制" />
        </FilterParams>
    </mi:Store>

      <!--		UT_227	吸塑指令单-委外发料需求  -->
    <mi:Store runat="server" ID="StoreUT237" Model="UT_237" IdField="ROW_IDENTITY_ID">
        <InsertParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_74" PropertyName="ROW_IDENTITY_ID" />
        </InsertParams>
        <FilterParams>
            <mi:StoreCurrentParam ControlID="StoreUT101" Name="COL_74" PropertyName="ROW_IDENTITY_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
    </mi:Store>



    <mi:Viewport runat="server" ID="viewport1" Padding="4" ItemMarginRight="6">

        <mi:SearchFormLayout runat="server" ID="searchFormLayout1" StoreID="StoreUT101" Region="North" Visible="false" >

            <mi:TextBox runat="server" ID="textBox8" FieldLabel="生产单号" DataField="COL_1" DataLogic="like" />
            <mi:TextBox runat="server" ID="textBox9" FieldLabel="产品类型" DataField="COL_5" DataLogic="like" />
            <mi:DateRangePicker runat="server" ID="data1" FieldLabel="时间" DataField="COL_4" />
            <mi:ComboBox runat="server" ID="ComboBox1" FieldLabel="分析状态" DataField="COL_13">
                    <mi:ListItem Text="分析完毕" Value="分析完毕" />
                    <mi:ListItem Text="待分析" Value="待分析" />
            </mi:ComboBox>

            <mi:SearchButtonGroup runat="server" ID="btnGroups"></mi:SearchButtonGroup>

        </mi:SearchFormLayout>


         <!--UT_101	吸塑指令单-单头-->
        <mi:Panel runat="server" ID="LeftPanel" Region="West" Scroll="None" Width="320" >
            <mi:Toolbar runat="server" ID="Toolbar2" >
                <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchFormLayout1.toggle();$(window).resize();" />
                <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT101.Refresh();" />
            </mi:Toolbar>
            <mi:Table runat="server" ID="UT_101leftTable" Dock="Full" StoreID="StoreUT101"  PagerVisible="true" ReadOnly="true">
            <Columns>
                <mi:RowNumberer />
       
                <mi:BoundField HeaderText="状态" DataField="COL_13"  Width="50" />
                <mi:BoundField HeaderText="类型" DataField="COL_5"  Width="50" />
                <mi:BoundField HeaderText="指令单号" DataField="COL_1" Width="80" />
                <mi:BoundField HeaderText="制单时间" DataField="COL_4"  Width="95" />
                <mi:BoundField HeaderText="制单员" DataField="COL_2" Width="60" />
                                

            </Columns>
            </mi:Table>
        </mi:Panel>

        
        <mi:Panel runat="server" ID="Panel1" Region="Center" Scroll="None" >

        
            <!--UT_101	吸塑指令单-单头-->
            <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="400" ItemLabelAlign="Right" 
                Height="200" Dock="Top" Region="Center" SaveEnabled="true" 
                FlowDirection="LeftToRight" ItemClass="mi-box-item" Layout="HBox" >
                
                        
                <mi:TextBox runat="server" ID="COL_8TB" FieldLabel="单据类型" DataField="COL_5"  />
                <mi:TextBox runat="server" ID="COL_9TB" FieldLabel="指令单号" DataField="COL_1" />
                <mi:TextBox runat="server" ID="COL_11TB" FieldLabel="制单时间" DataField="COL_4" />


                <mi:TextBox runat="server" ID="COL_18TB" FieldLabel="分析状态" DataField="COL_13" />
                <mi:TextBox runat="server" ID="TextBox1" FieldLabel="分析时间" DataField="COL_14"   />
                <mi:TextBox runat="server" ID="COL_12TB" FieldLabel="制单员" DataField="COL_2" />
                <mi:ComboBox runat="server" ID="BIZ_SIDTB" FieldLabel="分析状态" DataField="COL_13">
                    <mi:ListItem Text="已分析" Value="已分析" />
                    <mi:ListItem Text="待分析" Value="待分析" />
                </mi:ComboBox>
            </mi:FormLayout>

            <!--	UT_102	吸塑指令单-订单明细-->
            <mi:Panel runat="server" ID="UT_102Panel" Dock="Top" Scroll="None" Height="200">
                <mi:Toolbar runat="server" ID="Toolbar1" >
                    <mi:ToolBarButton Text="获取BOM配方" Command="GetBomFormula" />
                    <mi:ToolBarButton Text="订单BOM" Command="GetUT219To196" />
                    <mi:ToolBarButton Text="生产BOM" Command="GetUT103Bom" />
                    <mi:ToolBarButton Text="清空单个"  Command="ClearBOM" />
                    <mi:ToolBarButton Text="清空全部"  Command="ClearBOMAll" />
                    <mi:ToolBarButton Text="确认分析结果"  Command="BizSid0_2" />
                    <mi:ToolBarButton Text="撤销分析结果"  Command="BizSid2_0" />
                </mi:Toolbar>
                <mi:Table runat="server" ID="UT_102Table" Dock="Full" StoreID="StoreUT102"  PagerVisible="true" ReadOnly="true">
                    <Columns>
                        <mi:RowNumberer />
                        <mi:RowCheckColumn />
                        <mi:NumColumn HeaderText="[单头ID]" DataField="COL_13"  Width="70" />
                        <mi:BoundField HeaderText="类型" DataField="COL_18"  Width="50" />

                        <mi:BoundField HeaderText="生产模式" DataField="COL_20"  Width="70" />
                        <mi:BoundField HeaderText="时间" DataField="COL_21"  Width="100"  Visible="false"   />
                        <mi:BoundField HeaderText="客户编码" DataField="COL_22"  Width="70" />
                        <mi:BoundField HeaderText="客户名称" DataField="COL_1"  Width="100" />
                        <mi:BoundField HeaderText="订单号" DataField="COL_2"  Width="90" />
                        <mi:BoundField HeaderText="产品编号" DataField="COL_3"  Width="70" />
                        <mi:BoundField HeaderText="产品名称" DataField="COL_4"  Width="180" />
                        <mi:BoundField HeaderText="规格" DataField="COL_5"  Width="70" />
                       
                        <mi:BoundField HeaderText="计量单位" DataField="COL_6"   Width="70" />

                        <mi:NumColumn HeaderText="订单数量" DataField="COL_7" Format="0.##"  Width="70" />
                        <mi:NumColumn HeaderText="排摸个数" DataField="COL_8" Format="0.##"  Width="70" />
                        <mi:NumColumn HeaderText="生产数量" DataField="COL_9" Format="0.##"  Width="70" />
                        <mi:BoundField HeaderText="计划完工时间" DataField="COL_11"  Width="100" />
                        <mi:BoundField HeaderText="客户方编号" DataField="COL_27"  Width="90" />
                        <mi:BoundField HeaderText="混合排版" DataField="COL_56"  Width="70" />

                     </Columns>
                 </mi:Table>
            </mi:Panel>

            <!-- 下面的 tab 标签  -->
            <mi:TabPanel runat="server" ID="tabPanel1" Region="South" Dock="Full" Plain="True" Height="300" >

                <mi:Tab runat="server" ID="tab4" Text="物料汇总"  FlowDirection="TopDown" ItemClass="mi-box-item" 
                    Layout="HBox" Scroll="Vertical"  >
               
                    <mi:Table runat="server" ID="Table1" Dock="Full" StoreID="StoreUT196B" Height="500"  PagerVisible="true" ReadOnly="true">
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


                <mi:Tab runat="server" ID="tabUT_196" Text="用量明细"  FlowDirection="TopDown" ItemClass="mi-box-item" 
                    Layout="HBox" Scroll="Vertical"  >
                
                    <!--	UT_196	生产管理BOM配方表-构件明细-->
                    <mi:Toolbar runat="server" ID="Toolbar3" >
                        <mi:ToolBarButton Text="物料分析" Command="NumFormula" />
                         <mi:ToolBarButton Text="物控分析" Command="GetUT196To238" />
                        
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

                <mi:Tab runat="server" ID="tab1" Text="TD领料需求"  FlowDirection="TopDown" ItemClass="mi-box-item" Layout="HBox" 
                    Scroll="Vertical" IsDelayRender="true">
                    <mi:Toolbar runat="server" ID="Toolbar5" IsDelayRender="true" >
                        <mi:ToolBarButton Text="新建" OnClick="ser:StoreUT238.Insert();"  />
                        <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT238.Refresh();"  />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT238.Delete();" />
                     </mi:Toolbar>
                     <!--UT_196	生产管理BOM配方表-构件明细   T领料需求-->
                    <mi:Table runat="server" ID="UT_196_ATable" Dock="Full" StoreID="StoreUT238"  
                        PagerVisible="true"   IsDelayRender="true">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:RowCheckColumn />
                            <mi:NumColumn HeaderText="[单头ID]" DataField="COL_15" Width="70"  />
                            <mi:BoundField HeaderText="生产指令单号" DataField="COL_16"  Width="90" />
                            <mi:TriggerColumn HeaderText="材料编号" DataField="COL_1" Width="100" ButtonClass="mi-icon-more" 
                                OnButtonClick="showDialgoForTable_102(this,'VIEW',740)" 
                                OnMaping="mapping_103(this,'VIEW',740,'COL_2')"  >
                                <MapItems>
                                    <mi:MapItem SrcField="ROW_IDENTITY_ID" TargetField="COL_52" Remark="材料ID" />
                                    <mi:MapItem SrcField="COL_12" TargetField="COL_55" Remark="材料类型编码" />
                                    <mi:MapItem SrcField="COL_2" TargetField="COL_1" Remark="编号" />
                                    <mi:MapItem SrcField="COL_3" TargetField="COL_2" Remark="材料名称" />
                                    <mi:MapItem SrcField="COL_4" TargetField="COL_3" Remark="材料规格" />
                                    <mi:MapItem SrcField="COL_5" TargetField="COL_9" Remark="计量单位" />
                                    <mi:MapItem SrcField="COL_7" TargetField="COL_17" Remark="厚度" />
                                    <mi:MapItem SrcField="COL_8" TargetField="COL_5" Remark="密度" />
                                    <mi:MapItem SrcField="COL_6" TargetField="COL_4" Remark="宽度" />
                                    <mi:MapItem SrcField="COL_13" TargetField="COL_19" Remark="长度" />
                                    <mi:MapItem SrcField="COL_11" TargetField="COL_37" Remark="默认仓库编号" />
                                    <mi:MapItem SrcField="COL_9" TargetField="COL_38" Remark="仓库名称" />
                                </MapItems>
                            </mi:TriggerColumn>
                           <mi:TriggerColumn HeaderText="材料名称" DataField="COL_2" Width="160" ButtonClass="mi-icon-more" 
                                OnButtonClick="showDialgoForTable_102(this,'VIEW',740)" 
                                OnMaping="mapping_103(this,'VIEW',740,'COL_3')"  >
                                <MapItems>
                                    <mi:MapItem SrcField="ROW_IDENTITY_ID" TargetField="COL_52" Remark="材料ID" />
                                    <mi:MapItem SrcField="COL_12" TargetField="COL_55" Remark="材料类型编码" />
                                    <mi:MapItem SrcField="COL_2" TargetField="COL_1" Remark="编号" />
                                    <mi:MapItem SrcField="COL_3" TargetField="COL_2" Remark="材料名称" />
                                    <mi:MapItem SrcField="COL_4" TargetField="COL_3" Remark="材料规格" />
                                    <mi:MapItem SrcField="COL_5" TargetField="COL_9" Remark="计量单位" />
                                    <mi:MapItem SrcField="COL_7" TargetField="COL_17" Remark="厚度" />
                                    <mi:MapItem SrcField="COL_8" TargetField="COL_5" Remark="密度" />
                                    <mi:MapItem SrcField="COL_6" TargetField="COL_4" Remark="宽度" />
                                    <mi:MapItem SrcField="COL_13" TargetField="COL_19" Remark="长度" />
                                    <mi:MapItem SrcField="COL_11" TargetField="COL_37" Remark="默认仓库编号" />
                                    <mi:MapItem SrcField="COL_9" TargetField="COL_38" Remark="仓库名称" />
                                </MapItems>
                            </mi:TriggerColumn>
                            <mi:BoundField HeaderText="是否需要植绒" DataField="COL_13" Width="90" />
                            <mi:BoundField HeaderText="材料规格" DataField="COL_3"  Width="70" />
                            <mi:BoundField HeaderText="厚度" DataField="COL_17"  Width="45" />
                            <mi:BoundField HeaderText="密度" DataField="COL_5"  Width="45" />
                            <mi:BoundField HeaderText="宽度" DataField="COL_4"  Width="45" />
                            <mi:BoundField HeaderText="板长" DataField="COL_6"  Width="45" />
                            <mi:BoundField HeaderText="长度" DataField="COL_19"  Width="45" />
							
							<mi:BoundField HeaderText="需求单位" DataField="COL_91"  Width="70" />

                            <mi:BoundField HeaderText="需求单位" DataField="COL_91"  Width="70" />
                            <mi:BoundField HeaderText="需求量" DataField="COL_90"  Width="70" />
                            <mi:BoundField HeaderText="主单位" DataField="COL_9"  Width="70" />
                            <mi:BoundField HeaderText="主数量" DataField="COL_12"  Width="70" />
							
                            <mi:SelectColumn HeaderText="计量单位" DataField="COL_92"  Width="70" TriggerMode="None">
                                <mi:ListItem Value="公斤" Text="公斤" />
                                <mi:ListItem Value="KG" Text="KG" />
                                <mi:ListItem Value="米" Text="米" />
                                <mi:ListItem Value="张" Text="张" />
                            </mi:SelectColumn>

                            <mi:BoundField HeaderText="数量" DataField="COL_35"  Width="70" />
                            <mi:BoundField HeaderText="50KG生产个数" DataField="COL_18"  Width="120" />
                  

                            <mi:BoundField HeaderText="默认仓库" DataField="COL_38"  Width="70" />
                            <mi:BoundField HeaderText="默认仓库编码" DataField="COL_37"  Width="70"  />
                            <mi:BoundField HeaderText="备注" DataField="COL_14"  Width="70" />

                            <mi:NumColumn HeaderText="实需张数" DataField="COL_8"  Width="70" />
                            <mi:BoundField HeaderText="成品类型" DataField="COL_43"  Width="70" />
                            <mi:BoundField HeaderText="成品类型编码" DataField="COL_44"  Width="70" />
                            <mi:NumColumn HeaderText="材料ID" DataField="COL_52"  Width="70" />

                            <mi:NumColumn HeaderText="业务状态" DataField="BIZ_SID" />
              

                            <mi:BoundField HeaderText="排摸个数" DataField="COL_64" Width="70"  />
                            <mi:NumColumn HeaderText="生产总个数" DataField="COL_65"  Width="70" />
                            <mi:BoundField HeaderText="分析状态" DataField="COL_66"  Width="70" />
                            <mi:NumColumn HeaderText="分析值" DataField="COL_67"  Width="70" />
                            <mi:BoundField HeaderText="分析时间" DataField="COL_68"  Width="70" />
                            <mi:NumColumn HeaderText="分析记录时间" DataField="COL_69"  Width="70" />
                            <mi:BoundField HeaderText="预拉板数" DataField="COL_72" Width="70"  />
                            <mi:NumColumn HeaderText="主表ID" DataField="COL_73" />
                            <mi:NumColumn HeaderText="生产单ID" DataField="COL_74" />
                            <mi:BoundField HeaderText="生产单号" DataField="COL_75" />
                            <mi:NumColumn HeaderText="订单ID" DataField="COL_76" />
                            <mi:BoundField HeaderText="订单号" DataField="COL_77" Width="80"  />

                            <mi:NumColumn HeaderText="UT_102_ID" DataField="UT_102_ID" />
                            <mi:NumColumn HeaderText="UT_196_ID" DataField="UT_196_ID" />
                            <mi:BoundField HeaderText="来源--自制" DataField="COL_89" />
                            </Columns>
                    </mi:Table>
                </mi:Tab>

                <mi:Tab runat="server" ID="tab5" Text="TD生产需求"  FlowDirection="TopDown" ItemClass="mi-box-item" Layout="HBox" 
                    Scroll="Vertical" IsDelayRender="true">
                    <mi:Toolbar runat="server" ID="Toolbar7" IsDelayRender="true" >
                        <mi:ToolBarButton Text="新建" OnClick="ser:StoreUT196A.Insert();"  />
                        <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT196A.Refresh();"  />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT196A.Delete();" />
                     </mi:Toolbar>
                     <!--UT_196	生产管理BOM配方表-构件明细   T领料需求-->
                    <mi:Table runat="server" ID="Table2" Dock="Full" StoreID="StoreUT196A"  
                        PagerVisible="true" ReadOnly="true"   IsDelayRender="true">
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

                <mi:Tab runat="server" ID="tab6" Text="TD委外发料需求"  FlowDirection="TopDown" ItemClass="mi-box-item" Layout="HBox" 
                    Scroll="Vertical" IsDelayRender="true">
                    <mi:Toolbar runat="server" ID="Toolbar8" IsDelayRender="true" >
                        <mi:ToolBarButton Text="新建" OnClick="ser:StoreUT237.Insert();"  />
                        <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT237.Refresh();"  />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT237.Delete();" />
                     </mi:Toolbar>
                     <!--UT_196	生产管理BOM配方表-构件明细   T领料需求-->
                    <mi:Table runat="server" ID="Table3" Dock="Full" StoreID="StoreUT237"  
                        PagerVisible="true"   IsDelayRender="true">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:RowCheckColumn />
                            <mi:NumColumn HeaderText="[单头ID]" DataField="COL_15" Width="70"  />
                            <mi:BoundField HeaderText="生产指令单号" DataField="COL_16"  Width="90" />
                            <mi:TriggerColumn HeaderText="材料编号" DataField="COL_1" Width="100" ButtonClass="mi-icon-more" 
                                OnButtonClick="showDialgoForTable_102(this,'VIEW',740)" 
                                OnMaping="mapping_103(this,'VIEW',740,'COL_2')"  >
                                <MapItems>
                                    <mi:MapItem SrcField="ROW_IDENTITY_ID" TargetField="COL_52" Remark="材料ID" />
                                    <mi:MapItem SrcField="COL_12" TargetField="COL_55" Remark="材料类型编码" />
                                    <mi:MapItem SrcField="COL_2" TargetField="COL_1" Remark="编号" />
                                    <mi:MapItem SrcField="COL_3" TargetField="COL_2" Remark="材料名称" />
                                    <mi:MapItem SrcField="COL_4" TargetField="COL_3" Remark="材料规格" />
                                    <mi:MapItem SrcField="COL_5" TargetField="COL_9" Remark="计量单位" />
                                    <mi:MapItem SrcField="COL_7" TargetField="COL_17" Remark="厚度" />
                                    <mi:MapItem SrcField="COL_8" TargetField="COL_5" Remark="密度" />
                                    <mi:MapItem SrcField="COL_6" TargetField="COL_4" Remark="宽度" />
                                    <mi:MapItem SrcField="COL_13" TargetField="COL_19" Remark="长度" />
                                    <mi:MapItem SrcField="COL_11" TargetField="COL_37" Remark="默认仓库编号" />
                                    <mi:MapItem SrcField="COL_9" TargetField="COL_38" Remark="仓库名称" />
                                </MapItems>
                            </mi:TriggerColumn>
                           <mi:TriggerColumn HeaderText="材料名称" DataField="COL_2" Width="160" ButtonClass="mi-icon-more" 
                                OnButtonClick="showDialgoForTable_102(this,'VIEW',740)" 
                                OnMaping="mapping_103(this,'VIEW',740,'COL_3')"  >
                                <MapItems>
                                    <mi:MapItem SrcField="ROW_IDENTITY_ID" TargetField="COL_52" Remark="材料ID" />
                                    <mi:MapItem SrcField="COL_12" TargetField="COL_55" Remark="材料类型编码" />
                                    <mi:MapItem SrcField="COL_2" TargetField="COL_1" Remark="编号" />
                                    <mi:MapItem SrcField="COL_3" TargetField="COL_2" Remark="材料名称" />
                                    <mi:MapItem SrcField="COL_4" TargetField="COL_3" Remark="材料规格" />
                                    <mi:MapItem SrcField="COL_5" TargetField="COL_9" Remark="计量单位" />
                                    <mi:MapItem SrcField="COL_7" TargetField="COL_17" Remark="厚度" />
                                    <mi:MapItem SrcField="COL_8" TargetField="COL_5" Remark="密度" />
                                    <mi:MapItem SrcField="COL_6" TargetField="COL_4" Remark="宽度" />
                                    <mi:MapItem SrcField="COL_13" TargetField="COL_19" Remark="长度" />
                                    <mi:MapItem SrcField="COL_11" TargetField="COL_37" Remark="默认仓库编号" />
                                    <mi:MapItem SrcField="COL_9" TargetField="COL_38" Remark="仓库名称" />
                                </MapItems>
                            </mi:TriggerColumn>
                            <mi:BoundField HeaderText="是否需要植绒" DataField="COL_13" Width="90" />
                            <mi:BoundField HeaderText="材料规格" DataField="COL_3"  Width="70" />
                            <mi:BoundField HeaderText="厚度" DataField="COL_17"  Width="45" />
                            <mi:BoundField HeaderText="密度" DataField="COL_5"  Width="45" />
                            <mi:BoundField HeaderText="宽度" DataField="COL_4"  Width="45" />
                            <mi:BoundField HeaderText="板长" DataField="COL_6"  Width="45" />
                            <mi:BoundField HeaderText="长度" DataField="COL_19"  Width="45" />
							
							<mi:BoundField HeaderText="需求单位" DataField="COL_91"  Width="70" />

                            <mi:BoundField HeaderText="需求单位" DataField="COL_91"  Width="70" />
                            <mi:BoundField HeaderText="需求量" DataField="COL_90"  Width="70" />
                            <mi:BoundField HeaderText="主单位" DataField="COL_9"  Width="70" />
                            <mi:BoundField HeaderText="主数量" DataField="COL_12"  Width="70" />
							
                            <mi:SelectColumn HeaderText="计量单位" DataField="COL_92"  Width="70" TriggerMode="None">
                                <mi:ListItem Value="公斤" Text="公斤" />
                                <mi:ListItem Value="KG" Text="KG" />
                                <mi:ListItem Value="米" Text="米" />
                                <mi:ListItem Value="张" Text="张" />
                            </mi:SelectColumn>

                            <mi:BoundField HeaderText="数量" DataField="COL_35"  Width="70" />
                            <mi:BoundField HeaderText="50KG生产个数" DataField="COL_18"  Width="120" />
                  

                            <mi:BoundField HeaderText="默认仓库" DataField="COL_38"  Width="70" />
                            <mi:BoundField HeaderText="默认仓库编码" DataField="COL_37"  Width="70"  />
                            <mi:BoundField HeaderText="备注" DataField="COL_14"  Width="70" />

                            <mi:NumColumn HeaderText="实需张数" DataField="COL_8"  Width="70" />
                            <mi:BoundField HeaderText="成品类型" DataField="COL_43"  Width="70" />
                            <mi:BoundField HeaderText="成品类型编码" DataField="COL_44"  Width="70" />
                            <mi:NumColumn HeaderText="材料ID" DataField="COL_52"  Width="70" />

                            <mi:NumColumn HeaderText="业务状态" DataField="BIZ_SID" />
              

                            <mi:BoundField HeaderText="排摸个数" DataField="COL_64" Width="70"  />
                            <mi:NumColumn HeaderText="生产总个数" DataField="COL_65"  Width="70" />
                            <mi:BoundField HeaderText="分析状态" DataField="COL_66"  Width="70" />
                            <mi:NumColumn HeaderText="分析值" DataField="COL_67"  Width="70" />
                            <mi:BoundField HeaderText="分析时间" DataField="COL_68"  Width="70" />
                            <mi:NumColumn HeaderText="分析记录时间" DataField="COL_69"  Width="70" />
                            <mi:BoundField HeaderText="预拉板数" DataField="COL_72" Width="70"  />
                            <mi:NumColumn HeaderText="主表ID" DataField="COL_73" />
                            <mi:NumColumn HeaderText="生产单ID" DataField="COL_74" />
                            <mi:BoundField HeaderText="生产单号" DataField="COL_75" />
                            <mi:NumColumn HeaderText="订单ID" DataField="COL_76" />
                            <mi:BoundField HeaderText="订单号" DataField="COL_77" Width="80"  />

                            <mi:NumColumn HeaderText="UT_102_ID" DataField="UT_102_ID" />
                            <mi:NumColumn HeaderText="UT_196_ID" DataField="UT_196_ID" />
                            <mi:BoundField HeaderText="来源--自制" DataField="COL_89" />
                            </Columns>
                    </mi:Table>
                </mi:Tab>



                <mi:Tab runat="server" ID="tab2" Text="TD外发产品需求"  FlowDirection="TopDown" ItemClass="mi-box-item" Layout="HBox" Scroll="Vertical" IsDelayRender="true" >
                    <mi:Toolbar runat="server" ID="Toolbar6" IsDelayRender="true" >
                        <mi:ToolBarButton Text="新建" OnClick="ser:StoreUT239.Insert();"  />
                        <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT239.Refresh();"  />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT239.Delete();" />
                     </mi:Toolbar>
                     <!--UT_196	生产管理BOM配方表-构件明细    外发加工需求-->

                    <mi:Table runat="server" ID="UT_196_BTable" Dock="Full" StoreID="StoreUT239"  PagerVisible="true"   IsDelayRender="true">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:RowCheckColumn />
                            <mi:NumColumn HeaderText="[单头ID]" DataField="COL_15" Width="70"  />
                            <mi:BoundField HeaderText="生产指令单号" DataField="COL_16"  Width="70" />

                            <mi:TriggerColumn HeaderText="材料编号" DataField="COL_1" Width="100" ButtonClass="mi-icon-more" 
                                OnButtonClick="showDialgoForTable_102(this,'VIEW',740)" 
                                OnMaping="mapping_103(this,'VIEW',740,'COL_2')"  >
                                <MapItems>
                                    <mi:MapItem SrcField="ROW_IDENTITY_ID" TargetField="COL_52" Remark="材料ID" />
                                    <mi:MapItem SrcField="COL_12" TargetField="COL_55" Remark="材料类型编码" />
                                    <mi:MapItem SrcField="COL_2" TargetField="COL_1" Remark="编号" />
                                    <mi:MapItem SrcField="COL_3" TargetField="COL_2" Remark="材料名称" />
                                    <mi:MapItem SrcField="COL_4" TargetField="COL_3" Remark="材料规格" />
                                    <mi:MapItem SrcField="COL_5" TargetField="COL_9" Remark="计量单位" />
                                    <mi:MapItem SrcField="COL_7" TargetField="COL_17" Remark="厚度" />
                                    <mi:MapItem SrcField="COL_8" TargetField="COL_5" Remark="密度" />
                                    <mi:MapItem SrcField="COL_6" TargetField="COL_4" Remark="宽度" />
                                    <mi:MapItem SrcField="COL_13" TargetField="COL_19" Remark="长度" />
                                    <mi:MapItem SrcField="COL_11" TargetField="COL_37" Remark="默认仓库编号" />
                                    <mi:MapItem SrcField="COL_9" TargetField="COL_38" Remark="仓库名称" />
                                </MapItems>
                            </mi:TriggerColumn>
                           <mi:TriggerColumn HeaderText="材料名称" DataField="COL_2" Width="160" ButtonClass="mi-icon-more" 
                                OnButtonClick="showDialgoForTable_102(this,'VIEW',740)" 
                                OnMaping="mapping_103(this,'VIEW',740,'COL_3')"  >
                                <MapItems>
                                    <mi:MapItem SrcField="ROW_IDENTITY_ID" TargetField="COL_52" Remark="材料ID" />
                                    <mi:MapItem SrcField="COL_12" TargetField="COL_55" Remark="材料类型编码" />
                                    <mi:MapItem SrcField="COL_2" TargetField="COL_1" Remark="编号" />
                                    <mi:MapItem SrcField="COL_3" TargetField="COL_2" Remark="材料名称" />
                                    <mi:MapItem SrcField="COL_4" TargetField="COL_3" Remark="材料规格" />
                                    <mi:MapItem SrcField="COL_5" TargetField="COL_9" Remark="计量单位" />
                                    <mi:MapItem SrcField="COL_7" TargetField="COL_17" Remark="厚度" />
                                    <mi:MapItem SrcField="COL_8" TargetField="COL_5" Remark="密度" />
                                    <mi:MapItem SrcField="COL_6" TargetField="COL_4" Remark="宽度" />
                                    <mi:MapItem SrcField="COL_13" TargetField="COL_19" Remark="长度" />
                                    <mi:MapItem SrcField="COL_11" TargetField="COL_37" Remark="默认仓库编号" />
                                    <mi:MapItem SrcField="COL_9" TargetField="COL_38" Remark="仓库名称" />
                                </MapItems>
                            </mi:TriggerColumn>
                            <mi:BoundField HeaderText="是否需要植绒" DataField="COL_13"  Width="70" />
                            <mi:BoundField HeaderText="材料规格" DataField="COL_3"  Width="70" />
                            <mi:BoundField HeaderText="厚度" DataField="COL_17"  Width="45" />
                            <mi:BoundField HeaderText="密度" DataField="COL_5"  Width="45" />
                            <mi:BoundField HeaderText="长度" DataField="COL_19"  Width="50" />
                            <mi:BoundField HeaderText="宽度" DataField="COL_4"  Width="45" />

                            <mi:BoundField HeaderText="需求单位" DataField="COL_91"  Width="70" />
                            <mi:BoundField HeaderText="需求量" DataField="COL_90" Width="70"  />


                            <mi:BoundField HeaderText="主单位" DataField="COL_9"  Width="70" />
                            <mi:BoundField HeaderText="主数量" DataField="COL_12"  Width="70" />

                            <mi:SelectColumn HeaderText="计量单位" DataField="COL_39"  Width="70" TriggerMode="None">
                                <mi:ListItem Value="公斤" Text="公斤" />
                                <mi:ListItem Value="KG" Text="KG" />
                                <mi:ListItem Value="米" Text="米" />
                                <mi:ListItem Value="张" Text="张" />
                            </mi:SelectColumn>

                            <mi:BoundField HeaderText="数量" DataField="COL_10"  Width="70" />
                            <mi:BoundField HeaderText="500KG生产个数" DataField="COL_18"  Width="100" />

                            <mi:BoundField HeaderText="默认仓库" DataField="COL_38" Width="70"  />
                            <mi:BoundField HeaderText="默认仓库编码" DataField="COL_37"  Width="90" />
                            <mi:BoundField HeaderText="备注" DataField="COL_14"  Width="70" />
                            <mi:NumColumn HeaderText="实需张数" DataField="COL_8"  Width="70" />
                            <mi:NumColumn HeaderText="材料ID" DataField="COL_52"  Width="70" />
                            <mi:NumColumn HeaderText="业务状态" DataField="BIZ_SID" Width="70"  />
                            <mi:BoundField HeaderText="排摸个数" DataField="COL_64"  Width="70" />
                            <mi:NumColumn HeaderText="生产总个数" DataField="COL_65"  Width="80" />
                            <mi:BoundField HeaderText="分析状态" DataField="COL_66" Width="70"  />
                            <mi:NumColumn HeaderText="分析值" DataField="COL_67" />
                            <mi:BoundField HeaderText="分析时间" DataField="COL_68"  Width="90" />
                            <mi:NumColumn HeaderText="分析记录时间" DataField="COL_69" />
                            <mi:BoundField HeaderText="预拉板数" DataField="COL_72"  Width="70" />
                            <mi:BoundField HeaderText="主表ID" DataField="COL_73" />
                            <mi:BoundField HeaderText="父键ID" DataField="BIZ_PARENT_ID" />
                            <mi:BoundField HeaderText="UT_102_ID" DataField="UT_102_ID" />
                            <mi:BoundField HeaderText="UT_196_ID" DataField="UT_196_ID" />
                            <mi:BoundField HeaderText="来源" DataField="COL_89"  Width="90" />
                            </Columns>
                    </mi:Table>

                </mi:Tab>


                <mi:Tab runat="server" ID="tab3" Text="TD采购需求"  FlowDirection="TopDown" ItemClass="mi-box-item" Layout="HBox" Scroll="Vertical" IsDelayRender="true" >
                    <!--	UT_196	生产管理BOM配方表-构件明细-->
                    <mi:Toolbar runat="server" ID="Toolbar4" IsDelayRender="true" >
                        <mi:ToolBarButton Text="新建" OnClick="ser:StoreUT216.Insert();"  />
                        <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT216.Refresh();"  />
                        <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT216.Delete();" />
                         <mi:ToolBarButton  Icon="/res/icon_sys/Setup.png"  Text="采购需求分析" Command="btnChangeRowSid0_2"  />
                         <mi:ToolBarButton  Text="撤销需求分析"  Command="btnChangeRowSid2_0" />
                    </mi:Toolbar>
 
                    <mi:Table runat="server" ID="UT_216Table" Dock="Full" StoreID="StoreUT216"  PagerVisible="true"   IsDelayRender="true">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:NumColumn HeaderText="[单头ID]" DataField="COL_15" Width="70"  />
                            <mi:BoundField HeaderText="分析状态" DataField="COL_66"  Width="70" />
                            <mi:NumColumn HeaderText="分析值" DataField="COL_67" />
                            <mi:BoundField HeaderText="分析时间" DataField="COL_68" Width="90"  />
                            <mi:NumColumn HeaderText="分析记录时间" DataField="COL_69" />
                            <mi:BoundField HeaderText="生产指令单号" DataField="COL_16"  Width="90" />
                            <mi:TriggerColumn HeaderText="材料编号" DataField="COL_1" Width="100" ButtonClass="mi-icon-more" 
                                OnButtonClick="showDialgoForTable_102(this,'VIEW',740)" 
                                OnMaping="mapping_103(this,'VIEW',740,'COL_2')"  >
                                <MapItems>
                                    <mi:MapItem SrcField="ROW_IDENTITY_ID" TargetField="COL_52" Remark="材料ID" />
                                    <mi:MapItem SrcField="COL_12" TargetField="COL_55" Remark="材料类型编码" />
                                    <mi:MapItem SrcField="COL_2" TargetField="COL_1" Remark="编号" />
                                    <mi:MapItem SrcField="COL_3" TargetField="COL_2" Remark="材料名称" />
                                    <mi:MapItem SrcField="COL_4" TargetField="COL_3" Remark="材料规格" />
                                    <mi:MapItem SrcField="COL_5" TargetField="COL_9" Remark="计量单位" />
                                    <mi:MapItem SrcField="COL_7" TargetField="COL_17" Remark="厚度" />
                                    <mi:MapItem SrcField="COL_8" TargetField="COL_5" Remark="密度" />
                                    <mi:MapItem SrcField="COL_6" TargetField="COL_4" Remark="宽度" />
                                    <mi:MapItem SrcField="COL_13" TargetField="COL_19" Remark="长度" />
                                    <mi:MapItem SrcField="COL_11" TargetField="COL_37" Remark="默认仓库编号" />
                                    <mi:MapItem SrcField="COL_9" TargetField="COL_38" Remark="仓库名称" />
                                </MapItems>
                            </mi:TriggerColumn>
                            <mi:TriggerColumn HeaderText="材料名称" DataField="COL_2" Width="160" ButtonClass="mi-icon-more" 
                                OnButtonClick="showDialgoForTable_102(this,'VIEW',740)" 
                                OnMaping="mapping_103(this,'VIEW',740,'COL_3')"  >
                                <MapItems>
                                    <mi:MapItem SrcField="ROW_IDENTITY_ID" TargetField="COL_52" Remark="材料ID" />
                                    <mi:MapItem SrcField="COL_12" TargetField="COL_55" Remark="材料类型编码" />
                                    <mi:MapItem SrcField="COL_2" TargetField="COL_1" Remark="编号" />
                                    <mi:MapItem SrcField="COL_3" TargetField="COL_2" Remark="材料名称" />
                                    <mi:MapItem SrcField="COL_4" TargetField="COL_3" Remark="材料规格" />
                                    <mi:MapItem SrcField="COL_5" TargetField="COL_9" Remark="计量单位" />
                                    <mi:MapItem SrcField="COL_7" TargetField="COL_17" Remark="厚度" />
                                    <mi:MapItem SrcField="COL_8" TargetField="COL_5" Remark="密度" />
                                    <mi:MapItem SrcField="COL_6" TargetField="COL_4" Remark="宽度" />
                                    <mi:MapItem SrcField="COL_13" TargetField="COL_19" Remark="长度" />
                                    <mi:MapItem SrcField="COL_11" TargetField="COL_37" Remark="默认仓库编号" />
                                    <mi:MapItem SrcField="COL_9" TargetField="COL_38" Remark="仓库名称" />
                                </MapItems>
                            </mi:TriggerColumn>
                            <mi:BoundField HeaderText="是否需要植绒" DataField="COL_13"  Width="100" />
                            <mi:BoundField HeaderText="材料规格" DataField="COL_3"  Width="70"  />
                            <mi:BoundField HeaderText="厚度" DataField="COL_17"  Width="45" />
                            <mi:BoundField HeaderText="密度" DataField="COL_5"  Width="45" />
                            <mi:BoundField HeaderText="长度" DataField="COL_19"  Width="50" />
                            <mi:BoundField HeaderText="宽度" DataField="COL_4"  Width="45" />
                            <mi:BoundField HeaderText="板长" DataField="COL_6"  Width="45" />

                             <mi:BoundField HeaderText="需求量" DataField="COL_96"  Width="70" />
                             <mi:BoundField HeaderText="需求单位" DataField="COL_97"  Width="70" />
                             <mi:BoundField HeaderText="主数量" DataField="COL_12"  Width="70" />
                             <mi:BoundField HeaderText="主单位" DataField="COL_9"  Width="70" />

                            <mi:SelectColumn HeaderText="计量单位" DataField="COL_39"  Width="70"  TriggerMode="None">
                                <mi:ListItem Value="公斤" Text="公斤" />
                                <mi:ListItem Value="KG" Text="KG" />
                                <mi:ListItem Value="米" Text="米" />
                                <mi:ListItem Value="张" Text="张" />
                            </mi:SelectColumn>
                            <mi:BoundField HeaderText="数量" DataField="COL_35"  Width="70" />
                            <mi:BoundField HeaderText="50KG生产个数" DataField="COL_18"  Width="100" />

                            <mi:BoundField HeaderText="备注" DataField="COL_14"  Width="70" />
                            <mi:NumColumn HeaderText="实需张数" DataField="COL_8"  Width="70" />
                            <mi:BoundField HeaderText="是否采购" DataField="COL_47"  Width="70" />
                            <mi:BoundField HeaderText="库存数量" DataField="COL_48"  Width="70" />
                            <mi:BoundField HeaderText="采购在途" DataField="COL_70"  Width="70" />
                            <mi:BoundField HeaderText="待领料" DataField="COL_71"  Width="70" />
                            <mi:BoundField HeaderText="可用数量" DataField="COL_49"  Width="70" />
                            <mi:BoundField HeaderText="采购数量" DataField="COL_50"  Width="70" />
                            <mi:BoundField HeaderText="建议采购量" DataField="COL_51"  Width="80" />
                            <mi:BoundField HeaderText="默认仓库" DataField="COL_38"  Width="90" />
                            <mi:BoundField HeaderText="默认仓库编码" DataField="COL_37"  Width="70" />
                            <mi:BoundField HeaderText="材料ID" DataField="COL_52"  Width="70" />

                            <mi:NumColumn HeaderText="业务状态" DataField="BIZ_SID"  Width="70" />
                            <mi:BoundField HeaderText="排摸个数" DataField="COL_64"  Width="70" />
                            <mi:NumColumn HeaderText="生产总个数" DataField="COL_65"  Width="80" />


                            <mi:BoundField HeaderText="预拉板数" DataField="COL_72"  Width="70" />
                            </Columns>
                    </mi:Table>

                </mi:Tab>

            </mi:TabPanel>

         </mi:Panel>

    </mi:Viewport>
</form>

<script type="text/javascript">

    function ShowGif(text) {
        var urlStr = $.format("/App/InfoGrid2/View/Biz/JLSLBZ/XSSCZL/Waiting2.aspx");

        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: 'TD智能分析',
            iframe: true,
            width: 300,
            height: 250,
            startPosition: 'center_screen',
            url: urlStr,
            maskOpacity: 1
        });
        win.show();

        win.formClosed(function () {
            Mini2.Msg.alert('提示', text);
        })
    }


    ///这是弹窗按钮事件
    function showDialgoForTable_102(owner, type_id, view_id) {

        var me = owner,
            tag = me.tag;
        var record = me.record;


        if (!type_id || !view_id) {
            console.log("弹窗事件没有必要的参数！，type_id  和 view_id");
            return;
        }


        var urlStr = $.format("/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={0}&viewId={1}",
            type_id, view_id);


        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '选择',
            iframe: true,
            width: 800,
            height: 600,
            state: 'max',
            startPosition: 'center_screen',
            url: urlStr
        });

        win.editor = me;
        win.record = record;
        win.show();

        win.formClosed(form_Closed102);

    }
   //这是引用映射规则
   function form_Closed102(e) {
        var me = this,
           record = me.record,
           editor = me.editor;

        if (e.result != 'ok') { return; }

        var row = e.row;
        var map = e.map;

        if (editor.mapItems && editor.mapItems.length > 0) {

            map = editor.mapItems;

            for (var i = 0; i < map.length; i++) {
                var m = map[i];
                var v = row[m.srcField];

                record.set(m.targetField, v);

                if (editor.dataIndex == m.targetField) {
                    editor.setValue(v);
                }
            }
        }
        else {
            for (var i = 0; i < map.length; i++) {
                var m = map[i];
                var v = row[m.src];

                record.set(m.to, v);

                if (editor.dataIndex == m.to) {
                    editor.setValue(v);
                }
            }
        }

    }


     //根据输入的文字获取数据   回车按钮触发
    function mapping_103(owner, type_id, view_id, one_field) {

        var me = owner;
        var record = me.record;


        var id = owner.getValue();
        var mapItems = owner.mapItems;  //映射字段的规则

        var base64 = window.BASE64.encoder("[{field:'"+one_field+"',logic:'like',value:'%" + id + "%'}]");//返回编码后的字符 

        $.get("/App/InfoGrid2/view/MoreView/OneMapAction.aspx", {
            type: type_id,
            viewId: view_id,
            id: id,
            one_field: '',
            filter2: base64,
            _rum: RndNum(8)
        },
        function (responseText, textStatus) {

            var model = eval('(' + responseText + ')');

            console.log(model);

            if (model.result != "ok") { return; }

            //如果只有一个就赋值到文本框中
            if (model.count == 1) {
                setMapValues(record, model.data, mapItems);
            }
            else {

                var urlStr = $.format("/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={0}&viewId={1}&filter2={2}",
                    type_id, view_id, base64);

                var win = Mini2.create('Mini2.ui.Window', {
                    mode: true,
                    text: '选择',
                    iframe: true,
                    width: 800,
                    height: 600,
                    state: 'max',
                    startPosition: 'center_screen',
                    url: urlStr
                });

                win.editor = me;
                win.record = record;
                win.show();

                win.formClosed(form_Closed102);


            }

        });

    }
    //这是生成随机数的
    function RndNum(n) {
        var rnd = "";
        for (var i = 0; i < n; i++)
            rnd += Math.floor(Math.random() * 10);
        return rnd;
    }
    //设置映射值
    function setMapValues(record, model, mapItems) {

        var me = this,
            i,
            item,
            srcValue;

        if (mapItems && model) {

            for (i = 0; i < mapItems.length; i++) {

                item = mapItems[i];

                if ('' == item.srcField || '' == item.targetField) {
                    continue;
                }

                srcValue = model[item.srcField];

                record.set(item.targetField, srcValue);
            }

        }
    }



</script>
