using App.InfoGrid2.Excel_Template.Bll;
using HWQ.Entity.LightModels;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.V1
{
    /// <summary>
    /// 多个子表分别数据不同的模板
    /// </summary>
    public class MoreSubTableTemp
    {
        /// <summary>
        /// Excel文件助手
        /// </summary>
        public NOPIUtil m_nopi;


        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 根据实体数据 和模板对象来创建Excel文件
        /// </summary>
        /// <param name="sp">>整个模板sheet对象</param>
        /// <param name="ds">多子表数据集</param>
        /// <param name="file_path">Excel文件路径</param>
        public void CreateExcel(SheetParam sp, MoreSubTableDataSet ds, string file_path)
        {

            m_nopi = new NOPIUtil();


            //处理头部模板
            sp.Header = m_nopi.HandlerHeaderAndFooterTemp(sp.HeaderTemp, ds.Head);

            //处理底部模板
            sp.Footer = m_nopi.HandlerHeaderAndFooterTemp(sp.FooterTemp, ds.Head);

            //填充数据区
            CreateExcel_FillDataArea(sp, ds);

            //重新计算 底部 和 中间数据区 的 图片位置和合并单元格位置
            CreateExcel_ComputeImgAndSpanCellPosition(sp);

            //中间数据区结束行索引
            sp.TempParam.LastRowIndex = sp.TempParam.FirstRowIndex + sp.DataArea.Count - 1;

            sp.Rows.Clear();
            //把三块的数据和再一起
            sp.Rows.AddRange(sp.Header);
            sp.Rows.AddRange(sp.DataArea);
            sp.Rows.AddRange(sp.Footer);

            //动态计算行高度
            m_nopi.GetDynHegiht(sp);


            m_nopi.CreateExcel(sp, file_path);


        }
        /// <summary>
        /// 填充数据区
        /// </summary>
        /// <param name="sp">整个模板sheet对象</param>
        /// <param name="ds">多子表数据集</param>
        void CreateExcel_FillDataArea(SheetParam sp, MoreSubTableDataSet ds)
        {

            TempParam tp = sp.TempParam;

            SModelList sub_table_param = tp.Sm.Get<SModelList>("sub_table_param");


            //模板数量，用来计算合并单元格的行索引的
            int temp_num = 0;


            foreach (SModel sm in sub_table_param)
            {
                string table_name = sm.Get<string>("table_name");

                LModelList<LModel> lms = GetLModesByTableName(ds, table_name);

                RowProCollection title_temp = GetTitleTempByTable(sp, sm);

                RowProCollection dataArea_temp = new RowProCollection();


                int i = 0;

                foreach (LModel lm in lms)
                {
                    RowProCollection rpList = GetDataAreaTempByTable(sp, sm);

                    foreach (RowPro rp in rpList)
                    {
                        foreach (CellPro cp in rp)
                        {
                            JTemplate jt = new JTemplate();
                            jt.ModelList = null;
                            jt.SrcText = cp.Value;
                            jt.Model = lm;
                            cp.Value = jt.Exec();
                        }

                        dataArea_temp.Add(rp);

                    }

                    foreach(var item in rpList.SpanCellAndRow)
                    {

                        CellPro cp1 = new CellPro();
                        cp1.SpanFirstRow = item.SpanFirstRow + rpList.Count * i + sp.DataArea.Count - temp_num;
                        cp1.SpanLastRow = item.SpanLastRow + rpList.Count * i + sp.DataArea.Count - temp_num;
                        cp1.SpanFirstCell = item.SpanFirstCell;
                        cp1.SpanLastCell = item.SpanLastCell;
                        sp.DataArea.SpanCellAndRow.Add(cp1);

                    }

                    i++;
                    
                }

                RowProCollection rpList_1 = GetDataAreaTempByTable(sp, sm);

                sp.DataArea.AddRange(title_temp);

                sp.DataArea.AddRange(dataArea_temp);

                temp_num += rpList_1.Count;
            }

        }


        /// <summary>
        /// 根据表名获取数据集合
        /// </summary>
        /// <param name="ds">多子表数据集合</param>
        /// <param name="table_name">表名</param>
        /// <returns></returns>
        LModelList<LModel> GetLModesByTableName(MoreSubTableDataSet ds,string table_name)
        {
            if (ds.Items.ContainsKey(table_name))
            {
                return ds.Items[table_name];
            }

            return null;
        }

        /// <summary>
        /// 获取每个表中标题部分
        /// </summary>
        /// <param name="sp">整个模板sheet对象</param>
        /// <param name="sm">表自身的指定json对象</param>
        /// <returns></returns>
        RowProCollection GetTitleTempByTable(SheetParam sp,SModel sm)
        {

            RowProCollection rpc = new RowProCollection();

            int beg_index = sm.Get<int>("beg_index") - 1;
            //数据区参数对象
            SModel dataArea = sm.Get<SModel>("dataArea");

            int da_beg_index = dataArea.Get<int>("beg_index") - 1;

            foreach(RowPro rp in sp.DataAreaTemp)
            {

                if(rp.RowIndex >= beg_index && rp.RowIndex < da_beg_index)
                {
                    rpc.Add(rp.Clone());
                }
               
            }

            return rpc; 
        }

        /// <summary>
        /// 获取每个表中的数据区部分
        /// </summary>
        /// <param name="sp">整个模板sheet对象</param>
        /// <param name="sm">表自身的指定json对象</param>
        /// <returns></returns>
        RowProCollection GetDataAreaTempByTable(SheetParam sp, SModel sm)
        {

            RowProCollection rpc = new RowProCollection();

            //数据区参数对象
            SModel dataArea = sm.Get<SModel>("dataArea");

            int beg_index = dataArea.Get<int>("beg_index") - 1;

            int end_index = dataArea.Get<int>("end_index") - 1;

            foreach (RowPro rp in sp.DataAreaTemp)
            {

                if (rp.RowIndex >= beg_index && rp.RowIndex <= end_index)
                {
                    rpc.Add(rp.Clone());
                
                }
            }


            foreach(var item in sp.DataAreaTemp.SpanCellAndRow)
            {

                if(item.SpanFirstRow >= beg_index && item.SpanLastRow <= end_index)
                {
                    rpc.SpanCellAndRow.Add(item.Clone());
                }

            }


            return rpc;

        }

        /// <summary>
        /// 重新计算  底部 和 中间数据区 的 图片位置和合并单元格位置  
        /// </summary>
        /// <param name="sp">整个模板sheet对象</param>
        void CreateExcel_ComputeImgAndSpanCellPosition(SheetParam sp)
        {
            //中间数据区行数量
            int count = sp.DataArea.Count;

            TempSheet ts = sp.TempSheet;

            TempParam tp = sp.TempParam;


            int num = tp.LastRowIndex - tp.FirstRowIndex + 1;


            #region  底部

            //这是图片的位置变化
            foreach (HSSFClientAnchor hca in sp.Footer.hcaList)
            {
                hca.Row1 += count - sp.DataAreaTemp.Count;
                hca.Row2 += count - sp.DataAreaTemp.Count;

            }

            //这是合并单元格变化
            foreach (CellPro cp in sp.Footer.SpanCellAndRow)
            {
                cp.SpanFirstRow += count - sp.DataAreaTemp.Count;
                cp.SpanLastRow += count - sp.DataAreaTemp.Count;
            }


            #endregion


            #region 统一放在一起  图片

            ts.Pictures.Clear();

            ts.Pictures.AddRange(sp.Header.picturesList);

            ts.Pictures.AddRange(sp.DataArea.picturesList);

            ts.Pictures.AddRange(sp.Footer.picturesList);

            ts.Hcas.Clear();

            ts.Hcas.AddRange(sp.Header.hcaList);
            ts.Hcas.AddRange(sp.DataArea.hcaList);
            ts.Hcas.AddRange(sp.Footer.hcaList);

            #endregion


            #region 合并单元格统一放一起

            ts.SpanCellAndRow.Clear();

            ts.SpanCellAndRow.AddRange(sp.Header.SpanCellAndRow);

            ts.SpanCellAndRow.AddRange(sp.DataArea.SpanCellAndRow);

            ts.SpanCellAndRow.AddRange(sp.Footer.SpanCellAndRow);

            #endregion


        }




    }
}
