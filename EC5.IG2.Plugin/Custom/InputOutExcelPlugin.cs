using System;
using System.Collections.Generic;
using System.Web;
using EasyClick.Web.Mini2;
using Newtonsoft.Json.Linq;
using HWQ.Entity.LightModels;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model.DataSet;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.Util;
using App.InfoGrid2.Model;
using EC5.Utility.Web;
using System.Collections.Specialized;
using EC5.Utility;
using System.Text;
using HWQ.Entity;

namespace EC5.IG2.Plugin.Custom
{
    /// <summary>
    /// 导出Excel插件
    /// </summary>
    public class InputOutExcelPlugin : PagePlugin
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 包含字段名
        /// </summary>
        public bool IncludeFieldName { get; set; }

        /// <summary>
        /// 页面上的标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 导出
        /// 修改于2018 - 11- 22 
        /// 小渔夫
        /// storeUi.Select() 改成 storeUi.GetList()
        /// </summary>
        public void InputOut()
        {

            Store storeUi = this.SrcStore;
            Table tableUi = this.SrcTable;
            string srcUrl = this.SrcUrl;

            string plugParam = this.Params;

            Uri uri = new Uri(this.SrcUrl);


            NameValueCollection queryList = HttpUtility.ParseQueryString(uri.Query, Encoding.UTF8);

            string idStr = queryList["id"];

            int table_id = StringUtil.ToInt(idStr);




            //这是服务器路径
            string urlPaht = "";


            List<LModel> lm;

            try
            {
                //lm = (List<LModel>)storeUi.Select();
                lm = storeUi.GetList() as List<LModel>;

            }
            catch (Exception ex)
            {
                log.Error("获取数据集出错了！", ex);
                MessageBox.Alert("获取数据集出错了！");
                return;
            }



            try
            {
                //   /_Temporary/Excel/g

                //导出路径
                string mapath = System.Web.HttpContext.Current.Server.MapPath("/_Temporary/Excel");

                //判断文件夹是否存在
                if (!Directory.Exists(mapath))
                {
                    Directory.CreateDirectory(mapath);
                }

                //文件名为当前时间时分秒都有
                string fileName = DateTime.Now.ToString("yyMMddHHmmss");

                //这是绝对物理路径
                string filePath = string.Format("{0}\\{1}.xls", mapath, fileName);

                //这是服务器路径
                urlPaht = string.Format("/_Temporary/Excel/{0}.xls", fileName);



                InputOutExcelFile(table_id, lm, filePath);


                string downloa = "Mini2.create('Mini2.ui.extend.DownloadWindow', {fileName: '下载 Excel 文件',fielUrl:'" + urlPaht + "'}).show();";

                EasyClick.Web.Mini.MiniHelper.Eval(downloa);

            }
            catch (Exception ex)
            {
                log.Error("导出Excel文件出错了！", ex);

                MessageBox.Alert("导出 Excel 文件错误");

            }


        }



        /// <summary>
        /// 导出Excel文件
        /// </summary>
        /// <param name="id">表ID</param>
        /// <param name="models">数据仓库</param>
        /// <param name="path">Excel文件存放路径</param>
        public void InputOutExcelFile(int id, List<LModel> models, string path)
        {
            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();
                TableSet ts = TableSet.SelectSID_0_5(decipher, id);


                #region 加入用户权限

                PagePluginEventArgs ea = new PagePluginEventArgs("sec_filter");
                ea.Params["display"] = ts.Table.DISPLAY;
                ea.Params["table_set"] = ts;

                OnDymEvent(ea);

                #endregion


                if (ts == null)
                {
                    return;
                }


                IWorkbook workbook = new HSSFWorkbook();

                ISheet sheet = workbook.CreateSheet("sheet1");

                int colCount = GetVisibleCount(ts.Cols);

                string display = (string)ea.Params["display"];

                //界面上的标题不为空就用界面上的标题
                if (!string.IsNullOrWhiteSpace(Title))
                {
                    display = Title;
                }


                //设置标题
                SetHead(sheet, colCount, workbook, display);

                //设置数据
                SetData(sheet, workbook, ts, models);

                //新建一个Excel文件
                FileStream file = new FileStream(path, FileMode.OpenOrCreate);

                //把数据流写进Excel中
                workbook.Write(file);

                file.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("导出 Excel文件出错了！", ex);
            }
        }

        /// <summary>
        /// 获取显示列的数量
        /// </summary>
        /// <param name="cols"></param>
        /// <returns></returns>
        private int GetVisibleCount(List<IG2_TABLE_COL> cols)
        {
            int n = 0;

            foreach (var item in cols)
            {

                if (item.IS_VISIBLE && item.IS_LIST_VISIBLE)
                {
                    n++;
                }

            }

            return n;
        }

        /// <summary>
        /// 这是设置标题样式和值的
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="count"></param>
        /// <param name="workbook"></param>
        private void SetHead(ISheet sheet, int count, IWorkbook workbook, string value)
        {

            ICellStyle ics = workbook.CreateCellStyle();



            ics.Alignment = HorizontalAlignment.Center; //左右对齐方式


            ics.VerticalAlignment = VerticalAlignment.Center;   //垂直对齐方式

            IFont ifont = workbook.CreateFont();


            ifont.FontHeightInPoints = 25;  //字体大小


            ifont.FontName = "新宋体"; //字体名称


            ifont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;    //字体粗细  

            ics.SetFont(ifont);

            IRow head = sheet.CreateRow(0);

            ICell headCell = head.CreateCell(0);

            //单元格值
            headCell.SetCellValue(value);

            headCell.CellStyle = ics;

            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, count - 1));

        }



        private void SetData(ISheet sheet, IWorkbook workbook, TableSet ts, List<LModel> models)
        {
            try
            {
                ICellStyle ics = workbook.CreateCellStyle();

                //左右对齐方式
                ics.Alignment = HorizontalAlignment.Center;
                //垂直对齐方式
                ics.VerticalAlignment = VerticalAlignment.Center;

                IFont ifont = workbook.CreateFont();
                //字体大小
                ifont.FontHeightInPoints = 12;
                //字体名称
                ifont.FontName = "新宋体";
                //字体粗细
                ifont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

                ics.SetFont(ifont);

                List<IG2_TABLE_COL> cols = GetDisplayCols(ts);


                int i = 1;  //从第二行开始

                IRow ir;

                int colIndex = 0;

                if (this.IncludeFieldName)
                {
                    ir = sheet.CreateRow(i);

                    colIndex = 0;

                    foreach (var item in SrcTable.Columns)
                    {
                        if (!item.Visible)
                        {
                            continue;
                        }

                        ICell ic = ir.CreateCell(colIndex++);

                        ic.SetCellValue(item.DataField);

                        ic.CellStyle = ics;

                    }

                    //foreach (IG2_TABLE_COL col in cols)
                    //{
                    //    ICell ic = ir.CreateCell(colIndex++);

                    //    ic.SetCellValue(col.DB_FIELD);

                    //    ic.CellStyle = ics;
                    //}

                    i++;
                }



                ir = sheet.CreateRow(i);

                colIndex = 0;

                foreach (var item in SrcTable.Columns)
                {

                    if (!item.Visible)
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.DataField))
                    {
                        continue;
                    }


                    ICell ic = ir.CreateCell(colIndex++);

                    ic.SetCellValue(item.HeaderText);

                    ic.CellStyle = ics;

                }

                //foreach (IG2_TABLE_COL col in cols)
                //{
                //    ICell ic = ir.CreateCell(colIndex++);

                //    ic.SetCellValue(col.DISPLAY);

                //    ic.CellStyle = ics;

                //}

                i++;


                foreach (LModel lm in models)
                {
                    NewRow(sheet, workbook, cols, lm, i);

                    i++;
                }
               
                ACOUNT(sheet, workbook, cols, i);
            }
            catch (Exception ex)
            {
                throw new Exception("设置数据部分出错了！", ex);
            }
        }


        Dictionary<string, int> dic = new Dictionary<string, int>();

        private void NewRow(ISheet sheet, IWorkbook workbook, List<IG2_TABLE_COL> cols, LModel lm, int rowIndex)
        {
            IRow ir = sheet.CreateRow(rowIndex);


            LModelFieldElement fieldElem;

            LModelElement modelElem = lm.GetModelElement();

            ICell ic;


            int j = -1;

            //这是直接从界面表格中拿列
            foreach (BoundField item in SrcTable.Columns)
            {
                if (!item.Visible)
                {
                    continue;
                }
                



                if (string.IsNullOrWhiteSpace(item.DataField))
                {

                    continue;

                }

                if(!modelElem.TryGetField(item.DataField, out  fieldElem))
                {
                    continue;
                }


                j++;


                if (lm.IsNull(item.DataField))
                {
                    ic = ir.CreateCell(j);
                    ic.SetCellValue("");

                    continue;
                }


                ic = ir.CreateCell(j);

                
                if (item.SummaryType == "SUM")
                {
                    if (!dic.ContainsKey(item.DataField))
                    {
                        dic.Add(item.DataField, j);
                    }

                   
                }

                string value;

                //数值类型
                if (item is NumColumn)
                {
                    ic.SetCellType(CellType.Numeric);
                    ic.SetCellValue(lm.Get<double>(item.DataField));

                    //value = string.Format("{0:0.######}", lm[dbField]);
                }
                //时间类型
                else if (item is DateColumn dateCol)
                {
                    DateTime dateValue = (DateTime)lm[item.DataField];

                    
                    ic.SetCellType(CellType.String);

                    if (dateCol.DataFormatString == "Y-m-d" || dateCol.Format == "Y-m-d")
                    {
                        log.Debug($"检测到日期类型... value={dateValue}");

                        ic.SetCellValue("'" + dateValue.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        ic.SetCellValue(dateValue.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
                //下拉框类型
                else if (item is SelectColumn)
                {

                    value = lm[item.DataField].ToString();

                    SelectColumn sc = item as SelectColumn;

                    foreach (var select_item in sc.Items)
                    {

                        if (select_item.Value == value)
                        {
                            value = select_item.Text;

                            ic.SetCellValue(value);

                            break;

                        }



                    }

                }
                //下拉框类型
                else if (item is SelectBaseColumn)
                {

                    value = lm[item.DataField].ToString();

                    SelectBaseColumn sc = item as SelectBaseColumn;


                    var select_item = sc.Items.FindBy(sc.ItemValueField, value);

                    //在item中找不到值就用直接值
                    if (select_item == null)
                    {
                        ic.SetCellValue(value);

                    }
                    else
                    {

                        string display_text = select_item.GetAttribute(sc.ItemDisplayField);


                        value = display_text;


                        ic.SetCellValue(value);

                    }

                }
                else
                {

                    if ( item.DataFormatString == "Y-m-d" )
                    {
                        DateTime dateValue = (DateTime)lm[item.DataField];

                        ic.SetCellType(CellType.String);
                        ic.SetCellValue( dateValue.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        value = lm[item.DataField].ToString();
                        ic.SetCellValue(value);
                    }

                }






            }

            //foreach (IG2_TABLE_COL col in cols)
            //{
            //    if (!col.IS_VISIBLE || !col.IS_LIST_VISIBLE)
            //    {
            //        continue;
            //    }

            //    j++;

            //    string dbField = col.DB_FIELD;

            //    if (!modelElem.TryGetField(dbField, out fieldElem))
            //    {
            //        continue;
            //    }

            //    if (lm.IsNull(dbField))
            //    {
            //        ic = ir.CreateCell(j);
            //        ic.SetCellValue("");

            //        continue;
            //    }


            //    ic = ir.CreateCell(j);
            //    string value;

            //    if (fieldElem.IsNumber)
            //    {
            //        ic.SetCellType(CellType.NUMERIC);
            //        ic.SetCellValue(Convert.ToDouble(lm[dbField]));

            //        //value = string.Format("{0:0.######}", lm[dbField]);
            //    }
            //    else if (ModelHelper.IsDataTimeType(fieldElem.DBType))
            //    {

            //        ic.SetCellType(CellType.STRING);
            //        ic.SetCellValue(((DateTime)lm[dbField]).ToString("yyyy-MM-dd HH:mm:ss"));
            //    }
            //    else
            //    {
            //        value = lm[dbField].ToString();
            //        ic.SetCellValue(value);
            //    }

            //}
        }

        /// <summary>
        /// 获取显示的列
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        private List<IG2_TABLE_COL> GetDisplayCols(TableSet ts)
        {
            List<IG2_TABLE_COL> cols = new List<IG2_TABLE_COL>();


            foreach (IG2_TABLE_COL col in ts.Cols)
            {
                if (!col.IS_VISIBLE || !col.IS_LIST_VISIBLE)
                {
                    continue;
                }

                cols.Add(col);
            }

            return cols;
        }


/// <summary>
/// 合计
/// </summary>
/// <param name="sheet"></param>
/// <param name="workbook"></param>
/// <param name="cols"></param>
/// <param name="rowIndex"></param>
       private void  ACOUNT(ISheet sheet, IWorkbook workbook, List<IG2_TABLE_COL> cols, int rowIndex)
        {
            IRow ir = sheet.CreateRow(rowIndex);


            ICell ic;

            int j = -1;

            foreach (var item in SrcTable.Columns)
            {

                j++;
                if (!item.Visible)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(item.DataField))
                {

                    continue;

                }

                if (item.SummaryType != "SUM")
                {

                    continue;
                }

                int numj = dic[item.DataField];

                decimal sum = this.SrcStore.GetSummary(item.DataField, SummaryType.SUM);

                ic = ir.CreateCell(numj);

                //string value;

                //value = lm[item.DataField].ToString();
                //数值类型


                ic.SetCellType(CellType.Numeric);

                ic.SetCellValue(Convert.ToDouble(sum));

                   
                

            }

            //foreach (IG2_TABLE_COL item in cols)
            //{
                

            //    string fild=item.DB_FIELD;

            //    if (item.SUMMARY_TYPE == "SUM")
            //    {

            //        ic = ir.CreateCell(colIndex);
            //        decimal sum = this.SrcStore.GetSummary(item.DB_FIELD, SummaryType.SUM);

            //        ic.SetCellValue(Convert.ToDouble(sum));

            //    }

            //    colIndex++;
            //}

        }

    }
}