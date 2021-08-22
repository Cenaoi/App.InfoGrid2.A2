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
    /// 样式规则脚本
    /// </summary>
    [Description("样式规则脚本")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [System.Web.UI.ParseChildren(true, "Script"), System.Web.UI.PersistChildren(false)]
    public class StyleRuleScript:StyleRuleItem
    {
        /// <summary>
        /// 脚本
        /// </summary>
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public string Script { get; set; }



    }


}
