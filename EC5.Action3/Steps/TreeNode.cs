using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.Steps
{

    public class TreeNode
    {
        public string Text { get; set; }

        /// <summary>
        /// 上级节点
        /// </summary>
        public TreeNode Parent { get; internal set; }


        /// <summary>
        /// 第一个子节点
        /// </summary>
        public TreeNode FirstChild
        {
            get
            {
                if (m_Childs != null && m_Childs.Count > 0)
                {
                    return m_Childs[0];
                }

                return null;
            }
        }

        /// <summary>
        /// 上一节点
        /// </summary>
        public TreeNode Prev { get; set; }

        /// <summary>
        /// 下一节点
        /// </summary>
        public TreeNode Next { get; set; }



        /// <summary>
        /// 层次的深度
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// 步骤节点集合
        /// </summary>
        TreeNodeCollection m_Childs;

        /// <summary>
        /// 步骤节点集合
        /// </summary>
        public TreeNodeCollection Childs
        {
            get
            {
                if (m_Childs == null)
                {
                    m_Childs = new TreeNodeCollection(this);
                }
                return m_Childs;
            }
        }

        /// <summary>
        /// 判断是否有子节点
        /// </summary>
        /// <returns></returns>
        public bool HasChild()
        {
            return (m_Childs != null && m_Childs.Count > 0);
        }
    }

}
