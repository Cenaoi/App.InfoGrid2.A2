using HWQ.Entity.Decipher.LightDecipher;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HWQ.Entity.LightModels;

namespace EC5.Action3.CodeProcessors
{
    /// <summary>
    /// 编码上下文
    /// </summary>
    public class CodeContext
    {
        /// <summary>
        /// (构造函数)编码上下文
        /// </summary>
        public CodeContext()
        {

        }

        /// <summary>
        /// 主要的数据连接对象
        /// </summary>
        public DbDecipher Decipher { get; set; }


        public DrawingLibrary Library { get; set; }
        


        /// <summary>
        /// 当前节点
        /// </summary>
        TreeNode m_CurNode;

        /// <summary>
        /// 根节点
        /// </summary>
        TreeNode m_RootNode = new TreeNode();

        /// <summary>
        /// 路径
        /// </summary>
        ConcurrentStack<TreeNode> m_Path = new ConcurrentStack<TreeNode>();

        /// <summary>
        /// 当前节点
        /// </summary>
        public TreeNode CurNode
        {
            get { return m_CurNode; }
            set { m_CurNode = value; }
        }

        /// <summary>
        /// 当前子项目
        /// </summary>
        public ActionItemBase CurSubItem { get; internal set; }



        /// <summary>
        /// 根节点
        /// </summary>
        public TreeNode RootNode
        {
            get { return m_RootNode; }
            set { m_RootNode = value; }
        }

        /// <summary>
        /// 步骤的路径
        /// </summary>
        public ConcurrentStack<TreeNode> Path
        {
            get { return m_Path; }
        }


        /// <summary>
        /// 当前的扩展参数
        /// </summary>
        SModel m_CurParams = null;

        /// <summary>
        /// 当前的扩展参数
        /// </summary>
        public SModel CurParams
        {
            get
            {
                return m_CurParams;
            }
            set { m_CurParams = value; }
        }
    }
}
