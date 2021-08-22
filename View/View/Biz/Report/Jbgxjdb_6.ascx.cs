using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Report
{
    public partial class Jbgxjdb_6 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetBX();
            ProductSale();
            GetUt279Data1();
            GetUt279Data2();
        }
        public string columnChart_data1 = string.Empty;

        public string columnChart_data2 = string.Empty;


        /// <summary>
        /// 第一个图形报表获取数据
        /// </summary>
        private void GetUt279Data1()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql1 = "select max(COL_71)  as  max71  from UT_276 where ROW_SID>=0";


            SModel sm = decipher.GetSModel(sql1);
            if (sm["max71"] == null)
            {
                return;
            }

            DateTime d = sm.Get<DateTime>("max71");


            DateTime startTime = new DateTime(d.Year, d.Month, 01, 00, 00, 00);


            string sql = "select COL_3,COL_8,COL_81 from UT_276 where ROW_SID>=0 and COL_71>='"+startTime+"' and COL_71<='"+ d + "'";


            List<LModel> ut279s = decipher.GetModelList(sql);

            List<string> col3 = new List<string>();//产品名称

            List<string> col8 = new List<string>();//金额

            List<string> col81 = new List<string>();//订单金额

            SModel smdata = new SModel();


            foreach (var item in ut279s)
            {
                col3.Add($"{item.Get<string>("COL_3")}");

                col8.Add($"{item.Get<decimal>("COL_8").ToString("0.##")}");

                col81.Add($"{item.Get<decimal>("COL_81").ToString("0.##")}");

            }

            smdata["title"] = col3.ToArray();

            smdata["data1"] = col8.ToArray();

            smdata["data2"] = col81.ToArray();


            columnChart_data1 = smdata.ToJson();

        }



        /// <summary>
        /// 第二个图形报表获取数据
        /// </summary>
        private void GetUt279Data2()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql1 = "select max(COL_71)  as  max71  from UT_276 where ROW_SID>=0";


            SModel sm = decipher.GetSModel(sql1);
            if (sm["max71"] == null)
            {
                return;
            }

            DateTime d = sm.Get<DateTime>("max71");


            DateTime endTime = new DateTime(d.Year, 01, 01, 00, 00, 00);


            List<string> col8 = new List<string>();

            List<string> col81 = new List<string>();

            SModel smdata = new SModel();



            Dictionary<string, string> key_value_1 = new Dictionary<string, string>();

            Dictionary<string, string> key_value_2 = new Dictionary<string, string>();

        


            string sql = "select SUM(COL_8) as COL_8,SUM(COL_81) as COL_81,year(COL_71) as year71,month(COL_71) as month71  from UT_276 " +
                        $" where ROW_SID >= 0 and COL_71 >= '{endTime}'" +
                        $" and COL_71<='{d}' group by  year(COL_71),month(COL_71)";

            SModelList esm = decipher.GetSModelList(sql);


            foreach (var item in esm)
            {


                string key = item["year71"] +"-"+ item["month71"];

                if (item["COL_8"] == null)
                {
                    item["COL_8"] = 0;
                }

                if (item["COL_81"] == null)
                {
                    item["COL_81"] = 0;
                }


                if (key_value_1.ContainsKey(key))
                {

                    key_value_1[key] = item.Get<decimal>("COL_8").ToString("0.##");

                }
                else {
                    key_value_1.Add(key, item.Get<decimal>("COL_8").ToString("0.##"));
                }

                if (key_value_2.ContainsKey(key))
                {
                    key_value_2[key] = item.Get<decimal>("COL_81").ToString("0.##");

                }
                else {

                    key_value_2.Add(key, item.Get<decimal>("COL_81").ToString("0.##"));
                }

               

            }


            smdata["title"] = key_value_1.Keys.ToArray();

            smdata["data1"] = key_value_1.Values.ToArray();

            smdata["data2"] = key_value_2.Values.ToArray();
    

            columnChart_data2 = smdata.ToJson();


        }







        public string smlisttitle = "";
        public string smlistji = "";

        /// <summary>
        /// 客户销售额分析
        /// </summary>
        public void GetBX()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

           

            string sql = "select sum(COL_22) as NEW_ROW,COL_38 from UT_181 where  dateadd(month, -12, getdate())<COL_2 and  COl_2<getdate() and COL_1='收' GROUP by COL_38";

            LModelList<LModel> modellist = decipher.GetModelList(sql);

            SModelList smList = new SModelList();

            foreach (LModel model in modellist)
            {
                SModel sm = new SModel();

                sm["value"] = model["NEW_ROW"];

                sm["name"] = model["COL_38"];

                if (string.IsNullOrWhiteSpace(sm["name"]))
                {
                    continue;
                }
                smList.Add(sm);

                if (smlisttitle.Length != 0)
                {

                    smlisttitle += ",";
                }          
                 smlisttitle += "'" + sm["name"] + "'";
               
            }
            smlistji = smList.ToJson();


        }




       
        public string yingsk = "";
        public string yingsklist = "";


        public void ProductSale()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql = "select sum(COL_22) as NWE_ROW,COL_33,COL_35 from UT_181 where dateadd(month, -12, getdate())<COL_2 and  COl_2<getdate() and COL_1='收' GROUP by  COL_33,COL_35";

             LModelList<LModel> modellist = decipher.GetModelList(sql);

            SModelList smList = new SModelList();


            StringBuilder sb = new StringBuilder();
            foreach (LModel model in modellist)
            {

                SModel sm = new SModel();

                sm["value"] = model["NWE_ROW"];

                sm["name"] = model["COL_33"] +"-"+model["COL_35"];

                if (string.IsNullOrWhiteSpace(sm["name"]))
                {
                    continue;
                }
                smList.Add(sm);


                if (yingsk.Length != 0)
                {
                    yingsk += ",";
                }
                    yingsk += "'" + sm["name"] + "'";

            }

             yingsklist = smList.ToJson();

        }

    }
}