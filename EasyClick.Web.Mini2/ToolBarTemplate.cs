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
    /// 工具栏-模板栏目
    /// </summary>
    [Description("工具栏-模板栏目")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Content"), PersistChildren(false)]
    public class ToolBarTemplate : ToolBarItem
    {
        /// <summary>
        /// (构造函数)工具栏-模板栏目
        /// </summary>
        public ToolBarTemplate()
        {

        }

        /// <summary>
        /// 内容
        /// </summary>
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public string Content { get; set; }


        public override string GetConfigJS()
        {

            StringBuilder sb = new StringBuilder("{ type:'template'");

            if (string.IsNullOrWhiteSpace(this.ID))
            {
                sb.Append($", id: '{this.ID}'");
            }

            string contentJson = EC5.Utility.JsonUtil.ToJson(this.Content, EC5.Utility.JsonQuotationMark.SingleQuotes);
            
            sb.Append(", content: '").Append(contentJson).Append("'");

            if (!this.Visible)
            {
                sb.Append(", visible:false");
            }

            sb.Append("}");

            return sb.ToString();
        }
    }
}
