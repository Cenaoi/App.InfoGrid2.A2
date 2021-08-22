using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.Steps
{

    /// <summary>
    /// 执行步骤的上下文管理(或当前执行的实例)
    /// </summary>
    public class StepContext
    {

        /// <summary>
        /// 当前节点
        /// </summary>
        StepNode m_CurNode;

        /// <summary>
        /// 根节点
        /// </summary>
        StepNode m_RootNode = new StepNode();

        /// <summary>
        /// 路径
        /// </summary>
        ConcurrentStack<StepNode> m_Path = new ConcurrentStack<StepNode>();

        /// <summary>
        /// 当前节点
        /// </summary>
        public StepNode CurNode
        {
            get { return m_CurNode; }
            set { m_CurNode = value; }
        }

        /// <summary>
        /// 根节点
        /// </summary>
        public StepNode RootNode
        {
            get { return m_RootNode; }
            set { m_RootNode = value; }
        }

        /// <summary>
        /// 步骤的路径
        /// </summary>
        public ConcurrentStack<StepNode> Path
        {
            get { return m_Path; }
        }
    }

}
