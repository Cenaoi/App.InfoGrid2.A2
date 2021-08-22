<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanForm.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin2.Plan.PlanForm" %>
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
    <mi:Store runat="server" ID="storeMain1" Model="UT_015_PLAN" IdField="ROW_IDENTITY_ID" ReadOnly="true">
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
            <mi:QueryStringParam Name="ROW_IDENTITY_ID" QueryStringField="row_id" />
        </FilterParams>
        <UpdateParams>
            <mi:ServerParam Name="ROW_DATE_UPDATE" ServerField="TIME_NOW" />
        </UpdateParams>
    </mi:Store>
    <mi:Store runat="server" ID="storeProd1" Model="UT_020" IdField="ROW_IDENTITY_ID" >
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic=">=" />
            <mi:QueryStringParam Name="DOC_PARENT_ID" QueryStringField="row_id" />
        </FilterParams>
        <UpdateParams>
            <mi:ServerParam Name="ROW_DATE_UPDATE" ServerField="TIME_NOW" />
        </UpdateParams>
    </mi:Store>
</div>

<mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="0" Padding="0">

    <mi:Toolbar runat="server" ID="mainToolbar1">

        <mi:ToolBarButton Text="提交" Command="GoChangeBizSID_0_2" />

                
        <mi:ToolBarButton Text="撤销提交" Command="GoChangeBizSID_2_0" BeforeAskText="确定‘撤销提交’操作？" />

        <mi:ToolBarButton Text="打回" Command="GoChangeBizSID_2_1" BeforeAskText="确定‘打回’操作" />

        <mi:ToolBarButton Text="审核" Command="GoChangeBizSID_2_4" BeforeAskText="确定‘审核’操作？" />

        <mi:ToolBarHr />

        <mi:ToolBarButton Text="作废" Command="GoChangeBizSID_4_F3" BeforeAskText="确定‘作废’操作？" />
    </mi:Toolbar>

    <mi:Panel runat="server" ID="HeadPanel" Height="60" Scroll="None" PaddingTop="10">
        <mi:Label runat="server" ID="headLab" Value="单据" HideLabel="true" Dock="Center" Mode="Transform"  />
    </mi:Panel>

    <mi:ClipImage runat="server" ID="bizSID_ci" DataField="BIZ_SID" HideLabel="true" Value="0" DataSource="storeMain1" Position="Relative"
         Left="10px" Top="40px" Width="129" Height="47">
        <mi:ListItem Value="-3" Text="/res/signet_sid/SID_F3.png" /> 
        <mi:ListItem Value="0" Text="/res/signet_sid/SID_0.png" />            
        <mi:ListItem Value="2" Text="/res/signet_sid/SID_2.png" />
        <mi:ListItem Value="4" Text="/res/signet_sid/SID_4.png" />
    </mi:ClipImage>

    <mi:FormLayout runat="server" ID="FormLayoutTop" ItemWidth="200" ItemLabelWidth="60" PaddingTop="10" StoreID="storeMain1"  Dock="Right" Top="40px"
                ItemLabelAlign="Right" Width="380px"  FlowDirection="LeftToRight" AutoSize="true" Position="Relative" Right="0px" Left="auto" >   
             
        <mi:TextBox runat="server" ID="textBox4" DataField="DOC_CREATE_USER_TEXT" FieldLabel="开单人" ReadOnly="true" Width="160px" Validate="{}" />
        <mi:TextBox runat="server" ID="textBox13" DataField="DOC_CREATE_DATE" FieldLabel="开单时间" ReadOnly="true" />
                
        <div class="mi-newline" style="width:96%;height:1px;" ></div>

        <mi:TextBox runat="server" ID="textBox14" DataField="DOC_CHECK_USER_TEXT" FieldLabel="审核人" ReadOnly="true" Width="160px" />
        <mi:TextBox runat="server" ID="textBox15" DataField="DOC_CHECK_DATE" FieldLabel="审核时间" ReadOnly="true" />
        
    </mi:FormLayout>
                     
    <mi:TabPanel runat="server" ID="tabPanel1" Dock="Full" Region="Center" UI="win10" ButtonVisible="false" TabLeft="8">
        <mi:Tab runat="server" ID="tab1" Text="常规信息" >
            <mi:FormLayout runat="server" ID="HanderForm1" ItemWidth="300" PaddingTop="10" StoreID="storeMain1" 
                ItemLabelAlign="Right" Dock="Top" Region="North" Height="200" FlowDirection="LeftToRight" AutoSize="true" >
        

                <mi:ComboBox runat="server" ID="bizSID_cb" DataField="BIZ_SID" Visible="false" TriggerMode="None" FieldLabel="状态" ReadOnly="true" 
                    StringItems="-3=作废;0=草稿;2=已提交;4=已审核" >
            
                </mi:ComboBox>
        
                <mi:TextBox runat="server" ID="textBox1" DataField="BILL_NO" FieldLabel="单号" ReadOnly="true" Placeholder="自动产生" />
                <mi:TriggerBox runat="server" ID="triggerBox1" DataField="CLIENT_CODE" FieldLabel="客户代码" LabelAlign="Right" ButtonType="More" Required="true"  >
                    <MapItems>
                        <mi:MapItem SrcField="CLIENT_CODE" TargetField="CLIENT_CODE" />
                        <mi:MapItem SrcField="CLIENT_TEXT" TargetField="CLIENT_TEXT" />
                    </MapItems>
                </mi:TriggerBox>
                <mi:TextBox runat="server" ID="textBox12" DataField="CLIENT_TEXT" FieldLabel="客户名称" ButtonType="More" Required="true"/>


                <mi:TriggerBox runat="server" ID="triggerBox2" DataField="S_PROD_CODE" FieldLabel="货物代码" LabelAlign="Right" ButtonType="More" Required="true" >
                    <MapItems>
                        <mi:MapItem SrcField="PROD_CODE" TargetField="S_PROD_CODE" />
                        <mi:MapItem SrcField="PROD_TEXT" TargetField="S_PROD_TEXT" />
                    </MapItems>
                </mi:TriggerBox>
                <mi:TextBox runat="server" ID="textBox9" DataField="S_PROD_TEXT" FieldLabel="货物名称" ButtonType="More" Required="true"/>

                <%-- <mi:NumberBox runat="server" ID="numBox1" DataField="NUM_TOTAL" FieldLabel="数量" Required="true" />--%>

                <table border="0" cellpadding="0" cellspacing="0" class="" style=" border-collapse:collapse; " >
                    <tr>
                        <td><mi:NumberBox runat="server" ID="COL_21_nb" DataField="PROD_NUM" FieldLabel="数量" LabelAlign="Right" Width="200" Required="true" /></td>
                        <td><mi:ComboBox runat="server" ID="COL_22_tb" DataField="PROD_NUM_UNIT" FieldLabel="数量单位名称" StringItems="桶;袋;其它" HideLabel="true" Width="100%" TriggerMode="None" /></td>
                    </tr>
                </table>
            
                <table border="0" cellpadding="0" cellspacing="0" style=" border-collapse:collapse;"  >
                    <tr>
                        <td><mi:NumberBox runat="server" ID="COL_23_nb" DataField="PROD_WEIGHT" FieldLabel="重量" LabelAlign="Right" Width="200"  /></td>
                        <td><mi:ComboBox runat="server" ID="COL_24_tb" DataField="PROD_WEIGHT_UNIT" FieldLabel="重量单位名称" StringItems="吨;公斤;其它" HideLabel="true" Width="100%" TriggerMode="None" /></td>
                    </tr>
                </table>

                
                <div class="mi-newline"><div class="mi-newline-border1"></div></div>

                <mi:ComboBox runat="server" ID="cb_store" DataField="STORE_TEXT" FieldLabel="仓库" TriggerMode="None">

                </mi:ComboBox>

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


                <div class="mi-newline"></div>

                <mi:TextBox runat="server" ID="textBox5" DataField="REMARK" FieldLabel="备注" MinWidth="606px" />
                

        
            </mi:FormLayout>

              <mi:Panel runat="server" ID="panel2"  Dock="Full"  Padding="6" Scroll="None">
                <mi:Toolbar runat="server" ID="prodToolbar1" >
                    <mi:ToolBarButton Text="删除" Command="GoDeleteProd" />
                    <mi:ToolBarButton ID="OutCodeList" Text="出库扫码" Command="GoOutWin" />
                    <mi:ToolBarButton ID="SelectInOrderBtn" Text="从入库单选择" Command="GoShowInOrder" />
                </mi:Toolbar>
                <mi:Table runat="server" ID="table1" StoreID="storeProd1" Dock="Full" >
                    <Columns>
                        <mi:RowNumberer />
                        <mi:RowCheckColumn />
                        <mi:SelectColumn DataField="LOCK_TAG" HeaderText="锁状态" StringItems="0=;2=锁住" EditorMode="None" TriggerMode="None" Width="50" ItemAlign="Center">
                        </mi:SelectColumn>
                        <mi:BoundField HeaderText="条码" DataField="PROD_CODE" EditorMode="None" />

                        <mi:SelectColumn HeaderText="出库状态" DataField="OUT_SID" Width="70" TriggerMode="None" StringItems="0=未出库; 2=已出库;" EditorMode="None" >
                        </mi:SelectColumn>

<%--                        
                        <mi:BoundField HeaderText="检验" DataField="CHECK_TEXT" />--%>

                        <mi:SelectColumn HeaderText="检验等级" DataField="CHECK_CODE"  TriggerMode="None" >

                        </mi:SelectColumn>

                        <mi:BoundField HeaderText="检验号" DataField="CHECK_NO" />
                        <mi:DateColumn HeaderText="检验时间" DataField="CHECK_DATE" />
                    </Columns>
                </mi:Table>
            
            </mi:Panel>

        </mi:Tab>
        <mi:Tab runat="server" ID="tab2" Text="货物明细" Scroll="None">

            <mi:FormLayout runat="server" ID="formLayout2" ItemWidth="300" PaddingTop="10" StoreID="storeMain1" 
                    ItemLabelAlign="Right" Region="North"  Dock="Full" FlowDirection="LeftToRight" >

                <mi:TextBox runat="server" ID="PROD_DJ_tb" DataField="PROD_DJ" FieldLabel="等级" />
                <mi:TextBox runat="server" ID="PROD_CJ_tb" DataField="PROD_CJ" FieldLabel="厂家" />
                <mi:TextBox runat="server" ID="PROD_PACK_tb" DataField="PROD_PACK" FieldLabel="包装" />
                <mi:DatePicker runat="server" ID="PROD_SCRQ_tb" DataField="PROD_SCRQ" FieldLabel="生产日期" />
                <mi:TextBox runat="server" ID="PROD_KH_tb" DataField="PROD_KH" FieldLabel="货物卡号" />
<%--                <mi:TextBox runat="server" ID="PROD_HWPM_tb" DataField="PROD_HWPM" FieldLabel="货物品名" />--%>
                <mi:TextBox runat="server" ID="PROD_CL_tb" DataField="PROD_CL" FieldLabel="材料" />
                <mi:TextBox runat="server" ID="PROD_GG_tb" DataField="PROD_GG" FieldLabel="规格" />
                <mi:TextBox runat="server" ID="PROD_PP_tb" DataField="PROD_PP" FieldLabel="品牌" />
                <mi:TextBox runat="server" ID="PROD_CD_tb" DataField="PROD_CD" FieldLabel="产地" />
                <mi:TextBox runat="server" ID="PROD_CXH_tb" DataField="PROD_CXH" FieldLabel="车（箱）号" />
                <mi:DatePicker runat="server" ID="PROD_DHSJ_tb" DataField="PROD_DHSJ" FieldLabel="到货时间" />
                <mi:DatePicker runat="server" ID="PROD_YSDJ_tb" DataField="PROD_YSDJ" FieldLabel="验收时间" />
                <mi:ComboBox runat="server" ID="PROD_YSFS_tb" DataField="PROD_YSFS" FieldLabel="验收方式" StringItems="点件;过磅;量体积;其它" TriggerMode="None"  />
                <mi:ComboBox runat="server" ID="PROD_GRZX_tb" DataField="PROD_GRZX" FieldLabel="工人装卸"  StringItems="是;否" TriggerMode="None"  />
                <mi:ComboBox runat="server" ID="PROD_FJ_tb" DataField="PROD_FJ" FieldLabel="分拣"  TriggerMode="None" StringItems="是;否" />
                <mi:ComboBox runat="server" ID="PROD_HD_tb" DataField="PROD_HD" FieldLabel="换袋"  TriggerMode="None" StringItems="是;否" />
                <mi:TextBox runat="server" ID="PROD_DBCL_tb" DataField="PROD_DBCL" FieldLabel="打包材料" />
                <mi:TextBox runat="server" ID="PROD_CFHW_tb" DataField="PROD_CFHW" FieldLabel="存放货位" />
                <mi:TextBox runat="server" ID="PROD_HW_REMARK_tb" DataField="PROD_HW_REMARK" FieldLabel="货位备注" />
                <mi:NumberBox runat="server" ID="PROD_YSSL_tb" DataField="PROD_YSSL" FieldLabel="应收数量" />
                <mi:NumberBox runat="server" ID="PROD_YSZL_tb" DataField="PROD_YSZL" FieldLabel="应收重量" />
                <mi:TextBox runat="server" ID="PROD_YSSJ_tb" DataField="PROD_YSSJ" FieldLabel="应收散件" />

                <mi:TextBox runat="server" ID="PROD_SJ_tb" DataField="PROD_SJ" FieldLabel="散件" />
                <mi:TextBox runat="server" ID="PROD_RENMAR_tb" DataField="PROD_RENMAR" FieldLabel="货物备注" />
                
                <div class="mi-newline" ><div class="mi-newline-border"></div></div>

                <mi:TextBox runat="server" ID="textBox10" DataField="DOC_UPDATE_USER_TEXT" FieldLabel="更新人" ReadOnly="true" />
                <mi:TextBox runat="server" ID="textBox11" DataField="DOC_UPDATE_DATE" FieldLabel="更新时间" ReadOnly="true" />

            </mi:FormLayout>
        </mi:Tab>
    </mi:TabPanel>      
    

</mi:Viewport>
    

</form>
