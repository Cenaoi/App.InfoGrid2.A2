<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormWwjglb.ascx.cs" Inherits="App.InfoGrid2.View.Biz.JLSLBZ.WWJGFLD.FormWwjglb" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" method="post">

<!--	UT_104	吸塑指令单-工序工艺-->
<mi:Store runat="server" ID="StoreUT104" Model="UT_104" IdField="ROW_IDENTITY_ID">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:Param Name="COL_41" DefaultValue="是" Logic="=" />
        <mi:Param Name="BIZ_SID" DefaultValue="2" Logic="=" />
        <mi:Param Name="COL_32" DefaultValue=".,未完成" Logic="in" />
        <mi:Param Name="COL_2" DefaultValue="委外加工" Logic="=" />
        <mi:Param Name="COL_34" DefaultValue="0" Logic="&gt;" />
    </FilterParams>
</mi:Store>
<mi:Store runat="server" ID="StoreUT102" Model="UT_102" IdField="ROW_IDENTITY_ID">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:StoreCurrentParam ControlID="StoreUT104" Name="COL_17" PropertyName="COL_11" />
    </FilterParams>
</mi:Store>


<mi:Viewport runat="server" ID="viewport1" Padding="4" ItemMarginRight="6">
    <mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="10">
        <mi:Label runat="server" ID="headLab" Value="外发加工列表" HideLabel="true" Dock="Center" Mode="Transform" />
    </mi:Panel>
    <mi:Panel runat="server" ID="centerPanel" Region="North" Scroll="None" Height="300">
       <mi:SearchFormLayout runat="server" ID="searchForm1" StoreID="store1" Visible="false" >
           
                

                <mi:SearchButtonGroup runat="server" ID="searchBtnGroup"></mi:SearchButtonGroup>
                           
        </mi:SearchFormLayout>
         <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm1.toggle()" />
        </mi:Toolbar>
    <mi:Table runat="server" ID="table1" StoreID="StoreUT104" Region="North" Dock="Full" ReadOnly="true"   >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField HeaderText="生产单号" DataField="COL_11" Width="90" />
                
            
                <mi:BoundField HeaderText="销售订单号" DataField="COL_16" Visible="False" />
                <mi:BoundField HeaderText="上报生产数量" DataField="COL_17" Width="120"  Visible="False" />
                <mi:BoundField HeaderText="差额" DataField="COL_18" Visible="False" />
              
                
                

                <mi:BoundField HeaderText="类型名称" DataField="COL_28" Visible="false"/>
                <mi:BoundField HeaderText="类型编号" DataField="COL_29" Visible="false"/>
                <mi:BoundField HeaderText="成品类型" DataField="COL_30"  Width="70"  />
                <mi:BoundField HeaderText="成品类型编码" DataField="COL_31" Visible="false"/>
                <mi:BoundField HeaderText="工序完成状态" DataField="COL_32" Visible="false" />
                <mi:NumColumn HeaderText="工序顺序" DataField="COL_GXSX" Width="70"  />
                <mi:BoundField HeaderText="【所属用户编号】" DataField="BIZ_USER_CODE" Visible="false" />
          
                <mi:BoundField HeaderText="新旧" DataField="COL_26" Visible="false"  />
                <mi:BoundField HeaderText="工序编号" DataField="COL_27"  Width="70"  />
                 
                <mi:BoundField HeaderText="存放位置" DataField="COL_39" Visible="false" />
          
                <mi:BoundField HeaderText="工序名称" DataField="COL_1"  Width="90"  />
        
                <mi:BoundField HeaderText="部件名称" DataField="COL_3"  Width="70" />
                <mi:BoundField HeaderText="单位" DataField="COL_4"  Width="70" />
                <mi:BoundField HeaderText="单个用量" DataField="COL_5"  Visible="false" />
                <mi:BoundField HeaderText="数量" DataField="COL_9"  Width="70" />
                <mi:BoundField HeaderText="下单量" DataField="COL_33"  Width="90" />
                <mi:BoundField HeaderText="未下单数量" DataField="COL_34"  Width="90" />
              
           
                <mi:BoundField HeaderText="工序详细要求" DataField="COL_6"   Width="160" />
         
                <mi:BoundField HeaderText="实需产量" DataField="COL_9" Visible="false"/>
                <mi:BoundField HeaderText="备注" DataField="COL_10"  Width="150" />
                <mi:BoundField HeaderText="材料说明" DataField="COL_47"  Width="150" />
            </Columns>
        </mi:Table>
    </mi:Panel>
    <mi:Panel runat="server" ID="centerPane1" Region="Center" Scroll="None" Height="200"> 

         <mi:Table runat="server" ID="table2" StoreID="StoreUT102" Dock="Full" ReadOnly="true" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
           
                <mi:BoundField HeaderText="产品类型" DataField="COL_16"  Visible="false" />
                <mi:BoundField HeaderText="生产单号" DataField="COL_17"  Width="90" />
            
                <mi:BoundField HeaderText="生产模式" DataField="COL_20"  Width="90"  />
                <mi:BoundField HeaderText="时间" DataField="COL_21"  Width="90" />
                <mi:BoundField HeaderText="客户编码" DataField="COL_22"  Width="70" />
                <mi:BoundField HeaderText="客户名称" DataField="COL_1" Width="130"  />
                <mi:BoundField HeaderText="订单号" DataField="COL_2"  Width="90" />
                <mi:BoundField HeaderText="产品编号" DataField="COL_3"  Width="70" />
                <mi:BoundField HeaderText="产品名称" DataField="COL_4"  Width="150" />
                <mi:BoundField HeaderText="规格" DataField="COL_5"  Width="70" />
                <mi:BoundField HeaderText="计量单位" DataField="COL_6"  Width="70" />
                <mi:BoundField HeaderText="订单数量" DataField="COL_7"  Width="70" />
                <mi:BoundField HeaderText="排摸个数" DataField="COL_8"  Width="70" />
                <mi:BoundField HeaderText="生产数量" DataField="COL_9"  Width="70" />
                <mi:BoundField HeaderText="计划完工时间" DataField="COL_11"  Width="110" />
                <mi:BoundField HeaderText="备注" DataField="COL_25"  Visible="false" />
                
            </Columns>
        </mi:Table>

    </mi:Panel>
    <mi:WindowFooter ID="WindowFooter1" runat="server">
         
           
            <mi:Button runat="server" ID="SubmitBtn" Width="120" Height="26" Command="GoLast" Text="确定"  Dock="Center" />
         <mi:Button runat="server" ID="GoNextBtn" Width="80" Height="26" OnClick="ownerWindow.close();" Text="取消" Dock="Center" />
        </mi:WindowFooter>
</mi:Viewport>
</form>






