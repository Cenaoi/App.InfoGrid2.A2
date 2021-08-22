using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 时间选择器
    /// </summary>
    public class TimePicker :TextBox
    {
        /// <summary>
        /// 时间选择器(构造函数)
        /// </summary>
        public TimePicker()
        {
            this.InReady = "Mini2.ui.form.field.Time";

            this.JsNamespace = "Mini2.ui.form.field.Time";
        }

        string m_Format = "H:m";
        

        /// <summary>
        /// 格式化时间
        /// </summary>
        [Description("格式化时间")]
        [DefaultValue("Y-m-d")]
        public string Format
        {
            get { return m_Format; }
            set { m_Format = value; }
        }
        

        /// <summary>
        /// 获取时间
        /// </summary>
        public TimeSpan? Time
        {
            get
            {
                string value = this.Value;
                
                return null;
            }
        }


        protected override void FullScript(StringBuilder sb)
        {
            string clientId = this.ClientID;

            Labelable lab = this.Labelable;

            sb.AppendLine("var field = Mini2.create('" + this.JsNamespace + "', {");
            JsParam(sb, "id", this.ID);
            JsParam(sb, "clientId", clientId);
            JsParam(sb, "name", StringUtil.NoBlank(this.Name, clientId));

            if (this.SubItemMode)
            {
                JsParam(sb, "subItemMode", this.SubItemMode, false);

                sb.AppendLine("    parentEl: e.itemEl ,");
            }

            JsParam(sb, "dataField", this.DataField);
            JsParam(sb, "width", this.Width, "100%");
            JsParam(sb, "minWidth", this.MinWidth);
            JsParam(sb, "maxWidth", this.MaxWidth);

            //JsParam(sb, "format", this.Format, "Y-m-d");


            JsParam(sb, "fieldLabel", JsonUtil.ToJson(lab.FieldLabel, JsonQuotationMark.SingleQuotes));
            JsParam(sb, "labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
            JsParam(sb, "hideLabel", lab.HideLabel, false);
            JsParam(sb, "labelWidth", lab.LabelWidth, 100);
            JsParam(sb, "required", this.Required, false);

            JsParam(sb, "placeholder", JsonUtil.ToJson(this.Placeholder, JsonQuotationMark.SingleQuotes));
            JsParam(sb, "tabStop", this.TabStop, true);


            JsParam(sb, "colspan", StringUtil.ToInt(GetAttribute("colspan")));
            JsParam(sb, "dirty", this.Dirty, false);
            JsParam(sb, "readOnly", this.ReadOnly, false);

            JsParam(sb, "value", JsonUtil.ToJson(this.Value, JsonQuotationMark.SingleQuotes));

            JsParam(sb, "dock", this.Dock, DockStyle.None, TextTransform.Lower);
            
            JsParam(sb, "visible", this.Visible, true);
            JsParam(sb,"secFunCode", this.SecFunCode);   //权限编码
            JsParam(sb, "secReadonly", this.SecReadonly);   //只读权限

            if (this.SubItemMode)
            {
                sb.AppendFormat("    applyTo: '[data-id=\\\'{0}\\\']'", this.ID).AppendLine();
            }
            else
            {
                sb.AppendFormat("    applyTo: '#{0}'", clientId).AppendLine();
            }

            sb.AppendLine("});");

            sb.AppendLine("field.render();");

            if (this.SubItemMode)
            {

            }
            else
            {
                sb.AppendFormat("window.{0} = field;\n", clientId);
                sb.AppendFormat("Mini2.onwerPage.controls['{0}'] = field;\n", this.ID);
            }


        }

    }
}
