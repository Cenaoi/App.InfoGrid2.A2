using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.V1
{
    /// <summary>
    /// sheet参数
    /// </summary>
    public class SheetParam
    {

        private RowProCollection m_Header;
        private RowProCollection m_DataArea;
        private RowProCollection m_Bottom;

        private RowProCollection m_HeaderTemp;
        private RowProCollection m_DataAreaTemp;
        private RowProCollection m_FooterTemp;

        //合计模板
        private RowProCollection m_TotalTemp;


        private Dictionary<int, RowProCollection> m_PageData;

        private RowProCollection m_Rows;

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
        public RowProCollection Footer
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
        /// 头部模板
        /// </summary>
        public RowProCollection HeaderTemp {

            get
            {
                if (m_HeaderTemp == null)
                {
                    m_HeaderTemp = new RowProCollection();

                }
                return m_HeaderTemp;
            }

            set { m_HeaderTemp = value; }

        }

        /// <summary>
        /// 中间区域模板
        /// </summary>
        public RowProCollection DataAreaTemp {

            get
            {
                if (m_DataAreaTemp == null)
                {
                    m_DataAreaTemp = new RowProCollection();

                }
                return m_DataAreaTemp;
            }

            set { m_DataAreaTemp = value; }
        }

        /// <summary>
        /// 底部模板
        /// </summary>
        public RowProCollection FooterTemp {

            get
            {
                if (m_FooterTemp == null)
                {
                    m_FooterTemp = new RowProCollection();

                }
                return m_FooterTemp;
            }

            set { m_FooterTemp = value; }

        }

        /// <summary>
        /// 合计模板数据
        /// </summary>
        public RowPro TotalTemp
        {
            get;


            set;

        }


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
        /// 模板的第二页参数对象
        /// </summary>
        public TempParam TempParam { get; set; }

        /// <summary>
        /// 模板工作表的属性对象
        /// </summary>
        public TempSheet TempSheet { get; set; }

        /// <summary>
        /// 所有行信息
        /// </summary>
        public RowProCollection Rows
        {
            get
            {
                if (m_Rows == null)
                {
                    m_Rows = new RowProCollection();
                }
                return m_Rows;
            }
            set { m_Rows = value; }
        }


    }
}
