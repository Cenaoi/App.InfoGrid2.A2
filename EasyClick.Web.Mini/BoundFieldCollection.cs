using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{

    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public sealed class BoundFieldCollection : List<BoundField>
    {
        System.Web.UI.Control m_Owner;

        public BoundFieldCollection(System.Web.UI.Control owner)
        {
            m_Owner = owner;
        }

        public System.Web.UI.Control Owner
        {
            get { return m_Owner; }
            internal set { m_Owner = value; }
        }

        public new void Add(BoundField item)
        {
            item.m_Owner = m_Owner;

            base.Add(item);
        }
    }
}
