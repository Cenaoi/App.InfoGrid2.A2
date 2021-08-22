using App.BizCommon;
using App.InfoGrid2.Bll;
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
    public partial class WelcomeJB3 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                try
                {
                    InitData();
                    GetChart1Data();
                    GetChart2Data();
                    GetChart3Data();
                    GetPieChart1Data();
                    GetPieChart2Data();

                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

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


        public string chart1_data = string.Empty;


        /// <summary>
        /// 销售出货情况柱状形报表数据
        /// </summary>
        public void GetChart1Data()
        {

            string[] col_18s = { "胶盒成品", "彩盒成品", "吸塑成品" };  //产品类型

            List<string> legend = new List<string>();  //时间
            legend.Add("昨天");

            DbDecipher decipher = ModelAction.OpenDecipher();

            DateTime nowdate = DateTime.Now;

            DateTime Yesterday = new DateTime(nowdate.Year, nowdate.Month, nowdate.Day - 1, 0, 0, 0);  //昨天日期

            DateTime YesterdayEnd = new DateTime(nowdate.Year, nowdate.Month, nowdate.Day, 0, 0, 0);

            string sql1 = $"select SUM(COL_9) as COL_9,COL_18  From UT_097 where COL_25 >= '{Yesterday}' and COL_25 < '{YesterdayEnd}' and COL_18 in ({GetCOL_18(col_18s)}) group by COL_18 ";   //最近一天数据

            SModelList sm1 = decipher.GetSModelList(sql1);

            Dictionary<string, decimal> data1 = new Dictionary<string, decimal>();

            DicInit(data1, col_18s);

            if (sm1 != null)
            {
                foreach (var item in sm1)
                {
                    if (item["COL_9"] == null)
                    {
                        continue;
                    }
                    data1[item["COL_18"]] = item.Get<decimal>("COL_9");
                }
            }

            nowdate = DateTime.Now;

            Yesterday = new DateTime(nowdate.Year - 1, nowdate.Month, nowdate.Day - 1, 0, 0, 0);

            YesterdayEnd = new DateTime(nowdate.Year - 1, nowdate.Month, nowdate.Day, 0, 0, 0);

            string sql2 = $"select SUM(COL_9) as COL_9,COL_18  From UT_097 where COL_25 >= '{Yesterday}' and COL_25 < '{YesterdayEnd}' and COL_18 in ({GetCOL_18(col_18s)}) group by COL_18 ";

            SModelList sm2 = decipher.GetSModelList(sql2);

            legend.Add($"{nowdate.Year - 1}年同天");

            Dictionary<string, decimal> data2 = new Dictionary<string, decimal>();

            DicInit(data2, col_18s);

            if (sm2 != null)
            {
                foreach (var item in sm2)
                {
                    if (item["COL_9"] == null)
                    {
                        continue;
                    }
                    data2[item["COL_18"]] = item.Get<decimal>("COL_9");
                }
            }

            nowdate = DateTime.Now;

            Yesterday = new DateTime(nowdate.Year - 2, nowdate.Month, nowdate.Day - 1, 0, 0, 0);

            YesterdayEnd = new DateTime(nowdate.Year - 2, nowdate.Month, nowdate.Day, 0, 0, 0);

            string sql3 = $"select SUM(COL_9) as COL_9,COL_18  From UT_097 where COL_25 >= '{Yesterday}' and COL_25 < '{YesterdayEnd}' and COL_18 in ({GetCOL_18(col_18s)}) group by COL_18 ";

            SModelList sm3 = decipher.GetSModelList(sql3);

            legend.Add($"{nowdate.Year - 2}年同天");

            Dictionary<string, decimal> data3 = new Dictionary<string, decimal>();

            DicInit(data3, col_18s);

            if (sm3 != null)
            {
                foreach (var item in sm3)
                {
                    if (item["COL_9"] == null)
                    {
                        continue;
                    }
                    data3[item["COL_18"]] = item.Get<decimal>("COL_9");
                }
            }

            SModel sm_data = new SModel();




            sm_data["data1"] = data1.Values.ToArray();

            sm_data["data2"] = data2.Values.ToArray();

            sm_data["data3"] = data3.Values.ToArray();

            sm_data["data_legend"] = legend.ToArray();

            sm_data["title"] = col_18s;

            chart1_data = sm_data.ToJson();


        }


        /// <summary>
        /// 产品类型名称数组转成 Sql In()使用的字符串 
        /// </summary>
        /// <param name="col_18s"></param>
        /// <returns></returns>
        public string GetCOL_18(string[] col_18s)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in col_18s)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                sb.Append($"'{item}'");
                sb.Append(",");
            }

            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }


        public void DicInit(Dictionary<string, decimal> dic, string[] col_18s)
        {
            foreach (var item in col_18s)
            {
                dic.Add(item, 0);
            }
        }


        public string chart2_data = string.Empty;


        /// <summary>
        /// 销售退货情况柱状形报表数据
        /// </summary>
        public void GetChart2Data()
        {
            string[] col_18s = { "胶盒成品", "彩盒成品", "吸塑成品" };  //产品类型

            List<string> legend = new List<string>();  //时间
            legend.Add("昨天");

            DbDecipher decipher = ModelAction.OpenDecipher();

            DateTime nowdate = DateTime.Now;

            DateTime Yesterday = new DateTime(nowdate.Year, nowdate.Month, nowdate.Day - 1, 0, 0, 0);  //昨天日期

            DateTime YesterdayEnd = new DateTime(nowdate.Year, nowdate.Month, nowdate.Day, 0, 0, 0);

            string sql = $"select SUM(COL_8) as COL_8,COL_17  From UT_099 where COL_23 >= '{Yesterday}' and COL_23 < '{YesterdayEnd}' and COL_17 in ({GetCOL_18(col_18s)}) group by COL_17 ";   //最近一月数据

            SModelList sm1 = decipher.GetSModelList(sql);

            Dictionary<string, decimal> data1 = new Dictionary<string, decimal>();

            DicInit(data1, col_18s);


            if (sm1 != null)
            {
                foreach (var item in sm1)
                {
                    if (item["COL_8"] == null)
                    {
                        continue;
                    }
                    data1[item["COL_17"]] = item.Get<decimal>("COL_8");
                }
            }




            string sql2 = $"select SUM(COL_8) as COL_8,COL_17  From UT_099 where COL_23 >= dateadd(month,-1,getdate()) and COL_17 in ({GetCOL_18(col_18s)}) group by COL_17 ";   //最近一天数据

            SModelList sm2 = decipher.GetSModelList(sql2);

            legend.Add("最近30天");

            Dictionary<string, decimal> data2 = new Dictionary<string, decimal>();

            DicInit(data2, col_18s);

            if (sm2 != null)
            {
                foreach (var item in sm2)
                {
                    if (item["COL_8"] == null)
                    {
                        continue;
                    }
                    data2[item["COL_17"]] = item.Get<decimal>("COL_8");
                }
            }



            SModel sm_data = new SModel();

            sm_data["data1"] = data1.Values.ToArray();

            sm_data["data2"] = data2.Values.ToArray();

            sm_data["title"] = col_18s;

            sm_data["data_legend"] = legend.ToArray();

            chart2_data = sm_data.ToJson();
        }

        public string chart3_data = string.Empty;


        /// <summary>
        /// 采购情况柱状形报表数据
        /// </summary>
        public void GetChart3Data()
        {


            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql = "select SUM(COL_8) as COL_8  From UT_161 where COL_3 >= dateadd(month,-1,getdate()) ";   //最近一月数据

            SModel sm1 = decipher.GetSModel(sql);

            List<decimal> data1 = new List<decimal>();

            if (sm1["COL_8"] == null)
            {
                data1.Add(0);
            }
            else
            {
                data1.Add(sm1.Get<decimal>("COL_8"));
            }


            DateTime nowdate = DateTime.Now;

            DateTime Yesterday = new DateTime(nowdate.Year, nowdate.Month, nowdate.Day - 1, 0, 0, 0);  //昨天日期

            DateTime YesterdayEnd = new DateTime(nowdate.Year, nowdate.Month, nowdate.Day, 0, 0, 0);

            string sql2 = $"select SUM(COL_8) as COL_8  From UT_161 where COL_3 >= '{Yesterday}' and COL_3 < '{YesterdayEnd}' ";   //最近一天数据

            SModel sm2 = decipher.GetSModel(sql2);

            List<decimal> data2 = new List<decimal>();

            if (sm2["COL_8"] == null)
            {
                data2.Add(0);
            }
            else
            {
                data2.Add(sm2.Get<decimal>("COL_8"));
            }


            SModel sm_data = new SModel();

            sm_data["data1"] = data1.ToArray();

            sm_data["data2"] = data2.ToArray();

            List<string> title = new List<string>();

            title.Add("最近30天/昨天");

            sm_data["title"] = title.ToArray();

            chart3_data = sm_data.ToJson();



        }



        public string pie_chart1_title = string.Empty;
        public string pie_chart1_data = string.Empty;

        /// <summary>
        /// 昨天销售出货情况饼形报表数据
        /// </summary>
        public void GetPieChart1Data()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            string[] col_18s = { "胶盒成品", "彩盒成品", "吸塑成品" };  //产品类型

            DateTime nowdate = DateTime.Now;

            DateTime Yesterday = new DateTime(nowdate.Year, nowdate.Month, nowdate.Day - 1, 0, 0, 0);  //昨天日期

            DateTime YesterdayEnd = new DateTime(nowdate.Year, nowdate.Month, nowdate.Day, 0, 0, 0);

            string sql = $"select SUM(COL_8) as COL_8,COL_18  From UT_097 where COL_25 >= '{Yesterday}' and COL_25 < '{YesterdayEnd}' and COL_18 in ({GetCOL_18(col_18s)}) group by COL_18";   //最近一天数据

            SModelList sms = decipher.GetSModelList(sql);

            SModelList data = new SModelList();

            foreach (var item in sms)
            {
                if (item["COL_8"] == null)
                {
                    continue;
                }

                if (item.Get<decimal>("COL_8") == 0)
                {
                    continue;
                }

                SModel sm = new SModel();

                sm["value"] = item.Get<decimal>("COL_8");

                sm["name"] = item.Get<string>("COL_18");

                data.Add(sm);

            }

            pie_chart1_title = GetPieTitle(col_18s);
            pie_chart1_data = data.ToJson();

        }


        public string GetPieTitle(string[] title)
        {
            StringBuilder str_title = new StringBuilder();

            str_title.Append("[");

            foreach (var item in title)
            {
                str_title.Append($"'{item}'");
                str_title.Append(",");
            }

            str_title.Remove(str_title.Length - 1, 1);

            str_title.Append("]");

            return str_title.ToString();

        }


        public string pie_chart2_title = string.Empty;
        public string pie_chart2_data = string.Empty;


        /// <summary>
        /// 未生产销售订单占比率饼形报表数据
        /// </summary>
        public void GetPieChart2Data()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            string[] col_18s = { "胶盒成品", "彩盒成品", "吸塑成品" };  //产品类型

            DateTime nowdate = DateTime.Now;

            DateTime Yesterday = new DateTime(nowdate.Year - 1, nowdate.Month, nowdate.Day - 1, 0, 0, 0);  //昨天日期

            DateTime YesterdayEnd = new DateTime(nowdate.Year - 1, nowdate.Month, nowdate.Day, 0, 0, 0);

            string sql = $"select SUM(COL_17) as COL_17,COL_1  From UT_091 where  COL_17>0 and BIZ_SID=2 and COL_1 in ({GetCOL_18(col_18s)}) group by COL_1";   //最近一天数据

            SModelList sms = decipher.GetSModelList(sql);

            SModelList data = new SModelList();

            foreach (var item in sms)
            {
                if (item["COL_17"] == null)
                {
                    continue;
                }

                if (item.Get<decimal>("COL_17") == 0)
                {
                    continue;
                }

                SModel sm = new SModel();

                sm["value"] = item.Get<decimal>("COL_17");

                sm["name"] = item.Get<string>("COL_1");

                data.Add(sm);

            }

            pie_chart2_title = GetPieTitle(col_18s);
            pie_chart2_data = data.ToJson();
        }



        /// <summary>
        /// 时间数组 js用的
        /// </summary>
        public string m_dateList;


        /// <summary>
        /// 数量数组 js用的
        /// </summary>
        public string m_munberList;

        /// <summary>
        /// 金额数组 js用的
        /// </summary>
        public string m_moneyList;



        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {



            DbDecipher decipher = ModelAction.OpenDecipher();



            #region 获取左边的曲线图数据

            DateTime beginDate = DateTime.Now.AddMonths(-1);
            DateTime endDate = DateTime.Now;

            string sql = "select CONVERT(varchar(12) ,COL_2, 111 ) as COL_2 ,sum(COL_9) as COL_9 ,sum(COL_13) as COL_13 from UT_090 " +
                            "where ROW_SID >=0 and COL_2 >= '" + beginDate.ToString("yyyy-MM-dd hh:mm:ss") + "' and COL_2 <= '" + endDate.ToString("yyyy-MM-dd hh:mm:ss") + "'" +
                            "group by CONVERT(varchar(12) ,COL_2, 111 )";


            List<LModel> lm090List = decipher.GetModelList(sql);

            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            StringBuilder sb3 = new StringBuilder();

            sb1.Append("[");
            sb2.Append("[");
            sb3.Append("[");

            for (int i = 0; i < lm090List.Count; i++)
            {
                if (i > 0)
                {
                    sb1.Append(",");
                    sb2.Append(",");
                    sb3.Append(",");
                }

                LModel item = lm090List[i];


                //这是时间
                sb1.AppendFormat("\"{0}\"", item["COL_2"]);
                //这是金额数据
                sb2.AppendFormat("\"{0}\"", item.Get<decimal>("COL_9").ToString("0.####"));
                //这是数量
                sb3.AppendFormat("\"{0}\"", item.Get<int>("COL_13"));

            }

            sb1.Append("]");
            sb2.Append("]");
            sb3.Append("]");


            m_dateList = sb1.ToString();
            m_moneyList = sb2.ToString();
            m_munberList = sb3.ToString();

            #endregion




        }


    }
}