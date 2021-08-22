<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditValue.ascx.cs" Inherits="App.InfoGrid2.View.OneValue.EditValue" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<form action="" method="post">

    <mi:Store runat="server" ID="store1" Model="IG2_VALUE_TABLE" IdField="IG2_VALUE_TABLE_ID" PageSize="0">
        <SelectQuery>
            <mi:QueryStringParam Name="IG2_VALUE_TABLE_ID" QueryStringField="id" DbType="Int32" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </SelectQuery>
    </mi:Store>

    <mi:Store runat="server" ID="store2" Model="IG2_VALUE_IF" IdField="IG2_VALUE_IF_ID" PageSize="0">
        <SelectQuery>
            <mi:QueryStringParam Name="IG2_VALUE_TABLE_ID" QueryStringField="id" DbType="Int32" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </SelectQuery>
        <InsertParams>
            <mi:QueryStringParam Name="IG2_VALUE_TABLE_ID" QueryStringField="id" />
            <mi:Param Name="F_LOGIC" DefaultValue="==" />
        </InsertParams>
        <DeleteQuery>
            <mi:ControlParam Name="IG2_VALUE_IF_ID" ControlID="table2" PropertyName="CheckedRows" />
        </DeleteQuery>
    </mi:Store>

    <mi:Store runat="server" ID="store3" Model="IG2_VALUE_THEN" IdField="IG2_VALUE_THEN_ID" PageSize="0" DeleteRecycle="true">
        <FilterParams>
            <mi:StoreCurrentParam ControlID="store2" Name="IG2_VALUE_IF_ID" PropertyName="IG2_VALUE_IF_ID" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
        <InsertParams>
            <mi:StoreCurrentParam ControlID="store2" Name="IG2_VALUE_IF_ID" PropertyName="IG2_VALUE_IF_ID" />
        </InsertParams>
        <InsertParams>
            <mi:QueryStringParam Name="IG2_VALUE_TABLE_ID" QueryStringField="id" />
            <mi:Param Name="F_LOGIC" DefaultValue="EVERY" />
        </InsertParams>
        <DeleteQuery>
            <mi:ControlParam Name="IG2_VALUE_THEN_ID" ControlID="table3" PropertyName="CheckedRows" />
        </DeleteQuery>
        <DeleteRecycleParams>
            <mi:Param Name="ROW_SID" DefaultValue="-3" />
            <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
        </DeleteRecycleParams>
    </mi:Store>


     <mi:Store runat="server" ID="store4" Model="IG2_VALUE_TRIGGER" IdField="IG2_VALUE_TRIGGER_ID" PageSize="0" DeleteRecycle="true">
        <FilterParams>
            <mi:StoreCurrentParam ControlID="store2" Name="IG2_VALUE_IF_ID" PropertyName="IG2_VALUE_IF_ID" />
            <mi:Param  Name="ROW_SID" DefaultValue="0" Logic=">="/>
        </FilterParams>
        <InsertParams>
            <mi:StoreCurrentParam ControlID="store2" Name="IG2_VALUE_IF_ID" PropertyName="IG2_VALUE_IF_ID" />
        </InsertParams>
        <InsertParams>
            <mi:QueryStringParam Name="IG2_VALUE_TABLE_ID" QueryStringField="id" />
        </InsertParams>
        <DeleteQuery>
            <mi:ControlParam Name="IG2_VALUE_TRIGGER_ID" ControlID="table4" PropertyName="CheckedRows" />
        </DeleteQuery>
        <DeleteRecycleParams>
            <mi:Param Name="ROW_SID" DefaultValue="-3" />
            <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />
        </DeleteRecycleParams>
    </mi:Store>


    <mi:Viewport runat="server" ID="viewport1" Main="true">
        <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="North" Scroll="None" Height="80">
            <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full"  PagerVisible="false">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField HeaderText="表名" DataField="TABLE_NAME" />
                    <mi:BoundField HeaderText="表描述" DataField="TABLE_DISPAY" />
                    <mi:BoundField HeaderText="备注" DataField="REMARKS" />
                    <mi:CheckColumn HeaderText="激活" DataField="ENABLED" />
                </Columns>
            </mi:Table>
        </mi:Panel>

        <mi:Panel runat="server" ID="Panel1" Dock="Full" Region="West" Scroll="None" Width="600">
            <mi:Toolbar ID="Toolbar1" runat="server">

                <mi:ToolBarTitle ID="tableNameTB1" Text="逻辑条件" />

                <mi:ToolBarButton Text="新增" OnClick="ser:store2.Insert()" />
                <mi:ToolBarButton Text="保存" OnClick="ser:store2.SaveAll()" />
                <mi:ToolBarButton Text="刷新" OnClick="ser:store2.Refresh()" />
                <mi:ToolBarHr />
                <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?" Command="DeleteAll" />

            </mi:Toolbar>
            <mi:Table runat="server" ID="table2" StoreID="store2" Dock="Full" PagerVisible="false">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />
                    <mi:BoundField HeaderText="字段描述" DataField="F_DISPAY" />
                    <mi:BoundField HeaderText="字段名" DataField="F_NAME" />
                    <%--<mi:BoundField HeaderText="原值" DataField="VALUE_FROM" />--%>
                    <mi:SelectColumn DataField="F_LOGIC" HeaderText="逻辑" TriggerMode="None" Width="80" DropDownWidth="120">
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
                    <mi:BoundField HeaderText="值变化为" DataField="VALUE_TO" />
                    <mi:BoundField HeaderText="组名" DataField="F_GROUP" Width="60" />
                </Columns>
            </mi:Table>
        </mi:Panel>

        <mi:Panel runat="server" ID="Panel2" Dock="Full" Region="Center" Scroll="None" Width="600">

             <mi:Panel runat="server" ID="Panel4" Dock="Top" Scroll="None" Width="600" Height="200">
                <mi:Toolbar ID="Toolbar3" runat="server">

                    <mi:ToolBarTitle ID="ToolBarTitle2" Text="触发字段" />

                    <mi:ToolBarButton Text="新增" OnClick="ser:store4.Insert()" />
                    <mi:ToolBarButton Text="保存" OnClick="ser:store4.SaveAll()" />
                    <mi:ToolBarButton Text="刷新" OnClick="ser:store4.Refresh()" />
                    <mi:ToolBarHr />
                    <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?" OnClick="ser:store4.Delete()" />

                </mi:Toolbar>
                <mi:Table runat="server" ID="table4" StoreID="store4" Dock="Full" PagerVisible="false">
                    <Columns>
                        <mi:RowNumberer />
                        <mi:RowCheckColumn />
                        <mi:BoundField HeaderText="字段描述" DataField="F_DISPAY" />
                        <mi:BoundField HeaderText="字段名" DataField="F_NAME" />
                    </Columns>
                </mi:Table>
            </mi:Panel>


            <mi:Panel runat="server" ID="panel3"  Dock="Full" Scroll="None">
                <mi:Toolbar ID="Toolbar2" runat="server">

                    <mi:ToolBarTitle ID="ToolBarTitle1" Text="执行动作" />

                    <mi:ToolBarButton Text="新增" OnClick="ser:store3.Insert()" />
                    <mi:ToolBarButton Text="保存" OnClick="ser:store3.SaveAll()" />
                    <mi:ToolBarButton Text="刷新" OnClick="ser:store3.Refresh()" />
                    <mi:ToolBarHr />
                    <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?" OnClick="ser:store3.Delete()" />

                </mi:Toolbar>
                <mi:Table runat="server" ID="table3" StoreID="store3" Dock="Full" PagerVisible="false">
                    <Columns>
                        <mi:RowNumberer />
                        <mi:RowCheckColumn />
                        <mi:BoundField HeaderText="字段描述" DataField="F_DISPAY" />
                        <mi:BoundField HeaderText="字段名" DataField="F_NAME" />
                        <mi:SelectColumn DataField="F_LOGIC" HeaderText="逻辑" TriggerMode="None" Width="80" DropDownWidth="120">
                            <mi:ListItem Value="IF_NULL" Text="如果为空" />
                            <mi:ListItem Value="EVERY" Text="每次" />
                        </mi:SelectColumn>
                        <mi:BoundField HeaderText="值等于" DataField="VALUE_TO" Width="300" />
                    </Columns>
                </mi:Table>

            </mi:Panel>

        </mi:Panel>


        <mi:WindowFooter ID="WindowFooter1" runat="server">
            <mi:Button runat="server" ID="Button1" Width="80" Height="26" Command="GoLast" Text="完成" Dock="Center" />
        </mi:WindowFooter>

    </mi:Viewport>


</form>
