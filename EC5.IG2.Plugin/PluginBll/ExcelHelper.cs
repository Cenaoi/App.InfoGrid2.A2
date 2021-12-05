using HWQ.Entity.LightModels;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EC5.IG2.Plugin.PluginBll
{
    public class ExcelHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 解析导入文件（准备废弃）
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static SModel AnalysisImportFile(SModel fileInfo, SModel ruleInfo, Dictionary<string, string> mapList, Dictionary<string, string> orderItemMapList)
        {
            #region 自定义规则 1

            ruleInfo["电梯型号"] = "C16";

            ruleInfo["明细开始行"] = 57;
            ruleInfo["序号列号"] = "A";
            ruleInfo["编码列号"] = "B";
            ruleInfo["名称列号"] = "C";
            ruleInfo["规格列号"] = "D:E";
            ruleInfo["图号列号"] = "F:G";
            ruleInfo["数量列号"] = "H";
            ruleInfo["单位列号"] = "I";
            ruleInfo["密箱列号"] = "J";
            ruleInfo["备注列号"] = "K";

            #endregion

            SModel res = new SModel()
            {
                ["orderInfo"] = new SModel(),
                ["orderItems"] = new SModelList()
            };

            try
            {
                //导入文件路径
                string filePath = fileInfo.GetString("文件路径");
                string fileName = fileInfo.GetString("文件名称");

                ISheet sheet = null;

                try
                {
                    FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                    IWorkbook workbook = null;

                    //byte[] fileData = System.IO.File.ReadAllBytes(filePath);
                    if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                        workbook = new XSSFWorkbook(fs);
                    else if (fileName.IndexOf(".xls") > 0) // 2003版本
                        workbook = new HSSFWorkbook(fs);

                    sheet = workbook.GetSheetAt(0);
                }
                catch (Exception ex)
                {
                    throw new Exception("读取文件出错", ex);
                }

                if (sheet == null)
                {
                    throw new Exception("找不到工作簿");
                }

                #region 读取单据单头

                SModel orderInfo = new SModel();

                foreach (var item in mapList)
                {
                    orderInfo[item.Key] = GetCellValue(sheet, ruleInfo.GetString(item.Value));
                }

                res["orderInfo"] = orderInfo;

                #endregion 

                #region 读取单据明细

                SModelList detailList = new SModelList();

                int emptyRowCount = 0;

                int startRowIndex = ruleInfo.GetInt("明细开始行");

                while (true)
                {
                    //如果超过两个空行
                    if (emptyRowCount >= 2)
                    {
                        break;
                    }

                    IRow row = sheet.GetRow(startRowIndex++);

                    //如果是空行
                    if (row == null)
                    {
                        emptyRowCount++;

                        continue;
                    }

                    string firstCellValue = Convert.ToString(row.GetCell(0));

                    //如果第一个单元格值是空
                    if (string.IsNullOrWhiteSpace(firstCellValue))
                    {
                        emptyRowCount++;

                        continue;
                    }

                    SModel detail = new SModel();

                    foreach (var item in orderItemMapList)
                    {
                        string cellValue = "";

                        string cellAddress = ruleInfo.GetString(item.Value);

                        if (!string.IsNullOrWhiteSpace(cellAddress))
                        {
                            string[] addressRange = cellAddress.Split(':');

                            int cellIndex = CellAddress.ToIndex(addressRange[0]);

                            var cell = row.GetCell(cellIndex);

                            if (cell == null && addressRange.Length >= 2)
                            {
                                cellIndex = CellAddress.ToIndex(addressRange[1]);

                                cell = row.GetCell(cellIndex);
                            }

                            if (cell != null)
                            {
                                cellValue = Convert.ToString(cell);
                            }
                        }

                        detail[item.Key] = cellValue;
                    }

                    detailList.Add(detail);
                }

                res["orderItems"] = detailList;

                #endregion
            }
            catch (Exception ex)
            {
                log.Error("解析导入文件出错", ex);

                return null;
            }

            return res;
        }


        /// <summary>
        /// 获取单元格值（准备废弃）
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="startCellIndex"></param>
        /// <param name="endRowIndex"></param>
        /// <param name="endCellIndex"></param>
        /// <returns></returns>
        public static string GetCellValue(ISheet sheet, int startRowIndex, int startCellIndex, int endRowIndex, int endCellIndex)
        {
            string res = "";

            for (int i = 0; i < sheet.NumMergedRegions; i++)
            {
                var region = sheet.GetMergedRegion(i);

                res += region.FormatAsString();



                //if(region.FirstRow==startRowIndex && region.FirstColumn == startCellIndex && 
                //   region.LastRow == endRowIndex && region.LastColumn == endCellIndex) 
                //{
                //    res = region.FormatAsString();
                //}
            }

            return res;
        }


        /// <summary>
        /// 获取单元格值
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static object GetCellValue(ISheet sheet, string address)
        {
            CellAddress cellAddress = new CellAddress(address);

            if (cellAddress.HasMoreCell)
            {
                return GetMoreCellValue(sheet, cellAddress);
            }

            return GetCellValue(sheet, cellAddress);
        }


        /// <summary>
        /// 获取单元格值
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="cellAddress"></param>
        /// <returns></returns>
        public static object GetCellValue(ISheet sheet, CellAddress cellAddress)
        {
            if (!cellAddress.IsParsed)
            {
                return "";
            }

            ICell cell = null;

            if (cellAddress.IsMerged)
            {
                var row = sheet.GetRow(cellAddress.StartRow);

                if (row == null)
                {
                    return "";
                }

                cell = row.GetCell(cellAddress.StartCell);

                if (cell == null)
                {
                    return "";
                }
            }
            else
            {
                var row = sheet.GetRow(cellAddress.StartRow);

                if (row == null)
                {
                    return "";
                }

                cell = row.GetCell(cellAddress.StartCell);

                if (cell == null)
                {
                    return "";
                }
            }

            return cell;
        }


        /// <summary>
        /// 获取多个单元格值合并（非合并单元格）
        /// </summary>
        /// <param name="cellAddress"></param>
        /// <returns></returns>
        public static string GetMoreCellValue(ISheet sheet, CellAddress cellAddress)
        {
            //string addressText = cellAddress.AddressText;

            //if (!addressText.Contains("/"))
            //{
            //    addressText = addressText.Replace("-", "");
            //}

            string res = cellAddress.CellValueTemplate;

            foreach (var item in cellAddress.MoreCellAddressList)
            {
                object cellValue = GetCellValue(sheet, item);

                string cellValueStr = Convert.ToString(cellValue);

                res = res.Replace($"$.{item.AddressText}", cellValueStr);

                //addressText = addressText.Replace(item.AddressText, cellValueStr);
            }

            return res;
        }


        /// <summary>
        /// 解析导入文件
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static ImportDataInfo AnalysisImportFileV2(SModel fileInfo, ImportRuleInfo ruleInfo)
        {
            ImportDataInfo importDataInfo = new ImportDataInfo(fileInfo);

            try
            {
                //导入文件路径
                string filePathInfoStr = fileInfo.GetString("COL_127");

                //filePathInfoStr = @"/UserFile/测试用的单据-21H003320原订单.xls||/UserFile/测试用的单据-21H003320原订单.xls||测试用的单据-21H003320原订单.xls";

                //filePathInfoStr = @"/UserFile/1107116原订单(1)(1).xls||/UserFile/1107116原订单(1)(1).xls||1107116原订单(1)(1).xls";

                if (string.IsNullOrWhiteSpace(filePathInfoStr))
                {
                    importDataInfo.ErrorMsg = "文件路径不能为空";

                    return importDataInfo;
                }

                //filePathInfoStr = filePathInfoStr.Replace("/UserFile", "F:/固铂五金/UserFile");

                string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;

                filePathInfoStr = filePathInfoStr.Replace("/UserFile", $"{baseDir}/UserFile");

                log.Debug($"导入文件路径信息：{filePathInfoStr}");

                string[] filePathInfo = filePathInfoStr.Split(new char[2] { '|', '|' });

                List<string> filePathInfoList = new List<string>();

                foreach (var item in filePathInfo)
                {
                    if (string.IsNullOrWhiteSpace(item))
                    {
                        continue;
                    }

                    filePathInfoList.Add(item);
                }

                filePathInfo = filePathInfoList.ToArray();

                if (filePathInfo.Length < 3)
                {
                    importDataInfo.ErrorMsg = "文件路径信息格式不对";

                    return importDataInfo;
                }

                string filePath = filePathInfo[0];

                string fileName = filePathInfo[2];

                ISheet sheet = null;

                try
                {
                    FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                    IWorkbook workbook = null;

                    //byte[] fileData = System.IO.File.ReadAllBytes(filePath);
                    if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                        workbook = new XSSFWorkbook(fs);
                    else if (fileName.IndexOf(".xls") > 0) // 2003版本
                        workbook = new HSSFWorkbook(fs);

                    sheet = workbook.GetSheetAt(0);
                }
                catch (Exception ex)
                {
                    importDataInfo.Exception = ex;
                    importDataInfo.ErrorMsg = "读取文件出错";
                    return importDataInfo;
                }

                if (sheet == null)
                {
                    importDataInfo.ErrorMsg = "找不到工作簿";

                    return importDataInfo;
                }

                #region 读取单据单头

                SModel orderInfo = new SModel(BizHelper.SaleOrderTableName);

                foreach (var item in ruleInfo.OrderFieldList)
                {
                    orderInfo[item.FieldName] = GetFieldValue(sheet, importDataInfo, item);

                    //orderInfo[item.FieldName] = GetCellValue(sheet, ruleInfo.GetString(item.Value));
                }

                importDataInfo.OrderInfo = orderInfo;

                #endregion 

                #region 读取单据明细

                SModelList detailList = new SModelList();

                int emptyRowCount = 0;

                //ruleInfo.GetInt("明细开始行")

                var majorField = ruleInfo.GetOrderItemMajorField();

                CellAddress startCell = new CellAddress(majorField.CellAddress);

                int rowIndex = startCell.StartRow;

                int detailCount = 1;

                while (true)
                {
                    //如果超过两个空行
                    if (emptyRowCount >= ruleInfo.MaxEmptyRowCount)
                    {
                        break;
                    }

                    IRow row = sheet.GetRow(rowIndex++);

                    //如果是空行
                    if (row == null)
                    {
                        emptyRowCount++;

                        continue;
                    }

                    //
                    //string firstCellValue = Convert.ToString(row.GetCell(startCell.StartCell));

                    ////如果第一个单元格值是空
                    //if (string.IsNullOrWhiteSpace(firstCellValue))
                    //{
                    //    emptyRowCount++;

                    //    continue;
                    //}

                    var readMajorFieldList = ruleInfo.GetAppendReadMajorFieldList();

                    string readMajorFieldValue = "";

                    foreach (var item in readMajorFieldList)
                    {
                        object fieldValue = GetItemFieldValue(sheet, rowIndex, importDataInfo, item);

                        string fieldValueStr = Convert.ToString(fieldValue);

                        //if (string.IsNullOrWhiteSpace(fieldValueStr))
                        //{
                        //    emptyRowCount++;

                        //    continue;
                        //}

                        readMajorFieldValue += fieldValueStr;
                    }

                    if (readMajorFieldList.Count > 0 && string.IsNullOrWhiteSpace(readMajorFieldValue))
                    {
                        emptyRowCount++;

                        continue;
                    }

                    //如果以上条件判断当前行不属于空行，就重置当前空行数量为0（避免累计空行数量出现误判）
                    emptyRowCount = 0;

                    SModel detail = new SModel(BizHelper.SaleOrderListTableName);

                    var ocrFieldList = ruleInfo.GetOcrFieldList();

                    string ocrContent = GetOcrFieldValue(sheet, rowIndex, ocrFieldList);

                    //客户产品名称对比信息
                    SModel goodsMapInfo = BizHelper.GetGoodsMapInfo(importDataInfo.CustomerId, ocrContent);

                    #region 获取老系统产品信息

                    if (goodsMapInfo != null)
                    {
                        if (importDataInfo.AppendTableData.ContainsKey(BizHelper.CustomerGoodsMapTableName))
                        {
                            importDataInfo.AppendTableData[BizHelper.CustomerGoodsMapTableName] = goodsMapInfo;
                        }
                        else
                        {
                            importDataInfo.AppendTableData.Add(BizHelper.CustomerGoodsMapTableName, goodsMapInfo);
                        }

                        int goodsId = goodsMapInfo.GetInt("COL_146");

                        //老系统产品信息
                        SModel goodsInfo = BizHelper.GetGoods(goodsId);

                        if (importDataInfo.AppendTableData.ContainsKey(BizHelper.GoodsTableName))
                        {
                            importDataInfo.AppendTableData[BizHelper.GoodsTableName] = goodsInfo;
                        }
                        else
                        {
                            importDataInfo.AppendTableData.Add(BizHelper.GoodsTableName, goodsInfo);
                        }
                    }
                    else
                    {
                        SModel emptyGoodsInfo = BizHelper.GetEmptyGoodsInfo();

                        SModel emptyGoodsMapInfo = BizHelper.GetEmptyGoodsMapInfo();

                        if (importDataInfo.AppendTableData.ContainsKey(BizHelper.CustomerGoodsMapTableName))
                        {
                            importDataInfo.AppendTableData[BizHelper.CustomerGoodsMapTableName] = emptyGoodsMapInfo;
                        }
                        else
                        {
                            importDataInfo.AppendTableData.Add(BizHelper.CustomerGoodsMapTableName, emptyGoodsMapInfo);
                        }

                        //if (importDataInfo.AppendTableData.ContainsKey("Goods"))
                        //{
                        //    importDataInfo.AppendTableData["Goods"] = emptyGoodsInfo;
                        //}
                        //else
                        //{
                        //    importDataInfo.AppendTableData.Add("Goods", emptyGoodsInfo);
                        //}
                    }

                    #endregion

                    string serialField = "";

                    foreach (var item in ruleInfo.OrderItemFieldList)
                    {
                        //如果字段是自动填入序列号
                        if (item.RuleFieldId == 207)
                        {
                            serialField = item.FieldName;
                            continue;
                        }

                        detail[item.FieldName] = GetItemFieldValue(sheet, rowIndex, importDataInfo, item);
                    }

                    if (!string.IsNullOrWhiteSpace(serialField))
                    {
                        detail[serialField] = detailCount;

                        detailCount++;
                    }

                    detailList.Add(detail);
                }

                importDataInfo.OrderItems = detailList;

                #endregion
            }
            catch (Exception ex)
            {
                importDataInfo.Exception = ex;
                importDataInfo.ErrorMsg = "解析导入文件出错";
                importDataInfo.Analysed = false;
            }

            return importDataInfo;
        }


        /// <summary>
        /// 获取导入规则字段值
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="ruleField"></param>
        /// <returns></returns>
        public static object GetFieldValue(ISheet sheet, ImportDataInfo importDataInfo, ImportRuleField ruleField)
        {
            object res = GetDefaultValue(ruleField);

            if (ruleField.RuleFieldId == 601)
            {
                res = DateTime.Now;
            }
            else if (ruleField.RuleFieldId == 602)
            {
                res = ruleField.FieldInfo["COL_10"];
            }
            else if (ruleField.RuleFieldId == 501)
            {
                //到对应的数据表获取字段值
                string tableName = ruleField.FieldInfo.GetString("COL_16");

                SModel tableData = importDataInfo.GetTableDataByName(tableName);

                if (tableData != null)
                {
                    string fieldName = ruleField.FieldInfo.GetString("COL_17");

                    if (tableData.HasField(fieldName))
                    {
                        res = tableData[fieldName];
                    }
                    else
                    {
                        res = GetDefaultValue(ruleField);
                    }
                }
                else
                {
                    res = GetDefaultValue(ruleField);
                }
            }
            else if (ruleField.RuleFieldId == 101)
            {
                res = GetCellValueV2(sheet, ruleField);
            }
            else if (ruleField.RuleFieldId == 102)
            {
                res = GetCellValueV2(sheet, ruleField);
            }
            else if (ruleField.RuleFieldId == 205)
            {
                object cellVal = GetCellValueV2(sheet, ruleField);

                res = GetFieldValueByMode205(ruleField, cellVal);
            }

            if (ruleField.HasNeglectStringValue && res.GetType() == typeof(string))
            {
                string resStr = Convert.ToString(res);

                //res = resStr.Replace(ruleField.NeglectStringValue, "");

                res = DeleteNeedNeglectString(ruleField, resStr);
            }

            return res;
        }


        /// <summary>
        /// 获取识别字段（列）值
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetOcrFieldValue(ISheet sheet, int rowIndex, List<ImportRuleField> list)
        {
            string res = "";

            foreach (var item in list)
            {
                //字段单元格位置
                //string cellAddress = item.CellAddress + rowIndex;
                //获取字段列值
                //object cellVal = GetCellValue(sheet, cellAddress);

                object cellVal = GetItemFieldValue3(sheet, item, rowIndex);

                string cellValStr = Convert.ToString(cellVal);

                if (item.HasNeglectStringValue)
                {
                    cellValStr = DeleteNeedNeglectString(item, cellValStr);
                }

                if (string.IsNullOrWhiteSpace(cellValStr))
                {
                    continue;
                }

                res += cellValStr;
            }

            return res;
        }


        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="ruleField"></param>
        /// <returns></returns>
        public static object GetDefaultValue(ImportRuleField ruleField)
        {
            object res = null;

            string dataType = ruleField.FieldInfo.GetString("COL_3");

            if (string.IsNullOrWhiteSpace(dataType))
            {
                return null;
            }

            if (dataType == "varchar")
            {
                res = "";
            }
            else if (dataType == "int" || dataType == "numeric" || dataType == "tinyint")
            {
                res = 0;
            }
            else if (dataType == "float")
            {
                res = (float)0;
            }
            else if (dataType == "datetime")
            {
                res = DateTime.Now;
            }
            else if (dataType == "bit")
            {
                res = 0;
            }
            else
            {
                return null;
            }

            return res;
        }


        /// <summary>
        /// 获取导入规则明细字段值
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="ruleField"></param>
        /// <returns></returns>
        public static object GetItemFieldValue(ISheet sheet, int rowIndex, ImportDataInfo importDataInfo, ImportRuleField ruleField)
        {
            object res = GetDefaultValue(ruleField);

            if (ruleField.RuleFieldId == 601)
            {
                res = DateTime.Now;
            }
            else if (ruleField.RuleFieldId == 602)
            {
                res = ruleField.FieldInfo["COL_10"];
            }
            else if (ruleField.RuleFieldId == 501)
            {
                //到对应的数据表获取字段值
                string tableName = ruleField.FieldInfo.GetString("COL_16");

                SModel tableData = importDataInfo.GetTableDataByName(tableName);

                if (tableData != null)
                {
                    string fieldName = ruleField.FieldInfo.GetString("COL_17");

                    if (tableData.HasField(fieldName))
                    {
                        res = tableData[fieldName];
                    }
                    else
                    {
                        res = GetDefaultValue(ruleField);
                    }
                }
                else
                {
                    res = GetDefaultValue(ruleField);
                }
            }
            else if (ruleField.RuleFieldId == 205)
            {
                //string cellAddress = ruleField.CellAddress + rowIndex;

                //object cellVal = GetCellValueV2(sheet, ruleField, cellAddress);

                object cellVal = GetItemFieldValue3(sheet, ruleField, rowIndex);

                res = GetFieldValueByMode205(ruleField, cellVal);
            }
            else if (ruleField.RuleFieldId == 201 || ruleField.RuleFieldId == 202)
            {
                //res = GetCellValueV2(sheet, ruleField);

                res = GetItemFieldValue3(sheet, ruleField, rowIndex);
            }

            if (res == null)
            {
                return GetDefaultValue(ruleField);
            }

            if (ruleField.HasNeglectStringValue && res.GetType() == typeof(string))
            {
                string resStr = Convert.ToString(res);

                //res = resStr.Replace(ruleField.NeglectStringValue, "");

                res = DeleteNeedNeglectString(ruleField, resStr);
            }

            return res;
        }


        /// <summary>
        /// 根据字段类型转换单元格值
        /// </summary>
        /// <param name="ruleField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object TransitionCellValueByType(ImportRuleField ruleField, object value)
        {
            if (value == null)
            {
                return null;
            }

            string dataType = ruleField.FieldInfo.GetString("COL_3");

            string valueStr = Convert.ToString(value);

            if (string.IsNullOrWhiteSpace(dataType) || string.IsNullOrWhiteSpace(valueStr))
            {
                return GetDefaultValue(ruleField);
            }

            object res = null;

            bool transitionFailure = true;

            if (dataType == "varchar")
            {
                res = valueStr.TrimEnd().TrimStart();

                transitionFailure = false;
            }
            else if (dataType == "int" || dataType == "numeric" || dataType == "tinyint")
            {
                transitionFailure = !int.TryParse(valueStr, out int number);

                res = number;
            }
            else if (dataType == "float")
            {
                transitionFailure = !Double.TryParse(valueStr, out double df);

                res = df;
            }
            else if (dataType == "datetime")
            {
                transitionFailure = !DateTime.TryParse(valueStr, out DateTime datetime);

                res = datetime;
            }
            else if (dataType == "bit")
            {
                transitionFailure = !Boolean.TryParse(valueStr, out Boolean bv);

                res = bv;
            }
            else
            {
                return GetDefaultValue(ruleField);
            }

            if (transitionFailure)
            {
                return GetDefaultValue(ruleField);
            }

            return res;
        }


        /// <summary>
        /// 获取单元格值V2（根据字段类型转换单元格值）
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="ruleField"></param>
        /// <returns></returns>
        public static object GetCellValueV2(ISheet sheet, ImportRuleField ruleField)
        {
            return GetCellValueV2(sheet, ruleField, ruleField.CellAddress);
        }


        /// <summary>
        /// 获取单元格值V2（根据字段类型转换单元格值）
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="ruleField"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static object GetCellValueV2(ISheet sheet, ImportRuleField ruleField, string address)
        {
            object res = GetCellValue(sheet, address);

            return TransitionCellValueByType(ruleField, res);
        }


        /// <summary>
        /// 删除忽略字符串
        /// </summary>
        /// <param name="ruleField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DeleteNeedNeglectString(ImportRuleField ruleField, string value)
        {
            if (!ruleField.HasNeglectStringValue)
            {
                return value;
            }

            string res = value;

            string neglectString = ruleField.NeglectStringValue;

            //CUSTOMER(用户):襄阳工贸家电商贸有限公司木箱共个;件
            //neglectString = "delxxx襄阳,共delxxx,工贸";

            string[] neglectStringSubList = neglectString.Split(',');

            string delCode = "delxxx";

            //删除前面
            string code1 = "$.front.del.";
            //删除后面
            string code2 = "$.back.del.";
            //删除左边开始
            string code3 = "$.left.del.";
            //删除右边开始
            string code4 = "$.right.del.";
            //删除指定
            string code5 = "$.assign.del.";

            foreach (var item in neglectStringSubList)
            {
                bool IsNotSpecified = item.Contains(delCode);

                if (IsNotSpecified)
                {
                    string codeContent = item.Replace(delCode, "");

                    //删除前面
                    bool delFront = item.EndsWith(codeContent);

                    //删除后面
                    bool delBehind = item.StartsWith(codeContent);

                    int dindex = res.IndexOf(codeContent);

                    if (dindex == -1)
                    {
                        return res;
                    }

                    if (delFront)
                    {
                        res = res.Substring(dindex + 1);
                    }

                    if (delBehind)
                    {
                        res = res.Substring(0, dindex);
                    }

                }
                else if (item.StartsWith(code1))
                {
                    string codeContent = item.Replace(code1, "");

                    int dindex = res.IndexOf(codeContent);

                    if (dindex != -1)
                    {
                        res = res.Substring(dindex + 1);
                    }
                }
                else if (item.StartsWith(code2))
                {
                    string codeContent = item.Replace(code2, "");

                    int dindex = res.IndexOf(codeContent);

                    if (dindex != -1)
                    {
                        res = res.Substring(0, dindex);
                    }
                }
                else if (item.StartsWith(code3))
                {
                    string codeContent = item.Replace(code3, "");

                    if (int.TryParse(codeContent, out int dindex))
                    {
                        res = res.Substring(dindex);
                    }
                }
                else if (item.StartsWith(code4))
                {
                    string codeContent = item.Replace(code4, "");

                    if (int.TryParse(codeContent, out int dindex))
                    {
                        res = res.Substring(0, res.Length - dindex);
                    }
                }
                else
                {
                    //指定删除
                    res = res.Replace(item, "");
                }
            }

            return res;
        }


        /// <summary>
        /// 获取明细单元格值
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="ruleField"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public static object GetItemFieldValue2(ISheet sheet, ImportRuleField ruleField, int rowIndex)
        {
            object res = GetDefaultValue(ruleField);

            string caNumber = CommonHelper.CuttingSz(ruleField.CellAddress);

            char splitC = '-';

            bool hasGh = caNumber.Contains("-");
            bool hasXg = caNumber.Contains("/");

            if (hasXg)
            {
                splitC = '/';
            }

            if (string.IsNullOrWhiteSpace(caNumber) && !hasGh && !hasXg)
            {
                //string cellAddress = ruleField.CellAddress.Replace(caNumber, "");

                string cellAddress = ruleField.CellAddress + rowIndex;

                object cellVal = GetCellValueV2(sheet, ruleField, cellAddress);

                res = cellVal;
            }
            else
            {
                string cellAddress = ruleField.CellAddress;

                if (hasGh || hasXg)
                {
                    string[] numbers = caNumber.Split(splitC);

                    foreach (var item in numbers)
                    {
                        cellAddress = cellAddress.Replace(item, rowIndex.ToString());
                    }
                }
                else
                {
                    cellAddress = cellAddress.Replace(caNumber, rowIndex.ToString());
                }

                object cellVal = GetCellValueV2(sheet, ruleField, cellAddress);

                res = cellVal;
            }

            return res;
        }


        /// <summary>
        /// 根据规则【205】方式获取字段值
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="ruleField"></param>
        /// <returns></returns>
        public static object GetFieldValueByMode205(ImportRuleField ruleField, object cellVal)
        {
            object res = GetDefaultValue(ruleField);

            string cellValStr = Convert.ToString(cellVal);

            if (!string.IsNullOrWhiteSpace(cellValStr))
            {
                if (int.TryParse(cellValStr, out int fieldVal))
                {
                    if (fieldVal > 0)
                    {
                        res = (decimal)fieldVal / 100;
                    }
                    else
                    {
                        res = fieldVal;
                    }
                }
                else
                {
                    res = GetDefaultValue(ruleField);
                }
            }
            else
            {
                res = 0;
            }

            return res;
        }


        /// <summary>
        /// 获取明细单元格值
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="ruleField"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public static object GetItemFieldValue3(ISheet sheet, ImportRuleField ruleField, int rowIndex)
        {
            object res = GetDefaultValue(ruleField);

            string address = ruleField.CellAddress;

            //默认分隔符
            char defaultSplitC = '+';

            bool hasSplitC = address.Contains(defaultSplitC);

            if (hasSplitC)
            {
                List<string> addressInfoList = address.Split(defaultSplitC).ToList();

                List<string> cellAddressList = new List<string>();

                string cellValueTemplate = "";

                foreach (var item in addressInfoList)
                {
                    if (item.StartsWith("\""))
                    {
                        cellValueTemplate += item.Replace("\"", "");
                    }
                    else
                    {
                        string fullNewAddress = TransitionItemAddress(item, rowIndex);

                        cellValueTemplate += $"$.{fullNewAddress}";

                        cellAddressList.Add(fullNewAddress);
                    }
                }

                //int index = 1;

                foreach (var ad in cellAddressList)
                {
                    object cellVal = GetCellValueV2(sheet, ruleField, ad);

                    string cellValStr = Convert.ToString(cellVal);

                    //string cellValStr = $"{index++}";

                    cellValueTemplate = cellValueTemplate.Replace($"$.{ad}", cellValStr);
                }

                res = cellValueTemplate;
            }
            else
            {
                string fullNewAddress = TransitionItemAddress(address, rowIndex);

                object cellVal = GetCellValueV2(sheet, ruleField, fullNewAddress);

                string cellValStr = Convert.ToString(cellVal);

                res = cellValStr;
            }

            return res;
        }


        /// <summary>
        /// 转换明细单元格地址
        /// </summary>
        /// <param name="address"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public static string TransitionItemAddress(string address, int rowIndex)
        {
            List<string> addressInfoList = address.Split('-').ToList();

            List<string> addressNewInfoList = new List<string>();

            foreach (var item in addressInfoList)
            {
                string caNumber = CommonHelper.CuttingSz(item);

                string adnew = item.Replace(caNumber, rowIndex.ToString());

                addressNewInfoList.Add(adnew);
            }

            //bool hasGh = caNumber.Contains("-");

            //string[] numbers = caNumber.Split('-');

            //foreach (var n in numbers)
            //{
            //    address = address.Replace(n, rowIndex.ToString());
            //}

            string fullNewAddress = string.Join("-", addressNewInfoList.ToArray());

            return fullNewAddress;
        }


    }


    /// <summary>
    /// 单元格地址
    /// </summary>
    public class CellAddress
    {
        /// <summary>
        /// 原地址
        /// </summary>
        public string AddressText { get; set; }

        /// <summary>
        /// 开始行
        /// </summary>
        public int StartRow { get; set; }

        /// <summary>
        /// 开始列
        /// </summary>
        public int StartCell { get; set; }

        /// <summary>
        /// 结束行
        /// </summary>
        public int EndRow { get; set; }

        /// <summary>
        /// 结束列
        /// </summary>
        public int EndCell { get; set; }

        /// <summary>
        /// 是否合并单元格
        /// </summary>
        public bool IsMerged { get; set; }

        /// <summary>
        /// 是否解析
        /// </summary>
        public bool IsParsed { get; set; } = false;

        public CellAddress Their { get; set; }

        public CellAddress(string address)
        {
            this.AddressText = address;

            this.AnalysisV2(address);
        }

        public CellAddress(string address, CellAddress their)
        {
            this.AddressText = address;
            this.Their = their;
            this.AnalysisV2(address);
        }

        #region - 由数字转换为Excel中的列字母 -

        public static int ToIndex(string columnName)
        {
            if (!Regex.IsMatch(columnName.ToUpper(), @"[A-Z]+")) { throw new Exception("invalid parameter"); }

            int index = 0;
            char[] chars = columnName.ToUpper().ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                index += ((int)chars[i] - (int)'A' + 1) * (int)Math.Pow(26, chars.Length - i - 1);
            }
            return index - 1;
        }


        public static string ToName(int index)
        {
            if (index < 0) { throw new Exception("invalid parameter"); }

            List<string> chars = new List<string>();

            do
            {
                if (chars.Count > 0) index--;
                chars.Insert(0, ((char)(index % 26 + (int)'A')).ToString());
                index = (int)((index - index % 26) / 26);
            } while (index > 0);

            return String.Join(string.Empty, chars.ToArray());
        }

        #endregion


        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="address"></param>
        public void Transition(string address)
        {
            try
            {
                bool hasMh = address.Contains(":");
                bool hasHg = address.Contains("-");

                char splitC = ':';

                if (hasHg)
                {
                    splitC = '-';
                }

                string[] range = address.Split(splitC);

                if (range.Length == 0)
                {
                    return;
                }

                IsParsed = true;

                string rindex = CommonHelper.CuttingSz(range[0]);
                string cindex = CommonHelper.CuttingZm(range[0]);
                StartCell = ToIndex(cindex);
                StartRow = Convert.ToInt32(rindex) - 1;

                if (range.Length >= 2)
                {
                    IsMerged = true;
                    string rindex2 = CommonHelper.CuttingSz(range[1]);
                    string cindex2 = CommonHelper.CuttingZm(range[1]);
                    EndCell = ToIndex(cindex2);
                    EndRow = Convert.ToInt32(rindex2) - 1;
                }
            }
            catch (Exception ex)
            {
                IsParsed = false;
            }
        }


        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="address"></param>
        public void Analysis(string address)
        {
            //默认分隔符
            char defaultSplitC = '/';

            bool hasSplitC = address.Contains(defaultSplitC);

            bool theirHasMoreCell = this.Their != null;

            if (!hasSplitC && !theirHasMoreCell)
            {
                bool hasSplitC2 = address.Contains("-");

                if (hasSplitC2)
                {
                    defaultSplitC = '-';

                    hasSplitC = true;
                }
            }

            if (hasSplitC)
            {
                string[] addressList = address.Split(defaultSplitC);

                this.MoreCellAddressList = new List<CellAddress>();

                foreach (var item in addressList)
                {
                    CellAddress cellAddress = new CellAddress(item, this);

                    this.IsParsed = cellAddress.IsParsed;

                    this.MoreCellAddressList.Add(cellAddress);
                }
            }
            else
            {
                Transition(address);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        public void AnalysisV2(string address)
        {
            //默认分隔符
            char defaultSplitC = '+';

            bool hasSplitC = address.Contains(defaultSplitC);

            if (hasSplitC)
            {
                List<string> addressInfoList = address.Split(defaultSplitC).ToList();

                List<string> cellAddressList = new List<string>();

                string cellValueTemplate = "";

                foreach (var item in addressInfoList)
                {
                    if (item.StartsWith("\""))
                    {
                        cellValueTemplate += item.Replace("\"", "");
                    }
                    else
                    {
                        cellValueTemplate += $"$.{item}";

                        cellAddressList.Add(item);
                    }
                }

                this.CellValueTemplate = cellValueTemplate;

                foreach (var item in cellAddressList)
                {
                    CellAddress cellAddress = new CellAddress(item, this);

                    this.IsParsed = cellAddress.IsParsed;

                    this.MoreCellAddressList.Add(cellAddress);
                }
            }
            else
            {
                Transition(address);
            }


        }


        /// <summary>
        /// 更多单元格地址
        /// </summary>
        public List<CellAddress> MoreCellAddressList { get; set; } = new List<CellAddress>();


        /// <summary>
        /// 是否有更多单元格地址
        /// </summary>
        public bool HasMoreCell
        {
            get
            {
                return this.MoreCellAddressList.Count > 0;
            }
        }


        /// <summary>
        /// 值模板
        /// </summary>
        public string CellValueTemplate { get; set; }


    }



}
