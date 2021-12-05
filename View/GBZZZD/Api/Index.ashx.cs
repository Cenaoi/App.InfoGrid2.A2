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
    /// Index 的摘要说明
    /// </summary>
    public class Index : IHttpHandler, IRequiresSessionState
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
                case "GET_USER_INFO":
                    result = GetUserInfo();
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
                case "PRINT_TASK_ORDER":
                    result = PrintTaskOrder(context);
                    break;
                case "PRINT_TASK_ORDER_FINISH_TAG":
                    result = PrintTaskOrderFinishTag(context);
                    break;
                case "SET_PARCEL_INFO":
                    result = SetParcelInfo(context);
                    break;
                case "GET_PRINT_INFO":
                    result = GetPrintInfo();
                    break;
                case "GET_RECHECK_USER_INFO":
                    result = GetRecheckUserInfo();
                    break;
                case "GET_PRINT_FILE_LIST":
                    result = GetPrintFileList();
                    break;
                case "GET_TASK_ORDER_ITEM_INFO":
                    result = GetTaskOrderItemInfo(context);
                    break;
                case "CONFIRM_RECHECK_PASS":
                    result = ConfirmRecheckPass(context);
                    break;
                case "COPY_PARCEL_INFOS":
                    result = CopyParcelInfo(context);
                    break;
                case "GET_PARCEL_UNIT_LIST":
                    result = GetParcelUnitList();
                    break;
                case "SUBMIT_PRINTER_LIST":
                    result = SubmitPrinterList();
                    break;
                case "GET_PRINT_FILE_STREAM":
                    result = GetPrintFileStream();
                    break;
                case "UPDATE_PRINT_FILE_STATUS":
                    result = UpdatePrintFileStatus();
                    break;
                case "GET_PRINT_FILE_INFO":
                    result = GetPrintFileInfo();
                    break;
                case "PRINT_TASK_ORDER_SMALL_TAG":
                    result = PrintTaskOrderSmallTag(context);
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
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public HttpResult GetUserInfo() 
        {
            string userId = WebUtil.FormTrim("userId");

            if (string.IsNullOrWhiteSpace(userId)) 
            {
                return HttpResult.Error("请传入卡号");
            }

            SModel user = ApiHelper.GetUserInfo(userId);

            if (user == null) 
            {
                return HttpResult.Error("找不到这个用户");
            }

            return HttpResult.Success(user);
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
            filter.And("BIZ_SID", 2);
            filter.And("COL_72", bizSid);

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
            }

            if (bizSid == "101" || bizSid == "102")
            {
                filter.TSqlOrderBy = "COL_77 asc, ROW_IDENTITY_ID asc";
            }
            else
            {
                filter.TSqlOrderBy = "COL_76 desc, ROW_IDENTITY_ID asc";
            }

            if (bizSid != "101") 
            {
                filter.And("COL_88", userId);
            }

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
            filter.And("COL_195", bizSid);
            filter.Limit = Limit.ByPageIndex(limit, page);

            filter.TSqlOrderBy = "COL_89 asc, ROW_IDENTITY_ID asc";

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModelList list = decipher.GetSModelList(filter);

            return HttpResult.Success(list);
        }


        /// <summary>
        /// 获取任务单明细信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResult GetTaskOrderItemInfo(HttpContext context)
        {
            string userId = ApiHelper.GetUserId(context);
            int orderId = WebUtil.FormInt("orderId");
            int itemId = WebUtil.FormInt("itemId");

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

            LModel lm = ApiHelper.GetTaskOrderItemInfo(orderId, itemId);

            if (lm == null)
            {
                return HttpResult.Error("找不到这个任务单明细");
            }

            return HttpResult.Success(lm);
        }


        /// <summary>
        /// 修改包装信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResult SetParcelInfo(HttpContext context)
        {
            string userId = ApiHelper.GetUserId(context);

            int orderId = WebUtil.FormInt("orderId");

            int itemId = WebUtil.FormInt("itemId");

            string strParcelInfo = WebUtil.FormTrim("parcelInfo");

            SModel user = ApiHelper.GetUserInfo(userId);

            if (user == null)
            {
                return HttpResult.Error("找不到这个用户");
            }

            if (orderId == 0)
            {
                return HttpResult.Error("请传入任务ID");
            }

            if (itemId == 0)
            {
                return HttpResult.Error("请传入任务单明细ID");
            }

            if (string.IsNullOrWhiteSpace(strParcelInfo))
            {
                return HttpResult.Error("请传入包装信息");
            }

            SModel parcelInfo = null;

            try
            {
                parcelInfo = SModel.ParseJson(strParcelInfo);
            }
            catch (Exception ex)
            {
                log.Error($"传入的包装信息数据格式错误，json={strParcelInfo}", ex);

                return HttpResult.Error("包装信息数据格式错误");
            }

            LModel order = ApiHelper.GetTaskOrder(userId, orderId);

            if (order == null)
            {
                return HttpResult.Error("找不到这个任务单");
            }

            LModel lm = ApiHelper.GetTaskOrderItemInfo(orderId, itemId);

            if (lm == null)
            {
                return HttpResult.Error("找不到这个任务单明细");
            }

            lm.SetTakeChange(true);
            lm["ROW_DATE_UPDATE"] = DateTime.Now;

            //lm["COL_170"] = parcelInfo.GetString("COL_170");
            //lm["COL_171"] = parcelInfo.GetInt("COL_171");
            //lm["COL_172"] = parcelInfo.GetInt("COL_172");

            lm["COL_167"] = parcelInfo.GetString("COL_167");
            lm["COL_168"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_168"));
            lm["COL_169"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_169"));
            lm["COL_232"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_232"));

            lm["COL_198"] = parcelInfo.GetString("COL_198");
            lm["COL_199"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_199"));
            lm["COL_200"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_200"));
            lm["COL_233"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_233"));

            lm["COL_201"] = parcelInfo.GetString("COL_201");
            lm["COL_204"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_204"));
            lm["COL_207"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_207"));
            lm["COL_234"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_234"));

            lm["COL_202"] = parcelInfo.GetString("COL_202");
            lm["COL_205"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_205"));
            lm["COL_208"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_208"));
            lm["COL_235"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_235"));

            lm["COL_203"] = parcelInfo.GetString("COL_203");
            lm["COL_206"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_206"));
            lm["COL_209"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_209"));
            lm["COL_236"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_236"));

            lm["COL_227"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_227"));
            lm["COL_237"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_237"));

            lm["COL_222"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_222"));
            lm["COL_223"] = ApiHelper.TryGetInt(parcelInfo.GetString("COL_223"));

            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.UpdateModel(lm, true);

            EC5.IG2.BizBase.DbCascadeRule.Update(lm);

            return HttpResult.SuccessMsg("ok");
        }


        /// <summary>
        /// 获取复核人用户信息
        /// </summary>
        /// <returns></returns>
        public HttpResult GetRecheckUserInfo()
        {
            string userId = WebUtil.FormTrim("userId");

            if (string.IsNullOrWhiteSpace(userId))
            {
                return HttpResult.Error("请传入卡号");
            }

            SModel user = ApiHelper.GetRecheckUserInfo(userId);

            if (user == null)
            {
                return HttpResult.Error("找不到这个复核人");
            }

            return HttpResult.Success(user);
        }


        /// <summary>
        /// 确定复核通过
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResult ConfirmRecheckPass(HttpContext context)
        {
            string userId = ApiHelper.GetUserId(context);

            string recheckUserId = WebUtil.FormTrim("recheckUserId");

            int orderId = WebUtil.FormInt("orderId");

            SModel user = ApiHelper.GetUserInfo(userId);

            if (user == null)
            {
                return HttpResult.Error("找不到这个用户");
            }

            if (string.IsNullOrWhiteSpace(recheckUserId))
            {
                return HttpResult.Error("请传入复核人员工卡号");
            }

            if (orderId == 0)
            {
                return HttpResult.Error("请传入任务ID");
            }

            SModel recheckUser = ApiHelper.GetRecheckUserInfo(recheckUserId);

            if (recheckUser == null)
            {
                return HttpResult.Error("找不到这个复核人");
            }

            LModel order = ApiHelper.GetTaskOrder(userId, orderId);

            if (order == null)
            {
                return HttpResult.Error("找不到这个任务单");
            }

            order.SetTakeChange(true);
            order["COL_97"] = DateTime.Now;
            order["COL_89"] = recheckUser.GetString("COL_1");
            order["COL_91"] = recheckUser.GetString("COL_32");
            order["COL_90"] = recheckUser.GetString("COL_2");

            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.UpdateModel(order, true);

            EC5.IG2.BizBase.DbCascadeRule.Update(order);

            return HttpResult.SuccessMsg("ok");
        }


        /// <summary>
        /// 复制包装信息
        /// </summary>
        /// <returns></returns>
        public HttpResult CopyParcelInfo(HttpContext context)
        {
            string userId = ApiHelper.GetUserId(context);

            int orderId = WebUtil.FormInt("orderId");

            int itemId = WebUtil.FormInt("itemId");

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

            LModel lm = ApiHelper.GetTaskOrderItemInfo(orderId, itemId);

            if (lm == null)
            {
                return HttpResult.Error("找不到这个任务单明细");
            }

            lm.SetTakeChange(true);
            lm["COL_196"] = DateTime.Now;

            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.UpdateModel(lm, true);

            EC5.IG2.BizBase.DbCascadeRule.Update(lm);

            return HttpResult.SuccessMsg("ok");
        }

        
        /// <summary>
        /// 获取包装规格单位列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetParcelUnitList()
        {
            int type = WebUtil.FormInt("type");

            LightModelFilter filter = new LightModelFilter("UT_084");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("BIZ_SID", 2);

            if (type == 1)
            {
                filter.And("COL_8", true);
            }
            if (type == 2)
            {
                filter.And("COL_9", true);
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModelList list = decipher.GetSModelList(filter);

            return HttpResult.Success(list);
        }


        #region 打印相关


        /// <summary>
        /// 打印任务单
        /// </summary>
        /// <returns></returns>
        public HttpResult PrintTaskOrder(HttpContext context)
        {
            string userId = ApiHelper.GetUserId(context);

            int orderId = WebUtil.FormInt("orderId");

            string strPrinterInfo = WebUtil.FormTrim("printerInfo");

            SModel user = ApiHelper.GetUserInfo(userId);

            if (user == null)
            {
                return HttpResult.Error("找不到这个用户");
            }

            if (orderId == 0)
            {
                return HttpResult.Error("请传入任务ID");
            }

            if (string.IsNullOrWhiteSpace(strPrinterInfo))
            {
                return HttpResult.Error("请传入打印机信息");
            }

            SModel printerInfo = null;

            try
            {
                printerInfo = SModel.ParseJson(strPrinterInfo);
            }
            catch (Exception ex)
            {
                log.Error("打印机信息数据格式错误. json:" + strPrinterInfo, ex);

                return HttpResult.Error("打印机信息数据格式错误");
            }

            LModel order = ApiHelper.GetTaskOrder(userId, orderId);

            if (order == null)
            {
                return HttpResult.Error("找不到这个任务单");
            }

            SModelList list = ApiHelper.GetTaskOrderItems(orderId);

            //2.生成打印文件
            //SModel orderInfo = ApiHelper.GetTaskOrderV2(userId, orderId);

            SModel orderInfo = new SModel();

            order.CopyTo(orderInfo);

            orderInfo["COL_59"] = $"{orderInfo["COL_59"]:yyyy-MM-dd}";
            orderInfo["COL_4"] = $"{orderInfo["COL_4"]:yyyy-MM-dd}";

            SModelList printFileList = new SModelList();

            int printRecordCount = list.Count;
            int pageIndex = 0;
            int pageCount = 7;

            while (printRecordCount > 0)
            {
                SModelList recordList = new SModelList();

                if (pageCount > printRecordCount) 
                {
                    pageCount = printRecordCount;
                }

                for (int i = 0; i < pageCount; i++)
                {
                    int rindex = (pageIndex * pageCount) + i;

                    SModel record = list[rindex];
                    record["idx"] = rindex + 1;
                    recordList.Add(record);
                }

                int r = ApiHelper.GetRandomNumber(1, 9999);

                string fileName = $"ORDER_{r:0000}_{orderId}.emf";

                SModel printData = orderInfo;
                printData["UT_104_LIST"] = recordList;

                string savePath = "/_Temporary/PrintFile/" + fileName;

                bool success = PrintHelper.Instance.DrawTaskOrderPrintFile(savePath, printData);

                if (!success)
                {
                    return HttpResult.Error("生成打印文件失败");
                }

                SModel printFile = new SModel()
                {
                    ["path"] = savePath,
                    ["name"] = fileName
                };

                printFileList.Add(printFile);

                printRecordCount -= pageCount;

                pageIndex++;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                int findex = 1;

                int r = ApiHelper.GetRandomNumber(1, 9999);

                string batchCode = $"{DateTime.Now:yyyyMMddHHmmss}{r:0000}";

                List<string> fileCodes = new List<string>();

                decipher.BeginTransaction();  

                foreach (var item in printFileList)
                {
                    string fileCode = $"{batchCode}_{findex++:000}";

                    LModel lm = new LModel("UT_475")
                    {
                        ["COL_1"] = fileCode,
                        ["COL_2"] = DateTime.Now,
                        ["COL_3"] = order.Get<string>("COL_27"),
                        ["COL_4"] = orderId,
                        ["COL_5"] = 2,
                        ["COL_6"] = 1,
                        ["COL_7"] = user.GetString("COL_32"),
                        ["COL_8"] = user.GetString("COL_2"),
                        ["COL_9"] = item.GetString("name"),
                        ["COL_10"] = item.GetString("path"),
                        ["COL_11"] = printerInfo.GetString("printerNo"),
                        ["COL_12"] = DateTime.Now,
                        //["COL_13"] = "",
                        //["COL_14"] = "",
                        ["COL_15"] = 0
                    };

                    decipher.InsertModel(lm);

                    fileCodes.Add(fileCode);
                }

                order.SetTakeChange(true);
                order["ROW_DATE_UPDATE"] = DateTime.Now;
                order["COL_72"] = 102;
                order["COL_73"] = "进行中";
                order["COL_88"] = userId;
                order["COL_112"] = DateTime.Now; 

                decipher.UpdateModel(order, true);

                decipher.TransactionCommit();

                EC5.IG2.BizBase.DbCascadeRule.Update(order);

                SModel result = new SModel()
                {
                    ["pid"] = 0,
                    ["print_batch_code"] = batchCode,
                    ["print_count"] = printFileList.Count,
                    ["printer_info"] = printerInfo.ToJson(),
                    ["print_file_codes"] = string.Join(",", fileCodes)
                };

                return HttpResult.Success(result);
            }
            catch (Exception ex)
            {
                log.Error("打印任务单出错", ex);

                decipher.TransactionRollback();

                return HttpResult.Error("打印任务单失败");
            }
        }


        /// <summary>
        /// 打印任务单完成标签
        /// </summary>
        /// <returns></returns>
        public HttpResult PrintTaskOrderFinishTag(HttpContext context)
        {
            string userId = ApiHelper.GetUserId(context);

            int orderId = WebUtil.FormInt("orderId");

            string strPrinterInfo = WebUtil.FormTrim("printerInfo");

            SModel user = ApiHelper.GetUserInfo(userId);

            if (user == null)
            {
                return HttpResult.Error("找不到这个用户");
            }

            if (orderId == 0)
            {
                return HttpResult.Error("请传入任务ID");
            }

            if (string.IsNullOrWhiteSpace(strPrinterInfo))
            {
                return HttpResult.Error("请传入打印机信息");
            }

            SModel printerInfo = null;

            try
            {
                printerInfo = SModel.ParseJson(strPrinterInfo);
            }
            catch (Exception ex)
            {
                log.Error("打印机信息数据格式错误. json:" + strPrinterInfo, ex);

                return HttpResult.Error("打印机信息数据格式错误");
            }

            LModel order = ApiHelper.GetTaskOrder(userId, orderId);

            if (order == null)
            {
                return HttpResult.Error("找不到这个任务单");
            }

            SModelList list = ApiHelper.GetTaskOrderItems(orderId);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModelList printFileList = new SModelList();

            foreach (var item in list)
            {
                int id = item.GetInt("ROW_IDENTITY_ID");

                SModel printData = item;

                int count = ApiHelper.TryGetInt(item.GetString("COL_169")) + ApiHelper.TryGetInt(item.GetString("COL_200")) +
                ApiHelper.TryGetInt(item.GetString("COL_207")) + ApiHelper.TryGetInt(item.GetString("COL_208")) +
                ApiHelper.TryGetInt(item.GetString("COL_209"));

                printData["OrderNo"] = order.Get<string>("COL_27");
                printData["BatchNo"] = $"{DateTime.Now:yyyyMMdd}";

                string col10 = item.GetString("COL_10");
                if (!string.IsNullOrWhiteSpace(col10))
                {
                    string[] col10vals = col10.Split(',');

                    if (col10vals.Length == 2)
                    {
                        printData["OrderNo"] = col10vals[0];
                        printData["COL_89"] = col10vals[1];
                    }
                }

                printData["COL_9"] = ApiHelper.TryGetInt(item.GetString("COL_168")) + ApiHelper.TryGetInt(item.GetString("COL_199")) +
                ApiHelper.TryGetInt(item.GetString("COL_204")) + ApiHelper.TryGetInt(item.GetString("COL_205")) + 
                ApiHelper.TryGetInt(item.GetString("COL_206"));

                string printTemplateName = printerInfo.GetString("template");

                if (printTemplateName == "散件标签(PCS)")
                {
                    printData["COL_4"] = "PCS";
                    printData["COL_9"] = printData.GetInt("COL_9") * 100;
                }
                else
                {
                    printData["COL_4"] = "套";
                }

                for (int i = 0; i < count; i++)
                {
                    int r = ApiHelper.GetRandomNumber(1, 9999);

                    string fileName = $"ORDER_FINISH_{r:0000}_{id}.emf";

                    string savePath = "/_Temporary/PrintFile/" + fileName;

                    bool success = PrintHelper.Instance.DrawTaskFinishTagPrintFile(savePath, printData);

                    if (!success)
                    {
                        return HttpResult.Error("生成打印文件失败");
                    }

                    SModel printFile = new SModel()
                    {
                        ["path"] = savePath,
                        ["name"] = fileName
                    };

                    printFileList.Add(printFile);
                }
            }

            try
            {
                int findex = 1;

                int r = ApiHelper.GetRandomNumber(1, 9999);

                string batchCode = $"{DateTime.Now:yyyyMMddHHmmss}{r:0000}";

                List<string> fileCodes = new List<string>();

                decipher.BeginTransaction();

                foreach (var item in printFileList)
                {
                    string fileCode = $"{batchCode}_{findex++:000}";

                    LModel lm = new LModel("UT_475")
                    {
                        ["COL_1"] = fileCode,
                        ["COL_2"] = DateTime.Now,
                        ["COL_3"] = order.Get<string>("COL_27"),
                        ["COL_4"] = orderId,
                        ["COL_5"] = 2,
                        ["COL_6"] = 1,
                        ["COL_7"] = user.GetString("COL_32"),
                        ["COL_8"] = user.GetString("COL_2"),
                        ["COL_9"] = item.GetString("name"),
                        ["COL_10"] = item.GetString("path"),
                        ["COL_11"] = printerInfo.GetString("printerNo"),
                        ["COL_12"] = DateTime.Now,
                        //["COL_13"] = "",
                        //["COL_14"] = "",
                        ["COL_15"] = 0
                    };

                    decipher.InsertModel(lm);

                    fileCodes.Add(fileCode);
                }

                order.SetTakeChange(true);
                order["ROW_DATE_UPDATE"] = DateTime.Now;
                order["COL_72"] = 103;
                order["COL_73"] = "已完成";

                decipher.UpdateModel(order, true);

                decipher.TransactionCommit();

                EC5.IG2.BizBase.DbCascadeRule.Update(order);

                SModel result = new SModel()
                {
                    ["pid"] = 0,
                    ["print_batch_code"] = batchCode,
                    ["print_count"] = printFileList.Count,
                    ["printer_info"] = printerInfo.ToJson(),
                    ["print_file_codes"] = string.Join(",", fileCodes)
                };

                return HttpResult.Success(result);
            }
            catch (Exception ex)
            {
                log.Error("新增任务完成标签打印文件记录失败", ex);

                decipher.TransactionRollback();

                return HttpResult.Error("新增任务完成标签打印文件记录失败");
            }
        }


        /// <summary>
        /// 提交打印机列表信息
        /// </summary>
        /// <returns></returns>
        public HttpResult SubmitPrinterList()
        {
            string tpGuid = WebUtil.FormTrim("tpGuid");

            string printerNames = WebUtil.FormTrim("printerNames");

            if (string.IsNullOrWhiteSpace(tpGuid))
            {
                return HttpResult.Error("请传入tpGuid");
            }

            if (string.IsNullOrWhiteSpace(printerNames))
            {
                return HttpResult.Error("请传入printerNames");
            }

            string[] printerNameArr = printerNames.Split(',');

            //LightModelFilter filter = new LightModelFilter("");
            //filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            //filter.And("", tpGuid);

            DbDecipher decipher = ModelAction.OpenDecipher();

            //List<LModel> list = decipher.GetModelList(filter);

            List<LModel> list = new List<LModel>();

            List<LModel> res = new List<LModel>();

            int i = 1;

            try
            {
                foreach (var pName in printerNameArr)
                {
                    LModel printer = null;

                    if (list.Count > 0)
                    {
                        printer = list.First(item => item.Get<string>("COL_1") == pName);
                    }

                    if (printer == null)
                    {
                        string pCode = Guid.NewGuid().ToString().Replace("-", "");

                        printer = new LModel("UT_475")
                        {
                            //["ROW_IDENTITY_ID"] = i++,
                            ["ROW_DATE_CREATE"] = DateTime.Now,
                            //["COL_4"] = tpGuid,
                            ["COL_1"] = pName,
                            ["COL_2"] = pCode,
                            ["COL_3"] = "",
                        };

                        //decipher.InsertModel(printer);
                    }

                    res.Add(printer);
                }
            }
            catch (Exception ex)
            {
                log.Error("提交打印机列表信息出错", ex);
            }

            return HttpResult.Success(res);
        }


        /// <summary>
        /// 获取打印文件列表
        /// </summary>
        /// <returns></returns>
        public HttpResult GetPrintFileList()
        {
            string printerNo = WebUtil.FormTrim("printerNo");

            LightModelFilter filter = new LightModelFilter("UT_475");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            //过滤打印状态
            filter.And("COL_5", 2);
            //过滤打印机
            filter.And("COL_11", printerNo);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModelList list = decipher.GetSModelList(filter);

            return HttpResult.Success(list);
        }


        /// <summary>
        /// 获取打印信息
        /// </summary>
        /// <returns></returns>
        public HttpResult GetPrintInfo()
        {
            int fileId = WebUtil.FormInt("fileId");

            if (fileId == 0)
            {
                return HttpResult.Error("请传入打印文件Id");
            }

            LModel printFile = ApiHelper.GetPrintFileInfo(fileId);

            if (printFile == null)
            {
                log.Fatal($"找不到打印记录, fileId:[{fileId}]");

                return HttpResult.Error("找不到这个打印文件记录");
            }

            return HttpResult.Success(printFile);
        }


        /// <summary>
        /// 获取打印文件流
        /// </summary>
        /// <returns></returns>
        public HttpResult GetPrintFileStream()
        {
            int fileId = WebUtil.FormInt("fileId");

            if (fileId == 0)
            {
                return HttpResult.Error("请传入打印文件Id");
            }

            LModel printFile = ApiHelper.GetPrintFileInfo(fileId);

            if (printFile == null)
            {
                log.Fatal($"找不到打印记录, fileId:[{fileId}]");

                return HttpResult.Error("找不到这个打印文件记录");
            }

            return HttpResult.Success(printFile);
        }


        /// <summary>
        /// 更新打印文件状态
        /// </summary>
        /// <returns></returns>
        public HttpResult UpdatePrintFileStatus()
        {
            int fileId = WebUtil.FormInt("fileId");

            int status = WebUtil.FormInt("status");

            if (fileId == 0)
            {
                return HttpResult.Error("请传入打印文件Id");
            }

            if (status == 0)
            {
                return HttpResult.Error("请传入打印状态");
            }

            LModel printFile = ApiHelper.GetPrintFileInfo(fileId);

            if (printFile == null)
            {
                log.Fatal($"找不到打印记录, fileId:[{fileId}]");

                return HttpResult.Error("找不到这个打印文件记录");
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            printFile.SetTakeChange(true);
            printFile["ROW_DATE_UPDATE"] = DateTime.Now;
            printFile["COL_5"] = status;

            decipher.UpdateModel(printFile, true);

            return HttpResult.SuccessMsg("ok");
        }


        /// <summary>
        /// 获取打印文件信息
        /// </summary>
        /// <returns></returns>
        public HttpResult GetPrintFileInfo() 
        {
            string fileCode = WebUtil.FormTrim("fileCode");

            if (string.IsNullOrWhiteSpace(fileCode))
            {
                return HttpResult.Error("请传入打印文件编号");
            }

            LModel printFile = ApiHelper.GetPrintFileInfo(fileCode);

            if (printFile == null)
            {
                log.Fatal($"找不到打印记录, fileCode:[{fileCode}]");

                return HttpResult.Error("找不到这个打印文件记录");
            }

            return HttpResult.Success(printFile);
        }


        /// <summary>
        /// 打印小标签
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResult PrintTaskOrderSmallTag(HttpContext context)
        {
            string userId = ApiHelper.GetUserId(context);

            int orderId = WebUtil.FormInt("orderId");

            string strPrinterInfo = WebUtil.FormTrim("printerInfo");

            int orderItemId = WebUtil.FormInt("orderItemId");

            SModel user = ApiHelper.GetUserInfo(userId);

            if (user == null)
            {
                return HttpResult.Error("找不到这个用户");
            }

            if (orderId == 0)
            {
                return HttpResult.Error("请传入任务ID");
            }

            if (orderItemId == 0)
            {
                return HttpResult.Error("请传入任务明细ID");
            }

            if (string.IsNullOrWhiteSpace(strPrinterInfo))
            {
                return HttpResult.Error("请传入打印机信息");
            }

            SModel printerInfo = null;

            try
            {
                printerInfo = SModel.ParseJson(strPrinterInfo);
            }
            catch (Exception ex)
            {
                log.Error("打印机信息数据格式错误. json:" + strPrinterInfo, ex);

                return HttpResult.Error("打印机信息数据格式错误");
            }

            LModel order = ApiHelper.GetTaskOrder(userId, orderId);

            if (order == null)
            {
                return HttpResult.Error("找不到这个任务单");
            }

            LModel taskItem = ApiHelper.GetTaskOrderItemInfo(orderId, orderItemId);

            SModel printData = new SModel();

            LModelElement lmes = taskItem.GetModelElement();

            foreach (var f in lmes.Fields)
            {
                string field = f.DBField;
                printData[field] = taskItem.Get(field);
            }

            //SModelList list = ApiHelper.GetTaskOrderItems(orderId);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModelList printFileList = new SModelList();

            int id = taskItem.Get<int>("ROW_IDENTITY_ID");

            int count = taskItem.Get<int>("COL_169") + taskItem.Get<int>("COL_200") +
                taskItem.Get<int>("COL_207") + taskItem.Get<int>("COL_208") + taskItem.Get<int>("COL_209");

            printData["OrderNo"] = order.Get<string>("COL_27");
            printData["BatchNo"] = $"{DateTime.Now:yyyyMMdd}";

            string col10 = taskItem.Get<string>("COL_10");
            if (!string.IsNullOrWhiteSpace(col10))
            {
                string[] col10vals = col10.Split(',');

                if (col10vals.Length == 2)
                {
                    printData["OrderNo"] = col10vals[0];
                    printData["COL_89"] = col10vals[1];
                }
            }

            printData["COL_9"] = taskItem.Get<int>("COL_168") + taskItem.Get<int>("COL_199") +
            taskItem.Get<int>("COL_204") + taskItem.Get<int>("COL_205") + taskItem.Get<int>("COL_206");

            string printTemplateName = printerInfo.GetString("template");

            if (printTemplateName == "散件标签(PCS)")
            {
                printData["COL_4"] = "PCS";
                printData["COL_9"] = printData.GetInt("COL_9") * 100;
            }
            else
            {
                printData["COL_4"] = "套";
            }

            for (int i = 0; i < count; i++)
            {
                int r = ApiHelper.GetRandomNumber(1, 9999);

                string fileName = $"ORDER_FINISH_{r:0000}_{id}.emf";

                string savePath = "/_Temporary/PrintFile/" + fileName;

                bool success = PrintHelper.Instance.DrawTaskFinishTagPrintFile(savePath, printData);

                if (!success)
                {
                    return HttpResult.Error("生成打印文件失败");
                }

                SModel printFile = new SModel()
                {
                    ["path"] = savePath,
                    ["name"] = fileName
                };

                printFileList.Add(printFile);
            }

            taskItem.SetTakeChange(true);
            taskItem["COL_238"] = DateTime.Now;

            decipher.UpdateModel(taskItem, true);

            EC5.IG2.BizBase.DbCascadeRule.Update(taskItem);

            try
            {
                int findex = 1;

                int r = ApiHelper.GetRandomNumber(1, 9999);

                string batchCode = $"{DateTime.Now:yyyyMMddHHmmss}{r:0000}";

                List<string> fileCodes = new List<string>();

                decipher.BeginTransaction();

                foreach (var item in printFileList)
                {
                    string fileCode = $"{batchCode}_{findex++:000}";

                    LModel lm = new LModel("UT_475")
                    {
                        ["COL_1"] = fileCode,
                        ["COL_2"] = DateTime.Now,
                        ["COL_3"] = order.Get<string>("COL_27"),
                        ["COL_4"] = orderId,
                        ["COL_5"] = 2,
                        ["COL_6"] = 1,
                        ["COL_7"] = user.GetString("COL_32"),
                        ["COL_8"] = user.GetString("COL_2"),
                        ["COL_9"] = item.GetString("name"),
                        ["COL_10"] = item.GetString("path"),
                        ["COL_11"] = printerInfo.GetString("printerNo"),
                        ["COL_12"] = DateTime.Now,
                        //["COL_13"] = "",
                        //["COL_14"] = "",
                        ["COL_15"] = 0
                    };

                    decipher.InsertModel(lm);

                    fileCodes.Add(fileCode);
                }

                decipher.TransactionCommit();

                SModel result = new SModel()
                {
                    ["pid"] = 0,
                    ["print_batch_code"] = batchCode,
                    ["print_count"] = printFileList.Count,
                    ["printer_info"] = printerInfo.ToJson(),
                    ["print_file_codes"] = string.Join(",", fileCodes)
                };

                return HttpResult.Success(result);
            }
            catch (Exception ex)
            {
                log.Error("新增任务小标签打印文件记录失败", ex);

                decipher.TransactionRollback();

                return HttpResult.Error("新增任务小标签打印文件记录失败");
            }
        }



        #endregion


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}