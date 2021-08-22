<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActionStepEdit2.ascx.cs" Inherits="App.InfoGrid2.View.OneAction.ActionStepEdit2" %>
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


<link href="/Core/Scripts/codemirror/codemirror-5.21.0/codemirror.css" rel="stylesheet" />
<script src="/Core/Scripts/codemirror/codemirror-5.21.0/codemirror.js"></script>
<script src="/Core/Scripts/codemirror/codemirror-5.21.0/mode/clike/clike.js"></script>

<form action="" id="form1" method="post">

<div runat="server" id="storeSet">

<mi:Store runat="server" ID="store1" Model="IG2_ACTION" IdField="IG2_ACTION_ID" PageSize="0" >
    <SelectQuery>
        <mi:QueryStringParam Name="IG2_ACTION_ID" QueryStringField="id" DbType="Int32" />
    </SelectQuery>
</mi:Store>

<mi:Store runat="server" ID="store2" Model="IG2_ACTION_ITEM" IdField="IG2_ACTION_ITEM_ID" DeleteRecycle="true" SortField="ROW_USER_SEQ"  PageSize="0">
    <FilterParams>
        <mi:QueryStringParam Name="IG2_ACTION_ID" QueryStringField="id" DbType="Int32"  />
    </FilterParams>
    <SelectQuery>
        <mi:Param Name="ROW_SID" DefaultValue="-1" Logic=">" />
    </SelectQuery>
    <InsertParams>
        <mi:QueryStringParam Name="IG2_ACTION_ID" QueryStringField="id" DbType="Int32"  />
        <mi:Param Name="ITEM_TYPE_ID" DefaultValue="SET" />
        <mi:Param Name="LINK_TYPE_ID" DefaultValue="=" />
    </InsertParams>
    <DeleteQuery>
        <mi:ControlParam Name="IG2_ACTION_ITEM_ID" ControlID="table2" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
    </DeleteRecycleParams>
</mi:Store>

<!--左边过滤条件-->
<mi:Store runat="server" ID="storeLeft1" Model="IG2_ACTION_FILTER" IdField="IG2_ACTION_FILTER_ID" DeleteRecycle="true" SortField="ROW_USER_SEQ"  PageSize="0" >
    <FilterParams>
        <mi:QueryStringParam Name="IG2_ACTION_ID" QueryStringField="id" DbType="Int32"  />
        <mi:Param Name="L_R_TAG" DefaultValue="L" />
    </FilterParams>
    <SelectQuery>
        <mi:Param Name="ROW_SID" DefaultValue="-1" Logic=">" />
    </SelectQuery>
    <InsertParams>
        <mi:QueryStringParam Name="IG2_ACTION_ID" QueryStringField="id" DbType="Int32"  />
        <mi:Param Name="L_R_TAG" DefaultValue="L" />
        <mi:Param Name="B_MODE" DefaultValue="DEFAULT" />
        <mi:Param Name="A_LOGIN" DefaultValue="==" />
    </InsertParams>
    <DeleteQuery>
        <mi:ControlParam Name="IG2_ACTION_FILTER_ID" ControlID="tableLeft" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
    </DeleteRecycleParams>
</mi:Store>
<!--条件判断-->
<mi:Store runat="server" ID="storeThen1" Model="IG2_ACTION_THEN" IdField="IG2_ACTION_THEN_ID" DeleteRecycle="true" PageSize="0">
    <FilterParams>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        <mi:QueryStringParam Name="IG2_ACTION_ID" QueryStringField="id" DbType="Int32"  />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="IG2_ACTION_THEN_ID" ControlID="tableThen1" PropertyName="CheckedRows" />
    </DeleteQuery>
    <InsertParams>
        <mi:QueryStringParam Name="IG2_ACTION_ID" QueryStringField="id" DbType="Int32"  />
        <mi:Param Name="A_TYPE_ID" DefaultValue="COUNT" />
    </InsertParams>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
    </DeleteRecycleParams>
</mi:Store>
<!--右边过滤条件-->
<mi:Store runat="server" ID="storeRight1" Model="IG2_ACTION_FILTER" IdField="IG2_ACTION_FILTER_ID" DeleteRecycle="true" SortField="ROW_USER_SEQ"  PageSize="0" >
    <FilterParams>
        <mi:QueryStringParam Name="IG2_ACTION_ID" QueryStringField="id" DbType="Int32"  />
        <mi:Param Name="L_R_TAG" DefaultValue="R" />
    </FilterParams>
    <SelectQuery>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </SelectQuery>
    <InsertParams>
        <mi:QueryStringParam Name="IG2_ACTION_ID" QueryStringField="id" DbType="Int32"  />
        <mi:Param Name="L_R_TAG" DefaultValue="R" />
        <mi:Param Name="B_MODE" DefaultValue="DEFAULT" />
        <mi:Param Name="A_LOGIN" DefaultValue="==" />
    </InsertParams>
    <DeleteQuery>
        <mi:ControlParam Name="IG2_ACTION_FILTER_ID" ControlID="tableRight" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
    </DeleteRecycleParams>
</mi:Store>

<!--右边监听字段-->
<mi:Store runat="server" ID="storeRListen" Model="IG2_ACTION_LISTEN" IdField="IG2_ACTION_LISTEN_ID" DeleteRecycle="true" SortField="ROW_USER_SEQ"  PageSize="0" >
    <FilterParams>
        <mi:QueryStringParam Name="IG2_ACTION_ID" QueryStringField="id" DbType="Int32"  />
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </FilterParams>
    <SelectQuery>
    </SelectQuery>
    <InsertParams>
        <mi:QueryStringParam Name="IG2_ACTION_ID" QueryStringField="id" DbType="Int32"  />
        <mi:Param Name="LISTEN_ENABLED" DefaultValue="true" DbType="Boolean" />
    </InsertParams>
    <DeleteQuery>
        <mi:ControlParam Name="IG2_ACTION_LISTEN_ID" ControlID="tableRListen" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
    </DeleteRecycleParams>
</mi:Store>

</div>

<mi:Viewport runat="server" ID="viewport">
    
    <mi:Panel runat="server" ID="Panel1" Dock="Top" Region="North" Scroll="None" Layout="Auto" Height="100">
        <mi:Toolbar runat="server" ID="toolbar4" Scroll="None">
            <mi:ToolBarTitle Text="数据表关联" />
            <mi:ToolBarButton Text="应用到系统" Command="GoApply" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1"  StoreID="store1" Height="90" PagerVisible="false"  Dock="Top" Region="North"  >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:CheckColumn DataField="AUTO_CONTINUE" HeaderText="触发自动下一联动" />
                <mi:SelectColumn DataField="L_ACT_CODE" HeaderText="动作-L" TriggerMode="None" Width="80">
                    <mi:ListItem Value="DELETE" Text="删除" />
                    <mi:ListItem Value="INSERT" Text="新建" />
                    <mi:ListItem Value="UPDATE" Text="更新" />
                </mi:SelectColumn>
                <mi:SelectColumn DataField="L_NOT_EXIST_THEN" HeaderText="如果记录不存在" TriggerMode="None" Width="120">
                    <mi:ListItem Value="" Text="" TextEx="--没有动作--" />
                    <mi:ListItem Value="A" Text="创建新记录" />
                </mi:SelectColumn>
                <mi:BoundField DataField="L_TABLE" HeaderText="表名-L" />
                <mi:BoundField DataField="L_TABLE_TEXT" HeaderText="表描述-L" Width="200" />

                <mi:BoundField DataField="LINK_TYPE_ID" Width="60" EditorMode="None" ItemAlign="Center" Resizable="false"  />
            
                <mi:SelectColumn DataField="R_ACT_CODE" HeaderText="动作-R" TriggerMode="None" Width="80">
                    <mi:ListItem Value="ALL" Text="全部" />
                    <mi:ListItem Value="DELETE" Text="删除" />
                    <mi:ListItem Value="INSERT" Text="新建" />
                    <mi:ListItem Value="UPDATE" Text="更新" />
                </mi:SelectColumn>
                <mi:BoundField DataField="R_TABLE" HeaderText="表名-R" />
                <mi:BoundField DataField="R_TABLE_TEXT" HeaderText="表描述-R" Width="200" />

                <mi:CheckColumn DataField="R_IS_SUB_FILTER" HeaderText="过滤子集合-R" Width="100" />

                <mi:SelectColumn DataField="LISTEN_AO" HeaderText="逻辑运算" Width="50" TriggerMode="None" ItemAlign="Center"  >
                    <mi:ListItem Value="AND" Text="与" />
                    <mi:ListItem Value="OR" Text="或" />
                </mi:SelectColumn>
            
                <mi:CheckColumn DataField="ENABLED" HeaderText="联动生效" Width="80" />

                <mi:BoundField Width="60" EditorMode="None" ItemAlign="Center" Resizable="false"  />

                <mi:BoundField DataField="REMARK" HeaderText="备注" Width="300" />

            </Columns>
        </mi:Table>

    </mi:Panel>


    <mi:Panel runat="server" ID="buttonPanel1" Dock="Top" Region="North" Scroll="None" Padding="0" ItemMarginRight="6" Height="260" >
        <mi:TabPanel runat="server" ID="tablePanel1" Dock="Left" Plain="true" Width="800" ButtonVisible="false" UI="win10" TabLeft="10">
            <mi:Tab runat="server" ID="tab1" Text="筛选-左" Scroll="None">
                <mi:Toolbar runat="server" ID="toolbar2">
                    <mi:ToolBarButton Text="新建" OnClick="ser:storeLeft1.Insert()" />
                    <mi:ToolBarButton Text="刷新" OnClick="ser:storeLeft1.Refresh()" />
                    <mi:ToolBarHr />
                    <mi:ToolBarButton OnClick="ser:storeLeft1.MoveUp()" Text="上移" />
                    <mi:ToolBarButton OnClick="ser:storeLeft1.MoveDown()" Text="下移" />
                    <mi:ToolBarHr />
                    <mi:ToolBarButton Text="删除" OnClick="ser:storeLeft1.Delete()" BeforeAskText="确定删除记录?" />
                </mi:Toolbar>
        
                <mi:Table runat="server" ID="tableLeft" StoreID="storeLeft1" PagerVisible="false" Dock="Full" 
                    Height="120" Width="300" Sortable="false" ColumnLines="false">
                    <Columns>
                        <mi:RowNumberer />
                        <mi:RowCheckColumn />

                        
                        <mi:SelectBaseColumn DataField="A_COL_TEXT" HeaderText="字段名-A"  Width="100" OnDropDown="mappingFilterL_cols(this)" 
                            ItemValueField="F_NAME" ItemDisplayField="F_NAME" TriggerMode="None" DropDownWidth="200">
                            <MapItems>
                                <mi:MapItem SrcField="DB_FIELD" TargetField="A_COL" />
                            </MapItems>
                        </mi:SelectBaseColumn>

                        <mi:BoundField DataField="A_COL" HeaderText="字段名-A" />


                        <mi:SelectColumn DataField="A_LOGIN" HeaderText="筛选逻辑-A" TriggerMode="None" Width="80" DropDownWidth="120">
                            <mi:ListItem Value="" Text="" TextEx="-- N/A --" />
                            <mi:ListItem Value="==" Text="等于" />
                            <mi:ListItem Value=">" Text="大于" />
                            <mi:ListItem Value="<" Text="小于" />
                            <mi:ListItem Value="!=" Text="不等于" />
                            <mi:ListItem Value="&lt;=" Text="小于或等于" />
                            <mi:ListItem Value="&gt;=" Text="大于或等于" />
                            <mi:ListItem Value="In" Text="In" />
                            <mi:ListItem Value="NotIn" Text="NotIn" />
                            <mi:ListItem Value="Like" Text="Like" />
                            <mi:ListItem Value="NotLike" Text="NotLike" />
                        </mi:SelectColumn>
                        

                        <mi:SelectColumn DataField="B_VALUE_MODE" HeaderText="值类型-B" TriggerMode="None" ItemAlign="Center"  Width="80">
                            <mi:ListItem Value="" Text="" TextEx="-- N/A --" />
                            <mi:ListItem Value="ALL" Text="全部" />
                            <mi:ListItem Value="FIXED" Text="固定值" />
                            <mi:ListItem Value="TABLE" Text="表字段" />
                            <mi:ListItem Value="FUN" Text="函数值" />
                        </mi:SelectColumn>
                
                        <mi:BoundField DataField="B_VALUE_FIXED" HeaderText="固定值-B" />

                        <mi:SelectBaseColumn DataField="B_VALUE_TABLE_TEXT" HeaderText="表名-R"  Width="200" OnDropDown="mappingItemR_table(this)" 
                            ItemValueField="DISPLAY" ItemDisplayField="DISPLAY" TriggerMode="None" DropDownWidth="200">
                            <MapItems>
                                <mi:MapItem SrcField="TABLE_NAME" TargetField="B_VALUE_TABLE" />
                            </MapItems>
                        </mi:SelectBaseColumn>

                        <mi:BoundField DataField="B_VALUE_TABLE" HeaderText="表名-B" />

                        <mi:SelectBaseColumn DataField="B_VALUE_COL_TEXT" HeaderText="字段描述-R"  Width="100" OnDropDown="mappingItemL_B_cols(this)" 
                            ItemValueField="F_NAME" ItemDisplayField="F_NAME" TriggerMode="None" DropDownWidth="200">
                            <MapItems>
                                <mi:MapItem SrcField="DB_FIELD" TargetField="B_VALUE_COL" />
                            </MapItems>
                        </mi:SelectBaseColumn>

                        <mi:BoundField DataField="B_VALUE_COL" HeaderText="字段名-B" />

                        <mi:BoundField DataField="B_VALUE_FUN" HeaderText="函数值-B" />


                    </Columns>
                </mi:Table>

            </mi:Tab>
            
            <mi:Tab runat="server" ID="mainTabChanged" Text="条件判断" Scroll="None">

                <mi:Toolbar ID="Toolbar6" runat="server">

                    <mi:ToolBarButton Text="新增" OnClick="ser:storeThen1.Insert()" />
                    <mi:ToolBarButton Text="保存" OnClick="ser:storeThen1.SaveAll()" />
                    <mi:ToolBarButton Text="刷新" OnClick="ser:storeThen1.Refresh()" />
                    <mi:ToolBarHr />
                    <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:storeThen1.Delete()" />


                </mi:Toolbar>
                <mi:Table runat="server" ID="tableThen1" StoreID="storeThen1" Dock="Full" PagerVisible="false"  ColumnLines="false" >
                    <Columns>
                        <mi:RowNumberer />
                        <mi:RowCheckColumn />
                        <mi:SelectColumn HeaderText="判断类型" DataField="A_TYPE_ID" TriggerMode="None">
                            <mi:ListItem Value="COUNT" Text="记录数量" />
                            <mi:ListItem Value="FIELD" Text="字段名" />
                            <mi:ListItem Value="TOTAL_FUN" Text="统计函数" />
                            <mi:ListItem Value="FIRST_FIELD" Text="首行字段名" />
                        </mi:SelectColumn>

                        
                        <mi:SelectBaseColumn DataField="A_FIELD_TEXT" HeaderText="字段描述"  Width="100" OnDropDown="mappingFilterL_cols(this)" 
                            ItemValueField="F_NAME" ItemDisplayField="F_NAME" TriggerMode="None" DropDownWidth="200">
                            <MapItems>
                                <mi:MapItem SrcField="DB_FIELD" TargetField="A_FIELD" />
                            </MapItems>
                        </mi:SelectBaseColumn>

                        <mi:BoundField DataField="A_FIELD" HeaderText="内部字段名" />

                        <mi:SelectColumn DataField="A_LOGIN" HeaderText="逻辑" TriggerMode="None" Width="80" DropDownWidth="120" >
                            <mi:ListItem Value="" Text="" TextEx="-- N/A --" />
                            <mi:ListItem Value="==" Text="等于" />
                            <mi:ListItem Value=">" Text="大于" />
                            <mi:ListItem Value="<" Text="小于" />
                            <mi:ListItem Value="!=" Text="不等于" />
                            <mi:ListItem Value="&lt;=" Text="小于或等于" />
                            <mi:ListItem Value="&gt;=" Text="大于或等于" />
                            <mi:ListItem Value="In" Text="In" />
                            <mi:ListItem Value="NotIn" Text="NotIn" />
                            <mi:ListItem Value="Like" Text="Like" />
                            <mi:ListItem Value="NotLike" Text="NotLike" />
                        </mi:SelectColumn>
                        <mi:BoundField HeaderText="值" DataField="A_VALUE" />
                        <mi:BoundField HeaderText="统计函数" DataField="A_TOTAL_FUN" />
                        <mi:CheckColumn HeaderText="停止执行" DataField="IS_STOP" Width="60" />
                        
                        <mi:BoundField HeaderText="返回的消息" DataField="RSULT_MESSAGE" Width="300" />
                    </Columns>
                </mi:Table>
            </mi:Tab>
        </mi:TabPanel>

        <mi:TabPanel runat="server" ID="TabPanel2" Dock="Full" Plain="true" Width="800" ButtonVisible="false" UI="win10">
            <mi:Tab runat="server" ID="tab2" Text="筛选-右" Scroll="None">
                <mi:Toolbar runat="server" ID="toolbar3">
                    <mi:ToolBarButton Text="新建" OnClick="ser:storeRight1.Insert()" />
                    <mi:ToolBarButton Text="刷新" OnClick="ser:storeRight1.Refresh()" />
                    <mi:ToolBarHr />
                    <mi:ToolBarButton OnClick="ser:storeRight1.MoveUp()" Text="上移" />
                    <mi:ToolBarButton OnClick="ser:storeRight1.MoveDown()" Text="下移" />
                    <mi:ToolBarHr />
                    <mi:ToolBarButton Text="删除" OnClick="ser:storeRight1.Delete()" BeforeAskText="确定删除记录?" />
                </mi:Toolbar>

                <mi:Table runat="server" ID="tableRight" StoreID="storeRight1" PagerVisible="false" Dock="Full" Height="120" Width="300" Sortable="false"  ColumnLines="false">
                    <Columns>
                        <mi:RowNumberer />
                        <mi:RowCheckColumn />

                        <mi:SelectBaseColumn DataField="A_COL_TEXT" HeaderText="字段名-A"  Width="100" OnDropDown="mappingFilterR_cols(this)" 
                            ItemValueField="F_NAME" ItemDisplayField="F_NAME" TriggerMode="None" DropDownWidth="200">
                            <MapItems>
                                <mi:MapItem SrcField="DB_FIELD" TargetField="A_COL" />
                            </MapItems>
                        </mi:SelectBaseColumn>

                        <mi:BoundField DataField="A_COL" HeaderText="字段名-A" />
                        <mi:SelectColumn DataField="A_LOGIN" HeaderText="筛选逻辑-A" TriggerMode="None" Width="80" DropDownWidth="120" >
                            <mi:ListItem Value="" Text="" TextEx="-- N/A --" />
                            <mi:ListItem Value="==" Text="等于" />
                            <mi:ListItem Value=">" Text="大于" />
                            <mi:ListItem Value="<" Text="小于" />
                            <mi:ListItem Value="!=" Text="不等于" />
                            <mi:ListItem Value="&lt;=" Text="小于或等于" />
                            <mi:ListItem Value="&gt;=" Text="大于或等于" />
                            <mi:ListItem Value="In" Text="In" />
                            <mi:ListItem Value="NotIn" Text="NotIn" />
                            <mi:ListItem Value="Like" Text="Like" />
                            <mi:ListItem Value="NotLike" Text="NotLike" />
                        </mi:SelectColumn>



                        <mi:SelectColumn DataField="B_VALUE_MODE" HeaderText="值类型-B" TriggerMode="None" ItemAlign="Center"  Width="80">
                            <mi:ListItem Value="" Text="" TextEx="-- N/A --" />
                            <mi:ListItem Value="ALL" Text="全部" />
                            <mi:ListItem Value="FIXED" Text="固定值" />
                            <mi:ListItem Value="FUN" Text="函数值" />
                            <mi:ListItem Value="TABLE" Text="表字段" />
                            <mi:ListItem Value="USER_FUNC" Text="自定义公式" />
                        </mi:SelectColumn>
                
                        <mi:BoundField DataField="B_VALUE_FIXED" HeaderText="固定值-B" />

                        
                        <mi:SelectBaseColumn DataField="B_VALUE_TABLE_TEXT" HeaderText="表名-R"  Width="200" OnDropDown="mappingItemR_table(this)" 
                            ItemValueField="DISPLAY" ItemDisplayField="DISPLAY" TriggerMode="None" DropDownWidth="200">
                            <MapItems>
                                <mi:MapItem SrcField="TABLE_NAME" TargetField="B_VALUE_TABLE" />
                            </MapItems>
                        </mi:SelectBaseColumn>
                        <mi:BoundField DataField="B_VALUE_TABLE" HeaderText="表名-B" />


                        <mi:SelectBaseColumn DataField="B_VALUE_COL_TEXT" HeaderText="字段描述-R"  Width="100" OnDropDown="mappingItemL_B_cols(this)" 
                            ItemValueField="F_NAME" ItemDisplayField="F_NAME" TriggerMode="None" DropDownWidth="200">
                            <MapItems>
                                <mi:MapItem SrcField="DB_FIELD" TargetField="B_VALUE_COL" />
                            </MapItems>
                        </mi:SelectBaseColumn>
                        <mi:BoundField DataField="B_VALUE_COL" HeaderText="字段名-B" />

                        <mi:BoundField DataField="B_VALUE_FUN" HeaderText="函数值-B" />


                    </Columns>
                </mi:Table>

            </mi:Tab>
        </mi:TabPanel>        

        <mi:TabPanel runat="server" ID="TabPanle3" Dock="Right" Plain="true" Width="300"  ButtonVisible="false" UI="win10">
            <mi:Tab runat="server" ID="tab3" Text="监听字段" Scroll="None">
                <mi:Toolbar runat="server" ID="toolbar5" >
                    <mi:ToolBarButton Text="新建" OnClick="ser:storeRListen.Insert()" />
                    <mi:ToolBarButton Text="刷新" OnClick="ser:storeRListen.Refresh()" />
                    <mi:ToolBarHr />
                    <mi:ToolBarButton OnClick="ser:storeRListen.MoveUp()" Text="上移" />
                    <mi:ToolBarButton OnClick="ser:storeRListen.MoveDown()" Text="下移" />
                    <mi:ToolBarHr />
                    <mi:ToolBarButton Text="删除" OnClick="ser:storeRListen.Delete()" BeforeAskText="确定删除记录?" />
                </mi:Toolbar>

                <mi:Table runat="server" ID="tableRListen" StoreID="storeRListen" PagerVisible="false" Dock="Full" Height="120" Width="300" Sortable="false"  ColumnLines="false">
                    <Columns>
                        <mi:RowNumberer />
                        <mi:RowCheckColumn />
                        
                        <mi:SelectBaseColumn DataField="R_FIELD_TEXT" HeaderText="字段描述-R"  Width="100" OnDropDown="mappingFilterR_cols(this)" 
                            ItemValueField="F_NAME" ItemDisplayField="F_NAME" TriggerMode="None" DropDownWidth="200">
                            <MapItems>
                                <mi:MapItem SrcField="DB_FIELD" TargetField="R_DB_FIELD" />
                            </MapItems>
                        </mi:SelectBaseColumn>
            
                        <mi:BoundField DataField="R_DB_FIELD" HeaderText="字段名" />
                        <mi:BoundField DataField="VALUE_FROM" HeaderText="值-变化前" />
                        <mi:BoundField DataField="VALUE_TO" HeaderText="值-变化后" />
                        <mi:CheckColumn DataField="LISTEN_ENABLED" HeaderText="激活" />

                    </Columns>
                </mi:Table>
            </mi:Tab>
        </mi:TabPanel>
    </mi:Panel>


    <mi:TabPanel runat="server" ID="mainTabPanel1" Dock="Full" Padding="4" Plain="true" Region="Center"  ButtonVisible="false" UI="win10" TabLeft="10">
        <mi:Tab runat="server" ID="mainTab1" Text="赋值-左" Scroll="None">

            <mi:Toolbar runat="server" ID="toolbar1" Scroll="None">
                <mi:ToolBarTitle Text="赋值" />
                <mi:ToolBarButton Text="新建" OnClick="ser:store2.Insert()" />
                <mi:ToolBarButton Text="刷新" OnClick="ser:store2.Refresh()" />
                <mi:ToolBarHr />
                <mi:ToolBarButton OnClick="ser:store2.MoveUp()" Text="上移" />
                <mi:ToolBarButton OnClick="ser:store2.MoveDown()" Text="下移" />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="删除" OnClick="ser:store2.Delete()" BeforeAskText="确定删除记录?" />
            </mi:Toolbar>
            <mi:Table runat="server" ID="table2"  StoreID="store2"  Dock="Full" Region="Center" PagerVisible="false" Sortable="false"  ColumnLines="false">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />

                    <mi:SelectBaseColumn DataField="L_COL_TEXT" HeaderText="字段名-L"  Width="100" OnDropDown="mappingItemL_cols(this)" 
                        ItemValueField="F_NAME" ItemDisplayField="F_NAME" TriggerMode="None" DropDownWidth="200">
                        <MapItems>
                            <mi:MapItem SrcField="DB_FIELD" TargetField="L_COL" />
                        </MapItems>
                    </mi:SelectBaseColumn>

                    <mi:BoundField DataField="L_COL" HeaderText="字段描述-L" Width="160" EditorMode="None" />

                    <mi:BoundField DataField="LINK_TYPE_ID" Width="40" EditorMode="None" ItemAlign="Center" Resizable="false"  />
           

                    <mi:SelectColumn DataField="R_VALUE_MODE" HeaderText="值模式-R" TriggerMode="None" ItemAlign="Center" Width="80">
                        <mi:ListItem Value="" Text="" TextEx="-- N/A --" />
                        <mi:ListItem Value="FIXED" Text="固定值" />
                        <mi:ListItem Value="TABLE" Text="表字段" />
                        <mi:ListItem Value="FUN" Text="函数值" />
                        <mi:ListItem Value="USER_FUNC" Text="自定义公式" />
                    </mi:SelectColumn>
                
                    <mi:BoundField DataField="R_VALUE_FIXED" HeaderText="固定值-R" />

                    <mi:SelectBaseColumn DataField="R_VALUE_TABLE_TEXT" HeaderText="表名-R"  Width="200" OnDropDown="mappingItemR_table(this)" 
                        ItemValueField="DISPLAY" ItemDisplayField="DISPLAY" TriggerMode="None" DropDownWidth="200">
                        <MapItems>
                            <mi:MapItem SrcField="TABLE_NAME" TargetField="R_VALUE_TABLE" />
                        </MapItems>
                    </mi:SelectBaseColumn>

                    <mi:BoundField DataField="R_VALUE_TABLE" HeaderText="表名-R" />

                    <mi:SelectBaseColumn DataField="R_VALUE_COL_TEXT" HeaderText="字段描述-R"  Width="100" OnDropDown="mappingItemR_cols(this)" 
                        ItemValueField="F_NAME" ItemDisplayField="F_NAME" TriggerMode="None" DropDownWidth="200">
                        <MapItems>
                            <mi:MapItem SrcField="DB_FIELD" TargetField="R_VALUE_COL" />
                        </MapItems>
                    </mi:SelectBaseColumn>

                    
                    <mi:BoundField DataField="R_VALUE_COL" HeaderText="字段名-R" />

                    <mi:SelectColumn DataField="R_VALUE_FUN" HeaderText="函数值-R" >
                        <mi:ListItem Value="TIME_NOW" Text="现在时间" />
                        <mi:ListItem Value="DATE_NOW" Text="现在日期" />
                    </mi:SelectColumn>

                    <mi:SelectColumn DataField="R_VALUE_TOTAL_FUN" HeaderText="统计函数-R" TriggerMode="None" ItemAlign="Center">
                        <mi:ListItem Value="" Text="" TextEx="-- N/A --" />
                        <mi:ListItem Value="SUM" Text="求和" />
                        <mi:ListItem Value="COUNT" Text="计数" />
                        <mi:ListItem Value="AVG" Text="平均值" />
                    </mi:SelectColumn>

                    <mi:BoundField DataField="R_VALUE_USER_FUNC"  HeaderText="自定义公式" Width="400" />

                    <mi:BoundField DataField="REMAR" HeaderText="备注" Width="200" />

                    <mi:SelectColumn DataField="L_ACT_CODE" HeaderText="动作" TriggerMode="None" ItemAlign="Center">
                        <mi:ListItem Value="" Text="" TextEx="-- N/A --" />
                        <mi:ListItem Value="A" Text="只在新建的时候赋值" />
                        <mi:ListItem Value="E" Text="只修改" />
                        <mi:ListItem Value="A-E" Text="新建和更新" />
                    </mi:SelectColumn>

                </Columns>
            </mi:Table>
        </mi:Tab>

        <mi:Tab runat="server" ID="actSCodeTab" Text="脚本" Scroll="None">
            
            <mi:FormLayout runat="server" ID="actSCodeForm"  StoreID="store1" Dock="Full" ItemLabelAlign="Right">

                <mi:CheckBox runat="server" ID="actNewEnabled1" DataField="ACT_NEW_ENABLED" FieldLabel="新建执行脚本" TrueText="启动" FalseText="关闭" />
                <mi:CodeEditor runat="server" ID="actNewSCodeTB22" DataField="ACT_NEW_SCODE" FieldLabel="新建脚本" Height="300" />

                <mi:CheckBox runat="server" ID="CheckBox1" DataField="ACT_UPDATE_ENABLED" FieldLabel="更新执行脚本" TrueText="启动" FalseText="关闭" />

               <mi:CodeEditor runat="server" ID="codeEditor1" DataField="ACT_UPDATE_SCODE" FieldLabel="脚本" />
            </mi:FormLayout>



        </mi:Tab>

    </mi:TabPanel>


<%--    <mi:WindowFooter runat="server" ID="Panel1" >
        <mi:Button runat="server" ID="OkBtn" Text="上一步" Width="80" Height="26" Command="GoPre" Dock="Center" />
        <mi:Button runat="server" ID="Button2" Text="完成" Width="80" Height="26" Command="GoLast" Dock="Center"  />
        <mi:Button runat="server" ID="Button1" Text="取消" Width="80" Height="26" Dock="Right" OnClick="ownerWindow.close()" />
    </mi:WindowFooter>--%>
</mi:Viewport>


</form>






<script type="text/javascript">


    /*   过滤条件，左边表格 */

    function mappingFilterL_cols(owner) {
        var me = owner,
            store = me.store;

        //alert(me.classType.fullName);

        var store1 = window.widget1_I_store1;

        var curRect = store1.getCurrent();

        var curTable = curRect.get('L_TABLE');

        Mini2.get('/App/InfoGrid2/View/OneTable/OneTableStructure.aspx', {
            action: 'GET_FIELDS',
            table: curTable
        },
        function (data) {
            try {
                store.removeAll();
                store.add(data);

                owner.refresh();
            }
            catch (ex) {
                alert("错误:" + ex.message);
            }
        });
    }



    function mappingItemL_B_cols(owner) {
        var me = owner,
            store = me.store,
            record = me.record;


        var curTable = record.get('B_VALUE_TABLE');


        Mini2.get('/App/InfoGrid2/View/OneTable/OneTableStructure.aspx', {
            action: 'GET_FIELDS',
            table: curTable
        },
        function (data) {

            try {
                store.removeAll();
                store.add(data);

                owner.refresh();
            }
            catch (ex) {
                alert("错误:" + ex.message);
            }


        });
    }

    //过滤，右边的字段名
    function mappingFilterR_cols(owner) {
        var me = owner,
            store = me.store;

        //alert(me.classType.fullName);

        var store1 = window.widget1_I_store1;

        var curRect = store1.getCurrent();

        var curTable = curRect.get('R_TABLE');

        Mini2.get('/App/InfoGrid2/View/OneTable/OneTableStructure.aspx', {
            action: 'GET_FIELDS',
            table: curTable
        },
        function (data) {

            try {                
                store.removeAll();
                store.add({ value: '', text: '', textEx: '--空--' });
                store.add(data);

                owner.refresh();
            }
            catch (ex) {
                alert("错误:" + ex.message);
            }


        });
    }

</script>

<script type="text/javascript" >

    /*   赋值表格的联动 , 最下面的表格 */

    function mappingItemL_cols(owner) {
        var me = owner,
            store = me.store;


        var store1 = window.widget1_I_store1;

        var curRect = store1.getCurrent();

        var curTable = curRect.get('L_TABLE');

        Mini2.get('/App/InfoGrid2/View/OneTable/OneTableStructure.aspx', {
            action: 'GET_FIELDS',
            table: curTable
        },
        function (data) {

            try {

                store.removeAll();
                store.add(data);

                owner.refresh();
            }
            catch (ex) {
                alert("错误:" + ex.message);
            }
        });
    }




    function mappingItemR_cols(owner) {
        var me = owner,
            store = me.store,
            record = me.record;


        var curTable = record.get('R_VALUE_TABLE');
        
        Mini2.get('/App/InfoGrid2/View/OneTable/OneTableStructure.aspx', {
            action: 'GET_FIELDS',
            table: curTable
        },
        function (data) {

            try {
                store.removeAll();
                store.add(data);

                owner.refresh();
            }
            catch (ex) {
                alert("错误:" + ex.message);
            }
        });
    }



    function mappingItemR_table(owner) {
        var me = owner,
            store = me.store,
            record=  me.record;

        //alert(me.classType.fullName);

        var store1 = window.widget1_I_store1;

        var curRect = store1.getCurrent();
        var curTable = curRect.get('R_TABLE');
        var curTableText = curRect.get('R_TABLE_TEXT');


        store.removeAll();
        store.add([{
            TABLE_NAME: '',
            DISPLAY: '--空--'
        },{
            TABLE_NAME: curTable,
            DISPLAY: curTableText
        }, {
            TABLE_NAME:'OTHER...',
            DISPLAY:'其它...'
        }]);
        owner.refresh();


    }

    

</script>


