using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using App.BizCommon;
using Sysboard.Web.Tasks;
using App.InfoGrid2.Model.Mall;

namespace App.InfoGrid2.Mall.Task
{
    public class SyncUT083 : WebTask
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public SyncUT083()
        {
            this.TaskSpan = new TimeSpan(0, 0, 120);

            this.TaskType = WebTaskTypes.Span;

            this.TaskTime = DateTime.Parse("2011-1-1");

        }


        public override void Exec()
        {

            DbDecipher decipher = null;

            try
            {
                decipher = DbDecipherManager.GetDecipherOpen();
            }
            catch (Exception ex)
            {
                return;
            }


            try
            {


                SyncData(decipher);
                SyncUT_383Data(decipher);
                SyncUT_384Data(decipher);



            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {

                decipher.Dispose();

            }


        }


        
        public void SyncData(DbDecipher decipher)
        {




            //库存数量明细表新增最后同步时间
            DateTime insert_end_sync_date = GlobelParam.GetValue(decipher, "UT_083_INSERT_END_SYNC_DATE", new DateTime(2000, 1, 1), "材料档案表新增最后同步时间");

            //库存数量明细表更新最后同步时间
            DateTime update_end_sync_date = GlobelParam.GetValue(decipher, "UT_083_UPDATE_END_SYNC_DATE", new DateTime(2000, 1, 1), "材料档案表更新最后同步时间");

            //库存数量明细表删除最后同步时间
            DateTime detele_end_sync_date = GlobelParam.GetValue(decipher, "UT_083_DETELE_END_SYNC_DATE", new DateTime(2000, 1, 1), "材料档案表删除最后同步时间");



            string inser_sql = $"select * from UT_083 where ROW_SID >=0 and  ROW_DATE_CREATE > '{insert_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //新增数据集合
            SModelList insert_accounts = decipher.GetSModelList(inser_sql);


            string update_sql = $"select * from UT_083 where ROW_SID >=0 and  ROW_DATE_UPDATE > '{update_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //更新数据集合
            SModelList update_accounts = decipher.GetSModelList(update_sql);


            string delete_sql = $"select * from UT_083 where ROW_SID = -3 and  ROW_DATE_DELETE > '{detele_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //删除数据集合
            SModelList delete_accounts = decipher.GetSModelList(delete_sql);



            SyncInserDoc(decipher, insert_accounts);

            SyncUpdateDoc(decipher, update_accounts);

            SyncDeleteDoc(decipher, delete_accounts);


            GlobelParam.SetValue(decipher, "UT_083_INSERT_END_SYNC_DATE", DateTime.Now, "材料档案表新增最后同步时间");
            GlobelParam.SetValue(decipher, "UT_083_UPDATE_END_SYNC_DATE", DateTime.Now, "材料档案表更新最后同步时间");
            GlobelParam.SetValue(decipher, "UT_083_DETELE_END_SYNC_DATE", DateTime.Now, "材料档案表删除最后同步时间");




        }



        /// <summary>
        /// 同步尺码规格表新增的数据
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="insert_accounts">新增的用户账号集合</param>
        void SyncInserDoc(DbDecipher decipher, SModelList insert_accounts)
        {

            if (insert_accounts.Count == 0)
            {
                return;
            }

            try
            {
                LModelList<LModel> mall_prods = new LModelList<LModel>();

                //插入新的账号进报价系统
                foreach (SModel account in insert_accounts)
                {


                    LModel lmProd = new LModel("MALL_PROD");

                    lmProd["PROD_ID"] = account["ROW_IDENTITY_ID"];
                    lmProd["ROW_DATE_CREATE"] = lmProd["ROW_DATE_UPDATE"] = DateTime.Now;
                    lmProd["PK_PROD_CODE"] = BillIdentityMgr.NewCodeForDay("PROD_CODE", "P");

                    mall_prods.Add(lmProd);



                }

                decipher.InsertModels(mall_prods);


            }
            catch (Exception ex)
            {
                log.Error($"同步新增数据失败{ex}");
                throw new Exception("同步新增数据失败", ex);
            }





        }


        /// <summary>
        /// 同步尺码规格表更新过后的数据
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="update_accounts">更新过后用户账号集合</param>
        void SyncUpdateDoc(DbDecipher decipher, SModelList update_accounts)
        {

            if (update_accounts.Count == 0)
            {
                return;
            }

            try
            {
                LModelList<LModel> mall_prods = new LModelList<LModel>();

                foreach (SModel account in update_accounts)
                {

                    LightModelFilter filter = new LightModelFilter("MALL_PROD");
                    filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    filter.And("PROD_ID", account["ROW_IDENTITY_ID"]);

                    LModel lmProd = decipher.GetModel(filter);


                    if (lmProd == null)
                    {
                        continue;
                    }

                    lmProd.SetTakeChange(true);

                    lmProd["ROW_DATE_UPDATE"] = DateTime.Now;

                    CopyDoc(lmProd, account);

                    mall_prods.Add(lmProd);

                }


                decipher.UpdateModels(mall_prods, true);

            }
            catch (Exception ex)
            {
                log.Error($"同步更新数据失败{ex}");
                throw new Exception("同步更新数据失败", ex);
            }




        }


        /// <summary>
        /// 同步尺码规格表删除过后的数据
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="delete_accounts"></param>
        void SyncDeleteDoc(DbDecipher decipher, SModelList delete_accounts)
        {

            if (delete_accounts.Count == 0)
            {
                return;
            }

            LModelList<LModel> mall_specs = new LModelList<LModel>();

            foreach (SModel account in delete_accounts)
            {



                LightModelFilter lmFilter = new LightModelFilter("MALL_PROD");
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("PROD_ID", account["ROW_IDENTITY_ID"]);

                mall_specs = decipher.GetModelList(lmFilter);

                foreach (var item in mall_specs)
                {
                    item.SetTakeChange(true);

                    item["ROW_DATE_DELETE"] = DateTime.Now;
                    item["ROW_SID"] = -3;
                }


            }

            decipher.UpdateModels(mall_specs, true);

        }


        /// <summary>
        /// 拷贝尺码规格表数据
        /// </summary>
        /// <param name="lm"></param>
        /// <param name="sm"></param>
        void CopyDoc(LModel lmProd, SModel lm083)
        {

            lmProd["CAN_SALE"] = lm083["CAN_SALE"] ?? false;
            lmProd["HOME_VISIBLE"] = lm083["HOME_VISIBLE"] ?? false;
            lmProd["IS_COMMON"] = lm083["IS_COMMON"] ?? false;
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

        }




        public void SyncUT_383Data(DbDecipher decipher)
        {




            //库存数量明细表新增最后同步时间
            DateTime insert_end_sync_date = GlobelParam.GetValue(decipher, "UT_383_INSERT_END_SYNC_DATE", new DateTime(2000, 1, 1), "材料档案尺码表新增最后同步时间");

            //库存数量明细表更新最后同步时间
            DateTime update_end_sync_date = GlobelParam.GetValue(decipher, "UT_383_UPDATE_END_SYNC_DATE", new DateTime(2000, 1, 1), "材料档案尺码表更新最后同步时间");

            //库存数量明细表删除最后同步时间
            DateTime detele_end_sync_date = GlobelParam.GetValue(decipher, "UT_383_DETELE_END_SYNC_DATE", new DateTime(2000, 1, 1), "材料档案尺码表删除最后同步时间");



            string inser_sql = $"select * from UT_383 where ROW_SID >=0 and  ROW_DATE_CREATE > '{insert_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //新增数据集合
            SModelList insert_accounts = decipher.GetSModelList(inser_sql);


            string update_sql = $"select * from UT_383 where ROW_SID >=0 and  ROW_DATE_UPDATE > '{update_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //更新数据集合
            SModelList update_accounts = decipher.GetSModelList(update_sql);


            string delete_sql = $"select * from UT_383 where ROW_SID = -3 and  ROW_DATE_DELETE > '{detele_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //删除数据集合
            SModelList delete_accounts = decipher.GetSModelList(delete_sql);



            SyncUT_383InserDoc(decipher, insert_accounts);

            SyncUT_383UpdateDoc(decipher, update_accounts);

            SyncUT_383DeleteDoc(decipher, delete_accounts);


            GlobelParam.SetValue(decipher, "UT_383_INSERT_END_SYNC_DATE", DateTime.Now, "材料档案尺码表新增最后同步时间");
            GlobelParam.SetValue(decipher, "UT_383_UPDATE_END_SYNC_DATE", DateTime.Now, "材料档案尺码表更新最后同步时间");
            GlobelParam.SetValue(decipher, "UT_383_DETELE_END_SYNC_DATE", DateTime.Now, "材料档案尺码表删除最后同步时间");




        }



        /// <summary>
        /// 同步尺码规格表新增的数据
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="insert_accounts">新增的用户账号集合</param>
        void SyncUT_383InserDoc(DbDecipher decipher, SModelList insert_accounts)
        {

            if (insert_accounts.Count == 0)
            {
                return;
            }

            try
            {
                LModelList<LModel> mall_ProdSpecs = new LModelList<LModel>();

                //插入新的账号进报价系统
                foreach (SModel account in insert_accounts)
                {

                    LightModelFilter filter = new LightModelFilter("UT_083");
                    filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    filter.And("ROW_IDENTITY_ID", account["COL_54"]);

                    LModel lm083 = decipher.GetModel(filter);

                    if (lm083 == null)
                    {
                        continue;
                    }

                    LightModelFilter prod_filter = new LightModelFilter("MALL_PROD");
                    prod_filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    prod_filter.And("PROD_ID", lm083["ROW_IDENTITY_ID"]);


                    LModel lmProd = decipher.GetModel(prod_filter);


                    if (lmProd == null)
                    {
                        continue;
                    }


                    LModel lmProdSpec = new LModel("MALL_PROD_SPEC");

                    lmProdSpec["FK_PROD_CODE"] = lmProd["PK_PROD_CODE"];

                    lmProdSpec["SPEC_ID"] = account["ROW_IDENTITY_ID"];

                    lmProdSpec["SPEC_TYPE"] = "size";

                    lmProdSpec["ROW_DATE_CREATE"] = lmProdSpec["ROW_DATE_UPDATE"] = DateTime.Now;

                    lmProdSpec["PK_SPEC_CODE"] = BillIdentityMgr.NewCodeForDay("SPEC_CODE", "S");

                    mall_ProdSpecs.Add(lmProdSpec);



                }

                decipher.InsertModels(mall_ProdSpecs);


            }
            catch (Exception ex)
            {
                log.Error($"同步新增数据失败{ex}");
                throw new Exception("同步新增数据失败", ex);
            }





        }


        /// <summary>
        /// 同步尺码规格表更新过后的数据
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="update_accounts">更新过后用户账号集合</param>
        void SyncUT_383UpdateDoc(DbDecipher decipher, SModelList update_accounts)
        {

            if (update_accounts.Count == 0)
            {
                return;
            }

            try
            {
                LModelList<LModel> mall_ProdSpecs = new LModelList<LModel>();

                foreach (SModel lm383 in update_accounts)
                {

                    LightModelFilter filter = new LightModelFilter("MALL_PROD_SPEC");
                    filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    filter.And("SPEC_ID", lm383["ROW_IDENTITY_ID"]);
                    filter.And("SPEC_TYPE", "size");

                    LModel lmProdSpec = decipher.GetModel(filter);

                    if (lmProdSpec == null)
                    {
                        continue;
                    }

                    lmProdSpec.SetTakeChange(true);


                    lmProdSpec["SPEC_NO"] = lm383["COL_58"] ?? "";
                    lmProdSpec["SPEC_TEXT"] = lm383["COL_59"] ?? "";
                    lmProdSpec["CATALOG_CODE"] = lm383["COL_56"] ?? "";
                    lmProdSpec["CATALOG_TEXT"] = lm383["COL_57"] ?? "";
                    lmProdSpec["COL_6"] = lm383["COL_61"] ?? "";
                    lmProdSpec["ROW_DATE_UPDATE"] = DateTime.Now;


                    mall_ProdSpecs.Add(lmProdSpec);

                }


                decipher.UpdateModels(mall_ProdSpecs, true);

            }
            catch (Exception ex)
            {
                log.Error($"同步更新数据失败{ex}");
                throw new Exception("同步更新数据失败", ex);
            }




        }


        /// <summary>
        /// 同步尺码规格表删除过后的数据
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="delete_accounts"></param>
        void SyncUT_383DeleteDoc(DbDecipher decipher, SModelList delete_accounts)
        {


            if (delete_accounts.Count == 0)
            {
                return;
            }

            LModelList<LModel> mall_ProdSpecs = new LModelList<LModel>();

            foreach (SModel account in delete_accounts)
            {

                LightModelFilter lmFilter = new LightModelFilter("MALL_PROD_SPEC");
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("SPEC_ID", account["ROW_IDENTITY_ID"]);
                lmFilter.And("SPEC_TYPE", "size");

                mall_ProdSpecs = decipher.GetModelList(lmFilter);

                foreach (var item in mall_ProdSpecs)
                {
                    item.SetTakeChange(true);

                    item["ROW_DATE_DELETE"] = DateTime.Now;

                    item["ROW_SID"] = -3;
                }


            }

            decipher.UpdateModels(mall_ProdSpecs, true);

        }






        public void SyncUT_384Data(DbDecipher decipher)
        {




            //库存数量明细表新增最后同步时间
            DateTime insert_end_sync_date = GlobelParam.GetValue(decipher, "UT_384_INSERT_END_SYNC_DATE", new DateTime(2000, 1, 1), "材料档案颜色表新增最后同步时间");

            //库存数量明细表更新最后同步时间
            DateTime update_end_sync_date = GlobelParam.GetValue(decipher, "UT_384_UPDATE_END_SYNC_DATE", new DateTime(2000, 1, 1), "材料档案颜色表更新最后同步时间");

            //库存数量明细表删除最后同步时间
            DateTime detele_end_sync_date = GlobelParam.GetValue(decipher, "UT_384_DETELE_END_SYNC_DATE", new DateTime(2000, 1, 1), "材料档案颜色表删除最后同步时间");



            string inser_sql = $"select * from UT_384 where ROW_SID >=0 and  ROW_DATE_CREATE > '{insert_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //新增数据集合
            SModelList insert_accounts = decipher.GetSModelList(inser_sql);


            string update_sql = $"select * from UT_384 where ROW_SID >=0 and  ROW_DATE_UPDATE > '{update_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //更新数据集合
            SModelList update_accounts = decipher.GetSModelList(update_sql);


            string delete_sql = $"select * from UT_384 where ROW_SID = -3 and  ROW_DATE_DELETE > '{detele_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //删除数据集合
            SModelList delete_accounts = decipher.GetSModelList(delete_sql);



            SyncUT_384InserDoc(decipher, insert_accounts);

            SyncUT_384UpdateDoc(decipher, update_accounts);

            SyncUT_384DeleteDoc(decipher, delete_accounts);


            GlobelParam.SetValue(decipher, "UT_384_INSERT_END_SYNC_DATE", DateTime.Now, "材料档案颜色表新增最后同步时间");
            GlobelParam.SetValue(decipher, "UT_384_UPDATE_END_SYNC_DATE", DateTime.Now, "材料档案颜色表更新最后同步时间");
            GlobelParam.SetValue(decipher, "UT_384_DETELE_END_SYNC_DATE", DateTime.Now, "材料档案颜色表删除最后同步时间");




        }



        /// <summary>
        /// 同步颜色规格表新增的数据
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="insert_accounts">新增的用户账号集合</param>
        void SyncUT_384InserDoc(DbDecipher decipher, SModelList insert_accounts)
        {

            if (insert_accounts.Count == 0)
            {
                return;
            }

            try
            {
                LModelList<LModel> mall_ProdSpecs = new LModelList<LModel>();

                //插入新的账号进报价系统
                foreach (SModel account in insert_accounts)
                {

                    LightModelFilter filter = new LightModelFilter("UT_083");
                    filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    filter.And("ROW_IDENTITY_ID", account["COL_54"]);

                    LModel lm083 = decipher.GetModel(filter);

                    if (lm083 == null)
                    {
                        continue;
                    }

                    LightModelFilter prod_filter = new LightModelFilter("MALL_PROD");
                    prod_filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    prod_filter.And("PROD_ID", lm083["ROW_IDENTITY_ID"]);

                    LModel lmProd = decipher.GetModel(prod_filter);

                    if (lmProd == null)
                    {
                        continue;
                    }

                    LModel lmProdSpec = new LModel("MALL_PROD_SPEC");

                    lmProdSpec["FK_PROD_CODE"] = lmProd["PK_PROD_CODE"];

                    lmProdSpec["SPEC_ID"] = account["ROW_IDENTITY_ID"];

                    lmProdSpec["SPEC_TYPE"] = "colour";

                    lmProdSpec["ROW_DATE_CREATE"] = lmProdSpec["ROW_DATE_UPDATE"] = DateTime.Now;

                    lmProdSpec["PK_SPEC_CODE"] = BillIdentityMgr.NewCodeForDay("SPEC_CODE", "S");


                    mall_ProdSpecs.Add(lmProdSpec);



                }

                decipher.InsertModels(mall_ProdSpecs);


            }
            catch (Exception ex)
            {
                log.Error($"同步新增数据失败{ex}");
                throw new Exception("同步新增数据失败", ex);
            }





        }


        /// <summary>
        /// 同步颜色规格表更新过后的数据
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="update_accounts">更新过后用户账号集合</param>
        void SyncUT_384UpdateDoc(DbDecipher decipher, SModelList update_accounts)
        {

            if (update_accounts.Count == 0)
            {
                return;
            }

            try
            {
                LModelList<LModel> mall_ProdSpecs = new LModelList<LModel>();

                foreach (SModel lm384 in update_accounts)
                {

                    LightModelFilter filter = new LightModelFilter("MALL_PROD_SPEC");
                    filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    filter.And("SPEC_ID", lm384["ROW_IDENTITY_ID"]);
                    filter.And("SPEC_TYPE", "colour");

                    LModel lmProdSpec = decipher.GetModel(filter);

                    if (lmProdSpec == null)
                    {
                        continue;
                    }

                    lmProdSpec.SetTakeChange(true);


                    lmProdSpec["SPEC_NO"] = lm384["COL_58"] ?? "";
                    lmProdSpec["SPEC_TEXT"] = lm384["COL_59"] ?? "";
                    lmProdSpec["CATALOG_CODE"] = lm384["COL_56"] ?? "";
                    lmProdSpec["CATALOG_TEXT"] = lm384["COL_57"] ?? "";
                    lmProdSpec["ROW_DATE_UPDATE"] = DateTime.Now;


                    mall_ProdSpecs.Add(lmProdSpec);

                }


                decipher.UpdateModels(mall_ProdSpecs, true);

            }
            catch (Exception ex)
            {
                log.Error($"同步更新数据失败{ex}");
                throw new Exception("同步更新数据失败", ex);
            }




        }


        /// <summary>
        /// 同步颜色规格表删除过后的数据
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="delete_accounts"></param>
        void SyncUT_384DeleteDoc(DbDecipher decipher, SModelList delete_accounts)
        {

            if (delete_accounts.Count == 0)
            {
                return;
            }


            LModelList<LModel> mall_ProdSpecs = new LModelList<LModel>();

            foreach (SModel account in delete_accounts)
            {

                LightModelFilter lmFilter = new LightModelFilter("MALL_PROD_SPEC");
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("SPEC_ID", account["ROW_IDENTITY_ID"]);
                lmFilter.And("SPEC_TYPE", "colour");

                mall_ProdSpecs = decipher.GetModelList(lmFilter);

                foreach (var item in mall_ProdSpecs)
                {
                    item.SetTakeChange(true);

                    item["ROW_DATE_DELETE"] = DateTime.Now;

                    item["ROW_SID"] = -3;
                }


            }

            decipher.UpdateModels(mall_ProdSpecs, true);

        }





    }
}