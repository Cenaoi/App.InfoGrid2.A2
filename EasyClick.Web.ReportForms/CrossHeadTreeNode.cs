using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.ReportForms
{
    public enum CrossHeadTreeNodeTypes
    {
        /// <summary>
        /// 值
        /// </summary>
        Value,
        /// <summary>
        /// 统计
        /// </summary>
        Total
    }

    /// <summary>
    /// 标题节点树
    /// </summary>
    public class CrossHeadTreeNode
    {
        ReportColumn m_Column;
        CrossHeadTreeNodeCollection m_Childs;

        internal CrossHeadTreeNode m_Owner;

        #region 统计类型的节点数据

        CrossHeadTreeNodeTypes m_NodeType = CrossHeadTreeNodeTypes.Value;

        /// <summary>
        /// 需要统计数据的节点
        /// </summary>
        List<CrossHeadTreeNode> m_TotalNodes;
        

        #endregion

        /// <summary>
        /// 限制一个子节点
        /// </summary>
        public bool OneChild { get; set; }

        ///// <summary>
        ///// 单元格的索引
        ///// </summary>
        //int m_CellIndex = 0;

        /// <summary>
        /// 单元格的索引
        /// </summary>
        int m_X = -1;


        ///// <summary>
        ///// 单元格的索引
        ///// </summary>
        //public int CellIndex
        //{
        //    get { return m_CellIndex; }
        //    set { m_CellIndex = value; }
        //}
        
        /// <summary>
        /// 指定需要统计的节点
        /// </summary>
        public List<CrossHeadTreeNode> TotalNodes
        {
            get
            {
                if(m_TotalNodes == null)
                {
                    m_TotalNodes = new List<CrossHeadTreeNode>();
                }
                return m_TotalNodes;
            }
            set { m_TotalNodes = value; }
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        public CrossHeadTreeNodeTypes NodeType
        {
            get { return m_NodeType; }
            set { m_NodeType = value; }
        }
        
        /// <summary>
        /// 单元格的索引
        /// </summary>
        public int X
        {
            get { return m_X; }
            set { m_X = value; }
        }


        public ReportColumn Column
        {
            get
            {
                return m_Column;
            }
            set
            {
                m_Column = value;
            }
        }

        /// <summary>
        /// 深度
        /// </summary>
        public int GetDepth()
        {
            int depth = 0;

            CrossHeadTreeNode owner = m_Owner;

            for (int i = 0; i < 99; i++)
            {

                if (owner == null)
                {
                    depth = i;
                    break;
                }

                owner = owner.m_Owner;
                
            }

            return depth;
        }

        public CrossHeadTreeNodeCollection Childs
        {
            get
            {
                if (m_Childs == null)
                {
                    m_Childs = new CrossHeadTreeNodeCollection(this);
                }
                return m_Childs;
            }
        }

        /// <summary>
        /// 是否有子节点
        /// </summary>
        /// <returns></returns>
        public bool HasChild()
        {
            return (m_Childs != null && m_Childs.Count > 0);
        }

        public int GetSpan()
        {
            if (!HasChild())
            {
                return 1;
            }

            int n = m_Childs.Count;

            foreach (CrossHeadTreeNode node in m_Childs)
            {
                n = Math.Max(n, node.GetSpan());
            }

            return n;
        }

        /// <summary>
        /// 所属节点
        /// </summary>
        public CrossHeadTreeNode Owner
        {
            get { return m_Owner; }
        }

    }

}
