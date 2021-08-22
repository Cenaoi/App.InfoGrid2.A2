using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Model;
using System.Text;
using EC5.SystemBoard.Interfaces;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model.SecModels;
using HWQ.Entity.Filter;
using EC5.IG2.Core;
using HWQ.Entity;
using EC5.Utility;

namespace App.InfoGrid2.View.Explorer
{
    public partial class Menu_v2015 : System.Web.UI.Page, IView
    {

        protected override void OnInit(EventArgs e)
        {
            this.Context.Items["ResponseJS"] = true;
            this.Response.ExpiresAbsolute = DateTime.Now.AddDays(-20);

            base.OnInit(e);

            
        }

        /// <summary>
        /// 是否为设计师
        /// </summary>
        /// <returns></returns>
        public bool IsBuilder()
        {
            
            return this.EcUser.Roles.Exist(IG2Param.Role.BUILDER);
        }


        public EC5.SystemBoard.EcUserState EcUser
        {
            get
            {
                EcUserState user = EC5.SystemBoard.EcContext.Current.User;

                return user;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 获取公司名称
        /// </summary>
        /// <returns></returns>
        public string GetCompanyName()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            BIZ_C_COMPANY item = decipher.SelectToOneModel<BIZ_C_COMPANY>("ROW_SID >= 0");

            if (item != null)
            {
                return item.SHORT_NAME;
            }

            return "EasyClick 软件开发公司";

        }


        /// <summary>
        ///从数据库获取菜单
        /// </summary>
        /// <returns></returns>
        public string GetMenu() 
        {
            EcContext context = EcContext.Current;
            EcUserState userState = context.User;



            int secLevel = userState.LoginID.Equals(IG2Param.Role.ADMIN, StringComparison.Ordinal) ? 6 : 0;

            if (userState.Roles.Exist(IG2Param.Role.BUILDER))
            {
                secLevel = 20;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();
            int parentId = 100;

            LModelList<BIZ_C_MENU> models;

            if(context.User.Roles.Exist(IG2Param.Role.BUILDER)){

                models = decipher.SelectModels<BIZ_C_MENU>(
                    LOrder.By("PARENT_ID,SEQ,BIZ_C_MENU_ID"),
                    "ROW_SID >= 0 AND MENU_ENABLED=1 AND SEC_FUN_ID <={0} ", secLevel);

            }
            else{
            
                int[] secFunIds = M2Helper.GetUserMenuId();

           
                LightModelFilter filter = new LightModelFilter(typeof(BIZ_C_MENU));
                filter.And("ROW_SID",0, Logic.GreaterThanOrEqual);
                filter.And("MENU_ENABLED", true);
                filter.And("BIZ_C_MENU_ID", secFunIds, Logic.In);
                filter.And("SEC_FUN_ID", secLevel, Logic.LessThanOrEqual);
                filter.TSqlOrderBy = "PARENT_ID,SEQ,BIZ_C_MENU_ID";

                models = decipher.SelectModels<BIZ_C_MENU>(filter);

            }

            if(models == null || models.Count == 0)
            {
                return "你的账号未经授权，请联系系统管理员";
            }


            LModelGroup<BIZ_C_MENU, int> groups = models.ToGroup<int>("PARENT_ID");

            LModelList<BIZ_C_MENU> root0;

            if (!groups.TryGetValue(parentId, out root0))
            {
                return "空" ;
            }



            StringBuilder sb = new StringBuilder();

            //root0.Sort("SEQ","BIZ_C_MENU_ID");

            foreach (BIZ_C_MENU item in root0)
            {
                sb.AppendFormat("<li><a href=\"{0}\" class=\"mi-unselectable\" >{1}</a>", StringUtil.NoBlank( item.URI,"#"), item.NAME);

                RecursiveMenu(sb, groups, item.BIZ_C_MENU_ID, "second-menu subMenu1");

                sb.Append("</li>");
            }


            return sb.ToString() ;
        }

        /// <summary>
        /// 递归获取子菜单
        /// </summary>
        /// <param name="items">菜单集合</param>
        /// <param name="sb">菜单Html</param>
        /// <param name="groups">所有菜单集合</param>
        public void RecursiveMenu(StringBuilder sb, LModelGroup<BIZ_C_MENU, int> groups, int parentId, string className)
        {
            if (!groups.ContainsKey(parentId))
            {
                return;
            }

            LModelList<BIZ_C_MENU> items = groups[parentId];

            if (items.Count == 0)
            {
                return;
            }

            sb.AppendFormat("<ul style=\"display: none;\" class=\"{0} mi-unselectable\">", className);


            //items.Sort("SEQ", "BIZ_C_MENU_ID");

            foreach (BIZ_C_MENU item in items)
            {
                string url = item.URI;

                if (!string.IsNullOrEmpty(url))
                {
                    if (!string.IsNullOrEmpty(item.SEC_PAGE_TAG))
                    {
                        if (!url.Contains("?"))
                        {
                            url += "?";
                        }

                        url += "&sec_tag=" + item.SEC_PAGE_TAG;
                    }

                    if (!string.IsNullOrEmpty(item.ALIAS_TITLE))
                    {
                        if (!url.Contains("?"))
                        {
                            url += "?";
                        }

                        url += "&alias_title=" + item.ALIAS_TITLE;
                    }

                    if (!url.Contains("?"))
                    {
                        url += "?";
                    }

                    url += "&menu_id=" + item.BIZ_C_MENU_ID;
                }

                sb.AppendFormat("<li><a href=\"{0}\" class=\"mi-unselectable\">{1}</a>", StringUtil.NoBlank( url,"#"), item.NAME);


                RecursiveMenu(sb, groups, item.BIZ_C_MENU_ID, "fourth-menu");

                sb.Append("</li>");

            }

            sb.Append("</ul>");

        }

       

    }
}