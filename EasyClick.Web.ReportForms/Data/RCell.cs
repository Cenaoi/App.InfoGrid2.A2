using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace EasyClick.Web.ReportForms.Data
{
    /// <summary>
    /// 单元格
    /// </summary>
    /// 
    [DebuggerDisplay("ID={ID}, Value={Value}, Visible={Visible}, ColSpan={ColSpan}, RowSpan={RowSpan}, IsMergeChild={IsMergeChild}")]
    public class RCell
    {

        bool m_IsNull = true;

        /// <summary>
        /// 合并的列
        /// </summary>
        int m_ColSpan = 1;
        /// <summary>
        /// 合并的行
        /// </summary>
        int m_RowSpan = 1;

        int m_X = 0;
        int m_Y = 0;

        string m_ID;
        object m_Value;

        string m_Type = "decimal";
        /// <summary>
        /// 表达式
        /// </summary>
        string m_Code;

        bool m_Visible = true;

        /// <summary>
        /// 被合并的父单元格坐标
        /// </summary>
        int m_MergeOwnerX = 0;
        /// <summary>
        /// 被合并的父单元格坐标
        /// </summary>
        int m_MergeOwnerY = 0;

        /// <summary>
        /// 被合并的子单元格
        /// </summary>
        bool m_IsMergeChild = false;

        CrossHeadTreeNode m_TreeNode;

        public CrossHeadTreeNode TreeNode
        {
            get { return m_TreeNode; }
            set { m_TreeNode = value; }
        }

        /// <summary>
        /// 空值
        /// </summary>
        /// <returns></returns>
        public bool IsNullValue()
        {
            return m_IsNull;
        }

        public void SetNull(bool isNull)
        {
            m_IsNull = isNull;
        }

        public int X
        {
            get { return m_X; }
            internal set { m_X = value; }
        }

        public int Y
        {
            get { return m_Y; }
            internal set { m_Y = value; }
        }

        internal void SetPosition(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        /// <summary>
        /// 被合并的子单元格
        /// </summary>
        public bool IsMergeChild
        {
            get { return m_IsMergeChild; }
            set { m_IsMergeChild = value; }
        }

        /// <summary>
        /// 被合并的父单元格坐标
        /// </summary>
        public int MergeOwnerX
        {
            get { return m_MergeOwnerX; }
            set { m_MergeOwnerX = value; }
        }

        /// <summary>
        /// 被合并的父单元格坐标
        /// </summary>
        public int MergeOwnerY
        {
            get { return m_MergeOwnerY; }
            set { m_MergeOwnerY = value; }
        }

        /// <summary>
        /// 单元格是否可见
        /// </summary>
        public bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }

        public object Value
        {
            get { return m_Value; }
            set
            {
                m_Value = value;
                m_IsNull = false;
            }
        }

        public RCell()
        {
        }



        public RCell(string id)
        {
            m_ID = id;
            m_IsNull = false;
        }

        public string ID
        {
            get { return m_ID; }
            set
            {
                m_ID = value;
                m_IsNull = false;
            }
        }

        public int ColSpan
        {
            get { return m_ColSpan; }
            set { m_ColSpan = value; }
        }

        public int RowSpan
        {
            get { return m_RowSpan; }
            set { m_RowSpan = value; }
        }


        /// <summary>
        /// 拷贝到
        /// </summary>
        /// <param name="targetCell"></param>
        public void CopyTo(RCell targetCell)
        {
            targetCell.m_Code = m_Code;
            targetCell.m_ColSpan = m_ColSpan;
            targetCell.m_ID = m_ID;
            targetCell.m_IsMergeChild = m_IsMergeChild;
            targetCell.m_IsNull = m_IsNull;
            targetCell.m_MergeOwnerX = m_MergeOwnerX;
            targetCell.m_MergeOwnerY = m_MergeOwnerY;
            targetCell.m_RowSpan = m_RowSpan;
            targetCell.m_TreeNode = m_TreeNode;
            targetCell.m_Type = m_Type;
            targetCell.m_Value = m_Value;
            targetCell.m_Visible = m_Visible;
            targetCell.m_X = m_X;
            targetCell.m_Y = m_Y;


        }
    }

}
