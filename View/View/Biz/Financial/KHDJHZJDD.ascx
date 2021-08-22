﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KHDJHZJDD.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Financial.KHDJHZJDD" %>


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
        <mi:SummaryField DataField="F_NUMBER" SummaryType="SUM"></mi:SummaryField>
        <mi:SummaryField DataField="RECEIVABLES" SummaryType="SUM"></mi:SummaryField>
        <mi:SummaryField DataField="PRE_RECEIVABLES" SummaryType="SUM"></mi:SummaryField>
    </SummaryFields>
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Main="true">
         <mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="10">
                <mi:Label runat="server" ID="headLab" Value="客户对账单" HideLabel="true" Dock="Center" Mode="Transform" />
         </mi:Panel>
         <mi:SearchFormLayout runat="server" ID="searchForm1" StoreID="store1"  Height="80">
               
                <mi:DateRangePicker ID="DateRangePicker1" runat="server"  FieldLabel="到期日期" ColSpan="2" />
                
                 <mi:ComboBox runat="server" ID="cbmCOL_30" FieldLabel="单据类型" CheckedMode="Multi" >
                    <mi:ListItem  Text="销售出货单" Value="销售出货单"/>
                    <mi:ListItem  Text="销售退货单" Value="销售退货单"/>
                    <mi:ListItem  Text="收款单" Value="收款单"/>
                    <mi:ListItem  Text="预收款单" Value="预收款单"/>
                    <mi:ListItem  Text="客户调整单" Value="客户调整单"/>
                </mi:ComboBox>  
                
                <mi:TriggerBox runat="server" FieldLabel="客户编号" OnButtonClick="onBtnClick(this)" ButtonClass="mi-icon-more" ID="tbID" />  
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
                <mi:BoundField HeaderText="单号" DataField="F_NO"  Width="90"  />

                <mi:BoundField HeaderText="摘要" DataField="ABSTRACT"  Width="200"  />
                <mi:NumColumn HeaderText="数量" DataField="F_NUMBER" SummaryType="SUM"  Width="80"  Format="0.##"  />
                <mi:NumColumn HeaderText="应收款" DataField="RECEIVABLES" SummaryType="SUM"  Width="80"  Format="0.##"  />
                <mi:NumColumn HeaderText="预收款" DataField="PRE_RECEIVABLES" SummaryType="SUM"  Width="80"  Format="0.##"  />

                <mi:BoundField HeaderText="备注" DataField="REMARKS"  Width="150"  />
                
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>
  <input type="hidden" id="HID" name="HID" />
  <input type="hidden" id="HWarehouse" name="HWarehouse" />
  <input type="hidden" id="HType" name="HType" />
</form>

<%= GetDisplayRule() %>

<script>
    function onBtnClick(ovwner) {

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


            $('#HID').val(e.row.ROW_IDENTITY_ID);
            ovwner.setValue(e.row.COL_2);

            //record.set('ACTION_TABLE_ITEMS', 'TABLE,' + e.sviewId);

        });

    }

</script>
    




