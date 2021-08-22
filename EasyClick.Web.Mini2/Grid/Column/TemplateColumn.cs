using EasyClick.Web.Mini;
using EC5.Utility;
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
    /// 模板列
    /// </summary>
    [Description("模板列")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [System.Web.UI.ParseChildren(true, "TemplateControl"), System.Web.UI.PersistChildren(false)]
    public class TemplateColumn:BoundField
    {
        /// <summary>
        /// (构造函数) 模板列
        /// </summary>
        public TemplateColumn()
        {
            this.MiType = "template";
        }

        string m_TemplateControl;

        /// <summary>
        /// 模板引擎名称
        /// </summary>
        string m_EngineName;

        /// <summary>
        /// 模板内容
        /// </summary>
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public string TemplateControl
        {
            get { return m_TemplateControl; }
            set { m_TemplateControl = value; }
        }

        /// <summary>
        /// 引擎名称
        /// </summary>
        public string EngineName
        {
            get { return m_EngineName; }
            set { m_EngineName = value; }
        }

        /// <summary>
        /// 创建 Html 模板
        /// </summary>
        /// <param name="cellType"></param>
        /// <param name="rowState"></param>
        /// <returns></returns>
        public override string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            //Mini2.ui.grid.column.Template

            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            sb.AppendFormat("role:'{0}', ", this.MiType);
            sb.AppendFormat("miType:'{0}', ", this.MiType);

            sb.AppendFormat("dataIndex:'{0}', ", this.DataField);
            sb.AppendFormat("headerText:'{0}', ", this.HeaderText);

            sb.AppendFormat("engineName: '{0}', ", this.EngineName);

            sb.AppendFormat("content: '{0}', ", JsonUtil.ToJson( this.TemplateControl, JsonQuotationMark.SingleQuotes) );

            if (this.Required)
            {
                sb.Append("required: true,");
            }

            if (!string.IsNullOrEmpty(this.Tag))
            {
               // sb.AppendFormat("tag:'{0}',", MiniHelper.GetItemJson(this.Tag));
            }

            if (!this.Sortable)
            {
                sb.AppendFormat("sortable:{0}, ", this.Sortable.ToString().ToLower());
            }

            if (this.SortMode != Mini.SortMode.Default)
            {
                sb.AppendFormat("sortable:'{0}', ", this.SortMode.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(this.SortExpression))
            {
                sb.AppendFormat("sortExpression:'{0}', ", this.SortExpression);
            }


            if (!this.Resizable)
            {
                sb.AppendFormat("resizable:{0}, ", this.Resizable.ToString().ToLower());
            }

            if (this.ItemAlign != Mini.CellAlign.Left)
            {
                sb.AppendFormat("align:'{0}', ", this.ItemAlign.ToString().ToLower());
            }

            if (this.HeaderAlign != Mini.CellAlign.Left)
            {
                sb.AppendFormat("headerAlign:'{0}', ", this.HeaderAlign.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(this.Click))
            {
                sb.AppendFormat("click: {0}, ", this.Click);
            }

            if (!string.IsNullOrEmpty(this.SummaryType))
            {
                sb.AppendFormat("summaryType: '{0}',", this.SummaryType);
            }

            if (!string.IsNullOrEmpty(this.SummaryFormat))
            {
                sb.AppendFormat("summaryFormat: '{0}',", this.SummaryFormat);
            }

            if (!string.IsNullOrEmpty(this.Renderer))
            {
                sb.AppendFormat("renderer: {0},", this.Renderer);
            }

            sb.Append(CreateEditor());



            sb.AppendFormat("width:{0}", this.Width);
            sb.Append("}");

            return sb.ToString();
        }


    }
}
