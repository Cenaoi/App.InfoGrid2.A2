<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlowDefineList.ascx.cs" Inherits="App.InfoGrid2.View.OneFlowBuilder.FlowDefineList" %>


<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form action="" method="post">
    <mi:Store runat="server" ID="store1" Model="FLOW_DEF" IdField="FLOW_DEF_ID" DeleteRecycle="true"
         PageSize="20" SortText="FLOW_DEF_ID desc" DefaultInsertPos="First">
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
        <DeleteQuery>
            <mi:ControlParam Name="FLOW_DEF_ID" ControlID="table1" PropertyName="CheckedRows" />
        </DeleteQuery>
        <DeleteRecycleParams>
            <mi:Param Name="ROW_SID" DefaultValue="-3" />
            <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
        </DeleteRecycleParams>
    </mi:Store>
    <mi:Viewport runat="server" ID="viewport1">

        <mi:Toolbar runat="server" ID="toolbar1">
            <mi:ToolBarTitle ID="tableNameTB1" Text="流程定义" />

            <mi:ToolBarButton Text="新增" OnClick="ser:store1.Insert()" />
            <mi:ToolBarButton Text="保存" OnClick="ser:store1.SaveAll()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:store1.Delete()" />

            <mi:ToolBarButton Text="复制新的流程" Command="GoCopyDef" />

            
        </mi:Toolbar>

        <mi:Table runat="server" ID="table1" StoreID="store1" ReadOnly="true" Region="Center">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:ActionColumn Width="60">
                    <mi:ActionItem Command="GoEditFlow" DisplayMode="Text" Text="修改" />
                </mi:ActionColumn>                
                <mi:NumColumn HeaderText="ID" DataField="FLOW_DEF_ID" />
                <mi:BoundField HeaderText="流程编码" DataField="DEF_CODE" />
                <mi:BoundField HeaderText="流程名称" DataField="DEF_TEXT" />
                <mi:BoundField HeaderText="备注" DataField="REMARK" />
                <mi:BoundField HeaderText="版本号" DataField="V_VERSION" />
                <mi:BoundField HeaderText="作者编码" DataField="AUTHOR_CODE" />
                <mi:BoundField HeaderText="作者名称" DataField="AUTHOR_TEXT" />
                <mi:DateTimeColumn HeaderText="流程创建时间" DataField="BIZ_DATE_CREATE" />
                <mi:DateTimeColumn HeaderText="流程更新时间" DataField="BIZ_DATE_UPDATE" />

            </Columns>
        </mi:Table>

    </mi:Viewport>
</form>

