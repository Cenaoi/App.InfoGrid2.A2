<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DwgSetupLine.ascx.cs" Inherits="App.InfoGrid2.View.MoreActionBuilder.DwgSetupLine" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form action="" method="post">


    <div runat="server" id="StoreSet">
        <mi:Store runat="server" ID="storeMain1" Model="AC3_DWG_LINE" IdField="AC3_DWG_LINE_ID" TranEnabled="false">
            <FilterParams>
                <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
                <mi:QueryStringParam Name="FK_DWG_CODE" QueryStringField="dwg_code" />
                <mi:QueryStringParam Name="PK_LINE_CODE" QueryStringField="line_code" />
            </FilterParams>
        </mi:Store>
    </div>

    <mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="0" Padding="0">


        <mi:Panel runat="server" ID="panelMain1" Region="Center" Padding="4" Scroll="None">


            <mi:TabPanel runat="server" ID="panel1" Padding="200" Region="Center" Dock="Full" PaddingTop="10" UI="win10" ButtonVisible="false">

                <mi:Tab runat="server" ID="TabPanel1" Text="路由">

                    <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="300" PaddingTop="10" StoreID="storeMain1"
                        ItemLabelAlign="Right" Region="North" FlowDirection="TopDown" AutoSize="true">

                        
                        <mi:TextBox runat="server" ID="TextBox7" DataField="LINE_IDENTIFIER" FieldLabel="ID" />

                        <mi:TextBox runat="server" ID="textBox3" DataField="LINE_TEXT" FieldLabel="线段名称" />
                        <mi:Label runat="server" ID="label2" DataField="PK_LINE_CODE" FieldLabel="线段编码"  />

<%--                        <mi:ComboBox runat="server" ID="ComboBox1" DataField="ROUTES_TYPE" FieldLabel="路由类型" TriggerMode="None" StringItems="only=唯一选择;cond=条件选择">
                        </mi:ComboBox>--%>
                        <mi:Textarea runat="server" ID="Textarea7" DataField="REMARK" FieldLabel="备注" />

                        <hr class="mi-newline" />

                        <mi:TextBox runat="server" ID="textBox4" DataField="FROM_NODE_CODE" FieldLabel="起始节点编码" ReadOnly="true" />
                        <mi:TextBox runat="server" ID="textBox5" DataField="FROM_NODE_TEXT" FieldLabel="起始节点名称" ReadOnly="true" />

                        <mi:TextBox runat="server" ID="textBox2" DataField="TO_NODE_CODE" FieldLabel="结束节点编码" ReadOnly="true" />
                        <mi:TextBox runat="server" ID="textBox6" DataField="TO_NODE_TEXT" FieldLabel="结束节点名称" ReadOnly="true" />


                    </mi:FormLayout>

                </mi:Tab>



            </mi:TabPanel>

        </mi:Panel>


        <mi:WindowFooter runat="server" ID="footer1">
            <mi:Button runat="server" ID="Button1" Text="取消" Width="80" Height="26" Dock="Center" OnClick="ownerClose()" />
        </mi:WindowFooter>

    </mi:Viewport>
</form>


<script>
    "use strict";

    function ownerClose() {

        //console.debug('ownerWindow', ownerWindow);

        if (ownerWindow) {
            ownerWindow.close();
        }

    }

    $(document).ready(function () {

        //Mini2.setTimer(function () {

        //    if (!window.ownerWindow) {
        //        return;
        //    }
            
            ownerWindow.formClosing(function () {
                var me = this;

                var tb = Mini2.find('textBox3');

                var value = tb.getValue();

                console.debug('ownerWindow', me);

                me.userData = {
                    text: value,
                    style_name: ''
                };

            });

        //    return false;
            
        //});



    });

</script>