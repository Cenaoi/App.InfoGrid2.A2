using System;
using System.Collections.Generic;
using System.Text;
using EasyClick.Web.Mini;
using System.ComponentModel;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    
    /// <summary>
    /// 文本框
    /// </summary>
    [Description("文本框")]
    public class TextBox : FieldBase
    {
        /// <summary>
        /// 文本框(构造函数)
        /// </summary>
        public TextBox()
        {
            this.InReady = "Mini2.ui.form.field.Text";
            this.JsNamespace = "Mini2.ui.form.field.Text";
        }

        /// <summary>
        /// 文本框类型
        /// </summary>
        TextBoxType m_Type = TextBoxType.Text;

        /// <summary>
        /// 水印
        /// </summary>
        string m_Placeholder;


        /// <summary>
        /// 文本框类型.
        /// </summary>
        [DefaultValue(TextBoxType.Text)]
        [Description("文本框类型")]
        public TextBoxType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        /// <summary>
        /// 显示清除按钮
        /// </summary>
        [DefaultValue(false)]
        [Description("显示清除按钮")]
        public bool ClearText { get; set; } = false;


        /// <summary>
        /// 水印
        /// </summary>
        [Description("水印")]
        [DefaultValue("")]
        public string Placeholder
        {
            get {return m_Placeholder;}
            set { m_Placeholder =value;}
        }

        protected virtual void FullScript(StringBuilder sb)
        {



            string clientId = this.ClientID;

            Labelable lab = this.Labelable;

            sb.AppendLine("var field = Mini2.create('" + this.JsNamespace + "', {");

            JsParam(sb, "type", m_Type, TextBoxType.Text, TextTransform.Lower);

            JsParam(sb, "id", this.ID);
            JsParam(sb, "clientId", clientId);

            if (this.SubItemMode)
            {
                JsParam(sb, "subItemMode", this.SubItemMode, false);

                sb.AppendLine("    parentEl: e.itemEl ,");
            }

            JsParam(sb, "name", StringUtil.NoBlank(this.Name, clientId));

            JsParam(sb, "dataField", this.DataField);

            JsParam(sb, "width", this.Width, "100%");
            JsParam(sb, "minWidth", this.MinWidth);
            JsParam(sb, "maxWidth", this.MaxWidth);

            JsParam(sb, "clearText", this.ClearText, false);


            JsParam(sb, "fieldLabel", JsonUtil.ToJson(lab.FieldLabel, JsonQuotationMark.SingleQuotes));
            JsParam(sb, "labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
            JsParam(sb, "hideLabel", lab.HideLabel, false);
            JsParam(sb, "labelWidth", lab.LabelWidth, 100);

            JsParam(sb, "required", this.Required, false);

            JsParam(sb, "placeholder", JsonUtil.ToJson(this.Placeholder, JsonQuotationMark.SingleQuotes));
            
            JsParam(sb, "colspan", StringUtil.ToInt(GetAttribute("colspan")));

            JsParam(sb, "dirty", this.Dirty, false);

            JsParam(sb, "readOnly", this.ReadOnly, false);

            JsParam(sb, "value", JsonUtil.ToJson(this.Value, JsonQuotationMark.SingleQuotes));

            JsParam(sb, "dock", this.Dock, DockStyle.None, TextTransform.Lower);

            JsParam(sb, "tabStop", this.TabStop, true);

            JsParam(sb, "autoTrim", this.AutoTrim, true);

            //帮助
            JsParam(sb, "hideHelper", this.HideHelper, false);
            JsParam(sb, "helperText", this.HelperText);
            JsParam(sb, "helperLayout", this.HelperLayout, HelperLayouts.Rigth, TextTransform.Lower);
            JsParam(sb, "helperStyle", this.HelperStyle, HelperStyles.Icon, TextTransform.Lower);

            JsParam(sb, "visible", this.Visible, true);

            JsParam(sb,"secFunCode", this.SecFunCode);   //权限编码
            JsParam(sb, "secReadonly", this.SecReadonly);   //只读权限

            //输入自动提示
            if (this.HasTypeahead())
            {
                sb.AppendFormat("typeahead: {0},", this.Typeahead.ToJson());

                if (this.TypeaheadParams.Count > 0)
                {
                    sb.Append("    typeaheadParams: ").Append(this.TypeaheadParams.ToJson()).AppendLine(",");
                }

                if (this.HasTypeaheadMapItems())
                {
                    sb.AppendFormat("typeaheadMapItems: {0},", this.TypeaheadMapItems.ToJson());
                }
            }



            //映射
            if (this.HasMapItems())
            {
                sb.AppendFormat("mapItems: {0},", this.MapItems.ToJson());
            }

            JsParam(sb, "style", this.Style);

            if (this.SubItemMode)
            {
                sb.AppendFormat("    applyTo: '[data-id=\\\'{0}\\\']'",this.ID).AppendLine();
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


            if (this.SubItemMode)
            {

            }
            else
            {
                sb.AppendFormat("window.{0} = field;\n", clientId);

                sb.AppendFormat("Mini2.onwerPage.controls['{0}'] = field;\n", this.ID);
            }

            //sb.AppendLine("});");

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
                

                sb.AppendFormat("<input type=\"text\" data-id=\"{0}\" style=\"width: 100%;display:none; \" />", this.ClientID);

                writer.Write(sb.ToString());

                StringBuilder sbJs = new StringBuilder();

                FullScript(sbJs);

                
                this.SubScript.Add(sbJs.ToString());

                return;
            }


            sb.AppendFormat("<input type=\"text\" id=\"{0}\" style=\"width: 100%;display:none; \" />", this.ClientID);
            
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
