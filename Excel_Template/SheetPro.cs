using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using System.Collections;

namespace App.InfoGrid2.Excel_Template
{

    public class RowProCollection : List<RowPro>
    {
        private RowPro m_spanCellAndRow;
        private List<HSSFClientAnchor> m_hcaList;

        private List<HSSFPictureData> m_pic;

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
        /// 自身高度
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// 图片流集合
        /// </summary>
        public List<HSSFPictureData> picturesList
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
        public List<HSSFClientAnchor> hcaList
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
        /// 计算自身高度的
        /// </summary>
        public void ProSize()
        {
            //计算高度

            Height = 0;

            foreach(RowPro rp in this)
            {
                Height += rp.RowHeight;
            }



        }

        
    }


    /// <summary>
    /// 工作表属性
    /// </summary>
    public class SheetPro
    {
        private RowProCollection m_rowProList;

        private Dictionary<int, int> m_ColumnWidth;

        private List<HSSFClientAnchor> m_hcaList;

        private List<HSSFPictureData> m_pic;

        private RowPro m_spanCellAndRow;

        private SheetMargin m_sheetMargin;

        private RowProCollection m_Header;
        private RowProCollection m_DataArea;
        private RowProCollection m_Bottom;
        private PrintPro m_pp;
        private Dictionary<int, RowProCollection> m_PageData;


        /// <summary>
        /// 所有行信息
        /// </summary>
        public RowProCollection rowProList 
        {
            get 
            {
                if (m_rowProList == null) 
                {
                    m_rowProList = new  RowProCollection();
                }
                return m_rowProList;
            }
            set { m_rowProList = value; }
        }



        /// <summary>
        /// 这是头部分
        /// </summary>
        public RowProCollection Header
        {
            get
            {
                if (m_Header == null)
                {
                    m_Header = new RowProCollection();

                }
                return m_Header;
            }

            set { m_Header = value; }
        }

        /// <summary>
        /// 这是数据区
        /// </summary>
        public RowProCollection DataArea
        {
            get
            {
                if (m_DataArea == null)
                {
                    m_DataArea = new RowProCollection();

                }
                return m_DataArea;
            }

            set { m_DataArea = value; }
        }

        /// <summary>
        /// 这是尾部分
        /// </summary>
        public RowProCollection Bottom
        {
            get
            {
                if (m_Bottom == null)
                {
                    m_Bottom = new RowProCollection();

                }
                return m_Bottom;
            }

            set { m_Bottom = value; }
        }



        /// <summary>
        /// 子表数据开始行
        /// </summary>
        public int subFirstRow { get; set; }
        /// <summary>
        /// 子表数据结束行
        /// </summary>
        public int subLastRow { get; set; }
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
                if(m_ColumnWidth == null)
                {
                    m_ColumnWidth = new Dictionary<int, int>();
                }
                return m_ColumnWidth;
            }
            set { m_ColumnWidth = value; }
        }

        /// <summary>
        /// 列数量
        /// </summary>
        public int ColumnNum { get; set; }


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
        /// 图片流集合
        /// </summary>
        public List<HSSFPictureData> picturesList
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
        public List<HSSFClientAnchor> hcaList
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
        /// 属于哪块区域
        /// </summary>
        public SheetTypeName typeName { get; set; }


        /// <summary>
        /// 页面边距
        /// </summary>
        public SheetMargin sheetMargin
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
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 分页数据
        /// </summary>
        public Dictionary<int, RowProCollection> PageData
        {
            get
            {
                if (m_PageData == null) { m_PageData = new Dictionary<int, RowProCollection>(); }
                return m_PageData;
            }
            set { m_PageData = value; }
        }


        /// <summary>
        /// 自定义宽度
        /// </summary>
        public double PageWidth { get; set; }
        ///自定义高度
        public double PageHeight { get; set; }

        /// <summary>
        /// 原始宽
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// 原始高
        /// </summary>
        public double Height { get; set; }


        /// <summary>
        /// 是否合计 false -- 不合计 ，ture -- 合计
        /// </summary>
        public bool IsTotal { get; set; }


    }

    /// <summary>
    /// 整个工作薄区域
    /// </summary>
    public enum SheetTypeName
    {
        /// <summary>
        /// 标题数据
        /// </summary>
        TITLE,
        /// <summary>
        /// 中间数据
        /// </summary>
        DATA,
        /// <summary>
        /// 底部数据
        /// </summary>
        BOTTOM
    }


  


}