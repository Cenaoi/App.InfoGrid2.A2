using App.InfoGrid2.Excel_Template.Bll;
using HWQ.Entity.LightModels;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.V1
{
    /// <summary>
    /// 这个是专门用来处理那种列重复的模板的
    /// </summary>
    public class ColumnRepeatTemp
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

            TempParam tp = sp.TempParam;

            int column_num = tp.Sm.Get<int>("column_num");

            int repeat_num = tp.Sm.Get<int>("repeat_num");

            //处理头部模板
            sp.Header = m_nopi.HandlerHeaderAndFooterTemp(sp.HeaderTemp, ds.Head);

            //处理底部模板
            sp.Footer = m_nopi.HandlerHeaderAndFooterTemp(sp.FooterTemp, ds.Head);

            //正常的分页数据
            Dictionary<int, LModelList> pageData = CreateExcel_CreatePageData(ds.Items, sp);
            //分裂过后的分页数据
            Dictionary<int, List<LModelList>> newPageData = CreateExcel_CreatePageDataByRepeatNum(pageData, repeat_num);
            //计算每一页开始行索引
            CreateExcel_ComputePageRowStartIndex(newPageData, sp);

            foreach(var page_data_item in newPageData)
            {
                CreateExcel_PageData(page_data_item.Key,page_data_item.Value, sp);

            }

            //重新计算 底部 和 中间数据区 的 图片位置和合并单元格位置
            CreateExcel_ComputeImgAndSpanCellPosition(sp);

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
        /// 把整个数据集合弄成一个分页数据的
        /// </summary>
        /// <param name="items">数据集合</param>
        /// <param name="sp">sheet对象</param>
        /// <returns></returns>
        Dictionary<int, LModelList> CreateExcel_CreatePageData(List<LModel> items,SheetParam sp)
        {

            Dictionary<int, LModelList> PageData = new Dictionary<int, LModelList>();

            float numHeight = 0;
            int i = 1;

            LModelList lms = new LModelList();

            try
            {

                PageData.Add(i, lms);


                foreach (LModel item in items)
                {

                    numHeight += sp.DataAreaTemp.Height;

                    if (numHeight > sp.TempSheet.ContentHeight)
                    {
                        i++;

                        lms = new LModelList();

                        PageData.Add(i, lms);

                        numHeight = sp.DataAreaTemp.Height;
                    }

                    lms.Add(item);

                }

                return PageData;

            }catch(Exception ex)
            {
                log.Error("把数据集合分页出错了！", ex);

                throw ex;

            }
        }

        /// <summary>
        /// 根据重复次数再重新创建分页数据
        /// </summary>
        /// <param name="page_data">分页数据</param>
        /// <param name="repeat_num">重复次数</param>
        /// <returns></returns>
        Dictionary<int,List<LModelList>> CreateExcel_CreatePageDataByRepeatNum(Dictionary<int, LModelList> page_data,int repeat_num)
        {


            Dictionary<int, List<LModelList>> pageData = new Dictionary<int, List<LModelList>>();

            int i = 0;

            int j = 1;

            int count = 0;

            //这里计算了每个分裂的实体数量，不够就new一个
            if(page_data.Count > 0)
            {

               count =  page_data[1].Count;


            }


            List<LModelList> datas = new List<LModelList>();

            pageData.Add(j, datas);

            foreach (var item in page_data)
            {
                i++;

                if(i > repeat_num)
                {
                    j++;

                    datas = new List<LModelList>();

                    pageData.Add(j, datas);

                    i = 0;


                }
                datas.Add(item.Value);
            }


            //填充大的分裂数据
            foreach(var key_value in pageData)
            {
                var dic_value = key_value.Value;

                if (dic_value.Count < repeat_num)
                {

                    LModelList lmList = new LModelList();

                    dic_value.Add(lmList);

                }

            }

            //填充小的分裂数据
            foreach(var key_value in pageData)
            {

                var dic_value = key_value.Value;

                foreach(var item in dic_value)
                {

                    var len = count - item.Count;

                    for (var k = 0; k < len; k++)
                    {
                        LModel lm = new LModel("SEC_LOGIN_ACCOUNT");

                        item.Add(lm);
                    }

                }

            }


            return pageData;
        }

        /// <summary>
        /// 创建每页开始行的所有
        /// </summary>
        /// <param name="page_data">已经变成分裂模板分页集合数据了</param>
        /// <param name="sp">整个模板对象</param>
        void CreateExcel_ComputePageRowStartIndex(Dictionary<int, List<LModelList>> page_data,SheetParam sp)
        {

            TempParam tp = sp.TempParam;

            TempSheet ts = sp.TempSheet;

            //页行的数量
            int page_row_num = 0;

            //页行开始索引
            int row_index = tp.FirstRowIndex;

            //数据区行数量
            int row_num = 0;


            foreach (var item in page_data)
            {
                //页行开始索引
                row_index += page_row_num;

                ts.PageRowIndex.Add(item.Key, row_index);

                page_row_num = item.Value[0].Count;

                row_num += page_row_num;

            }

            //这是设置真正的底部行索引
            tp.LastRowIndex += row_num - 1;

        }
        /// <summary>
        /// 处理每一页的数据
        /// </summary>
        /// <param name="page_index">页索引</param>
        /// <param name="page_data">每一页数据</param>
        /// <param name="sp">整个模板sheet对象</param>
        void CreateExcel_PageData(int page_index,List<LModelList> page_data,SheetParam sp)
        {
            TempParam tp = sp.TempParam;

            TempSheet ts = sp.TempSheet;

            int column_num = tp.Sm.Get<int>("column_num");

            //列开始索引
            int column_start_index = 0;
            //列结束索引
            int column_end_index = 0;

            //每页开始行索引
            int row_start_index =  ts.PageRowIndex[page_index];

            //当前行索引
            int cur_row_index = 0;

            int i = 0;



            foreach (LModelList lmList in page_data)
            {
                column_start_index = i * column_num;

                column_end_index = column_start_index + column_num - 1;

                int j = 0;

                foreach (LModel lm in lmList)
                {
                    cur_row_index = row_start_index + j;

                    //等于0时是新建行
                    if (i == 0)
                    {

                        RowProCollection rpc = m_nopi.CopyTemp(sp.DataAreaTemp);

                        foreach (RowPro rp in rpc)
                        {
                            foreach (CellPro cp in rp)
                            {
                                //不在这个范围的列模板不用处理
                                if (cp.ColsIndex < column_start_index || cp.ColsIndex > column_end_index) { continue; }
                                JTemplate jt = new JTemplate();
                                jt.ModelList = null;
                                jt.SrcText = cp.Value;
                                jt.Model = lm;
                                cp.Value = jt.Exec();
                            }

                            rp.RowIndex = row_start_index + j;

                            sp.DataArea.Add(rp);

                        }

                    }else
                    {


                        foreach(RowPro rp in sp.DataArea)
                        {

                            if(rp.RowIndex == cur_row_index)
                            {
                                foreach (CellPro cp in rp)
                                {
                                    //不在这个范围的列模板不用处理
                                    if (cp.ColsIndex < column_start_index || cp.ColsIndex > column_end_index) { continue; }
                                    JTemplate jt = new JTemplate();
                                    jt.ModelList = null;
                                    jt.SrcText = cp.Value;
                                    jt.Model = lm;
                                    cp.Value = jt.Exec();
                                }

                                break;

                            }


                        }

                    }


                    j++;
        
                }

                i++;

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


            #region  中间数据区

            RowPro rp = new RowPro();

            for (int i = 0; i < count; i++)
            {

                foreach(CellPro cp in sp.DataArea.SpanCellAndRow)
                {
                    CellPro cp1 = new CellPro();
                    cp1.SpanFirstRow = cp.SpanFirstRow * i;
                    cp1.SpanLastRow = cp.SpanLastRow * i;
                    cp1.SpanFirstCell = cp.SpanFirstCell;
                    cp1.SpanLastCell = cp.SpanLastCell;
                    rp.Add(cp1);
                }
            }

            //重新赋值的合并单元格信息
            sp.DataArea.SpanCellAndRow = rp;

            #endregion

            #region  底部

            //这是图片的位置变化
            foreach (HSSFClientAnchor hca in sp.Footer.hcaList)
            {
                hca.Row1 += (count - 1) ;
                hca.Row2 += (count - 1);

            }

            //这是合并单元格变化
            foreach (CellPro cp in sp.Footer.SpanCellAndRow)
            {
                cp.SpanFirstRow += (count - 1);
                cp.SpanLastRow += (count - 1) ;
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
