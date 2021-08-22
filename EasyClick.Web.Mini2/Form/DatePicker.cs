using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 日期下拉框
    /// </summary>
    public class DatePicker:TextBox
    {
        /// <summary>
        /// 日期下拉框(构造函数)
        /// </summary>
        public DatePicker()
        {
            this.InReady = "Mini2.ui.form.field.Date";

            this.JsNamespace = "Mini2.ui.form.field.Date";
        }

        string m_Format = "Y-m-d";

        bool m_ShowTime = false;

        /// <summary>
        /// 显示时间
        /// </summary>
        [Description("显示时间")]
        [DefaultValue(false)]
        public bool ShowTime
        {
            get { return m_ShowTime; }
            set { m_ShowTime = value; }
        }


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
        /// 获取开始时间 (例: 2017-1-1 0:0:0.000)
        /// </summary>
        public DateTime? Date
        {
            get
            {
                string value = this.Value;

                DateTime date;

                if (DateTime.TryParse(value, out date))
                {
                    return date;
                }

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

            JsParam(sb, "format", this.Format, "Y-m-d");


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

            JsParam(sb, "showTime", this.ShowTime, false);
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
