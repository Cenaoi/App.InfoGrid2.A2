using App.BizCommon;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.View.Biz.Rosin.Plan
{

    public class PlanMgr
    {
        
        public static bool ChangeBizSID_0_2(LModel model, Store store, DataRecord record, string io_tag)
        {
            //int row_id = WebUtil.QueryInt("row_id");
            //string io_tag = WebUtil.Query("IO_TAG").ToUpper();

            DbDecipher decipher = ModelAction.OpenDecipher();
            

            bool success = ChangeBizSID(model,store, record, 0, 2);

            if (!success)
            {
                return false;
            }

            int row_id = (int)model.GetPk();

            if (StringUtil.IsBlank(model["BILL_NO"]))
            {
                string newBillNo = BillIdentityMgr.NewCodeForDay("BILL", io_tag, 4);

                decipher.UpdateModelByPk("UT_008", row_id, new object[] {
                    "BILL_NO", newBillNo,
                    "COL_3", DateTime.Now,
                    "COL_1", BizServer.LoginName
                });

                if(record != null && store != null)
                {
                    store.SetRecordValue(record.Id, "BILL_NO", newBillNo);
                    store.SetRecordValue(record.Id, "COL_3", DateTime.Now);
                    store.SetRecordValue(record.Id, "COL_1", BizServer.LoginName);

                }
            }
            else
            {
                string upCode = BillIdentityMgr.NewCodeForDay("UPDATE_BILL", io_tag + "UP-", 4);

                decipher.UpdateModelByPk("UT_008", row_id, new object[] {
                    "COL_4",upCode,
                    "COL_5", BizServer.LoginName,
                    "COL_6", DateTime.Now
                });

                if(record != null && store != null)
                {
                    store.SetRecordValue(record.Id, "COL_4", upCode);
                    store.SetRecordValue(record.Id, "COL_5", BizServer.LoginName);
                    store.SetRecordValue(record.Id, "COL_6", DateTime.Now);
                }
            }

            return true;

        }

        /// <summary>
        /// 改变状态
        /// </summary>
        /// <param name="model"></param>
        /// <param name="recrod"></param>
        /// <param name="startSID"></param>
        /// <param name="endSID"></param>
        /// <returns>改变状态，</returns>
        private static bool ChangeBizSID(LModel model,Store store, DataRecord record, int startSID, int endSID)
        {
            if (model.Get<int>("BIZ_SID") == startSID)
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                model["BIZ_SID"] = endSID;

                decipher.UpdateModelProps(model,"BIZ_SID");

                if(record != null && store != null)
                {
                    store.SetRecordValue(record.Id, "BIZ_SID", endSID);
                }

                //this.bizSID_ci.Value = endSID.ToString();
                //bizSID_cb.Value = endSID.ToString();

                //if (startSID < endSID)
                //{
                //    AutoAdd_UT001(model);
                //}

                return true;
            }

            return false;
        }

    }

}