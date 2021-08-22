<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlowNodeStyleSetup.ascx.cs" Inherits="App.InfoGrid2.View.OneFlowBuilder.FlowNodeStyleSetup" %>


<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form action="" method="post">


    <div runat="server" id="StoreSet">
        <mi:Store runat="server" ID="storeMain1" Model="FLOW_DEF_NODE" IdField="FLOW_DEF_NODE_ID" TranEnabled="false">
            <FilterParams>
                <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
                <mi:QueryStringParam Name="DEF_ID" QueryStringField="def_id" />
                <mi:QueryStringParam Name="FLOW_DEF_NODE_ID" QueryStringField="def_node_id" />
            </FilterParams>
        </mi:Store>


        <mi:Store runat="server" ID="store1" Model="FLOW_DEF_NODE_PARTY" IdField="FLOW_DEF_NODE_PARTY_ID" TranEnabled="false" PageSize="0" DeleteRecycle="true">
            <FilterParams>
                <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
                <mi:QueryStringParam Name="DEF_ID" QueryStringField="def_id" />
                <mi:QueryStringParam Name="NODE_ID" QueryStringField="def_node_id" />
            </FilterParams>

            <DeleteQuery>
                <mi:ControlParam Name="FLOW_DEF_NODE_PARTY_ID" ControlID="table1" PropertyName="CheckedRows" />
            </DeleteQuery>
            <DeleteRecycleParams>
                <mi:Param Name="ROW_SID" DefaultValue="-3" />
                <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
            </DeleteRecycleParams>

        </mi:Store>


    </div>

    <mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="0" Padding="0">

        <mi:Panel runat="server" ID="panelMain1" Region="Center" Padding="4" Scroll="None">
            <mi:TabPanel runat="server" ID="panel1" Padding="200" Region="Center" Dock="Full" PaddingTop="10" UI="win10" ButtonVisible="false">
                <mi:Tab runat="server" ID="TabPanel1" Text="参与者" Scroll="None">
                    <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="300" PaddingTop="10" StoreID="storeMain1" ItemLabelWidth="150"
                        ItemLabelAlign="Right" Region="North" FlowDirection="TopDown" AutoSize="true">


                        <mi:TextBox runat="server" ID="textBox3" DataField="NODE_CODE" FieldLabel="节点编码" ReadOnly="true" />
                        <mi:TextBox runat="server" ID="textBox4" DataField="NODE_TEXT" FieldLabel="节点名称" />
                        <mi:ComboBox runat="server" ID="ComboBox1" DataField="P_MODE_ID" FieldLabel="人员参与" TriggerMode="None" StringItems="all=全部参与;part=部分参与">
                        </mi:ComboBox>
                        
                        <mi:CheckBox runat="server" ID="cb1" DataField="P_IS_CONSIGN" FieldLabel="会签" TrueText="是" FalseText="否" />

                        <mi:ComboBox runat="server" ID="comboBox2" DataField="P_CONSIGN_TYPE" FieldLabel="会签类型" TriggerMode="None" StringItems="parallel=并行会签;only=唯一会签"></mi:ComboBox>
                        <mi:ComboBox runat="server" ID="comboBox3" DataField="P_CONSIGN_BACK_TYPE" FieldLabel="会签时退回方式" TriggerMode="None" StringItems="at_once=立即退回"></mi:ComboBox>
                        
                        
                        <mi:CheckBox runat="server" ID="CheckBox1" DataField="P_ALLOW_OTHER_PARTY" FieldLabel="选择其他人员" TrueText="允许" FalseText="不允许"  />
                        <mi:CheckBox runat="server" ID="CheckBox2" DataField="P_AUTO_ORG" FieldLabel="自动对应部门人员" TrueText="启动" FalseText="关闭" />
                        


                    </mi:FormLayout>

                    <mi:Toolbar ID="Toolbar1" runat="server">
                        <mi:ToolBarTitle Text="活动参与者" />
                        <mi:ToolBarButton Text="新增" Command="GoShowSelectUser" />
                        <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?" OnClick="ser:store1.Delete()" />

                    </mi:Toolbar>
                    <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" PagerVisible="false">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:RowCheckColumn />
                            <mi:BoundField HeaderText="用户编码" DataField="P_USER_CODE" />
                            <mi:BoundField HeaderText="用户名称" DataField="P_USER_TEXT" />
                        </Columns>
                    </mi:Table>

                </mi:Tab>


                <mi:Tab runat="server" ID="Tab1" Text="邮件">

                    <mi:FormLayout runat="server" ID="FormLayout2" ItemWidth="370" PaddingTop="10" StoreID="storeMain1"
                        ItemLabelAlign="Right" Region="North" FlowDirection="TopDown" AutoSize="true" ItemLabelWidth="150">

                        <mi:ComboBox runat="server" ID="comboBox7" DataField="MSG_TYPE" LabelWidth="120" FieldLabel="消息类型" TriggerMode="None" StringItems="email=邮件;sms=短信">
                        </mi:ComboBox>


                        <mi:TextBox runat="server" ID="textBox5" DataField="MSG_NEW_TITLE" LabelWidth="120" FieldLabel="新任务到达时标题" />


                        <mi:Textarea runat="server" ID="Textarea1" LabelWidth="120" DataField="MSG_NEW_CONTENT" FieldLabel="新任务到达时内容" />

                        <mi:TextBox runat="server" ID="textBox6" LabelWidth="120" DataField="MSG_RESET_TITLE" FieldLabel="重新分派标题" />

                        <mi:Textarea runat="server" ID="Textarea2" LabelWidth="120" DataField="MSG_RESET_CONTENT" FieldLabel="重新分派内容" />
                        <mi:TextBox runat="server" ID="textBox8" LabelWidth="120" DataField="MSG_RECYCLE_TITLE" FieldLabel="收回时标题" />

                        <mi:Textarea runat="server" ID="Textarea3" LabelWidth="120" DataField="MSG_RECYCLE_CONTENT" FieldLabel="收回时内容" />

                        <mi:TextBox runat="server" ID="textBox9" LabelWidth="120" DataField="MSG_CHECK_TITLE" FieldLabel="审核完成时标题" />
                        <mi:Textarea runat="server" ID="Textarea4" LabelWidth="120" DataField="MSG_CHECK_CONTENT" FieldLabel="审核完成时内容" />

                    </mi:FormLayout>


                </mi:Tab>


                <mi:Tab runat="server" ID="Tab2" Text="按钮设置">


                    <mi:FormLayout runat="server" ID="HanderForm1" ItemWidth="300" PaddingTop="10" StoreID="storeMain1"
                        ItemLabelAlign="Right" Region="North" FlowDirection="TopDown" AutoSize="true" ItemLabelWidth="150">

                        <mi:CheckBox runat="server" ID="CheckBox3" DataField="BTN_SUBMIT_VISIBLE" FieldLabel="提交流程按钮" TrueText="显示" FalseText="隐藏"  />
                        <mi:CheckBox runat="server" ID="CheckBox4" DataField="BTN_TS_VISIBLE" FieldLabel="暂存流程按钮" TrueText="显示" FalseText="隐藏"  />
                        <mi:CheckBox runat="server" ID="CheckBox5" DataField="BTN_BACKFIRST_VISIBLE" FieldLabel="退回首环节按钮" TrueText="显示" FalseText="隐藏"  />
                        <mi:CheckBox runat="server" ID="CheckBox6" DataField="BTN_BACK_VISIBLE" FieldLabel="退回上一环节按钮" TrueText="显示" FalseText="隐藏"  />
                        <mi:CheckBox runat="server" ID="CheckBox7" DataField="BTN_RECYCLE_VISIBLE" FieldLabel="显示回收流程按钮" TrueText="显示" FalseText="隐藏"  />
                        
                    </mi:FormLayout>

                </mi:Tab>


                <mi:Tab runat="server" ID="Tab3" Text="意见设置">



                    <mi:FormLayout runat="server" ID="FormLayout3" ItemWidth="300" PaddingTop="10" StoreID="storeMain1"
                        ItemLabelAlign="Right" Region="North" FlowDirection="TopDown" AutoSize="true">

                        <mi:ComboBox runat="server" ID="comboBox13" DataField="IDEA_TYPE" FieldLabel="意见类型" TriggerMode="None" StringItems="可以填写办理">
                        </mi:ComboBox>

                        <mi:Textarea runat="server" ID="Textarea6" DataField="IEEA_CONTENT" FieldLabel="填写意见" />

                    </mi:FormLayout>

                </mi:Tab>

                <mi:Tab runat="server" ID="Tab4" Text="催办设置">



                    <mi:FormLayout runat="server" ID="FormLayout4" ItemWidth="400" ItemLabelWidth="140" PaddingTop="10" StoreID="storeMain1"
                        ItemLabelAlign="Right" Region="North" FlowDirection="TopDown" AutoSize="true">

                        
                        <mi:CheckBox runat="server" ID="CheckBox8" DataField="URGE_ENABLED" FieldLabel="催办设置" TrueText="启动" FalseText="关闭"  />

                        <mi:NumberBox runat="server" ID="NumberBox1" DataField="URGE_ACTION_HOUR" FieldLabel="活动持续周期(小时)" />
                        
                        <mi:CheckBox runat="server" ID="CheckBox9" DataField="URGE_AUTO_CHECK" FieldLabel="逾期后自动审核" TrueText="启动" FalseText="关闭"  />

                        <mi:NumberBox runat="server" ID="NumberBox2" DataField="URGE_TIEMSPAN" FieldLabel="每隔多少分钟催办一次" />


                        <mi:TextBox runat="server" ID="textBox1" DataField="URGE_TITLE" FieldLabel="催办标题" />


                        <mi:Textarea runat="server" ID="Textarea5" DataField="URGE_CONTENT" FieldLabel="催办内容" />

                    </mi:FormLayout>

                </mi:Tab>

                <mi:Tab runat="server" ID="Tab5" Text="其它">
                    <mi:FormLayout runat="server" ID="FormLayout5" ItemWidth="300" PaddingTop="10" StoreID="storeMain1"
                        ItemLabelAlign="Right" Region="North" Height="200" FlowDirection="TopDown" AutoSize="true">

                        <mi:Textarea runat="server" ID="Textarea7" DataField="REMARK" FieldLabel="备注" />

                    </mi:FormLayout>
                </mi:Tab>



            </mi:TabPanel>

        </mi:Panel>

        <mi:WindowFooter runat="server" ID="footer1">
            <mi:Button runat="server" ID="Button1" Text="取消" Width="80" Height="26" Dock="Center" OnClick="ownerWindow.close()" />
        </mi:WindowFooter>

    </mi:Viewport>
</form>

<script>

    $(document).ready(function () {

        ownerWindow.formClosing(function () {

            Mini2.getControl('');

            Mini2.onwerPage.controls[] = '';

            var tb = Mini2.getControl('textBox4');

            var value = tb.val();

            ownerWindow.NodeName = value;



        });

    });

</script>