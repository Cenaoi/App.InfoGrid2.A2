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
    public class SyncSpec:WebTask
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public SyncSpec()
        {
            this.TaskSpan = new TimeSpan(0, 0, 25);

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

                SyncUT_379Data(decipher);

                SyncUT_380Data(decipher);



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


        /// <summary>
        /// 同步尺码规格表数据
        /// </summary>
        /// <param name="decipher"></param>
        public void SyncUT_379Data(DbDecipher decipher)
        {




            //库存数量明细表新增最后同步时间
            DateTime insert_end_sync_date = GlobelParam.GetValue(decipher, "UT_379_INSERT_END_SYNC_DATE", new DateTime(2000, 1, 1), "尺码规格表新增最后同步时间");

            //库存数量明细表更新最后同步时间
            DateTime update_end_sync_date = GlobelParam.GetValue(decipher, "UT_379_UPDATE_END_SYNC_DATE", new DateTime(2000, 1, 1), "尺码规格表更新最后同步时间");

            //库存数量明细表删除最后同步时间
            DateTime detele_end_sync_date = GlobelParam.GetValue(decipher, "UT_379_DETELE_END_SYNC_DATE", new DateTime(2000, 1, 1), "尺码规格表删除最后同步时间");



            string inser_sql = $"select * from UT_379 where ROW_SID >=0 and  ROW_DATE_CREATE > '{insert_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //新增数据集合
            SModelList insert_accounts = decipher.GetSModelList(inser_sql);


            string update_sql = $"select * from UT_379 where ROW_SID >=0 and  ROW_DATE_UPDATE > '{update_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //更新数据集合
            SModelList update_accounts = decipher.GetSModelList(update_sql);


            string delete_sql = $"select * from UT_379 where ROW_SID = -3 and  ROW_DATE_DELETE > '{detele_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //删除数据集合
            SModelList delete_accounts = decipher.GetSModelList(delete_sql);



            SyncInserDoc(decipher, insert_accounts);

            SyncUpdateDoc(decipher, update_accounts);

            SyncDeleteDoc(decipher, delete_accounts);


            GlobelParam.SetValue(decipher, "UT_379_INSERT_END_SYNC_DATE", DateTime.Now, "尺码规格表新增最后同步时间");
            GlobelParam.SetValue(decipher, "UT_379_UPDATE_END_SYNC_DATE", DateTime.Now, "尺码规格表更新最后同步时间");
            GlobelParam.SetValue(decipher, "UT_379_DETELE_END_SYNC_DATE", DateTime.Now, "尺码规格表删除最后同步时间");




        }



        /// <summary>
        /// 同步颜色规格表数据
        /// </summary>
        /// <param name="decipher"></param>
        public void SyncUT_380Data(DbDecipher decipher)
        {




            //库存数量明细表新增最后同步时间
            DateTime insert_end_sync_date = GlobelParam.GetValue(decipher, "UT_380_INSERT_END_SYNC_DATE", new DateTime(2000, 1, 1), "颜色规格表新增最后同步时间");

            //库存数量明细表更新最后同步时间
            DateTime update_end_sync_date = GlobelParam.GetValue(decipher, "UT_380_UPDATE_END_SYNC_DATE", new DateTime(2000, 1, 1), "颜色规格表更新最后同步时间");

            //库存数量明细表删除最后同步时间
            DateTime detele_end_sync_date = GlobelParam.GetValue(decipher, "UT_380_DETELE_END_SYNC_DATE", new DateTime(2000, 1, 1), "颜色规格表删除最后同步时间");



            string inser_sql = $"select * from UT_380 where ROW_SID >=0 and  ROW_DATE_CREATE > '{insert_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //新增数据集合
            SModelList insert_accounts = decipher.GetSModelList(inser_sql);


            string update_sql = $"select * from UT_380 where ROW_SID >=0 and  ROW_DATE_UPDATE > '{update_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //更新数据集合
            SModelList update_accounts = decipher.GetSModelList(update_sql);


            string delete_sql = $"select * from UT_380 where ROW_SID = -3 and  ROW_DATE_DELETE > '{detele_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //删除数据集合
            SModelList delete_accounts = decipher.GetSModelList(delete_sql);



            SyncUT_380InserDoc(decipher, insert_accounts);

            SyncUT_380UpdateDoc(decipher, update_accounts);

            SyncUT_380DeleteDoc(decipher, delete_accounts);


            GlobelParam.SetValue(decipher, "UT_380_INSERT_END_SYNC_DATE", DateTime.Now, "颜色规格表新增最后同步时间");
            GlobelParam.SetValue(decipher, "UT_380_UPDATE_END_SYNC_DATE", DateTime.Now, "颜色规格表更新最后同步时间");
            GlobelParam.SetValue(decipher, "UT_380_DETELE_END_SYNC_DATE", DateTime.Now, "颜色规格表删除最后同步时间");




        }





        /// <summary>
        /// 同步尺码规格表新增的数据
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="insert_accounts">新增的用户账号集合</param>
        void SyncInserDoc(DbDecipher decipher, SModelList insert_accounts)
        {

            try
            {
                LModelList<LModel> mall_specs = new LModelList<LModel>();

                //插入新的账号进报价系统
                foreach (SModel account in insert_accounts)
                {


                    LModel lmSpec = new LModel("MALL_SPEC");

                    lmSpec["SPEC_ID"] = account["ROW_IDENTITY_ID"];
                    lmSpec["SPEC_TYPE"] = "size";
                    lmSpec["ROW_DATE_CREATE"] = lmSpec["ROW_DATE_UPDATE"] = DateTime.Now;
                    lmSpec["PK_SPEC_CODE"] = BillIdentityMgr.NewCodeForDay("SPEC_CODE", "S");

                    CopyDoc(lmSpec, account);

                    mall_specs.Add(lmSpec);



                }

                decipher.InsertModels(mall_specs);


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

            try
            {
                LModelList<LModel> mall_specs = new LModelList<LModel>();

                foreach (SModel account in update_accounts)
                {

                    LightModelFilter filter = new LightModelFilter("MALL_SPEC");
                    filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    filter.And("SPEC_ID", account["ROW_IDENTITY_ID"]);

                    LModel lmSpec = decipher.GetModel(filter);


                    if (lmSpec == null)
                    {
                        continue;
                    }

                    lmSpec.SetTakeChange(true);

                    lmSpec["ROW_DATE_UPDATE"] = DateTime.Now;

                    CopyDoc(lmSpec, account);

                    mall_specs.Add(lmSpec);

                }


                decipher.UpdateModels(mall_specs, true);

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


            LModelList<LModel> mall_specs = new LModelList<LModel>();

            foreach (SModel account in delete_accounts)
            {



                LightModelFilter lmFilter = new LightModelFilter("MALL_SPEC");
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("SPEC_ID", account["ROW_IDENTITY_ID"]);

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
        void CopyDoc(LModel lm, SModel sm)
        {

            lm["SPEC_NO"] = sm["COL_1"];
            lm["SPEC_TEXT"] = sm["COL_4"];
            lm["CATALOG_CODE"] = sm["COL_2"];
            lm["CATALOG_TEXT"] = sm["COL_3"];
            lm["COL_6"] = sm["COL_6"];

        }







        /// <summary>
        /// 同步颜色规格表新增的数据
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="insert_accounts">新增的用户账号集合</param>
        void SyncUT_380InserDoc(DbDecipher decipher, SModelList insert_accounts)
        {

            try
            {
                LModelList<LModel> mall_specs = new LModelList<LModel>();

                //插入新的账号进报价系统
                foreach (SModel account in insert_accounts)
                {


                    LModel lmSpec = new LModel("MALL_SPEC");

                    lmSpec["SPEC_ID"] = account["ROW_IDENTITY_ID"];
                    lmSpec["SPEC_TYPE"] = "colour";
                    lmSpec["ROW_DATE_CREATE"] = lmSpec["ROW_DATE_UPDATE"] = DateTime.Now;
                    lmSpec["PK_SPEC_CODE"] = BillIdentityMgr.NewCodeForDay("SPEC_CODE", "S");

                    CopyUT_380Doc(lmSpec, account);

                    mall_specs.Add(lmSpec);



                }

                decipher.InsertModels(mall_specs);


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
        void SyncUT_380UpdateDoc(DbDecipher decipher, SModelList update_accounts)
        {

            try
            {
                LModelList<LModel> mall_specs = new LModelList<LModel>();

                foreach (SModel account in update_accounts)
                {

                    LightModelFilter filter = new LightModelFilter("MALL_SPEC");
                    filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    filter.And("SPEC_ID", account["ROW_IDENTITY_ID"]);

                    LModel lmSpec = decipher.GetModel(filter);

                    if (lmSpec == null)
                    {
                        continue;
                    }

                    lmSpec.SetTakeChange(true);

                    lmSpec["ROW_DATE_UPDATE"] = DateTime.Now;

                    CopyUT_380Doc(lmSpec, account);

                    mall_specs.Add(lmSpec);

                }


                decipher.UpdateModels(mall_specs, true);

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
        void SyncUT_380DeleteDoc(DbDecipher decipher, SModelList delete_accounts)
        {


            LModelList<LModel> mall_specs = new LModelList<LModel>();

            foreach (SModel account in delete_accounts)
            {



                LightModelFilter lmFilter = new LightModelFilter("MALL_SPEC");
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("SPEC_ID", account["ROW_IDENTITY_ID"]);

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
        /// 拷贝颜色规格表数据
        /// </summary>
        /// <param name="lm"></param>
        /// <param name="sm"></param>
        void CopyUT_380Doc(LModel lm, SModel sm)
        {


            lm["CATALOG_CODE"] = sm["COL_1"];
            lm["CATALOG_TEXT"] = sm["COL_2"];
            lm["SPEC_NO"] = sm["COL_3"];
            lm["SPEC_TEXT"] = sm["COL_4"];


        }




    }
}