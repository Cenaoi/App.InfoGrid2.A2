<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlowDocListForNotcheck.ascx.cs" Inherits="App.InfoGrid2.View.OneFlowBuilder.FlowDocListForNotcheck" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form action="" method="post">
    <mi:Store runat="server" ID="store1" Model="FLOW_INST_PARTY" IdField="FLOW_INST_PARTY_ID" AutoFocus="false" 
         PageSize="20" SortText="FLOW_INST_PARTY_ID desc"  >
        <StringFields>EXTEND_DOC_TEXT,EXTEND_DOC_URL</StringFields>
    </mi:Store>
    <mi:Viewport runat="server" ID="viewport1">

        <mi:Toolbar runat="server" ID="toolbar1">
            <mi:ToolBarTitle ID="tableNameTB1" Text="流程定义" />
            
        </mi:Toolbar>

        <mi:Panel runat="server" ID="mainLeft" Width="160" Region="West">
            <div>未审批流程</div>
        </mi:Panel>

        <mi:Table runat="server" ID="table1" StoreID="store1" ReadOnly="true" Region="Center" JsonMode="Full" ColumnLines="false">
            <Columns>

                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:ActionColumn Width="60">
                    <mi:ActionItem Command="GoViewDoc" DisplayMode="Text" Text="查看" />
                </mi:ActionColumn>                
                <mi:SelectColumn HeaderText="" DataField="TAG_SID" TriggerMode="None" Width="50" ItemAlign="Center" StringItems="0=.;10=加急;" >
                </mi:SelectColumn>
                <mi:BoundField HeaderText="流程实例编码" DataField="INST_CODE" />
                <mi:BoundField HeaderText="流程实例编码" DataField="INST_TEXT" />
                <mi:BoundField HeaderText="文档类型" DataField="EXTEND_BILL_TYPE" />
                <mi:BoundField HeaderText="文档编码" DataField="EXTEND_BILL_CODE" />
                
                <mi:BoundField HeaderText="环节名称" DataField="DEF_NODE_TEXT" />

                <mi:BoundField HeaderText="启动时间" DataField="DATE_START" />
                <mi:BoundField HeaderText="启动流程的用户" DataField="START_USER_TEXT" />
                
                <mi:BoundField HeaderText="用户编码" DataField="P_USER_CODE" />
                <mi:BoundField HeaderText="用户名称" DataField="P_USER_TEXT" />
                <mi:DateTimeColumn HeaderText="创建时间" DataField="ROW_DATE_CREATE" />

            </Columns>
        </mi:Table>

    </mi:Viewport>
</form>
