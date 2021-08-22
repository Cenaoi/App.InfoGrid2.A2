using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Bll;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;

namespace App.InfoGrid2.View.InputExcel
{
    public partial class StepManyNew3 : WidgetControl, IView
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
                List<string> ExcelTitel = InitExcel(rule.SRC_FILE);

                ///添加Excel字段到下拉框
                if (ExcelTitel.Count > 0)
                {
                    SelectColumn col = this.table1.Columns.FindByDataField("SRC_FIELD_INDEX") as SelectColumn;

                    int count = ExcelTitel.Count;
                    col.Items.Add(new ListItem("-1") { TextEx = "--空--" });

                    for (int i = 0; i < count; i++)
                    {
                        col.Items.Add(i.ToString(), ExcelTitel[i]);
                    }

                }


            }
            catch (Exception ex)
            {
                log.Error("初始化数据出错了!", ex);
            }



        }



        /// <summary>
        /// 读取Excel文件的标题
        /// </summary>
        /// <param name="url">Excel文件路径</param>
        public List<string> InitExcel(string url)
        {
            string path = Server.MapPath(url);

            ///存放Excel标题的集合
            List<string> ExcelTitle = new List<string>();


            ///判断文件是否存在
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

                ///拿到每一行的对象
                IRow row = sheet.GetRow(0);
                ///判断行是否有数据
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
            if (!IsPostBack)
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


            List<IG2_IMPORT_RULE_MAP> mapList = decipher.SelectModels<IG2_IMPORT_RULE_MAP>("IG2_IMPORT_RULE_ID={0}", rule.IG2_IMPORT_RULE_ID);

            if (mapList.Count == 0)
            {
                MessageBox.Alert("没有插入数据");
                return;
            }


            string path = Server.MapPath(rule.SRC_FILE);

            if (!File.Exists(path))
            {
                MessageBox.Alert("没有找到文件！");
                return;
            }


            ReadExcel_2Table(path, rule, mapList);


        }


        class GroupTable
        {
            LModel m_Main;

            List<LModel> m_SubList;

            public LModel Main
            {
                get { return m_Main; }
                set { m_Main = value; }
            }

            public List<LModel> SubList
            {
                get
                {
                    if (m_SubList == null)
                    {
                        m_SubList = new List<LModel>();
                    }
                    return m_SubList;
                }
            }
        }


        /// <summary>
        /// 读取Excel文件里面的数据插入到数据库中
        /// </summary>
        /// <param name="url">文件路径</param>
        private void ReadExcel_2Table(string url, IG2_IMPORT_RULE rule, List<IG2_IMPORT_RULE_MAP> mapList)
        {

            SortedList<string,GroupTable> groups = new SortedList<string, GroupTable>();
            

            LModelElement mainModelElem = LModelDna.GetElementByName(rule.MAIN_TABLE);
            LModelElement subModelElem = LModelDna.GetElementByFullName(rule.TARGET_TABLE);

            try
            {

                IWorkbook workbook;

                using (FileStream fs = new FileStream(url, FileMode.Open))
                {

                    //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
                    workbook = new HSSFWorkbook(fs);

                }


                //获取excel的第一个sheet
                ISheet sheet = workbook.GetSheetAt(0);


                int lastNum = sheet.LastRowNum;


                #region 先分组

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


                for (int i = 1; i <= lastNum; i++)
                {
                    ///拿到每一行的对象
                    IRow row = sheet.GetRow(i);
                    ///判断行是否有数据
                    if (row == null)
                    {
                        return;
                    }


                    LModel mainModel = new LModel(rule.MAIN_TABLE);

                    LModel subModel = new LModel(rule.TARGET_TABLE);

                    GetLModel(mapList, mainModel, mainModelElem, row, commonMaps, foreignMaps);
                    GetLModel(mapList, subModel, subModelElem, row, commonMaps, foreignMaps);

                    string mainJsonValue = mainModel.Get<string>(rule.JOIN_MAIN_FIELE);
                    string subJsonValue = subModel.Get<string>(rule.JOIN_SUB_FIELE);


                    GroupTable models;

                    if (!groups.TryGetValue(mainJsonValue,out models))
                    {
                        models = new GroupTable();

                        models.Main = mainModel;

                        groups.Add(mainJsonValue,models);
                    }

                    models.SubList.Add(subModel);

                }



            }
            catch (Exception ex)
            {

                throw new Exception("读Excel文件出错了！", ex);
            }



            DbDecipher decipher = ModelAction.OpenDecipher();


            int subCount = 0;
            decipher.BeginTransaction();

            try
            {
                string mainPkField = mainModelElem.PrimaryKey;
                string subJoinPField = rule.JOIN_SUB_P_FIELE;

                foreach (GroupTable group in groups.Values)
                {
                    decipher.InsertModel(group.Main);

                    object parentId = group.Main[mainPkField];

                    foreach (LModel subLModel in group.SubList)
                    {
                        subLModel[subJoinPField] = parentId;
                        decipher.InsertModel(subLModel);

                        subCount++;
                    }

                }


                decipher.TransactionCommit();

                MessageBox.Alert("导入数据成功！主表 " + groups.Count + " 条记录,子表 " + subCount + " 条记录");
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("插入数据错误", ex);

                MessageBox.Alert("导入数据错误.");
            }



        }






        /// <summary>
        /// 获取外链值
        /// </summary>
        /// <param name="mapItem"></param>
        /// <param name="triggerException">触发异常</param>
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
        /// 更改一个实体数据
        /// </summary>
        /// <param name="mapList"></param>
        /// <returns></returns>
        public void GetLModel(List<IG2_IMPORT_RULE_MAP> mapList, LModel model, LModelElement modelElem,IRow row,
            List<IG2_IMPORT_RULE_MAP> commonMaps, List<IG2_IMPORT_RULE_MAP> foreignMaps) 
        {

            string errMsg = null;

            foreach (IG2_IMPORT_RULE_MAP mapItem in commonMaps)
            {
                if (mapItem.TARGET_TABLE != modelElem.DBTableName)
                {
                    continue;
                }


                ICell ice = row.GetCell(mapItem.SRC_FIELD_INDEX);

                if (ice == null)
                {
                    continue;
                }

                LModelFieldElement fieldElem = modelElem.Fields[mapItem.TARGET_FIELD];

                object excelValue = GetCellValue(ice);//取 excel 

                object value = HWQ.Entity.ModelConvert.ChangeType(excelValue, fieldElem);

                model[mapItem.TARGET_FIELD] = value;


            }

            foreach (IG2_IMPORT_RULE_MAP mapItem in foreignMaps)
            {
                if (mapItem.TARGET_TABLE != modelElem.DBTableName)
                {
                    continue;
                }

                object foreignValue = GetForeignValue(mapItem, model, true, out errMsg);

                model[mapItem.TARGET_FIELD] = foreignValue;
            }
                      


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

    }
}