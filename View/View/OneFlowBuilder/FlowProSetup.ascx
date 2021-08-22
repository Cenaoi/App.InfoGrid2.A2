<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlowProSetup.ascx.cs" Inherits="App.InfoGrid2.View.OneFlowBuilder.FlowProSetup" %>


<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form action="" method="post">


    <div runat="server" id="StoreSet">
        <mi:Store runat="server" ID="storeMain1" Model="FLOW_DEF" IdField="FLOW_DEF_ID" TranEnabled="false">
            <FilterParams>
                <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
                <mi:QueryStringParam Name="FLOW_DEF_ID" QueryStringField="def_id" />
            </FilterParams>
        </mi:Store>
    </div>

    <mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="10" PaddingTop="10" >

        <mi:TabPanel runat="server" ID="panel1" Padding="200" Region="Center" Dock="Full" PaddingTop="10" UI="win10" ButtonVisible="false" TabLeft="6">

            <mi:Tab runat="server" ID="TabPanel1" Text="基础属性">
                <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="600" PaddingTop="10" StoreID="storeMain1"
                    ItemLabelAlign="Right" Region="North" Height="200" FlowDirection="TopDown" AutoSize="true">

                    <mi:TextBox runat="server" ID="textBox3" DataField="DEF_CODE" FieldLabel="流程编码" ReadOnly="true" />

                    <mi:TextBox runat="server" ID="textBox4" DataField="DEF_TEXT" FieldLabel="流程名称"  />

                    <mi:TextBox runat="server" ID="secStruceCodeTb" DataField="SEC_STRUCE_CODE" FieldLabel="权限结构编码"  />

                    <mi:Textarea runat="server" ID="Textarea3" DataField="REMARK" FieldLabel="备注" />


                </mi:FormLayout>
            </mi:Tab>
            <mi:Tab runat="server" ID="Tab1" Text="表单设置">
                <mi:FormLayout runat="server" ID="FormLayout2" ItemWidth="370" PaddingTop="10" StoreID="storeMain1"
                    ItemLabelAlign="Right" Region="North" Height="200" FlowDirection="TopDown" AutoSize="true">

                    <mi:NumberBox runat="server" ID="NumberBox1" DataField="EI_JK_BFB" LabelWidth="300" FieldLabel="健康指标 n% 以上超时属于不健康状态" />
                    <mi:NumberBox runat="server" ID="NumberBox2" DataField="EI_YX" LabelWidth="300" FieldLabel="优秀指标 n 小时内完成审核" />
                    <mi:NumberBox runat="server" ID="NumberBox3" DataField="EI_LH" LabelWidth="300" FieldLabel="良好指标 n 小时内完成审核" />
                    <mi:NumberBox runat="server" ID="NumberBox4" DataField="EI_HG" LabelWidth="300" FieldLabel="合格指标 n 小时内完成审核" />
                    <mi:NumberBox runat="server" ID="NumberBox5" DataField="EI_YZ" LabelWidth="300" FieldLabel="严重超标 n 小时内未完成审核" />
                </mi:FormLayout>
            </mi:Tab>
            <mi:Tab runat="server" ID="Tab2" Text="版本">
                <mi:FormLayout runat="server" ID="HanderForm1" ItemWidth="300" PaddingTop="10" StoreID="storeMain1"
                    ItemLabelAlign="Right" Region="North" Height="200" FlowDirection="TopDown" AutoSize="true">


                    <mi:TextBox runat="server" ID="textBox1" DataField="AUTHOR_CODE" FieldLabel="作者编码"  />
                    <mi:TextBox runat="server" ID="textBox2" DataField="AUTHOR_TEXT" FieldLabel="作者名称"  />
                    <mi:TextBox runat="server" ID="textBox7" DataField="V_VERSION" FieldLabel="版本号"  />

                </mi:FormLayout>
            </mi:Tab>

        </mi:TabPanel>

        <mi:WindowFooter runat="server" ID="footer1">
            <mi:Button runat="server" ID="Button1" Text="取消" Width="80" Height="26" Dock="Center" OnClick="ownerWindow.close()" />
        </mi:WindowFooter>

    </mi:Viewport>
</form>









