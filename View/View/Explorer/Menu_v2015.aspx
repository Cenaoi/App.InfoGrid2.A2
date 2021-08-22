<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu_v2015.aspx.cs" Inherits="App.InfoGrid2.View.Explorer.Menu_v2015" %>

<ul  id="mobanwang_com" class="first-menu" style="z-index: 99; position: fixed;">
    <li><a href="#" style="color: #ff0; background: none; border: none; font-weight: bold;"
        target="_self" class="mi-unselectable"><%= GetCompanyName() %></a></li>
        <%=GetMenu() %>

        <% if (this.IsBuilder())
           { %>
    <li style="display:;"><a href="#">工具</a>
        <ul style="display: none;" class="second-menu subMenu1 mi-unselectable">
            <li>
                <a href="#">创建&nbsp;&nbsp;>></a>
                <ul style="display: none;" class="fourth-menu mi-unselectable">
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
                <a href="/app/InfoGrid2/View/OneValue/ValueTableMgr.aspx">简单业务流程</a>
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
                    <li><a href="/app/InfoGrid2/View/Biz/Core_Company/EditCompanyInfo.aspx"">公司</a></li>
                    <li><a href="/app/InfoGrid2/View/Biz/Core_Menu/EditMenuTree.aspx"">菜单设置</a></li>
                    <li><a href="/app/InfoGrid2/View/Biz/Input_Data/Test.aspx"">导入插件设置...</a></li>
                    <li><a href="/App/InfoGrid2/Sec/ArrLogin/ArrLoginList.aspx">管理下级权限模块</a></li>
                    <li><a href="/App/InfoGrid2/View/DisplayRule/RuleEdit.aspx">显示规则</a></li> 
                     <li><a href="/App/InfoGrid2/View/Biz/Input_Data/Table1CopyToTable2.aspx">拷贝数据</a></li> 

                    
                     <li><a href="/app/infogrid2/View/SysBugRepair/SecTableRepart.aspx">权限数据修复</a></li> 
                </ul>
            </li> 
            <li>
                <a href="#">流程&nbsp;&nbsp;>> </a>
                <ul style="display: none;" class="fourth-menu">
                    <li><a href="/app/InfoGrid2/View/OneFlowBuilder/FlowDefineList.aspx"">流程定义列表</a></li>
                    <a href="/App/InfoGrid2/View/OneFlowBuilder/FlowDocList.aspx">全部流程实例</a>
                    <a href="/App/InfoGrid2/View/OneFlowBuilder/FlowDocListForUser.aspx">当前用户流程实例</a>
                </ul>
            </li> 
            <li>
                <a href="#">权限设置&nbsp;&nbsp;>> </a>
                <ul style="display: none;" class="fourth-menu">
                    <li>
                        <a href="/app/InfoGrid2/Sec/UIFilterDesigner/MainTableList.aspx">(设计师用)角色权限定义</a>
                    </li>
                    <li>
                        <a href="/app/InfoGrid2/Sec/UIFilter/MainUserList.aspx">(管理员用)角色权限定义</a>
                    </li>
                    <li>
                        <a href="/app/InfoGrid2/Sec/UIFilterTU/MainTableList.aspx">(设计师用)用户权限定义</a>
                    </li>
                    <li>
                        <a href="/app/InfoGrid2/Sec/UIFilter2/MainTableList.aspx">(管理员用)用户权限定义</a>
                    </li>
                    <li>
                        <a href="/App/InfoGrid2/Sec/StructDefine/TreeStruceEdit.aspx">系统结构管理</a>
                    </li> 
                    <li>
                        <a href="/App/InfoGrid2/Sec/M2User/UserSecSetup1.aspx">用户菜单定义</a>
                    </li>  
                    <li>
                        <a href="/App/InfoGrid2/Sec/User/LoginPassEditM2.aspx">修改密码</a>
                    </li>  
                </ul>
            </li>

            <li>
                <a href="#">打印管理&nbsp;&nbsp;>> </a>
                <ul style="display: none;" class="fourth-menu">
                    <li><a href="/app/InfoGrid2/View/PrintManager/EditPrintInfo.aspx"">打印助手管理</a></li>
                    <li><a href="/app/InfoGrid2/View/PrintManager/SelectPrintInfo.aspx"">打印机管理</a></li>
                </ul>
            </li>             
            <li>
                <a href="/App/InfoGrid2/View/Biz/Core_Code/CodeMgr.aspx">编码规则管理</a>
            </li> 


            <li>
                <a href="/App/InfoGrid2/View/LogAct/LogActList.aspx">调试信息</a>
            </li> 

            <li>
                <a href="#">其它定制模块&nbsp;&nbsp;>> </a>
                <ul style="display: none;" class="fourth-menu">

                    
                </ul>
            </li> 
            <li>
                <a href="/App/InfoGrid2/View/Biz/Core_Catalog/CataTree.aspx">目录</a>
            </li> 
            <li>
                <a href="/App/InfoGrid2/View/Biz/Core_Catalog/CataTypeList.aspx">目录类型</a>
            </li> 

            <li>
                <a href="/App/InfoGrid2/View/DbSetup/ClearRubbish.aspx">数据清理</a>
            </li> 
            
            <li>
                <a href="/App/InfoGrid2/View/MoreActionBuilder/DwgList.aspx">联动v3</a>
            </li> 
            <li>
                <a href="/App/InfoGrid2/View/CMS/CMSItemList.aspx">CMS</a>
            </li> 
        </ul>
    </li>    

    <% } %>

</ul>



<div style="float:right; height:37px; width:100%; background-color:#333333; vertical-align: bottom;" 
    class="AText">
    <div style="height:16px;"></div>
    <span style="float:right;" id="user_info">
    
        <a class="TimeX mi-unselectable" href="#" id="TimeX"></a>

        <a class="UserInfo mi-unselectable" href="#"  >加载...</a>


        <a href="#" onclick="window.location.href='/App/BizExplorer/View/Logout.aspx'" class="mi-unselectable" >退出</a>
        &nbsp;&nbsp;
    </span>
</div>

<script type="text/javascript">

    function findTab(url) {

        tabs = tabs || window['widget1_I_layout1'];
        
        var items = tabs.items;

        var len = items.length;

        var curTab = null;

        for (var i = 0; i < len; i++) {

            var item = items[i];

            if (item.url == url) {

                curTab = item;

                break;
            }

        }

        return curTab;

    }

    function AddTab2(sender) {

        var url = $(this).attr("href");
        var label = $(this).text();

        if (url == "#" || url == "") {
            return false;
        }

        tabs = tabs || window['widget1_I_layout1'];
        
        var curTab = findTab(url);

        
        var tab = curTab || tabs.add({
            url: url,
            text: label,
            label: label,
            closable: true,
            iframe:true,
            scroll: 'none'
        });

        if (tabs.muid) {
            tabs.setActiveTab(tab);
        }
        else {
            var panel = tab.panel;
            $(panel).attr("srcUrl", url);
        }

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

        //setTimeout(getUserInfo,1000);

        timeReset();
    });

    function timeReset() {



        Mini2.create('Mini2.Timer', {

            interval: 10 * 1000,

            tick: function () {

                getUserInfo();
            }

        });
    }

    function getUserInfo() {

        $.get('/app/InfoGrid2/View/Explorer/SysHeartbeat.aspx?tag=USER_INFO&' + Mini2.Guid.newGuid(), function (data, state) {

            try {

                var obj = eval('(' + data + ')');

                var panel = $('#user_info');

                var timeXEl = panel.find('.TimeX');
                var uInfoEl = panel.find('.UserInfo');

                timeXEl.text(obj.time);
                uInfoEl.text(obj.loginName + '(' + obj.roleName + ')');

                if (obj.roleName === '临时虚拟用户') {
                    window.location.href = "/app/infogrid2/login/index.aspx";
                    return;
                }

                if (obj.isVirtual == 1) {
                    window.location.href = "/app/infogrid2/login/index.aspx";
                }



            }
            catch (ex) {
                alert("xxx");
            }

        });
    }

</script>