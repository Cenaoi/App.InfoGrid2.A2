//using HWQ.Entity.Caching;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EC5.IG2.Plugin.PluginBll
{
    public class ImportHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 处理导入文件
        /// </summary>
        public static void HandleImport()
        {
            SModelList fileList = BizHelper.GetImportFileList();

            if (fileList.Count == 0)
            {
                return;
            }

            log.Debug($"准备处理导入文件, 数量:{fileList.Count}");

            SModelList ruleAppendInfo = BizHelper.GetRuleAppendInfo();

            foreach (var item in fileList)
            {
                //规则id
                int ruleId = item.GetInt("COL_142");

                //规则信息
                //SModel ruleInfo = GetImportRule(ruleId);

                SModelList ruleInfoList = BizHelper.GetRuleInfoList(ruleId);

                ImportRuleInfo ruleInfo = new ImportRuleInfo(ruleInfoList, ruleAppendInfo);

                ImportDataInfo importDataInfo = ExcelHelper.AnalysisImportFileV2(item, ruleInfo);
                importDataInfo.OrderNo = item.GetString("COL_19");

                SModel orderInfo = importDataInfo.OrderInfo;
                SModelList orderItems = importDataInfo.OrderItems;

                ImportResultInfo importResultInfo = null;
                string progressCode = "103";
                string progressText = "已完成";

                //解析失败
                if (!importDataInfo.Analysed || importDataInfo.HasError)
                {
                    progressCode = "301";
                    progressText = "扫描失败";

                    log.Error("解析文件出错", importDataInfo.Exception);
                }
                else
                {
                    //调整要返回导入情况：导入总数量，成功数量，失败数量，导入老系统返回的单头id
                    importResultInfo = ImportOrderInfoV2(item, importDataInfo, ruleInfo);

                    //301=导入失败, 103=导入成功
                    progressCode = importResultInfo.Success ? "103" : "301";
                    progressText = importResultInfo.Success ? "已完成" : "扫描失败";
                }

                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    int pkId = item.GetInt("ROW_IDENTITY_ID");

                    LModel fileInfo = BizHelper.GetImportFileInfo(decipher, pkId);

                    if (fileInfo == null)
                    {
                        //更新导入文件处理状态
                        SModel upValue = new SModel()
                        {
                            ["ROW_DATE_UPDATE"] = DateTime.Now,
                            ["COL_144"] = progressCode,
                            //状态的中文名
                            ["COL_145"] = progressText,
                            //导入Excel表完成时间
                            ["COL_132"] = DateTime.Now,
                            //明细表导入条数
                            ["COL_149"] = 0,
                            //明细表导入成功条数
                            ["COL_150"] = 0,
                            //明细表导入失败条数
                            ["COL_151"] = 0,
                            //单头的备注内容
                            ["COL_11"] = "",
                            //导入老系统后返回的单头id
                            ["COL_148"] = 0
                        };

                        upValue["COL_149"] = orderItems != null ? orderItems.Count : 0;
                        upValue["COL_150"] = importResultInfo != null ? importResultInfo.FinishCount : 0;
                        upValue["COL_151"] = importResultInfo != null ? importResultInfo.FailureCount : 0;
                        upValue["COL_11"] = orderInfo != null ? orderInfo.GetString("Des") : "";
                        upValue["COL_148"] = importResultInfo != null ? importResultInfo.OrderId : 0;

                        decipher.UpdateSModel(upValue, BizHelper.UploadOrderFileTableName, $" ROW_IDENTITY_ID = {pkId} ");

                        log.Debug($"更新导入文件的处理信息完成.");
                    }
                    else
                    {
                        fileInfo.SetTakeChange(true);
                        fileInfo["ROW_DATE_UPDATE"] = DateTime.Now;
                        fileInfo["COL_144"] = progressCode;
                        //状态的中文名
                        fileInfo["COL_145"] = progressText;
                        //导入Excel表完成时间
                        fileInfo["COL_132"] = DateTime.Now;
                        //明细表导入条数
                        fileInfo["COL_149"] = 0;
                        //明细表导入成功条数
                        fileInfo["COL_150"] = 0;
                        //明细表导入失败条数
                        fileInfo["COL_151"] = 0;
                        //单头的备注内容
                        fileInfo["COL_11"] = "";
                        //导入老系统后返回的单头id
                        fileInfo["COL_148"] = 0;

                        fileInfo["COL_149"] = orderItems != null ? orderItems.Count : 0;
                        fileInfo["COL_150"] = importResultInfo != null ? importResultInfo.FinishCount : 0;
                        fileInfo["COL_151"] = importResultInfo != null ? importResultInfo.FailureCount : 0;
                        fileInfo["COL_11"] = orderInfo != null ? orderInfo.GetString("Des") : "";
                        fileInfo["COL_148"] = importResultInfo != null ? importResultInfo.OrderId : 0;

                        decipher.UpdateModel(fileInfo, true);

                        log.Debug($"更新导入文件的处理信息完成，准备出发联动...");

                        //触发联动
                        EC5.IG2.BizBase.DbCascadeRule.Update(decipher, null, fileInfo);
                    }
                }
            }

            log.Debug($"处理文件导入数据结束， 文件数量：{fileList.Count}");
        }


        /// <summary>
        /// 导入单据数据（准备废弃）
        /// </summary>
        /// <param name="fileInfo">导入文件附加信息</param>
        /// <param name="orderInfo">单据单头信息</param>
        /// <param name="items">单据明细信息</param>
        /// <returns></returns>
        public static bool ImportOrderInfo(SModel fileInfo, SModel importData, SModel ruleInfo)
        {
            try
            {
                //单头
                SModel orderInfo = importData["orderInfo"];

                //单明细
                SModelList orderItems = importData["orderItems"];

                //固铂数据档案的客户ID
                int customerId = fileInfo.GetInt("");

                //匹配查询产品档案信息
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    SModel customerInfo = BizHelper.GetCustomer(decipher, customerId);

                    foreach (var item in orderItems)
                    {
                        //匹配查询值：根据规则设置的单元格位置获取映射字段值（D，3）
                        string queryVal = item.GetString("");

                        //产品映射信息
                        SModel mapGoodsInfo = GetGoodsMapInfo(decipher, customerId, queryVal);

                        //产品档案信息
                        SModel goodsInfo = null;

                        if (mapGoodsInfo != null)
                        {
                            //固铂数据档案的产品ID
                            int goodsId = mapGoodsInfo.GetInt("");

                            //获取产品信息
                            goodsInfo = BizHelper.GetGoods(decipher, goodsId);

                            item["goodsId"] = goodsId;
                        }
                        else
                        {
                            //未知产品
                        }

                        item["goodsInfo"] = goodsInfo;
                    }
                }

                //导入固铂数据库（获取产品最近报价）
                using (DbDecipher decipher = new SqlServer2005Decipher(BizHelper.SDbConn))
                {
                    decipher.InsertSModel(orderInfo);

                    foreach (var item in orderItems)
                    {
                        SModel goodsInfo = item["goodsInfo"];

                        if (goodsInfo != null)
                        {
                            int goodsId = item.GetInt("goodsId");

                            SModel quotedInfo = BizHelper.GetLastQuoted(decipher, customerId, goodsId);

                            //固铂档案数据的产品报价数值
                            //item[""] = quotedInfo.GetDecimal("");
                        }

                        decipher.InsertSModel(item);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("导入数据出错", ex);

                return false;
            }

            return true;
        }


        /// <summary>
        /// 获取产品映射信息（准备废弃）
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="customerId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static SModel GetGoodsMapInfo(DbDecipher decipher, int customerId, string queryVal)
        {
            LightModelFilter filter = new LightModelFilter("");
            filter.And("", customerId);
            filter.And("", queryVal);

            try
            {
                SModel mapInfo = decipher.GetSModel(filter);

                return mapInfo;
            }
            catch (Exception ex)
            {
                log.Error("获取产品映射信息出错", ex);

                return null;
            }
        }


        /// <summary>
        /// 导入单据数据
        /// </summary>
        /// <param name="fileInfo">导入文件信息</param>
        /// <param name="importDataInfo">导入文件数据信息</param>
        /// <param name="ruleInfoList">规则信息</param>
        /// <returns></returns>
        public static ImportResultInfo ImportOrderInfoV2(SModel fileInfo, ImportDataInfo importDataInfo, ImportRuleInfo ruleInfo)
        {
            ImportResultInfo res = new ImportResultInfo()
            {
                TotalCount = importDataInfo.OrderItems.Count
            };

            importDataInfo.OrderNo = fileInfo.GetString("COL_19");

            try
            {
                //导入固铂数据库
                using (DbDecipher decipher = new SqlServer2005Decipher(BizHelper.SDbConn))
                {
                    decipher.Open();

                    importDataInfo.OrderInfo["BillNo"] = importDataInfo.OrderNo;
                    importDataInfo.OrderInfo["CustomerID"] = importDataInfo.CustomerId;
                    importDataInfo.OrderInfo["TabMan"] = fileInfo.GetString("COL_63");

                    //新增订单
                    //decipher.InsertSModel(importDataInfo.OrderInfo, out int orderId);
                    InsertSModel(decipher, "SaleOrder", importDataInfo.OrderInfo, out int orderId);

                    res.OrderId = orderId;

                    int finishCount = 0;

                    foreach (var item in importDataInfo.OrderItems)
                    {
                        item["BillNO"] = importDataInfo.OrderNo;

                        try
                        {
                            //新增订单明细
                            //int count = decipher.InsertSModel(item);
                            InsertSModel(decipher, "SaleOrderList", item, out int orderItemId);

                            finishCount += 1;
                        }
                        catch (Exception ex)
                        {
                            log.Error($"新增订单明细出错，数据库连接：{BizHelper.SDbConn}， 明细数据：{item.ToJson()}", ex);
                        }
                    }

                    res.FinishCount = finishCount;
                    res.FailureCount = res.TotalCount - res.FinishCount;
                }

                res.Success = true;
            }
            catch (Exception ex)
            {
                log.Error("导入数据出错", ex);

                res.Success = false;
                res.Exception = ex;
                res.ErrorMsg = "导入数据出错";
            }

            return res;
        }


        /// <summary>
        /// 插入SModel数据
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="tableName"></param>
        /// <param name="model"></param>
        /// <param name="identity"></param>
        public static int InsertSModel(DbDecipher decipher, string tableName, SModel model, out int identity)
        {
            identity = 0;

            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();
            StringBuilder stringBuilder3 = new StringBuilder();

            string cmdText = "";

            SqlConnection conn = (SqlConnection)decipher.Connection;

            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(cmdText, conn))
                {
                    int num = 0;
                    foreach (KeyValuePair<string, object> keyValuePair in model)
                    {
                        string text = "@" + keyValuePair.Key;
                        object obj = keyValuePair.Value;
                        bool flag2 = DBNull.Value == keyValuePair.Value;

                        if (flag2)
                        {
                            obj = null;
                        }

                        bool flag3 = obj != null;

                        if (flag3)
                        {
                            Type type = keyValuePair.Value.GetType();
                            SqlDbType dbType = ModelConvert.ToSqlDbType(type);
                            SqlParameter sqlParameter = new SqlParameter(text, dbType);
                            sqlParameter.Value = obj;
                            sqlCommand.Parameters.Add(sqlParameter);
                        }
                        else
                        {
                            SqlParameter value = new SqlParameter(text, DBNull.Value);
                            sqlCommand.Parameters.Add(value);
                        }

                        bool flag4 = num++ > 0;

                        if (flag4)
                        {
                            stringBuilder.Append(",");
                            stringBuilder2.Append(",");
                        }

                        stringBuilder.Append(keyValuePair.Key);
                        stringBuilder2.Append(text);
                    }

                    stringBuilder3.AppendFormat("INSERT INTO {0} ", tableName);
                    stringBuilder3.Append("(");
                    stringBuilder3.Append(stringBuilder.ToString());
                    stringBuilder3.Append(") VALUES (");
                    stringBuilder3.Append(stringBuilder2.ToString());
                    stringBuilder3.Append(")");
                    stringBuilder3.Append(";SELECT Scope_identity() AS 'Identity'");

                    sqlCommand.CommandText = stringBuilder3.ToString();

                    object ret = sqlCommand.ExecuteScalar();

                    identity = Convert.ToInt32(ret);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("插入数据出错", ex);
            }

            return 1;
        }


    }


    /// <summary>
    /// 导入数据
    /// </summary>
    public class ImportDataInfo
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 单头信息
        /// </summary>
        public SModel OrderInfo { get; set; }

        /// <summary>
        /// 明细信息
        /// </summary>
        public SModelList OrderItems { get; set; }

        /// <summary>
        /// 是否解析成功
        /// </summary>
        public bool Analysed { get; set; } = true;

        /// <summary>
        /// 异常对象
        /// </summary>
        public Exception Exception { get; set; }

        string m_ErrorMsg = string.Empty;

        /// <summary>
        /// 失败信息
        /// </summary>
        public string ErrorMsg
        {
            get
            {
                return this.m_ErrorMsg;
            }
            set
            {
                this.m_ErrorMsg = value;

                this.HasError = !string.IsNullOrWhiteSpace(this.m_ErrorMsg);
            }
        }

        /// <summary>
        /// 是否有错误
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// 附加表格数据
        /// </summary>
        public Dictionary<string, SModel> AppendTableData { get; set; } = null;

        /// <summary>
        /// 导入文件信息
        /// </summary>
        public SModel ImportFileInfo { get; set; } = null;

        /// <summary>
        /// 获取附加表格数据
        /// </summary>
        public void GetAppendTableData()
        {
            this.AppendTableData = new Dictionary<string, SModel>();

            if (this.ImportFileInfo != null)
            {
                this.AppendTableData.Add("UT_477", this.ImportFileInfo);

                SModel customerInfo = BizHelper.GetCustomer(this.CustomerId);

                this.AppendTableData.Add("Customer", customerInfo);
            }
        }

        public ImportDataInfo(SModel importFileInfo)
        {
            this.ImportFileInfo = importFileInfo;

            this.CustomerId = this.ImportFileInfo.GetInt("COL_137");

            GetAppendTableData();
        }

        /// <summary>
        /// 获取表格数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public SModel GetTableDataByName(string tableName)
        {
            if (!AppendTableData.TryGetValue(tableName, out SModel tableData))
            {
                return null;
            }

            return tableData;
        }

        /// <summary>
        /// 老系统客户id（固铂数据库的客户ID）
        /// </summary>
        public int CustomerId { get; set; }
    }


    /// <summary>
    /// 导入结果
    /// </summary>
    public class ImportResultInfo
    {
        /// <summary>
        /// 是否导入成功
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// 导入数据总数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        public int FinishCount { get; set; }

        /// <summary>
        /// 失败数量
        /// </summary>
        public int FailureCount { get; set; }

        /// <summary>
        /// 异常对象
        /// </summary>
        public Exception Exception { get; set; }

        string m_ErrorMsg = string.Empty;

        /// <summary>
        /// 失败信息
        /// </summary>
        public string ErrorMsg
        {
            get
            {
                return this.m_ErrorMsg;
            }
            set
            {
                this.m_ErrorMsg = value;

                this.HasError = !string.IsNullOrWhiteSpace(this.m_ErrorMsg);
            }
        }

        /// <summary>
        /// 是否有错误
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// 老系统的订单id
        /// </summary>
        public int OrderId { get; set; }
    }


    /// <summary>
    /// 导入规则字段信息
    /// </summary>
    public class ImportRuleInfo
    {
        /// <summary>
        /// 单头字段集合
        /// </summary>
        public List<ImportRuleField> OrderFieldList { get; set; }

        /// <summary>
        /// 明细字段集合
        /// </summary>
        public List<ImportRuleField> OrderItemFieldList { get; set; }

        public ImportRuleInfo(SModelList list, SModelList ruleAppendInfoList)
        {
            if (list.Count == 0)
            {
                return;
            }

            OrderFieldList = new List<ImportRuleField>();
            OrderItemFieldList = new List<ImportRuleField>();

            List<SModel> saleOrderFieldList = list.Where(p => p["COL_5"] == "SaleOrder").ToList();
            List<SModel> saleOrderItemFieldList = list.Where(p => p["COL_5"] == "SaleOrderList").ToList();

            foreach (var item in saleOrderFieldList)
            {
                ImportRuleField field = new ImportRuleField()
                {
                    FieldInfo = item,
                };

                string col29 = item.GetString("COL_29");

                if (!int.TryParse(col29, out int id))
                {
                    continue;
                }

                //int id = item.GetInt("COL_29");

                SModel ruleAppendInfo = ruleAppendInfoList.Where(p => p["COL_1"] == id).FirstOrDefault();

                field.RuleAppendInfo = ruleAppendInfo;

                OrderFieldList.Add(field);
            }

            foreach (var item in saleOrderItemFieldList)
            {
                ImportRuleField field = new ImportRuleField()
                {
                    FieldInfo = item
                };

                string col29 = item.GetString("COL_29");

                if (!int.TryParse(col29, out int id))
                {
                    continue;
                }

                //int id = item.GetInt("COL_29");

                SModel ruleAppendInfo = ruleAppendInfoList.Where(p => p["COL_1"] == id).FirstOrDefault();

                field.RuleAppendInfo = ruleAppendInfo;

                OrderItemFieldList.Add(field);
            }

        }

        /// <summary>
        /// 获取识别字段（列）
        /// </summary>
        /// <returns></returns>
        public List<ImportRuleField> GetOcrFieldList()
        {
            return this.OrderItemFieldList.Where(p => p.IsOcrField == true).ToList();
        }

        /// <summary>
        /// 获取主项字段
        /// </summary>
        /// <returns></returns>
        public ImportRuleField GetOrderItemMajorField()
        {
            return this.OrderItemFieldList.Where(p => p.IsMajorField == true).FirstOrDefault();
        }

        /// <summary>
        /// 获取追加规则的主项数据字段
        /// </summary>
        /// <returns></returns>
        public List<ImportRuleField> GetAppendReadMajorFieldList()
        {
            var res = this.OrderItemFieldList.Where(p => p.IsAppendMajorField == true).ToList();

            return res;
        }

        /// <summary>
        /// 允许最大空行数量
        /// </summary>
        public int MaxEmptyRowCount
        {
            get
            {
                var fieldList = GetAppendReadMajorFieldList();

                int count = 1;

                foreach (var item in fieldList)
                {
                    if (count < item.MaxEmptyRowCount)
                    {
                        count = item.MaxEmptyRowCount;
                    }
                }

                return count;
            }
        }
    }


    /// <summary>
    /// 规则字段
    /// </summary>
    public class ImportRuleField
    {
        /// <summary>
        /// 规则字段信息（UT_484）
        /// </summary>
        public SModel FieldInfo { get; set; }

        /// <summary>
        /// 规则附加信息（UT_483）
        /// </summary>
        public SModel RuleAppendInfo { get; set; }

        /// <summary>
        /// 字段名（UT_484.COL_9）
        /// </summary>
        public string FieldName
        {
            get
            {
                if (this.FieldInfo == null)
                {
                    return "";
                }

                return this.FieldInfo.GetString("COL_9");
            }
        }

        /// <summary>
        /// 数据类型（UT_484.COL_3）
        /// </summary>
        public string DbTypeString
        {
            get
            {
                if (this.FieldInfo == null)
                {
                    return "";
                }

                return this.FieldInfo.GetString("COL_3");
            }
        }

        /// <summary>
        /// 数据长度（UT_484.COL_4）
        /// </summary>
        public int DbDataLength
        {
            get
            {
                if (this.FieldInfo == null)
                {
                    return 0;
                }

                string v = this.FieldInfo.GetString("COL_4");

                if (string.IsNullOrWhiteSpace(v))
                {
                    return 0;
                }

                if (!int.TryParse(v, out int res))
                {
                    return 0;
                }

                return res;
            }
        }

        /// <summary>
        /// 规则附加编号（UT_483.COL_1）
        /// </summary>
        public int RuleFieldId
        {
            get
            {
                if (this.RuleAppendInfo == null)
                {
                    return 0;
                }

                string v = this.RuleAppendInfo.GetString("COL_1");

                if (string.IsNullOrWhiteSpace(v))
                {
                    return 0;
                }

                if (!int.TryParse(v, out int res))
                {
                    return 0;
                }

                return res;

                //return this.RuleAppendInfo.GetInt("COL_1");
            }
        }

        /// <summary>
        /// 是否为附加主项数据字段（UT_483.COL_3）
        /// </summary>
        public bool IsAppendMajorField
        {
            get
            {
                if (this.RuleAppendInfo == null)
                {
                    return false;
                }

                string remark = this.RuleAppendInfo.GetString("COL_3");

                string str = this.RuleAppendInfo.GetString("COL_1");

                if (string.IsNullOrWhiteSpace(str))
                {
                    return false;
                }

                if (!int.TryParse(str, out int raId))
                {
                    return false;
                }

                //int raId = this.RuleAppendInfo.GetInt("COL_1");

                return remark == "主项数据" || raId == 201 || raId == 203;
            }
        }

        /// <summary>
        /// 是否为识别列（UT_484.COL_35）
        /// </summary>
        public bool IsOcrField
        {
            get
            {
                if (this.FieldInfo == null)
                {
                    return false;
                }

                string str = this.FieldInfo.GetString("COL_35");

                if (string.IsNullOrWhiteSpace(str))
                {
                    return false;
                }

                if (!int.TryParse(str, out int res))
                {
                    return false;
                }

                return res == 1;
            }
        }

        /// <summary>
        /// 是否为主项数据字段（UT_484.COL_26）
        /// </summary>
        public bool IsMajorField
        {
            get
            {
                if (this.FieldInfo == null)
                {
                    return false;
                }

                string v = this.FieldInfo.GetString("COL_26");

                if (string.IsNullOrWhiteSpace(v))
                {
                    return false;
                }

                if (!int.TryParse(v, out int res))
                {
                    return false;
                }

                return res == 1;
            }
        }

        /// <summary>
        /// 单元格位置（UT_484.COL_27）
        /// </summary>
        public string CellAddress
        {
            get
            {
                if (this.FieldInfo == null)
                {
                    return "";
                }

                return this.FieldInfo.GetString("COL_27");
            }
        }

        /// <summary>
        /// 允许最大空行数量（UT_484.COL_46）
        /// </summary>
        public int MaxEmptyRowCount
        {
            get
            {
                if (this.FieldInfo == null)
                {
                    return 1;
                }

                string v = this.FieldInfo.GetString("COL_46");

                if (string.IsNullOrWhiteSpace(v))
                {
                    return 1;
                }

                if (!int.TryParse(v, out int res))
                {
                    return 1;
                }

                return res;
                //int count = this.FieldInfo.GetInt("COL_46");

                //return count;
            }
        }

        /// <summary>
        /// 忽略字符值（UT_484.COL_34）
        /// </summary>
        public string NeglectStringValue
        {
            get
            {
                if (this.FieldInfo == null)
                {
                    return "";
                }

                return this.FieldInfo.GetString("COL_34");
            }
        }

        /// <summary>
        /// 是否有忽略字符值
        /// </summary>
        public bool HasNeglectStringValue
        {
            get
            {
                return !string.IsNullOrWhiteSpace(NeglectStringValue);
            }
        }


    }



}
