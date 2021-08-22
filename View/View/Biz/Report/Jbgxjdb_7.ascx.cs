using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model.JsonModel;
using App.InfoGrid2.View.OneTable;
using System.Data;
using EasyClick.BizWeb2;
using EC5.IG2.Core.UI;
using EC5.IG2.BizBase;
using HWQ.Entity.Filter;
using Newtonsoft.Json.Linq;
using EC5.IG2.Plugin;
using System.Reflection;
using System.Collections.Specialized;
using System.IO;
using App.InfoGrid2.Excel_Template;
using EC5.IG2.Plugin.Custom;
using System.Linq;

namespace App.InfoGrid2.View.Biz.Report
{
    public partial class Jbgxjdb_7 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetBX();
                GetHZX();
                GetChartData3();

            }
        }


        public string smlisttitle = "";
        public string smlistji = "";


        /// <summary>
        /// 第一个饼状图形报表数据
        /// </summary>
        public void GetBX()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql = "select max(ROW_DATE_CREATE) as MAXDATE  from UT_347 where ROW_SID>=0 ";

            SModel sm1 = decipher.GetSModel(sql);

            DateTime maxdate = sm1.Get<DateTime>("MAXDATE");

            DateTime begdate = new DateTime(maxdate.Year, maxdate.Month, 01);

            DateTime enddate = DateUtil.EndByMonth(begdate);


            string sql2 = $"select SUM(COL_19) as COL_19,COL_4  from UT_347 where ROW_SID>=0 and ROW_DATE_CREATE>='{begdate}' and ROW_DATE_CREATE<='{enddate}' GROUP by COL_4";

            LModelList<LModel> modellist = decipher.GetModelList(sql2);

            SModelList smList = new SModelList();

            foreach (LModel model in modellist)
            {
                if (model.Get<decimal>("COL_19") == 0)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(model["COL_4"]))
                {
                    continue;
                }

                SModel sm = new SModel();

                decimal col_19 = model.Get<decimal>("COL_19");

                sm["value"] = col_19.ToString("0.##");

                sm["name"] = model["COL_4"];



                smList.Add(sm);
                if (smlisttitle.Length == 0)
                {

                    smlisttitle += "'" + sm["name"] + "'";


                }
                else
                {
                    smlisttitle += "," + "'" + sm["name"] + "'";
                }
            }
            smlistji = smList.ToJson();


        }



        public string smlisttitle2 = "";
        public string smlistji2 = "";



        /// <summary>
        /// 第二个饼状图形报表数据
        /// </summary>
        public void GetHZX()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql = "select max(ROW_DATE_CREATE) as MAXDATE  from UT_347 where ROW_SID>=0 ";

            SModel sm1 = decipher.GetSModel(sql);

            DateTime maxdate = sm1.Get<DateTime>("MAXDATE");

            DateTime begdate = new DateTime(maxdate.Year, maxdate.Month, 01);

            DateTime enddate = DateUtil.EndByMonth(begdate);

            string sql2 = $"select sum(COL_19) as COL_19,COL_14  from UT_347 where ROW_SID>=0 and ROW_DATE_CREATE>='{begdate}' and ROW_DATE_CREATE<='{enddate}' GROUP by COL_14";

            LModelList<LModel> modellist = decipher.GetModelList(sql2);

            SModelList smList = new SModelList();

            foreach (LModel model in modellist)
            {
                if (model.Get<decimal>("COL_19") == 0)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(model["COL_14"]))
                {
                    continue;
                }

                SModel sm = new SModel();

                decimal col_19 = model.Get<decimal>("COL_19");

                sm["value"] = col_19.ToString("0.##");

                sm["name"] = model["COL_14"];


                smList.Add(sm);

                if (smlisttitle2.Length == 0)
                {

                    smlisttitle2 += "'" + sm["name"] + "'";


                }
                else
                {
                    smlisttitle2 += "," + "'" + sm["name"] + "'";
                }
            }

            smlistji2 = smList.ToJson();

        }







        public string hetongxdf = string.Empty;
        public string weifuje = string.Empty;


        public string chart_data3 = string.Empty;


        /// <summary>
        /// 第三个图形报表数据
        /// </summary>
        public void GetChartData3()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            string[] col15s = new string[] { "660106", "660107", "660110", "660206", "660210" };  //费用二分类编号

            LightModelFilter filter = new LightModelFilter("UT_347");
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("COL_15", col15s, Logic.In);
            filter.Fields = new string[] { "max(row_date_create) as MAXDATE" };

            SModel sm = decipher.GetSModel(filter);

            if (sm == null)
            {
                sm["MAXDATE"] = DateTime.Now;
            }

            DateTime d = sm.Get<DateTime>("MAXDATE");

            DateTime endTime = new DateTime(d.Year, 01, 01, 00, 00, 00);


            SModel smdata = new SModel();


            List<string> legend = new List<string>();

            List<string> keys = new List<string>();


            string sql = "select SUM(COL_19) as COL_19 ,COL_14,year(ROW_DATE_CREATE) as year_create, month(ROW_DATE_CREATE) as month_create   from 	UT_347 " +
                        $" where ROW_SID >= 0 and ROW_DATE_CREATE > '{endTime}' and COL_15 IN ('660106', '660107', '660110', '660206', '660210') " +
                        $" group by  year(ROW_DATE_CREATE), month(ROW_DATE_CREATE),COL_14";

            SModelList esm = decipher.GetSModelList(sql);

            foreach (var item in esm)
            {
                string col_14 = item["COL_14"];


                if (legend.IndexOf(col_14) == -1)
                {
                    legend.Add(col_14);
                }

            }

            SModelList sms = new SModelList();

            foreach (var leg in legend)
            {

                SModel legSm = new SModel();
                legSm["name"] = leg;
                legSm["type"] = "bar";


                StringBuilder sb = new StringBuilder();

                sb.Append("[");

                int i = 0;

                foreach (var item in esm)
                {
                    if (item.IsNull("COL_14"))
                    {
                        item["COL_14"] = "";
                    }

                    string col_14 = item["COL_14"];

                    if (col_14 != leg)
                    {
                        continue;
                    }

                    if (i++> 0) { sb.Append(","); };

                   

                    if (item["COL_19"] == null)
                    {
                        item["COL_19"] = 0;
                    }

                    string year_create = item.Get<string>("year_create");

                    string month_create = item.Get<string>("month_create");


                    string key = $"{year_create}-{month_create}";

                    if (!keys.Contains(key))
                    {
                        keys.Add(key);  //键集合
                    }

                    List<decimal> sbItem = new List<decimal>();

                    decimal col_19 = item.Get<decimal>("COL_19");

                    var index = keys.IndexOf(key);
                    sb.Append($"[{index},{col_19}]");


                }

                sb.Append("]");

                legSm["data"] = sb.ToString();

                sms.Add(legSm);

            }



            string series = sms.ToJson();




            smdata["title"] = keys.ToArray();

            smdata["series"] = sms;

            smdata["data6"] = legend.ToArray();



            chart_data3 = smdata.ToJson();

        }



















        public string hetongfu = string.Empty;
        public string weifujew = string.Empty;

        /// <summary>
        /// 逾期未付款
        /// </summary>
        public void GetSZXTwo()
        {


            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql = "select Sum(COL_55) as NEW_ROW,COL_37  from UT_265 where ROW_SID>=0 and  COL_58='付' and COL_56>getdate() GROUP by COL_37";

            LModelList<LModel> modellist = decipher.GetModelList(sql);


            SModelList sm = new SModelList();


            foreach (LModel model in modellist)
            {
                if (string.IsNullOrWhiteSpace(model["COL_37"]))
                {
                    continue;
                }

                if (hetongfu.Length == 0)
                {
                    hetongfu += "'" + model["COL_37"] + "'";
                    weifujew += model["NEW_ROW"];

                }
                else
                {

                    hetongfu += "," + "'" + model["COL_37"] + "'";
                    weifujew += "," + model["NEW_ROW"];
                }

            }


        }


    }
}