using App.BizCommon;
using EasyClick.Web.Mini2;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.Biz.Rosin.ImportExcel
{
    public partial class SelectExcel2 : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.fieldUpdate1.Uploader += FieldUpdate1_Uploader;
        }

        private void FieldUpdate1_Uploader(object sender, EventArgs e)
        {

            int row_id = WebUtil.QueryInt("row_id");

            //拿到文件名
            string fileName = this.fieldUpdate1.FileName;

            //判断是否为空
            if (StringUtil.IsBlank(fileName))
            {
                MessageBox.Alert("请选择要上传的文件");
                return;
            }

            FileInfo fileInfo = new FileInfo(fileName);
            //看文件是否是zip格式的
            if (fileInfo.Extension != ".xls")
            {
                MessageBox.Alert("只能上传xls文件");
                return;
            }


            MemoryStream stream = new MemoryStream(this.fieldUpdate1.FileBytes);


            IWorkbook workbook = new HSSFWorkbook(stream);

            //获取excel的第一个sheet
            HSSFSheet sheet = workbook.GetSheetAt(0) as HSSFSheet;

            //行的长度
            int row_length = sheet.LastRowNum;

            List<LModel> lm009s = new List<LModel>();

            for (int i = 1; i <= row_length; i++)
            {
                ///拿到每一行的对象
                IRow row = sheet.GetRow(i);

                if (row == null)
                {
                    continue;
                }

                //fs = fs.replaceAll("[\"|\']","").replaceAll("[年|月|日|时|分|秒|毫秒|微秒]", "");

                LModel lm009 = new LModel("UT_009");
                lm009["COL_1"] = row_id;
                lm009["IO_TAG"] = "I";
                lm009["COL_27"] = GetCellValue(row, 0);                         //货主
                lm009["COL_28"] = GetCellValue(row, 1);                         //等级
                lm009["COL_29"] = GetCellValue(row, 2);                         //厂家
                lm009["COL_2"] = GetCellValue(row, 3);                          //货物卡号
                lm009["PROD_CODE"] = GetCellValue(row, 4);                      //货物编号
                lm009["COL_3"] = GetCellValue(row, 5);                          //货物品名
                lm009["COL_4"] = GetCellValue(row, 6);                          //材料
                lm009["COL_5"] = GetCellValue(row, 7);                          //规格
                lm009["COL_6"] = GetCellValue(row, 8);                          //品牌
                lm009["COL_7"] = GetCellValue(row, 9);                          //场地
                lm009["COL_8"] = GetCellValue(row, 10);                         //车箱号
                lm009["COL_9"] = GetCellValue(row, 11);                         //到货日期
                lm009["COL_10"] = GetCellValue(row, 12);                        //验收时间
                lm009["COL_11"] = GetCellValue(row, 13);                        //验收方式
                lm009["COL_12"] = GetCellValue(row, 14);                        //工人装卸
                lm009["COL_13"] = GetCellValue(row, 15);                        //分拣
                lm009["COL_14"] = GetCellValue(row, 16);                        //换袋
                lm009["COL_15"] = GetCellValue(row, 17);                        //打包材料
                lm009["COL_16"] = GetCellValue(row, 18);                        //存放货位
                lm009["COL_17"] = GetCellValue(row, 19);                        //货位备注
                lm009["COL_18"] = GetCellValue(row, 20);                        //应收数量
                lm009["COL_19"] = GetCellValue(row, 21);                        //应收重量
                lm009["COL_20"] = GetCellValue(row, 22);                        //应收散件
                lm009["COL_21"] = GetCellValue(row, 23);                        //数量
                lm009["COL_22"] = GetCellValue(row, 24);                        //数量单位名称
                lm009["COL_23"] = GetCellValue(row, 25);                        //重量
                lm009["COL_24"] = GetCellValue(row, 26);                        //重量单位名称
                lm009["COL_25"] = GetCellValue(row, 27);                        //散件
                lm009["COL_26"] = GetCellValue(row, 28);                        //货物备注
                lm009["PACK_TEXT"] = GetCellValue(row, 29);                     //包装
                lm009["PROD_DATE"] = GetCellValue(row, 30);                     //生产日期

                lm009s.Add(lm009);

            }


            if (lm009s.Count == 0)
            {
                EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok'});");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.BeginTransaction();

            try
            {


                decipher.InsertModels<LModel>(lm009s);

                decipher.TransactionCommit();

                
                EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok'});");

            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();
                log.Error("导入Excel数据出错了！", ex);

                MessageBox.Alert("额，出错了！请检查Excel数据是否有错！");

                return;
            }



        }

        /// <summary>
        /// 获取单元格里面的值
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="index">单元格索引</param>
        /// <returns>对象</returns>
        object GetCellValue(IRow row, int index)
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