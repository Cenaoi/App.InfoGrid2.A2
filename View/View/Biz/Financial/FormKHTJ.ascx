<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormKHTJ.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Financial.FormKHTJ" %>

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
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">
         <mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="10">
                <mi:Label runat="server" ID="headLab" Value="商品对账单" HideLabel="true" Dock="Center" Mode="Transform" />
         </mi:Panel>
         <mi:SearchFormLayout runat="server" ID="searchForm1" StoreID="store1"  Height="30">
                <mi:DateRangePicker ID="DateRangePicker1" runat="server"  FieldLabel="到期日期" ColSpan="2" />
                <mi:TriggerBox runat="server" ID="tbName" FieldLabel="客户名称" OnButtonClick="onBtnClick2(this)" ButtonClass="mi-icon-more"  />
                <mi:Button ID="Button1" runat="server" Text="查询" Command="btnSelect" Width="80" />
                <mi:Button ID="Button2" runat="server" Text="清空" Command="btnClear" Width="80"  />
                           
        </mi:SearchFormLayout>

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >

        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarTitle ID="tableNameTB1" Text="财务统计" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarButton Text="导出EXCEL" Command="ExportEXCEL" />
            <mi:ToolBarButton Text="模板管理" Command="ManageTemplate" />
            <mi:ToolBarButton Text="打印" Command="btnPrint" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" SummaryVisible="true" >
            <Columns>
                <mi:RowNumberer />
                <mi:BoundField HeaderText="编码" DataField="F_NO" />
                <mi:BoundField HeaderText="客户名称" DataField="REMARKS" />
                <mi:NumColumn HeaderText="数量" DataField="F_NUMBER" Format="0.##"  />
                <mi:NumColumn HeaderText="金额" DataField="F_PRICE" Format="0.##" />
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>
  <input type="hidden" id="HID" name="HID"  />
</form>


<script>

    function onBtnClick2(ovwner) {

        var url = "/App/InfoGrid2/View/OneSearch/SelectPreview.aspx?viewId=822";

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


            $('#HID').val(e.row.ROW_IDENTITY_ID);     ///仓库编码
            ovwner.setValue(e.row.COL_2);          ///仓库名称

            //record.set('ACTION_TABLE_ITEMS', 'TABLE,' + e.sviewId);

        });

    }

   



</script>
    