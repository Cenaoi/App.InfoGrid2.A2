using App.InfoGrid2.Excel_Template.Bll;
using HWQ.Entity.LightModels;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.V1
{
    /// <summary>
    /// 处理正常类型的模板类
    /// </summary>
    public class NoneTemp
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
        /// <param name="ds">数据集</param>
        /// <param name="file_path">Excel文件路径</param>
        public void CreateExcel(SheetParam sp, DataSet ds, string file_path)
        {
            m_nopi = new NOPIUtil();

            #region  把数据填充进模板sheet对象中
            //处理头部模板
            sp.Header = m_nopi.HandlerHeaderAndFooterTemp(sp.HeaderTemp, ds.Head);

            //处理底部模板
            sp.Footer = m_nopi.HandlerHeaderAndFooterTemp(sp.FooterTemp, ds.Head);


            //处理中间这块
            CreateExcel_UpdateSubData(ds, sp);

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

            #endregion

            m_nopi.CreateExcel(sp, file_path);

        }

        /// <summary>
        /// 更新模板数据
        /// </summary>
        /// <param name="mf">数据集合</param>
        /// <param name="sp">模板数据</param>
        void CreateExcel_UpdateSubData(DataSet mf, SheetParam sp)
        {

            Dictionary<int, List<LModel>>  page_data =  m_nopi.CreateExcel_CreatePageData(mf.Items, sp);

            int num = sp.TempParam.LastRowIndex - sp.TempParam.FirstRowIndex + 1;

            int i = 0;

            foreach(var item in page_data)
            {
                //这是处理分页数据的
                foreach(LModel sub_item in item.Value)
                {
                    RowProCollection rpList = m_nopi.CopyTemp(sp.DataAreaTemp);

                    foreach (RowPro rp in rpList)
                    {

                        foreach (CellPro cp in rp)
                        {
                            JTemplate jt = new JTemplate();
                            jt.ModelList = mf.Items;
                            jt.SrcText = cp.Value;
                            jt.Model = sub_item;
                            jt.RowIndex = i;
                            cp.Value = jt.Exec();

                            m_nopi.ReadImg(cp, num, i, sp);

                        }

                        //这是合并单元格数据
                        RowPro n_rp =  m_nopi.GetDataAreaSpanCellAndRow(rpList.SpanCellAndRow, sp.TempParam,i);

                        sp.DataArea.Add(rp);

                        sp.DataArea.SpanCellAndRow.AddRange(n_rp);

                        i++;


                    }


                }


                //有小计的才处理小计
                if (sp.TempParam.IsTotal)
                {

                    //这是处理小计的
                    RowPro total_row = sp.TotalTemp.Clone();

                    foreach (CellPro cp in total_row)
                    {
                        if (string.IsNullOrWhiteSpace(cp.Value))
                        {
                            continue;
                        }


                        JTemplate jt = new JTemplate();
                        jt.ModelList = item.Value;
                        jt.SrcText = cp.Value;
                        jt.Model = mf.Head;
                        cp.Value = jt.Exec();

                    }

                    sp.DataArea.Add(total_row);
                }
            }


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

            //#region  中间数据区

            //RowPro rp = new RowPro();

            //for (int i = 0; i < count; i++)
            //{

            //    foreach (CellPro cp in sp.DataAreaTemp.SpanCellAndRow)
            //    {
            //        CellPro cp1 = new CellPro();
            //        cp1.SpanFirstRow = cp.SpanFirstRow + num * i;
            //        cp1.SpanLastRow = cp.SpanLastRow + num * i;
            //        cp1.SpanFirstCell = cp.SpanFirstCell;
            //        cp1.SpanLastCell = cp.SpanLastCell;
            //        rp.Add(cp1);
            //    }
            //}

            ////重新赋值的合并单元格信息
            //sp.DataArea.SpanCellAndRow = rp;

            //#endregion

            #region  底部

            //这是图片的位置变化
            foreach (HSSFClientAnchor hca in sp.Footer.hcaList)
            {
                hca.Row1 += (count - 1) * num;
                hca.Row2 += (count - 1) * num;

            }

            //这是合并单元格变化
            foreach (CellPro cp in sp.Footer.SpanCellAndRow)
            {
                cp.SpanFirstRow += (count - 1) * num;
                cp.SpanLastRow += (count - 1) * num;

                //有合计就再减一行索引
                if (sp.TempParam.IsTotal)
                {
                    cp.SpanFirstRow--;
                    cp.SpanLastRow--;
                }

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
