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
using EC5.SystemBoard;
using EC5.IG2.Core;

namespace App.InfoGrid2.View.Biz.Report
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Jbgxjdb_4 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);




        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                InitChart();
                YuJingD();
                YyJingG();

            }


        }


        public bool IsBuilder()
        {
            EcUserState userState = EcContext.Current.User;

            return userState.Roles.Exist(IG2Param.Role.BUILDER);

        }


        public string columnChart_data1 = string.Empty;

        public string columnChart_data2 = string.Empty;


        private void InitChart()
        {
            GetUt279Data1();

            GetUt279Data2();

            GetUt162Data1();
        }


        /// <summary>
        /// 第一个图形报表获取数据
        /// </summary>
        private void GetUt279Data1()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql = "select max(COL_49) as MAXDATE  from UT_279 where ROW_SID>=0 ";

            SModel sm = decipher.GetSModel(sql);

            if (string.IsNullOrWhiteSpace(sm.Get<string>("MAXDATE")))
            {
                sm["MAXDATE"] = DateTime.Now;
            }

            DateTime maxdate = sm.Get<DateTime>("MAXDATE");

            maxdate = new DateTime(maxdate.Year, maxdate.Month, 01);

            DateTime begTime = new DateTime(maxdate.Year, maxdate.Month, 01, 00, 00, 00);

            DateTime endTime = DateUtil.EndByMonth(begTime);




            LightModelFilter efilter = new LightModelFilter("UT_279");
            efilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            efilter.And("COL_49", begTime, Logic.GreaterThanOrEqual);
            efilter.And("COL_49", endTime, Logic.LessThanOrEqual);
            efilter.And("COL_71", 0, Logic.GreaterThan);
            efilter.And("COL_3", "", Logic.Inequality);
            efilter.And("COL_7", 0, Logic.GreaterThan);

            List<LModel> ut279s = decipher.GetModelList(efilter);

            List<string> col3 = new List<string>();

            List<string> col7 = new List<string>();

            List<string> col69 = new List<string>();

            SModel smdata = new SModel();


            foreach (var item in ut279s)
            {

                col3.Add($"{item.Get<string>("COL_3") + "-" + item.Get<string>("COL_4")}");

                decimal col7_danjia = item.Get<decimal>("COL_7");

                col7.Add($"{col7_danjia.ToString("0.##")}");


                decimal col_69_danjia = item.Get<decimal>("COL_69");

                col69.Add($"{col_69_danjia.ToString("0.##")}");

            }

            smdata["title"] = col3.ToArray();

            smdata["data1"] = col7.ToArray();

            smdata["data2"] = col69.ToArray();


            columnChart_data1 = smdata.ToJson();

        }



        /// <summary>
        /// 第二个图形报表获取数据
        /// </summary>
        private void GetUt279Data2()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            string sqld = "select max(COL_49) as MAXDATE  from UT_279 where ROW_SID>=0 ";

            SModel sm = decipher.GetSModel(sqld);

            if (string.IsNullOrWhiteSpace(sm.Get<string>("MAXDATE")))
            {
                sm["MAXDATE"] = DateTime.Now;
            }

            DateTime maxdate = sm.Get<DateTime>("MAXDATE");

            maxdate = new DateTime(maxdate.Year, maxdate.Month, 01);

            DateTime begTime = new DateTime(maxdate.Year, 01, 01, 00, 00, 00);



            List<string> col7 = new List<string>();

            List<string> col69 = new List<string>();

            SModel smdata = new SModel();



            Dictionary<string, string> key_value_1 = new Dictionary<string, string>();

            Dictionary<string, string> key_value_2 = new Dictionary<string, string>();

            List<string> keys = new List<string>();

            string sql = "select SUM(COL_7) as COL_7,SUM(COL_69) as COL_69,year(COL_49) as year_49, month(COL_49) as month_49  from 	UT_279 " +
                        $" where ROW_SID >= 0 and COL_49 >= '{begTime}'  " +
                        $" group by year(COL_49), month(COL_49)";

            SModelList esm = decipher.GetSModelList(sql);


            foreach (var item in esm)
            {


                if (item["COL_7"] == null)
                {
                    item["COL_7"] = 0;
                }

                if (item["COL_69"] == null)
                {
                    item["COL_69"] = 0;
                }


                if (item.Get<decimal>("COL_7") == 0 && item.Get<decimal>("COL_69") == 0)   
                {
                    continue;
                }

                string year_49 = item.Get<string>("year_49");

                string month_49 = item.Get<string>("month_49");


                string key = $"{year_49}-{month_49}";

                keys.Add(key);


                //DateTime d49 = item.Get<DateTime>("COL_49");

                //string date49 = new DateTime(d49.Year, d49.Month, 01).ToString("yyyy-MM");

                //if (dic_col_7.ContainsKey(date49))
                //{
                //    decimal danjia = item.Get<decimal>("COL_7");

                //    dic_col_7[date49] += danjia.ToString("0.##");

                //}

                //if (dic_col_69.ContainsKey(date49))
                //{
                //    decimal danjia = item.Get<decimal>("COL_69");

                //    dic_col_69[date49] += danjia.ToString("0.##");
                //}




                if (key_value_1.ContainsKey(key))
                {

                    key_value_1[key] = item.Get<decimal>("COL_7").ToString("0.##");

                }
                else
                {
                    key_value_1.Add(key, item.Get<decimal>("COL_7").ToString("0.##"));


                }

                if (key_value_2.ContainsKey(key))
                {
                    key_value_2[key] = item.Get<decimal>("COL_69").ToString("0.##");
                }
                else
                {

                    key_value_2.Add(key, item.Get<decimal>("COL_69").ToString("0.##"));

                }

                

            }


            smdata["title"] = keys.ToArray();

            smdata["data1"] = key_value_1.Values.ToArray();

            smdata["data2"] = key_value_2.Values.ToArray();


            columnChart_data2 = smdata.ToJson();


        }



        /// <summary>
        /// 第二个图形报表获取数据
        /// </summary>
        private void GetUt279Data2v2()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            string sqld = "select max(COL_49) as MAXDATE  from UT_279 where ROW_SID>=0 ";

            SModel sm = decipher.GetSModel(sqld);

            DateTime maxdate = sm.Get<DateTime>("MAXDATE");

            maxdate = new DateTime(maxdate.Year, maxdate.Month, 01);

            DateTime begTime = new DateTime(maxdate.Year, 01, 01, 00, 00, 00);


            SModel smdata = new SModel();



            Dictionary<string, string> key_value_1 = new Dictionary<string, string>();


            List<string> keys = new List<string>();

            string sql = "select SUM(COL_7) as COL_7,SUM(COL_69) as COL_69,year(COL_49) as year_49, month(COL_49) as month_49  from 	UT_279 " +
                        $" where ROW_SID >= 0 and COL_49 >= '{begTime}'  " +
                        $" group by year(COL_49), month(COL_49)";

            SModelList esm = decipher.GetSModelList(sql);


            foreach (var item in esm)
            {


                if (item["COL_7"] == null)
                {
                    item["COL_7"] = 0;
                }

                if (item["COL_69"] == null)
                {
                    item["COL_69"] = 0;
                }


                if (item.Get<decimal>("COL_7") == 0 && item.Get<decimal>("COL_69") == 0)
                {
                    continue;
                }

                string year_49 = item.Get<string>("year_49");

                string month_49 = item.Get<string>("month_49");


                string key = $"{year_49}-{month_49}";

                keys.Add(key);

                decimal col7_value = item.Get<decimal>("COL_7");

                decimal col69_value = item.Get<decimal>("COL_69");

                decimal complete = (col69_value / col7_value) * 100;


                if (key_value_1.ContainsKey(key))
                {

                    key_value_1[key] = complete.ToString("0.##");

                }
                else
                {
                    key_value_1.Add(key, complete.ToString("0.##"));

                }

            }


            smdata["title"] = keys.ToArray();

            smdata["data1"] = key_value_1.Values.ToArray();



            columnChart_data2 = smdata.ToJson();






        }



        public string columnChart_data3 = string.Empty;

        /// <summary>
        /// 第三个图形报表获取数据
        /// </summary>
        private void GetUt162Data1()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql = "select max(ROW_DATE_CREATE) as MAXDATE  from UT_162 where ROW_SID>=0 ";

            SModel sm = decipher.GetSModel(sql);

            DateTime maxdate = sm.Get<DateTime>("MAXDATE");

            DateTime endTime = new DateTime(maxdate.Year, 01, 01, 00, 00, 00);

            LightModelFilter filter = new LightModelFilter("UT_162");
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("COL_1", "原材料", Logic.Inequality);
            //filter.And("ROW_DATE_CREATE", endTime, Logic.GreaterThanOrEqual);

            SModelList sms = decipher.GetSModelList(filter);

            if (sms == null)
            {
                return;
            }

            List<string> col1 = new List<string>();

            List<string> col2 = new List<string>();

            List<string> col3 = new List<string>();

            SModel datasm = new SModel();

            foreach (var item in sms)
            {

                if (string.IsNullOrWhiteSpace(item.Get<string>("COL_6")))
                {
                    continue;
                }

                DateTime date = item.Get<DateTime>("ROW_DATE_CREATE");

                col1.Add($"{date.ToString("yyyy-MM-dd")}");

                decimal danjia = item.Get<decimal>("COL_6");

                col2.Add($"{danjia.ToString("0.##")}");

                col3.Add($"{item.Get<string>("COL_3")}");

            }

            sm["title"] = col1.ToArray();
            sm["data1"] = col2.ToArray();
            sm["data2"] = col3.ToArray();

            columnChart_data3 = sm.ToJson();

        }


        public  string columnChart_data4 = "";
        private void YuJingD()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql = "select COL_3,COL_4,COL_35,COL_52,COL_50 from  UT_083 where  COL_50>0 and COL_50 is not null  and BIZ_SID=2";

           // string sql = "select COL_3,COL_4,COL_50,COL_35 from UT_083 where COL_50 >=0 and COL_51>0";


            List<LModel> modellist = decipher.GetModelList(sql);

            List<string> title1 = new List<string>();//标题
            List<decimal> kucunshu = new List<decimal>();
            List<decimal> cunkunzd = new List<decimal>();

            SModelList mslist = new SModelList();

            foreach (LModel model in modellist)
            {
               
                if (string.IsNullOrWhiteSpace(model["COL_3"]))
                {
                    continue;
                }



              


                    title1.Add(model["COL_3"] + "-" + model["COL_4"]);
                     kucunshu.Add(model.Get<decimal>("COL_35"));
                     cunkunzd.Add(model.Get<decimal>("COL_50"));

            }
            SModel sm = new SModel();
            sm["title"] = title1.ToArray();
            sm["data"] = kucunshu.ToArray();
            sm["data1"] = cunkunzd.ToArray();
            columnChart_data4 = sm.ToJson();



        }


        public string columnChart_data5 = "";

        private void YyJingG()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();



            string sql = "select COL_3,COL_4,COL_35,COL_52 from  UT_083 where  COL_52>0 and COL_52 is not null  and COL_53>0 and BIZ_SID=2";


            List<LModel> modellist = decipher.GetModelList(sql);

            List<string> title1 = new List<string>();
            List<decimal> kucunshu = new List<decimal>();
            List<decimal> cunkunzd = new List<decimal>();

            SModelList mslist = new SModelList();

            foreach (LModel model in modellist)
            {

                if (string.IsNullOrWhiteSpace(model["COL_3"]))
                {
                    continue;
                }



                title1.Add(model["COL_3"] + "-" + model["COL_4"]);
                kucunshu.Add(model.Get<decimal>("COL_35"));
                cunkunzd.Add(model.Get<decimal>("COL_52"));

            }
            SModel sm = new SModel();
            sm["title"] = title1.ToArray();
            sm["data"] = kucunshu.ToArray();
            sm["data1"] = cunkunzd.ToArray();
            columnChart_data5 = sm.ToJson();



        }

    }
}