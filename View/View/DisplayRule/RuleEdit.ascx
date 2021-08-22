<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RuleEdit.ascx.cs" Inherits="App.InfoGrid2.View.DisplayRule.RuleEdit" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<% if (false)
   { %>
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<% } %>

<form method="post">

    <!--	IG2_DISPLAY_RULE	展示规则-->
    <mi:Store runat="server" ID="Store1" Model="IG2_DISPLAY_RULE_B" IdField="IG2_DISPLAY_RULE_ID" PageSize="0" DeleteRecycle="true">
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
        <DeleteQuery>
            <mi:ControlParam Name="IG2_DISPLAY_RULE_ID" ControlID="table1" PropertyName="CheckedRows" />
        </DeleteQuery>

        <DeleteRecycleParams>
            <mi:Param Name="ROW_SID" DefaultValue="-3" />
            <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
        </DeleteRecycleParams>
    </mi:Store>

    <mi:Viewport runat="server" ID="viewport1" Padding="4" ItemMarginRight="6">
        <!--		IG2_DISPLAY_RULE	展示规则-->
        <mi:Panel runat="server" ID="UT_102Panel" Region="West" Scroll="None" Width="400">
            <mi:Toolbar runat="server" ID="Toolbar1">
                <mi:ToolBarButton Text="新增" OnClick="ser:Store1.Insert();" />
                <mi:ToolBarButton Text="刷新" OnClick="ser:Store1.Refresh();" />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="删除" BeforeAskText="确定删除记录?" OnClick="ser:Store1.Delete();" />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="应用到系统" Command="GoApply" />
            </mi:Toolbar>
            <mi:Table runat="server" ID="table1" Dock="Full" StoreID="Store1" PagerVisible="false">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField HeaderText="规则名称" DataField="RULE_TEXT" />
                    <mi:BoundField HeaderText="规则编码" DataField="RULE_CODE" />
                    <mi:CheckColumn HeaderText="激活" DataField="ENABLED" />
                    <mi:NumColumn  HeaderText="权限等级" DataField="SEC_LEVEL" />
                    <mi:BoundField HeaderText="备注" DataField="REMARK" />
                </Columns>
            </mi:Table>
        </mi:Panel>
        <mi:Panel runat="server" ID="UT_102Panels" Region="Center" Scroll="None">
           
            <mi:TabPanel runat="server" ID="tabPanel1" Dock="Full">
                <mi:Tab runat="server" ID="tab1" Text="显示JS">
                    
                    <textarea rows="5" style="overflow-x: auto; overflow-y: auto;" dock="full" id="tbxJS" runat="server"></textarea>

                </mi:Tab>
                <mi:Tab runat="server" ID="tab2" Text="显示CSS">
                <textarea rows="5" style=" overflow-x: auto; overflow-y: auto;" dock="full" id="tbxCSS" runat="server"></textarea>
                </mi:Tab>
                <mi:Tab runat="server" ID="tab3" Text="返回JS函数">
                    <textarea rows="5" style=" overflow-x: auto; overflow-y: auto;" dock="full" id="tbReturnFun" runat="server"></textarea>
                </mi:Tab>
            </mi:TabPanel>
            <mi:WindowFooter ID="WindowFooter1" runat="server">
                <mi:Button runat="server" ID="SubmitBtn" Width="120" Height="26" Command="btnSave"
                    Text="完成" Dock="Center" />
                <mi:Button runat="server" ID="Button2" Width="80" Height="26" OnClick="ownerWindow.close()"
                    Text="取消" Dock="Center" />
            </mi:WindowFooter>
        </mi:Panel>
    </mi:Viewport>

</form>


<script type="text/javascript">
    function SetValue(mJs, mCss,RJs) {
        $("#widget1_I_tbxJS").val(mJs);
        $("#widget1_I_tbxCSS").val(mCss);
        $("#widget1_I_tbReturnFun").val(RJs);
    }
</script>
