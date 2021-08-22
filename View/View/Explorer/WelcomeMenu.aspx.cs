using App.BizCommon;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using App.InfoGrid2.Model;
using EC5.SystemBoard;
using HWQ.Entity.Filter;
using HWQ.Entity;
using App.InfoGrid2.Model.SecModels;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;

namespace App.InfoGrid2.View.Explorer
{
    public partial class WelcomeMenu : System.Web.UI.Page,IView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = WebUtil.Query("action");
            if (action == "main") 
            {
                GetMainMenu();
            }
            else if (action == "right") 
            {
                GetSubMenu();
            }
        }

        /// <summary>
        /// 拿到动态导航的快捷菜单
        /// </summary>
        void GetMainMenu() 
        {
            EcUserState user = EcContext.Current.User;

            if (user.Identity == 0 || user.LoginName == "临时虚拟用户") 
            {

                return;
            }

            StringBuilder sb = new StringBuilder();

            string html = "<a href='{0}' title='{1}' onclick='return AddTab2(this);' class='btn btn-default bigger-230 col-md-3 col-sm-4 col-xs-6' >";

            html += "<img class='tile-image big-illustration'  src='{2}' />";

            html +="<div class='text'>{1}</div></a>";
                               
            
            int id = WebUtil.QueryInt("id");
            
            
            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter lmFilter = new LightModelFilter(typeof(BIZ_C_MENU));
            lmFilter.And("PARENT_ID",id);
            lmFilter.And("ROW_SID",0, Logic.GreaterThanOrEqual);
            lmFilter.And("MENU_TYPE_ID", "QUICK");

            BIZ_C_MENU quickMenu = decipher.SelectToOneModel<BIZ_C_MENU>(lmFilter);

            if (quickMenu == null) 
            {

                return;
            }




            LightModelFilter filter2 = new LightModelFilter(typeof(BIZ_C_MENU));
            filter2.And("ROW_SID", 0);
            filter2.And("PARENT_ID", quickMenu.BIZ_C_MENU_ID);
            filter2.TSqlOrderBy = "SEQ";


            #region 如果不是设计师。就进行菜单筛选

            bool isFilterMenu = GlobelParam.GetValue<bool>("IS_FILTER_MENU", false, "过滤首页菜单");

            if (isFilterMenu)
            {
                EcContext context = EcContext.Current;
                EcUserState userState = context.User;

                if (!userState.Roles.Exist(IG2Param.Role.BUILDER))
                {
                    int[] menuIds = M2Helper.GetUserMenuId();
                    filter2.And("OWNER_MENU_ID", menuIds, Logic.In);
                }
            }

            #endregion

            List<BIZ_C_MENU> mainMenuList = decipher.SelectModels<BIZ_C_MENU>(filter2);

            foreach(var item in mainMenuList)
            {
                sb.AppendFormat(html, item.URI, item.NAME, item.ICO);
            }



            Response.Write(sb.ToString());


        }


        /// <summary>
        /// 拿到动态导航的子菜单
        /// </summary>
        void GetSubMenu() 
        {
            StringBuilder sb = new StringBuilder();

            string beginHtml = "<dl class='col-sm-6 col-sm-6 col-xs-12'><dt>{0}</dt>";
            string subHtml = "<dd><a href='{0}' title='{1}' onclick='return AddTab2(this)'>{1}</a></dd>";
            string endHtml = "</dl>";
                               
            int id = WebUtil.QueryInt("id");


            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter lmFilter = new LightModelFilter(typeof(BIZ_C_MENU));
            lmFilter.And("ROW_SID", 0,Logic.GreaterThanOrEqual);
            lmFilter.And("PARENT_ID", id);
            lmFilter.And("MENU_TYPE_ID", "LIST");
            lmFilter.And("MENU_ENABLED", true);

            BIZ_C_MENU quickMenu = decipher.SelectToOneModel<BIZ_C_MENU>(lmFilter);

            if (quickMenu == null)
            {
                return;
            }


            LightModelFilter filter = new LightModelFilter(typeof(BIZ_C_MENU));
            filter.And("PARENT_ID", quickMenu.BIZ_C_MENU_ID);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.TSqlOrderBy = "SEQ";

            #region 如果不是设计师。就进行菜单筛选

            bool isFilterMenu = GlobelParam.GetValue<bool>("IS_FILTER_MENU", false, "过滤首页菜单");

            int[] menuIds = null;
            bool isBuilder = false;

            EcContext context = EcContext.Current;
            EcUserState userState = context.User;

            isBuilder = userState.Roles.Exist(IG2Param.Role.BUILDER);


            if (isFilterMenu && !isBuilder)
            {
                menuIds = M2Helper.GetUserMenuId();
                filter.And("OWNER_MENU_ID", menuIds, Logic.In);
            }

            #endregion

            List<BIZ_C_MENU> mainMenuList = decipher.SelectModels<BIZ_C_MENU>(filter);


            foreach (var item in mainMenuList)
            {
                LightModelFilter filter2 = new LightModelFilter(typeof(BIZ_C_MENU));
                filter2.And("PARENT_ID", item.BIZ_C_MENU_ID);
                filter2.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter2.And("MENU_ENABLED", true);
                filter2.TSqlOrderBy = "SEQ";
                
                if (isFilterMenu && !isBuilder)
                {
                    filter.And("OWNER_MENU_ID", menuIds, Logic.In);
                }

                sb.AppendFormat(beginHtml, item.NAME);

                //拿到下面的子栏目菜单
                List<BIZ_C_MENU> subMenuList = decipher.SelectModels<BIZ_C_MENU>(filter2);

                foreach (var subItem in subMenuList) 
                {
                    sb.AppendFormat(subHtml, subItem.URI, subItem.NAME);
                }

                sb.Append(endHtml);
            }

            Response.Write(sb.ToString());
        }


    }
}