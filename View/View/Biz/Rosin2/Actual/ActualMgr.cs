using App.BizCommon;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.IG2.Core.UI;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.View.Biz.Rosin2.Actual
{
    public static class ActualMgr
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


        /// <summary>
        /// 填充货物编码
        /// </summary>
        /// <param name="ut_016_id">UT_016表ID</param>
        /// <param name="goods_name">货物名称</param>
        /// <param name="goods_code">货物编码</param>
        public static void FillGoodsCode()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();


            #region  更新UT_016的货物信息

            LightModelFilter lmFilter016 = new LightModelFilter("UT_016");
            lmFilter016.TSqlWhere = "(S_PROD_TEXT = '' or S_PROD_CODE = '')";
            lmFilter016.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter016.And("BIZ_SID", 2, Logic.GreaterThanOrEqual);


            List<LModel> lm016s = decipher.GetModelList(lmFilter016);


            foreach(LModel lm016 in lm016s)
            {

                LightModelFilter lmFilter = new LightModelFilter("UT_017_PROD");
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("DOC_PARENT_ID", lm016["ROW_IDENTITY_ID"]);

                LModel lm017 = decipher.GetModel(lmFilter);

                if(lm017 == null)
                {
                    continue;
                }

                lm016["S_PROD_TEXT"] = lm017["S_PROD_TEXT"];

                lm016["S_PROD_CODE"] = lm017["S_PROD_CODE"];

                lm016["ROW_DATE_UPDATE"] = DateTime.Now;


                decipher.UpdateModelProps(lm016, "S_PROD_TEXT", "S_PROD_CODE", "ROW_DATE_UPDATE");



            }

            #endregion


            #region 更新UT_015表里面的货物信息

            LightModelFilter lmFilter015 = new LightModelFilter("UT_015_PLAN");
            lmFilter015.TSqlWhere = "(S_PROD_TEXT = '' or S_PROD_CODE = '')";
            lmFilter015.And("BIZ_SID", 2, Logic.GreaterThanOrEqual);
            lmFilter015.And("ROW_SID", 0, Logic.GreaterThanOrEqual);


            List<LModel> lm015s = decipher.GetModelList(lmFilter015);

            foreach(LModel lm015 in lm015s)
            {

                LightModelFilter lmFilter020 = new LightModelFilter("UT_020");
                lmFilter020.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter020.And("DOC_PARENT_ID", lm015["ROW_IDENTITY_ID"]);

                LModel lm020 = decipher.GetModel(lmFilter020);

                if (lm020 == null)
                {
                    continue;
                }

                lm015["S_PROD_TEXT"] = lm020["S_PROD_TEXT"];

                lm015["S_PROD_CODE"] = lm020["S_PROD_CODE"];

                lm015["ROW_DATE_UPDATE"] = DateTime.Now;


                decipher.UpdateModelProps(lm015, "S_PROD_TEXT", "S_PROD_CODE", "ROW_DATE_UPDATE");

            }


            #endregion



        }

    }
}