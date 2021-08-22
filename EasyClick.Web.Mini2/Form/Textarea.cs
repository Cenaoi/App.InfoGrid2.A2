using System;
using System.Collections.Generic;
using System.Text;
using EasyClick.Web.Mini;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 大文本框
    /// </summary>
    public class Textarea: FieldBase
    {
        public Textarea()
        {
            this.InReady = "Mini2.ui.form.field.TextArea";
            this.JsNamespace = "Mini2.ui.form.field.TextArea";
        }

        string m_Placeholder;

        public string Placeholder
        {
            get {return m_Placeholder;}
            set { m_Placeholder =value;}
        }


        private void FullScript(StringBuilder sb)
        {

            string clientId = this.ClientID;
            Labelable lab = this.Labelable;

            sb.AppendLine("var field = Mini2.create('" + this.JsNamespace + "', {");
            JsParam(sb, "clientId", clientId);
            JsParam(sb, "name", StringUtil.NoBlank(this.Name,clientId));

            if (this.SubItemMode)
            {
                JsParam(sb, "subItemMode", this.SubItemMode, false);

                sb.AppendLine("    parentEl: e.itemEl ,");
            }

            JsParam(sb, "dataField", this.DataField);

            JsParam(sb, "width", this.Width, "100%");
            JsParam(sb, "minWidth", this.MinWidth);
            JsParam(sb, "maxWidth", this.MaxWidth);

            JsParam(sb, "height", this.Height);
            JsParam(sb, "minHeight", this.MinHeight);
            JsParam(sb, "maxHeight", this.MaxHeight);

            JsParam(sb, "fieldLabel",  JsonUtil.ToJson(lab.FieldLabel, JsonQuotationMark.SingleQuotes));
            JsParam(sb, "labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
            JsParam(sb, "hideLabel", lab.HideLabel, false);
            JsParam(sb, "labelWidth", lab.LabelWidth, 100);
            JsParam(sb, "dirty", this.Dirty, false);
            JsParam(sb, "readOnly", this.ReadOnly, false);

            JsParam(sb, "extraFieldBodyCls", this.ExtraFieldBodyCls);   //扩展字段

            JsParam(sb, "required", this.Required, false);

            JsParam(sb, "placeholder", JsonUtil.ToJson(this.Placeholder, JsonQuotationMark.SingleQuotes));

            JsParam(sb, "dock", this.Dock, DockStyle.None, TextTransform.Lower);
            JsParam(sb, "visible", this.Visible, true);
            JsParam(sb, "tabStop", this.TabStop, true);

            //帮助
            JsParam(sb, "hideHelper", this.HideHelper, false);
            JsParam(sb, "helperText", this.HelperText);
            JsParam(sb, "helperLayout", this.HelperLayout, HelperLayouts.Rigth, TextTransform.Lower);
            JsParam(sb, "helperStyle", this.HelperStyle, HelperStyles.Icon, TextTransform.Lower);

            JsParam(sb,"secFunCode", this.SecFunCode);   //权限编码
            JsParam(sb, "secReadonly", this.SecReadonly);   //只读权限

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



            //sb.AppendLine("});");

            //sb.AppendLine("field.render();");

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
                

                sb.AppendFormat("<textarea data-id=\"{0}\" style=\"width: 100%;display:none;\" ></textarea>", this.ClientID);

                writer.Write(sb.ToString());

                StringBuilder sbJs = new StringBuilder();

                FullScript(sbJs);

                
                this.SubScript.Add(sbJs.ToString());

                return;
            }

            sb.AppendFormat("<textarea id=\"{0}\" style=\"width: 100%;display:none;\" ></textarea>", this.ClientID);

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

