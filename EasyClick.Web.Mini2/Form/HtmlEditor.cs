using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// Html 富文本框
    /// </summary>
    [Description("Html 富文本框")]
    public class HtmlEditor: FieldBase
    {
        /// <summary>
        /// Html 富文本框(构造函数)
        /// </summary>
        public HtmlEditor()
        {
            this.InReady = "Mini2.ui.form.field.HtmlEditor";
            this.JsNamespace = "Mini2.ui.form.field.HtmlEditor";
        }

        string m_Placeholder;

        public string Placeholder
        {
            get {return m_Placeholder;}
            set { m_Placeholder =value;}
        }


        /// <summary>
        /// 图片上传路径
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 文件上传路径
        /// </summary>
        public string FileUrl { get; set; }

        private void FullScript(StringBuilder sb)
        {
            string clientId = this.ClientID;

            Labelable lab = this.Labelable;

            sb.AppendLine("var field = Mini2.create('" + this.JsNamespace + "', {");
            JsParam(sb, "id", this.ID);
            JsParam(sb, "clientId", clientId);
            JsParam(sb, "name", StringUtil.NoBlank(this.Name, clientId));
            JsParam(sb, "dataField", this.DataField);

            JsParam(sb, "width", this.Width, "100%");
            JsParam(sb, "minWidth", this.MinWidth);
            JsParam(sb, "maxWidth", this.MaxWidth);


            JsParam(sb, "fieldLabel", lab.FieldLabel);
            JsParam(sb, "labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
            JsParam(sb, "hideLabel", lab.HideLabel, false);
            JsParam(sb, "labelWidth", lab.LabelWidth, 100);

            JsParam(sb, "placeholder", this.Placeholder);
            
            JsParam(sb, "colspan", StringUtil.ToInt(GetAttribute("colspan")));

            JsParam(sb, "readOnly", this.ReadOnly, false);

            JsParam(sb, "imageUrl", this.ImageUrl);
            JsParam(sb, "fileUrl", this.FileUrl);

            JsParam(sb, "value", JsonUtil.ToJson(this.Value, JsonQuotationMark.SingleQuotes));

            JsParam(sb, "dock", this.Dock, DockStyle.None, TextTransform.Lower);
            JsParam(sb, "visible", this.Visible, true);

            JsParam(sb,"secFunCode", this.SecFunCode);   //权限编码
            JsParam(sb, "secReadonly", this.SecReadonly);   //只读权限

            sb.AppendFormat("    applyTo: '#{0}'", clientId).AppendLine();
            sb.AppendLine("});");

            sb.AppendLine("field.render();");

            sb.AppendFormat("window.{0} = field;\n", clientId);
            sb.AppendFormat("Mini2.onwerPage.controls['{0}'] = field;\n", this.ID);
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

            sb.AppendFormat("<textarea id=\"{0}\" style=\"width: 100%;display:none; \" ></textarea>", this.ClientID);


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
