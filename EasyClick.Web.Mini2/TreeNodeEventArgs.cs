using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2
{

    public sealed class TreeNodeEventArgs : EventArgs
    {

        private TreeNode m_Node;


        public TreeNodeEventArgs(TreeNode node)
        {
            m_Node = node;
        }


        public TreeNode Node
        {
            get
            {
                return m_Node;
            }
        }
    }
}
