using App.BizCommon;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.IG2.Core;
using EC5.IG2.Core.UI;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace App.InfoGrid2.View.Biz.Rosin.Forms
{
    public class WareMsg
    {

        public static bool ChangeBizSID_0_2(LModel model, Store store, DataRecord record, string io_tag)
        {
            //int row_id = WebUtil.QueryInt("row_id");
            //string io_tag = WebUtil.Query("IO_TAG").ToUpper();

            DbDecipher decipher = ModelAction.OpenDecipher();


            bool success = ChangeBizSID(model, store, record, 0, 2);

            if (!success)
            {
                return false;
            }

            int row_id = (int)model.GetPk();

            if (StringUtil.IsBlank(model["BILL_NO"]))
            {
                string newBillNo = BillIdentityMgr.NewCodeForDay("BILL", io_tag, 4);

                model.SetTakeChange(true);

                model["BILL_NO"] = newBillNo;
                model["COL_3"] = DateTime.Now;
                model["COL_1"] = BizServer.LoginName;

                decipher.UpdateModel(model, true);

            }
            else
            {
                string upCode = BillIdentityMgr.NewCodeForDay("UPDATE_BILL", io_tag + "UP-", 4);

                model.SetTakeChange(true);
                model["COL_4"] = upCode;
                model["COL_5"] = BizServer.LoginName;
                model["COL_6"] = DateTime.Now;

                decipher.UpdateModel(model, true);
            }


            if (record != null && store != null)
            {
                string[] cFields = model.GetBlemishFields();

                foreach (string cField in cFields)
                {
                    store.SetRecordValue(record.Id, cField, model[cField]);
                }
            }


            return true;

        }

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
        /// 把所有的库存更新一遍
        /// </summary>
        public static void UpdataStockAll()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter lmFilter001 = new LightModelFilter("UT_001");
            lmFilter001.AddFilter("ROW_SID >= 0");
            lmFilter001.And("BIZ_SID", 2, Logic.GreaterThanOrEqual);
            lmFilter001.And("IO_TAG", "I");

            //所有入库数据
            List<LModel> lmI001s = decipher.GetModelList(lmFilter001);

            LightModelFilter lmFilter002 = new LightModelFilter("UT_002");
            lmFilter002.AddFilter("ROW_SID >= 0");

            //拿出所有UT_002的数据
            List<LModel> lm002s = decipher.GetModelList(lmFilter002);


            //循环所有入库数据
            foreach(LModel lmI001 in lmI001s)
            {

                List<LModel> lmI002s = lm002s.FindAll(l=>l["COL_1"] == lmI001.GetPk() && l["IO_TAG"] == lmI001["IO_TAG"]);

                foreach (var lmI002 in lmI002s)
                {
                    LModel total = GetTotal(lmI002.Get<string>("SRC_PROD_ITEM_CODE"));

                    lmI002["OUT_NUM"] = total["TOTAL_NUM"];   //出库数量
                    lmI002["OUT_NUM"] = total["TOTAL_WEIGHT"];   //出库重量

                    lmI002["SURPLUS_NUM"] = lmI002.Get<decimal>("COL_21") - lmI002.Get<decimal>("OUT_NUM");   //数量

                    lmI002["SURPLUS_WEIGHT"] = lmI002.Get<decimal>("COL_23") - lmI002.Get<decimal>("OUT_WEIGHT"); //重量

                    decipher.UpdateModelProps(lmI002, "SURPLUS_NUM", "SURPLUS_WEIGHT");


                }

            }



        }



        private static LModel GetTotal(string inBillCode)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter("UT_002");
            filter.AddFilter("ROW_SID >= 0");
            filter.And("COL_1", new FilterSQL("select ROW_IDENTITY_ID from UT_001 where ROW_IDENTITY_ID = UT_002.COL_1 and ROW_SID >=0 and BIZ_SID >= 2"));
            filter.And("SRC_PROD_ITEM_CODE", inBillCode);
            filter.Fields = new string[] { "SUM(COL_21) as TOTAL_NUM", "SUM(COL_23) as TOTAL_WEIGHT" };

            LModel model = decipher.GetModel(filter);

            return model;
        }


    }
}