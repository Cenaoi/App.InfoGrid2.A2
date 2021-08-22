using System;
using System.Collections.Generic;
using System.Web;
using EasyClick.Web.Mini2;
using Newtonsoft.Json.Linq;
using HWQ.Entity.LightModels;
using System.IO;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model.DataSet;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using App.InfoGrid2.Model;
using System.Text;
using System.Collections.Specialized;
using EC5.Utility;

namespace EC5.IG2.Plugin.Custom
{
    /// <summary>
    /// 打印插件
    /// </summary>
    public class PrintPlugin : PagePlugin
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void PrintExcel() 
        {
            Store storeUi = this.SrcStore;
            Table tableUi = this.SrcTable;
            string srcUrl = this.SrcUrl;

            string plugParam = this.Params;

            Uri uri = new Uri(this.SrcUrl);


            NameValueCollection queryList = HttpUtility.ParseQueryString(uri.Query, Encoding.UTF8);

            string idStr = queryList["id"];


            int table_id = StringUtil.ToInt(idStr);


            JObject ppm;


            string title;

            ///这是服务器路径
            string urlPaht = "";

            //try
            //{
            //    ppm = JObject.Parse(plugParam);

            //    title = ppm.Value<string>("title");


            //}
            //catch (Exception ex)
            //{
            //    log.Error("解析命令参数出错", ex);
            //    MessageBox.Alert("解析命令参数出错.");
            //    return;
            //}

            List<LModel> lm;
            try
            {

                lm = (List<LModel>)storeUi.Select();

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

                ///导出路径
                string mapath = System.Web.HttpContext.Current.Server.MapPath("/_Temporary/Excel");

                ///判断文件夹是否存在
                if (!Directory.Exists(mapath))
                {
                    Directory.CreateDirectory(mapath);
                }

                ///文件名为当前时间时分秒都有
                string fileName = DateTime.Now.ToString("yyMMddHHmmss");

                ///这是绝对物理路径
                string filePath = string.Format("{0}\\{1}.xls", mapath, fileName);

                ///这是服务器路径
                urlPaht = string.Format("{0}/{1}.xls", "/_Temporary/Excel", fileName);



                InputOutExcelFile(table_id, lm, filePath);
            }
            catch (Exception ex)
            {
                log.Error("导出Excel文件出错了！", ex);

                MessageBox.Alert("打印失败!");

                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();


            BIZ_PRINT_FILE pd = new BIZ_PRINT_FILE();

            pd.FILE_URL = urlPaht;

            ///以后有打印机名要添加进去
            pd.PRINT_NAME = "";

            try
            {
                decipher.InsertModel(pd);

            }
            catch (Exception ex) 
            {
                log.Error("打印失败,插入打印信息出错了！", ex);
                MessageBox.Alert("打印失败！");
                return;
            }



        }

        /// <summary>
        /// 导出Excel文件
        /// </summary>
        /// <param name="id">表ID</param>
        /// <param name="title">Excel第一行标题</param>
        /// <param name="models">数据仓库</param>
        /// <param name="path">Excel文件存放路径</param>
        private void InputOutExcelFile(int id, List<LModel> models, string path)
        {
            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();
                TableSet ts = TableSet.SelectSID_0_5(decipher, id);

                if (ts == null)
                {
                    return;
                }


                IWorkbook workbook = new HSSFWorkbook();

                ISheet sheet = workbook.CreateSheet("sheet1");
                ///设置标题
                SetHead(sheet, ts.Cols.Count, workbook, ts.Table.DISPLAY);
                ///设置数据
                SetData(sheet, workbook, ts, models);


                for (int i = 0; i < ts.Cols.Count - 1; i++ )
                {
                    sheet.SetColumnWidth(i, 15 * 256);
                }

               // sheet.DefaultColumnWidth = 100 * 256;

                ///新建一个Excel文件
                FileStream file = new FileStream(path, FileMode.OpenOrCreate);

                ///把数据流写进Excel中
                workbook.Write(file);

                file.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("导出Excel文件出错了！", ex);
            }
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

            ///左右对齐方式
            ics.Alignment = HorizontalAlignment.Center;
            ///垂直对齐方式
            ics.VerticalAlignment = VerticalAlignment.Center;

            IFont ifont = workbook.CreateFont();
            ///字体大小
            ifont.FontHeightInPoints = 25;
            ///字体名称
            ifont.FontName = "新宋体";
            ///字体粗细
            ifont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

            ics.SetFont(ifont);

            IRow head = sheet.CreateRow(0);

            ICell headCell = head.CreateCell(0);
            ///单元格值
            headCell.SetCellValue(value);

            headCell.CellStyle = ics;
            ///合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, count - 2));

        }

        private void SetData(ISheet sheet, IWorkbook workbook, TableSet ts, List<LModel> models)
        {
            try
            {
                IRow ir = sheet.CreateRow(1);
                ICellStyle ics = workbook.CreateCellStyle();

                ///左右对齐方式
                ics.Alignment = HorizontalAlignment.Center;
                ///垂直对齐方式
                ics.VerticalAlignment = VerticalAlignment.Center;

                ///下边框
                ics.BorderBottom = BorderStyle.Thin;
                ///左边框
                ics.BorderLeft = BorderStyle.Thin;
                ///右边框
                ics.BorderRight = BorderStyle.Thin;
                ///上边框
                ics.BorderTop = BorderStyle.Thin;


                IFont ifont = workbook.CreateFont();
                ///字体大小
                ifont.FontHeightInPoints = 12;
                ///字体名称
                ifont.FontName = "新宋体";
                ///字体粗细
                ifont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

                ics.SetFont(ifont);

                int i = 0;

                foreach (IG2_TABLE_COL col in ts.Cols)
                {
                    if (!col.IS_VISIBLE || !col.IS_LIST_VISIBLE)
                    {
                        continue;
                    }

                    ICell ic = ir.CreateCell(i);

                    ic.SetCellValue(col.DISPLAY);

                    ic.CellStyle = ics;

                    i++;
                }

                i = 2;


                foreach (LModel lm in models)
                {
                    LModelElement modelElem = lm.GetModelElement();

                    ir = sheet.CreateRow(i);

                    int j = 0;

                    foreach (IG2_TABLE_COL col in ts.Cols)
                    {
                        if (!col.IS_VISIBLE || !col.IS_LIST_VISIBLE)
                        {
                            continue;

                        }

                        if (modelElem.Fields.ContainsField(col.DB_FIELD))
                        {

                            if (lm[col.DB_FIELD] != null)
                            {
                                string value = lm[col.DB_FIELD].ToString();
                                ICell ic = ir.CreateCell(j);
                                ic.SetCellValue(value);
                                ic.CellStyle = ics;
                            }
                            else
                            {
                                ICell ic = ir.CreateCell(j);
                                ic.SetCellValue("");
                                ic.CellStyle = ics;
                            }
                        }

                        j++;

                    }


                    i++;
                }
            }
            catch
            {
                throw new Exception("设置数据部分出错了！");
            }
        }


    }
}