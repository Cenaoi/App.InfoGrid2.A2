<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormSPDZD_JB.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Financial.FormSPDZD_JB" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<style type="text/css">
    .page-head
    {
         font-size:26px;
         font-weight:bold;
    }
</style>

<!--财务统计-->
<form action="" method="post" >
<mi:Store runat="server" ID="store1" Model="FINANCIAL_STATISTICS" IdField="FINANCIAL_STATISTICS_ID" PageSize="20">
    <FilterParams>
        <mi:ServerParam Name="SESSION_ID" ServerField="SESSION_ID" />
    </FilterParams>
    <SummaryFields>
        <mi:SummaryField DataField="NUMBER_IN" SummaryType="SUM"></mi:SummaryField>
        <mi:SummaryField DataField="MONERY_IN" SummaryType="SUM"></mi:SummaryField>
        <mi:SummaryField DataField="NUMBER_OUT" SummaryType="SUM"></mi:SummaryField>
        <mi:SummaryField DataField="MONERY_OUT" SummaryType="SUM"></mi:SummaryField>

    </SummaryFields>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">
         <mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="10">
                <mi:Label runat="server" ID="headLab" Value="商品对账单" HideLabel="true" Dock="Center" Mode="Transform" />
         </mi:Panel>
         <mi:SearchFormLayout runat="server" ID="searchForm1" StoreID="store1"  Height="30">
           
                
                <mi:DateRangePicker ID="DateRangePicker1" runat="server"  FieldLabel="到期日期" ColSpan="2" />
             
                <mi:TextBox runat="server" ID="tbxID" FieldLabel="产品编码" />
                <mi:Button runat="server"  ID="tbnShow" Text="成品" OnClick="onBtnClick()" />
                <mi:Button runat="server"  ID="tbnShow1" Text="原材料" OnClick="onBtnClick1()"/>
                <mi:TextBox runat="server" ID="tbName" FieldLabel="产品名称" />
                
                <mi:TriggerBox runat="server" ID="tbWarehouse" FieldLabel="仓库" OnButtonClick="onBtnClick2(this)" ButtonClass="mi-icon-more"  />
                <mi:Button ID="Button1" runat="server" Text="查询" Command="btnSelect" Width="80" />
                <mi:Button ID="Button2" runat="server" Text="清空" Command="btnClear" Width="80"  />
                           
        </mi:SearchFormLayout>

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >

        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarTitle ID="tableNameTB1" Text="财务统计" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarButton Text="模板管理" Command="ManageTemplate" />
            <mi:ToolBarButton Text="打印" Command="btnPrint" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" SummaryVisible="true" >
            <Columns>
                <mi:RowNumberer />
                <mi:BoundField HeaderText="时间" DataField="DATE_TIME"  Width="100"  />
                <mi:BoundField HeaderText="摘要" DataField="ABSTRACT"  Width="150"  />
                <mi:NumColumn HeaderText="期初-数量" DataField="BEG_RECEIVABLES" Renderer="Mini2.DisplayRule.getRule('SET_COLOR')"  Width="80"  Format="0.##"  />
                <mi:NumColumn HeaderText="期初-金额" DataField="BEG_PRE_RECEIVABLES" Renderer="Mini2.DisplayRule.getRule('SET_COLOR')"  Width="80"  Format="0.##"  />
                <mi:NumColumn HeaderText="进仓-数量" DataField="NUMBER_IN" SummaryType="SUM"  Width="80"  Format="0.######"  />
                <mi:NumColumn HeaderText="进仓-金额" DataField="MONERY_IN" SummaryType="SUM"  Width="80"  Format="0.##"  />
                <mi:NumColumn HeaderText="出仓-数量" DataField="NUMBER_OUT" SummaryType="SUM"  Width="80"  Format="0.######"  />
                <mi:NumColumn HeaderText="出仓-金额" DataField="MONERY_OUT" SummaryType="SUM"  Width="80"  Format="0.##"  />
                <mi:NumColumn HeaderText="期末-数量" DataField="END_RECEIVABLES" Renderer="Mini2.DisplayRule.getRule('SET_COLOR')"  Width="80"  Format="0.######"  />
                <mi:NumColumn HeaderText="期末-金额" DataField="END_PRE_RECEIVABLES" Renderer="Mini2.DisplayRule.getRule('SET_COLOR')"  Width="80"  Format="0.##"  />
                <mi:BoundField HeaderText="仓库编码" DataField="WAREHOUSE_CODE"  Width="70"  />
                <mi:BoundField HeaderText="仓库名称" DataField="WAREHOUSE_NAME"  Width="70"  />
                <mi:BoundField HeaderText="备注" DataField="REMARKS"  Width="150"  />
                
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>
  <input type="hidden" id="HID" name="HID" />
  <input type="hidden" id="HWarehouse" name="HWarehouse" />
</form>

<%= GetDisplayRule() %>

<script>
    function onBtnClick() {

        var url = "/App/InfoGrid2/View/OneSearch/SelectPreview.aspx?viewId=1150";

        ////新的弹出窗口设置
        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '选择客户',
            iframe: true,
            state: 'max',
            startPosition: 'center_screen',
            url: url
        });

        win.show();

        win.formClosed(function (e) {
            if (e.result != 'ok') { return; }

            $('#HID').val(e.row.ROW_IDENTITY_ID);   ///这是产品ID
            $("#widget1_I_tbxID").val(e.row.COL_2);  ///产品编码

            $("#widget1_I_tbName").val(e.row.COL_3);



            //record.set('ACTION_TABLE_ITEMS', 'TABLE,' + e.sviewId);

        });

    }

    function onBtnClick1() {

        var url = "/App/InfoGrid2/View/OneSearch/SelectPreview.aspx?viewId=902";

        ////新的弹出窗口设置
        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '选择客户',
            iframe: true,
            state: 'max',
            startPosition: 'center_screen',
            url: url
        });

        win.show();

        win.formClosed(function (e) {
            if (e.result != 'ok') { return; }

            $('#HID').val(e.row.ROW_IDENTITY_ID);   ///这是产品ID
            $("#widget1_I_tbxID").val(e.row.COL_2);  ///产品编码

            //record.set('ACTION_TABLE_ITEMS', 'TABLE,' + e.sviewId);

        });

    }



    function onBtnClick2(ovwner) {

        var url = "/App/InfoGrid2/View/OneSearch/SelectPreview.aspx?viewId=1151";

        ////新的弹出窗口设置
        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '选择客户',
            iframe: true,
            state: 'max',
            startPosition: 'center_screen',
            url: url
        });

        win.show();

        win.formClosed(function (e) {
            if (e.result != 'ok') { return; }


            $('#HWarehouse').val(e.row.COL_1);     ///仓库编码
            ovwner.setValue(e.row.COL_2);          ///仓库名称

            //record.set('ACTION_TABLE_ITEMS', 'TABLE,' + e.sviewId);

        });

    }

   



</script>
