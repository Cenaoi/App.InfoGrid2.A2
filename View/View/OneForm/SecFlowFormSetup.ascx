<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SecFlowFormSetup.ascx.cs" Inherits="App.InfoGrid2.View.OneForm.SecFlowFormSetup" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<!--字段二次权限设置界面-->
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


<form action="" method="post">


    <mi:Store runat="server" ID="store1" Model="SEC_TABLE_FLOW" IdField="PK_SEC_TF_CODE">
        <StringFields></StringFields>
        <FilterParams>
            <mi:QueryStringParam Name="PAGE_ID" QueryStringField="page_id" DbType="Int32" />
        </FilterParams>
        <SelectQuery>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </SelectQuery>
    </mi:Store>

    <mi:Store runat="server" ID="store2" Model="SEC_TABLE_FLOW_NODE" IdField="SEC_TABLE_FLOW_NODE_ID">
        <StringFields></StringFields>
        <FilterParams>
            <mi:QueryStringParam Name="PAGE_ID" QueryStringField="page_id" DbType="Int32" />
        </FilterParams>
        <SelectQuery>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:StoreCurrentParam ControlID="store1" Name="FK_SEC_TF_CODE" PropertyName="PK_SEC_TF_CODE" />
        </SelectQuery>
    </mi:Store>

    <mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="0" Padding="0">

        <mi:Toolbar runat="server">
            <mi:ToolBarButton Text="重置流程权限列表" Command="GoResetFlowList" />
        </mi:Toolbar>

        <mi:Table runat="server" ID="table1" Width="300" Region="West" StoreID="store1" PagerVisible="false">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="FLOW_CODE" HeaderText="流程编码" EditorMode="None" />
                <mi:BoundField DataField="FLOW_TEXT" HeaderText="流程名称" />
                <mi:BoundField DataField="REMARK" HeaderText="备注" />
            </Columns>
        </mi:Table>

        <mi:Table runat="server" ID="table2" Width="300" Region="Center" StoreID="store2" PagerVisible="false">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="FLOW_NODE_CODE" HeaderText="流程节点编码" EditorMode="None" />
                <mi:BoundField DataField="FLOW_NODE_TEXT" HeaderText="节点描述" />

                <mi:GroupColumn HeaderText="主表">
                    <mi:BoundField DataField="T_HEADER" HeaderText="主表名" />
                    <mi:BoundField DataField="T_HEADER_V_FIELDS" HeaderText="主表-可改字段集合" />
                    
                </mi:GroupColumn>

                <mi:GroupColumn HeaderText="尾部">
                    <mi:BoundField DataField="T_FOOTER" HeaderText="尾部表名" />
                    <mi:BoundField DataField="T_FOOTER_V_FIELDS" HeaderText="尾部-可改字段集合" />
                </mi:GroupColumn>


                <mi:GroupColumn HeaderText="子表 1">
                    <mi:BoundField DataField="T_SUB_1" HeaderText="子表1-表名" />
                    <mi:CheckColumn DataField="T_SUB_1_EDITOR" HeaderText="增删" />
                    <mi:BoundField DataField="T_SUB_1_V_FIELDS" HeaderText="子表1-可改字段集合" Width="400" />
                </mi:GroupColumn>

                <mi:GroupColumn HeaderText="子表 2">
                    <mi:BoundField DataField="T_SUB_2" HeaderText="子表2-表名" />
                    <mi:CheckColumn DataField="T_SUB_2_EDITOR" HeaderText="增删" />
                    <mi:BoundField DataField="T_SUB_2_V_FIELDS" HeaderText="子表2-可改字段集合" Width="400"  />
                </mi:GroupColumn>

                <mi:GroupColumn HeaderText="子表 3">

                    <mi:BoundField DataField="T_SUB_3" HeaderText="子表3-表名" />
                    <mi:CheckColumn DataField="T_SUB_3_EDITOR" HeaderText="增删" />
                    <mi:BoundField DataField="T_SUB_3_V_FIELDS" HeaderText="子表3-可改字段集合" Width="400"  />
                </mi:GroupColumn>

                <mi:GroupColumn HeaderText="子表 4">

                    <mi:BoundField DataField="T_SUB_4" HeaderText="子表4-表名" />
                    <mi:CheckColumn DataField="T_SUB_4_EDITOR" HeaderText="增删" />
                    <mi:BoundField DataField="T_SUB_4_V_FIELDS" HeaderText="子表4-可改字段集合" Width="400"  />
                </mi:GroupColumn>

                <mi:GroupColumn HeaderText="子表 5">

                    <mi:BoundField DataField="T_SUB_5" HeaderText="子表5-表名" />
                    <mi:CheckColumn DataField="T_SUB_5_EDITOR" HeaderText="增删" />
                    <mi:BoundField DataField="T_SUB_5_V_FIELDS" HeaderText="子表5-可改字段集合" Width="400"  />
                </mi:GroupColumn>

                <mi:GroupColumn HeaderText="子表 6">

                    <mi:BoundField DataField="T_SUB_6" HeaderText="子表6-表名" />
                    <mi:CheckColumn DataField="T_SUB_6_EDITOR" HeaderText="增删" />
                    <mi:BoundField DataField="T_SUB_6_V_FIELDS" HeaderText="子表6-可改字段集合" Width="400"  />
                </mi:GroupColumn>

                <mi:GroupColumn HeaderText="子表 7">

                    <mi:BoundField DataField="T_SUB_7" HeaderText="子表7-表名" />
                    <mi:CheckColumn DataField="T_SUB_7_EDITOR" HeaderText="增删" />
                    <mi:BoundField DataField="T_SUB_7_V_FIELDS" HeaderText="子表7-可改字段集合" Width="400"  />
                </mi:GroupColumn>

                <mi:GroupColumn HeaderText="子表 8">
                    <mi:BoundField DataField="T_SUB_8" HeaderText="子表8-表名" />
                    <mi:CheckColumn DataField="T_SUB_8_EDITOR" HeaderText="增删" />
                    <mi:BoundField DataField="T_SUB_8_V_FIELDS" HeaderText="子表8-可改字段集合" Width="400"  />
                </mi:GroupColumn>

                <mi:GroupColumn HeaderText="子表 9">
                    <mi:BoundField DataField="T_SUB_9" HeaderText="子表9-表名" />
                    <mi:CheckColumn DataField="T_SUB_9_EDITOR" HeaderText="增删" />
                    <mi:BoundField DataField="T_SUB_9_V_FIELDS" HeaderText="子表9-可改字段集合" Width="400"  />
                </mi:GroupColumn>

            </Columns>
        </mi:Table>

    </mi:Viewport>

</form>
<script>




</script>
