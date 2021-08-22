<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InstCopyList.ascx.cs" Inherits="App.InfoGrid2.View.OneFlow.InstCopyList" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<link href="/Core/Scripts/bootstrap/3.3.4/css/bootstrap.css" rel="stylesheet" />

<style>

    

    .mail{
        background-image:url(/res/mail/mail_201611.png);
        width:16px;
        height:16px;
        background-position-x:-48px;
        background-position-y:0px;
    }

    .mail-open{
        background-position-x: -48px;
        background-position-y: -16px;
    }

</style>

<form action="" method="post">
    <mi:Store runat="server" ID="store1" Model="FLOW_INST_COPY" IdField="FLOW_INST_COPY_ID" AutoFocus="false" 
         PageSize="20" SortText="FLOW_INST_COPY_ID desc"  >
        <StringFields>EXTEND_DOC_TEXT,EXTEND_DOC_URL</StringFields>
    </mi:Store>
    <mi:Viewport runat="server" ID="viewport1">

        <mi:Panel runat="server" ID="mainLeft" Width="160" Region="West" PaddingLeft="4" PaddingRight="4">
            <div style="height:40px;width:100%;" class="mi-newline"></div>
            <a href="UserFlowList.aspx?state=no_check" class="btn btn-warning" style="width:100%; margin-bottom:6px;" role="button">未审批流程</a>
            <a href="UserFlowList.aspx?state=check" class="btn btn-success" style="width:100%; margin-bottom:6px;" role="button">已审批流程</a>
            <a href="UserFlowStartList.aspx" class="btn btn-info" style="width:100%; margin-bottom:6px;" role="button">我发起流程</a>
            <a href="InstCopyList.aspx?is_open=false" class="btn btn-info" style="width:100%; margin-bottom:6px;" role="button">收到抄送 - 未读</a>
            <a href="InstCopyList.aspx?is_open=true" class="btn btn-info" style="width:100%; margin-bottom:6px;" role="button">收到抄送 - 已读</a>
        </mi:Panel>

        <mi:Panel runat="server" ID="mainBox" Region="Center" Scroll="None">

            <mi:SearchFormLayout runat="server" ID="searchFormLayout1" StoreID="store1">
                
                <mi:TextBox runat="server" ID="textBox1" FieldLabel="文档编码" />
                <mi:DateRangePicker runat="server" ID="dateRange1" FieldLabel="日期" />

                <mi:Button runat="server" ID="searchBtn1" Text="查询" Command="GoSearch" />
            </mi:SearchFormLayout>
            <mi:Toolbar runat="server" ID="toolbar1">
                <mi:ToolBarTitle Text="抄送列表" ID="headText1" />
            </mi:Toolbar>


            <mi:Table runat="server" ID="table1" StoreID="store1" ReadOnly="true" Dock="Full" JsonMode="Full" ColumnLines="false" >
                <Columns>

                    <mi:RowNumberer />
                    <mi:RowCheckColumn />

                    <mi:TemplateColumn DataField="IS_OPEN" Width="32"  EngineName="j" Resizable="false">
                        {#if $T.get('IS_OPEN') == true }
                            <div class="mail mail-open"  title="已打开">&nbsp;</div>
                        {#else}
                            <div class="mail" title="未打开过">&nbsp;</div>
                        {#/if}
                    </mi:TemplateColumn>
                    
                    <mi:ActionColumn Width="60">
                        <mi:ActionItem Command="GoViewDoc" DisplayMode="Text" Text="查看" />
                    </mi:ActionColumn>                
                    
                    <mi:SelectColumn HeaderText="" DataField="TAG_SID" TriggerMode="None" Width="50" ItemAlign="Center" StringItems="0=.;10=加急;" >
                    </mi:SelectColumn>
                    <mi:BoundField HeaderText="文档类型" DataField="EXTEND_BILL_TYPE" />
                    <mi:BoundField HeaderText="文档编码" DataField="EXTEND_BILL_CODE" />
                                    
                    <mi:BoundField HeaderText="环节名称" DataField="DEF_NODE_TEXT" />
                
                    <mi:BoundField HeaderText="用户编码" DataField="P_USER_CODE" Visible="false" />
                    <mi:BoundField HeaderText="用户名称" DataField="P_USER_TEXT" />
                    <mi:BoundField HeaderText="发起人" DataField="START_USER_TEXT" Width="80" />

                    <mi:TemplateColumn HeaderText="文档信息" EngineName="j">
                        {$T.get('EXT_COL_VALUE_1')}<br />
                        {$T.get('EXT_COL_VALUE_2')}
                    </mi:TemplateColumn>
                    <mi:TemplateColumn HeaderText="时间" DataField="ROW_DATE_CREATE" ItemAlign="Right" EngineName="j" Width="160">          
                        <div style="padding-bottom:4px;">{ moment( $T.get('ROW_DATE_CREATE') ).fromNow() }</div>                        
                        <div style="color:#808080;">{ Mini2.Date.format(new Date($T.get('ROW_DATE_CREATE')),'m月d日 (l) H:i ') }</div>
                    </mi:TemplateColumn>

                    <mi:BoundField HeaderText="流程实例编码" DataField="INST_CODE" />
                </Columns>
            </mi:Table>

        </mi:Panel>
       

    </mi:Viewport>
</form>


<script>

    $(function () {

        Mini2.onwerPage.reload = function () {
            
            widget1.submit('form:first', {
                action: 'Reload'
            });

        };
    });

</script>