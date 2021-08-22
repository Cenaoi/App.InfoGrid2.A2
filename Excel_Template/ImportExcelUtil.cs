using HWQ.Entity.LightModels;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace App.InfoGrid2.Excel_Template
{
    /// <summary>
    /// 导入Excel文件帮助类
    /// </summary>
   public class ImportExcelUtil
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 根据 Excel 文件
        /// </summary>
        /// <param name="table_name"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static List<LModel> CreateLModels(string table_name, string path)
        {
             FileStream stream = File.OpenRead(path);

            return CreateLModels(table_name, stream);
        }

        /// <summary>
        /// 根据传进来的excel数据流，创建 数据集合  
        /// </summary>
        /// <param name="table_name">表名</param>
        /// <param name="stream">数据流</param>
        /// <returns>数据集合</returns>
        public static List<LModel> CreateLModels(string table_name, Stream stream)
        {


            //MemoryStream stream = new MemoryStream(btyes);


            IWorkbook workbook = new HSSFWorkbook(stream);

            //获取excel的第一个sheet
            HSSFSheet sheet = workbook.GetSheetAt(0) as HSSFSheet;

            //行的长度
            int row_length = sheet.LastRowNum;

            if(row_length < 2)
            {
                throw new Exception("excel数据不正确，行数不能小于2！");
            }

            LModelElement modelElem = LModelDna.GetElementByName(table_name);
            LModelFieldElement fieldElem;

            IRow field_row = sheet.GetRow(0);

            List<LModel> lms = new List<LModel>();


            //从第三行开始拿数据
            for (int i = 2; i <= row_length; i++)
            {
                //拿到每一行的对象
                IRow row = sheet.GetRow(i);

                if (row == null)
                {
                    continue;
                }

                LModel lm = new LModel(table_name);

                //把数据放到相应的列里面
                for (int j = 0; j < row.Cells.Count; j++)
                {
                    string field_text = GetField(field_row,j);

                    if(!modelElem.TryGetField(field_text,out fieldElem))
                    {
                        throw new Exception($"这个'{field_text}'字段在'{table_name}'表里没有");
                    }

                    ICell ic = row.GetCell(j);
                    
                    object value = GetCellValue(row, j);

                    try
                    {
                        lm[field_text] = HWQ.Entity.ModelConvert.ChangeType(value, fieldElem);
                    }
                    catch(Exception ex)
                    {
                        throw new Exception($"转换类型出错了！字段 '{field_text}',获取第{i}行的第{j}列出错了！",ex);
                    }
                }

                lms.Add(lm);

            }

            return lms;


        }


        /// <summary>
        /// 获取字段名称
        /// </summary>
        /// <param name="ir">行对象</param>
        /// <param name="index">列索引</param>
        /// <returns></returns>
        static string GetField(IRow ir,int index)
        {

            ICell ic = ir.GetCell(index);

            if(ic == null || ic.CellType == CellType.Blank)
            {
                throw new Exception($"获取第{ir.RowNum}行的第{index}列出错了！");
            }

            return ic.ToString();

        }

        /// <summary>
        /// 获取单元格里面的值
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="index">单元格索引</param>
        /// <returns>对象</returns>
        static object GetCellValue(IRow row, int index)
        {

            ICell ic = row.GetCell(index);

            if (ic == null || ic.CellType == CellType.Blank)
            {
                return DBNull.Value;
            }

            try
            {

                if (ic.CellType == CellType.Numeric)
                {

                    ICellStyle style = ic.CellStyle;

                    int i = style.DataFormat;

                    string fs = style.GetDataFormatString();

                    //把日期格式中的中文去掉，就能判断是否是日期了
                    fs = Regex.Replace(fs, "\"|\'|年|月|日|时|分|秒|毫秒|微秒", string.Empty);

                    if (NPOI.SS.UserModel.DateUtil.IsADateFormat(i, fs))
                    {
                        DateTime dt = ic.DateCellValue;
                        return dt;
                    }

                    return ic.NumericCellValue;

                }
                else if (ic.CellType == CellType.String)
                {
                    return ic.StringCellValue;
                }
                else
                {

                    return ic.ToString();

                }

            }
            catch (Exception ex)
            {
                log.Error("拿单元格的日期类型出错了！", ex);
                throw ex;
            }

        }


    }
}
