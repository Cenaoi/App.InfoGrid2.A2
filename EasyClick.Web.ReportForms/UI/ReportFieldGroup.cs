using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.ComponentModel;
using System.Security.Permissions;

namespace EasyClick.Web.ReportForms.UI
{
    /// <summary>
    /// 报表字段组
    /// </summary>
    [ParseChildren(true, "Items")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ReportFieldGroup
    {
        List<ReportField> m_Items;

        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public List<ReportField> Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new List<ReportField>();
                }
                return m_Items;
            }
        }
    }
}
