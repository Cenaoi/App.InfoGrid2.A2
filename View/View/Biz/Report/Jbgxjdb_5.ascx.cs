using App.BizCommon;
using EC5.IO;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Report
{
    public partial class Jbgxjdb_5 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetBX();
            GetHZX();
            GetSZXOne();
            GetSZXTwo();
        }



       public string smlisttitle = "";
       public  string smlistji="";
 
        /// <summary>
        /// 类型饼状图
        /// </summary>
        public void GetBX()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql = "select count(COL_42) as NEW_ROW,COL_42  from UT_265 where ROW_SID>=0 GROUP by COL_42";

            LModelList<LModel> modellist = decipher.GetModelList(sql);

            SModelList smList = new SModelList();

            foreach (LModel model in modellist)
            {
              
                SModel sm = new SModel();
             
                sm["value"] = model["NEW_ROW"];
                sm["name"] = model["COL_42"];

                if (string.IsNullOrWhiteSpace(sm["name"]))
                {
                    continue;
                }
                smList.Add(sm);
                if (smlisttitle.Length != 0)
                {
                    smlisttitle += ",";
                }
            
                  
                    smlisttitle +="'"+ sm["name"] +"'";


                
            }
            smlistji= smList.ToJson();
            

        }



       public  string hzxname= "";
       public string hzxbiaode = "";
        /// <summary>
        /// 收支付类型
        /// </summary>
        public void GetHZX() {

            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql = "select sum(COL_33) as NEW_ROW,COL_58  from UT_265 where ROW_SID>=0 GROUP by COL_58";

            LModelList<LModel> modellist = decipher.GetModelList(sql);


           

            SModelList sm = new SModelList();


            foreach (LModel model in modellist)
            {
                if (string.IsNullOrWhiteSpace(model["COL_58"]))
                {
                    continue;
                }

                if (hzxname.Length != 0)
                {
                    hzxname += ",";
                    hzxbiaode += ",";

                }

                    hzxname += "'" + model["COL_58"] +"'";



              


                    hzxbiaode += model["NEW_ROW"];

             

            }

        }







        public string hetongxdf = "";
        public string weifuje = "";
        /// <summary>
        /// 逾期未收款
        /// </summary>
        public void GetSZXOne() {
            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql = "select Sum(COL_55) as NEW_ROW,COL_37  from UT_265 where ROW_SID>=0 and  COL_58='应收类' and COL_56<getdate() GROUP by COL_37";

            LModelList<LModel> modellist = decipher.GetModelList(sql);


            SModelList sm = new SModelList();


            foreach (LModel model in modellist)
            {
                if (string.IsNullOrWhiteSpace(model["COL_37"]))
                {
                    continue;
                }

                if (hetongxdf.Length != 0)
                {
                    hetongxdf += ",";
                    weifuje += ",";
                }            
                    hetongxdf += "'" + model["COL_37"] + "'";
                    weifuje +=model.Get<decimal>("NEW_ROW").ToString("0.##");
              
            }


        }



       

        public string hetongfu = "";
        public string weifujew = "";

        /// <summary>
        /// 逾期未付款
        /// </summary>
        public void GetSZXTwo() {


            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql = "select Sum(COL_55) as NEW_ROW,COL_37  from UT_265 where ROW_SID>=0 and  COL_58='应付类' and COL_56<getdate() GROUP by COL_37";

            LModelList<LModel> modellist = decipher.GetModelList(sql);


            SModelList sm = new SModelList();


            foreach (LModel model in modellist)
            {
                if (string.IsNullOrWhiteSpace(model["COL_37"]))
                {
                    continue;
                }

                if (hetongfu.Length != 0)
                {
                    hetongfu += ",";
                    weifujew += ",";
                }

                    hetongfu += "'" + model["COL_37"] + "'";
                    weifujew += model.Get<decimal>("NEW_ROW").ToString("0.##");

              

            }


        }
    }
}