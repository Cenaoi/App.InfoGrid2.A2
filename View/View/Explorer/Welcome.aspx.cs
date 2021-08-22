using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.SecModels;
using EC5.IG2.Core;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Explorer
{
    public partial class Welcome : System.Web.UI.Page,IView
    {

        //拿到动态导航的一级菜单
        public List<BIZ_C_MENU> mainMenus;

        public int CurMenuId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                InitData();
            }
        }

        /// <summary>
        /// 初始化一级菜单
        /// </summary>
        public void InitData() 
        {
            DbDecipher decipher = ModelAction.OpenDecipher();


            DateTime date = DateTime.Parse("2015-08-11");


            LightModelFilter filter = new LightModelFilter(typeof(BIZ_C_MENU));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("PARENT_ID", 70);

            filter.TSqlOrderBy = "SEQ";

            #region 如果不是设计师。就进行菜单筛选

            bool isFilterMenu = GlobelParam.GetValue<bool>("IS_FILTER_MENU",false,"过滤首页菜单");

            if (isFilterMenu)
            {
                EcContext context = EcContext.Current;
                EcUserState userState = context.User;

                if (!userState.Roles.Exist(IG2Param.Role.BUILDER))
                {
                    int[] menuIds = M2Helper.GetUserMenuId();
                    filter.And("OWNER_MENU_ID", menuIds, Logic.In);
                }
            }

            #endregion

            mainMenus = decipher.SelectModels<BIZ_C_MENU>(filter);

            if (mainMenus.Count > 0)
            {
                CurMenuId = mainMenus[0].BIZ_C_MENU_ID;
            }

        }




    }
}