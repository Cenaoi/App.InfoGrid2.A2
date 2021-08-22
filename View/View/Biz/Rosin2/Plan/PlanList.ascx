<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanList.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin2.Plan.PlanList" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<% if (false)
   { %>
<link href="/App_Themes/Blue/common.css" rel="stylesheet" type="text/css" />
<link href="/App_Themes/Vista/table.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>
<link href="/Core/Scripts/ui-lightness/jquery-ui-1.8.6.custom.css" rel="stylesheet"
    type="text/css" />
<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
<script src="/Core/Scripts/validate/jquery.validate-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>
<script src="/Core/Scripts/Mini/_CodeHelp.js" type="text/javascript"></script>
<% } %>

<style type="text/css">
    .page-head
    {
         font-size:26px;
         font-weight:bold;         
    }
</style>
  
<form action="" method="post">

<mi:Store runat="server" ID="store1" Model="UT_015_PLAN" IdField="ROW_IDENTITY_ID" PageSize="20" LockedField="BIZ_SID" 
    SortText="DOC_CREATE_DATE DESC,ROW_DATE_CREATE DESC"
    DeleteRecycle="true"  ReadOnly="true" >
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" DbType="Int32" />
        <mi:Param Name="BIZ_SID" DefaultValue="0" Logic="&gt;=" DbType="Int32" />
        <mi:QueryStringParam Name="IO_TAG" QueryStringField="IO_TAG" ValidRequired="true" ValidValueEnum="I;O" />
    </FilterParams>
    <InsertParams>
        <mi:QueryStringParam Name="IO_TAG" QueryStringField="IO_TAG" />
        <mi:ServerParam Name="DOC_CREATE_DATE" ServerField="TIME_NOW" />
        <mi:Param Name="PROD_NUM_UNIT" DefaultValue="桶" />
    </InsertParams>
    <DeleteQuery>
        <mi:Param Name="ROW_SID" DefaultValue="0" />
        <mi:Param Name="BIZ_SID" DefaultValue="0" />
        <mi:ControlParam Name="ROW_IDENTITY_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="BIZ_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>
</mi:Store>

<mi:Panel runat="server" ID="HeadPanel" Height="40" Scroll="None" PaddingTop="0" >
    <mi:Label runat="server" ID="headLab" Value="单号" HideLabel="true" Mode="Transform" BodyAlign="Center" />
</mi:Panel>

<mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="40">

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        
        <mi:SearchFormLayout runat="server" ID="searchFrom" StoreID="store1">
            <mi:TextBox runat="server" ID="textBox1" DataField="BILL_NO" FieldLabel="单号" DataLogic="like" />
            <mi:TextBox runat="server" ID="textBox2" DataField="CCK_NO" FieldLabel="储存卡号" DataLogic="like" />

            <mi:TriggerBox runat="server" ID="triggerBox1" DataField="CLIENT_TEXT" FieldLabel="客户名称" LabelAlign="Right" ButtonType="More" Required="true" DataLogic="like" >
  
            </mi:TriggerBox>

            <mi:TextBox runat="server" ID="textBox4" DataField="COL_1" FieldLabel="开单人" DataLogic="like" />
            <mi:TextBox runat="server" ID="textBox5" DataField="COL_2" FieldLabel="审核人" DataLogic="like" />
            <mi:SearchButtonGroup runat="server" ID="serachBtnGroup1"></mi:SearchButtonGroup>
        </mi:SearchFormLayout>
        
        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarButton Text="新建" OnClick="ser:store1.Insert()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?" OnClick="ser:store1.Delete()"/>
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="导入" Command="GoFromExcel" />
            <mi:ToolBarButton Text="【导入模板下载】" Href="/app/导入计划模板.xls" HrefTarget="_blank" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="提交" Command="GoChangeBizSID_0_2" />
            <mi:ToolBarButton Text="审核" ID="tbb_create_ut_001" Command="GoCreateInBill" Tooltip="直接产生入库单" />

            <mi:ToolBarHr />
            <mi:ToolBarButton Text="回收站" Command="GoRecycled"  />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" JsonMode="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:ActionColumn HeaderText="功能" Width="60">
                    <mi:ActionItem Text="编辑" Command="GoEdit" DisplayMode="Text"  />
                </mi:ActionColumn>
                <mi:SelectColumn HeaderText="状态" DataField="BIZ_SID" Width="70" TriggerMode="None" StringItems="-3=作废;0=草稿; 2=已提交; 4=已审核;" >
                </mi:SelectColumn>
                <mi:BoundField HeaderText="单号" DataField="BILL_NO" />
                <mi:BoundField HeaderText="客户编码" DataField="CLIENT_CODE"  />
                <mi:BoundField HeaderText="客户名称" DataField="CLIENT_TEXT" />
<%--                <mi:NumColumn HeaderText="合计" DataField="ITEM_TOTAL" ItemAlign="Right"  Width="80" NotDisplayValue="0" />--%>
                <mi:NumColumn HeaderText="数量" DataField="PROD_NUM" ItemAlign="Right" Width="80"  NotDisplayValue="0" />
                <mi:BoundField HeaderText="单位" DataField="PROD_NUM_UNIT" Width="60" />
<%--                <mi:NumColumn HeaderText="重量" DataField="WEIGHT_TOTAL" ItemAlign="Right" Width="80"  NotDisplayValue="0" />
                                
                <mi:BoundField HeaderText="到货方式" DataField="DH_TYPE" Width="70" />--%>
<%--                <mi:BoundField HeaderText="储存卡号" DataField="CCK_NO" />
                <mi:BoundField HeaderText="到货性质" DataField="DH_XZ" Width="70"  />

                <mi:BoundField HeaderText="打包" DataField="IS_PACKER"  Width="70"  />
                <mi:BoundField HeaderText="集装箱尺寸" DataField="CONT_SIZE" />
                <mi:BoundField HeaderText="装卸线" DataField="ZXX"  Width="70"  />--%>
                <mi:BoundField HeaderText="备注" DataField="REMARK" />
                <mi:BoundField HeaderText="开单人" DataField="DOC_CREATE_USER_TEXT" Width="80"  />
                <mi:DateTimeColumn HeaderText="开单时间" DataField="DOC_CREATE_DATE" />
                <mi:BoundField HeaderText="审核人" DataField="DOC_CHECK_USER_TEXT"  Width="80" />
<%--                <mi:BoundField HeaderText="更新序号" DataField="COL_4" />
                <mi:BoundField HeaderText="更新人" DataField="COL_5" />
                <mi:BoundField HeaderText="更新时间" DataField="COL_6" />--%>
<%--                <mi:BoundField HeaderText="抽检状态" DataField="CJ_SID" />
                <mi:BoundField HeaderText="抽样送检委托书" DataField="CJ_WTS" />
                <mi:BoundField HeaderText="抽检单号" DataField="CJ_CODE" />--%>
            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>


    <mi:Hidden runat="server" ID="H_ID" />

</form>