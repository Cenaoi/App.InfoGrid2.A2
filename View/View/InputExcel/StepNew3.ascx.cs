using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using EasyClick.Web.Mini2;
using EC5.Utility;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Bll;
using HWQ.Entity.Filter;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using System.Reflection;

namespace App.InfoGrid2.View.InputExcel
{
    public partial class StepNew3 : WidgetControl, IView
    {


        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            
            base.OnInit(e);
        }

        protected override void OnInitCustomControls(EventArgs e)
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_IMPORT_RULE rule = decipher.SelectModelByPk<IG2_IMPORT_RULE>(id);

            if (rule == null)
            {
                Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
            }

            try
            {

                List<string> excelTitle = InitExcel(rule.SRC_FILE);

                SelectColumn col = this.table1.Columns.FindByDataField("SRC_FIELD_INDEX") as SelectColumn;
                col.Items.Add(new ListItem("-1") { TextEx = "--空--" });

                //添加Excel字段到下拉框
                if (excelTitle.Count > 0)
                {

                    int count = excelTitle.Count;
                    for (int i = 0; i < count; i++)
                    {
                        col.Items.Add(i.ToString(), excelTitle[i]);
                    }

                }

                //如果在映射规则表中已有值，就不用再重复插入数据了
                List<IG2_IMPORT_RULE_MAP> mapList = decipher.SelectModels<IG2_IMPORT_RULE_MAP>("IG2_IMPORT_RULE_ID={0}", id);

                if (mapList.Count > 0)
                {
                    return;
                }


                IG2_TABLE it = decipher.SelectToOneModel<IG2_TABLE>("TABLE_TYPE_ID='TABLE' and ROW_SID >=0 and TABLE_NAME='{0}' ", rule.TARGET_TABLE);

                if (it == null)
                {
                    Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
                }

                List<IG2_TABLE_COL> colList = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID={0} and SEC_LEVEL <= 6 ", it.IG2_TABLE_ID);




                foreach (IG2_TABLE_COL item in colList)
                {
                    IG2_IMPORT_RULE_MAP map = new IG2_IMPORT_RULE_MAP();

                    map.TARGET_FIELD = item.DB_FIELD;
                    map.TARGET_FIELD_TEXT = item.DISPLAY;
                    map.ROW_DATE_CREATE = DateTime.Now;
                    map.ROW_DATE_UPDATE = DateTime.Now;
                    map.IG2_IMPORT_RULE_ID = id;
                    map.EQUAL = "=";
                    map.SRC_FIELD_INDEX = -1;

                    decipher.InsertModel(map);

                }



            }
            catch (Exception ex)
            {
                log.Error("初始化数据出错了!", ex);
            }

        }


        /// <summary>
        /// 显示上传界面
        /// </summary>
        public void ShowInputFile()
        {
            int id = WebUtil.QueryInt("id");

            string url = "/App/InfoGrid2/View/InputExcel/FileUpload.aspx?id=" + id;

            EasyClick.Web.Mini.MiniHelper.EvalFormat("ShowInputFile('{0}')", url);

        }


        /// <summary>
        /// 读取Excel文件的标题
        /// </summary>
        /// <param name="url">Excel文件路径</param>
        public List<string> InitExcel(string url) 
        {
            string path = Server.MapPath(url);

            //存放Excel标题的集合
            List<string> ExcelTitle = new List<string>();


            //判断文件是否存在
            if (!File.Exists(path))
            {
                throw new Exception("模板文件不存在！");
            }


            try
            {

                IWorkbook workbook;

                using (FileStream fs = new FileStream(path, FileMode.Open))
                {

                    //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
                    workbook = new HSSFWorkbook(fs);

                }

                //获取excel的第一个sheet
                ISheet sheet = workbook.GetSheetAt(0);

                //拿到每一行的对象
                IRow row = sheet.GetRow(0);
                //判断行是否有数据
                if (row == null)
                {
                    return null;
                }

                for (int j = 0; j <= row.LastCellNum; j++)
                {
                    ///拿到每一个单元格的对象
                    ICell icell = row.GetCell(j);
                    ///判断单元格是否有数据
                    if (icell == null)
                    {
                        continue;
                    }

                    ExcelTitle.Add(icell.StringCellValue);

                }

                return ExcelTitle;

            }
            catch 
            {
                throw new Exception("读Excel文件出错了！");
            }


        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                this.store1.DataBind();
                this.store2.DataBind();
            }
        }


        /// <summary>
        /// 导入Excel数据
        /// </summary>
        public void GoLast() 
        {

            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_IMPORT_RULE rule = decipher.SelectModelByPk<IG2_IMPORT_RULE>(id);

            if (rule == null)
            {
                Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
            }


            List<IG2_IMPORT_RULE_MAP> mapList = decipher.SelectModels<IG2_IMPORT_RULE_MAP>("IG2_IMPORT_RULE_ID={0} AND ROW_SID >= 0", rule.IG2_IMPORT_RULE_ID);

            if(mapList.Count == 0)
            {
                MessageBox.Alert("没有插入数据");
                return;
            }


            string path = Server.MapPath(rule.SRC_FILE);

            if(!File.Exists(path))
            {
                MessageBox.Alert("没有找到文件！");
                return;
            }



            //必填
            List<IG2_IMPORT_RULE_MAP> requiredFields = new List<IG2_IMPORT_RULE_MAP>();

            //唯一
            List<IG2_IMPORT_RULE_MAP> uniqeFields = new List<IG2_IMPORT_RULE_MAP>();

            //空白跳过
            List<IG2_IMPORT_RULE_MAP> blankSkipFields = new List<IG2_IMPORT_RULE_MAP>();


            foreach (IG2_IMPORT_RULE_MAP ruleMap in mapList)
            {
                if (ruleMap.VALID_REQUIRED)
                {
                    requiredFields.Add(ruleMap);
                }

                if (ruleMap.VALID_UNIQUE)
                {
                    uniqeFields.Add(ruleMap);
                }

                if (ruleMap.VALID_BLANK_SKIP)
                {
                    blankSkipFields.Add(ruleMap);
                }
            }


            int skipCount = 0;

            List<LView> models = null;

            try
            {
                models = ReadExcel(path, rule, mapList, out skipCount);
            }
            catch(Exception ex)
            {
                log.Error(ex);
                MessageBox.Alert(ex.Message);
            }





            decipher.BeginTransaction();

            try
            {
                int autoSkipCount = 0;
                int blankShipCoun = skipCount;

                string errMsg = null;

                foreach (LView view in models)
                {
                    LModel model = view.Model;

                    IG2_IMPORT_RULE_MAP errRule = null;
                    ValidRule vRule = ValidRule.None;

                    bool valid = ValidModel(model, requiredFields, uniqeFields, ref errRule, out vRule, out errMsg);

                    if (!valid )
                    {
                        //如果主键相同，就自动跳过
                        if (vRule == ValidRule.Unique )
                        {
                            if (errRule != null && errRule.VALID_AUTO_SKIP)
                            {
                                autoSkipCount++;
                                log.Debug(string.Format("自动跳过：{0}，工作表 “{1}”，行“{2}”。", errMsg, view.SheetName, view.RowIndex));
                                continue;
                            }
                            else
                            {
                                throw new Exception(string.Format("验证失败x，数据不唯一：{0}，工作表 “{1}”，行“{2}”。", errMsg, view.SheetName, view.RowIndex)); 
                            }
                        }
                        else
                        {
                            throw new Exception(string.Format("验证失败：{0}，工作表 “{1}”，行“{2}”。", errMsg, view.SheetName, view.RowIndex));
                        }


                    }





                    decipher.InsertModel(view.Model);
                }

                decipher.TransactionCommit();

                MessageBox.Alert(string.Format("导入数据成功！共 {0} 条记录，跳过相同的 {1} 行，跳过空白 {2} 行。",
                    models.Count,autoSkipCount,blankShipCoun));
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("导入数据失败", ex);

                MessageBox.Alert("导入数据失败." + ex.Message);

            }
        }
        /// <summary>
        /// 填充默认值
        /// </summary>
        /// <param name="model"></param>
        private void FillDefaultValue(LModel model)
        {

        }

        /// <summary>
        /// 验证规则
        /// </summary>
        enum ValidRule
        {
            None,
            /// <summary>
            /// 必填
            /// </summary>
            Required,
            /// <summary>
            /// 唯一性
            /// </summary>
            Unique,
            /// <summary>
            /// 空白跳过
            /// </summary>
            BlankSkip,
            /// <summary>
            /// 配合唯一性，相同自动跳过
            /// </summary>
            AutoSkip
        }

        /// <summary>
        /// 验证数据唯一性
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool ValidModel(LModel model,List<IG2_IMPORT_RULE_MAP> requiredFields,
            List<IG2_IMPORT_RULE_MAP> uniqeFields,
            ref IG2_IMPORT_RULE_MAP errRule,
            out ValidRule vRule,
            out string errMsg)
        {

            errMsg = null;
            vRule = ValidRule.None;


            foreach (IG2_IMPORT_RULE_MAP ruleMap in requiredFields)
            {
                bool isNull = model.IsNull(ruleMap.TARGET_FIELD);

                if (isNull)
                {
                    errMsg = string.Format("字段“{0}”不能为空!", ruleMap.SRC_FIELD);
                    vRule = ValidRule.Required;

                    errRule = ruleMap;

                    return false;
                }
            }

            if (uniqeFields.Count > 0)
            {

                DbDecipher decipher = ModelAction.OpenDecipher();

                LModelElement modelElem = model.GetModelElement();

                foreach (IG2_IMPORT_RULE_MAP ruleMap in uniqeFields)
                {
                    object value = model[ruleMap.TARGET_FIELD];

                    LightModelFilter filter = new LightModelFilter(modelElem.DBTableName);
                    filter.And(ruleMap.TARGET_FIELD, value);
                    filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

                    bool exist = decipher.ExistsModels(filter);

                    if (exist)
                    {
                        errRule = ruleMap;

                        errMsg = string.Format("字段“{0}”不唯一!", ruleMap.SRC_FIELD);
                        vRule = ValidRule.Unique;

                        return false;
                    }

                }
            }

            return true;
        }

        /// <summary>
        /// 读取Excel文件里面的数据插入到数据库中
        /// </summary>
        /// <param name="url">文件路径</param>
        private List<LView> ReadExcel(string url, IG2_IMPORT_RULE rule, List<IG2_IMPORT_RULE_MAP> mapList,out int allSkipCount) 
        {


            #region 先分组,为处理外连接做准备。

            List<IG2_IMPORT_RULE_MAP> commonMaps = new List<IG2_IMPORT_RULE_MAP>();
            List<IG2_IMPORT_RULE_MAP> foreignMaps = new List<IG2_IMPORT_RULE_MAP>();

            foreach (IG2_IMPORT_RULE_MAP mapItem in mapList)
            {
                if (mapItem.FOREIGN_ENABLED)
                {
                    foreignMaps.Add(mapItem);
                    continue;
                }

                if (mapItem.SRC_FIELD_INDEX >= 0)
                {
                    commonMaps.Add(mapItem);
                    continue;
                }
            }

            #endregion

            List<LView> models = new List<LView>();
            
            allSkipCount = 0;

            try
            {

                IWorkbook workbook;

                using (FileStream fs = new FileStream(url, FileMode.Open))
                {

                    //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
                    workbook = new HSSFWorkbook(fs);

                }


                LModelElement modelElem = LModelDna.GetElementByName(rule.TARGET_TABLE);


                int sheetNum = workbook.NumberOfSheets;

                for (int i = 0; i < sheetNum; i++)
                {
                    //获取excel的第一个sheet

                    ISheet sheet = workbook.GetSheetAt(i);

                    int lastNum = sheet.LastRowNum;

                    UpdateIndex(sheet, mapList);

                    int skipCount = 0;

                    ReadSheet(sheet, rule, mapList, models, modelElem, commonMaps, foreignMaps, out skipCount);


                    allSkipCount += skipCount;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("读 Excel 文件出错了！",ex);
            }


            return models;
        }

        /// <summary>
        /// 封装了的视图实体,针对验证数据使用。能报告错误的工作页和错误的行数
        /// </summary>
        class LView
        {
            /// <summary>
            /// 工作表名称
            /// </summary>
            public string SheetName { get; set; }

            /// <summary>
            /// Excel 中对应的行数
            /// </summary>
            public int RowIndex { get; set; }

            /// <summary>
            /// 原实体
            /// </summary>
            public LModel Model { get; set; }

            public LView(LModel model, string sheetName, int rowIndex)
            {
                this.Model = model;
                this.SheetName = sheetName;
                this.RowIndex = RowIndex;
            }
        }

        /// <summary>
        /// 行处理过程
        /// </summary>
        enum RowProcessType
        {
            /// <summary>
            /// 默认
            /// </summary>
            Auto,
            /// <summary>
            /// 跳过
            /// </summary>
            Skip,
            /// <summary>
            /// 结束
            /// </summary>
            End
        }

        /// <summary>
        /// 读取单个sheet数据
        /// </summary>
        /// <remarks>
        /// 循环每一行记录的时候，先处理"常规"字段，然后才处理"外连接"字段。
        /// 避免找不到外连接找不到值。
        /// </remarks>
        /// <param name="sheet">当前sheet</param>
        /// <param name="rule">导入规则</param>
        /// <param name="mapList">规则映射集合</param>
        /// <param name="models">数据集</param>
        /// <param name="modelElem">拿对应字段的</param>
        /// <param name="commonMaps">常规映射字段</param>
        /// <param name="foreignMaps">外键链接字段</param>
        private void ReadSheet(ISheet sheet, IG2_IMPORT_RULE rule, List<IG2_IMPORT_RULE_MAP> mapList, IList<LView> models, 
            LModelElement modelElem,
            List<IG2_IMPORT_RULE_MAP> commonMaps, List<IG2_IMPORT_RULE_MAP> foreignMaps,out int skipCount)
        {
            int lastNum = sheet.LastRowNum;

            skipCount = 0;

            for (int i = 1; i <= lastNum; i++)
            {
                //拿到每一行的对象
                IRow row = sheet.GetRow(i);

                //判断行是否有数据
                if (row == null)
                {
                    continue ;
                }

                LModel model;

                RowProcessType rowProType = ReadRow(row, rule, modelElem, commonMaps, foreignMaps, out model);

                if (rowProType == RowProcessType.End)
                {
                    return;
                }

                if (rowProType == RowProcessType.Skip)
                {
                    skipCount++;
                    continue;
                }

                LView view = new LView(model, sheet.SheetName, i);
                models.Add(view);
            }


        }

        private RowProcessType ReadRow(IRow row,IG2_IMPORT_RULE rule,
            LModelElement modelElem,
            List<IG2_IMPORT_RULE_MAP> commonMaps, List<IG2_IMPORT_RULE_MAP> foreignMaps,out LModel outModel)
        {

            LModel model = new LModel(rule.TARGET_TABLE);

            outModel = model;

            string errMsg = null;   //错误消息

            foreach (IG2_IMPORT_RULE_MAP mapItem in commonMaps)
            {
                ICell ice = row.GetCell(mapItem.SRC_FIELD_INDEX);

                if (ice == null)
                {
                    if (mapItem.VALID_BLANK_SKIP)
                    {
                        return RowProcessType.Skip;
                    }

                    continue;
                }

                LModelFieldElement fieldElem = modelElem.Fields[mapItem.TARGET_FIELD];

                object excelValue = GetCellValue(ice);  //取 excel

                if (excelValue != null && String.IsNullOrEmpty(excelValue.ToString().Trim()))
                {
                    continue;    //return RowProcessType.Skip;
                }

                object value = HWQ.Entity.ModelConvert.ChangeType(excelValue, fieldElem);

                model[mapItem.TARGET_FIELD] = value;


            }

            foreach (IG2_IMPORT_RULE_MAP mapItem in foreignMaps)
            {
                try
                {
                    object foreignValue = GetForeignValue(mapItem, model, true, out errMsg);

                    model[mapItem.TARGET_FIELD] = foreignValue;
                }
                catch (Exception ex)
                {
                    throw new Exception("处理外联错误,SRC_FIELD=“" + mapItem.SRC_FIELD + "”，TARGET_FIELD=“"
                        + mapItem.TARGET_FIELD + "”", ex);
                }
            }

            return RowProcessType.Auto;
        }

        /// <summary>
        /// 拿到单元格的值
        /// </summary>
        /// <returns></returns>
        private string GetCellValue(ICell icell)
        {


            if (icell.CellType == CellType.Blank)
            {
                return "";
            }

            try
            {
                if (icell.CellType == CellType.Numeric)
                {

                    if (NPOI.SS.UserModel.DateUtil.IsCellDateFormatted(icell))
                    {
                        DateTime dt = icell.DateCellValue;
                        return dt.ToString();

                    }

                    return icell.NumericCellValue.ToString();

                }



            }
            catch (Exception ex)
            {
                log.Error("拿单元格的日期类型出错了！", ex);
            }
        
            return icell.ToString();
        }



        /// <summary>
        /// 获取外链值
        /// </summary>
        /// <param name="mapItem"></param>
        /// <param name="srcModel">原实体</param>
        /// <param name="triggerError">触发异常</param>
        /// <param name="errMsg">错误消息</param>
        private object GetForeignValue(IG2_IMPORT_RULE_MAP mapItem, LModel srcModel, bool triggerError, out string errMsg)
        {
            errMsg = null;

            string reField = mapItem.FOREIGN_RE_FIELD;
            string table = mapItem.FOREIGN_TABLE;

            string filter = mapItem.FOREIGN_FILTER;


            LModelElement modelElem = TableMgr.GetModelElem(table);

            if (modelElem == null)
            {
                errMsg = string.Format("外链处理失败：“{0}”表不存在。", table);

                if (triggerError)
                {
                    throw new Exception(errMsg);
                }

                return null;
            }

            if (!modelElem.Fields.ContainsField(reField))
            {
                errMsg = string.Format("外链处理失败：“{0}”表不存在此“{1}”字段。", table, reField);

                if (triggerError)
                {
                    throw new Exception(errMsg);
                }

                return null;
            }


            App.InfoGrid2.Excel_Template.JTemplate jTemplate = new App.InfoGrid2.Excel_Template.JTemplate(filter);
            jTemplate.Model = srcModel;

            LModelFieldElement fieldElem = modelElem.Fields[reField];

            string tSqlSelect = string.Format("SELECT {0} FROM {1} ", reField, table);

            string tSqlWhere = jTemplate.Exec();


            string tSql = tSqlSelect + "WHERE " + tSqlWhere;

            object resultValue = null;

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                resultValue = decipher.ExecuteScalar(tSql);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("执行语句错误: {0}", tSql), ex);
            }


            return resultValue;

        }



        /// <summary>
        /// 更改规格映射表集合里面的标题索引
        /// </summary>
        /// <param name="sheet">工作薄</param>
        /// <param name="mapList">规则映射表集合</param>
        private void UpdateIndex(ISheet sheet, List<IG2_IMPORT_RULE_MAP> mapList)
        {

            //存放Excel标题的集合
            List<string> excelTitle = new List<string>();

            try
            {
                //拿到每一行的对象
                IRow row = sheet.GetRow(0);

                //判断行是否有数据
                if (row == null)
                {
                    return;
                }

                for (int j = 0; j <= row.LastCellNum; j++)
                {
                    //拿到每一个单元格的对象
                    ICell icell = row.GetCell(j);

                    //判断单元格是否有数据
                    if (icell == null)
                    {
                        continue;
                    }

                    //标题的值如果为空就不用添加到集合中去
                    if (string.IsNullOrEmpty(icell.StringCellValue))
                    {
                        continue;
                    }


                    excelTitle.Add(icell.StringCellValue);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取单个 sheet 标题发生错误！", ex);
            }



            foreach (IG2_IMPORT_RULE_MAP item in mapList)
            {
                int index = excelTitle.IndexOf(item.SRC_FIELD);
                item.SRC_FIELD_INDEX = index;

                if (index >= 0)
                {
                    item.SRC_FIELD_TEXT = excelTitle[index];
                }
                else
                {
                    item.SRC_FIELD_TEXT = "";
                }
            }


        }

        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="defaultValueParam"></param>
        /// <returns></returns>
        private object GetDefaultValue(string defaultValueParam)
        {
            if (StringUtil.StartsWith(defaultValueParam, "(") && StringUtil.EndsWith(defaultValueParam, ")"))
            {

            }
            else
            {
                log.DebugFormat("默认值格式错误:" + defaultValueParam);
                return null;
            }

            EC5.LCodeEngine.JTemplateFunc func;
 
            string funContent = defaultValueParam.Substring(1, defaultValueParam.Length - 2);

            if (!StringUtil.EndsWith(funContent, "()"))
            {
                log.DebugFormat("默认值格式错误: " + defaultValueParam);
                return null;
            }

            string funName = funContent.Substring(0, funContent.Length - 2);

            if (!EC5.LCodeEngine.JTemplateFuncManager.Commons.TryGetItem(funName, out func))
            {
                throw new Exception(string.Format("函数名“{0}”不存在。", funName));
            }

            MethodInfo method = func.Method;


            object result = method.Invoke(func.Owner, null);

            return result;


            //ParameterInfo[] pInfos = method.GetParameters();

            //object[] ps;

            //int j;

            //if (pInfos.Length > 0 && pInfos[0].ParameterType.IsArray)
            //{

            //    object[] ps2 = new object[sp.Length - 1];

            //    for (int i = 1; i < sp.Length; i++)
            //    {
            //        j = i - 1;
            //        ps2[j] = sp[i];
            //    }

            //    ps = new object[] { ps2 };
            //}
            //else
            //{
            //    ps = new object[sp.Length - 1];

            //    for (int i = 1; i < sp.Length; i++)
            //    {
            //        j = i - 1;

            //        ParameterInfo pi = pInfos[j];

            //        Type paramT = pi.ParameterType;

            //        try
            //        {
            //            if (paramT.IsValueType)
            //            {
            //                ps[j] = Convert.ChangeType(sp[i], paramT);
            //            }
            //            else
            //            {
            //                ps[j] = sp[i];
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            throw new Exception(string.Format("“{0}”第 {1} 个参数“{2}”转换为“{3}”错误。",
            //                item.Text, i, sp[i], pInfos[i].ParameterType.FullName), ex);
            //        }
            //    }
            //}

        }

    }
}