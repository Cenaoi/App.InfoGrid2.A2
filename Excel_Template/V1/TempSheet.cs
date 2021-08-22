using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.V1
{
    /// <summary>
    /// 模板sheet里面的设定
    /// </summary>
    public class TempSheet
    {
        private SheetMargin m_sheetMargin;

        private RowPro m_spanCellAndRow;

        private Dictionary<int, int> m_ColumnWidth;

        private Dictionary<int, int> m_ColumnWidthPx;

        private Dictionary<int, int> m_PageRowIndex;

        private PrintPro m_pp;

        private List<HSSFClientAnchor> m_hcaList;

        private List<HSSFPictureData> m_pic;

        private List<DefPictures> m_DefPictures;


        /// <summary>
        /// 打印页面边距
        /// </summary>
        public SheetMargin SheetMargin
        {
            get
            {
                if (m_sheetMargin == null)
                {

                    m_sheetMargin = new SheetMargin();
                }
                return m_sheetMargin;
            }
            set { m_sheetMargin = value; }

        }


        /// <summary>
        /// 总页数 打印用的
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 合并单元格信息
        /// </summary>
        public RowPro SpanCellAndRow
        {
            get
            {
                if (m_spanCellAndRow == null) { m_spanCellAndRow = new RowPro(); }
                return m_spanCellAndRow;
            }
            set { m_spanCellAndRow = value; }
        }


        /// <summary>
        /// 整列的宽度
        /// </summary>
        public Dictionary<int, int> ColumnWidth
        {
            get
            {
                if (m_ColumnWidth == null)
                {
                    m_ColumnWidth = new Dictionary<int, int>();
                }
                return m_ColumnWidth;
            }
            set { m_ColumnWidth = value; }
        }

        /// <summary>
        /// 这是整列的宽度 像素单位
        /// </summary>
        public Dictionary<int, int> ColumnWidthPx
        {
            get
            {
                if(m_ColumnWidthPx == null)
                {
                    m_ColumnWidthPx = new Dictionary<int, int>();
                }

                return m_ColumnWidthPx;

            }
            set { m_ColumnWidthPx = value; }
        }


        /// <summary>
        /// 打印属性
        /// </summary>
        public PrintPro pp
        {
            get
            {
                if (m_pp == null) { m_pp = new PrintPro(); }
                return m_pp;
            }
            set { m_pp = value; }
        }

        /// <summary>
        /// 列数量
        /// </summary>
        public int ColumnNum { get; set; }

        /// <summary>
        /// 图片流集合
        /// </summary>
        public List<HSSFPictureData> Pictures
        {
            get
            {
                if (m_pic == null) { m_pic = new List<HSSFPictureData>(); }

                return m_pic;
            }

            set { m_pic = value; }
        }


        /// <summary>
        /// 这是图片绝对坐标
        /// </summary>
        public List<HSSFClientAnchor> Hcas
        {
            get
            {
                if (m_hcaList == null)
                {
                    m_hcaList = new List<HSSFClientAnchor>();

                }
                return m_hcaList;
            }
            set
            {
                m_hcaList = value;
            }
        }


        /// <summary>
        /// 这是读取出来的图片集合
        /// </summary>
        public List<DefPictures> DefPictures
        {

            get
            {
                if (m_DefPictures == null)
                {
                    m_DefPictures = new List<V1.DefPictures>();
                }

                return m_DefPictures;

            }

            set { m_DefPictures = value; }

        }


        /// <summary>
        /// 头部高度 加外边距的
        /// </summary>
        public float HeaderHeight { get; set; }

        /// <summary>
        /// 底部高度  加外边距的
        /// </summary>
        public float FooterHeight { get; set; }

        /// <summary>
        /// 中间内容高度
        /// </summary>
        public float ContentHeight { get; set; }

        /// <summary>
        /// 打印纸张大小
        /// </summary>
        public PaperSize PaperSize { get; set; }

        /// <summary>
        /// 每页的的开始行索引
        /// </summary>
        public Dictionary<int, int> PageRowIndex
        {


            get
            {

                if (m_PageRowIndex == null)
                {

                    m_PageRowIndex = new Dictionary<int, int>();
                }

                return m_PageRowIndex;
            }

            set
            {
                m_PageRowIndex = value;
            }

        }
        

    }
}
