<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AreaLinkStepEdit1.ascx.cs" Inherits="App.InfoGrid2.View.OnePage.AreaLinkStepEdit1" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<!--域设置-->

<form action="" id="form1" method="post">
<mi:Store runat="server" ID="store1" Model="IG2_TABLE" 
    IdField="IG2_TABLE_ID" PageSize="20" 
    oncurrentchanged="store1_CurrentChanged">

</mi:Store>
<mi:Store runat="server" ID="store2" Model="IG2_TABLE_COL" IdField="IG2_TABLE_COL_ID" PageSize="20">
    
</mi:Store>
<mi:Viewport runat="server" ID="viewport1" Padding="10">

    <mi:FormLayout runat="server" ID="formLayout1" Padding="10" ItemWidth="200" Height="100" AutoSize="true"
        ItemLabelAlign="Right" FlowDirection="LeftToRight" Layout="Form" PaddingTop="4" Scroll="None">

        <mi:TextBox runat="server" ID="text1" FieldLabel="标签名称" Width="200" />  
        <mi:ComboBox runat="server" ID="comboBox1" FieldLabel="关联的字段" TriggerMode="None" Width="400" >
            
        </mi:ComboBox>
        <mi:CheckBox runat="server" ID="linkEnabledCB" FieldLabel="激活关联" />

        
        <mi:CheckBox runat="server" ID="tabShareDataCB" FieldLabel="复杂表-共享数据" />

    </mi:FormLayout>


    <mi:TabPanel runat="server" ID="tabPanel2"  Region="Center" Dock="Full" Height="140" Plain="true" ButtonVisible="false" UI="win10" TabLeft="8">
        <mi:Tab runat="server" ID="tab2" Text="链接目标工作表" Scroll="None">        
            <mi:Toolbar runat="server" ID="toolbar1">
                <mi:ToolBarButton Text="刷新" Command="OnStore1_Refresh" />
            </mi:Toolbar>
        
            <mi:Panel runat="server" ID="panelXX" Dock="Full" Scroll="None" >
                <mi:Table runat="server" ID="table1" StoreID="store1" PagerVisible="false" Dock="Left" 
                    CheckedMode="Single"
                    AutoRowCheck="true"
                    Height="120" Width="300" Sortable="false">
                    <Columns>
                        <mi:RowNumberer />
                        <mi:RowCheckColumn />
                        <mi:BoundField DataField="DISPLAY" HeaderText="目标表名" EditorMode="None" Width="200" />
                    </Columns>
                </mi:Table>

                <mi:Table runat="server" ID="table2" StoreID="store2"  ReadOnly="true" Dock="Full" Sortable="false" 
                    CheckedMode="Single" 
                    AutoRowCheck="true" 
                    PagerVisible="false" Width="800" >
                    <Columns>
                        <mi:RowNumberer />
                        <mi:RowCheckColumn />
                        <mi:BoundField DataField="F_NAME" HeaderText="目标字段名" Width="200" />
                        <mi:BoundField DataField="DISPLAY" HeaderText="显示" Width="200" />
                    </Columns>
                </mi:Table>
            </mi:Panel>
        </mi:Tab>
    </mi:TabPanel>


    <mi:WindowFooter runat="server" ID="buttomPanel" >
        <mi:Button runat="server" ID="Button1" Width="80" Height="26" Command="GoLast" Text="确定" Dock="Center" />
        <mi:Button runat="server" ID="Button2" Width="80" Height="26" OnClick="ownerWindow.close()" Text="取消" Dock="Center" />
    </mi:WindowFooter>

</mi:Viewport>


</form>