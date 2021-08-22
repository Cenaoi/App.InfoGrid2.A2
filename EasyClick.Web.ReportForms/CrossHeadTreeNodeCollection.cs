using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.ReportForms
{

    public class CrossHeadTreeNodeCollection : List<CrossHeadTreeNode>
    {
        CrossHeadTreeNode m_Owner;

        public CrossHeadTreeNodeCollection(CrossHeadTreeNode owner)
        {
            m_Owner = owner;
        }


        public new void Add(CrossHeadTreeNode item)
        {
            item.m_Owner = m_Owner;
            base.Add(item);
        }
    }
}
