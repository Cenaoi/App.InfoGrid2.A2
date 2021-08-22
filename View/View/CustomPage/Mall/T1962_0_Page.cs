using App.BizCommon;
using EasyClick.BizWeb2;
using EC5.Entity.Expanding.ExpandV1;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.View.CustomPage.Mall
{
    /// <summary>
    /// 服装厂的颜色规格档案表
    /// </summary>
    public class T1962_0_Page : ExPage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit()
        {


            MainStore.Inserted += MainStore_Inserted;

            MainStore.Updated += MainStore_Updated;

            MainStore.Deleted += MainStore_Deleted;




        }

        private void MainStore_Deleted(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            LModel lm = e.Object as LModel;

            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter("MALL_SPEC");
            filter.And("SPEC_ID", lm.GetPk());
            filter.And("SPEC_TYPE", "colour");
            LModel lmSpec = decipher.GetModel(filter);

            if (lmSpec == null)
            {
                return;
            }

            lmSpec["ROW_SID"] = -3;
            lmSpec["ROW_DATE_UPDATE"] = DateTime.Now;


            decipher.UpdateModelProps(lmSpec, "ROW_SID", "ROW_DATE_UPDATE");


        }

        private void MainStore_Updated(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {



            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = e.Object as LModel;


            LModel lm380 = decipher.GetModelByPk("UT_380", lm.GetPk());


            BizFilter filter = new BizFilter("MALL_SPEC");
            filter.And("SPEC_ID", lm.GetPk());
            filter.And("SPEC_TYPE", "colour");

            LModel lmSpec = decipher.GetModel(filter);

            if (lmSpec == null)
            {
                lmSpec = InsertData(lm);
                
            }

            lmSpec["SPEC_NO"] = lm380["COL_3"];
            lmSpec["SPEC_TEXT"] = lm380["COL_4"];
            lmSpec["CATALOG_CODE"] = lm380["COL_1"];
            lmSpec["CATALOG_TEXT"] = lm380["COL_2"];
            lmSpec["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.UpdateModelProps(lmSpec, "SPEC_NO", "SPEC_TEXT", "CATALOG_CODE", "CATALOG_TEXT", "ROW_DATE_UPDATE");



        }

        private void MainStore_Inserted(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = e.Object as LModel;


            InsertData(lm);


        }

        LModel InsertData(LModel lm)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lmSpec = new LModel("MALL_SPEC");
            lmSpec["SPEC_ID"] = lm["ROW_IDENTITY_ID"];
            lmSpec["SPEC_TYPE"] = "colour";
            lmSpec["ROW_DATE_CREATE"] = lmSpec["ROW_DATE_UPDATE"] = DateTime.Now;
            lmSpec["PK_SPEC_CODE"] = BillIdentityMgr.NewCodeForDay("SPEC_CODE", "S");

            decipher.InsertModel(lmSpec);

            return lmSpec;

        }


        }
    }