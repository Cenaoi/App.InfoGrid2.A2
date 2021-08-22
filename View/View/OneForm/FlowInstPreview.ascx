<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlowInstPreview.ascx.cs" Inherits="App.InfoGrid2.View.OneForm.FlowInstPreview" %>


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

<style type="text/css">
    .page-head {
        font-size: 26px;
        font-weight: bold;
    }
</style>

<form action="" method="post">


    <div runat="server" id="StoreSet">
        <mi:Store runat="server" ID="storeMain1" Model="FLOW_INST" IdField="FLOW_INST_ID" ReadOnly="true">
            <FilterParams>
                <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
                <mi:QueryStringParam Name="INST_CODE" QueryStringField="inst_code" />
            </FilterParams>
        </mi:Store>
        <mi:Store runat="server" ID="storeProd1" Model="FLOW_INST_STEP" IdField="FLOW_INST_STEP_ID">
            <FilterParams>
                <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
                <mi:QueryStringParam Name="INST_CODE" QueryStringField="inst_code" />
            </FilterParams>
        </mi:Store>
    </div>

    <mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="0" Padding="0">


        <mi:Panel runat="server" ID="HeadPanel" Height="30" Scroll="None" PaddingTop="0">
            <mi:Label runat="server" ID="headLab" Value="流程步骤" HideLabel="true" Dock="Center" Mode="Transform" />
        </mi:Panel>


        <mi:FormLayout runat="server" ID="HanderForm1" Dock="Top" ItemWidth="300" PaddingTop="0" StoreID="storeMain1"
            ItemLabelAlign="Right" Region="North" FlowDirection="LeftToRight" AutoSize="true" >
            <mi:TextBox runat="server" ID="textBox1" DataField="INST_TEXT" FieldLabel="流程名称" ReadOnly="true" />
            <mi:TextBox runat="server" ID="textBox4" DataField="DEF_CODE" FieldLabel="流程编码" ReadOnly="true" />
            <mi:TextBox runat="server" ID="textBox2" DataField="START_USER_TEXT" FieldLabel="发起人" ReadOnly="true" />
            <mi:ComboBox runat="server" ID="comboBox1" DataField="FLOW_SID" FieldLabel="流程状态" ReadOnly="true" TriggerMode="None"  StringItems="0=未启动;2=进行中...;4=已经完成;">
               
            </mi:ComboBox>
            

            <mi:TextBox runat="server" ID="textBox5" DataField="DATE_START" FieldLabel="流程发起时间" ReadOnly="true" />
            <mi:TextBox runat="server" ID="textBox6" DataField="DATE_END" FieldLabel="流程结束时间" ReadOnly="true" />
<%--            <mi:TextBox runat="server" ID="textBox7" DataField="FLOW_TYPE" FieldLabel="流程类型" ReadOnly="true" />--%>


<%--            <mi:TextBox runat="server" ID="textBox8" DataField="ADMIN_TEXT" FieldLabel="流程管理员名称" ReadOnly="true" />--%>

<%--            
            <div class="mi-newline"></div>
            <mi:TextBox runat="server" ID="textBox9" DataField="INST_REMARK" FieldLabel="备注" ReadOnly="true" MinWidth="600" />--%>
        </mi:FormLayout>
        <mi:Panel runat="server" ID="panel2" Dock="Full" Region="Center" Height="600" MinHeight="300" Padding="6" Scroll="None">
<%--            <mi:Toolbar runat="server" ID="prodToolbar1">
                <mi:ToolBarTitle Text="流程任务信息" />
            </mi:Toolbar>--%>
            
            <iframe id="flowIframe" class="mi-box-item" Dock="Full" Region="Center" 
                
                style="height:600px; width:100%;" >

            </iframe>

        </mi:Panel>

        <mi:Panel runat="server" ID="panel1" Dock="Full" Region="South" Padding="6" Height="300" MinHeight="200" Scroll="None">
<%--            <mi:Toolbar runat="server" ID="prodToolbar1">
                <mi:ToolBarTitle Text="流程任务信息" />
            </mi:Toolbar>--%>

            <mi:Table runat="server" ID="table1" StoreID="storeProd1" Dock="Full" Region="South" ReadOnly="true" ColumnLines="false" PagerVisible="false">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
    <%--                    <mi:BoundField HeaderText="流程节点编码" DataField="DEF_NODE_CODE" />--%>
                    <mi:BoundField HeaderText="环节名称" DataField="DEF_NODE_TEXT" />
                    
                    <mi:BoundField HeaderText="审核人" DataField="OP_CHECK_DESC" Width="200" />
                    
                    <mi:CheckColumn HeaderText="会签" DataField="P_IS_CONSIGN" />

                    <mi:SelectColumn HeaderText="审核状态" DataField="STEP_SID" TriggerMode="None" ItemAlign="Center" 
                        Renderer="checkMsg_renderer" StringItems="0=未执行;2=审核中...;4=审核完成;">
                    </mi:SelectColumn>

                    <%--<mi:DateTimeColumn HeaderText="开始时间" DataField="DATE_START" />--%>
                    <mi:DateTimeColumn HeaderText="审核时间" DataField="DATE_END" Format="Y-m-d H:i" />

                    <mi:SelectColumn HeaderText="会签类型" DataField="P_CONSIGN_TYPE" TriggerMode="None" ItemAlign="Center" 
                        StringItems="parallel=并行会签;only=唯一会签">
                    </mi:SelectColumn>

                    <mi:BoundField HeaderText="审批意见" Width="300" DataField="OP_CHECK_COMMENTS" />

<%--                    <mi:NumColumn HeaderText="必须多少人会签" DataField="P_MEET_COUNT" />
                    <mi:NumColumn HeaderText="当前已经签署数量" DataField="P_CUR_COUNT" />
                    <mi:NumColumn HeaderText="剩余多少人未签署" DataField="P_SURPLUS_COUNT" />--%>
                </Columns>
            </mi:Table>
        </mi:Panel>



    </mi:Viewport>


</form>

<script>
    "use strict";

    $(document).ready(function () {

        var frame = $('#flowIframe');

        var flow_inst_code = $.query.get('inst_code');
        
        var url = '/App/InfoGrid2/View/OneFlowBuilder/FlowInstPreview.aspx?flow_inst_code=' + flow_inst_code;

        $(frame).attr('src', url);

    });

    // private
    function checkMsg_renderer(value, metaData, cellValues, record, rowIdx, colIdx, store) {
        "use strict";

        var txt;

        console.log("record", record);

        if (record.get('IS_BACK_OPERATE') == true) {
            txt = '<span style="color:red;">退回</span>';
        }
        else {
            switch (value) {
                case 0: txt = '未执行...'; break;
                case 2: txt = '审核中...'; break;
                case 4: txt = '审核完成'; break;
            }
        }

        return txt;
    }

</script>