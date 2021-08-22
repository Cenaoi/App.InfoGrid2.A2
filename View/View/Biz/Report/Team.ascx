<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Team.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Report.Team" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<!-- 这是团队统计报表 小渔夫写的 -->


<form action="" id="form1" method="post">
    <mi:Store runat="server" ID="store1" Model="IG2_MAP" IdField="IG2_MAP_ID" PageSize="200" ReadOnly="true" AutoFocus="false" SortText="REP_ROW_IDENTITY asc">
    </mi:Store>
    <mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="10">
        <mi:Label runat="server" ID="headLab" Value="团队统计报表" HideLabel="true" Mode="Transform" BodyAlign="Center" />
    </mi:Panel>



    <mi:Viewport runat="server" ID="viewport1" MarginTop="40" Scroll="Auto">


        <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" MinHeight="600">
            <mi:SearchFormLayout runat="server" ID="searchForm" StoreID="store1" Region="North">
            </mi:SearchFormLayout>
            <mi:Toolbar runat="server" ID="toolbar1">
                <mi:ToolBarTitle Text="团队统计报表" />
            </mi:Toolbar>
            <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" PagerVisible="true" Region="Center" MinHeight="400" OnItemReseted="test">
                <Columns>
                    <mi:RowNumberer />
                    <mi:NumColumn DataField="ROW_IDENTITY_ID" HeaderText="主键"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_94" HeaderText="排序码"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_1" HeaderText="项目ID"></mi:NumColumn>
                    <mi:BoundField DataField="COL_45" HeaderText="线路类型"></mi:BoundField>
                    <mi:BoundField DataField="COL_7" HeaderText="区域"></mi:BoundField>
                    <mi:BoundField DataField="COL_8" HeaderText="航空公司"></mi:BoundField>
                    <mi:BoundField DataField="COL_9" HeaderText="团号"></mi:BoundField>
                    <mi:BoundField DataField="COL_11" HeaderText="行程"></mi:BoundField>
                    <mi:NumColumn DataField="COL_56" HeaderText="开团订单ID"></mi:NumColumn>
                    <mi:BoundField DataField="COL_10" HeaderText="订单号"></mi:BoundField>
                    <mi:BoundField DataField="COL_87" HeaderText="合同号"></mi:BoundField>
                    <mi:BoundField DataField="COL_107" HeaderText="客户行程"></mi:BoundField>
                    <mi:BoundField DataField="COL_14" HeaderText="客户"></mi:BoundField>

                    <mi:BoundField DataField="COL_15" HeaderText="销售员"></mi:BoundField>
                    <mi:NumColumn DataField="COL_16" HeaderText="数量"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_17" HeaderText="基本团费"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_18" HeaderText="签证费"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_19" HeaderText="小费"></mi:NumColumn>

                    <mi:NumColumn DataField="COL_20" HeaderText="现收签证费"></mi:NumColumn>

                    <mi:NumColumn DataField="COL_21" HeaderText="现收小费"></mi:NumColumn>

                    <mi:NumColumn DataField="COL_22" HeaderText="返佣"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_23" HeaderText="单房差"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_24" HeaderText="其他"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_25" HeaderText="订单金额"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_27" HeaderText="手续费"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_31" HeaderText="未收金额"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_91" HeaderText="收款记录ID"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_26" HeaderText="已收金额"></mi:NumColumn>
                    <mi:DateTimeColumn DataField="COL_28" HeaderText="收款日期"></mi:DateTimeColumn>
                    <mi:BoundField DataField="COL_29" HeaderText="收款方式"></mi:BoundField>
                    <mi:BoundField DataField="COL_44" HeaderText="备注"></mi:BoundField>

                    <mi:NumColumn DataField="COL_90" HeaderText="发票记录ID"></mi:NumColumn>

                    <mi:BoundField DataField="COL_12" HeaderText="发票号"></mi:BoundField>

                    <mi:DateTimeColumn DataField="COL_13" HeaderText="开票日期"></mi:DateTimeColumn>

                    <mi:NumColumn DataField="COL_78" HeaderText="开票金额"></mi:NumColumn>

                    <mi:NumColumn DataField="COL_106" HeaderText="未开票金额"></mi:NumColumn>

                    <mi:BoundField DataField="COL_79" HeaderText="开票备注"></mi:BoundField>

                    <mi:NumColumn DataField="COL_92" HeaderText="费用记录ID"></mi:NumColumn>


                    <mi:BoundField DataField="COL_32" HeaderText="成本内容"></mi:BoundField>

                    <mi:NumColumn DataField="COL_33" HeaderText="数量"></mi:NumColumn>

                    <mi:NumColumn DataField="COL_34" HeaderText="单价"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_30" HeaderText="手续费"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_35" HeaderText="成本小计"></mi:NumColumn>

                    <mi:NumColumn DataField="COL_93" HeaderText="付款记录ID"></mi:NumColumn>
                    <mi:BoundField DataField="COL_36" HeaderText="供应商"></mi:BoundField>

                    <mi:DateTimeColumn DataField="COL_37" HeaderText="付款日期"></mi:DateTimeColumn>

                    <mi:BoundField DataField="COL_38" HeaderText="付款方式"></mi:BoundField>

                    <mi:BoundField DataField="COL_124" HeaderText="付款类型"></mi:BoundField>

                    <mi:NumColumn DataField="COL_100" HeaderText="预付款"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_88" HeaderText="已付款"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_89" HeaderText="未付款"></mi:NumColumn>

                    <mi:BoundField DataField="COL_39" HeaderText="经办人"></mi:BoundField>

                    <mi:BoundField DataField="COL_40" HeaderText="备注"></mi:BoundField>

                    <mi:NumColumn DataField="COL_41" HeaderText="利润"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_42" HeaderText="销售收入"></mi:NumColumn>
                    <mi:NumColumn DataField="COL_43" HeaderText="成本支出"></mi:NumColumn>

                </Columns>
            </mi:Table>
        </mi:Panel>
    </mi:Viewport>
    <mi:Hidden runat="server" ID="bufferTableHid" Value="" />
</form>


<style type="text/css">

    .us-total-row .mi-grid-td{
         background-color:#399bff;
         color:white;
    }

</style>

<script>


    //索引
    var index = -1;


    function test(e) {


        if (e.rowIndex <= index) {
            return;
        }

        index = e.rowIndex;

        console.log(e.record.data.COL_40);

        if (e.record.data.COL_40 !== '合计') {

            return;
        }

        console.log(e, "11111111111111111111111111111");

        e.rowEl.addClass('us-total-row');

    }




</script>




