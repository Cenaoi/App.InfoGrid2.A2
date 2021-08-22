<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormScjhxqzx.ascx.cs" Inherits="App.InfoGrid2.View.Biz.JLSLBZ.Scjhxqzx2.FormScjhxqzx" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" method="post">

<!--UT_102 吸塑指令单-订单明细-->
<mi:Store runat="server" ID="StoreUT102" Model="UT_102" IdField="ROW_IDENTITY_ID" DeleteRecycle="true" 
    SortText="COL_3 ASC,COL_17 DESC,ROW_IDENTITY_ID ASC" PageSize="20">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" DbType="Int32" />
        <mi:Param Name="BIZ_SID" DefaultValue="0" Logic="&gt;=" DbType="Int32" />
        <mi:QueryStringParam Name="COL_1" QueryStringField="client_name" IgnoreEmpty="true" />
        <mi:Param Name="COL_19" DefaultValue="模板" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="ROW_IDENTITY_ID" ControlID="mainClientTable" PropertyName="CheckedRows" />
    </DeleteQuery>

    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" DbType="Int32" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>

    <SummaryFields>
        <mi:SummaryField DataField="COL_9" SummaryType="SUM"></mi:SummaryField>
        <mi:SummaryField DataField="COL_7" SummaryType="SUM"></mi:SummaryField>
        <mi:SummaryField DataField="COL_10" SummaryType="SUM"></mi:SummaryField>
    </SummaryFields>
</mi:Store>
<!--	UT_103	吸塑指令单-主材料	-->
<mi:Store runat="server" ID="StoreUT103" Model="UT_103" IdField="ROW_IDENTITY_ID">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
</mi:Store>
<!--	UT_104	吸塑指令单-工序工艺	-->
<mi:Store runat="server" ID="StoreUT104" Model="UT_104" IdField="ROW_IDENTITY_ID" SortText="COL_GXSX ASC">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:Param Name="COL_29" DefaultValue="999" />
<%--        <mi:Param Name="COL_2" DefaultValue="外发加工"  />--%>
    </FilterParams>
</mi:Store>


<!--UT_105  吸塑指令单-模具-->
<mi:Store runat="server" ID="StoreUT105" Model="UT_104" IdField="ROW_IDENTITY_ID">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:Param Name="COL_29" DefaultValue="888" />
    </FilterParams>
</mi:Store>


<!--UT_106  吸塑指令单-刀具-->
<mi:Store runat="server" ID="StoreUT106" Model="UT_104" IdField="ROW_IDENTITY_ID">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:Param Name="COL_29" DefaultValue="777" />
    </FilterParams>
</mi:Store>

<mi:Viewport runat="server" ID="viewport1" Padding="4" ItemMarginRight="6">

    <mi:SearchFormLayout runat="server" ID="searchFormLayout1" StoreID="StoreUT102" Region="North" Visible="false" >

       <%-- <mi:ComboBox runat="server" ID="textBox1" FieldLabel="单据类型" DataField="COL_19" >
            <mi:ListItem Value="模板" Text="模板" />
            <mi:ListItem Value="正常生产单" Text="正常生产单" />
        </mi:ComboBox>
        <mi:ComboBox runat="server" ID="comboBox1" FieldLabel="生产模式"  DataField="COL_20" >
            <mi:ListItem Value="整单外发" Text="整单外发" />
            <mi:ListItem Value="自制" Text="自制" />
        </mi:ComboBox>--%>
        <mi:TextBox runat="server" ID="textBox3" FieldLabel="产品类型" DataField="COL_16" DataLogic="like" />
        <mi:TextBox runat="server" ID="textBox12" FieldLabel="客户编码" DataField="COL_22" DataLogic="like" />
        <mi:TextBox runat="server" ID="textBox4" FieldLabel="客户名称" DataField="COL_1" />
<%--        <mi:TextBox runat="server" ID="textBox5" FieldLabel="电话" DataField="COL_19" />
        <mi:TextBox runat="server" ID="textBox6" FieldLabel="联系人" DataField="COL_19" />--%>
        <mi:TextBox runat="server" ID="textBox7" FieldLabel="销售订单号" DataField="COL_2" DataLogic="like" />
        <mi:TextBox runat="server" ID="textBox8" FieldLabel="生产单号" DataField="COL_17" DataLogic="like" />
        <mi:TextBox runat="server" ID="textBox9" FieldLabel="产品编号" DataField="COL_3" DataLogic="like" />
        <mi:TextBox runat="server" ID="textBox10" FieldLabel="产品名称" DataField="COL_4" DataLogic="like" />
        <mi:TextBox runat="server" ID="textBox11" FieldLabel="产品规格" DataField="COL_5" DataLogic="like" />

        <mi:DateRangePicker runat="server" ID="data1" FieldLabel="制单日期" DataField="COL_19" />

        <mi:SearchButtonGroup runat="server" ID="btnGroups"></mi:SearchButtonGroup>

    </mi:SearchFormLayout>


    <mi:Panel runat="server" ID="Panel1" Region="Center" Dock="Center" Scroll="None" Height="200" >
        <mi:Toolbar runat="server" ID="Toolbar2" >
            <mi:ToolBarTitle Text="订单" />
            <mi:ToolBarButton Text="修改" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:StoreUT102.Refresh();"  />

            <mi:ToolBarHr />

            <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchFormLayout1.toggle();$(window).resize();" />

            
            <mi:ToolBarButton Text="自动关联" Align="Right" Command="GoAutoLink"  />
<%--            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:StoreUT102.Delete();" />--%>
        </mi:Toolbar>

        <!--两个表关联显示  。	UT_101	吸塑指令单-单头 + 	UT_102	吸塑指令单-订单明细		 -->
        <mi:Table runat="server" ID="mainClientTable" Dock="Full" StoreID="StoreUT102"  PagerVisible="true" SummaryVisible="true" RowAltEnabled="false" ReadOnly="false"
             OnItemReseted="table_itemReset">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                
                <mi:ActionColumn Width="50" AutoHide="true">
                    <mi:ActionItem Text="修改" Icon="/res/icon/edit.png" Handler="RowEdit" />
                </mi:ActionColumn>

                <mi:SelectColumn HeaderText="状态" DataField="BIZ_SID" TriggerMode="None" Width="60" ItemAlign="Center" >
                    <mi:ListItem Value="0" Text="未提交" />
                    <mi:ListItem Value="2" Text="已审核" />
                    <mi:ListItem Value="99" Text="结案" />
                </mi:SelectColumn>
                                
                <mi:BoundField HeaderText="产品类型" DataField="COL_16" EditorMode="None" Width="70" />
                <mi:BoundField HeaderText="生产单号" DataField="COL_17"  EditorMode="None"  Width="100" />

                <mi:BoundField HeaderText="单据类型" DataField="COL_19" EditorMode="None" Width="70" />
                <mi:BoundField HeaderText="生产模式" DataField="COL_20" EditorMode="None" Width="70" Visible="false" />
                <mi:DateColumn HeaderText="时间" DataField="COL_21"  EditorMode="None"  Width="90" />
                <mi:BoundField HeaderText="客户名称" DataField="COL_1"  EditorMode="None"  Width="90" />
                <mi:BoundField HeaderText="订单号" DataField="COL_2"   EditorMode="None"  Width="70" />
                <mi:BoundField HeaderText="产品编号" DataField="COL_3"  EditorMode="None"  Width="70" />
                <mi:BoundField HeaderText="产品名称" DataField="COL_4"  EditorMode="None"  Width="120" />
                <mi:BoundField HeaderText="客户方编号" DataField="COL_27" EditorMode="None" Width="80" />
                <mi:BoundField HeaderText="规格" DataField="COL_5"  EditorMode="None"  Width="70" />
                <mi:BoundField HeaderText="计量单位" DataField="COL_6"  EditorMode="None"  Width="70" />
                <mi:NumColumn HeaderText="单价" DataField="COL_32" Width="70" Format="0.###"  />

             
                <mi:BoundField HeaderText="订单数量" DataField="COL_7"  EditorMode="None" SummaryType="SUM"  Width="70" />
                <mi:BoundField HeaderText="排摸个数" DataField="COL_8" EditorMode="None"   Width="70"  />
                <mi:BoundField HeaderText="生产数量" DataField="COL_9" SummaryType="SUM" EditorMode="None" Width="70"  />
                <mi:BoundField HeaderText="当前库存数量" DataField="COL_10" SummaryType="SUM" EditorMode="None" Width="90" Visible="false" />
                <mi:DateColumn HeaderText="计划完工时间" DataField="COL_11" EditorMode="None"  Width="90" Visible="false" />
                <mi:BoundField HeaderText="用料说明" DataField="COL_12" EditorMode="None" Width="180" />
                
                <mi:BoundField HeaderText="备注" DataField="COL_25" EditorMode="None" Width="180" />
                <mi:BoundField HeaderText="送货地址" DataField="COL_26" EditorMode="None" Width="180" Visible="false" />

            </Columns>
        </mi:Table>
    </mi:Panel>

    <mi:TabPanel runat="server" ID="tabPanel1" Region="South" Dock="Bottom" Plain="True" Height="300"  >
        <mi:Tab runat="server" ID="tab1" Text="材料信息"  FlowDirection="TopDown" ItemClass="mi-box-item" Layout="HBox" Scroll="Vertical" IsDelayRender="true">

            <mi:Toolbar runat="server" ID="Toolbar1"  IsDelayRender="true">
                <mi:ToolBarTitle Text="主材料" />
            </mi:Toolbar>

            <mi:Table runat="server" ID="Table1" Dock="Full" StoreID="StoreUT103" PagerVisible="false"  Height="100" IsDelayRender="true" >
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField HeaderText="材料编号" DataField="COL_1" EditorMode="None" Width="100" />
                    <mi:BoundField HeaderText="材料名称" DataField="COL_2" EditorMode="None" Width="150" />
                    <mi:BoundField HeaderText="材料规格" DataField="COL_3" EditorMode="None" Width="70" />
                    <mi:BoundField HeaderText="密度" DataField="COL_5" EditorMode="None" Width="45" />                        
                    <mi:BoundField HeaderText="厚度C" DataField="COL_17" EditorMode="None" Width="70" />
                    <mi:BoundField HeaderText="宽度MM" DataField="COL_4" EditorMode="None" Width="70"  />
                    <mi:BoundField HeaderText="板长M" DataField="COL_6" EditorMode="None" Width="70" />
                    <mi:BoundField HeaderText="长度MM" DataField="COL_19" EditorMode="None" Width="70"  />
                    <mi:BoundField HeaderText="排摸个数" DataField="COL_64" EditorMode="None" Width="70"   />                        
                    <mi:BoundField HeaderText="计划张数" DataField="COL_7"  SummaryType="SUM" EditorMode="None" Width="70" Visible="false" />
                    <mi:BoundField HeaderText="损耗张数" DataField="COL_11"  EditorMode="None" Width="70" Visible="false"  />
                    <mi:BoundField HeaderText="实需张数" DataField="COL_8"  SummaryType="SUM" EditorMode="None" Width="90" />
                    <mi:BoundField HeaderText="50KG生产个数" DataField="COL_18" Width="98" EditorMode="None" />
                    <mi:BoundField HeaderText="计量单位" DataField="COL_9"  EditorMode="None" Width="70"  />
                    <mi:BoundField HeaderText="计划用料" DataField="COL_10" EditorMode="None" Width="70" Visible="false" />

                    <mi:BoundField HeaderText="实需用料" DataField="COL_12"  EditorMode="None" Width="70"  />
                    <mi:BoundField HeaderText="是否植绒" DataField="COL_13" EditorMode="None" Width="70"  />
                    <mi:BoundField HeaderText="备注" DataField="COL_14" EditorMode="None" Width="180" />
                </Columns>
            </mi:Table>
        </mi:Tab>
        <mi:Tab runat="server" ID="tab2" Text="模具信息" Scroll="None" IsDelayRender="true">
        
            <mi:Toolbar runat="server" ID="Toolbar4"  IsDelayRender="true">
                <mi:ToolBarTitle Text="模具" />
            </mi:Toolbar>

            <mi:Table runat="server" ID="tableUT_105" Dock="Full" StoreID="StoreUT105" Height="100" PagerVisible="false"  IsDelayRender="true">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField HeaderText="模具编号" DataField="COL_1" EditorMode="None" Width="70" />
                    <mi:BoundField HeaderText="模具名称" DataField="COL_2" EditorMode="None" Width="100" />
                    <mi:BoundField HeaderText="新旧" DataField="COL_3" EditorMode="None" Width="45"  />
                    <mi:BoundField HeaderText="数量" DataField="COL_4" EditorMode="None" Width="70" />
                    <mi:BoundField HeaderText="来源" DataField="COL_5" EditorMode="None" Width="70" />
                    <mi:BoundField HeaderText="存放位置" DataField="COL_6" EditorMode="None" Width="80" />
                    <mi:BoundField HeaderText="工艺详细要求" DataField="COL_7" EditorMode="None" Width="100" />
                    <mi:BoundField HeaderText="备注" DataField="COL_8" EditorMode="None" Width="180"  />
                </Columns>
            </mi:Table>

        </mi:Tab>
        <mi:Tab runat="server" ID="tab4" Text="刀具信息" Scroll="None"  IsDelayRender="true">

            <mi:Toolbar runat="server" ID="Toolbar5"  IsDelayRender="true">
                <mi:ToolBarTitle Text="刀具" />
            </mi:Toolbar>

            <mi:Table runat="server" ID="table3" Dock="Full" StoreID="StoreUT106" Height="100" PagerVisible="false"  IsDelayRender="true">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField HeaderText="刀具编号" DataField="COL_1" EditorMode="None" Width="70" />
                    <mi:BoundField HeaderText="刀具名称" DataField="COL_2" EditorMode="None" Width="100" />
                    <mi:BoundField HeaderText="新旧" DataField="COL_3" EditorMode="None"  Width="45" />
                    <mi:BoundField HeaderText="数量" DataField="COL_4" EditorMode="None"  Width="70" />
                    <mi:BoundField HeaderText="来源" DataField="COL_5" EditorMode="None"  Width="70" />
                    <mi:BoundField HeaderText="存放位置" DataField="COL_6" EditorMode="None"  Width="70" />
                    <mi:BoundField HeaderText="工艺详细要求" DataField="COL_7"  EditorMode="None" Width="100" />
                    <mi:BoundField HeaderText="备注" DataField="COL_8"  EditorMode="None" Width="180"  />
                </Columns>
            </mi:Table>
        </mi:Tab>
        <mi:Tab runat="server" ID="tab5" Text="工艺信息" IsDelayRender="true">
             
            <mi:Toolbar runat="server" ID="Toolbar3"  IsDelayRender="true">
                <mi:ToolBarTitle Text="工艺工序" />
            </mi:Toolbar>

            <mi:Table runat="server" ID="Table2" Dock="Full" StoreID="StoreUT104" PagerVisible="false" SummaryVisible="true" Height="260" IsDelayRender="true">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField HeaderText="工序顺序" DataField="COL_GXSX" EditorMode="None" Width="70"  />
                    <mi:BoundField HeaderText="工序名称" DataField="COL_1" EditorMode="None" Width="100" />
                    <mi:BoundField HeaderText="工序来源" DataField="COL_2" EditorMode="None" Width="70"  />
                    <mi:BoundField HeaderText="部件名称" DataField="COL_3" EditorMode="None" Width="70"   />
                    <mi:BoundField HeaderText="计量单位" DataField="COL_4" EditorMode="None" Width="70" />
                    <mi:BoundField HeaderText="单个用量" DataField="COL_5" EditorMode="None" Width="70" Visible="false" />
                    <mi:BoundField HeaderText="工序详细要求" DataField="COL_6" EditorMode="None" Width="70" />
                    <mi:BoundField HeaderText="计划数量" DataField="COL_7"   EditorMode="None" Width="70" Visible="false" />
                    <mi:BoundField HeaderText="损耗" DataField="COL_8" EditorMode="None" Width="130"  />
                    <mi:BoundField HeaderText="实需产量" DataField="COL_9" EditorMode="None" Width="70"  />
                    <mi:BoundField HeaderText="备注" DataField="COL_10" EditorMode="None" Width="180" />
                </Columns>
            </mi:Table>
        </mi:Tab>
        <mi:Tab runat="server" ID="tab6" Text="备注信息">
            
            <mi:Toolbar runat="server" ID="toobarXXX">
                <mi:ToolBarTitle Text="备注" />
            </mi:Toolbar>
            <mi:Textarea runat="server" ID="RemarkTB" Width="500" FieldLabel="备注" Dock="Top" HideLabel="true" />

        </mi:Tab>
    </mi:TabPanel>
</mi:Viewport>
</form>
<style type="text/css">

.mi-grid-row-group-1 td.mi-grid-cell-row-numberer
{
    background-color: #F9D700;
}


.mi-grid-row-group-1 td
{
    background-color: #d8d8d8;
}


.mi-grid-row-group-2 td.mi-grid-cell-row-numberer
{
    background-color:#A8FF24;
}


.mi-grid-row-group-2 td
{
    
}

</style>
<script type="text/javascript">

    //最后单号
    var m_LastBillNo = '';

    var m_LastStartAlt = false;

    function table_itemReset(e) {

        var recd = e.record;

        var billNo = recd.get('COL_17');    //生产单号
        var rowEl = e.rowEl;

        if (e.rowIndex == 0) {
            m_LastBillNo = billNo;
            m_LastStartAlt = false;
        }

        if (m_LastBillNo != billNo) {
            m_LastStartAlt = !m_LastStartAlt;
            m_LastBillNo = billNo;
        }
        
        if (m_LastStartAlt) {
            rowEl.toggleClass('mi-grid-row-group-1', m_LastStartAlt);

        } else
        {
            rowEl.addClass('mi-grid-row-group-2');
        }

        
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
            type_id: '',
            view_id: 233
        };

        var urlStr = $.format("/App/InfoGrid2/view/MoreView/DataImportDialog.aspx?type={0}&id={1}",
            ps.type_id, ps.view_id);

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

        win.formClosed(function (e) {
            if (e.result != 'ok') { return; }

            widget1.subMethod($('form:first'), {
                action: 'GoInBill1',
                actionPs: e.ids
            });

        });

    }


    function RowEdit(view, cell, recordIndex, cellIndex, e, record, row) {

        var tableId = record.getId();

        var headId = record.get('COL_13');  //表头ID

        var billType = record.get('COL_5'); //吸塑、胶盒

        
        EcView.show("/App/InfoGrid2/View/Biz/JLSLBZ/Scjhxqzx/EditBill.aspx?id=" + headId, "生产指令单");


    }
</script>
