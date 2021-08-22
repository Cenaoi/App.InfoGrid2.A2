<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormDcgqd.ascx.cs" Inherits="App.InfoGrid2.View.Biz.JLSLBZ.WWJGFLD.FormDcgqd" %>


<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" method="post">

<!--		UT_103	吸塑指令单-主材料-->
<mi:Store runat="server" ID="StoreUT216" Model="UT_216" IdField="ROW_IDENTITY_ID">
    <FilterParams>
        
        <mi:Param Name="COL_47" DefaultValue="是" Logic="=" Remark="是否采购" />


        <mi:Param Name="COL_46" DefaultValue="0" Logic="&gt;" Remark="未下单数量" />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:Param Name="BIZ_SID" DefaultValue="2" Logic="=" />

    </FilterParams>
</mi:Store>
<mi:Store runat="server" ID="StoreUT102" Model="UT_102" IdField="ROW_IDENTITY_ID">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:StoreCurrentParam ControlID="StoreUT216" Name="COL_17" PropertyName="COL_16" />
    </FilterParams>
</mi:Store>


<mi:Viewport runat="server" ID="viewport1" Padding="4" ItemMarginRight="6">
    <mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="10">
        <mi:Label runat="server" ID="headLab" Value="待采购清单" HideLabel="true" Dock="Center" Mode="Transform" />
    </mi:Panel>
    <mi:Panel runat="server" ID="centerPanel" Region="North" Scroll="None" Height="300">
       <mi:SearchFormLayout runat="server" ID="searchForm1" StoreID="StoreUT216" Visible="false" >
           
                <mi:TextBox runat="server" ID="textBox8" FieldLabel="生产单号" DataField="COL_1" DataLogic="like" />
                <mi:TextBox runat="server" ID="textBox9" FieldLabel="生产模式" DataField="COL_3" DataLogic="like" />
                <mi:TextBox runat="server" ID="textBox10" FieldLabel="是否生产" DataField="COL_10" DataLogic="like" />

                <mi:SearchButtonGroup runat="server" ID="searchBtnGroup"></mi:SearchButtonGroup>
                           
        </mi:SearchFormLayout>
         <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm1.toggle()" />
        </mi:Toolbar>
    <mi:Table runat="server" ID="table1" StoreID="StoreUT216" Region="North" Dock="Full" ReadOnly="true"   >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField HeaderText="成品类型" DataField="COL_43"  Width="70" />
                <mi:BoundField HeaderText="成品类型编码" DataField="COL_44"  Width="90"  Visible="False" />
                <mi:BoundField HeaderText="生产单号" DataField="COL_16" Width="90" Visible="False" />
                <mi:BoundField HeaderText="材料编号" DataField="COL_1" Width="90" />
                <mi:BoundField HeaderText="材料名称" DataField="COL_2" Width="180" />
                <mi:BoundField HeaderText="是否植绒" DataField="COL_13"  Width="70" />
                <mi:BoundField HeaderText="材料规格" DataField="COL_3"  Width="70" />
                <mi:BoundField HeaderText="500KG生产个数" DataField="COL_18" Visible="False" />
                <mi:BoundField HeaderText="基本用料-计量单位" DataField="COL_9"  Visible="False" />
                <mi:BoundField HeaderText="计划量" DataField="COL_12" Width="70" />
                <mi:BoundField HeaderText="采购数量" DataField="COL_50"  Width="70" />
                <mi:BoundField HeaderText="下单数量" DataField="COL_45" Width="70" />
                <mi:BoundField HeaderText="未下单数量" DataField="COL_46"  Width="90"  />
                <mi:BoundField HeaderText="库存数量" DataField="COL_48" Visible="False"  />
                <mi:BoundField HeaderText="可用数量" DataField="COL_49" Visible="False" />
                <mi:BoundField HeaderText="厚度" DataField="COL_17"  Width="45" />
                <mi:BoundField HeaderText="密度" DataField="COL_5"   Width="45" />
                <mi:BoundField HeaderText="宽度" DataField="COL_4"  Width="45"  />
                <mi:BoundField HeaderText="长度" DataField="COL_19"   Width="45" />
                <mi:BoundField HeaderText="板长" DataField="COL_6"  Width="45" />
                <mi:BoundField HeaderText="备注" DataField="COL_14"   Width="120" />
                <mi:BoundField HeaderText="是否采购" DataField="COL_47" Visible="False" />

            </Columns>
        </mi:Table>
    </mi:Panel>
    <mi:Panel runat="server" ID="centerPane1" Region="Center" Scroll="None" Height="200"> 

         <mi:Table runat="server" ID="table2" StoreID="StoreUT102" Dock="Full" ReadOnly="true" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
           
                <mi:BoundField HeaderText="产品类型" DataField="COL_16"  Width="70" />
                <mi:BoundField HeaderText="生产单号" DataField="COL_17"  Width="90" />
            
                <mi:BoundField HeaderText="生产模式" DataField="COL_20"  Width="70" />
                <mi:BoundField HeaderText="时间" DataField="COL_21"  Width="100" />
                <mi:BoundField HeaderText="客户编码" DataField="COL_22"  Width="70" />
                <mi:BoundField HeaderText="客户名称" DataField="COL_1"  Width="120" />
                <mi:BoundField HeaderText="订单号" DataField="COL_2"  Width="80" />
                <mi:BoundField HeaderText="产品编号" DataField="COL_3"  Width="70" />
                <mi:BoundField HeaderText="产品名称" DataField="COL_4"  Width="180" />
                <mi:BoundField HeaderText="规格" DataField="COL_5"  Width="70" />
                <mi:BoundField HeaderText="计量单位" DataField="COL_6"  Width="70" />
                <mi:BoundField HeaderText="订单数量" DataField="COL_7"  Width="70" />
                <mi:BoundField HeaderText="排摸个数" DataField="COL_8"  Width="70" />
                <mi:BoundField HeaderText="生产数量" DataField="COL_9"  Width="70" />
                <mi:BoundField HeaderText="计划完工时间" DataField="COL_11"  Width="100" />
                <mi:BoundField HeaderText="备注" DataField="COL_25"  Width="150"  Visible="False" />
                
            </Columns>
        </mi:Table>

    </mi:Panel>
    <mi:WindowFooter ID="WindowFooter1" runat="server">
         
           
            <mi:Button runat="server" ID="SubmitBtn" Width="120" Height="26" Command="GoLast" Text="确定"  Dock="Center" />
         <mi:Button runat="server" ID="GoNextBtn" Width="80" Height="26" OnClick="ownerWindow.close();" Text="取消" Dock="Center" />
        </mi:WindowFooter>
</mi:Viewport>
</form>


