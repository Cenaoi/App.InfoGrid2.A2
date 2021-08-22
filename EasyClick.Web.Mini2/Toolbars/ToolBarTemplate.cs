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

        /// <summary>
        /// 获取配置的 json
        /// </summary>
        /// <returns></returns>
        public override string GetConfigJS()
        {
            ScriptTextWriter st = new ScriptTextWriter(QuotationMarkConvertor.SingleQuotes);
            st.RetractBengin("{");
            {
                st.WriteParam("type", "template");
                st.WriteParam("id", this.ID);
                st.WriteParam("class", this.Class);
                st.WriteParam("content", this.Content);
                st.WriteParam("visible", this.Visible, true);

                st.WriteParam("secFunCode", this.SecFunCode);   //权限编码
            }
            st.RetractEnd("}");
            
            return st.ToString();
        }
    }
}
