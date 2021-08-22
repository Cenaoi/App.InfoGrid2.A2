<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WareForm.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin.Forms.WareForm" %>
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

<style type="text/css">
    .page-head
    {
         font-size:26px;
         font-weight:bold;
    }
</style>

<form action="" method="post">


<div runat="server" id="StoreSet">
    <mi:Store runat="server" ID="storeMain1" Model="UT_001" IdField="ROW_IDENTITY_ID" TranEnabled="false">
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
            <mi:QueryStringParam Name="ROW_IDENTITY_ID" QueryStringField="row_id" />
        </FilterParams>
    </mi:Store>

    <mi:Store runat="server" ID="storeProd1" Model="UT_002" IdField="ROW_IDENTITY_ID" PageSize="0" ReadOnly="true" DeleteRecycle="true">
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
            <mi:QueryStringParam Name="COL_1" QueryStringField="row_id" />
            <mi:QueryStringParam Name="IO_TAG" QueryStringField="io_tag" />
        </FilterParams>
        <InsertParams>
            <mi:QueryStringParam Name="COL_1" QueryStringField="row_id" />
            <mi:QueryStringParam Name="IO_TAG" QueryStringField="io_tag" />
        </InsertParams>
        <DeleteQuery>
            <mi:QueryStringParam Name="COL_1" QueryStringField="row_id" />
            <mi:QueryStringParam Name="IO_TAG" QueryStringField="io_tag" />
            <mi:ControlParam Name="ROW_IDENTITY_ID" ControlID="tableProd1" PropertyName="CheckedRows" />
        </DeleteQuery>
        <DeleteRecycleParams>
            <mi:Param Name="ROW_SID" DefaultValue="-3" />
            <mi:ServerParam Name="ROW_DATE_DELETE" ServerField="TIME_NOW" />    
        </DeleteRecycleParams>
        <SummaryFields>
            <mi:SummaryField DataField="COL_21" SummaryType="SUM"></mi:SummaryField>
            <mi:SummaryField DataField="COL_23" SummaryType="SUM"></mi:SummaryField>
        </SummaryFields>
    </mi:Store>
</div>

<mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="0" Padding="0">

    <mi:Toolbar runat="server" ID="mainToolbar1">
        <mi:ToolBarButton Text="提交" Command="GoSave" />
<%--        <mi:ToolBarButton Text="刷新" Command="GoRefresh" />--%>
        <mi:ToolBarButton Text="审核" Command="GoChangeBizSID_2_4" />

        <mi:ToolBarHr />
        
        <mi:ToolBarButton Text="撤销提交" Command="GoChangeBizSID_2_0" />
        <mi:ToolBarButton Text="撤销审核" Command="GoChangeBizSID_4_2" />

        <mi:ToolBarButton Text="打印条码" Command="GoPrintCode1" ID="PrintCode1Btn" />


    </mi:Toolbar>

    <mi:Panel runat="server" ID="HeadPanel" Height="60" Scroll="None" PaddingTop="10">
        <mi:Label runat="server" ID="headLab" Value="单据" HideLabel="true" Dock="Center" Mode="Transform"  />
    </mi:Panel>
<mi:ClipImage runat="server" ID="bizSID_ci" DataField="BIZ_SID" HideLabel="true" Value="0" DataSource="storeMain1" Position="Relative" Left="10px" Top="40px">
    <mi:ListItem Value="0" Text="/res/signet_sid/SID_0.png" />            
    <mi:ListItem Value="2" Text="/res/signet_sid/SID_2.png" />
    <mi:ListItem Value="4" Text="/res/signet_sid/SID_4.png" />
</mi:ClipImage>
    <mi:FormLayout runat="server" ID="HanderForm1" ItemWidth="300" PaddingTop="10" StoreID="storeMain1"
        ItemLabelAlign="Right" Region="North" Height="200" FlowDirection="LeftToRight" AutoSize="true">

                
        <mi:TextBox runat="server" ID="textBox1" DataField="BILL_NO" FieldLabel="单号" ReadOnly="true" Placeholder="自动产生" />


        <mi:TriggerBox runat="server" ID="triggerBox1" DataField="CLIENT_CODE" FieldLabel="客户代码" LabelAlign="Right" ButtonType="More" 
            Required="true" 
            ButtonClickCallbackDType="Sample">
            <MapItems>
                <mi:MapItem TargetField="CLIENT_CODE" SrcField="CLIENT_CODE" Remark="客户代码" />
                <mi:MapItem TargetField="CLIENT_NAME" SrcField="CLIENT_TEXT" Remark="客户名称" />
            </MapItems>
        </mi:TriggerBox>
        <mi:TextBox runat="server" ID="textBox14" DataField="CLIENT_NAME" FieldLabel="客户名称" Required="true"/>

        <mi:ComboBox runat="server" ID="comboBox1" DataField="DH_TYPE" FieldLabel="到货方式" TriggerMode="None" 
            StringItems="海运;汽运;铁路;其它">
        </mi:ComboBox>        
        <mi:TextBox runat="server" ID="textBox2" DataField="CCK_NO" FieldLabel="储存卡号" />
        <mi:ComboBox runat="server" ID="comboBox2" DataField="DH_XZ" FieldLabel="到货性质" TriggerMode="None"
            StringItems="自送;配送;其它">
        </mi:ComboBox>
        <mi:ComboBox runat="server" ID="comboBox3" DataField="IS_PACKER" FieldLabel="打包" TriggerMode="None" 
            StringItems="打包;不打包;其它">
        </mi:ComboBox>
        <mi:ComboBox runat="server" ID="comboBox4" DataField="CONT_SIZE" FieldLabel="集装箱尺寸" TriggerMode="None"
            StringItems="20英尺;40英尺;其它">
        </mi:ComboBox>
        <mi:ComboBox runat="server" ID="comboBox5" DataField="ZXX" FieldLabel="装卸箱" TriggerMode="None"
            StringItems="分;不分">
        </mi:ComboBox>
        <mi:ComboBox runat="server" ID="cb_store" DataField="STORE_TEXT" FieldLabel="仓库" TriggerMode="None">

        </mi:ComboBox>
        
         <div class="mi-newline" ><div class="mi-newline-border1"></div></div>

        <mi:ComboBox runat="server" ID="comboBox6" DataField="CJ_SID" FieldLabel="抽检状态" TriggerMode="None"
            StringItems="分;不分">
        </mi:ComboBox>
        

        <mi:TextBox runat="server" ID="textBox3" DataField="CJ_WTS" FieldLabel="抽样送检委托书" />
        <mi:TextBox runat="server" ID="textBox4" DataField="CJ_CODE" FieldLabel="抽检单号" />
        
        
        <div class="mi-newline" ><div class="mi-newline-border1"></div></div>

         <mi:TextBox runat="server" ID="tbx_COL_7" DataField="COL_7" FieldLabel="运输车牌" />
         <mi:TextBox runat="server" ID="tbx_COL_8" DataField="COL_8" FieldLabel="送货起始地" />
         <mi:TextBox runat="server" ID="tbx_COL_9" DataField="COL_9" FieldLabel="送货目的地" />
         <mi:DatePicker runat="server" ID="dp_COL_10" DataField="COL_10" FieldLabel="送货日期" />

        


        <mi:TextBox runat="server" ID="textBox5" DataField="REMARK" FieldLabel="备注" MinWidth="606px" />


        <table class="mi-newline" border="0" cellpadding="0" cellspacing="0" >
            <tr>
                <td style="width:300px;"><mi:TextBox runat="server" ID="textBox12" FieldLabel="扫码框" Width="300" LabelAlign="Right" /></td>
                <td style="  padding-left: 4px;vertical-align: top;"><mi:Button runat="server" ID="button1" Text="提交" Command="GoScanCode" Width="80" /></td>
                <td>&nbsp;</td>
            </tr>
        </table>

    </mi:FormLayout>

    <mi:Panel runat="server" ID="panel1" Padding="8"  Region="Center" Scroll="None" MinHeight="300"  >
        <mi:TabPanel runat="server" ID="detailTabPanel" Height="400" Region="Center" Dock="Full" ButtonVisible="false" Plain="true" UI="win10" >

            <mi:Tab runat="server" ID="tab1" Scroll="None" Text="货物批次明细">
                <mi:Toolbar runat="server" ID="toolbar1">
                    <mi:ToolBarButton Text="新建" OnClick="ser:storeProd1.Insert()" />
                    <mi:ToolBarButton Text="刷新" OnClick="ser:storeProd1.Refresh()" />
                    <mi:ToolBarHr />
                    
                    <mi:ToolBarButton Text="从计划中导入" OnClick="ImportData()" Visible="false" />
                    <mi:ToolBarButton  Text="从入库中导入" OnClick="ImportData_2()" ID="tbb_002_to_002"/>

                    <mi:ToolBarHr />                   
                    
                    <mi:ToolBarButton  Text="导出 Excel" Command="GoProdToExcel" />

                    <mi:ToolBarButton ID="QuickLevelEditBtn" Text="快速修改等级" Command="GoShowQuickkLevel" />

                    <mi:ToolBarHr />
                    <mi:ToolBarButton Text="删除" OnClick="ser:storeProd1.Delete()" BeforeAskText="确定删除选中的记录?" />
                </mi:Toolbar>
                <mi:Table runat="server" ID="tableProd1" Dock="Full" StoreID="storeProd1" PagerVisible="false" SummaryVisible="true" ReadOnly="true">
                    <Columns>
                        <mi:RowNumberer />
                        <mi:RowCheckColumn />
                        <mi:ActionColumn HeaderText="功能" Width="60">
                            <mi:ActionItem Text="编辑" Command="GoEdit" DisplayMode="Text" />
                        </mi:ActionColumn>
                        <mi:BoundField HeaderText="货主" DataField="COL_27" />
                        <mi:BoundField HeaderText="货物编码" DataField="PROD_CODE" />
                        <mi:BoundField HeaderText="货物品名" DataField="COL_3" />
                        <mi:BoundField HeaderText="等级" DataField="COL_28" />
                        <mi:BoundField HeaderText="厂家" DataField="COL_29" />
                        <mi:BoundField HeaderText="包装" DataField="PACK_TEXT" />
                        <mi:BoundField HeaderText="产地" DataField="COL_7" />
                        <mi:NumColumn HeaderText="数量" DataField="COL_21" SummaryType="SUM" SummaryFormat="数量: {0}" />
                        <mi:BoundField HeaderText="单位" DataField="COL_22" Width="60" />

                        <mi:NumColumn HeaderText="重量" DataField="COL_23"  SummaryType="SUM" SummaryFormat="重量: {0}" />
                        <mi:BoundField HeaderText="单位" DataField="COL_24" Width="60"  />                        
                        <mi:DateColumn HeaderText="生产日期" DataField="PROD_DATE" />
                    </Columns>
                </mi:Table>
            </mi:Tab>

            <mi:Tab runat="server" ID="tab2" Text="货物条码分配表">



            </mi:Tab>

        </mi:TabPanel>
    </mi:Panel>


    <mi:FormLayout runat="server" ID="footerForm1" ItemWidth="300" PaddingTop="8" PaddingBottom="8" ItemLabelAlign="Right" Region="South" 
        FlowDirection="LeftToRight" AutoSize="true" StoreID="storeMain1">
        <mi:TextBox runat="server" ID="textBox6" DataField="COL_1" FieldLabel="开单人" ReadOnly="true" />
        <mi:TextBox runat="server" ID="textBox7" DataField="COL_2" FieldLabel="审核人" ReadOnly="true" />
        <mi:TextBox runat="server" ID="textBox8" DataField="COL_3" FieldLabel="开单时间" ReadOnly="true" />
        
        <div class="mi-newline" style="width:96%;height:1px;" ></div>

        <mi:TextBox runat="server" ID="textBox9" DataField="COL_4" FieldLabel="更新序号" ReadOnly="true" />
        <mi:TextBox runat="server" ID="textBox10" DataField="COL_5" FieldLabel="更新人" ReadOnly="true" />
        <mi:TextBox runat="server" ID="textBox11" DataField="COL_6" FieldLabel="更新时间" ReadOnly="true" />
    </mi:FormLayout>
</mi:Viewport>
</form>
<script>

    "use strict";
    
    //根据输入的文字获取数据   回车按钮触发
    function biz_mapping(owner, type_id, view_id, one_field) {

        var me = owner;
        var record = me.record;

        log.debug(one_field);

        var id = owner.getValue();
        var mapItems = owner.mapItems;  //映射字段的规则

        var base64 = window.BASE64.encoder("[{field:'" + one_field + "',logic:'like',value:'%" + id + "%'}]");//返回编码后的字符 

        $.get("/App/InfoGrid2/view/MoreView/OneMapAction.aspx", {
            type: type_id,
            viewId: view_id,
            id: id,
            one_field: '',
            filter2: base64,
            _rum: RndNum(8)
        },
        function (responseText, textStatus) {

            var model = eval('(' + responseText + ')');

            console.log(model);

            if (model.result != "ok") { return; }

            //如果只有一个就赋值到文本框中
            if (model.count == 1) {
                biz_setMapValues(record, model.data, mapItems);
            }
            else {

                var urlStr = $.format("/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={0}&viewId={1}&filter2={2}",
                    type_id, view_id, base64);

                var win = Mini2.create('Mini2.ui.Window', {
                    mode: true,
                    text: '选择',
                    iframe: true,
                    width: 800,
                    height: 600,
                    state: 'max',
                    startPosition: 'center_screen',
                    url: urlStr
                });

                win.editor = me;
                win.record = record;
                win.show();

                win.formClosed(biz_FormClosed);


            }

        });

    }

    //设置映射值
    function biz_setMapValues(record, model, mapItems) {

        var me = this,
            i,
            item,
            srcValue;

        if (mapItems && model) {

            console.log(model);
            console.log(mapItems);

            for (i = 0; i < mapItems.length; i++) {

                item = mapItems[i];

                if ('' == item.srcField || '' == item.targetField) {
                    continue;
                }

                srcValue = model[item.srcField];

                record.set(item.targetField, srcValue);
            }

        }
    }


    function ImportData() {

        //进出库类型
        var io_tag = $.query.get("io_tag");

        console.log(io_tag);

        var base64 = window.BASE64.encoder("[{field:'UT_009.IO_TAG',logic:'=',value:'"+io_tag+"'}]");//返回编码后的字符 

        var urlStr = $.format("/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={0}&viewId={1}&filter2={2}",
                  'VIEW', 40,base64);

        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '选择',
            iframe: true,
            width: 800,
            height: 600,
            state: 'max',
            startPosition: 'center_screen',
            url: urlStr
        });


        win.show();

        win.formClosed(function(e){
        
            var row = e.row;

            //没有选择一条记录，不用处理
            if ( !row || row.length === 0) {

                return;
            }

            var ids = "";

            row.forEach(function (v,i) {
                
                console.log(v);

                if (i > 0) { ids += "," };

                ids += v.UT_009_ROW_IDENTITY_ID;


            });

            widget1.submit('form:first', {
                action: 'GoImportData',
                actionPs: ids
            });

      

        });

    }


    //从入库明细导入数据
    function ImportData_2() {


        var base64 = window.BASE64.encoder("[{field:'UT_001.BIZ_SID',logic:'>=',value:'" + 2 + "'}]");//返回编码后的字符 

        var urlStr = $.format("/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={0}&viewId={1}&filter2={2}",
                  'VIEW', 14, base64);

        var win = Mini2.createTop('Mini2.ui.Window', {
            mode: true,
            text: '选择',
            iframe: true,
            width: 800,
            height: 600,
            state: 'max',
            startPosition: 'center_screen',
            url: urlStr
        });


        win.show();

        win.formClosed(function (e) {

            var row = e.row;

            //没有选择一条记录，不用处理
            if (!row || row.length === 0) {

                return;
            }

            var ids = "";

            row.forEach(function (v, i) {

                console.log(v);

                if (i > 0) { ids += "," };

                ids += v.UT_002_ROW_IDENTITY_ID;
            });

            widget1.submit('form:first', {
                action: 'GoImportData_2',
                actionPs: ids
            });



        });

    }



</script>