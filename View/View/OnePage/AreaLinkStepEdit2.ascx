<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AreaLinkStepEdit2.ascx.cs" Inherits="App.InfoGrid2.View.OnePage.AreaLinkStepEdit2" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<!--域设置-->

<form action="" id="form1" method="post">


<mi:Store runat="server" ID="store2" Model="join_item" IdField="field" PageSize="0" AutoSave="false" JsonMode="Full" >
    <StringFields>field,field_text,join_field,join_field_text</StringFields>
</mi:Store>

<mi:Viewport runat="server" ID="viewport1" Padding="10">

    <mi:FormLayout runat="server" ID="formLayout1" Padding="10" ItemWidth="600" Height="130" AutoSize="true"
        ItemLabelAlign="Right" FlowDirection="TopDown" Layout="Form" PaddingTop="4" Scroll="None">
        
        <mi:TextBox runat="server" ID="tabName1" FieldLabel="标签名称" />

        <mi:CheckBox runat="server" ID="linkEnabledCB" FieldLabel="激活关联" />

        <mi:RadioGroup runat="server" ID="joinVersion1" FieldLabel="版本">
            <mi:ListItem Value="1" Text="1.0 版本" />
            <mi:ListItem Value="2" Text="2.0 版本" />
        </mi:RadioGroup>

        <mi:ComboBox runat="server" ID="join_table1" FieldLabel="关联表" DataField="join_table"
            ValueField="TABLE_NAME" DisplayField="DISPLAY" TriggerMode="None" >

        </mi:ComboBox>

    </mi:FormLayout>


    <mi:TabPanel runat="server" ID="tabPanel2"  Region="Center" Dock="Full" Height="140" Plain="true" 
        ButtonVisible="false" UI="win10" TabLeft="8">

        <mi:Tab runat="server" ID="tab2" Text="链接目标工作表" Scroll="None">        
            <mi:Toolbar runat="server" ID="toolbar1">
                <mi:ToolBarButton Text="新建" Command="OnStore1_Add" />
                <mi:ToolBarButton Text="刷新" Command="OnStore1_Refresh" />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="删除" Command="OnStore1_Delete" />
            </mi:Toolbar>
            
            <mi:Table runat="server" ID="table1" StoreID="store2" PagerVisible="false" Dock="Full" 
               
                AutoRowCheck="true"
                Height="120" Width="300" Sortable="false">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:SelectColumn DataField="field" HeaderText="外键字段名" DropDownWidth="350"  Width="200" DisplayField="value" >
                        <MapItems>
                            <mi:MapItem SrcField="value" TargetField="field" />
                            <mi:MapItem SrcField="text" TargetField="field_text" />
                        </MapItems>
                    </mi:SelectColumn>
                    <mi:BoundField DataField="field_text" HeaderText="外键字段-描述" />
                    <mi:BoundField HeaderText="" Width="60" EditorMode="None" />
                    <mi:SelectColumn DataField="join_field" HeaderText="关联表-字段名" DropDownWidth="300" Width="200"  DisplayField="value" ID="joinFieldX"  >
                        <MapItems>
                            <mi:MapItem SrcField="value" TargetField="join_field" />
                            <mi:MapItem SrcField="text" TargetField="join_field_text" />
                        </MapItems>
                    </mi:SelectColumn>
                    <mi:BoundField DataField="join_field_text" HeaderText="关联字段-描述" />
                </Columns>
            </mi:Table>

        </mi:Tab>
        <mi:Tab runat="server" Text="其它">
            <mi:Button runat="server" ID="repair1" Text="修复 v1 版转为 v2 版格式" Command="GoRepairToV2" />
        </mi:Tab>

    </mi:TabPanel>
    
    <mi:WindowFooter runat="server" ID="buttomPanel" >
        <mi:Button runat="server" ID="Button1" Width="80" Height="26" Command="GoLast" Text="确定" Dock="Center" />
        <mi:Button runat="server" ID="Button2" Width="80" Height="26" OnClick="ownerWindow.close()" Text="取消" Dock="Center" />
    </mi:WindowFooter>

</mi:Viewport>


</form>