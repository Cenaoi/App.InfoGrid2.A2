using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using EC5.Entity.Expanding.ExpandV1;
using EC5.IG2.Core;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.View.Explorer
{
    public partial class WelcomeKXJ :WidgetControl, IView
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

        /// <summary>
        /// 获取性别比率
        /// </summary>
        /// <returns></returns>
        public string GetSexData()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();
            BizDecipher bizDecipher = new BizDecipher(decipher);

            BizFilter filter = new BizFilter("UT_005_SCIENTIST");
            filter.And("COL_2", "男性");

            int manCount = decipher.SelectCount(filter);

            filter = new BizFilter("UT_005_SCIENTIST");
            filter.And("COL_2", "女性");

            int womanCount = decipher.SelectCount(filter);

            SModelList data = new SModelList();

            data.Add(SModel.Parse(new
            {
                value = manCount,
                name = "男性"
            }));

            data.Add(SModel.Parse(new
            {
                value = womanCount,
                name = "女性"
            }));

            return data.ToJson();
        }

        /// <summary>
        /// 获取产业分类
        /// </summary>
        /// <returns></returns>
        public string GetCYFL()
        {
            return GetGroupsJson("UT_005_SCIENTIST", "COL_35");
        }


        /// <summary>
        /// 任职地域比例
        /// </summary>
        /// <returns></returns>
        public string GetRzdy()
        {
            return GetGroupsJson("UT_005_SCIENTIST", "COL_36");
        }

        /// <summary>
        /// 获取产业分类
        /// </summary>
        /// <returns></returns>
        public string GetGroupsJson(string table, string cataField)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter(table);
            filter.Fields = new string[] { cataField };
            filter.Distinct = true;

            LModelReader reader = decipher.GetModelReader(filter);

            string[] groupList = ModelHelper.GetColumnData<string>(reader);


            List<string> groupNames = new List<string>();
            SModelList data = new SModelList();

            foreach (var groupName in groupList)
            {
                if (StringUtil.IsBlank(groupName))
                {
                    continue;
                }


                BizFilter filter2 = new BizFilter(table);
                filter2.And(cataField, groupName);

                int count = decipher.SelectCount(filter2);

                if (count == 0)
                {
                    continue;
                }

                data.Add(SModel.Parse(new
                {
                    value = count,
                    name = groupName
                }));

                groupNames.Add(groupName);
            }


            SModel jd = new SModel();
            jd["data"] = data;
            jd["title_list"] = groupNames;


            return jd.ToJson();
        }


        /// <summary>
        /// 获取各种头衔的人数
        /// </summary>
        /// <returns></returns>
        public string GetGroup()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();
            BizDecipher bizDecipher = new BizDecipher(decipher);

            BizFilter filter = new BizFilter("UT_009");
            filter.And("COL_12", "是");
            filter.Fields = new string[] { "COL_7" };
            filter.Distinct = true;

            LModelReader reader = decipher.GetModelReader(filter);

            string[] titleList = ModelHelper.GetColumnData<string>(reader);

            List<string> titles = new List<string>(titleList);
            if (titles.Remove("其他"))
            {
                titles.Add("其他");
            }


            List<string> groupNames = new List<string>();

            SModelList data = new SModelList();

            foreach (var title in titles)
            {
                if (StringUtil.IsBlank(title))
                {
                    continue;
                }

                string sqlVTitle = title.Replace("'", "''");

                BizFilter filter2 = new BizFilter("UT_005_SCIENTIST");
                filter2.And("ROW_IDENTITY_ID", new FilterSQL($"select distinct COL_1 from UT_009 where ROW_SID >= 0 and COL_7='{sqlVTitle}'"), Logic.In);

                int count = decipher.SelectCount(filter2);

                if (count == 0)
                {
                    continue;
                }

                data.Add(SModel.Parse(new
                {
                    value = count,
                    name = title
                }));

                groupNames.Add(title);
            }


            SModel jd = new SModel();
            jd["data"] = data;
            jd["title_list"] = groupNames;


            return jd.ToJson();

        }

        public string GetEmptyAndBl()
        {
            string[] fields = new string[]
            {
                "COL_1","COL_2","COL_3","COL_5","COL_6","COL_7", "COL_8","COL_9","COL_11","COL_12","COL_13","COL_14",
                "COL_15","COL_16","COL_17","COL_18","COL_23","COL_24","COL_25","COL_26","COL_35"
            };

            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT ");
            sb.Append("sum(").AppendLine();

            int n = 0;
            foreach (var item in fields)
            {
                if (n++ > 0)
                {
                    sb.Append(" + ").AppendLine();
                }

                sb.Append($"(case when ({item} is null or datalength({item}) = 0) then 1 else 0 end)");
            }

            sb.AppendLine();
            sb.Append(") FROM  UT_005_SCIENTIST where ROW_SID >= 0 ");

            DbDecipher decipher = ModelAction.OpenDecipher();

            int emptyCount = decipher.ExecuteScalar<int>(sb.ToString());

            int rowCount = decipher.ExecuteScalar<int>("select count(ROW_IDENTITY_ID) FROM  UT_005_SCIENTIST where ROW_SID >= 0 ");

            int allCount = rowCount * fields.Length;    //总记录 * 字段数 = 所有单元格数

            int noEmptyCount = allCount - emptyCount;   // 总记录数 - 空白记录数= 非空记录数

            SModelList data = new SModelList();

            data.Add(SModel.Parse(new {
                value = emptyCount,
                name = "未填写"
            }));

            data.Add(SModel.Parse(new
            {
                value = noEmptyCount,
                name = "已填写"
            }));

            SModel jd = new SModel();
            jd["data"] = data;
            jd["title_list"] = new string[] { "已填写", "未填写" } ;


            return jd.ToJson();
        }


        public string GetEmptyAndBl2()
        {
            string[] fields = new string[]
            {
                "COL_1","COL_2","COL_3","COL_5","COL_6","COL_7", "COL_8","COL_9","COL_11","COL_12","COL_13","COL_14",
                "COL_15","COL_16","COL_17","COL_18","COL_23","COL_24","COL_25","COL_26","COL_35"
            };

            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT ");
            sb.Append("").AppendLine();

            int n = 0;
            foreach (var item in fields)
            {
                if (n++ > 0)
                {
                    sb.Append(" , ").AppendLine();
                }

                sb.Append($"SUM(case when ({item} is null or datalength({item}) = 0) then 1 else 0 end) as {item}");
            }

            sb.AppendLine();
            sb.Append(" FROM  UT_005_SCIENTIST where ROW_SID >= 0 ");

            string tSql = sb.ToString();

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModel dd = decipher.GetSModel(tSql);

            int rowCount = decipher.ExecuteScalar<int>("select count(ROW_IDENTITY_ID) FROM  UT_005_SCIENTIST where ROW_SID >= 0 ");

            //int allCount = rowCount * fields.Length;    //总记录 * 字段数 = 所有单元格数

            //int noEmptyCount = allCount - emptyCount;   // 总记录数 - 空白记录数= 非空记录数

            LModelElement modelElem = LModelDna.GetElementByName("UT_005_SCIENTIST");

            //SModelList data = new SModelList();

            List<string> titleList = new List<string>();
            List<int> emptyList = new List<int>();
            List<int> notEmptyList = new List<int>();

            LModelFieldElement fieldElem;

            foreach (var field in dd.GetFields())
            {
                if(modelElem.TryGetField(field,out fieldElem))
                {
                    titleList.Add(fieldElem.Description);

                    int emptyCount = dd.Get<int>(field);

                    int notEmptyCount = rowCount - emptyCount;

                    emptyList.Add(emptyCount);
                    notEmptyList.Add(notEmptyCount);
                }

            }
            
            
            SModel jd = new SModel();
            jd["title_list"] = titleList;

            jd["data_1"] = notEmptyList ;
            jd["data_2"] = emptyList;


            return jd.ToJson();
        }
    }
}