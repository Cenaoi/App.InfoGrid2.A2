using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 字段模板
    /// </summary>
    [DefaultProperty("Value")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(false)]
    [PersistChildren(true)]
    public class FieldTemplate :FieldBase
    {
        /// <summary>
        /// (构造函数)字段模板
        /// </summary>
        public FieldTemplate()
        {

        }
        
        

    }
}
