<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetToolbar.ascx.cs" Inherits="App.InfoGrid2.View.OneToolbar.SetToolbar" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" id="form1" method="post">

<mi:Store runat="server" ID="store1" Model="IG2_TOOLBAR" IdField="IG2_TOOLBAR_ID"  >
    <FilterParams>
        <mi:QueryStringParam Name="IG2_TOOLBAR_ID" QueryStringField="id" DbType="Int32" />
        <mi:QueryStringParam Name="TABLE_ID" QueryStringField="table_id" DbType="Int32" />
    </FilterParams>
</mi:Store>
<mi:Store runat="server" ID="store2" Model="IG2_TOOLBAR_ITEM" IdField="IG2_TOOLBAR_ITEM_ID" DeleteRecycle="true" SortField="ROW_USER_SEQ" >
    <DeleteQuery>
        <mi:ControlParam Name="IG2_TOOLBAR_ITEM_ID" ControlID="table2" PropertyName="CheckedRows" />
    </DeleteQuery>
    <DeleteRecycleParams>
        <mi:Param Name="ROW_SID" DefaultValue="-3" DbType="Int32" />
        <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
    </DeleteRecycleParams>
     <FilterParams>
        <mi:QueryStringParam Name="IG2_TOOLBAR_ID" QueryStringField="id" DbType="Int32" />
        <mi:QueryStringParam Name="TABLE_ID" QueryStringField="table_id" DbType="Int32" />
        <mi:Param Name="ROW_SID" DefaultValue="-1" DbType="Int32"  Logic=">" />
    </FilterParams>
     <InsertParams>
        <mi:QueryStringParam Name="IG2_TOOLBAR_ID" QueryStringField="id" DbType="Int32" />
        <mi:QueryStringParam Name="TABLE_ID" QueryStringField="table_id" DbType="Int32" />
        <mi:Param Name="DISPLAY_MODE_ID" DefaultValue="DEFAULT" />
        <mi:Param Name="EVENT_MODE_ID" DefaultValue="DEFAULT" />
        <mi:Param Name="ITEM_TYPE_ID" DefaultValue="BTN" />
        <mi:Param Name="ICON_ID" DefaultValue="0" />
        <mi:Param Name="VISIBLE" DefaultValue="true" DbType="Boolean" />
        <mi:Param Name="ENABLED" DefaultValue="true" DbType="Boolean" />
     </InsertParams>
</mi:Store>


<mi:Viewport runat="server" ID="viewport">

    <mi:Panel runat="server" ID="centerPanel" Dock="Top" Region="North" Scroll="None" Height="100" >
   
        <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="true" StoreID="store1" Dock="Full" PagerVisible="false" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />

                <mi:BoundField DataField="DISPLAY" HeaderText="工具栏描述"  />

            </Columns>
        </mi:Table>
    </mi:Panel>
    
    <mi:Panel runat="server" ID="Panel1" Dock="Full" Region="Center" Scroll="None">
   
        <mi:Toolbar runat="server" ID="toolbar1">
            <mi:ToolBarTitle Text="按钮项目" />
            <mi:ToolBarButton Text="新增" OnClick="ser:store2.Insert()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store2.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton OnClick="ser:store2.MoveUp()" Text="上移" />
            <mi:ToolBarButton OnClick="ser:store2.MoveDown()" Text="下移" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" OnClick="ser:store2.Delete()"  BeforeAskText="确定删除项目?" />
        </mi:Toolbar>
        <mi:Table runat="server" ID="table2" RowLines="true" ColumnLines="true" StoreID="store2" Dock="Full" Sortable="false" >
            <Columns>
                <mi:RowNumberer />

                <mi:RowCheckColumn />

                <mi:SelectColumn DataField="ITEM_TYPE_ID" HeaderText="类型" TriggerMode="None" Width="80" ItemAlign="Center">
                    <mi:ListItem  Value="BTN" Text="按钮"/>
                    <mi:ListItem  Value="HR" Text="分隔符"/>
                    <mi:ListItem  Value="MENU" Text="小菜单"/> 
                </mi:SelectColumn>

                <mi:BoundField DataField="ITEM_TEXT" HeaderText="按钮名称"  />
                
                 <mi:SelectColumn DataField="ICON_ID" HeaderText="图标" TriggerMode="None">
                    <mi:ListItem  Value="0" Text="" />
                </mi:SelectColumn>

                <mi:CheckColumn DataField="VISIBLE" HeaderText="可视" />
                <mi:CheckColumn DataField="ENABLED" HeaderText="激活" />

                <mi:SelectColumn DataField="ALIGN" HeaderAlign="Center" HeaderText="对齐方式" TriggerMode="None">
                    <mi:ListItem Value="LEFT" Text="左对齐" />
                    <mi:ListItem Value="RIGHT" Text="右对齐" />
                </mi:SelectColumn>

                 <mi:SelectColumn DataField="EVENT_MODE_ID"  HeaderText="事件模式"  TriggerMode="None">
                    <mi:ListItem  Value="DEFAULT" Text="" TextEx="--默认--" />
                    <mi:ListItem  Value="COMMAND" Text="服务器命令"/>
                    <mi:ListItem  Value="PLUG" Text="插件命令"/>
                    <mi:ListItem  Value="SYS" Text="内部常规命令"/> 
                </mi:SelectColumn>

                <mi:SelectColumn DataField="ITEM_ID"  HeaderText="常规命令"  TriggerMode="None">
                    <mi:ListItem  Value="" Text="" TextEx="--没有--"/>
                    <mi:ListItem  Value="INSERT" Text="新增" />
                    <mi:ListItem  Value="REFRESH" Text="刷新"/> 
                    <mi:ListItem  Value="SAVE" Text="保存"/>
                    <mi:ListItem  Value="SEARCH" Text="查询"/> 
                    <mi:ListItem  Value="TO_EXCEL" Text="导出 Excel"/> 
                    <mi:ListItem  Value="TO_PRINT" Text="打印"/> 
                    <mi:ListItem  Value="TO_PRINT_MAIN" Text="打印主表头"/> 
                    <mi:ListItem  Value="DELETE" Text="删除"/>
                    <mi:ListItem  Value="CHANGE_BIZ_SID" Text="改业务状态"/>
                    <mi:ListItem  Value="CHANGE_BIZ_SID_ALL" Text="改业务状态(全部)"/>
                    <mi:ListItem  Value="CHANGE_FIELD" Text="改数值字段的值" />
                    <mi:ListItem  Value="CHANGE_FSTABLE" Text="改数值字段的值(子表)" />
                    <mi:ListItem  Value="CHANGE_FIELD_NOW" Text="改为当前时间" />
                    <mi:ListItem  Value="SHOW_CATA_CHANGE" Text="转移" TextEx="转移-配合左边结构使用" />
                    <mi:ListItem  Value="CLOSE_AND_NEW" Text="关闭并新建"  />
                </mi:SelectColumn>


                <mi:SelectColumn DataField="DISPLAY_MODE_ID" HeaderText="显示模式"  TriggerMode="None">
                    <mi:ListItem  Value="DEFAULT" Text="" TextEx="--默认--" />
                    <mi:ListItem  Value="ICON_TEXT" Text="按钮图标模式" />
                    <mi:ListItem  Value="ICON" Text="图标模式"/>
                    <mi:ListItem  Value="TEXT" Text="文字模式"/> 
                </mi:SelectColumn>

                <mi:BoundField DataField="PLUG_CLASS" HeaderText="插件-类全名" Width="300"  />

                <mi:BoundField DataField="PLUG_METHOD" HeaderText="插件-函数名"  />
                <mi:BoundField DataField="PLUG_PARAMS" HeaderText="插件-参数" Width="200"  />

                <mi:BoundField DataField="ASK_MSG" HeaderText="点击前,提示信息"  />

                <mi:BoundField DataField="ON_BEFORE_CLICK" HeaderText="点击前,触发的脚本"  />

                <mi:BoundField DataField="ON_CLICK" HeaderText="点击事件脚本"  />

                <mi:BoundField DataField="COMMAND" HeaderText="服务器命令名称"  />
                <mi:BoundField DataField="COMMAND_PARAMS" HeaderText="服务器命令参数"  />


                <mi:BoundField DataField="ICON_PATH" HeaderText="图标文件路径" />

            </Columns>

        </mi:Table>
    </mi:Panel>

    <mi:WindowFooter ID="WindowFooter1" runat="server">
        <mi:Button runat="server" ID="SubmitBtn" Width="120" Height="26" Command="GoLast" Text="完成"  Dock="Center" />
    </mi:WindowFooter>

</mi:Viewport>

</form>