using System;
using System.Collections.Generic;
using System.Text;
using EasyClick.Web.Mini;
using System.ComponentModel;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 进度条
    /// </summary>
    [Description("进度条")]
    public class ProgressBar : TextBox
    {
        decimal m_Step = 1;

        int m_DecimalPrecision = 2;

        decimal m_MinValue = decimal.MinValue;

        decimal m_MaxValue = decimal.MaxValue;

        /// <summary>
        /// 数字框构造函数
        /// </summary>
        public ProgressBar()
        {
            this.InReady = "Mini2.ui.form.field.ProgressBar";
            this.JsNamespace = "Mini2.ui.form.field.ProgressBar";
        }


        [DefaultValue(1)]
        public decimal Step
        {
            get { return m_Step; }
            set { m_Step = value; }
        }

        [DefaultValue(2)]
        public int DecimalPrecision
        {
            get { return m_DecimalPrecision; }
            set { m_DecimalPrecision = value; }
        }

        private void FullScript(StringBuilder sb)
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


            JsParam(sb, "value", this.Value, "0");

            JsParam(sb, "step", this.m_Step);
            JsParam(sb, "minValue", m_MinValue, decimal.MinValue);
            JsParam(sb, "maxValue", m_MaxValue, decimal.MaxValue);
            JsParam(sb, "decimalPrecision", 2, 2);

            JsParam(sb, "fieldLabel", JsonUtil.ToJson(lab.FieldLabel, JsonQuotationMark.SingleQuotes));
            JsParam(sb, "labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
            JsParam(sb, "hideLabel", lab.HideLabel, false);
            JsParam(sb, "labelWidth", lab.LabelWidth, 100);

            JsParam(sb, "required", this.Required, false);

            JsParam(sb, "placeholder", JsonUtil.ToJson(this.Placeholder, JsonQuotationMark.SingleQuotes));
            JsParam(sb, "dock", this.Dock, DockStyle.None, TextTransform.Lower);
            JsParam(sb, "visible", this.Visible, true);
            JsParam(sb, "dirty", this.Dirty, false);
            JsParam(sb, "readOnly", this.ReadOnly, false);

            JsParam(sb, "tabStop", this.TabStop, true);

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


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {



            if (this.DesignMode)
            {
                return;
            }


            StringBuilder sb = new StringBuilder();
            string clientId = this.ClientID;

            if (this.SubItemMode)
            {
                DataViewItem item = new DataViewItem();


                sb.AppendFormat("<div data-id=\"{0}\" style=\"width: 100%;display:none; \" ></div>", this.ClientID);

                writer.Write(sb.ToString());

                StringBuilder sbJs = new StringBuilder();

                FullScript(sbJs);


                this.SubScript.Add(sbJs.ToString());

                return;
            }


            sb.AppendFormat("<div id=\"{0}\" style=\"width: 100%;display:none; \" ></div>", this.ClientID);

            ScriptManager script = ScriptManager.GetManager(this.Page);

            if (script != null)
            {
                StringBuilder sbJs = new StringBuilder();

                BeginReady(sbJs);
                FullScript(sbJs);
                EndReady(sbJs);

                script.AddScript(sbJs.ToString());

                writer.Write(sb.ToString());
            }
            else
            {

                BeginScript(sb);
                BeginReady(sb);

                FullScript(sb);

                EndReady(sb);
                EndScript(sb);

                writer.Write(sb.ToString());
            }
        }
    }
}
