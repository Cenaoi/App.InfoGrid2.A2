using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.V1
{
    /// <summary>
    /// 行集合
    /// </summary>
    public class RowProCollection : List<RowPro>
    {
        private RowPro m_spanCellAndRow;
        private List<HSSFClientAnchor> m_hcaList;

        private List<HSSFPictureData> m_pic;

        private List<DefPictures> m_DefPictures;

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
        /// 这是读取出来的图片集合
        /// </summary>
        public List<DefPictures> DefPictures
        {

            get
            {
                if(m_DefPictures == null)
                {
                    m_DefPictures = new List<V1.DefPictures>();
                }

                return m_DefPictures;

            }

            set { m_DefPictures = value; }

        }


        /// <summary>
        /// 计算自身高度的
        /// </summary>
        public void ProSize()
        {
            //计算高度

            Height = 0;

            foreach (RowPro rp in this)
            {
                Height += rp.RowHeight;
            }



        }


    }

}
