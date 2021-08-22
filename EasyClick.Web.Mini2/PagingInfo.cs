using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class PagingInfo
    {

        int m_MaxRows;

        int m_StartRowIndex;

        int m_RowTotal;

        /// <summary>
        /// 分页的信息
        /// </summary>
        public PagingInfo()
        {

        }

        /// <summary>
        /// 分页的信息
        /// </summary>
        /// <param name="maxRows">每页显示的记录数量</param>
        /// <param name="startRowIndex">开始行索引位置</param>
        /// <param name="rowTotal">总的记录数</param>
        public PagingInfo(int maxRows, int startRowIndex, int rowTotal)
        {
            m_MaxRows = maxRows;
            m_StartRowIndex = startRowIndex;
            m_RowTotal = rowTotal;
        }

        /// <summary>
        /// 每页显示的记录数量
        /// </summary>
        public int MaxRows
        {
            get { return m_MaxRows; }
            set { m_MaxRows = value; }
        }

        /// <summary>
        /// 总的记录数
        /// </summary>
        public int RowTotal
        {
            get { return m_RowTotal; }
            set { m_RowTotal = value; }
        }

        /// <summary>
        /// 开始行索引位置
        /// </summary>
        public int StartRowIndex
        {
            get { return m_StartRowIndex; }
            set { m_StartRowIndex = value; }
        }
    }
}
