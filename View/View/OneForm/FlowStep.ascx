<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlowStep.ascx.cs" Inherits="App.InfoGrid2.View.OneForm.FlowStep" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<% if (false)
   { %>
<link href="/App_Themes/Blue/common.css" rel="stylesheet" type="text/css" />
<link href="/App_Themes/Vista/table.css" rel="stylesheet" type="text/css" />
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/JQuery.Query/jquery.query-2.1.7.js" type="text/javascript"></script>
<link href="/Core/Scripts/ui-lightness/jquery-ui-1.8.6.custom.css" rel="stylesheet"
    type="text/css" />
<script src="/Core/Scripts/ui-lightness/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
<script src="/Core/Scripts/validate/jquery.validate-vsdoc.js" type="text/javascript"></script>
<script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>
<script src="/Core/Scripts/Mini/_CodeHelp.js" type="text/javascript"></script>
<% } %>


<form action="" id="form1"  method="post">


    <mi:Store runat="server" ID="store1" Model="FLOW_DEF_NODE_PARTY" IdField="FLOW_DEF_NODE_PARTY_ID">
        <TSqlQuery Enabeld="true">
            
        </TSqlQuery>
    </mi:Store>


<mi:Viewport runat="server" ID="viewport1" MarginTop="10" Padding="0">

    <%--
    <mi:TabPanel runat="server" ID="tabPabel1" Region="Center" UI="win10" ButtonVisible="false" TabLeft="10" >
        
        <mi:Tab runat="server" ID="tab1" Text="流程信息" Scroll="None">           

        </mi:Tab>
    </mi:TabPanel>
    --%>

    <mi:FormLayout runat="server" ID="formLayout1" ItemLabelAlign="Right" Padding="8" Region="North" Dock="Top" ItemWidth="400" AutoSize="true">
        
        <mi:TextBox runat="server" ID="moniPartyCodeTb" FieldLabel="模拟用户代码" />

        <mi:ComboBox runat="server" ID="cmb1" FieldLabel="推送" TriggerMode="None">
        </mi:ComboBox>


        <mi:RadioGroup runat="server" ID="radioGroup1" FieldLabel="标识" DefaultValue="0">
            <mi:ListItem Value="0" Text="正常"  />
            <mi:ListItem Value="10" Text="加急" />
        </mi:RadioGroup>

        <mi:Textarea runat="server" ID="textarea1" FieldLabel="意见" Height="100" Value="同意" />
        
    </mi:FormLayout>

    <mi:Panel runat="server" ID="panel2" PaddingLeft="114" Region="Center" Scroll="None">
        <mi:Table runat="server" ID="table1" StoreID="store1" PagerVisible="false" Height="160" Width="294"  JsonMode="Full" >
            <Columns>
                <mi:RowCheckColumn />
                <mi:BoundField DataField="P_USER_TEXT" HeaderText="姓名" EditorMode="None" />
            </Columns>
        </mi:Table>
    </mi:Panel>
            

    <mi:WindowFooter runat="server" ID="footer1" Height="44">
        <mi:Button runat="server" ID="okBtn" Command="GoEnter" Text="确定" Dock="Center" Width="80" />
        <mi:Button runat="server" ID="clearBtn" Text="取消" OnClick="ownerWindow.close()" Dock="Center" Width="80" />
    </mi:WindowFooter>
    

</mi:Viewport>

</form>
<script>


    $(document).ready(function () {

        //ownerWindow.formClosing(function () {

        //    console.log("窗体关闭...");

        //});
               

    });


    //Mini2.ready(function () {
    //    var table = Mini2.find('table1');
    //    table.setRowCheckAll(true);
    //});

</script>