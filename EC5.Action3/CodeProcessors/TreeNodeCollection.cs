using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.CodeProcessors
{
    /// <summary>
    /// 节点集合
    /// </summary>
    public class TreeNodeCollection : List<TreeNode>
    {
        TreeNode m_Owner;

        public TreeNode Owner
        {
            get { return m_Owner; }
        }

        public TreeNodeCollection()
        {

        }

        public TreeNodeCollection(TreeNode owner)
        {
            m_Owner = owner;
        }

        public new void Add(TreeNode item)
        {
            int count = this.Count;
            TreeNode prev = null;

            if(count > 0)
            {
                prev = base[count - 1];

                prev.Next = item;

                item.Prev = prev;
            }

            item.Parent = m_Owner;

            base.Add(item);
        }
    }
}
