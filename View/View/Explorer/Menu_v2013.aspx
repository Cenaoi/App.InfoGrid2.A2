<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu_v2013.aspx.cs" Inherits="App.InfoGrid2.View.Explorer.Menu_v2013" %>

<ul  id="mobanwang_com" class="first-menu" style="z-index: 99; position: fixed; width: 1020px;">
    <li><a href="#" style="color: #ff0; background: none; border: none; font-weight: bold;"
        target="_self">广州市玖龙塑料包装有限公司</a></li>

    <li><a href="#">销售管理</a>
        <ul style="display: none;" class="second-menu subMenu1">
            <li>
                <a href="/app/infogrid2/view/onebuilder/previewpage.aspx?pageid=136">销售订单</a>
            </li>
<%--            <li>
                <a href="#">销售订单汇总查询</a>
            </li>
            --%>
            <li>
                <a href="/app/infogrid2/view/moreview/moreviewpreview.aspx?id=196">销售订单明细查询</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onebuilder/previewpage.aspx?pageid=192">销售出货单</a>
            </li>
<%--            <li>
                <a href="/app/InfoGrid2/View/OneAction/ActList.aspx">销售出货单明细查询</a>
            </li>
            --%>
            <li>
                <a href="/app/infogrid2/view/moreview/moreviewpreview.aspx?id=206">销售出货单汇总查询</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onebuilder/previewpage.aspx?pageid=202">销售退货单</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/moreview/moreviewpreview.aspx?id=207">销售退货单汇总查询</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/moreview/MoreViewPreview.aspx?id=224">销售管理进度跟踪中心</a>
            </li>
        </ul>
    </li>
      
    <li><a href="#">生产管理</a>
        <ul style="display: none;" class="second-menu subMenu1">
            <li>
                <a href="/app/infogrid2/view/moreview/MoreViewPreview.aspx?id=233">待生产订单.</a>
            </li>
            <li>
                <a href="/App/InfoGrid2/View/Biz/JLSLBZ/XSSCZL/FormCreate.aspx">吸塑生产指令单</a>
            </li>
            <li>
                <a href="/App/InfoGrid2/View/Biz/JLSLBZ/JHSCZL/FormCreate.aspx">胶盒生产指令单</a>
            </li>
            <li>
                <a href="/App/InfoGrid2/View/Biz/JLSLBZ/Scjhxqzx/MainFrm.aspx">生产计划需求分析中心.</a>
            </li>
            <li>
                <a href="/app/InfoGrid2/View/Biz/JLSLBZ/DLMX/TableDlmx.aspx">待领料列表.</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onebuilder/previewpage.aspx?pageid=250">领料单</a>
            </li>
            <li>
                <a href="#">工序产量待上报列表.</a>
            </li>
            <li>
                <a href="#">工序产量上报</a>
            </li>
            <li>
                <a href="#">工序产量异常监控中心.</a>
                <ul style="display: none;" class="fourth-menu">
                    <li><a href="#">制单状况</a></li>
                    <li><a href="#">采购状况</a></li>
                    <li><a href="#">领料状况</a></li>
                    <li><a href="#">内部生产状况</a></li>
                    <li><a href="#">委外加工状况</a></li>
                </ul>
            </li>
            <li>
                <a href="/app/infogrid2/view/onebuilder/previewpage.aspx?pageid=261">成品入库</a>
            </li>
            <li>
                <a href="/app/InfoGrid2/View/OneMap/OneMapList.aspx">生产进度跟踪中心.</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onebuilder/previewpage.aspx?pageid=261">车间退料</a>
            </li>
        </ul>
    </li>
    
    <li><a href="#">采购管理</a>
        <ul style="display: none;" class="second-menu subMenu1">
            <li>
                <a href="#">待采购物料表</a>
            </li>
            <li>
                <a href="#">采购订单</a>
            </li>
            <li>
                <a href="#">采购预付款管理</a>
            </li>
            <li>
                <a href="#">采购未入库列表</a>
            </li>
            <li>
                <a href="#">采购入库单</a>
            </li>
            <li>
                <a href="#">采购入库单明细查询</a>
            </li>
            <li>
                <a href="#">退厂单</a>
            </li>
            <li>
                <a href="#">采购进度跟踪分析中心</a>
            </li>
        </ul>
    </li>
    <li><a href="#">仓库管理</a>
        <ul style="display: none;" class="second-menu subMenu1">
            <li>
                <a href="/app/infogrid2/view/onetable/tablepreview.aspx?id=279">仓库查询</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onebuilder/previewpage.aspx?pageid=282">调拨管理</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onebuilder/previewpage.aspx?pageid=288">盘点管理</a>
            </li>
        </ul>
    </li>    
    <li><a href="#">应收应付管理</a>
        <ul style="display: none;" class="second-menu subMenu1">
            <li>
                <a href="/app/infogrid2/view/onebuilder/previewpage.aspx?pageid=294">预收款单</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onebuilder/previewpage.aspx?pageid=302">收款单</a>
            </li>
            <li>
                <a href="#">应收款调整单</a>
            </li>
            <li>
                <a href="#">客户对账中心</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onebuilder/previewpage.aspx?pageid=306">预付款单</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onebuilder/previewpage.aspx?pageid=312">付款单</a>
            </li>
            <li>
                <a href="#">应收款调整单</a>
            </li>
            <li>
                <a href="#">供应商对账中心</a>
            </li>
            <li>
                <a href="#">日常费用单</a>
            </li>
            <li>
                <a href="#">日常费用统计</a>
            </li>
        </ul>
    </li>
    <li><a href="#">预警管理</a>
        <ul style="display: none;" class="second-menu subMenu1">
            <li>
                <a href="#">最低最高库存报警</a>
            </li>
            <li>
                <a href="#">采购未到货提醒</a>
            </li>
            <li>
                <a href="#">销售订单预警</a>
            </li>
        </ul>
    </li>
    <li><a href="#">基础档案</a>
        <ul style="display: none;" class="second-menu subMenu1">
            <li>
                <a href="/app/infogrid2/view/onetable/tablepreview.aspx?id=122">成品档案</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onetable/tablepreview.aspx?id=85">客户供应商</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onetable/TablePreview.aspx?id=277">人员档案</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onetable/tablepreview.aspx?id=133">付款方式</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onetable/tablepreview.aspx?id=123">材料档案</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onetable/tablepreview.aspx?id=240">工序工艺档案</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onetable/tablepreview.aspx?id=127">仓库档案</a>
            </li>
            <li>
                <a href="/app/infogrid2/view/onetable/tablepreview.aspx?id=123">材料档案</a>
            </li>
            
            <li><a href="/app/InfoGrid2/View/Biz/Core_Company/EditCompanyInfo.aspx"">公司</a></li>
            <li>
                <a href="/app/InfoGrid2/Sec/User/UserListM2.aspx">账号管理</a>
            </li>
        </ul>
    </li>

    <li style="display:;"><a href="#">后台管理</a>
        <ul style="display: none;" class="second-menu subMenu1">
            <li>
                <a href="#">创建&nbsp;&nbsp;>></a>
                <ul style="display: none;" class="fourth-menu">
                    <li><a href="/app/InfoGrid2/View/OneTable/StepNew1.aspx">工作表</a></li>
                    <li><a href="/app/InfoGrid2/view/OneView/StepNew2.aspx">视图表</a></li>
                    <li><a href="/app/InfoGrid2/View/OnePage/StepNew1.aspx">复杂工作表</a></li>
                    <li><a href="/app/InfoGrid2/View/MoreView/MViewStepNew1.aspx">关联视图表</a></li>
                </ul>
            </li>
            <li>
                <a href="/app/InfoGrid2/View/Manager/TreePageManager.aspx">后台管理</a>
            </li>

            <li>
                <a href="/app/InfoGrid2/View/OneAction/ActList.aspx">联动设置</a>
            </li>
            <li>
                <a href="/app/InfoGrid2/View/MoreView/MViewList.aspx">关联表</a>
            </li>
            <li>
                <a href="/app/InfoGrid2/View/OneMap/OneMapList.aspx">映射规则</a>
            </li>
            <li>
                <a href="#">导入&nbsp;&nbsp;>> </a>
                <ul style="display: none;" class="fourth-menu">
                    <li><a href="/app/InfoGrid2/View/InputExcel/OneStepEdit1.aspx">规则列表</a></li>
                    <li><a href="/app/InfoGrid2/View/InputExcel/StepNew1.aspx"">单表导入</a></li>
                    <li><a href="/app/InfoGrid2/view/InputExcel/StepManyNew1.aspx">多表导入</a></li>
                </ul>
            </li>
            <li>
                <a href="#">测试&nbsp;&nbsp;>> </a>
                <ul style="display: none;" class="fourth-menu">
                    <li><a href="/app/InfoGrid2/View/Explorer/ConTest.aspx"">控件测试</a></li>
                    <li><a href="/app/InfoGrid2/view/Explorer/PrintTest.aspx">打印测试</a></li>
                    <li><a href="/app/InfoGrid2/View/Biz/Core_Company/EditCompanyInfo.aspx"">公司</a></li>
                    <li><a href="/app/InfoGrid2/View/Biz/Core_Menu/EditMenuTree.aspx"">菜单设置</a></li>
                    <li><a href="/app/InfoGrid2/View/Biz/Input_Data/Test.aspx"">导入插件设置...</a></li>
                    <li><a href="/App/InfoGrid2/Sec/ArrLogin/ArrLoginList.aspx">管理下级权限模块</a></li>
                </ul>
            </li> 
            <li>
                <a href="/app/InfoGrid2/View/OneFilter2/Filter2Main.aspx">页面权限（二次过滤）</a>
            </li>
            <li>
                <a href="/app/InfoGrid2/Sec/UIFilter/MainUserList.aspx">按角色权限设置</a>
            </li>
            <li>
                <a href="/app/InfoGrid2/Sec/UIFilterTU/MainTableList.aspx">按人员权限设置</a>
            </li>
        </ul>
    </li>    

</ul>


<div style="float:right; height:37px; width:100%; background-color:#333333; vertical-align: bottom;" 
    class="AText">
    <div style="height:16px;"></div>
    <span style="float:right;">
    
        <a href="#" id="TimeX"></a>

        <a href="#"><%= EcUser.LoginName %>(<%= EcUser.LoginID %>)</a>
        <a href="#" onclick="window.location.href='/App/BizExplorer/View/Logout.aspx'" >退出</a>
        &nbsp;&nbsp;
    </span>
</div>

<script type="text/javascript">

    function AddTab2(sender) {

        var url = $(this).attr("href");
        var label = $(this).text();

        if (url == "#" || url == "") {
            return false;
        }

        var tab = tabs.add({
            url: url,
            label: label
        });

        var panel = tab.panel;

        $(panel).attr("srcUrl", url.toLowerCase());

        $(".subMenu1").hide();

        return false;
    }

    var m_TimeX = null;

    $(document).ready(function () {

        $(".subMenu1").find("a[href!='#']").click(AddTab2);

        $("#HomeMenu").css({
            "height": "39px",
            "background-color": "#4A4A4A"
        });


        if (m_TimeX == null) {
            m_TimeX = $("#TimeX");
        }

        setInterval(function () {
            m_TimeX.load("/app/InfoGrid2/View/Explorer/SysHeartbeat.aspx");

        }, 10 * 1000);


    });

</script>