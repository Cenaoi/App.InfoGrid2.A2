<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectPrintInfo.ascx.cs" Inherits="App.InfoGrid2.View.PrintManager.SelectPrintInfo" %>




<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" id="form1" method="post">


<mi:Store runat="server" ID="store1" Model="BIZ_PRINT" IdField="BIZ_PRINT_ID"   >
    <InsertParams>
        <mi:ServerParam Name="LAST_LINE_TIME" ServerField="TIME_NOW" />
    </InsertParams>
    <DeleteQuery>
        <mi:ControlParam Name="BIZ_PRINT_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
</mi:Store>


<mi:Store runat="server" ID="store2" Model="BIZ_PRINT_CLIENT" IdField="BIZ_PRINT_CLIENT_ID"  >
</mi:Store>

<mi:Viewport runat="server" ID="viewport">
    
    <mi:Toolbar runat="server" ID="toolbar1">
        <mi:ToolBarButton Text="新建" OnClick="ser:store1.Insert()"  />
        <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
        <mi:ToolBarHr />
        <mi:ToolBarButton Text="删除" OnClick="ser:store1.Delete()" />
    </mi:Toolbar>

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="North" Scroll="None" Height="320" >
        <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="true" StoreID="store1" Dock="Full" JsonMode="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:SelectColumn DataField="BIZ_SID" HeaderText="状态" TriggerMode="None">
                    <mi:ListItem Value="0" Text="无效" />
                    <mi:ListItem Value="2" Text="激活" />
                </mi:SelectColumn>
                <mi:BoundField DataField="PRINT_CODE" HeaderText="打印机编码"  />
                <mi:BoundField DataField="PRINT_TEXT" HeaderText="打印机名称"   />
                <mi:SelectColumn DataField="IS_LINE" HeaderText="在线状态" EditorMode="None" ItemAlign="Center">
                    <mi:ListItem Value="False" Text="断线" />
                    <mi:ListItem Value="True" Text="在线" />
                </mi:SelectColumn>
                <mi:DateTimeColumn DataField="LAST_LINE_TIME" HeaderText="最后在线时间" EditorMode="None" />
                <mi:BoundField DataField="ROW_STRUCE_CODE" HeaderText="结构权限编码" Visible="false" />
            </Columns>
        </mi:Table>
    </mi:Panel>

     <mi:Panel runat="server" ID="Panel1" Dock="Full" Region="Center" Scroll="None"  >
        <mi:Table runat="server" ID="table2" RowLines="true" ColumnLines="true" StoreID="store2" 
            Dock="Full" CheckedMode="Single" 
            PagerVisible="false" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:SelectColumn DataField="BIZ_SID" HeaderText="状态" TriggerMode="None">
                    <mi:ListItem Value="0" Text="无效" />
                    <mi:ListItem Value="2" Text="激活" />
                </mi:SelectColumn>
                <mi:BoundField DataField="PCLIENT_GUID" HeaderText="客户端Guid"  EditorMode="None" />
                <mi:BoundField DataField="REMARK" HeaderText="备注" EditorMode="None" Width="300" />
                <mi:SelectColumn DataField="IS_LINE" HeaderText="" EditorMode="None" TriggerMode="None" ItemAlign="Center">
                    <mi:ListItem Value="False" Text="断线" />
                    <mi:ListItem Value="True" Text="在线" />
                </mi:SelectColumn>
                <mi:DateTimeColumn DataField="LAST_LINE_TIME" HeaderText="最后在线时间" />

            </Columns>
        </mi:Table>
    </mi:Panel>

</mi:Viewport>

</form>
