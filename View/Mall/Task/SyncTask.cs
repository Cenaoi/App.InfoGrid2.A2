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
    public class SyncTask:WebTask
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public SyncTask()
        {
            this.TaskSpan = new TimeSpan(0, 0, 20);

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
            DateTime insert_end_sync_date = GlobelParam.GetValue(decipher, "UT_118_INSERT_END_SYNC_DATE", new DateTime(2000, 1, 1), "库存数量明细表新增最后同步时间");

            //库存数量明细表更新最后同步时间
            DateTime update_end_sync_date = GlobelParam.GetValue(decipher, "UT_118_UPDATE_END_SYNC_DATE", new DateTime(2000, 1, 1), "库存数量明细表更新最后同步时间");

            //库存数量明细表删除最后同步时间
            DateTime detele_end_sync_date = GlobelParam.GetValue(decipher, "UT_118_DETELE_END_SYNC_DATE", new DateTime(2000, 1, 1), "库存数量明细表删除最后同步时间");



            string inser_sql = $"select * from UT_118 where ROW_SID >=0 and  ROW_DATE_CREATE > '{insert_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //新增数据集合
            SModelList insert_accounts = decipher.GetSModelList(inser_sql);


            string update_sql = $"select * from UT_118 where ROW_SID >=0 and  ROW_DATE_UPDATE > '{update_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //更新数据集合
            SModelList update_accounts = decipher.GetSModelList(update_sql);


            string delete_sql = $"select * from UT_118 where ROW_SID = -3 and  ROW_DATE_DELETE > '{detele_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";

            //删除数据集合
            SModelList delete_accounts = decipher.GetSModelList(delete_sql);



            SyncInserDoc(decipher, insert_accounts);

            SyncUpdateDoc(decipher, update_accounts);

            SyncDeleteDoc(decipher, delete_accounts);


            GlobelParam.SetValue(decipher, "UT_118_INSERT_END_SYNC_DATE", DateTime.Now, "库存数量明细表新增最后同步时间");
            GlobelParam.SetValue(decipher, "UT_118_UPDATE_END_SYNC_DATE", DateTime.Now, "库存数量明细表更新最后同步时间");
            GlobelParam.SetValue(decipher, "UT_118_DETELE_END_SYNC_DATE", DateTime.Now, "库存数量明细表删除最后同步时间");




        }





        /// <summary>
        /// 同步新增的数据
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="insert_accounts">新增的用户账号集合</param>
        void SyncInserDoc(DbDecipher decipher, SModelList insert_accounts)
        {

            try
            {
                //插入新的账号进报价系统
                foreach (SModel account in insert_accounts)
                {

                    LModelList<LModel> mall_stocks = new LModelList<LModel>();

                    for (int i = 0; i < 8; i++)
                    {
                        string sz_name = $"SZ_{i}";

                        decimal sz_count = account.Get<decimal>(sz_name);

                        if (sz_count > 0)
                        {


                            LModel mall_stock = new LModel("MALL_STOCK");
                            mall_stock["PK_STOCK_CODE"] = BillIdentityMgr.NewCodeForDay("PK_STOCK_CODE", "STOCK_");
                            mall_stock["ROW_DATE_CREATE"] = mall_stock["ROW_DATE_UPDATE"] = DateTime.Now;
                            mall_stock["ROW_SID"] = 0;
                            mall_stock["STOCK_NUM"] = sz_count;

                            CopyDoc(mall_stock, account);

                            SizeSpecToStock(decipher, mall_stock, sz_name);

                            ColourSpecToStock(decipher, mall_stock, account.Get<string>("COL_30"));

                            mall_stocks.Add(mall_stock);

                        }

                    }

                    decipher.InsertModels(mall_stocks);

                }
            }
            catch (Exception ex)
            {
                log.Error($"同步新增数据失败{ex}");
                throw new Exception("同步新增数据失败", ex);
            }





        }


        /// <summary>
        /// 同步更新过后的数据
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="update_accounts">更新过后用户账号集合</param>
        void SyncUpdateDoc(DbDecipher decipher, SModelList update_accounts)
        {

            try
            {
                foreach (SModel account in update_accounts)
                {
                    LModelList<LModel> mall_stocks = new LModelList<LModel>();

                    for (int i = 0; i < 8; i++)


                    {
                        string sz_name = $"SZ_{i}";

                        decimal sz_count = account.Get<decimal>(sz_name);

                        if (sz_count > 0)
                        {

                            LightModelFilter filter = new LightModelFilter("MALL_STOCK");
                            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                            filter.And("UT_118_ID", account["ROW_IDENTITY_ID"]);
                            filter.And("COL_6", sz_name);

                            LModel mall_stock = decipher.GetModel(filter);

                            if (mall_stock == null)
                            {
                                continue;
                            }

                            mall_stock.SetTakeChange(true);

                            mall_stock["ROW_DATE_UPDATE"] = DateTime.Now;
                            mall_stock["STOCK_NUM"] = sz_count;

                            CopyDoc(mall_stock, account);

                            SizeSpecToStock(decipher, mall_stock, sz_name);

                            ColourSpecToStock(decipher, mall_stock, account.Get<string>("COL_30"));



                            mall_stocks.Add(mall_stock);

                        }

                    }

                    decipher.UpdateModels(mall_stocks, true);


                }
            }
            catch (Exception ex)
            {
                log.Error($"同步更新数据失败{ex}");
                throw new Exception("同步更新数据失败", ex);
            }




        }


        /// <summary>
        /// 同步删除过后的数据
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="delete_accounts"></param>
        void SyncDeleteDoc(DbDecipher decipher, SModelList delete_accounts)
        {

            foreach (SModel account in delete_accounts)
            {

                LightModelFilter lmFilter = new LightModelFilter("MALL_STOCK");
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("UT_118_ID", account["ROW_IDENTITY_ID"]);

                LModelList<LModel> lms = decipher.GetModelList(lmFilter);

                foreach (var lm in lms)
                {
                    lm.SetTakeChange(true);
                    lm["BIZ_SID"] = -3;
                    lm["ROW_DATE_DELETE"] = DateTime.Now;
                }

                decipher.UpdateModels(lms, true);

            }



        }






        /// <summary>
        /// 拷贝
        /// </summary>
        /// <param name="lm"></param>
        /// <param name="sm"></param>
        void CopyDoc(LModel lm, SModel sm)
        {

            lm["UT_118_ID"] = sm["ROW_IDENTITY_ID"];  //库存数量明细表的主键ID

            lm["FK_PROD_CODE"] = sm["COL_15"];    //产品的主键编码
            lm["PROD_NO"] = sm["COL_1"];          //产品编号
            lm["PROD_TEXT"] = sm["COL_2"];        //产品名称
            lm["PROD_TYPE_CODE"] = sm["COL_13"];  //类型编码
            lm["PROD_TEXT_TEXT"] = sm["COL_14"];  //类型名称
            lm["STORE_CODE"] = sm["COL_12"];      //仓库编码
            lm["STORE_TEXT"] = sm["COL_11"];      //仓库名称
            lm["STORE_TEXT"] = sm["COL_11"];      //仓库名称
            //lm["STOCK_NUM"] = sm["COL_9"];        //数量

        }




        /// <summary>
        /// 找尺寸规格信息放到商品库存
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="mall_stock"></param>
        /// <param name="sz_name">字段名(尺寸内置码)</param>
        void SizeSpecToStock(DbDecipher decipher,LModel mall_stock,string sz_name)
        {

            LModel mall_spec = new LModel("MALL_SPEC");

            LightModelFilter filter = new LightModelFilter(typeof(MALL_SPEC));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("SPEC_TYPE", "size");
            filter.And("COL_6", sz_name);

            mall_spec = decipher.GetModel(filter);

            if (mall_spec == null)
            {
                //log.Debug($"获取尺寸规格为空,库存数量明细表ID：{mall_stock.Get<string>("UT_118_ID")},尺寸名称：{sz_name}");
                return;
            }

            mall_stock["SPEC_CODE_2"] = mall_spec["PK_SPEC_CODE"];
            mall_stock["SPEC_NO_2"] = mall_spec["SPEC_NO"];
            mall_stock["SPEC_TEXT_2"] = mall_spec["SPEC_TEXT"];
            mall_stock["COL_6"] = mall_spec["COL_6"];

        }



        /// <summary>
        /// 找颜色规格信息放到商品库存
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="mall_stock"></param>
        /// <param name="colour_code">颜色编码</param>
        void ColourSpecToStock(DbDecipher decipher, LModel mall_stock,string colour_code)
        {

            if (string.IsNullOrWhiteSpace(colour_code))   //颜色为空
            {
                return;
            }

            LModel mall_spec = new LModel("MALL_SPEC");

            LightModelFilter filter = new LightModelFilter(typeof(MALL_SPEC));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("SPEC_TYPE", "colour");
            filter.And("SPEC_NO", colour_code);

            mall_spec = decipher.GetModel(filter);

            if (mall_spec == null)
            {
                //log.Debug($"获取颜色规格为空,库存数量明细表ID：{mall_stock.Get<string>("UT_118_ID")},颜色编码：{colour_code}");
                return;
            }

            mall_stock["SPEC_CODE_1"] = mall_spec["PK_SPEC_CODE"];
            mall_stock["SPEC_NO_1"] = mall_spec["SPEC_NO"];
            mall_stock["SPEC_TEXT_1"] = mall_spec["SPEC_TEXT"];

        }




    }
}