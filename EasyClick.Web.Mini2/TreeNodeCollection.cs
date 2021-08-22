using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// 节点集合
    /// </summary>
    public class TreeNodeCollection : IEnumerable<TreeNode>, ICollection<TreeNode>
    {

        List<TreeNode> m_Items = new List<TreeNode>();

        TreeNode m_Owner;

        public TreeNode this[int index]
        {
            get
            {
                return m_Items[index];
            }
        }

        public TreeNodeCollection(TreeNode owner)
        {
            m_Owner = owner;
        }

        public TreeNode Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }

        //public void Add(TreeNode treeNode)
        //{
        //    throw new NotImplementedException();
        //}

        public IEnumerator<TreeNode> GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }


        public void Add(TreeNode item)
        {
            //item.Owner.m_NodeIDIdentity++;

            item.Owner = m_Owner.Owner;
            item.Parent = m_Owner;

            //item.Identity = item.Owner.m_NodeIDIdentity;

            m_Items.Add(item);
        }

        public void Clear()
        {
            m_Items.Clear();
        }

        public bool Contains(TreeNode item)
        {
            return m_Items.Contains(item);
        }

        public void CopyTo(TreeNode[] array, int arrayIndex)
        {
            m_Items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(TreeNode item)
        {
            return m_Items.Remove(item);
        }
    }
}
