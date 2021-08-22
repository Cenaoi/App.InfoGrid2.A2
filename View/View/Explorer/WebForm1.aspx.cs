using App.BizCommon;
using EC5.SystemBoard;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model.DataSet;
using System.Linq;

namespace App.InfoGrid2.View.Explorer
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


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
        /// 图形数组标题
        /// </summary>
        public string m_grapTitel;

        /// <summary>
        /// 图形数组数据
        /// </summary>
        public string m_grapData;


        /// <summary>
        /// 未出订单
        /// </summary>
        public string m_wcddNum = "0";

        /// <summary>
        /// 车间生产
        /// </summary>
        public string m_cjscNum = "0";

        /// <summary>
        /// 采购在途
        /// </summary>
        public string m_cgztNum = "0";


        /// <summary>
        /// 发外未归
        /// </summary>
        public string m_fwwgNum = "0";



        protected void Page_Load(object sender, EventArgs e)
        {
            EcUserState user = EcContext.Current.User;

            if (user.Roles.Count == 0 && string.IsNullOrEmpty(user.ExpandPropertys["ARR_ROLE_ID"]))
            {
                Error404.Send("请登录！");
                return;
            }



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


        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {



            DbDecipher decipher = ModelAction.OpenDecipher();



            #region 获取左边的曲线图数据

            DateTime beginDate = DateTime.Now.AddMonths(-1);
            DateTime endDate = DateTime.Now;

            string sql = "select CONVERT(varchar(12) ,COL_2, 111 ) as COL_2 ,sum(COL_9) as COL_9 ,sum(COL_13) as COL_13 from UT_090 "+
                            "where ROW_SID >=0 and COL_2 >= '"+beginDate.ToString("yyyy-MM-dd hh:mm:ss")+"' and COL_2 <= '"+endDate.ToString("yyyy-MM-dd hh:mm:ss")+"'"+
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




            #region 获取图形报表界面的值

            StringBuilder sbGrapTitel = new StringBuilder();
            StringBuilder sbGrapData = new StringBuilder();


            List<LModel> lm241List = decipher.GetModelList("select * from UT_241 where ROW_SID >=0 ");

            int j = 0;

            sbGrapTitel.Append("[");

            sbGrapData.Append("[");

            foreach (var item in lm241List)
            {
                if(j++> 0)
                {
                    sbGrapTitel.Append(",");
                    sbGrapData.Append(",");
                }


                string name = item.Get<string>("COL_3");

                string value = item.Get<decimal>("COL_5").ToString("0.######");


                sbGrapTitel.Append($"'{name}  {value}'");

                sbGrapData.Append($"{{value:{value}, name:'{name}  {value}'}}");


            }


            sbGrapTitel.Append("]");

            sbGrapData.Append("]");


            m_grapTitel = sbGrapTitel.ToString();

            m_grapData = sbGrapData.ToString();

            #endregion

            //发外未归
            var vs957 = ViewSet.Select(decipher, 957);

            // 未出订单
            var vs879 = ViewSet.Select(decipher, 879);

            //采购在途
            var vs882 = ViewSet.Select(decipher, 882);

            //车间在产
            var vs880 = ViewSet.Select(decipher, 880);



            var sql879   =  ViewMgr.GetCountForTSql(vs879);
            m_wcddNum = decipher.ExecuteScalar<string>(sql879);

            var sql880 = ViewMgr.GetCountForTSql(vs880);
            m_cjscNum = decipher.ExecuteScalar<string>(sql880);


            var sql882 = ViewMgr.GetCountForTSql(vs882);
            m_cgztNum = decipher.ExecuteScalar<string>(sql882);

            var sql957 = ViewMgr.GetCountForTSql(vs957);
            m_fwwgNum = decipher.ExecuteScalar<string>(sql957);


            //GetChart1Data();

            //GetChart2Data();



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

            DateTime Yesterday =  new DateTime(nowdate.Year, nowdate.Month, nowdate.Day - 1, 0, 0, 0);  //昨天日期

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

            DicInit(data3,col_18s);

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


        public void DicInit(Dictionary<string, decimal> dic,string[] col_18s)
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


    }
}