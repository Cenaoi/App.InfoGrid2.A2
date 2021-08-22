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
    public class SalesTimer : WebTask
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public SalesTimer()
        {
            this.TaskSpan = new TimeSpan(0, 0,5);

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

                SyncUT_090Data(decipher);



            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            finally
            {

                decipher.Dispose();

            }


        }


        public void SyncUT_090Data(DbDecipher decipher)
        {
            DateTime update_end_sync_date = GlobelParam.GetValue(decipher, "UT_091_INSERT_END_SYNC_DATE", new DateTime(2000, 1, 1), "销售订单单头");

            
            
            string inser_sql = $"select * from UT_090 where ROW_SID >=0 and  ROW_DATE_UPDATE > '{update_end_sync_date.ToString("yyyy-MM-dd HH:mm:ss")}'";


            SModelList lists = decipher.GetSModelList(inser_sql);

            SyncInserDoc(decipher, lists);
            GlobelParam.SetValue(decipher, "UT_091_INSERT_END_SYNC_DATE", DateTime.Now, "尺码规格表新增最后同步时间");


        }




        void SyncInserDoc(DbDecipher decipher, SModelList insert_accounts)
        {


           // Int32[] ids = new Int32[] { };
            List<int> ids = new List<int>();

            foreach (SModel model in insert_accounts)
            {
                int dd = model["COL_49"];


                ids.Add(model.Get<int>("COL_49"));
              

            }

            LightModelFilter filter = new LightModelFilter("MALL_ORDER");

            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            filter.And("MALL_ORDER_ID", ids, Logic.In);

           

            LModelList<LModel> models = decipher.GetModelList(filter);

            if (models.Count < 0)
            {
                return;

            }


            LModelList mlist = new LModelList();

            foreach (LModel model in models)
            {
                LightModelFilter filter1 = new LightModelFilter("UT_090");

                filter1.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

               filter1.And("COL_49", model["MALL_ORDER_ID"]);
                
              

                LModel model090 =   decipher.GetModel(filter1);

                model.SetTakeChange(true);



                model["ORDER_SID"] = model090.Get<string>("COL_52");


                model["ORDER_SID_TEXT"] = model090.Get<string>("COL_51");

                mlist.Add(model);
            }

            decipher.UpdateModels(mlist, true);
           
        }






    }
}