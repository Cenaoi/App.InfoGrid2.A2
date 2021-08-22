<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransformList.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.Transform_V1.TransformList" %>



<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<style type="text/css">
    .page-head
    {
         font-size:26px;
         font-weight:bold;
         
    }
</style>
  
<form action="" method="post">
<mi:Store runat="server" ID="store1" Model="UT_018" IdField="ROW_IDENTITY_ID" PageSize="20" SortText="ROW_DATE_CREATE desc,ROW_IDENTITY_ID desc">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:Param Name="BIZ_SID" DefaultValue="999" />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="ROW_IDENTITY_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>
</mi:Store>


<mi:Viewport runat="server" ID="viewport1" Main="true">

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        
            <mi:SearchFormLayout runat="server" ID="searchFrom" StoreID="store1" Visible="true">

                 <mi:DateRangePicker runat="server" ID="DateRangePicker1" FieldLabel="日期" DataField="ROW_DATE_CREATE" ColSpan="2" />

                <mi:TriggerBox runat="server" ID="TriggerBox1" FieldLabel="客户名称" DataField="CLIENT_TEXT"  ButtonType="More">

                </mi:TriggerBox>

                <mi:TextBox runat="server" ID="TextBox1" FieldLabel="客户编码" DataField="CLIENT_CODE" />

                 <mi:TriggerBox runat="server" ID="TriggerBox2" FieldLabel="原来客户名称" DataField="SRC_CLIENT_TEXT"   ButtonType="More">

                </mi:TriggerBox>

                  <mi:TextBox runat="server" ID="TextBox2" FieldLabel="原来客户编码" DataField="SRC_CLIENT_CODE" />

       


            <mi:SearchButtonGroup runat="server" ID="serachBtnGroup1"></mi:SearchButtonGroup>



        </mi:SearchFormLayout>
        
        <mi:Toolbar ID="Toolbar1" runat="server">
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" ReadOnly="true" JsonMode="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                
                <mi:SelectColumn  HeaderText="状态" DataField="BIZ_SID" TriggerMode="None" EditorMode="None" Width="80" >
                    <mi:ListItem Value="0" Text="草稿" />
                    <mi:ListItem Value="2" Text="审核中" />
                    <mi:ListItem Value="999" Text="审核完成" />
                </mi:SelectColumn>
                <mi:BoundField HeaderText="客户编码" DataField="CLIENT_CODE" />
                <mi:BoundField HeaderText="客户名称" DataField="CLIENT_TEXT" />
                <mi:BoundField HeaderText="原-客户编码" DataField="SRC_CLIENT_CODE" />
                <mi:BoundField HeaderText="原-客户名称" DataField="SRC_CLIENT_TEXT" />
                
                <mi:NumColumn HeaderText="数量" DataField="F_NUM" />

                <mi:DateTimeColumn HeaderText="转移时间" DataField="ROW_DATE_CREATE"/>

            </Columns>
        </mi:Table>
    </mi:Panel>


</mi:Viewport>


</form>