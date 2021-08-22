using App.BizCommon;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.IG2.BizBase;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.View.CustomPage
{
    /// <summary>
    /// 生成入库单用的
    /// </summary>
    public class T1267_546_Page:ExPage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit()
        {
            Button tbar = this.FindControl("Toolbar1") as Button;

            




        }


        /// <summary>
        /// 提交之后事件
        /// </summary>
        //public void ChangeBizSID(string psStr)
        //{

        //    LModel lmMain = MainStore.GetFirstData() as LModel;

        //    if (lmMain.Get<int>("BIZ_SID") != 4)
        //    {
        //        return;
        //    }


        //    DbDecipher decipher = ModelAction.OpenDecipher();

        //    LightModelFilter lmFilter = new LightModelFilter("UT_113");
        //    lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
        //    lmFilter.And("COL_1", lmMain.GetPk());
        //    lmFilter.And("BIZ_SID", 4);
        //    lmFilter.And("COL_61", "1");

        //    List<LModel> lm113s = decipher.GetModelList(lmFilter);


        //    foreach (var item in lm113s)
        //    {


        //        LightModelFilter lmFilter342 = new LightModelFilter("UT_342");
        //        lmFilter342.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
        //        lmFilter342.And("COL_52", item["COL_21"]);
        //        lmFilter342.And("COL_50", item["COL_2"]);

        //        List<LModel> lm342s = decipher.GetModelList(lmFilter342);


        //        decimal col_27 = item.Get<decimal>("COL_27");

        //        decimal poor = col_27;


        //        foreach (var lm342 in lm342s)
        //        {

        //            decimal col_36 = lm342.Get<decimal>("COL_36");

        //            lm342.SetTakeChange(true);

        //            lm342["ROW_DATE_UPDATE"] = DateTime.Now;

        //            if (poor > col_36)
        //            {
        //                lm342["COL_74"] = col_36;
        //                lm342["COL_75"] = col_36;
        //                poor = poor - col_36;
        //            }

        //            else
        //            {
        //                lm342["COL_74"] = poor;
        //                lm342["COL_75"] = poor;

        //                DbCascadeRule.Update(lm342);


        //                break;
        //            }



        //        }

        //    }

        //}


        public void ChangeBizSID_After(string psStr)
        {

            DataRecordCollection drc =   MainTable.CheckedRows;

            if(drc.Count == 0)
            {
                return;
            }


            ChangeBizSidCurrency(drc.GetIds());


        }


        /// <summary>
        /// 这是给黄哥的同步用友数据完后执行的
        /// </summary>
        /// <param name="ids"></param>
        public void ChangeBizSidCurrency(string[] ids)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter lmFilter112 = new LightModelFilter("UT_112");
            lmFilter112.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter112.And("ROW_IDENTITY_ID", ids, Logic.In);

            List<LModel> lm112s = decipher.GetModelList(lmFilter112);



            foreach (var lmMain in lm112s)
            {


                if (lmMain.Get<int>("BIZ_SID") != 4)
                {
                    continue;
                }


                LightModelFilter lmFilter = new LightModelFilter("UT_113");
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("COL_1", lmMain.GetPk());
                lmFilter.And("BIZ_SID", 4);
                lmFilter.And("COL_61", "1");

                List<LModel> lm113s = decipher.GetModelList(lmFilter);


                foreach (var item in lm113s)
                {


                    LightModelFilter lmFilter342 = new LightModelFilter("UT_342");
                    lmFilter342.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    lmFilter342.And("COL_52", item["COL_21"]);
                    lmFilter342.And("COL_50", item["COL_2"]);

                    List<LModel> lm342s = decipher.GetModelList(lmFilter342);


                    decimal col_27 = item.Get<decimal>("COL_27");

                    decimal poor = col_27;


                    foreach (var lm342 in lm342s)
                    {

                        decimal col_36 = lm342.Get<decimal>("COL_36");

                        if (col_36 <= 0)
                        {
                            continue;
                        }

                        lm342.SetTakeChange(true);

                        lm342["ROW_DATE_UPDATE"] = DateTime.Now;


                        lm342["COL_60"] = item["COL_62"];
                        lm342["COL_61"] = item["COL_61"];
                        lm342["COL_53"] = item["COL_4"];
                        lm342["COL_65"] = item["COL_48"];

                        lm342["COL_71"] = item["COL_29"];
                        lm342["COL_73"] = item["COL_28"];
                        lm342["COL_76"] = item["COL_43"];

                        lm342["COL_54"] = item["COL_5"];
                        lm342["COL_55"] = item["COL_6"];
                        lm342["COL_56"] = item["COL_7"];

                        lm342["COL_69"] = item["COL_73"];

                        lm342["COL_77"] = DateTime.Now;
                        lm342["COL_42"] = "117"
                        ;
                        lm342["COL_43"] = "生产入库单";

                        lm342["COL_41"] = DateTime.Now;
                        lm342["COL_45"] = item["ROW_IDENTITY_ID"];
                        lm342["COL_46"] = item["COL_1"];
                        lm342["COL_44"] = item["COL_12"];
                        lm342["COL_50"] = item["COL_2"];

                        if (poor > col_36)
                        {
                            lm342["COL_74"] = col_36;
                            lm342["COL_75"] = col_36;
                            lm342["COL_47"] = col_36;
                            poor = poor - col_36;

                            //decipher.UpdateModel(lm342);
                            DbCascadeRule.Update(lm342);

                        }

                        else
                        {
                            lm342["COL_74"] = poor;
                            lm342["COL_75"] = poor;
                            lm342["COL_47"] = poor;

                            decipher.UpdateModel(lm342);
                            DbCascadeRule.Update(lm342);


                            break;
                        }

                    }

                }

            }

        }





    }
}