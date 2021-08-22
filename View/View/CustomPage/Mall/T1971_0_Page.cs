using App.BizCommon;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5.Entity.Expanding.ExpandV1;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.View.CustomPage.Mall
{
    /// <summary>
    /// 服装厂的材料档案列表用的
    /// </summary>
    public class T1971_0_Page : ExPage
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit()
        {

            MainStore.Inserted += MainStore_Inserted;

            MainStore.Deleted += MainStore_Deleted;

            MainStore.Updated += MainStore_Updated;


            Store store383 = FindControl("Store_UT_383") as Store;

            store383.Inserted += Store383_Inserted;
            store383.Updated += Store383_Updated;
            store383.Deleted += Store383_Deleted;



            Store store384 = FindControl("Store_UT_384") as Store;

            store384.Inserted += Store384_Inserted;
            store384.Updated += Store384_Updated;
            store384.Deleted += Store384_Deleted;




        }

        private void Store384_Deleted(object sender, ObjectEventArgs e)
        {
            LModel lm = e.Object as LModel;

            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter("MALL_PROD_SPEC");
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

        private void Store384_Updated(object sender, ObjectEventArgs e)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = e.Object as LModel;


            LModel lm380 = decipher.GetModelByPk("UT_384", lm.GetPk());


            BizFilter filter = new BizFilter("MALL_PROD_SPEC");
            filter.And("SPEC_ID", lm.GetPk());
            filter.And("SPEC_TYPE", "colour");

            LModel lmSpec = decipher.GetModel(filter);

            if (lmSpec == null)
            {
                return;
            }

            lmSpec["SPEC_NO"] = lm380["COL_58"] ?? "";
            lmSpec["SPEC_TEXT"] = lm380["COL_59"] ?? "";
            lmSpec["CATALOG_CODE"] = lm380["COL_56"] ?? "";
            lmSpec["CATALOG_TEXT"] = lm380["COL_57"] ?? "";
            lmSpec["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.UpdateModelProps(lmSpec, "SPEC_NO", "SPEC_TEXT", "CATALOG_CODE", "CATALOG_TEXT", "ROW_DATE_UPDATE");

        }

        private void Store384_Inserted(object sender, ObjectEventArgs e)
        {
            string row_pk = WebUtil.QueryTrim("row_pk");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = e.Object as LModel;

            BizFilter filter = new BizFilter("MALL_PROD");
            filter.And("PROD_ID", row_pk);

            LModel lmProd = decipher.GetModel(filter);


            LModel lmSpec = new LModel("MALL_PROD_SPEC");
            lmSpec["FK_PROD_CODE"] = lmProd["PK_PROD_CODE"];
            lmSpec["SPEC_ID"] = lm["ROW_IDENTITY_ID"];
            lmSpec["SPEC_TYPE"] = "colour";
            lmSpec["ROW_DATE_CREATE"] = lmSpec["ROW_DATE_UPDATE"] = DateTime.Now;
            lmSpec["PK_SPEC_CODE"] = BillIdentityMgr.NewCodeForDay("SPEC_CODE", "S");

            decipher.InsertModel(lmSpec);

        }

        private void Store383_Deleted(object sender, ObjectEventArgs e)
        {

            LModel lm = e.Object as LModel;

            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter("MALL_PROD_SPEC");
            filter.And("SPEC_ID", lm.GetPk());
            filter.And("SPEC_TYPE", "size");
            LModel lmSpec = decipher.GetModel(filter);

            if (lmSpec == null)
            {
                return;
            }

            lmSpec["ROW_SID"] = -3;
            lmSpec["ROW_DATE_UPDATE"] = DateTime.Now;


            decipher.UpdateModelProps(lmSpec, "ROW_SID", "ROW_DATE_UPDATE");

        }

        private void Store383_Updated(object sender, ObjectEventArgs e)
        {


            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = e.Object as LModel;


            LModel lm383 = decipher.GetModelByPk("UT_383", lm.GetPk());

            BizFilter filter = new BizFilter("MALL_PROD_SPEC");
            filter.And("SPEC_ID", lm.GetPk());
            filter.And("SPEC_TYPE", "size");

            LModel lmSpec = decipher.GetModel(filter);

            if (lmSpec == null)
            {
                return;
            }

            lmSpec["SPEC_NO"] = lm383["COL_58"] ?? "";
            lmSpec["SPEC_TEXT"] = lm383["COL_59"] ?? "";
            lmSpec["CATALOG_CODE"] = lm383["COL_56"] ?? "";
            lmSpec["CATALOG_TEXT"] = lm383["COL_57"] ?? "";
            lmSpec["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.UpdateModelProps(lmSpec, "SPEC_NO", "SPEC_TEXT", "CATALOG_CODE", "CATALOG_TEXT", "ROW_DATE_UPDATE");

        }

        private void Store383_Inserted(object sender, ObjectEventArgs e)
        {

            string row_pk = WebUtil.QueryTrim("row_pk");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = e.Object as LModel;

            BizFilter filter = new BizFilter("MALL_PROD");
            filter.And("PROD_ID", row_pk);

            LModel lmProd = decipher.GetModel(filter);


            LModel lmSpec = new LModel("MALL_PROD_SPEC");
            lmSpec["FK_PROD_CODE"] = lmProd["PK_PROD_CODE"];
            lmSpec["SPEC_ID"] = lm["ROW_IDENTITY_ID"];
            lmSpec["SPEC_TYPE"] = "size";
            lmSpec["ROW_DATE_CREATE"] = lmSpec["ROW_DATE_UPDATE"] = DateTime.Now;
            lmSpec["PK_SPEC_CODE"] = BillIdentityMgr.NewCodeForDay("SPEC_CODE", "S");

            decipher.InsertModel(lmSpec);

        }

        private void MainStore_Updated(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = e.Object as LModel;


            BizFilter filter = new BizFilter("MALL_PROD");
            filter.And("PROD_ID", lm.GetPk());

            LModel lmProd = decipher.GetModel(filter);

            if (lmProd == null)
            {
                return;
            }


            LModel lm083 = decipher.GetModelByPk("UT_083", lm.GetPk());

            lmProd.SetTakeChange(true);

            lmProd["CAN_SALE"] = lm083["COL_61"] ?? false;
            lmProd["HOME_VISIBLE"] = lm083["COL_58"] ?? false;
            lmProd["IS_COMMON"] = lm083["COL_62"] ?? false;
            lmProd["PROD_TYPE_CODE"] = lm083["COL_12"] ?? "";
            lmProd["PROD_TYPE_TEXT"] = lm083["COL_1"] ?? "";
            lmProd["ROW_DATE_UPDATE"] = DateTime.Now;
            lmProd["PROD_NO"] = lm083["COL_2"] ?? "";
            lmProd["PROD_TEXT"] = lm083["COL_3"] ?? "";
            lmProd["BAR_CODE"] = lm083["COL_23"] ?? "";
            lmProd["PROD_INTRO"] = lm083["COL_57"] ?? "";
            lmProd["PROD_THUMB"] = lm083["COL_54"] ?? "";
            lmProd["MULTIPLE_PHOTO_LIST"] = lm083["COL_55"] ?? "";
            lmProd["STORE_CODE"] = lm083["COL_11"] ?? "";
            lmProd["STORE_TEXT"] = lm083["COL_9"] ?? "";
            lmProd["PRICE_MARKET"] = lm083["COL_21"] ?? 0;
            lmProd["PRICE_MEMBER"] = lm083["COL_21"] ?? 0;

            //decipher.UpdateModelProps(lmProd, "PROD_TYPE_CODE", "PROD_TYPE_TEXT", "ROW_DATE_UPDATE", "PROD_NO", "PROD_TEXT", "BAR_CODE", "PROD_INTRO", "PROD_THUMB", "MULTIPLE_PHOTO_LIST", "STORE_CODE", "STORE_TEXT", "PRICE_MARKET", "PRICE_MEMBER");

            decipher.UpdateModel(lmProd, true);

        }

        private void MainStore_Deleted(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = e.Object as LModel;


            BizFilter filter = new BizFilter("MALL_PROD");
            filter.And("PROD_ID", lm.GetPk());

            LModel lmProd = decipher.GetModel(filter);

            if(lmProd == null)
            {
                return;
            }

            lmProd["ROW_SID"] = -3;
            lmProd["ROW_DATE_DELETE"] = DateTime.Now;

            decipher.UpdateModelProps(lmProd, "ROW_SID", "ROW_DATE_DELETE");






        }

        private void MainStore_Inserted(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = e.Object as LModel;

            try
            {


                LModel lmSpec = new LModel("MALL_PROD");
                lmSpec["PROD_ID"] = lm["ROW_IDENTITY_ID"];
                lmSpec["ROW_DATE_CREATE"] = lmSpec["ROW_DATE_UPDATE"] = DateTime.Now;
                lmSpec["PK_PROD_CODE"] = BillIdentityMgr.NewCodeForDay("PROD_CODE", "P");

                decipher.InsertModel(lmSpec);


            }catch(Exception ex)
            {
                log.Error(ex);
            }

        }
    }
}