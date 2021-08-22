<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DwgSetupOperate.ascx.cs" Inherits="App.InfoGrid2.View.MoreActionBuilder.DwgSetupOperate" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %> 

    <link href="/Core/Scripts/jquery.ui/themes/ui-lightness/jquery-ui.css" rel="stylesheet" />
    <script src="/Core/Scripts/jquery.ui/ui/custom/jquery-ui.js"></script>


    <link href="/Core/Scripts/Mini2.Filter/Themes/theme-globel.css" rel="stylesheet" />

    <script src="/Core/Scripts/Mini2.Filter/dev/Item.js"></script>
    <script src="/Core/Scripts/Mini2.Filter/dev/FilterElem.js"></script>
    <script src="/Core/Scripts/Mini2.Filter/dev/FilterGroup.js"></script>
    <script src="/Core/Scripts/Mini2.Filter/dev/Panel.js"></script>



    <link href="/Core/Scripts/codemirror/codemirror-5.21.0/codemirror.css" rel="stylesheet" />
    <script src="/Core/Scripts/codemirror/codemirror-5.21.0/codemirror.js"></script>
    <script src="/Core/Scripts/codemirror/codemirror-5.21.0/mode/clike/clike.js"></script>

<style>

        .mi-cont {
            border: 1px solid #eee;
            min-width: 240px;
            min-height: 60px;
            list-style-type: none;
            margin: 0;
            padding: 5px 0 0;
            float: left;
            
            background-color:#FFFFFF;
        }

        .mi-cont li {
            margin: 0 5px 5px 5px;
            padding: 5px;
            font-size: 12px;
            display:table;
            min-width:220px;
        }

        .mi-andor{
        }

        
        .mi-andor .mi-text{
            padding:4px;
            display:table;
        }


        .mi-andor  ul{
            min-width:240px;
        }
    </style>

<form action="" method="post">

    <mi:Store runat="server" ID="store3" Model="AC3_DWG_NODE" IdField="AC3_DWG_NODE_ID" DeleteRecycle="true" >
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:QueryStringParam Name="FK_DWG_CODE" QueryStringField="dwg_code" />
            <mi:QueryStringParam Name="PK_NODE_CODE" QueryStringField="node_code" />
        </FilterParams>
    </mi:Store>

    <mi:Store runat="server" ID="store1" Model="AC3_OPERATE_TABLE" IdField="AC3_OPERATE_TABLE_ID" DeleteRecycle="true" >
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:QueryStringParam Name="FK_DWG_CODE" QueryStringField="dwg_code" />
            <mi:QueryStringParam Name="FK_NODE_CODE" QueryStringField="node_code" />
        </FilterParams>
    </mi:Store>

    <!--新建的字段赋值-->
    <mi:Store runat="server" ID="storeOpInsert" Model="AC3_OPERATE_TABLE_FIELD" IdField="AC3_OPERATE_TABLE_FIELD_ID" PageSize="0" DeleteRecycle="true">
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:QueryStringParam Name="FK_DWG_CODE" QueryStringField="dwg_code" />
            <mi:QueryStringParam Name="FK_NODE_CODE" QueryStringField="node_code" />
            <mi:Param Name="METHOD_TYPE" DefaultValue="insert" />
        </FilterParams>
        <InsertParams>
            <mi:QueryStringParam Name="FK_DWG_CODE" QueryStringField="dwg_code" />
            <mi:QueryStringParam Name="FK_NODE_CODE" QueryStringField="node_code" />
            <mi:QueryStringParam Name="FK_OPERATE_CODE" QueryStringField="node_code" />
            <mi:Param Name="DB_LOGIC" DefaultValue="=" />
            <mi:Param Name="VALUE_TYPE" DefaultValue="fixed" />
            <mi:Param Name="METHOD_TYPE" DefaultValue="insert" />
        </InsertParams>
        <DeleteQuery>
            <mi:ControlParam Name="AC3_OPERATE_TABLE_FIELD_ID" ControlID="tableOpInsert" PropertyName="CheckedRows" />
        </DeleteQuery>
        <DeleteRecycleParams>
            <mi:Param Name="ROW_SID" DefaultValue="-3" />
            <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
        </DeleteRecycleParams>
    </mi:Store>

    <!--更新的字段赋值-->
    <mi:Store runat="server" ID="storeOpUpdate" Model="AC3_OPERATE_TABLE_FIELD" IdField="AC3_OPERATE_TABLE_FIELD_ID" PageSize="0" DeleteRecycle="true">
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:QueryStringParam Name="FK_DWG_CODE" QueryStringField="dwg_code" />
            <mi:QueryStringParam Name="FK_NODE_CODE" QueryStringField="node_code" />
            <mi:Param Name="METHOD_TYPE" DefaultValue="update" />
        </FilterParams>
        <InsertParams>
            <mi:QueryStringParam Name="FK_DWG_CODE" QueryStringField="dwg_code" />
            <mi:QueryStringParam Name="FK_NODE_CODE" QueryStringField="node_code" />
            <mi:QueryStringParam Name="FK_OPERATE_CODE" QueryStringField="node_code" />
            <mi:Param Name="DB_LOGIC" DefaultValue="=" />
            <mi:Param Name="VALUE_TYPE" DefaultValue="fixed" />
            <mi:Param Name="METHOD_TYPE" DefaultValue="update" />
        </InsertParams>
        <DeleteQuery>
            <mi:ControlParam Name="AC3_OPERATE_TABLE_FIELD_ID" ControlID="tableUpdate" PropertyName="CheckedRows" />
        </DeleteQuery>
        <DeleteRecycleParams>
            <mi:Param Name="ROW_SID" DefaultValue="-3" />
            <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
        </DeleteRecycleParams>
    </mi:Store>



    <mi:Viewport runat="server" ID="viewport1" MarginTop="10">
        <mi:FormLayout runat="server" ID="mianForm1" AutoSize="true" StoreID="store3" ItemLabelAlign="Right" FlowDirection="LeftToRight" ItemWidth="400" >
            
            <mi:TextBox runat="server" ID="TextBox1" DataField="NODE_IDENTIFIER" FieldLabel="ID" />
            <mi:TextBox runat="server" ID="NODE_TEXT_tb3" DataField="NODE_TEXT" FieldLabel="节点名称" />
            <mi:Label runat="server" ID="PK_NODE_CODE_lab" DataField="PK_NODE_CODE" FieldLabel="节点内部编码" />

            <%--<mi:Textarea runat="server" ID="remarkTT" DataField="REMARK" FieldLabel="备注" />--%>
            <div style="height:10px;"></div>
        </mi:FormLayout>
        <mi:TabPanel runat="server" ID="tabPanel1" Dock="Full" UI="win10" TabLeft="10" ButtonVisible="false" Region="Center">

            <mi:Tab runat="server" Text="操作" >

                <mi:FormLayout runat="server" ID="formLayout1" FlowDirection="TopDown" StoreID="store1" Dock="Top" AutoSize="true" 
                    ItemWidth="400" ItemLabelAlign="Right" Padding="10">
                   
                    <mi:ComboBox runat="server" ID="ComboBox1" DataField="OP_METHOD" TriggerMode="None" FieldLabel="操作方式" 
                        StringItems="update=更新;insert=新建;delete=删除;select=查询;" />

                    <%--                    
                    <mi:RadioGroup runat="server" ID="ComboBox2" DataField="OP_METHOD" TriggerMode="None" FieldLabel="操作方式" 
                        StringItems="update=更新;insert=新建;delete=删除;" >

                        <mi:ListItem Value="update" Text="更新" />
                        <mi:ListItem Value="insert" Text="新建" />
                        <mi:ListItem Value="delete" Text="删除" />
                        
                    </mi:RadioGroup>
                    --%>
                    
                    <mi:TriggerBox runat="server" ID="DB_TABLE_tb" DataField="DB_TABLE" FieldLabel="数据表名" ButtonType="More" />
                    <mi:TextBox runat="server" ID="DB_TABLE_TEXT_tb" DataField="DB_TABLE_TEXT" FieldLabel="数据表描述" />

                    <mi:ComboBox runat="server" ID="ACTION_TYPE_cb" DataField="ACTION_TYPE" MaxWidth="300px" FieldLabel="操作脚本类型" 
                        StringItems="none=没有;xml=XML;cs=C# 脚本; json=JSON 图形" TriggerMode="None" />                   
                    
                    <mi:ComboBox runat="server" ID="ComboBox2" DataField="COND_SCRIPT_TYPE" MaxWidth="300px" FieldLabel="过滤脚本类型" 
                        StringItems="none=没有;xml=XML;cs=C# 脚本; json=JSON 图形" TriggerMode="None" />              
                    
                </mi:FormLayout>

                <mi:TabPanel runat="server" ID="tabPanelFields" Dock="Top" Height="340" UI="win10" ButtonVisible="false">
                    <mi:Tab runat="server" ID="tabNewFields" Text="新建的字段" Scroll="None">
                        <mi:Toolbar runat="server" ID="toolbar1">
                            <mi:ToolBarTitle ID="tableNameTB1" Text="赋值" />

                            <mi:ToolBarButton Text="新增" OnClick="ser:storeOpInsert.Insert()" />
                            <mi:ToolBarButton Text="保存" OnClick="ser:storeOpInsert.SaveAll()" />
                            <mi:ToolBarButton Text="刷新" OnClick="ser:storeOpInsert.Refresh()" />
                            <mi:ToolBarHr />
                            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:storeOpInsert.Delete()" />

                        
                        </mi:Toolbar>

                        <mi:Table runat="server" ID="tableOpInsert" StoreID="storeOpInsert"  Dock="Full" JsonMode="Full" Height="0" MinHeight="150" MaxHeight="300" PagerVisible="false">
                            <Columns>
                                <mi:RowNumberer />
                                <mi:RowCheckColumn />

                                <mi:TriggerColumn HeaderText="字段名" DataField="DB_FIELD" ButtonType="More"  />
                                <mi:BoundField HeaderText="字段描述" DataField="DB_FIELD_TEXT"  />
                        
                                <mi:SelectColumn HeaderText="逻辑" DataField="DB_LOGIC" TriggerMode="None">
                                    <mi:ListItem Value="" Text="" TextEx="-- N/A --" />
                                    <mi:ListItem Value="=" Text="等于" />
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

                                <mi:TriggerColumn HeaderText="值" DataField="DB_VALUE" ButtonType="More" ></mi:TriggerColumn>

        <%--                        <mi:TriggerColumn HeaderText="值 2" DataField="DB_VALUE_2" ButtonType="More" ></mi:TriggerColumn>--%>

                                <mi:SelectColumn HeaderText="值类型" DataField="VALUE_TYPE" StringItems="fixed=固定值;fun=函数值;" TriggerMode="None" >
                                </mi:SelectColumn>
                                                
        <%--                        <mi:SelectColumn HeaderText="值类型" DataField="VALUE_TYPE_2" StringItems="N/A=默认;fixed=固定值;fun=函数值;" TriggerMode="None" >
                                </mi:SelectColumn>--%>
                        
                                <mi:BoundField HeaderText="备注" DataField="REMARK" Width="300" />
                            </Columns>
                        </mi:Table>

                    </mi:Tab>
                    <mi:Tab runat="server" ID="tab1" Text="更新的字段集"  Scroll="None" Left="100">
                        <mi:Toolbar runat="server" ID="toolbar3">

                            <mi:ToolBarButton Text="新增" OnClick="ser:storeOpUpdate.Insert()" />
                            <mi:ToolBarButton Text="保存" OnClick="ser:storeOpUpdate.SaveAll()" />
                            <mi:ToolBarButton Text="刷新" OnClick="ser:storeOpUpdate.Refresh()" />
                            <mi:ToolBarHr />
                            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:storeOpUpdate.Delete()" />

                        
                        </mi:Toolbar>

                        <mi:Table runat="server" ID="tableUpdate" StoreID="storeOpUpdate"  Dock="Full" JsonMode="Full" Height="0" MinHeight="150" MaxHeight="300" PagerVisible="false">
                            <Columns>
                                <mi:RowNumberer />
                                <mi:RowCheckColumn />

                                <mi:TriggerColumn HeaderText="字段名" DataField="DB_FIELD" ButtonType="More"  />
                                <mi:BoundField HeaderText="字段描述" DataField="DB_FIELD_TEXT"  />
                        
                                <mi:SelectColumn HeaderText="逻辑" DataField="DB_LOGIC" TriggerMode="None">
                                    <mi:ListItem Value="" Text="" TextEx="-- N/A --" />
                                    <mi:ListItem Value="=" Text="等于" />
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

                                <mi:TriggerColumn HeaderText="值" DataField="DB_VALUE" ButtonType="More" ></mi:TriggerColumn>

        <%--                        <mi:TriggerColumn HeaderText="值 2" DataField="DB_VALUE_2" ButtonType="More" ></mi:TriggerColumn>--%>

                                <mi:SelectColumn HeaderText="值类型" DataField="VALUE_TYPE" StringItems="fixed=固定值;fun=函数值;" TriggerMode="None" >
                                </mi:SelectColumn>
                                                
        <%--                        <mi:SelectColumn HeaderText="值类型" DataField="VALUE_TYPE_2" StringItems="N/A=默认;fixed=固定值;fun=函数值;" TriggerMode="None" >
                                </mi:SelectColumn>--%>
                        
                                <mi:BoundField HeaderText="备注" DataField="REMARK" Width="300" />
                            </Columns>
                        </mi:Table>

                    </mi:Tab>
                </mi:TabPanel>
                
                
                <!--监听条件-->
                <mi:Panel runat="server" ID="panel3" Dock="Top" Height="400" MarginTop="4px" Scroll="None" >

                    <mi:Toolbar runat="server" ID="toolbar2">
                        <mi:ToolBarTitle Text="过滤条件" />

                        <mi:ToolBarButton Text="新增过滤" OnClick="filrePanel1.insertItem()" />
                        <mi:ToolBarButton Text="新增 AND" OnClick="filrePanel1.insertAnd()" />
                        <mi:ToolBarButton Text="新增 OR"  OnClick="filrePanel1.insertOr()" />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="保存" OnClick="save_filter()" />
                        <mi:ToolBarButton Text="刷新" OnClick="refresh_filter()"  />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  />
                        
                    </mi:Toolbar>

                    <div data-dock="full" style="overflow:auto; padding:10px;" class="flow-filter1">
                        <table class="mi-newline" style="min-width:600px;">
                            <tr>
                                <td style="width:80px; vertical-align:top;"><div class="" style="text-align:right;padding:4px;">过滤:</div></td>
                                <td>
                                    <ul id="filterPanel1"  class=" mi-newline connectedSortable mi-cont mi-filter-inner filter-where" style="border-color:#b6b6b6;">
                                        
                                    </ul>
                                </td>
                            </tr>
                        </table>

                        <!--字段集合-->
                        <table class="mi-newline"  style="width:100%;">
                            <tr>
                                <td style="width:80px; vertical-align:top;"><div class="" style="text-align:right;padding:4px;">字段集:</div></td>
                                <td>
                                    <ul class="order mi-cont mi-filter-inner mi-newline filter-fields"  style="min-height:20px; border-color:#b6b6b6;">
                                        <li style="padding:0px;"><input type="text" class="filter-fields-text" style="width:400px;" /></li>
                                    </ul>
                                </td>
                            </tr>
                        </table>

                        <!--排序-->
                        <table class="mi-newline"  style="width:600px;">
                            <tr>
                                <td style="width:80px; vertical-align:top;"><div class="" style="text-align:right;padding:4px;">排序:</div></td>
                                <td>
                                    <ul class="order mi-cont mi-filter-inner mi-newline filter-order"  style="min-height:20px; border-color:#b6b6b6;">
                                        <li><input type="text" class="filter-order-text" style="width:400px;" /></li>
                                        
                                    </ul>
                                </td>
                            </tr>
                        </table>
                        <table class="mi-newline" style="width:300px;">
                            <tr>
                                <td style="width:80px; vertical-align:top;"><div class="" style="text-align:right;padding:4px;">范围:</div></td>
                                <td>
                                    <input type="number" style="width:80px;" placeholder="页数量" class="filter-limit-count" /> - <input type="number" style="width:80px;" class="filter-limit-start" placeholder="起始行" />
                                </td>
                            </tr>
                        </table>
                        <table class="mi-newline"  style="width:300px;">
                            <tr>
                                <td style="width:80px; vertical-align:top;"><div class="" style="text-align:right;padding:4px;">去重:</div></td>
                                <td>
                                    <label><input type="checkbox" class="filter-distinct " /> 激活</label>
                                    
                                </td>
                            </tr>
                        </table>

                    </div>


                </mi:Panel>
                <div dock="top" style="height:100px; border:none; text-align:center; vertical-align:bottom; padding:20px; color:#808080;">
                    ---------------我是底线------------------
                </div>
            </mi:Tab>
            <mi:Tab runat="server" ID="tabInsert1" Text="新建前-脚本">
                <mi:FormLayout runat="server" ID="formLayout3" FlowDirection="TopDown"  StoreID="store1" AutoSize="true" Padding="10" ItemLabelAlign="Right">
                    <mi:CodeEditor runat="server" ID="CodeEditor1" DataField="INSERTING_SCRIPT" FieldLabel="脚本" Height="600"/>
                </mi:FormLayout>
            </mi:Tab>
            <mi:Tab runat="server" ID="tabInsert2" Text="新建后-脚本">
                <mi:FormLayout runat="server" ID="formLayout4" FlowDirection="TopDown"  StoreID="store1" AutoSize="true" Padding="10" ItemLabelAlign="Right">
                    <mi:CodeEditor runat="server" ID="CodeEditor2" DataField="INSERTED_SCRIPT" FieldLabel="脚本" Height="600"/>
                </mi:FormLayout>
            </mi:Tab>
            <mi:Tab runat="server" ID="tabUpdate1" Text="更新前-脚本">
                <mi:FormLayout runat="server" ID="formLayout5" FlowDirection="TopDown"  StoreID="store1" AutoSize="true" Padding="10" ItemLabelAlign="Right">
                    <mi:CodeEditor runat="server" ID="CodeEditor3" DataField="UPDATING_SCRIPT" FieldLabel="脚本" Height="600"/>
                </mi:FormLayout>
            </mi:Tab>
            <mi:Tab runat="server" ID="tabUpdate2" Text="更新后-脚本">
                <mi:FormLayout runat="server" ID="formLayout6" FlowDirection="TopDown"  StoreID="store1" AutoSize="true" Padding="10" ItemLabelAlign="Right">
                    <mi:CodeEditor runat="server" ID="CodeEditor4" DataField="UPDATED_SCRIPT" FieldLabel="脚本" Height="600"/>
                </mi:FormLayout>
            </mi:Tab>
            <mi:Tab runat="server" ID="tabDelete1" Text="删除前-脚本">
                <mi:FormLayout runat="server" ID="formLayout7" FlowDirection="TopDown"  StoreID="store1" AutoSize="true" Padding="10" ItemLabelAlign="Right">
                    <mi:CodeEditor runat="server" ID="CodeEditor5" DataField="DELETING_SCRIPT" FieldLabel="脚本" Height="600"/>
                </mi:FormLayout>
            </mi:Tab>
            <mi:Tab runat="server" ID="tabDelte2" Text="删除后-脚本">
                <mi:FormLayout runat="server" ID="formLayout8" FlowDirection="TopDown"  StoreID="store1" AutoSize="true" Padding="10" ItemLabelAlign="Right">
                    <mi:CodeEditor runat="server" ID="CodeEditor6" DataField="DELETED_SCRIPT" FieldLabel="脚本" Height="600"/>
                </mi:FormLayout>
            </mi:Tab>
            <mi:Tab runat="server" ID="tabSelect1" Text="查询前-脚本">
                <mi:FormLayout runat="server" ID="formLayout9" FlowDirection="TopDown"  StoreID="store1" AutoSize="true" Padding="10" ItemLabelAlign="Right">
                    <mi:CodeEditor runat="server" ID="CodeEditor7" DataField="SELECTING_SCRIPT" FieldLabel="脚本" Height="600"/>
                </mi:FormLayout>
            </mi:Tab>
            <mi:Tab runat="server" ID="tabSelect2" Text="查询后-脚本">
                <mi:FormLayout runat="server" ID="formLayout10" FlowDirection="TopDown"  StoreID="store1" AutoSize="true" Padding="10" ItemLabelAlign="Right">
                    <mi:CodeEditor runat="server" ID="CodeEditor8" DataField="SELECTED_SCRIPT" FieldLabel="脚本" Height="600"/>
                </mi:FormLayout>
            </mi:Tab>


            <mi:Tab runat="server" ID="tab2" Text="其它">

                <mi:FormLayout runat="server" ID="formLayout2" FlowDirection="TopDown"  StoreID="store1" AutoSize="true" Padding="10" ItemLabelAlign="Right">

                    
                    <mi:CodeEditor runat="server" ID="ACTION_CS_tb2" DataField="ACTION_CS" FieldLabel="C# 数据内容" Height="600"/>

                    <mi:TextBox runat="server" ID="ACTION_XML_tb" DataField="ACTION_XML" FieldLabel="XML 数据内容" />
                    <mi:Textarea runat="server" ID="REMARK_tb" DataField="REMARK" FieldLabel="备注" />

                </mi:FormLayout>

            </mi:Tab>

        </mi:TabPanel>


        <mi:WindowFooter runat="server" ID="footer1">
            <mi:Button runat="server" Text="确定" Width="80" Dock="Center" />
            <mi:Button runat="server" Text="取消" Width="80" Dock="Center" OnClick="ownerWindow.close()" />
        </mi:WindowFooter>
    </mi:Viewport>
</form>

<%--<script src="/Core/Scripts/EaselJs/easeljs-NEXT.combined.js"></script>--%>

<%--<script src="/Core/Scripts/EaselJs/tweenjs-NEXT.combined.js"></script>--%>

<script src="/Core/Scripts/Tween/tween.js"></script>

<script>

    var m_IntN = 0;

    $(function () {

        requestAnimationFrame(animate);

        function animate(time) {
            requestAnimationFrame(animate);
            TWEEN.update(time);
        }

    });

    Mini2.ready(function () {


        var filterPanel = Mini2.create('Mini2.ui.filter.Panel', {
            el: $('.flow-filter1 .filter-where'),
            orderEl: $('.flow-filter1 .filter-order'),  //排序的元素
            limitCountEl: $('.flow-filter1 .filter-limit-count'),
            limitStartEl: $('.flow-filter1 .filter-limit-start'),
            distinctEl: $('.flow-filter1 .filter-distinct'),
            fieldsEl: $('.flow-filter1 .filter-fields')
        });

        filterPanel.render();

        filterPanel.on('itemDblclick', function (evt) {

            var item = evt.item;

            var url = Mini2.urlAppend('/App/InfoGrid2/View/MoreActionBuilder/DwgFilterItemSetup.aspx', {
            }, true);

            var win = Mini2.createTop('Mini2.ui.Window', {

                text: '条件设置',
                mode: true,

                width: 800,
                height: 600,

                userData: item.getData(),
                url: url
            });


            win.formClosing(function (e) {
                if (e.success) {
                    var ud = this.userData;
                    evt.item.setData(ud);
                }
            });

            win.show();

        });



        window.filrePanel1 = filterPanel;



        refresh_filter();
    });


    function save_filter() {

        var dwgCode = $.query.get('dwg_code');
        var nodeCode = $.query.get('node_code');

        var data = filrePanel1.getJson();


        var url = Mini2.urlAppend('/View/MoreActionBuilder/DwgHandler.ashx', {
            action: 'SAVE_OPERATE_FILTER'
        }, true);

        Mini2.post(url, {
            dwg_code: dwgCode,
            item_code: nodeCode,
            data: Mini2.Json.toJson(data)
        },
        function () {

            Mini2.toast("保存成功");

        });

    }

    function refresh_filter() {

        var dwgCode = $.query.get('dwg_code');
        var nodeCode = $.query.get('node_code');

        var url = Mini2.urlAppend('/View/MoreActionBuilder/DwgHandler.ashx', {
            action: 'GET_OPERATE_FILTER'
        }, true);

        Mini2.post(url, {
            dwg_code: dwgCode,
            item_code: nodeCode
        },
        function (data) {
            
            if (Mini2.String.isBlank(data)) {
                return;
            }

            var json;
            
            try{
                json = JSON.parse(data);
            }
            catch (ex) {
                console.error('解析 json 数据错误. data=', data);
                return;
            }

            filrePanel1.clear();

            filrePanel1.setJson(json);
        });
    }
</script>