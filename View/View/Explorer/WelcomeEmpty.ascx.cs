using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.IG2.Core;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;


namespace App.InfoGrid2.View.Explorer
{
    public partial class WelcomeEmpty :WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public bool IsBuilder()
        {
            EcUserState userState = EcContext.Current.User;

            return userState.Roles.Exist(IG2Param.Role.BUILDER);

        }


        private string GetMenuUrl(BIZ_C_MENU item)
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


            url = StringUtil.NoBlank(url, "#");

            return url;
        }

        public string GetHtmlCommonMenu()
        {
            EcContext context = EcContext.Current;
            EcUserState userState = context.User;

            int parentId = 100; //默认从100开始

            int secLevel = userState.LoginID.Equals(IG2Param.Role.ADMIN, StringComparison.Ordinal) ? 6 : 0;

            if (userState.Roles.Exist(IG2Param.Role.BUILDER))
            {
                secLevel = 20;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();


            LModelList<BIZ_C_MENU> models;

            if (context.User.Roles.Exist(IG2Param.Role.BUILDER))
            {

                models = decipher.SelectModels<BIZ_C_MENU>(
                    LOrder.By("PARENT_ID,SEQ,BIZ_C_MENU_ID"),
                    "ROW_SID >= 0 AND MENU_ENABLED=1 AND SEC_FUN_ID <={0} ", secLevel);

            }
            else
            {

                int[] secFunIds = M2Helper.GetUserMenuId();


                LightModelFilter filter = new LightModelFilter(typeof(BIZ_C_MENU));
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter.And("MENU_ENABLED", true);
                filter.And("BIZ_C_MENU_ID", secFunIds, Logic.In);
                filter.And("SEC_FUN_ID", secLevel, Logic.LessThanOrEqual);
                filter.TSqlOrderBy = "PARENT_ID,SEQ,BIZ_C_MENU_ID";

                models = decipher.SelectModels<BIZ_C_MENU>(filter);

            }

            if (models == null || models.Count == 0)
            {
                return string.Empty;
            }


            LModelGroup<BIZ_C_MENU, int> groups = models.ToGroup<int>("PARENT_ID");

            LModelList<BIZ_C_MENU> root0;

            if (!groups.TryGetValue(parentId, out root0))
            {
                return string.Empty;
            }



            StringBuilder sb = new StringBuilder();

            //root0.Sort("SEQ","BIZ_C_MENU_ID");

            int n = 0;


            foreach (BIZ_C_MENU item in root0)
            {

                //string text = item.NAME;
                //string url = GetMenuUrl(item);

                ProMenuLevel1(sb, item, groups);


                //SModel t = new SModel();
                //t["ICON_CHAT"] = item.ICON_CHAT;
                //t["ICON_COLOR"] = item.ICON_COLOR;
                //t["ICON_BG_COLOR"] = item.ICON_BG_COLOR;
                //t["URL"] = url;
                //t["TEXT"] = text;
                //t["ID"] = item.BIZ_C_MENU_ID;


                //sb.Append(t.ToJson());
            }



            return sb.ToString();

        }

        private void ProMenuLevel1(StringBuilder sb, BIZ_C_MENU item, LModelGroup<BIZ_C_MENU, int> groups)
        {
            string url = GetMenuUrl(item);

            bool hasChild = groups.ContainsKey(item.BIZ_C_MENU_ID);

            sb.AppendLine("<li >");
            sb.AppendLine($"  <a href=\"{url}\" class='dropdown-toggle' >");
            sb.AppendLine("    <i class='menu-icon fa fa-list' ></i>");
            sb.AppendLine($"    <span class='menu-text'>{item.NAME}</span>");

            if (hasChild)
            {
                sb.AppendLine("    <b class='arrow fa fa-angle-down'></b>");
            }

            sb.AppendLine("  </a>");
            sb.AppendLine("  <b class='arrow'></b>");

            ProChildMenuLevel2(sb, item, groups);

            sb.AppendLine("</li>");

        }

        private void ProChildMenuLevel2(StringBuilder sb, BIZ_C_MENU parent, LModelGroup<BIZ_C_MENU, int> groups)
        {
            LModelList<BIZ_C_MENU> childs = null;

            if (!groups.TryGetValue(parent.BIZ_C_MENU_ID, out childs))
            {
                return;
            }

            if (childs.Count == 0)
            {
                return;
            }

            sb.AppendLine("<ul class='submenu nav-hide' style='display: none; '>");

            foreach (var subItem in childs)
            {
                ProMenuLevel2(sb, subItem, groups);
            }

            sb.AppendLine("</ul>");

        }

        private void ProMenuLevel2(StringBuilder sb, BIZ_C_MENU item, LModelGroup<BIZ_C_MENU, int> groups)
        {
            string url = GetMenuUrl(item);
            bool hasChild = groups.ContainsKey(item.BIZ_C_MENU_ID);

            sb.AppendLine("<li >");
            sb.AppendLine($"  <a href=\"{url}\"  class='dropdown-toggle'> ");
            sb.AppendLine("    <i class='menu-icon fa fa-caret-right'></i>");
            sb.AppendLine(item.NAME);

            if (hasChild)
            {
                sb.AppendLine("    <b class='arrow fa fa-angle-down'></b>");
            }

            sb.AppendLine("  </a>");
            sb.AppendLine("  <b class='arrow'></b>");

            ProChildMenuLevel3(sb, item, groups);

            sb.AppendLine("</li>");
        }


        private void ProChildMenuLevel3(StringBuilder sb, BIZ_C_MENU parent, LModelGroup<BIZ_C_MENU, int> groups)
        {
            LModelList<BIZ_C_MENU> childs = null;

            if (!groups.TryGetValue(parent.BIZ_C_MENU_ID, out childs))
            {
                return;
            }

            if (childs.Count == 0)
            {
                return;
            }

            sb.AppendLine("<ul class='submenu nav-hide' style='display: none; '>");

            foreach (var subItem in childs)
            {
                ProMenuLevel3(sb, subItem, groups);
            }

            sb.AppendLine("</ul>");

        }

        private void ProMenuLevel3(StringBuilder sb, BIZ_C_MENU item, LModelGroup<BIZ_C_MENU, int> groups)
        {
            string url = GetMenuUrl(item);

            sb.AppendLine("<li >");
            sb.AppendLine($"  <a href=\"{url}\" class='dropdown-toggle'>");
            sb.AppendLine("    <i class='menu-icon fa fa-caret-right'></i>");
            sb.AppendLine(item.NAME);
            //sb.AppendLine("    <b class='arrow fa fa-angle-down'></b>");
            sb.AppendLine("  </a>");
            sb.AppendLine("  <b class='arrow'></b>");


            sb.AppendLine("</li>");
        }


    }
}