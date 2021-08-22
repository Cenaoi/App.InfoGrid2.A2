using App.BizCommon;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.IG2.Core.UI;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.View.Biz.Rosin2.Plan
{
    /// <summary>
    /// 计划管理
    /// </summary>
    public static class PlanMgr
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 改变状态
        /// </summary>
        /// <param name="model"></param>
        /// <param name="record"></param>
        /// <param name="startSID"></param>
        /// <param name="endSID"></param>
        /// <returns>改变状态，</returns>
        public static bool ChangeBizSID(LModel model, Store store, DataRecord record, int startSID, int endSID)
        {

            if (startSID < endSID && endSID > 0)
            {
                M2ValidateFactory validFty = new M2ValidateFactory();

                bool isValid = validFty.ValidModelFieldAll(store, model, null);

                if (!isValid)
                {
                    throw new Exception("请检查填写正确后，再提交.");
                }
            }


            if (model.Get<int>("BIZ_SID") == startSID)
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                model["BIZ_SID"] = endSID;

                decipher.UpdateModelProps(model, "BIZ_SID");

                if (record != null && store != null)
                {
                    store.SetRecordValue(record.Id, "BIZ_SID", endSID);
                }

                return true;
            }

            return false;
        }
    }
}