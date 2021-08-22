using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public sealed class DynamicItemCollection : List<DynamicItem>
    {
        DynamicField m_Owner;

        public DynamicItemCollection(DynamicField owner)
        {
            m_Owner = owner;
        }

        public DynamicItemCollection()
        {

        }
    }

}
