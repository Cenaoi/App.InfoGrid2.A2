using App.BizCommon;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.GBZZZD.Bll
{
    /// <summary>
    /// 接口帮助类
    /// </summary>
    public class ApiHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <returns></returns>
        public static string GetUserId(HttpContext context)
        {
            string userId = WebUtil.FormTrim("Token");

            return userId;
        }


        /// <summary>
        /// 获取任务单信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public static LModel GetTaskOrder(string userId, string orderNo)
        {
            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_88", userId);
            filter.And("COL_27", orderNo);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel order = decipher.GetModel(filter);

            return order;
        }


        /// <summary>
        /// 获取任务单信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static LModel GetTaskOrder(string userId, int orderId)
        {
            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            //filter.And("COL_88", userId);
            filter.And("ROW_IDENTITY_ID", orderId);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel order = decipher.GetModel(filter);

            return order;
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static SModel GetUserInfo(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) 
            {
                return null;
            }

            LightModelFilter filter = new LightModelFilter("UT_116");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_32", userId);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModel user = decipher.GetSModel(filter);

            return user;
        }


        /// <summary>
        /// 获取打印信息
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public static LModel GetPrintFileInfo(int fileId)
        {
            LightModelFilter filter = new LightModelFilter("UT_475");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("ROW_IDENTITY_ID", fileId);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = decipher.GetModel(filter);

            return model;
        }


        /// <summary>
        /// 获取任务订单明细信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static LModel GetTaskOrderItemInfo(int orderId, int itemId)
        {
            LightModelFilter filter = new LightModelFilter("UT_104");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("ROW_IDENTITY_ID", itemId);
            filter.And("COL_12", orderId);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = decipher.GetModel(filter);

            return lm;
        }


        /// <summary>
        /// 获取复核人员信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static SModel GetRecheckUserInfo(string userId)
        {
            LightModelFilter filter = new LightModelFilter("UT_116");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_32", userId);
            filter.And("COL_31", true);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModel user = decipher.GetSModel(filter);

            return user;
        }


        /// <summary>
        /// 获取一个随机值
        /// </summary>
        /// <returns></returns>
        public static int GetRandomNumber(int min, int max)
        {
            Random random = new Random();

            int r = random.Next(min, max);

            return r;
        }


        /// <summary>
        /// 获取任务单明细列表
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static SModelList GetTaskOrderItems(int orderId) 
        {
            LightModelFilter filter = new LightModelFilter("UT_104");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_12", orderId);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModelList list = decipher.GetSModelList(filter);

            return list;
        }


        /// <summary>
        /// 获取任务单信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static SModel GetTaskOrderV2(string userId, int orderId)
        {
            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            //filter.And("COL_88", userId);
            filter.And("ROW_IDENTITY_ID", orderId);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModel order = decipher.GetSModel(filter);

            return order;
        }


        /// <summary>
        /// 获取打印文件信息
        /// </summary>
        /// <param name="fileCode"></param>
        /// <returns></returns>
        public static LModel GetPrintFileInfo(string fileCode)
        {
            LightModelFilter filter = new LightModelFilter("UT_475");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_1", fileCode);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = decipher.GetModel(filter);

            return model;
        }


        /// <summary>
        /// 尝试获取值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int TryGetInt(string str)
        {
            int v = 0;

            if (!int.TryParse(str, out v)) 
            {
                return 0;
            }

            return v;
        }


        /// <summary>
        /// 获取任务订单明细信息
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static LModel GetTaskOrderItemInfo(int itemId)
        {
            LightModelFilter filter = new LightModelFilter("UT_104");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("ROW_IDENTITY_ID", itemId);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = decipher.GetModel(filter);

            return lm;
        }


    }
}