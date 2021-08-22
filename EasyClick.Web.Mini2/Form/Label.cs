using System;
using System.Collections.Generic;
using System.Text;
using EasyClick.Web.Mini;
using System.ComponentModel;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// label 呈现模式
    /// </summary>
    public enum LabelMode
    {
        /// <summary>
        /// 字符串显示
        /// </summary>
        Encode,
        /// <summary>
        /// Html 显示
        /// </summary>
        Transform
    }


    /// <summary>
    /// 标签
    /// </summary>
    [Description("标签")]
    public  class Label:FieldBase
    {
        /// <summary>
        /// (构造函数)标签
        /// </summary>
        public Label()
        {
            this.InReady = "Mini2.ui.form.Label";
            this.JsNamespace = "Mini2.ui.form.Label";
        }

        string m_Placeholder;

        /// <summary>
        /// 标签显示模式
        /// </summary>
        LabelMode m_Mode = LabelMode.Encode;

        /// <summary>
        /// 标签显示模式
        /// </summary>
        [DefaultValue(LabelMode.Encode)]
        [Description("标签显示模式")]
        public LabelMode Mode
        {
            get { return m_Mode; }
            set { m_Mode = value; }
        }

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
            JsParam(sb, "id", this.ID);
            JsParam(sb, "clientId", clientId);
            JsParam(sb, "name", StringUtil.NoBlank(this.Name, clientId));

            if (this.SubItemMode)
            {
                JsParam(sb, "subItemMode", this.SubItemMode, false);

                sb.AppendLine("    parentEl: e.itemEl ,");
            }

            JsParam(sb, "dataField", this.DataField);

            JsParam(sb, "value",  JsonUtil.ToJson(this.Value, JsonQuotationMark.SingleQuotes));
            JsParam(sb, "width", this.Width, "100%");
            JsParam(sb, "minWidth", this.MinWidth);
            JsParam(sb, "maxWidth", this.MaxWidth);

            JsParam(sb, "mode", this.Mode, LabelMode.Encode, TextTransform.Upper);

            JsParam(sb, "fieldLabel", JsonUtil.ToJson(lab.FieldLabel, JsonQuotationMark.SingleQuotes));
            JsParam(sb, "labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
            JsParam(sb, "hideLabel", lab.HideLabel, false);
            JsParam(sb, "labelWidth", lab.LabelWidth, 100);

            JsParam(sb, "dock", this.Dock, DockStyle.None, TextTransform.Lower);
            JsParam(sb, "visible", this.Visible, true);

            JsParam(sb, "bodyAlign", this.BodyAlign, TextAlign.Left, TextTransform.Lower);

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
                

                sb.AppendFormat("<label data-id=\"{0}\" style=\"width: 100%;display:none;\" ></label>", this.ClientID);

                writer.Write(sb.ToString());

                StringBuilder sbJs = new StringBuilder();

                FullScript(sbJs);

                
                this.SubScript.Add(sbJs.ToString());

                return;
            }

            sb.AppendFormat("<label id=\"{0}\" style=\"width: 100%;display:none;\" ></label>", this.ClientID);


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
