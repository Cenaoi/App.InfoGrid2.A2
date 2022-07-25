using App.BizCommon;
using App.InfoGrid2.GBZZZD.Bll;
using EC5.IO;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.GBZZZD.Api
{
    /// <summary>
    /// QcApi 的摘要说明
    /// </summary>
    /// <summary>
    /// Index 的摘要说明
    /// </summary>
    public class QcApi : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            context.Response.AddHeader("Access-Control-Allow-Credentials", "true");

            context.Response.AddHeader("Access-Control-Allow-Headers", "Origin,Content-Type,Authorization,X-Token");

            string action = WebUtil.FormTrim("action");
            //context.Response.ContentType = "text/plain";
            HttpResult result = null;

            switch (action)
            {
                case "CHECK_API_CONNECTION":
                    result = CheckApi();
                    break;
                case "GET_TASK_ORDER_LIST":
                    result = GetTaskOrderList(context);
                    break;
                case "GET_TASK_ORDER_INFO":
                    result = GetTaskOrderInfo(context);
                    break;
                case "GET_TASK_ORDER_ITEMS":
                    result = GetTaskOrderItems(context);
                    break;
                case "SET_ORDER_ITEM_SCAN_STATE":
                    result = SetOrderItemScanState(context);
                    break;
                case "GET_ORDER_INFO_BY_SCANCODE":
                    result = GetOrderInfoByScanCode(context);
                    break;
                case "SUBMIT_QC":
                    result = SubmitQc(context);
                    break;
                case "GET_BOM_LIST":
                    result = GetBomList(context);
                    break;
                case "SET_ORDER_TIDY":
                    result = SetOrderTidy(context);
                    break;   
                default:
                    result = HttpResult.Error("找不到这个接口");
                    break;
            }

            context.Response.Write(result.ToJson());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HttpResult CheckApi()
        {
            return HttpResult.SuccessMsg("ok");
        }


        /// <summary>
        /// 获取任务单列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResult GetTaskOrderList(HttpContext context)
        {
            string userId = ApiHelper.GetUserId(context);
            string bizSid = WebUtil.FormTrim("bizSid");
            //string orderNo = WebUtil.FormTrim("orderNo");
            int page = WebUtil.FormInt("page");
            int limit = WebUtil.FormInt("limit");

            string searchDataStr = WebUtil.FormTrim("searchData");

            SModel user = ApiHelper.GetUserInfo(userId);

            if (user == null)
            {
                return HttpResult.Error("找不到这个用户");
            }

            SModel searchData = null;

            if (!string.IsNullOrWhiteSpace(searchDataStr))
            {
                searchData = SModel.ParseJson(searchDataStr);
            }

            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("BIZ_SID", 0, HWQ.Entity.Filter.Logic.GreaterThan);

            if (bizSid == "102")
            {
                filter.And("COL_126", new string[] { "101", "102" }, HWQ.Entity.Filter.Logic.In);
            }
            else
            {
                filter.And("COL_126", bizSid);
            }

            //if (!string.IsNullOrWhiteSpace(orderNo)) 
            //{
            //    filter.And("COL_27", $"%{orderNo}%", HWQ.Entity.Filter.Logic.Like);
            //}

            if (searchData != null)
            {
                string finishTimeRangeStr = searchData.GetString("finishTimeRange");
                string customerText = searchData.GetString("customerText");
                string taskOrderNo = searchData.GetString("taskOrderNo");
                string orderNo = searchData.GetString("orderNo");
                string remark = searchData.GetString("remark");

                if (!string.IsNullOrWhiteSpace(finishTimeRangeStr))
                {
                    string[] finishTimeRange = finishTimeRangeStr.Split(',');

                    if (finishTimeRange.Length == 2)
                    {
                        if (DateTime.TryParse(finishTimeRange[0], out DateTime finishTimeS) && DateTime.TryParse(finishTimeRange[1], out DateTime finishTimeE))
                        {
                            DateTime startTime = EC5.Utility.DateUtil.StartDate(finishTimeS);
                            DateTime endTime = EC5.Utility.DateUtil.EndDate(finishTimeE);

                            filter.And("COL_76", startTime, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                            filter.And("COL_76", endTime, HWQ.Entity.Filter.Logic.LessThan);
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(customerText))
                {
                    filter.And("COL_33", $"%{customerText}%", HWQ.Entity.Filter.Logic.Like);
                }

                if (!string.IsNullOrWhiteSpace(orderNo))
                {
                    filter.And("COL_27", $"%{orderNo}%", HWQ.Entity.Filter.Logic.Like);
                }

                if (!string.IsNullOrWhiteSpace(taskOrderNo))
                {
                    filter.And("COL_1", $"%{taskOrderNo}%", HWQ.Entity.Filter.Logic.Like);
                }

                if (!string.IsNullOrWhiteSpace(remark))
                {
                    filter.And("COL_6", $"%{remark}%", HWQ.Entity.Filter.Logic.Like);
                }
            }

            if (bizSid == "101")
            {
                filter.TSqlOrderBy = "COL_77 desc, ROW_IDENTITY_ID asc";
            }
            else if (bizSid == "102")
            {
                filter.TSqlOrderBy = "COL_77 asc, ROW_IDENTITY_ID asc";
            }
            else
            {
                filter.TSqlOrderBy = "COL_76 desc, ROW_IDENTITY_ID asc";
            }

            //if (bizSid != "101")
            //{
            //    filter.And("COL_88", userId);
            //}

            filter.Limit = Limit.ByPageIndex(limit, page);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModelList list = decipher.GetSModelList(filter);

            return HttpResult.Success(list);
        }


        /// <summary>
        /// 获取任务单信息
        /// </summary>
        /// <returns></returns>
        public HttpResult GetTaskOrderInfo(HttpContext context)
        {
            string userId = ApiHelper.GetUserId(context);

            int orderId = WebUtil.FormInt("orderId");

            SModel user = ApiHelper.GetUserInfo(userId);

            if (user == null)
            {
                return HttpResult.Error("找不到这个用户");
            }

            if (orderId == 0)
            {
                return HttpResult.Error("请传入任务ID");
            }

            LModel order = ApiHelper.GetTaskOrder(userId, orderId);

            if (order == null)
            {
                return HttpResult.Error("找不到这个任务单");
            }

            return HttpResult.Success(order);

        }


        /// <summary>
        /// 获取任务单明细列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResult GetTaskOrderItems(HttpContext context)
        {
            string userId = ApiHelper.GetUserId(context);
            int orderId = WebUtil.FormInt("orderId");
            string bizSid = WebUtil.FormTrim("bizSid");
            int page = WebUtil.FormInt("page");
            int limit = WebUtil.FormInt("limit");

            SModel user = ApiHelper.GetUserInfo(userId);

            if (user == null)
            {
                return HttpResult.Error("找不到这个用户");
            }

            if (orderId == 0)
            {
                return HttpResult.Error("请传入任务ID");
            }

            LModel order = ApiHelper.GetTaskOrder(userId, orderId);

            if (order == null)
            {
                return HttpResult.Error("找不到这个任务单");
            }

            LightModelFilter filter = new LightModelFilter("UT_104");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_12", orderId);
            //filter.And("COL_183", userId);
            filter.And("BIZ_SID", 2);
            //filter.And("COL_195", bizSid);
            filter.Limit = Limit.ByPageIndex(limit, page);

            filter.TSqlOrderBy = "COL_243 asc, COL_179 asc, COL_240 asc, ROW_IDENTITY_ID asc";

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModelList list = decipher.GetSModelList(filter);

            return HttpResult.Success(list);
        }


        /// <summary>
        /// 设置任务单明细扫描状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResult SetOrderItemScanState(HttpContext context)
        {
            string userId = ApiHelper.GetUserId(context);

            string scanCode = WebUtil.FormTrim("scanCode");

            SModel user = ApiHelper.GetUserInfo(userId);

            if (user == null)
            {
                return HttpResult.Error("找不到这个用户");
            }

            if (string.IsNullOrWhiteSpace(scanCode))
            {
                return HttpResult.Error("请传入扫描条码");
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter("UT_104");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_245", scanCode);

            LModel orderItem = decipher.GetModel(filter);

            if (orderItem == null)
            {
                return HttpResult.Error("找不到这个任务单明细");
            }

            orderItem.SetTakeChange(true);
            orderItem["COL_246"] = DateTime.Now;
            orderItem["COL_243"] = "103";
            orderItem["COL_244"] = "已完成";
            orderItem["COL_247"] = user.GetString("COL_2");

            decipher.UpdateModel(orderItem, true);

            EC5.IG2.BizBase.DbCascadeRule.Update(orderItem);

            return HttpResult.Success(orderItem);

        }


        /// <summary>
        /// 根据扫描条码获取任务单信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResult GetOrderInfoByScanCode(HttpContext context)
        {
            string userId = ApiHelper.GetUserId(context);

            string scanCode = WebUtil.FormTrim("scanCode");

            SModel user = ApiHelper.GetUserInfo(userId);

            if (user == null)
            {
                return HttpResult.Error("找不到这个用户");
            }

            if (string.IsNullOrWhiteSpace(scanCode))
            {
                return HttpResult.Error("请传入扫描条码");
            }

            int index = scanCode.IndexOf('S');
            scanCode = scanCode.Substring(index);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_131", scanCode);

            LModel order = decipher.GetModel(filter);

            if (order == null)
            {
                return HttpResult.Error("找不到这个任务单明细");
            }

            return HttpResult.Success(order);
        }


        /// <summary>
        /// 完成质检
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResult SubmitQc(HttpContext context)
        {
            string userId = ApiHelper.GetUserId(context);

            int orderId = WebUtil.FormInt("orderId");

            SModel user = ApiHelper.GetUserInfo(userId);

            if (user == null)
            {
                return HttpResult.Error("找不到这个用户");
            }

            if (orderId == 0)
            {
                return HttpResult.Error("请传入任务ID");
            }

            LModel order = ApiHelper.GetTaskOrder(userId, orderId);

            if (order == null)
            {
                return HttpResult.Error("找不到这个任务单");
            }

            order.SetTakeChange(true);
            order["ROW_DATE_UPDATE"] = DateTime.Now;
            order["COL_126"] = 103;
            order["COL_127"] = "已完成";
            order["COL_128"] = user.GetString("COL_2");
            order["COL_130"] = DateTime.Now;

            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.UpdateModel(order, true);

            EC5.IG2.BizBase.DbCascadeRule.Update(order);

            return HttpResult.SuccessMsg("ok");
        }


        /// <summary>
        /// 获取配件列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResult GetBomList(HttpContext context)
        {
            string userId = ApiHelper.GetUserId(context);
            int prodId = WebUtil.FormInt("prodId");

            SModel user = ApiHelper.GetUserInfo(userId);

            if (user == null)
            {
                return HttpResult.Error("找不到这个用户");
            }

            if (prodId == 0)
            {
                return HttpResult.Error("请传入产品ID");
            }

            LightModelFilter filter = new LightModelFilter("UT_191");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_1", prodId);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModelList list = decipher.GetSModelList(filter);

            return HttpResult.Success(list);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResult SetOrderTidy(HttpContext context)
        {
            string userId = ApiHelper.GetUserId(context);

            int orderId = WebUtil.FormInt("orderId");

            string strTidyInfo = WebUtil.FormTrim("tidyInfo");

            SModel user = ApiHelper.GetUserInfo(userId);

            if (user == null)
            {
                return HttpResult.Error("找不到这个用户");
            }

            if (orderId == 0)
            {
                return HttpResult.Error("请传入任务ID");
            }

            if (string.IsNullOrWhiteSpace(strTidyInfo))
            {
                return HttpResult.Error("请传入整单信息");
            }

            SModel tidyInfo = null;

            try
            {
                tidyInfo = SModel.ParseJson(strTidyInfo);
            }
            catch (Exception ex)
            {
                log.Error($"传入的整单信息数据格式错误，json={strTidyInfo}", ex);

                return HttpResult.Error("整单信息数据格式错误");
            }

            LModel order = ApiHelper.GetTaskOrder(userId, orderId);

            if (order == null)
            {
                return HttpResult.Error("找不到这个任务单");
            }

            order.SetTakeChange(true);
            order["COL_146"] = ApiHelper.TryGetInt(tidyInfo.GetString("COL_146"));
            order["COL_147"] = user.GetString("COL_2");
            order["COL_148"] = DateTime.Now;
            order["COL_149"] = ApiHelper.TryGetDecimal(tidyInfo, "COL_149");
            order["COL_192"] = ApiHelper.TryGetDecimal(tidyInfo, "COL_192");
            order["COL_193"] = ApiHelper.TryGetDecimal(tidyInfo, "COL_193");
            order["COL_194"] = ApiHelper.TryGetDecimal(tidyInfo, "COL_194");
            order["COL_195"] = ApiHelper.TryGetDecimal(tidyInfo, "COL_195");
            order["COL_196"] = ApiHelper.TryGetDecimal(tidyInfo, "COL_196");

            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.UpdateModel(order, true);

            EC5.IG2.BizBase.DbCascadeRule.Update(order);

            return HttpResult.SuccessMsg("ok");
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}