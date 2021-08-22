using App.InfoGrid2.Excel_Template.V1;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.LightModels;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace App.InfoGrid2.View.CustomPage
{
    public class T2191_404_Page : ExPage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit()
        {
            ////TabPanel tabPanel = this.FindControl("tabs0") as TabPanel;
            ////tabPanel.ButtonVisible = false;

            ToolBarButton tbarBom = new ToolBarButton("合并导出");

            tbarBom.OnClick = GetMethodJs("GoReport");

            MainToolbar.Items.Add(tbarBom);
        }


        public void GoReportUnfinished()
        {

        }


        /// <summary>
        /// 合并导出
        /// </summary>
        public void GoReport()
        {
            IList datas = MainStore.GetList();

            if (datas.Count == 0)
            {
                MessageBox.Alert("没有数据可以导出哦");
                return;
            }

            LModelList<LModel> lms = datas as LModelList<LModel>;

            try
            {
                ReportModule rm = InitReportFields();

                TreeNode tnData = GetTreeNodeData(lms, rm);

                if (tnData.Items.Count == 0)
                {
                    MessageBox.Alert("导出数据不能为空");
                }
                else
                {
                    ToExcel(rm, tnData, 801);
                }
            }
            catch (Exception ex)
            {
                log.Error("导出出错了", ex);
                MessageBox.Alert("导出出错了");
            }
        }



        /// <summary>
        /// 初始化导出字段
        /// </summary>
        /// <returns></returns>
        public ReportModule InitReportFields()
        {
            try
            {
                ReportModule rm = new ReportModule();

                //项目信息
                rm.Add(801, new string[] { "COL_1", "COL_4", "COL_5", "COL_141", "COL_6", "COL_45", "COL_7", "COL_9", "COL_11" },
                    new string[] { "项目ID", "单号", "制单时间", "出团时间", "制单员", "线路类型", "区域", "团号", "行程" });
                rm[801].SetHideFields(new string[] { "COL_4", "COL_5", "COL_6" });

                //订单信息
                rm.Add(802, new string[] { "COL_56", "COL_10", "COL_87", "COL_107", "COL_14", "COL_15", "COL_16", "COL_17", "COL_18",
                "COL_19", "COL_20", "COL_21", "COL_22", "COL_23", "COL_24", "COL_25", "COL_27", "COL_31" },
                    new string[] { "S开团订单ID", "订单号", "合同号", "客户行程", "客户", "销售员", "数量", "基本团费", "签证费",
                    "小费", "现收签证费", "现收小费", "返佣", "单房差", "其他", "订单金额", "手续费", "未收金额" });
                rm[802].SetHideFields(new string[] { "COL_56", "COL_87", "COL_107" });


                //收款信息
                rm.Add(124, new string[] { "COL_91", "COL_26", "COL_28", "COL_29", "COL_44" },
                    new string[] { "收款记录ID", "已收金额", "收款日期", "收款方式", "备注" });
                rm[124].SetHideFields(new string[] { "COL_91" });

                //客户发票信息
                rm.Add(156, new string[] { "COL_90", "COL_12", "COL_13", "COL_78" },
                    new string[] { "发票记录ID", "发票号", "开票日期", "开票金额" });
                rm[156].SetHideFields(new string[] { "COL_90" });

                //费用信息
                rm.Add(143, new string[] { "COL_92", "COL_32", "COL_33", "COL_34", "COL_100", "COL_135", "COL_30", "COL_35", "COL_36", "COL_89", "COL_39" },
                    new string[] { "费用记录ID", "成本内容", "数量", "单价", "定金", "已付定金", "手续费", "成本小计", "供应商", "未付款", "经办人" });
                rm[143].SetHideFields(new string[] { "COL_92" });

                //付款信息
                rm.Add(126, new string[] { "COL_93", "COL_37", "COL_38", "COL_88", "COL_40" },
                    new string[] { "付款记录ID", "付款日期", "付款方式", "已付款", "备注" });
                rm[126].SetHideFields(new string[] { "COL_93" });

                //利润信息
                rm[801].AddFields(new string[] {  "COL_42", "COL_43", "COL_41" }, new string[] { "销售收入", "成本支出", "利润" }, rm.Last().Value.EndIndex);

                ////合计信息
                //rm[801].AddTotalFields(new string[] { "COL_16", "COL_25", "COL_27", "COL_31", "COL_26", "COL_78", "COL_33", "COL_34", "COL_30", "COL_35", "COL_89", "COL_88" });

                rm[801].TotalField.AddField(801, "COL_45");
                rm[801].TotalField.AddField(801, "COL_7");
                rm[801].TotalField.AddField(801, "COL_9");
                rm[801].TotalField.AddField(801, "COL_11");

                rm[801].TotalField.AddField(802, "COL_16");
                rm[801].TotalField.AddField(802, "COL_25");
                rm[801].TotalField.AddField(802, "COL_27");
                rm[801].TotalField.AddField(802, "COL_31");
                rm[801].TotalField.AddField(124, "COL_26");
                rm[801].TotalField.AddField(156, "COL_78");
                //rm[801].TotalField.AddField(143, "COL_33");
                //rm[801].TotalField.AddField(143, "COL_34");
                //rm[801].TotalField.AddField(143, "COL_30");
                rm[801].TotalField.AddField(143, "COL_35");
                rm[801].TotalField.AddField(143, "COL_89");
                rm[801].TotalField.AddField(126, "COL_88");

                GetTotalCellIndex(rm);

                return rm;
            }
            catch (Exception ex)
            {
                throw new Exception("初始化导出字段出错了", ex);
            }
        }



        /// <summary>
        /// 整理数据
        /// </summary>
        /// <param name="lms"></param>
        /// <param name="rm"></param>
        /// <returns></returns>
        public TreeNode GetTreeNodeData(LModelList<LModel> lms, ReportModule rm)
        {
            try
            {
                TreeNode tn = new TreeNode();

                //做累计
                TotalCellCollection Atotal = rm[801].TotalField;

                foreach (var item in lms)
                {
                    if (item.Get<string>("COL_1") == null)
                    {
                        continue;
                    }

                    //if (item.Get<int>("COL_1") == 342)
                    //{

                    //}
                    //else
                    //{
                    //    continue;
                    //}

                    TreeNodeCollection tnc = tn.GetTreeNodeCollection(801);

                    if (!tnc.TryGetValue(item.Get<int>("COL_1"), out TreeNode tn2))
                    {
                        tn2 = new TreeNode();

                        //SModelMap(tn2.m_Data, item, rm[801].Fields);

                        //SModelMap(tn2.m_Data, item, rm[801].OtherField.Fields);

                        //获取联动的合计
                        //SModelMap(tn2.m_Data, item, rm[801].TotalField.Fields.ToArray());

                        //程序合计
                        SetDefaultSubTotal(tn2.m_Data, rm[801].TotalField.Fields.ToArray());

                        tnc.Add(item.Get<int>("COL_1"), tn2);
                    }

                    if (item.Get<int>("COL_127") == 801)
                    {
                        if (!tnc.TryGetValue(item.Get<int>("COL_1"), out TreeNode tn01))
                        {
                            tn01 = new TreeNode();

                            SModelMap(tn01.m_Data, item, rm[801].Fields);

                            SModelMapProfit(tn2.m_Data, item, rm[801]);

                            //SModelMap(tn01.m_Data, item, rm[801].TotalField.Fields.ToArray());

                            SetDefaultSubTotal(tn01.m_Data, rm[801].TotalField.Fields.ToArray());

                            tnc.Add(item.Get<int>("COL_1"), tn01);
                        }

                        SModelMap(tn01.m_Data, item, rm[801].Fields);

                        SModelMapProfit(tn2.m_Data, item, rm[801]);

                        //SModelMap(tn01.m_Data, item, rm[801].TotalField.Fields.ToArray());
                    }

                    if (item.Get<int>("COL_127") == 802 || item.Get<int>("COL_127") == 124 || item.Get<int>("COL_127") == 156)
                    {
                        if (item.Get<int>("COL_1") == 570 && item.Get<int>("COL_127") == 802)
                        {

                        }

                        tn2.R_RowCount++;

                        TreeNodeCollection tnc1 = tn2.GetTreeNodeCollection(802);

                        if (!tnc1.TryGetValue(item.Get<int>("COL_56"), out TreeNode tn3))
                        {
                            tn3 = new TreeNode();

                            tnc1.Add(item.Get<int>("COL_56"), tn3);
                        }

                        if (item.Get<int>("COL_127") == 802)
                        {
                            SModelMap(tn3.m_Data, item, rm[802].Fields, Atotal[802], tn2.m_Data);
                        }

                        if (item.Get<int>("COL_127") == 124)
                        {
                            tn3.R_RowCount++;

                            TreeNodeCollection tnc2 = tn3.GetTreeNodeCollection(124);

                            TreeNode tn4 = new TreeNode();

                            SModelMap(tn4.m_Data, item, rm[124].Fields, Atotal[124], tn2.m_Data);

                            tnc2.Add(tnc2.Count + 1, tn4);
                        }

                        if (item.Get<int>("COL_127") == 156)
                        {
                            tn3.R_RowCount++;

                            TreeNodeCollection tnc3 = tn3.GetTreeNodeCollection(156);

                            TreeNode tn5 = new TreeNode();

                            SModelMap(tn5.m_Data, item, rm[156].Fields, Atotal[156], tn2.m_Data);

                            tnc3.Add(tnc3.Count + 1, tn5);
                        }
                    }

                    if (item.Get<int>("COL_127") == 143 || item.Get<int>("COL_127") == 126)
                    {
                        tn2.R_RowCount++;

                        TreeNodeCollection tnc4 = tn2.GetTreeNodeCollection(143);

                        if (!tnc4.TryGetValue(item.Get<int>("COL_92"), out TreeNode tn6))
                        {
                            tn6 = new TreeNode();

                            tnc4.Add(item.Get<int>("COL_92"), tn6);
                        }

                        if(item.Get<int>("COL_127") == 143)
                        {
                            SModelMap(tn6.m_Data, item, rm[143].Fields, Atotal[143], tn2.m_Data);
                        }

                        if (item.Get<int>("COL_127") == 126)
                        {
                            tn6.R_RowCount++;

                            TreeNodeCollection tnc5 = tn6.GetTreeNodeCollection(126);

                            TreeNode tn7 = new TreeNode();

                            SModelMap(tn7.m_Data, item, rm[126].Fields, Atotal[126], tn2.m_Data);

                            tnc5.Add(tnc5.Count + 1, tn7);
                        }
                    }
                }

                return tn;
            }
            catch (Exception ex)
            {
                throw new Exception("整理数据出错了", ex);
            }
        }



        /// <summary>
        /// 映射值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="fields"></param>
        /// <param name="t_fields">累计</param>
        /// <param name="data">小合计</param>
        public void SModelMap(SModel target, LModel source, string[] fields, Dictionary<string, TotalCell> t_fields, SModel data)
        {
            foreach (var field in fields)
            {
                target[field] = source[field];

                if (target[field] == null)
                {
                    continue;
                }

                if (t_fields.ContainsKey(field))
                {
                    t_fields[field].Value += target.GetDouble(field);
                    data[field] += target.GetDouble(field);
                }
            }
        }



        /// <summary>
        /// 映射值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="fields"></param>
        public void SModelMap(SModel target, LModel source, string[] fields)
        {
            foreach (var field in fields)
            {
                target[field] = source[field];
            }
        }



        /// <summary>
        /// 合计默认值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="fields"></param>
        private void SetDefaultSubTotal(SModel target, string[] fields)
        {
            foreach (var field in fields)
            {
                target[field] = 0;
            }
        }



        /// <summary>
        /// 为了映射801利润和累计利润
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="rm"></param>
        private void SModelMapProfit(SModel target, LModel source, ReportFieldCollection rfc)
        {
            Dictionary<string, TotalCell> tcells = rfc.OtherTotal[801];

            foreach (var field in rfc.OtherField.Fields)
            {
                target[field] = source[field];

                double val = 0;

                if (target[field] != null)
                {
                    val = target.GetDouble(field);
                }
     
                tcells[field].Value += val;
            }       
        }



        IWorkbook workbook = null;


        /// <summary>
        /// 缓冲所有行
        /// </summary>
        private Dictionary<int, IRow> m_RowCollection = null;

        private ICellStyle m_Cellstyle = null;

        private ICellStyle m_TopCellStyle = null;

        private ICellStyle m_TotalStyle = null;

        private ICellStyle m_SubTotalStyle = null;

        ICell cell = null;

        IRow row = null;

        HSSFFont m_Font = null;


        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="rm"></param>
        /// <param name="tn"></param>
        /// <param name="level"></param>
        private void ToExcel(ReportModule rm, TreeNode tn, int level)
        {
            FileStream fs = null;

            workbook = new HSSFWorkbook();

            m_Font = (HSSFFont)workbook.CreateFont();
            m_Font.FontName = "微软雅黑";//字体
            //font.FontHeightInPoints = 16;//字号
            //font.Color = HSSFColor.PaleBlue.;//颜色
            //font.Underline = NPOI.SS.UserModel.FontUnderlineType.Double;//下划线
            //font.IsStrikeout = true;//删除线
            //font.IsItalic = true;//斜体
            //font.IsBold = true;//加粗

            ISheet sheet = workbook.CreateSheet("sheet1");

            m_RowCollection = new Dictionary<int, IRow>();

            m_Cellstyle = workbook.CreateCellStyle();
            m_Cellstyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            m_Cellstyle.VerticalAlignment = VerticalAlignment.Center;
            m_Cellstyle.BorderBottom = BorderStyle.Thin;
            m_Cellstyle.BorderLeft = BorderStyle.Thin;
            m_Cellstyle.BorderRight = BorderStyle.Thin;
            m_Cellstyle.BorderTop = BorderStyle.Thin;
            m_Cellstyle.SetFont(m_Font);

            m_SubTotalStyle = workbook.CreateCellStyle();
            m_SubTotalStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            m_SubTotalStyle.VerticalAlignment = VerticalAlignment.Center;
            m_SubTotalStyle.SetFont(m_Font);
            m_SubTotalStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            m_SubTotalStyle.FillPattern = FillPattern.SolidForeground;
            m_SubTotalStyle.SetFont(m_Font);

            m_TotalStyle = workbook.CreateCellStyle();
            m_TotalStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            m_TotalStyle.VerticalAlignment = VerticalAlignment.Center;
            m_TotalStyle.SetFont(m_Font);
            m_TotalStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
            m_TotalStyle.FillPattern = FillPattern.SolidForeground;
            m_TotalStyle.SetFont(m_Font);
            
            m_TopCellStyle = workbook.CreateCellStyle();
            m_TopCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            m_TopCellStyle.VerticalAlignment = VerticalAlignment.Center;
            m_TopCellStyle.BorderBottom = BorderStyle.Thin;
            m_TopCellStyle.BorderLeft = BorderStyle.Thin;
            m_TopCellStyle.BorderRight = BorderStyle.Thin;
            m_TopCellStyle.BorderTop = BorderStyle.Thin;
            HSSFFont hf = (HSSFFont)workbook.CreateFont();
            hf.FontName = "微软雅黑";//字体
            hf.FontHeightInPoints = 18;//字号
            m_TopCellStyle.SetFont(hf);

            int r_index = 0;

            try
            {
                r_index = SetTop(sheet, r_index, rm);

                r_index = SetHead(sheet, r_index, rm);

                SetBody(sheet, rm, tn, level, r_index);

                SetTotal(sheet, rm);

                SetAdaptWidth(sheet, rm);

                WebFileInfo wFile = new WebFileInfo("/_Temporary/Excel", FileUtil.NewFielname(".xls"));
                wFile.CreateDir();

                //保存Excel文件在服务器中
                using (FileStream file = new FileStream(wFile.PhysicalPath, FileMode.OpenOrCreate))
                {
                    workbook.Write(file);

                    m_RowCollection.Clear();

                    m_RowCollection = null;

                    Toast.Show("导出成功!");
                }

                DownloadWindow.Show(wFile.Filename, wFile.RelativePath);

            }
            catch (Exception ex)
            {
                log.Error("导出Excel文件出错了！", ex);
                MessageBox.Alert("导出Excel文件出错了！");
            }

        }



        /// <summary>
        /// 写入最左边
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rm"></param>
        /// <param name="tn"></param>
        /// <param name="level"></param>
        /// <param name="r_index"></param>
        public void SetBody(ISheet sheet, ReportModule rm, TreeNode tn, int level, int r_index)
        {
            TreeNodeCollection tnc = tn.Items[level];

            foreach (var node in tnc.Values)
            {
                SModel data = node.m_Data;

                int max_r_count = GetMaxCount(node);

                if (max_r_count == 0)
                {
                    max_r_count = 1;
                }

                int t_r_index = max_r_count + 1; //加一行做合计

                CreateRangRow(sheet, t_r_index);

                row = m_RowCollection[r_index];

                int c_index = 0;

                foreach (var item in rm[level].Values)
                {
                    if (!item.IsShow) { continue; }

                    cell = row.CreateCell(c_index);
                    cell.CellStyle = m_Cellstyle;

                    var value = data[item.Name];

                    if (value == null)
                    {
                        value = string.Empty;
                    }
                    else
                    {
                        value = GetFormatValue(value);
                    }

                    cell.SetCellValue(value);

                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(r_index, r_index + max_r_count - 1, c_index, c_index));

                    c_index++;
                }

                foreach (var item in node.Items)
                {
                    SetDataByLevel(sheet, item.Key, rm, item.Value, r_index);
                }

                c_index = rm[level].OtherField.StartIndex;

                row = m_RowCollection[r_index];

                //利润信息部分
                foreach (var item in rm[level].OtherField.Values)
                {
                    cell = row.CreateCell(c_index);
                    cell.CellStyle = m_Cellstyle;

                    var value = data[item.Name];

                    if (value == null)
                    {
                        value = string.Empty;
                    }
                    else
                    {
                        value = GetFormatValue(value);
                    }

                    cell.SetCellValue(value);

                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(r_index, r_index + max_r_count - 1, c_index, c_index));

                    c_index++;
                }

                SetSubTotal(rm, data);

                r_index += t_r_index;
            }
        }



        /// <summary>
        /// 写入从左边
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="level"></param>
        /// <param name="rm"></param>
        /// <param name="tnc"></param>
        /// <param name="s_r_index"></param>
        public void SetDataByLevel(ISheet sheet, int level, ReportModule rm, TreeNodeCollection tnc, int s_r_index)
        {
            int r_index = s_r_index;

            foreach (var node in tnc.Values)
            {
                int max_r_count = GetMaxCount(node);

                if (max_r_count == 0)
                {
                    max_r_count = 1;
                }

                row = m_RowCollection[r_index];

                SModel data = node.m_Data;

                int c_index = rm[level].StartIndex;

                foreach (var item in rm[level].Values)
                {
                    if (!item.IsShow) { continue; }

                    cell = row.CreateCell(c_index);
                    cell.CellStyle = m_Cellstyle;

                    var value = data[item.Name];

                    if (value == null)
                    {
                        value = string.Empty;
                    }
                    else
                    {
                        value = GetFormatValue(value);
                    }                

                    cell.SetCellValue(value);

                    if (node.Items.Count > 0)
                    {
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(r_index, r_index + max_r_count - 1, c_index, c_index));
                    }

                    c_index++;
                }

                if (node.Items.Count > 0)
                {
                    foreach (var item in node.Items)
                    {
                        SetDataByLevel(sheet, item.Key, rm, item.Value, r_index);
                    }        
                }

                r_index += max_r_count;
            }
        }



        /// <summary>
        /// 写入顶部
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="r_index"></param>
        /// <param name="rm"></param>
        public int SetTop(ISheet sheet, int r_index, ReportModule rm)
        {
            row = sheet.CreateRow(r_index);

            m_RowCollection.Add(r_index, row);

            int c_index = 0;

            Dictionary<int, string> big_title = new Dictionary<int, string>();
            big_title.Add(801, "项目信息");
            big_title.Add(802, "客户订单");
            big_title.Add(124, "收款信息");
            big_title.Add(156, "发票信息");
            big_title.Add(143, "费用明细");
            big_title.Add(126, "付款信息");

            foreach (var item in rm)
            {
                cell = row.CreateCell(item.Value.StartIndex);
                cell.CellStyle = m_TopCellStyle;
                cell.SetCellValue(big_title[item.Key]);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(r_index, r_index, item.Value.StartIndex, item.Value.EndIndex));            
            }
      
            cell = row.CreateCell(rm[801].OtherField.StartIndex);
            cell.CellStyle = m_TopCellStyle;
            cell.SetCellValue("利润信息");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(r_index, r_index, rm[801].OtherField.StartIndex, rm[801].OtherField.EndIndex));

            r_index++;

            return r_index;
        }



        /// <summary>
        /// 写入头部
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="r_index"></param>
        /// <param name="rm"></param>
        /// <returns></returns>
        public int SetHead(ISheet sheet, int r_index, ReportModule rm)
        {
            IRow first_row = sheet.CreateRow(r_index);

            m_RowCollection.Add(r_index, first_row);

            int c_index = 0;

            foreach (var item in rm.Values)
            {
                foreach (var f in item.Values)
                {
                    if (!f.IsShow) { continue; }

                    cell = first_row.CreateCell(c_index);
                    cell.CellStyle = m_Cellstyle;
                    cell.SetCellValue(f.Text);
                    c_index++;
                }
            }

            foreach (var item in rm.Values)
            {
                if (item.OtherField == null)
                {
                    continue;
                }

                foreach (var iof in item.OtherField)
                {
                    cell = first_row.CreateCell(c_index);
                    cell.CellStyle = m_Cellstyle;
                    cell.SetCellValue(iof.Value.Text);
                    c_index++;
                }
            }

            r_index++;

            return r_index;
        }



        /// <summary>
        /// 写入小计
        /// </summary>
        /// <param name="rm"></param>
        public void SetSubTotal(ReportModule rm, SModel data)
        {
            row = m_RowCollection[m_RowCollection.Count - 1];

            row.Height = 30 * 20;

            for (int i = 0; i < rm.Last().Value.EndIndex + 1 + rm[801].OtherField.Count; i++)
            {
                cell = row.CreateCell(i);
                cell.CellStyle = m_SubTotalStyle;
            }

            cell = row.GetCell(8);
            cell.SetCellValue("合计:");

            foreach (var item in rm[801].TotalField)
            {
                foreach (var item2 in item.Value)
                {
                    cell = row.GetCell(item2.Value.CellIndex);

                    var value = data[item2.Key];

                    if (value == null)
                    {
                        value = string.Empty;
                    }
                    else
                    {
                        value = GetFormatValue(value);
                    }

                    cell.SetCellValue(value);
                }
            }



        }



        /// <summary>
        /// 写入合计（累计）
        /// </summary>
        /// <param name="rm"></param>
        private void SetTotal(ISheet sheet, ReportModule rm)
        {
            row = sheet.CreateRow(m_RowCollection.Count);

            row.Height = 35 * 20;

            for (int i = 0; i < rm.Last().Value.EndIndex + 1 + rm[801].OtherField.Count; i++)
            {
                cell = row.CreateCell(i);
                cell.CellStyle = m_TotalStyle;
            }

            cell = row.GetCell(8);
            cell.SetCellValue("累计:");

            foreach (var item in rm[801].TotalField)
            {
                if (item.Key == 801)
                {
                    continue;
                }

                foreach (var item2 in item.Value)
                {
                    cell = row.GetCell(item2.Value.CellIndex);

                    cell.CellStyle = m_TotalStyle;

                    double value = item2.Value.Value;

                    cell.SetCellValue(value);
                }
            }

            foreach (var item in rm[801].OtherTotal[801])
            {
                cell = row.GetCell(item.Value.CellIndex);

                cell.CellStyle = m_TotalStyle;

                double value = item.Value.Value;

                cell.SetCellValue(value);
            }

        }



        /// <summary>
        /// 最大行数
        /// </summary>
        /// <param name="tn"></param>
        /// <returns></returns>
        private int GetMaxCount(TreeNode tn)
        {
            int count = 0;

            foreach (var level2 in tn.Items)
            {
                TreeNodeCollection tnc = level2.Value;

                if(count< tnc.Count)
                {
                    count = tnc.Count;
                }

                int item_count = 0;

                foreach (var level3 in tnc.Values)
                {
                    int level3_count = 0;

                    foreach (var level4 in level3.Items)
                    {
                        if (level3_count < level4.Value.Count)
                        {
                            level3_count = level4.Value.Count;
                        }
                    }

                    if (level3_count == 0)
                    {
                        level3_count = 1;
                    }

                    item_count += level3_count;
                }

                if (count < item_count)
                {
                    count = item_count;
                }

            }

            return count;
        }



        /// <summary>
        /// 创建行
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="r_count"></param>
        public void CreateRangRow(ISheet sheet, int r_count)
        {
            for (int i = 0; i < r_count; i++)
            {
                int r_index = m_RowCollection.Count;

                row = sheet.CreateRow(r_index);

                m_RowCollection.Add(r_index, row);
            }         
        }



        /// <summary>
        /// 格式化值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public object GetFormatValue(object val)
        {
            Type type = val.GetType();

            if (type == typeof(Decimal))
            {
                val = Convert.ToDouble(val);
            }

            if (type == typeof(DateTime))
            {
                val = $"{Convert.ToDateTime(val).Date:yyyy-MM-dd}";
            }

            return val;
        }



        /// <summary>
        /// 获取合计字段的列索引
        /// </summary>
        /// <param name="rm"></param>
        private void GetTotalCellIndex(ReportModule rm)
        {
            foreach (var item in rm[801].TotalField)
            {
                foreach (var item2 in item.Value)
                {
                    ReportFieldCollection rfc = rm[item.Key];

                    TotalCell tcell = item2.Value;

                    tcell.CellIndex = rfc[item2.Key].CellIndex;
                }
            }



        }


        
        /// <summary>
        /// 列宽按内容自适应
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rm"></param>
        private void SetAdaptWidth(ISheet sheet, ReportModule rm)
        {
            for (int columnNum = 0; columnNum < rm.Last().Value.EndIndex + 1 + rm[801].OtherField.Count; columnNum++)
            {
                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;//获取当前列宽度

                for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)//在这一列上循环行
                {
                    IRow currentRow = sheet.GetRow(rowNum);
                    ICell currentCell = currentRow.GetCell(columnNum);

                    if (currentCell == null)
                    {
                        currentCell = currentRow.CreateCell(columnNum);
                        currentCell.CellStyle = m_Cellstyle;
                    }

                    int length = Encoding.UTF8.GetBytes(currentCell.ToString()).Length;//获取当前单元格的内容宽度

                    //if (!isNumberic(currentCell.ToString()))
                    //{
                    //    length = length + 1;
                    //}

                    if (CheckStringChinese(currentCell.ToString()))
                    {
                        length = length / 2 + 1;
                    }

                    if (columnWidth < length)
                    {
                        columnWidth = length + 1;
                    }
                    //若当前单元格内容宽度大于列宽，则调整列宽为当前单元格宽度，后面的+1是我人为的将宽度增加一个字符
                }
                sheet.SetColumnWidth(columnNum, columnWidth * 256);
            }
        }



        /// <summary>
        /// 用 ASCII 码范围判断字符是不是汉字
        /// </summary>
        /// <param name="text">待判断字符或字符串</param>
        /// <returns>真：是汉字；假：不是</returns>
        public bool CheckStringChinese(string text)
        {
            bool res = false;

            foreach (char t in text)
            {
                if ((int)t > 127)
                    res = true;
            }

            return res;
        }

    }




    public class TreeNode
    {
        public TreeNode()
        {
            m_Data = new SModel();
            R_RowCount = 1;
        }

        public SModel m_Data { get; set; }

        public int R_RowCount { get; set; }

        private Dictionary<int, TreeNodeCollection> m_Items;

        public Dictionary<int, TreeNodeCollection> Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new Dictionary<int, TreeNodeCollection>();
                }

                return m_Items;
            }
        }

        public TreeNodeCollection GetTreeNodeCollection(int type)
        {
            if(!Items.TryGetValue(type, out TreeNodeCollection tnc))
            {
                tnc = new TreeNodeCollection();

                this.Items.Add(type, tnc);
            }

            return tnc;
        }

    }


    public class TreeNodeCollection : Dictionary<int, TreeNode>
    {

    }


    public class ReportModule : Dictionary<int, ReportFieldCollection>
    {
        public void Add(int key, string[] fields)
        {
            ReportFieldCollection rfc = new ReportFieldCollection(fields);

            rfc.EndIndex = rfc.Count - 1;

            if (this.Count != 0)
            {
                KeyValuePair<int, ReportFieldCollection> lrfc = this.Last();

                rfc.StartIndex = lrfc.Value.EndIndex + 1;
                rfc.EndIndex = rfc.EndIndex + rfc.StartIndex;
            }

            this.Add(key, rfc);
        }

        public void Add(int key, string[] fields, string[] texts)
        {
            ReportFieldCollection rfc = new ReportFieldCollection(fields, texts);

            //rfc.EndIndex = rfc.Count - 1;

            if (this.Count != 0)
            {
                KeyValuePair<int, ReportFieldCollection> lrfc = this.Last();

                rfc.StartIndex = lrfc.Value.EndIndex + 1;
                //rfc.EndIndex = rfc.EndIndex + rfc.StartIndex;
            }

            rfc.SetCellIndex();

            this.Add(key, rfc);
        }

        /// <summary>
        /// 总字段数量
        /// </summary>
        public int FieldCount
        {
            get
            {
                int count = 0;

                foreach (var item in this)
                {
                    count += item.Value.Count;
                }

                return count;
            }
        }
    }


    public class ReportFieldCollection : Dictionary<string, ReportField>
    {
        /// <summary>
        /// 
        /// </summary>
        public int StartIndex { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public int EndIndex { get; set; } = 0;

        public ReportFieldCollection()
        {

        }

        public ReportFieldCollection(string[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                this.Add(fields[i], new ReportField() { Name = fields[i], Text = fields[i] });
            }
        }

        public ReportFieldCollection(string[] fields, string[] texts)
        {
            if (fields.Length != texts.Length)
            {
                throw new Exception("字段与字段说明数量不一致");
            }

            for (int i = 0; i < fields.Length; i++)
            {
                this.Add(fields[i], new ReportField() { Name = fields[i], Text = texts[i] });
            }
        }

        public string[] Fields { get { return this.Keys.ToArray(); } }


        public ReportFieldCollection OtherField { get; set; }

        public TotalCellCollection m_TotalField { get; set; }

        public TotalCellCollection TotalField
        {
            get
            {
                if (m_TotalField == null)
                {
                    m_TotalField = new TotalCellCollection();
                }
                
                return m_TotalField;
            }
            set {; }
        }


        public TotalCellCollection m_OtherTotal = null;

        public TotalCellCollection OtherTotal
        {
            get
            {
                if (m_OtherTotal == null)
                {
                    m_OtherTotal = new TotalCellCollection();
                }

                return m_OtherTotal;
            }
            set {; }
        }

        public void AddFields(string[] fields, string[] texts, int l_start_index)
        {
            OtherField = new ReportFieldCollection(fields, texts);
            OtherField.StartIndex = l_start_index + 1;
            OtherField.EndIndex = OtherField.StartIndex + fields.Length - 1;

            Dictionary<string, TotalCell> tcells = new Dictionary<string, TotalCell>();

            int j = 0;

            for (int i = OtherField.StartIndex; i <= OtherField.EndIndex; i++)
            {
                tcells.Add(fields[j++], new TotalCell() { CellIndex = i, Value = 0 });
            }

            OtherTotal.Add(801, tcells);
        }

        //public void AddTotalFields(string[] fields)
        //{
        //    TotalField = new ReportFieldCollection();

        //    for (int i = 0; i < fields.Length; i++)
        //    {
        //        TotalField.Add(fields[i], new ReportField() { Name = fields[i], Text = fields[i] });
        //    }
        //}

        public void SetCellIndex()
        {
            //int j = 0;

            //for (int i = StartIndex; i <= EndIndex; i++)
            //{
            //    this[Fields[j]].CellIndex = i;

            //    j++;
            //}

            int i = StartIndex;

            foreach (var item in this.Values)
            {
                if (!item.IsShow)
                {
                    continue;
                }

                item.CellIndex = i;

                i++;
            }

            EndIndex = i - 1;
        }


        public void SetHideFields(string[] fields)
        {
            foreach (var field in fields)
            {
                this[field].IsShow = false;
            }

            SetCellIndex();
        }

    }


    public class ReportField
    {
        public string Name { get; set; }

        public string Text { get; set; }

        public bool IsTotal { get; set; }

        public int CellIndex { get; set; }

        public bool IsShow { get; set; } = true;
    }


    public class TotalCell
    {
        public int CellIndex { get; set; }

        public double Value { get; set; }
    }


    /// <summary>
    /// 合计字段
    /// </summary>
    public class TotalCellCollection: Dictionary<int, Dictionary<string, TotalCell>>
    {
        public List<string> Fields { get; set; } = new List<string>();

        public TotalCellCollection()
        {
        }

        public void AddField(int key, string field)
        {
            if (!this.TryGetValue(key, out Dictionary<string, TotalCell> item))
            {
                item = new Dictionary<string, TotalCell>();

                this.Add(key, item);
            }

            Fields.Add(field);

            item.Add(field, new TotalCell());
        }

    }

}