<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlowNoteSetup.ascx.cs" Inherits="App.InfoGrid2.View.OneFlowBuilder.FlowNoteSetup" %>
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
    </div>

    <mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="0" Padding="0">


        <mi:Panel runat="server" ID="panelMain1" Region="Center" Padding="4" Scroll="None">


            <mi:TabPanel runat="server" ID="panel1" Padding="200" Region="Center" TabLeft="10" Dock="Full" PaddingTop="10" UI="win10" ButtonVisible="false">

                <mi:Tab runat="server" ID="TabPanel1" Text="线段设置">

                    <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="300" PaddingTop="10" StoreID="storeMain1"
                        ItemLabelAlign="Right" Region="North" FlowDirection="TopDown" AutoSize="true">


                        <mi:TextBox runat="server" ID="textBox1" DataField="NODE_TEXT" FieldLabel="标签内容"   />

                        <mi:ComboBox runat="server" ID="comboBox1"   DataField="STYLE_NAME" FieldLabel="样式名称" TriggerMode="None">
                            <mi:ListItem Value="default" Text="默认标签" />
                            <mi:ListItem Value="no-bg" Text="文本标签" />
                        </mi:ComboBox>


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
            var me = this;

            var tb = Mini2.find('textBox1');
            var rg = Mini2.find('comboBox1');
            
            var value = tb.getValue();
            var style_name = rg.getValue();
            
            me.userData = {
                text: value,
                style_name: style_name
            };

        });

    });

</script>
