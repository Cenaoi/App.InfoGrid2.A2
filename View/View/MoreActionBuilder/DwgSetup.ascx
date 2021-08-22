<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DwgSetup.ascx.cs" Inherits="App.InfoGrid2.View.MoreActionBuilder.DwgSetup" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form action="" method="post">


    <div runat="server" id="StoreSet">
        <mi:Store runat="server" ID="storeMain1" Model="AC3_DWG" IdField="AC3_DWG_ID" TranEnabled="false">
            <FilterParams>
                <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
                <mi:QueryStringParam Name="PK_DWG_CODE" QueryStringField="dwg_code" />
            </FilterParams>
        </mi:Store>
    </div>

    <mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="0" Padding="0">

        <mi:TabPanel runat="server" ID="panel1" Padding="200" Region="Center" Dock="Full" PaddingTop="10" UI="win10" ButtonVisible="false">

            <mi:Tab runat="server" ID="TabPanel1" Text="属性">

                <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="300" PaddingTop="10" StoreID="storeMain1"
                    ItemLabelAlign="Right" Region="North" FlowDirection="TopDown" AutoSize="true">


                    <mi:Label runat="server" ID="PK_DWG_CODE_tb" DataField="PK_DWG_CODE" FieldLabel="图纸编码" />
                    <mi:TextBox runat="server" ID="DWG_TEXT_tb" DataField="DWG_TEXT" FieldLabel="图纸名称" />
                    <mi:TextBox runat="server" ID="V_VERSION_tb" DataField="V_VERSION" FieldLabel="版本号" />
                    <mi:TextBox runat="server" ID="AUTHOR_CODE_tb" DataField="AUTHOR_CODE" FieldLabel="作者编码" />
                    <mi:TextBox runat="server" ID="AUTHOR_TEXT_tb" DataField="AUTHOR_TEXT" FieldLabel="作者名称" />

                    <mi:Textarea runat="server" ID="REMARK_tb" DataField="REMARK" FieldLabel="备注" />

                </mi:FormLayout>

            </mi:Tab>

        </mi:TabPanel>

        

        <mi:WindowFooter runat="server" ID="footer1">
            <mi:Button runat="server" ID="Button1" Text="关闭" Width="80" Height="26" Dock="Center" OnClick="ownerClose()" />
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

</script>