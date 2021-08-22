using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public partial class RapidViewForm : Control, IAttributeAccessor
    {
        Template m_Template;

        BoundFieldCollection m_Fields;

        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public BoundFieldCollection Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new BoundFieldCollection(this);
                }
                return m_Fields;
            }
        }





    }
}
