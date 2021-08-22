<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormXssczl.ascx.cs" Inherits="App.InfoGrid2.View.Biz.JLSLBZ.Currency.FormXssczl" %>


<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>



<style type="text/css">
    .page-head
    {
         font-size:26px;
         font-weight:bold;
    }
</style>

<% if (false)
   { %>
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<% } %>


<form action="" method="post">

<!--UT_102 吸塑指令单-订单明细-->
<mi:Store runat="server" ID="StoreUT102" Model="UT_102" IdField="ROW_IDENTITY_ID" DeleteRecycle="true">
    <FilterParams>
        <mi:QueryStringParam Name="COL_13" QueryStringField="id" />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="ROW_IDENTITY_ID" ControlID="mainClientTable" PropertyName="CheckedRows" />
    </DeleteQuery>

    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>
    
    <InsertParams>
        <mi:QueryStringParam Name="COL_13" QueryStringField="id" />
        <mi:Param Name="COL_16" DefaultValue="大货生产单" />
         <mi:Param Name="COL_84" DefaultValue="1" />    
    </InsertParams>
    <SummaryFields>
        <mi:SummaryField DataField="COL_7" SummaryType="SUM"></mi:SummaryField>
        <mi:SummaryField DataField="COL_8" SummaryType="SUM"></mi:SummaryField>
        <mi:SummaryField DataField="COL_9" SummaryType="SUM"></mi:SummaryField>
    </SummaryFields>
</mi:Store>
<!--	UT_103	吸塑指令单-主材料	-->
<mi:Store runat="server" ID="StoreUT103" Model="UT_103" IdField="ROW_IDENTITY_ID" DeleteRecycle="true">
    <FilterParams>
        <mi:QueryStringParam Name="COL_15" QueryStringField="id" />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="ROW_IDENTITY_ID" ControlID="Table103" PropertyName="CheckedRows" />
    </DeleteQuery>

    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>
    <InsertParams> 
        <mi:QueryStringParam Name="COL_15" QueryStringField="id" />   
    </InsertParams>
</mi:Store>
<!--	UT_104	吸塑指令单-工序工艺	-->
<mi:Store runat="server" ID="StoreUT104" Model="UT_104" IdField="ROW_IDENTITY_ID" DeleteRecycle="true" SortText="COL_GXSX asc">
    <FilterParams>
        <mi:QueryStringParam Name="COL_12" QueryStringField="id" />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:Param Name="COL_29" DefaultValue="999"/>
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="ROW_IDENTITY_ID" ControlID="Table104" PropertyName="CheckedRows" />
    </DeleteQuery>

    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>

    <InsertParams>
        <mi:QueryStringParam Name="COL_12" QueryStringField="id" />
        <mi:Param Name="COL_29" DefaultValue="999"/>
        <mi:Param Name="COL_8" DefaultValue="0" />
        <mi:Param Name="BIZ_SID" DefaultValue="0" />
        <mi:Param  Name="COL_32" DefaultValue="."/>
        <mi:param Name="COL_38"  DefaultValue="."/>
        <mi:param Name="COL_42"  DefaultValue="."/>
    </InsertParams>
</mi:Store>

<!--UT_105  吸塑指令单-模具-->
<mi:Store runat="server" ID="StoreUT105" Model="UT_104" IdField="ROW_IDENTITY_ID" DeleteRecycle="true">
    <FilterParams>
        <mi:QueryStringParam Name="COL_12" QueryStringField="id" />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:Param Name="COL_29" DefaultValue="888"/>
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="ROW_IDENTITY_ID" ControlID="Table105" PropertyName="CheckedRows" />
    </DeleteQuery>

    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>
    <InsertParams>
        <mi:QueryStringParam Name="COL_12" QueryStringField="id" />
        <mi:Param Name="COL_29" DefaultValue="888"/>
        <mi:Param  Name="COL_32" DefaultValue="."/>
        <mi:param Name="COL_38"  DefaultValue="."/>
        <mi:param Name="COL_42"  DefaultValue="."/>
    </InsertParams>
</mi:Store>


<!--UT_106  吸塑指令单-生产机台-->
<mi:Store runat="server" ID="StoreUT313" Model="UT_313" IdField="ROW_IDENTITY_ID" DeleteRecycle="true">
    <FilterParams>
        <mi:QueryStringParam Name="COL_11" QueryStringField="id" />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />

    </FilterParams>    
    <DeleteQuery>
        <mi:ControlParam Name="ROW_IDENTITY_ID" ControlID="Table106" PropertyName="CheckedRows" />
    </DeleteQuery>

    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>
    <InsertParams>
        <mi:QueryStringParam Name="COL_11" QueryStringField="id" />
    </InsertParams>

</mi:Store>
    <mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="10">
        <mi:Label runat="server" ID="headLab" Value="吸塑生产指令单" HideLabel="true" Dock="Center" Mode="Transform" />
    </mi:Panel>
<mi:Viewport runat="server" ID="viewport1" Padding="4" ItemMarginRight="6" MarginTop="60">

    <mi:Panel runat="server" ID="TopPanel" Region="North" Scroll="None" Height="100" MinHeigh="70" >

        <mi:Toolbar runat="server" ID="topToolbar" >
<%--            <mi:ToolBarButton Text="增加产品组" OnClick="showDialgoForTable(this)" />--%>
            <mi:ToolBarButton Text="引入生产计划" OnClick="showDialgoFor1505(this)" />
            <mi:ToolBarButton Text="导入研发计划" OnClick="showDialgoFor1880(this)" />
   <%--         <mi:ToolBarButton Text="TD云智能工艺分析" Command="AutoInputData"/>--%>
            <mi:ToolBarButton Text="智能工艺分析" Command="GoAutoInputData" />
            <mi:ToolBarButton Text="撤销工艺分析" Command="GoChangeCOL_15_2To1" />
        <%--<mi:ToolBarButton Text="手工翻单"  />
            <mi:ToolBarButton Text="查看生产记录" Visible="false" />
            <mi:ToolBarHr />--%>
            <mi:ToolBarButton Text="提交" Command="GoSubmit" />
             <mi:ToolBarButton Text="撤销审核" Command="Revoke" />
            

            <mi:ToolBarButton Text="结案" Command="GoSubmitClosed" />
            <mi:ToolBarButton Text="撤销结案" Command="RevokeClosed" />

            <%--  这个是旧的打印 可以用的    <mi:ToolBarButton Text="打印"  OnClick="PrintExcel()"/>--%>
            <mi:ToolBarButton Text="打印"  Command="GoShowSelectPrintView" />
            <mi:ToolBarButton Text="需求量计算" Command="AutoCalculation" />

        </mi:Toolbar>

        <mi:FormLayout runat="server" ID="mainForm" ItemWidth="300"  FlowDirection="LeftToRight" ItemLabelAlign="Right" Dock="Full" PaddingTop="4" >
            <mi:DatePicker runat="server" ID="TextBox12" FieldLabel="时间" Width="400"  />
            <mi:TextBox  runat="server" ID="TextBox1" FieldLabel="生产单号"  />
            <mi:ComboBox runat="server" ID="comboBox2" FieldLabel="单据类型" DefaultValue="正常生产单" Width="180" TriggerMode="None" >
                <mi:ListItem Value="模板" Text="模板" />
                <mi:ListItem Value="正常生产单" Text="正常生产单" />
            </mi:ComboBox>
            <mi:ComboBox runat="server" ID="comboBox1" FieldLabel="生产模式" DefaultValue="自制" Width="180" TriggerMode="None" >
                <mi:ListItem Value="整单外发" Text="整单外发" />
                <mi:ListItem Value="自制" Text="自制" />
            </mi:ComboBox>
            <mi:ComboBox runat="server" ID="comboBox3" FieldLabel="是否生产" DefaultValue="是" Width="180" TriggerMode="None" >

                <mi:ListItem Value="是" Text="是" />
                <mi:ListItem Value="否" Text="否" />
            </mi:ComboBox>
            <mi:ComboBox runat="server" ID="comboBox4" FieldLabel="混合排版" DefaultValue="单客户" Width="180" TriggerMode="None"  Visible="false"  >
                <mi:ListItem Value="单客户" Text="单客户" />
                <mi:ListItem Value="多客户" Text="多客户" />
            </mi:ComboBox>
        </mi:FormLayout>

    </mi:Panel>

    <mi:Panel runat="server" ID="Panel1" Region="North" Scroll="None" Height="150" >
        <mi:Toolbar runat="server" ID="Toolbar2" >
            <mi:ToolBarTitle Text="订单" />
            <mi:ToolBarButton Text="新增" OnClick="ser:StoreUT102.Insert();"  />
            <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT102.Refresh();"  />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT102.Delete();" />
            <mi:ToolBarButton ID="tbMessage" />
        </mi:Toolbar>

        <mi:Table runat="server" ID="mainClientTable" Dock="Full" StoreID="StoreUT102"  PagerVisible="false" SummaryVisible="true">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:TriggerColumn HeaderText="客户编号" DataField="COL_22"  ButtonClass="mi-icon-more" 
                    OnButtonClick="showDialgoForTable_102(this,'VIEW',275)" 
                    OnMaping="mapping_103(this,'VIEW',275,'COL_1')" 
                    Tag='{"type_id":"VIEW","view_id":275, "one_field":"COL_1", "table_name":"UT_071","owner_table_id":213}' Width="70" >
                    <MapItems>
                        <mi:MapItem SrcField="COL_2" TargetField="COL_1" Remark="客户名称" />
                        <mi:MapItem SrcField="COL_1" TargetField="COL_22" Remark="客户编号" />
                    </MapItems>
                </mi:TriggerColumn>
                <mi:TriggerColumn HeaderText="客户名称" DataField="COL_1"  ButtonClass="mi-icon-more" 
                    OnButtonClick="showDialgoForTable_102(this,'VIEW',275)" 
                    OnMaping="mapping_103(this,'VIEW',275,'COL_2')" 
                    Tag='{"type_id":"VIEW","view_id":275, "one_field":"COL_1", "table_name":"UT_071","owner_table_id":213}' Width="70" >
                    <MapItems>
                        <mi:MapItem SrcField="COL_2" TargetField="COL_1" Remark="客户名称" />
                        <mi:MapItem SrcField="COL_1" TargetField="COL_22" Remark="客户编号" />
                    </MapItems>
                </mi:TriggerColumn>
                <mi:BoundField HeaderText="订单号" DataField="COL_2" Width="90" />
                <mi:TriggerColumn HeaderText="产品编号" DataField="COL_3" Width="90" ButtonClass="mi-icon-more" 
                    OnButtonClick="showDialgoForTable_102(this,'VIEW',1504)"  
                    Tag='{"type_id":"VIEW","view_id":723,"table_name":"UT_082","owner_table_id":213}' >
                    <MapItems>
                        <mi:MapItem SrcField="ROW_IDENTITY_ID" TargetField="COL_42" Remark="产品ID" />
                        <mi:MapItem SrcField="COL_12" TargetField="COL_29" Remark="产品类型编号" />
                        <mi:MapItem SrcField="COL_1" TargetField="COL_16" Remark="产品类型" />
                        <mi:MapItem SrcField="COL_2" TargetField="COL_3" Remark="产品编号" />
                         <mi:MapItem SrcField="COL_3" TargetField="COL_4" Remark="产品名称" />
                         <mi:MapItem SrcField="COL_4" TargetField="COL_5" Remark="规格" />
                         <mi:MapItem SrcField="COL_5" TargetField="COL_6" Remark="单位" />
                         <mi:MapItem SrcField="COL_14" TargetField="COL_23" Remark="仓库编号" />
                         <mi:MapItem SrcField="COL_11" TargetField="COL_24" Remark="仓库名称" />

                    </MapItems>
                </mi:TriggerColumn>
                 <mi:TriggerColumn HeaderText="产品名称" DataField="COL_4" Width="180" ButtonClass="mi-icon-more" 
                    OnButtonClick="showDialgoForTable_102(this,'VIEW',1504)" 
                    Tag='{"type_id":"VIEW","view_id":723,"table_name":"UT_082","owner_table_id":213}' >
                    <MapItems>
                          <mi:MapItem SrcField="ROW_IDENTITY_ID" TargetField="COL_42" Remark="产品ID" />
                        <mi:MapItem SrcField="COL_12" TargetField="COL_27" Remark="产品类型编号" />
                        <mi:MapItem SrcField="COL_1" TargetField="COL_16" Remark="产品类型" />
                        <mi:MapItem SrcField="COL_2" TargetField="COL_3" Remark="产品编号" />
                         <mi:MapItem SrcField="COL_3" TargetField="COL_4" Remark="产品名称" />
                         <mi:MapItem SrcField="COL_4" TargetField="COL_5" Remark="规格" />
                         <mi:MapItem SrcField="COL_5" TargetField="COL_6" Remark="单位" />
                         <mi:MapItem SrcField="COL_14" TargetField="COL_23" Remark="仓库编号" />
                         <mi:MapItem SrcField="COL_11" TargetField="COL_24" Remark="仓库名称" />
                    </MapItems>
                </mi:TriggerColumn>
                <mi:BoundField HeaderText="规格" DataField="COL_5"  Width="70" EditorMode="None" />
                <mi:BoundField HeaderText="计量单位" DataField="COL_6" Width="70" EditorMode="None" />
                <mi:NumColumn HeaderText="订单数量" DataField="COL_7" Width="70"  SummaryType="SUM" Format="0.##" />
                
                <mi:NumColumn HeaderText="单价" DataField="COL_32" Width="70"  Format="0.###"   Visible="false"  />

                <mi:NumColumn HeaderText="排摸个数" DataField="COL_8"  Width="70" SummaryType="SUM"  Format="0.##"   Visible="false"  />
                <mi:NumColumn HeaderText="生产数量" DataField="COL_9" SummaryType="SUM" Width="70"  Format="0.##" />
                <mi:NumColumn HeaderText="当前库存数量" DataField="COL_10" SummaryType="SUM" Width="90" EditorMode="None"   Visible="false"  Format="0.##" />
                <mi:DateColumn HeaderText="计划完工时间" DataField="COL_11" Width="90" />
              
                <mi:BoundField HeaderText="备注" DataField="COL_26" Width="200" />
                <mi:BoundField HeaderText="用料说明" DataField="COL_12" Width="180" EditorMode="None"   Visible="false"  />



                <mi:BoundField HeaderText="用料说明" DataField="COL_54" Width="180" EditorMode="None"  Visible="false"  />
                <mi:BoundField HeaderText="用料说明" DataField="COL_12" Width="180" EditorMode="None"  Visible="false" />
                <mi:BoundField HeaderText="用料说明" DataField="COL_12" Width="180" EditorMode="None"  Visible="false"  />
                <mi:BoundField HeaderText="送货地址" DataField="COL_25" Width="90" Visible="false"  />
                

<%--                <mi:BoundField HeaderText="自定义1" DataField="COL_27" />
                <mi:BoundField HeaderText="自定义2" DataField="COL_28" />
                <mi:BoundField HeaderText="自定义3" DataField="COL_29" />
                <mi:BoundField HeaderText="自定义4" DataField="COL_30" />
                <mi:BoundField HeaderText="自定义5" DataField="COL_31" />--%>

<%--           增加下列字段:

                <mi:BoundField HeaderText="产品类型编码" DataField=""  />
                <mi:BoundField HeaderText="产品类型" DataField=""  />
                <mi:BoundField HeaderText="仓库编码" DataField=""  />
                <mi:BoundField HeaderText="仓库名称" DataField="" />--%>

            </Columns>
        </mi:Table>
    </mi:Panel>

    <mi:Panel runat="server" Region="Center" ID="centerPanel1" ItemClass="mi-box-item" Layout="VBox" Scroll="Vertical">

        <mi:Panel runat="server" ID="panelXX1" Dock="Top" Scroll="None" Height="230">
            <mi:Toolbar runat="server" ID="Toolbar1" >
                <mi:ToolBarTitle Text="主材料" />
                <mi:ToolBarButton Text="新建" OnClick="ser:StoreUT103.Insert();"  />
                <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT103.Refresh();"  />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT103.Delete();" />
            </mi:Toolbar>

            <mi:Table runat="server" ID="Table103" Dock="Full" StoreID="StoreUT103" PagerVisible="false" Height="160" >
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:TriggerColumn HeaderText="材料编号" DataField="COL_1"  Width="80"
                        OnMaping="mapping_103(this,'VIEW',740,'COL_2')"
                        OnButtonClick="showDialgoForTable_102(this,'VIEW',740)" ButtonClass="mi-icon-more" 
                        Tag='{"type_id":"VIEW","view_id":740, "one_field":"COL_2", "table_name":"UT_083","owner_table_id":214}'
                        >
                        <MapItems>
                            <mi:MapItem SrcField="COL_3" TargetField="COL_2" Remark="材料名称" />
                            <mi:MapItem SrcField="COL_4" TargetField="COL_3" Remark="材料规格" />
                            <mi:MapItem SrcField="COL_6" TargetField="COL_4" Remark="宽度" />
                            <mi:MapItem SrcField="COL_8" TargetField="COL_5" Remark="密度" />
                            <mi:MapItem SrcField="COL_7" TargetField="COL_17" Remark="厚度" />
                            <mi:MapItem SrcField="ROW_IDENTITY_ID" TargetField="COL_52" Remark="产品ID" />
                            <mi:MapItem SrcField="COL_2" TargetField="COL_1" Remark="材料编号" />
                            <mi:MapItem SrcField="COL_5" TargetField="COL_9" Remark="计量单位" />
                            <mi:MapItem SrcField="COL_12" TargetField="COL_55" Remark="产品类型编号" />
                            <mi:MapItem SrcField="COL_11" TargetField="COL_37" Remark="仓库名称编号" />       
                            <mi:MapItem SrcField="COL_9" TargetField="COL_38" Remark="仓库名称" />   
                        </MapItems>
                    </mi:TriggerColumn>
                    <mi:TriggerColumn HeaderText="材料名称" DataField="COL_2" Width="150"
                        OnMaping="mapping_103(this,'VIEW',740,'COL_3')"
                        OnButtonClick="showDialgoForTable_102(this,'VIEW',740)" ButtonClass="mi-icon-more" 
                        Tag='{"type_id":"VIEW","view_id":740, "one_field":"COL_2", "table_name":"UT_083","owner_table_id":214}'
                        >
                        <MapItems>
                            <mi:MapItem SrcField="COL_3" TargetField="COL_2" Remark="材料名称" />
                            <mi:MapItem SrcField="COL_4" TargetField="COL_3" Remark="材料规格" />
                            <mi:MapItem SrcField="COL_6" TargetField="COL_4" Remark="宽度" />
                            <mi:MapItem SrcField="COL_8" TargetField="COL_5" Remark="密度" />
                            <mi:MapItem SrcField="COL_7" TargetField="COL_17" Remark="厚度" />
                            <mi:MapItem SrcField="ROW_IDENTITY_ID" TargetField="COL_52" Remark="产品ID" />
                            <mi:MapItem SrcField="COL_2" TargetField="COL_1" Remark="材料编号" />
                            <mi:MapItem SrcField="COL_5" TargetField="COL_9" Remark="计量单位" />
                            <mi:MapItem SrcField="COL_12" TargetField="COL_55" Remark="产品类型编号" />
                            <mi:MapItem SrcField="COL_11" TargetField="COL_37" Remark="仓库名称编号" />       
                            <mi:MapItem SrcField="COL_9" TargetField="COL_38" Remark="仓库名称" />    
                        </MapItems>
                    </mi:TriggerColumn>
                    <mi:BoundField HeaderText="规格" DataField="COL_3" Width="70" EditorMode="None" />

                    <mi:NumColumn HeaderText="密度" DataField="COL_5" Width="70" EditorMode="None" Format="0.##"   Visible="false" />
                    <mi:NumColumn HeaderText="厚度C" DataField="COL_17" Width="70" EditorMode="None" Format="0.##"  Visible="false"  />
                    <mi:NumColumn HeaderText="宽度MM" DataField="COL_4" Width="70" EditorMode="None" Format="0.##"  Visible="false" />
                    <mi:NumColumn HeaderText="板长M" DataField="COL_6" Width="70" Format="0.##"  Visible="false" />
                    <mi:NumColumn HeaderText="长度MM" DataField="COL_19" Width="70" Visible="false" Format="0.##"  />    
                    <mi:NumColumn HeaderText="排摸个数" DataField="COL_64" Width="70" EditorMode="None"  Visible="false" Format="0.##"   />
                    <mi:NumColumn HeaderText="生产总个数" DataField="COL_65"  Width="100" EditorMode="None" Visible="false" Format="0.##"/>       

                    <mi:NumColumn HeaderText="计划拉片张数" DataField="COL_7"  SummaryType="SUM" Width="90" Visible="false" Format="0.##"/>
                    <mi:NumColumn HeaderText="损耗张数" DataField="COL_11"  Width="70"  Visible="false" Format="0.##" />
                    <mi:NumColumn HeaderText="预拉板数" DataField="COL_72"  SummaryType="SUM" Width="70"  EditorMode="None" Format="0.##"  Visible="false" />
                    <mi:NumColumn HeaderText="需拉板数" DataField="COL_8"  SummaryType="SUM" Width="70"  EditorMode="None" Format="0.##"  Visible="false"  />
                    
                    <mi:NumColumn HeaderText="50KG生产个数" DataField="COL_18" Width="95"  EditorMode="None"  Visible="false"  />

                    <mi:SelectColumn HeaderText="计量单位" DataField="COL_9"  Width="70" TriggerMode="None">
                        <mi:ListItem Value="KG" Text="KG" />
                        <mi:ListItem Value="张" Text="张" />
                        <mi:ListItem Value="米" Text="米" />
                    </mi:SelectColumn>
                    <mi:NumColumn HeaderText="计划用量" DataField="COL_10" Width="70" Visible="false"  EditorMode="None"  />
                    
                    <mi:NumColumn HeaderText="实际用量" DataField="COL_12" Width="90"  EditorMode="None" />
                    
                    <mi:SelectColumn HeaderText="是否植绒" DataField="COL_13" TriggerMode="None" Width="70"   Visible="false" >
                        <mi:ListItem TextEx="--空--" />
                        <mi:ListItem Text="植绒" Value="植绒+" />
                    </mi:SelectColumn>
                    <mi:BoundField HeaderText="备注" DataField="COL_14" Width="200" />

                    <%-- 增加
                                    <mi:BoundField HeaderText="仓库编码" DataField=""  />
                <mi:BoundField HeaderText="仓库名称" DataField="" />--%>
                </Columns>
            </mi:Table>
        </mi:Panel>
        
        <mi:Panel runat="server" ID="panel2" Dock="Top" Scroll="None" Height="110">
            <mi:Toolbar runat="server" ID="Toolbar4" >
                <mi:ToolBarTitle Text="模具" />
                <mi:ToolBarButton Text="新建" OnClick="ser:StoreUT105.Insert();"  />
                <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT105.Refresh();"  />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT105.Delete();" />
            </mi:Toolbar>

            <mi:Table runat="server" ID="Table105" Dock="Full" StoreID="StoreUT105" Height="100" PagerVisible="false">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:TriggerColumn HeaderText="模具编号" DataField="COL_27" OnButtonClick="showDialgoForTable_102(this,'VIEW',711)" ButtonClass="mi-icon-more" Width="90"
                         OnMaping="mapping_103(this,'VIEW',711,'COL_3')"
                        Tag='{"type_id":"VIEW","view_id":711,"table_name":"UT_108","owner_table_id":236}' >

                        
                        <MapItems>
                            <mi:MapItem SrcField="COL_13" TargetField="COL_28" />
                            <mi:MapItem SrcField="COL_2" TargetField="COL_27" />
                            <mi:MapItem SrcField="COL_3" TargetField="COL_1" />
                            <mi:MapItem SrcField="COL_4" TargetField="COL_26" />
                            <mi:MapItem SrcField="COL_6" TargetField="COL_2" />
                            <mi:MapItem SrcField="COL_7" TargetField="COL_39" />
                            <mi:MapItem SrcField="COL_8" TargetField="COL_6" />
                        </MapItems>


                    </mi:TriggerColumn>
                    
                    <mi:BoundField HeaderText="模具名称" DataField="COL_1" Width="100" EditorMode="None" />
                    <mi:BoundField HeaderText="新旧" DataField="COL_26" Width="45"  />
                    <mi:BoundField HeaderText="数量" DataField="COL_9" Width="70" />
                    <mi:SelectColumn HeaderText="来源" DataField="COL_2" Width="100"  >
                        <mi:ListItem Text="自制" Value="自制" />
                        <mi:ListItem Text="委外加工" Value="委外加工" />
                    </mi:SelectColumn>
                    <mi:BoundField HeaderText="存放位置" DataField="COL_39" Width="90" />
                    <mi:BoundField HeaderText="工艺详细要求" DataField="COL_6" Width="150" />
                    <mi:BoundField HeaderText="备注" DataField="COL_10" Width="200" />


                    <%-- 增加
                                    <mi:BoundField HeaderText="仓库编码" DataField=""  />
                <mi:BoundField HeaderText="仓库名称" DataField="" />--%>
                </Columns>
            </mi:Table>
        </mi:Panel>
        
        <mi:Panel runat="server" ID="panel3" Dock="Top" Scroll="None" Height="110">
            <mi:Toolbar runat="server" ID="Toolbar5" >
                <mi:ToolBarTitle Text="生产机台" />
                <mi:ToolBarButton Text="新建" OnClick="ser:StoreUT313.Insert();"  />
                <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT313.Refresh();"  />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT313.Delete();" />
            </mi:Toolbar>

            <mi:Table runat="server" ID="Table106" Dock="Full" StoreID="StoreUT313" Height="100" PagerVisible="false">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField HeaderText="类型" DataField="COL_1" />
                    <mi:BoundField HeaderText="类型编码" DataField="COL_2" />

                    <mi:TriggerColumn HeaderText="机台编号" DataField="COL_3" OnButtonClick="showDialgoForTable_102(this,'VIEW',1489)" ButtonClass="mi-icon-more" 
                    OnMaping="mapping_103(this,'VIEW',1489,'COL_2')"
                    Tag='{"type_id":"VIEW","view_id":247,"table_name":"UT_107","owner_table_id":235}' Width="90" >

                    <MapItems>
                        <mi:MapItem SrcField="COL_1" TargetField="COL_1" Remark="类型" />
                        <mi:MapItem SrcField="COL_2" TargetField="COL_2" Remark="类型编码" />
                        <mi:MapItem SrcField="COL_3" TargetField="COL_3" Remark="机台编号" />
                        <mi:MapItem SrcField="COL_4" TargetField="COL_4" Remark="属性" />
                        <mi:MapItem SrcField="COL_5" TargetField="COL_5" Remark="工序名称" />
                        <mi:MapItem SrcField="COL_6" TargetField="COL_6" Remark="购入时间" />
                        <mi:MapItem SrcField="COL_7" TargetField="COL_7" Remark="描述" />
                        <mi:MapItem SrcField="COL_8" TargetField="COL_8" Remark="备注" />
                        <mi:MapItem SrcField="COL_9" TargetField="COL_9" Remark="数量" />
                        <mi:MapItem SrcField="COL_10" TargetField="COL_10" Remark="工艺说明" />
                        <mi:MapItem SrcField="COL_13" TargetField="COL_13" Remark="生产单号" />
                    </MapItems>

                </mi:TriggerColumn>

                    <mi:BoundField HeaderText="机台名称" DataField="COL_4" />
                    <mi:BoundField HeaderText="属性" DataField="COL_5" />
                    <mi:BoundField HeaderText="购入时间" DataField="COL_6" />
                    <mi:BoundField HeaderText="描述" DataField="COL_7" />
                    <mi:BoundField HeaderText="备注" DataField="COL_8" />
                    <mi:BoundField HeaderText="数量" DataField="COL_9" />
                    <mi:BoundField HeaderText="工艺说明" DataField="COL_10" />
                    <mi:NumColumn HeaderText="单头ID" DataField="COL_11" Visible="false"  />
                    <mi:NumColumn HeaderText="生产明细ID" DataField="COL_12" Visible="false"  />
                    <mi:BoundField HeaderText="生产单号" DataField="COL_13" Visible="false"  />
                </Columns>
            </mi:Table>
        </mi:Panel>

        
        <mi:Panel runat="server" ID="panel4" Dock="Top" Scroll="None" Height="200">
        <mi:Toolbar runat="server" ID="Toolbar3" >
            <mi:ToolBarTitle Text="工艺工序" />
            <mi:ToolBarButton Text="新建" OnClick="ser:StoreUT104.Insert();"  />
            <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT104.Refresh();"  />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT104.Delete();" />
        </mi:Toolbar>

        <mi:Table runat="server" ID="Table104" Dock="Full" StoreID="StoreUT104" PagerVisible="false" SummaryVisible="true" Height="160">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField HeaderText="工序顺序" DataField="COL_GXSX" Width="70" />
                <mi:TriggerColumn HeaderText="工序名称" DataField="COL_1" OnButtonClick="showDialgoForTable_102(this,'VIEW',247)" ButtonClass="mi-icon-more" 
                    OnMaping="mapping_103(this,'VIEW',247,'COL_2')"
                    Tag='{"type_id":"VIEW","view_id":247,"table_name":"UT_107","owner_table_id":235}' Width="90" >

                    <MapItems>
                        <mi:MapItem SrcField="COL_12" TargetField="COL_GXSX" Remark="工序顺序" />
                        <mi:MapItem SrcField="COL_1" TargetField="COL_27" Remark="工序编号" />
                        <mi:MapItem SrcField="COL_17" TargetField="COL_28" Remark="类型名称" />
                        <mi:MapItem SrcField="ROW_IDENTITY_ID" TargetField="COL_35" Remark="工序ID" />
                        <mi:MapItem SrcField="COL_2" TargetField="COL_1" Remark="工序名称" />
                        <mi:MapItem SrcField="COL_3" TargetField="COL_2" Remark="工序来源" />
                        <mi:MapItem SrcField="COL_4" TargetField="COL_3" Remark="部件名称" />
                        <mi:MapItem SrcField="COL_5" TargetField="COL_4" Remark="计量单位" />
                        <mi:MapItem SrcField="COL_6" TargetField="COL_5" Remark="单个用量" />
                        <mi:MapItem SrcField="COL_7" TargetField="COL_6" Remark="工序详细要求" />
                        <mi:MapItem SrcField="COL_9" TargetField="COL_13" Remark="是否需要上报产量" />
                        <mi:MapItem SrcField="COL_10" TargetField="COL_10" Remark="备注" />
                        <mi:MapItem SrcField="COL_19" TargetField="COL_43" Remark="智能运算" />
                        <mi:MapItem SrcField="COL_12" TargetField="COL_GXSX" Remark="工序编号" />
                    </MapItems>

                </mi:TriggerColumn>
                <mi:SelectColumn HeaderText="工序来源" DataField="COL_2" Width="100"  >
                        <mi:ListItem Text="自制" Value="自制" />
                        <mi:ListItem Text="委外加工" Value="委外加工" />
                    </mi:SelectColumn>
                <mi:BoundField HeaderText="部件名称" DataField="COL_3" Width="90" />
                <mi:SelectColumn HeaderText="计量单位" DataField="COL_4" Width="70">
                        <mi:ListItem Text="个" Value="个" />
                        <mi:ListItem Text="张" Value="张" />
                        <mi:ListItem Text="米" Value="米" />
                </mi:SelectColumn>
                <mi:NumColumn HeaderText="单个用量" DataField="COL_5" Width="70" Visible="false"  />
                <mi:BoundField HeaderText="工序详细要求" DataField="COL_6" Width="150" />
                <mi:NumColumn HeaderText="计划数量" DataField="COL_7" Width="70" Format="0.##" />
                <mi:NumColumn HeaderText="损耗" DataField="COL_8"  Width="70" Format="0.##" />
                <mi:CheckColumn HeaderText="上报产量" DataField="COL_13" Width="70" />
                <mi:NumColumn HeaderText="实需产量" DataField="COL_9" Width="70"  Format="0.##" />
                <mi:BoundField HeaderText="备注" DataField="COL_10" Width="200" />
            </Columns>
        </mi:Table>
        </mi:Panel>
        <mi:TabPanel runat="server" ID="TabPanel2" Region="Center" Plain="True">
            <mi:Tab runat="server" ID="tab3" Text="备注" Scroll="None">
                <mi:Textarea runat="server" ID="remarkTb" HideLabel="true" Width="400" />
            </mi:Tab>
        </mi:TabPanel>
    </mi:Panel>
</mi:Viewport>
</form>
<script type"text/javascript">




    function form_Closed102(e)
    {
        var me = this,
           record = me.record,
           editor = me.editor,
           editorMaps = editor.mapItems;

        if (e.result != 'ok') { return; }

        var row = e.row;
        var map = e.map;

        //log.begin("form_Closed102(...)");

        if (editorMaps && editorMaps.length) {

            map = editorMaps;

            console.log("映射...");
            console.log(map);

            record.beginEdit();

            for (var i = 0; i < map.length; i++) {
                var m = map[i];
                var v = row[m.srcField];


                record.set(m.targetField, v);

                if (editor.dataIndex == m.targetField) {
                    editor.setValue(v);
                }
            }

            record.endEdit();
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

        //log.end();
    }

    

    //导入研发计划 按钮点击事件
    //生产指令单 名称:导入研发计划  视图ID 1880  UT_370 映射规则：ID107 
    function showDialgoFor1880(owner) {

        var ps = {
            type_id: 'VIEW',
            view_id: 1880
        };

        var urlStr = $.format("/App/InfoGrid2/view/MoreView/DataImportDialog.aspx?type={0}&id={1}", ps.type_id, ps.view_id);

        var win = Mini2.createTop('Mini2.ui.Window', {
            mode: true,
            text: '选择',
            iframe: true,
            state: 'max',
            url: urlStr
        });

        win.show();

        win.formClosed(function (e) {

            if (e.result != 'ok') { return; }

            widget1.subMethod($('form:first'), {
                action: 'GoImportProductionPlan_107',
                actionPs: e.ids
            });

        });



    }


    //引入生产计划 按钮点击事件
    function showDialgoFor1505(owner) {

        var ps = {
            type_id : 'VIEW',
            view_id : 1505
        };

        var urlStr = $.format("/App/InfoGrid2/view/MoreView/DataImportDialog.aspx?type={0}&id={1}", ps.type_id, ps.view_id);

        var win = Mini2.createTop('Mini2.ui.Window', {
            mode: true,
            text: '选择',
            iframe: true,
            width: 800,
            height: 600,
            state: 'max',
            startPosition: 'center_screen',
            url: urlStr
        });

        win.show();

        win.formClosed(function (e) {

            if (e.result != 'ok') { return; }

            widget1.subMethod($('form:first'), {
                action: 'GoImportProductionPlan',
                actionPs: e.ids
            });

        });



    }


    function showDialgoForTable(owner) {

        var me = owner,
            tag = me.tag;
        var record = me.record;

//        if (!tag || tag == '') {
//            return;
//        }

        //var ps = eval('(' + tag + ')');

        var ps = {
            type_id : '',
            view_id : 809
        };


        var filter2 = '[{field:"UT_091.COL_1",logic:"=",value:"吸塑成品"}]';

        var base64 = window.BASE64.encoder(filter2);

        var urlStr = $.format("/App/InfoGrid2/view/MoreView/DataImportDialog.aspx?type={0}&id={1}&filter2={2}",
            ps.type_id, ps.view_id, base64);

        var win = Mini2.createTop('Mini2.ui.Window', {
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

        win.formClosed(function (e) {
            if (e.result != 'ok') { return; }

            widget1.subMethod($('form:first'), {
                action: 'GoInBill1',
                actionPs: e.ids
            });

        });
    }

    function showDialogForTable_103_click(owner) {



    }

    function RndNum(n) {
        var rnd = "";
        for (var i = 0; i < n; i++)
            rnd += Math.floor(Math.random() * 10);
        return rnd;
    }

    //根据输入的文字获取数据   回车按钮触发
    function mapping_103(owner, type_id, view_id, one_field) {

        var me = owner;
        var record = me.record;


        log.debug("---record.id = " + record.id);

        var id = owner.getValue();      
        var mapItems = owner.mapItems;  //映射字段的规则

        var base64 = window.BASE64.encoder("[{field:'" + one_field + "',logic:'like',value:'%" + id + "%'}]");//返回编码后的字符 

        base64 = base64.replace("+", "%2B");


        var urlStr = $.format("/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={0}&viewId={1}&filter2={2}",
            type_id, view_id, base64);


        $.get("/App/InfoGrid2/view/MoreView/OneMapAction.aspx", {
            type: type_id,
            viewId: view_id,
            id: id,
            one_field: '',
            filter2:base64,
            _rum: RndNum(8)
        },
        function (responseText, textStatus) {

            var model = eval('(' + responseText + ')');
            
            if (model.result != "ok") { return; }

            //如果只有一个就赋值到文本框中
            if (model.count == 1) {
                setMapValues(record, model.data, mapItems);
            }
            else
            {
                //找不到数据就把过滤条件清除掉
                if (model.count == 0)
                {
                    base64 = "";
                }

                var urlStr = $.format("/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={0}&viewId={1}&filter2={2}",
                    type_id, view_id,base64);

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

        //设置映射值
    function setMapValues(record, model, mapItems) {

        var me = this,
            i,
            item,
            srcValue;

        if (mapItems && model) {
            console.log(model);
            console.log(mapItems);
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


    function PrintExcel() {
        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '选择',
            iframe: true,
            width: 800,
            height: 600,
            state: 'max',
            startPosition: 'center_screen',
            url: "/App/InfoGrid2/View/PrintTemplate/SelectPrintByZLD.aspx"
        });


        
        win.show();

        win.formClosed(function (e) {
            if (e.result != 'ok') { return; }

            widget1.subMethod($('form:first'), {
                action: 'Print',
                actionPs: e.id
            });

        });
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

   

    function ShowGif(msg) {
        var urlStr = $.format("/App/InfoGrid2/View/Biz/JLSLBZ/XSSCZL/Waiting.aspx");

        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: 'TD智能分析',
            iframe: true,
            width: 722,
            height: 490,
            startPosition: 'center_screen',
            url: urlStr,
            maskOpacity:1
        });
        win.show();

        win.formClosed(function () {

            location.reload();

        });


    }



</script>
