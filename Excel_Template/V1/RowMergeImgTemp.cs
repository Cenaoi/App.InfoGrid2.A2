using App.InfoGrid2.Excel_Template.Bll;
using HWQ.Entity.LightModels;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace App.InfoGrid2.Excel_Template.V1
{
    /// <summary>
    /// 这是行合并有图片的，就是某一列的值相同行，都要合并在一起
    /// </summary>
    public class RowMergeImgTemp
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
            sp.Header = m_nopi.HandlerHeaderAndFooterTemp(sp.HeaderTemp, ds.Head, sp);

            //处理底部模板
            sp.Footer = m_nopi.HandlerHeaderAndFooterTemp(sp.FooterTemp, ds.Head);


            //处理中间这块
            CreateExcel_UpdateSubData(ds, sp);

            
            //处理数据区中要合并的数据
            CreateExcel_RowMerge(sp);


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

            if(sp.TempParam.Sm == null)
            {

                SModel sm = new SModel
                {
                    //这是要所有列相同才能合并
                    ["merge_columns"] = new int[] { 1, 2 },
                    
                };
            }

            m_nopi.CreateExcel(sp, file_path);

        }

        /// <summary>
        /// 更新模板数据
        /// </summary>
        /// <param name="mf">数据集合</param>
        /// <param name="sp">模板数据</param>
        void CreateExcel_UpdateSubData(DataSet mf, SheetParam sp)
        {

            Dictionary<int, List<LModel>> page_data = m_nopi.CreateExcel_CreatePageData(mf.Items, sp);

            int num = sp.TempParam.LastRowIndex - sp.TempParam.FirstRowIndex + 1;

            int i = 0;

            foreach (var item in page_data)
            {
                //这是处理分页数据的
                foreach (LModel sub_item in item.Value)
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
                            cp.Value = jt.Exec();

                            m_nopi.ReadImg(cp, num, i, sp);

                        }


                        rp.RowIndex = rp.RowIndex + num * i;

                        //这是合并单元格数据
                        RowPro n_rp = m_nopi.GetDataAreaSpanCellAndRow(rpList.SpanCellAndRow, sp.TempParam, i);

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

        /// <summary>
        /// 处理要合并的向同行的列
        /// </summary>
        /// <param name="sp">>整个模板sheet对象</param>
        void CreateExcel_RowMerge(SheetParam sp)
        {

            if(sp.TempParam.Sm == null)
            {
                return;
            }

            //这是要合并的列索引集合
            object[] cols = sp.TempParam.Sm.Get<object[]>("merge_columns");

            //如果没有合并的列下面就不用处理
            if (cols == null || cols?.Length == 0)
            {
                return;
            }
            

            //这里是哪一行要合并，向下合并几行  键是行索引，值是向下合并多少行
            Dictionary<int, int> key_value = new Dictionary<int, int>();

            //上一个关键字
            string last_key = string.Empty;

            //准备还是合并的行索引
            int row_index = 0;

            //这是计算有多少要合并的行，只拿头一个合并的行索引就好
            foreach(var item in sp.DataArea)
            {

                string me_key = string.Empty;

                foreach (var c_index in cols)
                {

                    foreach (var coll in item)
                    {
                        if(coll.ColsIndex == Convert.ToInt32(c_index))
                        {
                            me_key += coll.Value;
                            break;
                        }

                    }
                    
                }

                if(me_key != last_key)
                {
                    last_key = me_key;
                    row_index = item.RowIndex;
                    continue;
                }

                if (!key_value.ContainsKey(row_index))
                {
                    key_value.Add(row_index, 1);
                }
                else
                {
                    key_value[row_index]++;
                }

            }

            //如果没有要合并的就跳出去
            if(key_value.Count == 0)
            {
                return;
            }

            //这是改变或者新增跨行单元格
            foreach (var k_v in key_value)
            {
  
                foreach (int col_index in cols)
                {
                    //看看之前的合并有没有
                    bool flag = false;

                    //这是查看之前原有的跨行跨列单元格是否符合条件，如有符合，就只需要修改，不能新增了
                    foreach (var item in sp.DataArea.SpanCellAndRow)
                    {

                        if (item.SpanFirstRow == k_v.Key && item.SpanFirstCell == col_index)
                        {
                            item.SpanLastRow += k_v.Value;

                            flag = true;

                            break;

                        }
                    }


                    if (flag)
                    {
                        continue;
                    }

                    //这是之前的所有跨行跨列都不符合，所以要新增新的跨行单元格
                    CellPro cp = new CellPro()
                    {
                        SpanFirstCell = col_index,
                        SpanLastCell = col_index,
                        SpanFirstRow = k_v.Key,
                        SpanLastRow = k_v.Key + k_v.Value
                    };

                    sp.DataArea.SpanCellAndRow.Add(cp);

                }

            }

            //准备删除的图片对象
            List<DefPictures> delDps = new List<DefPictures>();
 
            //这是处理图片的了
            foreach(var k_v in key_value)
            {

                foreach(var item in sp.TempSheet.DefPictures)
                {

                    if(item.Row1 == k_v.Key)
                    {
                        item.Row2 += k_v.Value;
                    }
                    else
                    {   
                        //这是准备把原来单元格上的图片给去掉，因为不去掉会覆盖到合并单元格的图片上去
                        if(item.Row2 <= (k_v.Key + k_v.Value) && item.Row1 > k_v.Key)
                        {
                            delDps.Add(item);
                        }
                    }

                }

            }

            

            //这是准备把原来单元格上的图片给去掉，因为不去掉会覆盖到合并单元格的图片上去
            delDps.ForEach((d) =>{ sp.TempSheet.DefPictures.Remove(d); });


        }


    }
}
