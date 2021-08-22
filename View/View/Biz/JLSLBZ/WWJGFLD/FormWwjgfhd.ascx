<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormWwjgfhd.ascx.cs" Inherits="App.InfoGrid2.View.Biz.JLSLBZ.WWJGFLD.FormWwjgfhd" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" method="post">


    <!--	UT_137	委外加工发料单-工序工艺明细 -->
<mi:Store runat="server" ID="StoreUT137" Model="UT_137" IdField="ROW_IDENTITY_ID">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" Remark="记录状态"  /> 
        <mi:Param Name="BIZ_SID" DefaultValue="2" Logic="=" Remark="业务状态"  />     
        <mi:Param Name="COL_45"  DefaultValue="0" Logic="&gt;"  Remark="在加工数量"  />
        <mi:QueryStringParam Name="COL_23" QueryStringField="COL_11"  />
    </FilterParams>
</mi:Store>



<mi:Viewport runat="server" ID="viewport1" Padding="4" ItemMarginRight="6">
    <mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="10">
        <mi:Label runat="server" ID="headLab" Value="外发加工发料单" HideLabel="true" Dock="Center" Mode="Transform" />
    </mi:Panel>
    <mi:Panel runat="server" ID="centerPanel" Region="Center" Scroll="None" Dock="Full">
       <mi:SearchFormLayout runat="server" ID="searchForm1" StoreID="store1" Visible="false" >
           
                

                <mi:SearchButtonGroup runat="server" ID="searchBtnGroup"></mi:SearchButtonGroup>
                           
        </mi:SearchFormLayout>
         <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm1.toggle()" />
        </mi:Toolbar>
    <mi:Table runat="server" ID="table1" StoreID="StoreUT137" Region="North" Dock="Full" ReadOnly="true"   >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField HeaderText="成品类型" DataField="COL_37" width="70" />
                <mi:BoundField HeaderText="成品编码" DataField="COL_38"  Visible="False" />
                <mi:BoundField HeaderText="外发单号" DataField="COL_22" Width="90"/>
                <mi:BoundField HeaderText="时间" DataField="COL_27" Width="100"/>
                <mi:NumColumn HeaderText="工序顺序" DataField="COL_GXSX" width="70"   Visible="False" />
                <mi:BoundField HeaderText="编号" DataField="COL_39"  width="70" />
                <mi:BoundField HeaderText="工序名称" DataField="COL_1"  width="90" />
                <mi:BoundField HeaderText="部件名称" DataField="COL_3"  width="70" />
                <mi:BoundField HeaderText="计量单位" DataField="COL_4"  width="70" />
                <mi:BoundField HeaderText="数量" DataField="COL_25"  width="70" />       

                <mi:BoundField HeaderText="在加工数量" DataField="COL_45"  width="80" />
                <mi:BoundField HeaderText="收货数量" DataField="COL_44"  width="70" />
                <mi:BoundField HeaderText="材料编码" DataField="COL_24"  width="130"   Visible="False" />
                <mi:BoundField HeaderText="完成状态" DataField="COL_46"  Visible="False" />
                <mi:BoundField HeaderText="新旧" DataField="COL_40"  Visible="False" />
                <mi:BoundField HeaderText="工序详细要求" DataField="COL_6"  width="150" />
                <mi:BoundField HeaderText="材料说明" DataField="COL_56" width="180"  />
                <mi:BoundField HeaderText="材料编码" DataField="COL_55"  width="130"   Visible="False" />
                <mi:BoundField HeaderText="[生产指令单号]" DataField="COL_11"  Visible="False" />
                <mi:BoundField HeaderText="联系人’" DataField="COL_30"  Visible="False" />
                <mi:BoundField HeaderText="加工商代码" DataField="COL_23" width="80"  />
                <mi:BoundField HeaderText="加工商名称" DataField="COL_24"  width="130" />
                
            </Columns>
        </mi:Table>
    </mi:Panel>
    <mi:WindowFooter ID="WindowFooter1" runat="server">
         
            <mi:Button runat="server" ID="SubmitBtn" Width="120" Height="26" Command="GoLast" Text="确定"  Dock="Center" />
         <mi:Button runat="server" ID="GoNextBtn" Width="80" Height="26" OnClick="ownerWindow.close();" Text="取消" Dock="Center" />
        </mi:WindowFooter>
</mi:Viewport>
</form>

