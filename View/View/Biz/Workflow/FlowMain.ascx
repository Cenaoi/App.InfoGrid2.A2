<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlowMain.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Workflow.FlowMain" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form method="post">


    <!--UT_101	吸塑指令单-单头-->
    <mi:Store runat="server" ID="StoreUT091" Model="UT_091" IdField="ROW_IDENTITY_ID" SortText="ROW_DATE_UPDATE desc">
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:Param Name="BIZ_SID" DefaultValue="2" Logic="BETWEEN 0 AND 98" />
            <mi:Param Name="COL_24" DefaultValue="109" Logic="&lt;&gt;" />
            <mi:Param Name="COL_24" DefaultValue="110" Logic="&lt;&gt;" />
            <mi:Param Name="COL_24" DefaultValue="111" Logic="&lt;&gt;" />
            <mi:Param Name="COL_24" DefaultValue="103" Logic="&lt;&gt;" />

             
           <%-- <mi:Param Name="COL_17" DefaultValue="0" Logic="&gt;"/>--%>
        </FilterParams>
    </mi:Store>


    <mi:Viewport runat="server" ID="viewport1" Padding="4" ItemMarginRight="6">

       

        <!--	UT_102	吸塑指令单-订单明细-->
        <mi:Panel runat="server" ID="UT_102Panel" Dock="Top" Scroll="None" AutoSize="true" Height="300">

             <mi:SearchFormLayout runat="server" ID="searchForm1" StoreID="StoreUT091" Visible="false" >
           
                <mi:TextBox runat="server" ID="TextBox6" DataField="COL_19" FieldLabel="订单号" DataLogic="like" />
                <mi:TextBox runat="server" ID="TextBox1" DataField="COL_35" FieldLabel="客户编号" DataLogic="like" />
                <mi:TextBox runat="server" ID="TextBox2" DataField="COL_36" FieldLabel="客户名称" DataLogic="like" />
                <mi:TextBox runat="server" ID="TextBox3" DataField="COL_2" FieldLabel="产品编号" DataLogic="like" />
                <mi:TextBox runat="server" ID="TextBox4" DataField="COL_3" FieldLabel="产品名称" DataLogic="like" />
                <mi:TextBox runat="server" ID="TextBox5" DataField="COL_4" FieldLabel="规格" DataLogic="like" />
                <mi:DateRangePicker ID="DateRangePicker1" runat="server" DataField="COL_10" FieldLabel="交货日期" ColSpan="2" />

                <mi:SearchButtonGroup runat="server" ID="searchBtnGroup"></mi:SearchButtonGroup>
                           
        </mi:SearchFormLayout>


            <mi:Toolbar runat="server" ID="toolbar2">
                <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm1.toggle()" />
   
            </mi:Toolbar>

            <mi:Table runat="server" ID="UT_102Table" Dock="Full" StoreID="StoreUT091" PagerVisible="true" ReadOnly="true">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField HeaderText="制单时间" DataField="ROW_DATE_CREATE"   width="100" />
                    <mi:NumColumn HeaderText="客户ID" DataField="COL_34"  Visible="False" />
                    <mi:BoundField HeaderText="客户编号" DataField="COL_35"   width="70" />
                    <mi:BoundField HeaderText="客户名称" DataField="COL_36"   width="150" />
                    <mi:NumColumn HeaderText="外键-单据头ID" DataField="COL_12" Visible="False" />
                    <mi:NumColumn HeaderText="业务状态" DataField="BIZ_SID"  Visible="False" />
                    <mi:BoundField HeaderText="销售订单号" DataField="COL_19"   width="100" />
                    <mi:BoundField HeaderText="产品类型编码" DataField="COL_24"  Visible="False" />
                    <mi:BoundField HeaderText="产品类型" DataField="COL_1"  width="70"  />
                    <mi:BoundField HeaderText="费用项目" DataField="COL_41"  Visible="False" />
                    <mi:BoundField HeaderText="审阅状态" DataField="COL_45"  Visible="False" />
                    <mi:BoundField HeaderText="审阅时间" DataField="COL_46"  Visible="False" />
                    <mi:BoundField HeaderText="审阅人" DataField="COL_47"  Visible="False" />
                    <mi:BoundField HeaderText="产品编号" DataField="COL_2"  width="90"  />
                    <mi:BoundField HeaderText="产品名称" DataField="COL_3"  width="170"  />
                    <mi:BoundField HeaderText="客户方编码" DataField="COL_25"  Visible="False" />
                    <mi:BoundField HeaderText="规格" DataField="COL_4"   width="70" />
                    <mi:BoundField HeaderText="单位" DataField="COL_5"   width="70" />
                    <mi:BoundField HeaderText="单价" DataField="COL_6"   width="70"  Visible="False" />
                    <mi:BoundField HeaderText="数量" DataField="COL_7"   width="70" />
                    <mi:BoundField HeaderText="金额" DataField="COL_8"   width="70"  Visible="False" />
                    <mi:BoundField HeaderText="当前库存数量" DataField="COL_9"  Visible="False" />
                    <mi:BoundField HeaderText="交货时间" DataField="COL_10"   width="100" />
                    <mi:BoundField HeaderText="备注" DataField="COL_11"  Visible="False" />
                    <mi:BoundField HeaderText="出货数量" DataField="COL_13"   width="70" />
                    <mi:BoundField HeaderText="出货金额" DataField="COL_14"  Visible="False" />
                    <mi:BoundField HeaderText="退货数量" DataField="COL_15" Visible="False"  />
                    <mi:BoundField HeaderText="退货金额" DataField="COL_16"  Visible="False" />
                    <mi:BoundField HeaderText="未出数量" DataField="COL_17"   width="70" />
                    <mi:BoundField HeaderText="未出金额" DataField="COL_18"  Visible="False" />
                    <mi:BoundField HeaderText="订单进度状态" DataField="COL_27"  Visible="False" />
                    <mi:BoundField HeaderText="已下生产指令生产数量" DataField="COL_20"  Visible="False" />
                    <mi:BoundField HeaderText="未下生产指令单数量" DataField="COL_21" Visible="False"  />
                    <mi:BoundField HeaderText="指令单下单状态" DataField="COL_28"  Visible="False" />
                    <mi:BoundField HeaderText="仓库编码" DataField="COL_22"  Visible="False" />
                    <mi:BoundField HeaderText="仓库名称" DataField="COL_23" Visible="False"  />
                    <mi:BoundField HeaderText="仓库类型编码" DataField="COL_31" Visible="False"  />
                    <mi:BoundField HeaderText="【所属结构编号】" DataField="BIZ_CATA_CODE"  Visible="False" />
                    <mi:BoundField HeaderText="【所属组织编号】" DataField="BIZ_ORG_CODE"  Visible="False" />
                    <mi:BoundField HeaderText="【所属用户编号】" DataField="BIZ_USER_CODE"  Visible="False" />
                    <mi:BoundField HeaderText="测试-误删" DataField="COL_32"  Visible="False" />
                    <mi:NumColumn HeaderText="产品ID" DataField="COL_29"  Visible="False" />
                    <mi:BoundField HeaderText="单据类型" DataField="COL_30"  Visible="False" />

                    <mi:BoundField HeaderText="当前时间" DataField="COL_37"  Visible="False" />
                    <mi:BoundField HeaderText="天数" DataField="COL_38"  Visible="False" />
                    <mi:BoundField HeaderText="提示状态" DataField="COL_39"  Visible="False" />
                    <mi:BoundField HeaderText="时间" DataField="COL_40"  Visible="False" />
                    <mi:CheckColumn HeaderText="HAS_CHILD" DataField="HAS_CHILD" Visible="False"  />
                    <mi:BoundField HeaderText="配方调整时间" DataField="COL_42"  Visible="False" />
                    <mi:BoundField HeaderText="调整状态" DataField="COL_43"  Visible="False" />
                    <mi:BoundField HeaderText="默认排摸个数" DataField="COL_44"  Visible="False" />

                </Columns>
            </mi:Table>
        </mi:Panel>

        <!--	UT_102	吸塑指令单-订单明细-->
        <mi:Panel runat="server" ID="Panel1" Dock="Full" Scroll="None" Region="Center">
            <iframe dock="full" region="center" id="iform1" frameborder="0"></iframe>
        </mi:Panel>
    </mi:Viewport>


</form>
