<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OpTableBatch.ascx.cs" Inherits="App.InfoGrid2.View.OneTable.OpTableBatch" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form action="" id="form1" method="post">

    <mi:Store runat="server" ID="storeMain" Model="IG2_BATCH_TABLE_OPERATE" IdField="IG2_BATCH_TABLE_OPERATE_ID">
        <FilterParams>
            <mi:QueryStringParam Name="IG2_BATCH_TABLE_OPERATE_ID" QueryStringField="id" DbType="Int32" />
        </FilterParams>

    </mi:Store>

    <mi:Store runat="server" ID="storeNewCol" Model="IG2_BATCH_TO_COL" IdField="IG2_BATCH_TO_COL_ID" DeleteRecycle="true" PageSize="0">
        <FilterParams>
            <mi:QueryStringParam Name="BATCH_TABLE_OPERATE_ID" QueryStringField="id" DbType="Int32" />
            <mi:QueryStringParam Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
        <InsertParams>
            <mi:QueryStringParam Name="BATCH_TABLE_OPERATE_ID" QueryStringField="id" DbType="Int32" />
            <mi:Param Name="DB_TYPE" DefaultValue="string" />
            <mi:Param Name="SEC_LEVEL" DefaultValue="6" />
            <mi:Param Name="IS_MANDATORY" DefaultValue="true" />
        </InsertParams>
        <DeleteQuery>
            <mi:ControlParam Name="IG2_BATCH_TO_COL_ID" ControlID="tableNewCols" PropertyName="CheckedRows" />
        </DeleteQuery>
        <DeleteRecycleParams>
            <mi:Param Name="ROW_SID" DefaultValue="-3" />
            <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
        </DeleteRecycleParams>
    </mi:Store>
    
    <mi:Store runat="server" ID="storeTable" Model="IG2_BATCH_TO_TABLE" IdField="IG2_BATCH_TO_TABLE_ID" ReadOnly="true" DeleteRecycle="true">
        <FilterParams>
            <mi:QueryStringParam Name="BATCH_TABLE_OPERATE_ID" QueryStringField="id" DbType="Int32" />
            <mi:QueryStringParam Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
        <InsertParams>
            <mi:QueryStringParam Name="BATCH_TABLE_OPERATE_ID" QueryStringField="id" DbType="Int32" />
        </InsertParams>
        <DeleteQuery>
            <mi:ControlParam Name="IG2_BATCH_TO_TABLE_ID" ControlID="tableTables" PropertyName="CheckedRows" />
        </DeleteQuery>
        <DeleteRecycleParams>
            <mi:Param Name="ROW_SID" DefaultValue="-3" />
            <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
        </DeleteRecycleParams>
    </mi:Store>


<mi:Viewport runat="server" ID="viewport1">
    
    <mi:FormLayout runat="server" ID="mianPanel1" Height="40" Region="North" StoreID="storeMain">

    </mi:FormLayout>
    <mi:TabPanel runat="server" ID="tabPanel1" UI="win10" Region="Center" TabLeft="10" ButtonVisible="false">

        <mi:Tab runat="server" ID="tab1" Text="1.新增字段" Scroll="None">

            <mi:Toolbar runat="server" ID="toolbar1">
                <mi:ToolBarButton Text="刷新" OnClick="ser:storeNewCol.Refresh()" />
                <mi:ToolBarButton Text="添加" OnClick="ser:storeNewCol.Insert()" />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="删除" OnClick="ser:storeNewCol.Delete()" BeforeAskText="确定删除选中记录?" />
            </mi:Toolbar>

            <mi:Table runat="server" ID="tableNewCols" Dock="Full" StoreID="storeNewCol">

                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField DataField="DB_FIELD" HeaderText="系统字段名" Width="200" />
                    <mi:BoundField DataField="F_NAME" HeaderText="字段名" Width="200" />
                    <mi:SelectColumn DataField="DB_TYPE" HeaderText="数据类型" TriggerMode="None" DropDownWidth="200">
                        <mi:ListItem Value="string" Text="字符串" TextEx="字符串（例：张三）" />
                        <mi:ListItem Value="int" Text="整数" TextEx="整数（例：100）" />
                        <mi:ListItem Value="datetime" Text="日期" TextEx="日期（例：2012-12-30）" />
                        <mi:ListItem Value="boolean" Text="布尔型" TextEx="布尔型（例：True,False）" />
                        <mi:ListItem Value="currency" Text="货币" TextEx="货币（例：￥1,000.00）" />
                        <mi:ListItem Value="decimal" Text="数值" TextEx="数值（例：211.05）"  />
                    </mi:SelectColumn>
                    <mi:NumColumn  HeaderText="长度" DataField="DB_LEN" /> 
                    <mi:NumColumn  HeaderText="小数位" DataField="DB_DOT" />
                    <mi:CheckColumn HeaderText="不允许为空" DataField="IS_MANDATORY" />
                    <mi:CheckColumn DataField="IS_REMARK" HeaderText="大数据" Width="80" />
                    <mi:SelectColumn DataField="SEC_LEVEL" HeaderText="权限等级" TriggerMode="None" >
                        <mi:ListItem Value="0" Text="0-用户自定义" />
                        <mi:ListItem Value="2" Text="2-普通" />
                        <mi:ListItem Value="4" Text="4-主键" />
                        <mi:ListItem Value="6" Text="6-业务系统" />
                        <mi:ListItem Value="8" Text="8-系统内部" />
                    </mi:SelectColumn>
                </Columns>

            </mi:Table>

        </mi:Tab>

        <mi:Tab runat="server" ID="tab2" Text="2.修改的表集合" Scroll="None">

            <mi:Toolbar runat="server" ID="toolbar2">
                <mi:ToolBarButton Text="刷新" OnClick="ser:storeTable.Refresh()" />
                <mi:ToolBarButton Text="添加" Command="GoTableSelect" />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="删除" OnClick="ser:storeTable.Delete()" BeforeAskText="确定删除选中的记录?" />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="监测字段" Command="GoCheckField" />

                <mi:ToolBarButton Text="开始添加字段" Command="GoBatchAdd" BeforeAskText="确定批量增加字段?" />
                <mi:ToolBarButton Text="开始删除字段" Command="GoBatchDelete" BeforeAskText="确定批量删除字段?" />

            </mi:Toolbar>

            <mi:Table runat="server" ID="tableTables" Dock="Full" StoreID="storeTable">

                <Columns>
                                    
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField DataField="IG2_BATCH_TO_TABLE_ID" HeaderText="#" Width="60" ItemAlign="Center" EditorMode="None" />
                    <mi:BoundField DataField="TABLE_NAME" HeaderText="数据表" Width="160" EditorMode="None" />
                    <mi:BoundField DataField="TABLE_DISPLAY" HeaderText="表名" Width="300"  />
                    <mi:BoundField DataField="STATE_TEXT" HeaderText="状态" Width="300"  />


                    <mi:BoundField DataField="RESULT_TEXT" HeaderText="处理结果" Width="300"  />
                    <mi:DateColumn DataField="ROW_DATE_CREATE" HeaderText="创建时间" Format="Y-m-d H:i" Width="140"  EditorMode="None"/>

                </Columns>

            </mi:Table>

        </mi:Tab>

        <mi:Tab runat="server" ID="tab3" Text="3.参数设置">

        </mi:Tab>

    </mi:TabPanel>


</mi:Viewport>


</form>