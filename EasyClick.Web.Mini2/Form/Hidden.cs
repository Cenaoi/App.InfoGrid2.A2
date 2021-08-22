using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 隐藏框
    /// </summary>
    [Description("隐藏框")]
    public class Hidden : FieldBase
    {
                
        /// <summary>
        /// 文本框
        /// </summary>
        public Hidden()
        {
            this.InReady = "Mini2.ui.form.field.Hidden";
            this.JsNamespace = "Mini2.ui.form.field.Hidden";
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

            JsParam(sb, "value", JsonUtil.ToJson(this.Value, JsonQuotationMark.SingleQuotes));

            if (this.SubItemMode)
            {
                sb.AppendFormat("    applyTo: '[data-id=\\\'{0}\\\']'", this.ID).AppendLine();
            }
            else
            {
                sb.AppendFormat("    applyTo: '#{0}'", clientId).AppendLine();
            }

            sb.AppendLine("});");

            if (this.IsDelayRender)
            {
                sb.AppendLine("field.delayRender();");
            }
            else
            {
                sb.AppendLine("field.render();");
            }




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

                sb.AppendFormat("<input type=\"hidden\" id=\"{0}\" ", this.ClientID);

                if (m_HtmlAttrs != null)
                {
                    foreach (var item2 in this.m_HtmlAttrs)
                    {
                        sb.Append($"{item2.Key}=\"{item2.Value}\" ");
                    }
                }

                sb.Append("/>");

                writer.Write(sb.ToString());

                StringBuilder sbJs = new StringBuilder();

                FullScript(sbJs);

                
                this.SubScript.Add(sbJs.ToString());

                return;
            }

            sb.AppendFormat("<input type=\"hidden\" id=\"{0}\" ", this.ClientID);

            if (m_HtmlAttrs != null)
            {
                foreach (var item in this.m_HtmlAttrs)
                {
                    sb.Append($"{item.Key}=\"{item.Value}\" ");
                }
            }

            sb.Append("/>");


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
