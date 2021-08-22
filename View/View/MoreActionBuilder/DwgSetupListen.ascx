<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DwgSetupListen.ascx.cs" Inherits="App.InfoGrid2.View.MoreActionBuilder.DwgSetupListen" %>
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

    <mi:Store runat="server" ID="store1" Model="AC3_LISTEN_TABLE" IdField="AC3_LISTEN_TABLE_ID" DeleteRecycle="true" >
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:QueryStringParam Name="FK_DWG_CODE" QueryStringField="dwg_code" />
            <mi:QueryStringParam Name="FK_NODE_CODE" QueryStringField="node_code" />
        </FilterParams>
    </mi:Store>

    <mi:Store runat="server" ID="store2" Model="AC3_LISTEN_TABLE_FIELD" IdField="AC3_LISTEN_TABLEFIELD_ID" PageSize="0">
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:QueryStringParam Name="FK_DWG_CODE" QueryStringField="dwg_code" />
            <mi:QueryStringParam Name="FK_NODE_CODE" QueryStringField="node_code" />
        </FilterParams>
        <InsertParams>
            <mi:QueryStringParam Name="FK_DWG_CODE" QueryStringField="dwg_code" />
            <mi:QueryStringParam Name="FK_NODE_CODE" QueryStringField="node_code" />
            <mi:QueryStringParam Name="FK_LISTEN_CODE" QueryStringField="node_code" />
        </InsertParams>
        <DeleteQuery>
            <mi:ControlParam Name="AC3_LISTEN_TABLEFIELD_ID" ControlID="table2" PropertyName="CheckedRows" />
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

            <mi:Tab runat="server" Text="监听" Scroll="Vertical" >

                <mi:FormLayout runat="server" ID="formLayout1" FlowDirection="TopDown" StoreID="store1" Dock="Top" AutoSize="true" 
                    ItemWidth="400" ItemLabelAlign="Right" Padding="10">

                    <mi:ComboBox runat="server" ID="DB_METHOD_cb" DataField="DB_METHOD" TriggerMode="None" FieldLabel="操作方式" 
                        StringItems="all=全部;update=更新;insert=新建;delete=删除;" OnChanged="dbMethod_Changed(this)" />

                    <mi:TriggerBox runat="server" ID="DB_TABLE_tb" DataField="DB_TABLE" FieldLabel="数据表名" ButtonType="More" />
                    <mi:TextBox runat="server" ID="DB_TABLE_TEXT_tb" DataField="DB_TABLE_TEXT" FieldLabel="数据表描述" />
                    
                    <mi:ComboBox runat="server" ID="COND_SCRIPT_TYPE_cb" DataField="COND_SCRIPT_TYPE" MaxWidth="300px" FieldLabel="判断脚本类型" 
                        StringItems="xml=XML;cs=C# 脚本;json=图形脚本;" TriggerMode="None" />                   
                    

                </mi:FormLayout>

                
                <mi:Panel runat="server" ID="panel1" Dock="Top" Height="160" Scroll="None" >
                    <mi:Toolbar runat="server" ID="toolbar1">
                        <mi:ToolBarTitle ID="tableNameTB1" Text="监听字段" />

                        <mi:ToolBarButton Text="新增" OnClick="ser:store2.Insert()" />
                        <mi:ToolBarButton Text="保存" OnClick="ser:store2.SaveAll()" />
                        <mi:ToolBarButton Text="刷新" OnClick="ser:store2.Refresh()" />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:store2.Delete()" />

                        
                    </mi:Toolbar>
                    <mi:Table runat="server" ID="table2" StoreID="store2"  Dock="Full" JsonMode="Full" Height="200" PagerVisible="false">
                        <Columns>
                            <mi:RowNumberer />
                            <mi:RowCheckColumn />
                            <mi:TriggerColumn HeaderText="字段名" DataField="DB_FIELD" ButtonType="More"  />
                            <mi:BoundField HeaderText="字段描述" DataField="DB_FIELD_TEXT"  />                        
                            <mi:BoundField HeaderText="备注" DataField="REMARK" />
                        </Columns>
                    </mi:Table>
                </mi:Panel>
                
                <!--监听条件-->
                <mi:Panel runat="server" ID="panel3" Dock="Top" Height="400" MarginTop="4px" Scroll="None" >

                    <mi:Toolbar runat="server" ID="toolbar2">
                        <mi:ToolBarTitle Text="监听条件" />

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

                <!--监听值变化-->
                <mi:Panel runat="server" ID="panel2" Dock="Top" Height="400" MarginTop="8px" Scroll="None" >

                    <mi:Toolbar runat="server" ID="toolbar3">
                        <mi:ToolBarTitle Text="监听值变化" />

                        <mi:ToolBarButton Text="新增过滤" OnClick="vcPanel.insertItem()" />
                        <mi:ToolBarButton Text="新增 AND" OnClick="vcPanel.insertAnd()"  />
                        <mi:ToolBarButton Text="新增 OR" OnClick="vcPanel.insertOr()" />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="保存" OnClick="save_vChange()" />
                        <mi:ToolBarButton Text="刷新" OnClick="refresh_vChange()" />
                        <mi:ToolBarHr />
                        <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  />
                        
                    </mi:Toolbar>
                    
                    <div data-dock="full" style="overflow:auto; padding:10px;" class="flow-filter2">
                        <table class="mi-newline" style="min-width:600px;">
                            <tr>
                                <td style="width:80px; vertical-align:top;"><div class="" style="text-align:right;padding:4px;">过滤:</div></td>
                                <td>
                                    <ul class=" mi-newline connectedSortable mi-cont mi-filter-inner filter-where" style="border-color:#b6b6b6;">
                                        
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


                    </div>



                </mi:Panel>


                <div dock="top" style="height:100px; border:none; text-align:center; vertical-align:bottom; padding:20px; color:#808080;">
                    ---------------我是底线------------------
                </div>


            </mi:Tab>

            <mi:Tab runat="server" ID="tab2" Text="其它">

                <mi:FormLayout runat="server" ID="formLayout2" FlowDirection="TopDown"  StoreID="store1" AutoSize="true" Padding="10" ItemLabelAlign="Right">

                    <mi:CodeEditor runat="server" ID="ACTION_CS_tb2" DataField="COND_SCRIPT_CS" FieldLabel="C# 数据内容" Height="600"/>

<%--                    <mi:TextBox runat="server" ID="COND_SCRIPT_XML_tb" DataField="COND_SCRIPT_XML" FieldLabel="XML 数据内容" />--%>
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
    
<% if (false)
    { %>{
    <script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js"></script>
<% } %>

    <script>
        $(function () {
            //setTimeout(function () {

            //    console.log("xxxxxxxxxxxxxxxxxxxxxxxx");

            //    $(".mi-cont").sortable({
            //        connectWith: ".connectedSortable"
            //    }).disableSelection();

            //}, 2000);

        });



        function dbMethod_Changed(sender){

            var method = sender.getValue();

            if ('update' == method || 'all' == method) {

                Mini2.find('panel1').show();
                
                Mini2.find('panel2').show();
            }
            else {

                Mini2.find('panel1').hide();

                Mini2.find('panel2').hide();
            }

            //console.log("method : ", sender.getValue());

        }


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
                    height:600,
                   
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


            var vcPanel = Mini2.create('Mini2.ui.filter.Panel', {
                el: $('.flow-filter2 .filter-where'),
                //orderEl: $('.flow-filter2 .filter-order'),  //排序的元素
                //limitCountEl: $('.flow-filter2 .filter-limit-count'),
                //limitStartEl: $('.flow-filter2 .filter-limit-start'),
                //distinctEl: $('.flow-filter2 .filter-distinct'),
                fieldsEl: $('.flow-filter2 .filter-fields')

            });

            vcPanel.render();

            vcPanel.on('itemDblclick', function (evt) {

                var item = evt.item;

                var url = Mini2.urlAppend('/App/InfoGrid2/View/MoreActionBuilder/DwgFilterVChanegSetup.aspx', {
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

                        console.debug('ud = ', ud);

                        item.setData(ud);
                    }
                });

                win.show();

            });


            window.vcPanel = vcPanel;



            refresh_filter();
            refresh_vChange();

            setTimeout(function () {

                var cb = Mini2.find('DB_METHOD_cb');

                dbMethod_Changed(cb);

            }, 500);


        });

        function save_filter() {

            var dwgCode = $.query.get('dwg_code');
            var nodeCode = $.query.get('node_code');

            var data = filrePanel1.getJson();


            var url = Mini2.urlAppend('/View/MoreActionBuilder/DwgHandler.ashx', {
                action: 'SAVE_LISTEN_FILTER'
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
                action: 'GET_LISTEN_FILTER'
            }, true);

            Mini2.post(url, {
                dwg_code: dwgCode,
                item_code: nodeCode
            },
            function (data) {

                if (!Mini2.String.isBlank(data)) {

                    var json;

                    try{
                        json = JSON.parse(data);
                    }
                    catch (ex) {
                        console.error("解析 json 数据错误. ", data);
                    }

                    filrePanel1.clear();

                    filrePanel1.setJson(json);
                }

            });
        }



        function save_vChange() {

            var dwgCode = $.query.get('dwg_code');
            var nodeCode = $.query.get('node_code');

            var data = vcPanel.getJson();

            console.debug("保存 data: ", data);

            var url = Mini2.urlAppend('/View/MoreActionBuilder/DwgHandler.ashx', {
                action: 'SAVE_LISTEN_VCHANGE'
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

        /**
        * 值发生变
        *
        */
        function refresh_vChange() {

            var dwgCode = $.query.get('dwg_code');
            var nodeCode = $.query.get('node_code');

            var url = Mini2.urlAppend('/View/MoreActionBuilder/DwgHandler.ashx', {
                action: 'GET_LISTEN_VCHANGE'
            }, true);

            Mini2.post(url, {
                dwg_code: dwgCode,
                item_code: nodeCode
            },
            function (data) {

                var json;

                try {
                    json = JSON.parse(data);
                }
                catch (ex) {
                    console.error("解析 json 数据错误. ", data);
                }

                vcPanel.clear();

                vcPanel.setJson(json);

            });
        }



    </script>