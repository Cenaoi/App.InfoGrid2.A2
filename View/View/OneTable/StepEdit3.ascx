<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StepEdit3.ascx.cs" Inherits="App.InfoGrid2.View.OneTable.StepEdit3" %>

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
<form action="" id="form1" method="post">

    <mi:Store runat="server" ID="storeTable" Model="IG2_TABLE" IdField="IG2_TABLE_ID">
        <FilterParams>
            <mi:QueryStringParam Name="IG2_TABLE_ID" QueryStringField="id" DbType="Int32" />
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
        </FilterParams>
        <UpdateParams>
            <mi:ServerParam Name="ROW_DATE_UPDATE" ServerField="TIME_NOW" />
        </UpdateParams>
    </mi:Store>

<mi:Store runat="server" ID="store1" Model="IG2_TABLE_COL" IdField="IG2_TABLE_COL_ID" SortField="FIELD_SEQ" PageSize="0" >
    <SelectQuery>
        <mi:QueryStringParam Name="IG2_TABLE_ID" QueryStringField="id" DbType="Int32" />
<%--        <mi:Param Name="SEC_LEVEL" DefaultValue="6" Logic="<" />--%>
        <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
    </SelectQuery>
    <UpdateParams>
        <mi:ServerParam Name="ROW_DATE_UPDATE" ServerField="TIME_NOW" />
    </UpdateParams>
</mi:Store>


<mi:Viewport runat="server" ID="viewport">

    <mi:TabPanel runat="server" ID="tabPanel1" UI="win10" TabLeft="10" Region="Center" ButtonVisible="false">

        <mi:Tab runat="server" ID="tab1" Text="字段集" Scroll="None">
            
        <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarButton Text="保存" OnClick="ser:store1.SaveAll()"  />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="上移" OnClick="ser:store1.MoveUp()" />
            <mi:ToolBarButton Text="下移" OnClick="ser:store1.MoveDown()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="重建排序" OnClick="ser:store1.SortReset()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="应用到全部视图" Command="GoApplyToViewAll" />

            <mi:ToolBarHr />

            <mi:ToolBarButton Text="添加视图字段" Command="GoAddVField" />
            <mi:ToolBarButton Text="删除视图字段" Command="GoDeleteVField" />

        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="true" StoreID="store1" Sortable="false" Dock="Full" PagerVisible="false" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="DB_FIELD" HeaderText="内部字段" EditorMode="None" />

                <mi:BoundField DataField="GROUP_ID" HeaderText="分组" />

                <mi:BoundField DataField="F_NAME" HeaderText="字段名" EditorMode="None" />
                <mi:BoundField DataField="DISPLAY" HeaderText="显示名" />
                <mi:SelectColumn DataField="DB_TYPE" HeaderText="数据类型" EditorMode="None" TriggerMode="None"  >
                    <mi:ListItem Value="string" Text="字符串" TextEx="字符串（例：张三）" />
                    <mi:ListItem Value="int" Text="整数" TextEx="整数（例：100）" />
                    <mi:ListItem Value="datetime" Text="日期" TextEx="日期（例：2012-12-30）" />
                    <mi:ListItem Value="boolean" Text="布尔型" TextEx="布尔型（例：True,False）" />
                    <mi:ListItem Value="currency" Text="货币" TextEx="货币（例：￥1,000.00）" />
                    <mi:ListItem Value="decimal" Text="数值" TextEx="数值（例：211.05）"  />
                </mi:SelectColumn>

                <mi:CheckColumn DataField="IS_VIEW_FIELD" HeaderText="视图字段" ItemAlign="Center" Width="60"  EditorMode="None"  />
                <mi:CheckColumn DataField="IS_BIZ_MANDATORY" HeaderText="业务必填" ItemAlign="Center" Width="60" />
                <mi:CheckColumn DataField="IS_READONLY" HeaderText="只读" ItemAlign="Center" Width="60"  />
                <mi:CheckColumn DataField="IS_VISIBLE" HeaderText="显示" ItemAlign="Center" Width="60"  />
                <mi:CheckColumn DataField="IS_LIST_VISIBLE" HeaderText="列表显示" ItemAlign="Center" Width="60"  />
                <mi:CheckColumn DataField="IS_SEARCH_VISIBLE" HeaderText="查询显示" ItemAlign="Center" Width="60"  />
                <mi:BoundField DataField="DEFAULT_VALUE" HeaderText="默认值" />
                                                
                <mi:NumColumn DataField="FIELD_SEQ" HeaderText="顺序" Format="0.000" />

                <mi:CheckColumn DataField="CAN_APPLY_VIEW" HeaderText="能应用到视图" Width="100" />

                <mi:ActionColumn AutoHide="true" >
                    <mi:ActionItem Text="应用到全部视图" Icon="/res/icon/01664.png" />
                </mi:ActionColumn>
            </Columns>
        </mi:Table>


        </mi:Tab>

        <mi:Tab runat="server" ID="tab2" Text="表属性" Scroll="None">

            <mi:FormLayout runat="server" ID="tablAttrFL" Dock="Full" StoreID="storeTable" ItemLabelWidth="200" ItemWidth="600" ItemLabelAlign="Right">

                <mi:Label runat="server" ID="label1" DataField="IG2_TABLE_ID" FieldLabel="IG2_TABLE_ID" />
                
                <mi:TextBox runat="server" ID="tb1" DataField="TABLE_NAME" FieldLabel="内部表名" ReadOnly="true" />
                <mi:TextBox runat="server" ID="TextBox1" DataField="TABLE_NAME" FieldLabel="内部表名" ReadOnly="true" />

                <mi:TextBox runat="server" ID="TextBox2" DataField="DISPLAY" FieldLabel="显示名" />
                <mi:TextBox runat="server" ID="TextBox3" DataField="ID_FIELD" FieldLabel="主键字段名" />
                <mi:TextBox runat="server" ID="TextBox4" DataField="IDENTITY_FIELD" FieldLabel="自增长字段名" />
                <mi:TextBox runat="server" ID="TextBox5" DataField="USER_SEQ_FIELD" FieldLabel="用户排序字段名" />
                <mi:TextBox runat="server" ID="TextBox6" DataField="LOCKED_FIELD" FieldLabel="锁行字段名" />
                <mi:TextBox runat="server" ID="TextBox7" DataField="REMARK" FieldLabel="备注" />

                <mi:CheckBox runat="server" ID="CheckBox6" DataField="IS_BIG_TITLE_VISIBLE" FieldLabel="顶部大标题" TrueText="显示" FalseText="隐藏" />

                <mi:CheckBox runat="server" ID="CheckBox1" DataField="ATTACH_FILE_VISIBLE" FieldLabel="附件上传" TrueText="显示" FalseText="隐藏" />

                <div class="mi-newline" style="padding-left:20px;">
                    <div class="mi-newline-text">流程信息</div>
                </div>

                <mi:CheckBox runat="server" ID="CheckBox2" DataField="FLOW_ENABLED" FieldLabel="流程状态" TrueText="激活" FalseText="关闭" />
                <mi:Textarea runat="server" ID="textarea1" DataField="FLOW_PARAMS" FieldLabel="流程参数"  />


                <div class="mi-newline" style="padding-left:20px;">
                    <div class="mi-newline-text">表单信息</div>
                </div>

                <mi:CheckBox runat="server" ID="CheckBox3" DataField="VISIBLE_BTN_EDIT" FieldLabel="编辑按钮" TrueText="显示" FalseText="隐藏" />
                <mi:ComboBox runat="server" ID="combox1" DataField="FORM_NEW_TYPE" FieldLabel="新建表单的类型" TriggerMode="None">
                    <mi:ListItem Value="" Text="--空--" />
                    <mi:ListItem Value="ONE_FORM" Text="表单" />
                    <mi:ListItem Value="TABLE_FORM" Text="单表-表单" />
                </mi:ComboBox>
                <mi:NumberBox runat="server" ID="num2" DataField="FORM_NEW_PAGEID" FieldLabel="新建表单的 ID" />
                <mi:TextBox runat="server" ID="textbox10" DataField="FORM_NEW_ALIAS_TITLE" FieldLabel="新建的标题别名" />

                <mi:ComboBox runat="server" ID="ComboBox1" DataField="FORM_EDIT_TYPE" FieldLabel="编辑表单的类型"  TriggerMode="None">
                    <mi:ListItem Value="" Text="--空--" />
                    <mi:ListItem Value="ONE_FORM" Text="表单" />
                    <mi:ListItem Value="TABLE_FORM" Text="单表-表单" />
                </mi:ComboBox>
                <mi:NumberBox runat="server" ID="NumberBox1" DataField="FORM_EDIT_PAGEID" FieldLabel="编辑表单的 ID" />
                <mi:TextBox runat="server" ID="TextBox8" DataField="FORM_EDIT_ALIAS_TITLE" FieldLabel="编辑标题别名" />


                <mi:CheckBox runat="server" ID="CheckBox4" DataField="FORM_BIG_BIZSID_VISIBLE" FieldLabel="左上角的大印章" TrueText="显示" FalseText="隐藏" />
                <mi:CheckBox runat="server" ID="CheckBox5" DataField="FORM_RT_VISIBLE" FieldLabel="右上角的状态" TrueText="显示" FalseText="隐藏" />
                <mi:NumberBox runat="server" ID="NumberBox3" DataField="FORM_WIDTH" FieldLabel="表单容器的宽度" />
                <mi:NumberBox runat="server" ID="NumberBox2" DataField="FORM_ITEM_LABLE_WIDTH" FieldLabel="项目的标签宽度" />
                <mi:ComboBox runat="server" ID="ComboBox3" DataField="FORM_ALIGN" FieldLabel="表单的位置"  TriggerMode="None">
                    <mi:ListItem Value="" Text="--默认左对齐--" />
                    <mi:ListItem Value="left" Text="左对齐" />
                    <mi:ListItem Value="center" Text="居中" />
                    <mi:ListItem Value="rigth" Text="右对齐" />
                </mi:ComboBox>

                
                <div dock="top" style="height:100px; border:none; text-align:center; vertical-align:bottom; padding:20px; color:#808080;">
                    ---------------我是底线------------------
                </div>

            </mi:FormLayout>

        </mi:Tab>

    </mi:TabPanel>
    <mi:WindowFooter ID="WindowFooter1" runat="server">
        <mi:Button runat="server" ID="Button1" Width="80" Height="26" Command="GoLast" Text="完成" Dock="Center" />
        <mi:Button runat="server" ID="Button2" Width="80" Height="26" Command="GoCancel" Text="取消" Dock="Center" />
    </mi:WindowFooter>
</mi:Viewport>

</form>

<script type="text/javascript">

    function queryDelete(owner) {
        var me = owner;

        Mini2.Msg.prompt("询问", "将影响到整个系统的界面 , 确定应用到全部视图?", function () {
            me.click();
        });

        return false;
    }


</script>