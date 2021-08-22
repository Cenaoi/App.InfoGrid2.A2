<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlowAutoNodeSetup.ascx.cs" Inherits="App.InfoGrid2.View.OneFlowBuilder.FlowAutoNodeSetup" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<style>

    .mi-textarea-code{

    }

    .mi-textarea-code .mi-form-text{
        font-family:'新宋体';
        font-size:12px;
    }


</style>

<form action="" method="post">


    <div runat="server" id="StoreSet">
        <mi:Store runat="server" ID="storeMain1" Model="FLOW_DEF_NODE" IdField="FLOW_DEF_NODE_ID" >
            <FilterParams>
                <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
                <mi:QueryStringParam Name="DEF_ID" QueryStringField="def_id" />
                <mi:QueryStringParam Name="FLOW_DEF_NODE_ID" QueryStringField="def_node_id" />
            </FilterParams>
        </mi:Store>

        <mi:Store runat="server" ID="store2" Model="FLOW_DEF_NODE_COPY_PARTY" IdField="FLOW_DEF_NODE_COPY_PARTY_ID" TranEnabled="false" PageSize="0" DeleteRecycle="true">
            <FilterParams>
                <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
                <mi:QueryStringParam Name="DEF_ID" QueryStringField="def_id" />
                <mi:QueryStringParam Name="NODE_ID" QueryStringField="def_node_id" />
            </FilterParams>

            <DeleteQuery>
                <mi:ControlParam Name="FLOW_DEF_NODE_COPY_PARTY_ID" ControlID="table2" PropertyName="CheckedRows" />
            </DeleteQuery>
            <DeleteRecycleParams>
                <mi:Param Name="ROW_SID" DefaultValue="-3" />
                <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
            </DeleteRecycleParams>

        </mi:Store>

    </div>

    <mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="6" Padding="0">



        <mi:TabPanel runat="server" ID="panel1" Padding="200" Region="Center" TabLeft="10" Dock="Full" PaddingTop="10" UI="win10" ButtonVisible="false">

            <mi:Tab runat="server" ID="TabPanel1" Text="自动活动" >

                <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="400" PaddingTop="10" StoreID="storeMain1"
                    ItemLabelAlign="Right" Region="North" FlowDirection="TopDown" AutoSize="true">
                    
                    <mi:TextBox runat="server" ID="textBox3" DataField="NODE_CODE" FieldLabel="节点编码" ReadOnly="true" />

                    <mi:TextBox runat="server" ID="textBox1" DataField="NODE_TEXT" FieldLabel="标签内容"   />

                    <mi:ComboBox runat="server" ID="comboBox1"   FieldLabel="样式名称" TriggerMode="None">
                        <mi:ListItem Value="default" Text="默认标签" />
                        <mi:ListItem Value="no-bg" Text="文本标签" />
                    </mi:ComboBox>

                    
                    <mi:ComboBox runat="server" ID="comboBox3"   FieldLabel="外部发送类型" TriggerMode="None">
                        <mi:ListItem Value="" Text="没有操作" />
                        <mi:ListItem Value="sent-email" Text="发送邮件" />
                    </mi:ComboBox>

                    <mi:TriggerBox runat="server" ID="textBox2"  FieldLabel="收件人"  ButtonType="More" />
                    
                    <mi:TextBox runat="server" ID="textBox4"  FieldLabel="标题"   />
                    <mi:Textarea runat="server" ID="textarea2" FieldLabel="内容" Height="100" />

                </mi:FormLayout>

            </mi:Tab>

            <mi:Tab runat="server" ID="tab2" Text="动作列表">
                
                <mi:Button runat="server" Text="校验代码" Command="GoValidCode" Dock="Top" />

                <mi:FormLayout runat="server" ID="FormLayout2" ItemWidth="300" PaddingTop="10" StoreID="storeMain1" Dock="Top"
                    ItemLabelAlign="Right" Region="North" FlowDirection="TopDown" AutoSize="true">

                    
                    <mi:ComboBox runat="server" ID="comboBox2"   FieldLabel="动作类型" DataField="ACTION_TYPE" TriggerMode="None">
                        <mi:ListItem Value="" Text="--禁止--" />
                        <mi:ListItem Value="DATA" Text="数据内容" />
                        <mi:ListItem Value="SCRIPT-CS" Text="C# 脚本" />
                    </mi:ComboBox>
                    <mi:Textarea runat="server" ID="textarea1" FieldLabel="代码" Height="400" MinWidth="700px" DataField="ACTION_SCRIPT" ExtraFieldBodyCls="mi-textarea-code" />

                </mi:FormLayout>


            </mi:Tab>

            <mi:Tab runat="server" ID="tab1" Text="退回动作列表">
                
                <mi:Button runat="server" Text="校验代码" Command="GoValidCodeForBack" Dock="Top" />

                <mi:FormLayout runat="server" ID="FormLayout3" ItemWidth="300" PaddingTop="10" StoreID="storeMain1" Dock="Top"
                    ItemLabelAlign="Right" Region="North" FlowDirection="TopDown" AutoSize="true">

                    
                    <mi:ComboBox runat="server" ID="comboBox4"   FieldLabel="动作类型" DataField="BACK_ACTION_TYPE" TriggerMode="None">
                        <mi:ListItem Value="" Text="--禁止--" />
                        <mi:ListItem Value="DATA" Text="数据内容" />
                        <mi:ListItem Value="SCRIPT-CS" Text="C# 脚本" />
                    </mi:ComboBox>
                    <mi:Textarea runat="server" ID="textarea3" FieldLabel="代码" Height="400" MinWidth="700px" DataField="BACK_ACTION_SCRIPT" ExtraFieldBodyCls="mi-textarea-code" />

                </mi:FormLayout>


            </mi:Tab>

            
            <mi:Tab runat="server" ID="Tab6" Text="抄送" Scroll="None">
                    
                <mi:FormLayout runat="server" ID="FormLayout6" ItemWidth="300" PaddingTop="10" StoreID="storeMain1"
                    ItemLabelAlign="Right" Dock="Top" Height="80" ItemLabelWidth="140" FlowDirection="TopDown" AutoSize="true">

                    <mi:CheckBox runat="server" ID="chaoshong1" DataField="IS_COPY_ENABLED" FieldLabel="抄送" TrueText="启动" FalseText="关闭" />
                    <mi:CheckBox runat="server" ID="CheckBox10" DataField="COPY_TO_START_PARTY_ENABLED" FieldLabel="抄送给发起人" TrueText="启动" FalseText="关闭" />

                </mi:FormLayout>
                <mi:Panel runat="server" ID="panel3" PaddingLeft="150" PaddingRight="80" Region="Center" Dock="Full" Scroll="None">
                    <mi:Toolbar ID="Toolbar2" runat="server" Dock="Center">
                        <mi:ToolBarTitle Text="抄送人员" />
                        <mi:ToolBarButton Text="新增" Command="GoShowSelectCopyUser" />
                        <mi:ToolBarButton Text="刷新" OnClick="ser:store2.Refresh()" />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?" OnClick="ser:store2.Delete()" />

                    </mi:Toolbar>
                    <mi:Table runat="server" ID="table2" StoreID="store2" Dock="Full" PagerVisible="false" ReadOnly="true">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:RowCheckColumn />
                            <mi:BoundField HeaderText="用户编码" DataField="P_USER_CODE" />
                            <mi:BoundField HeaderText="用户名称" DataField="P_USER_TEXT" />
                        </Columns>
                    </mi:Table>
                </mi:Panel>
            </mi:Tab>
        </mi:TabPanel>



        <mi:WindowFooter runat="server" ID="footer1">
            <mi:Button runat="server" ID="Button1" Text="取消" Width="80" Height="26" Dock="Center" OnClick="ownerWindow.close()" />
        </mi:WindowFooter>

    </mi:Viewport>
</form>

<script>


    $(document).ready(function () {

        ownerWindow.formClosing(function () {

            var tb = Mini2.find('textBox1');
            var rg = Mini2.find('comboBox1');
            
            var value = tb.getValue();
            var style_name = rg.getValue();

            ownerWindow.userData = {
                text: value,
                style_name: style_name
            };

        });

    });

</script>
