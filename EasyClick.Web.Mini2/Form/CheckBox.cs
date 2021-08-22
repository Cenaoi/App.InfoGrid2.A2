using System;
using System.Collections.Generic;
using System.Text;
using EasyClick.Web.Mini;
using System.ComponentModel;
using EC5.Utility;
using System.Web;

namespace EasyClick.Web.Mini2
{


    /// <summary>
    /// 复选框
    /// </summary>
    [DefaultProperty("Checked")]
    [Description("复选框")]
    public class CheckBox : FieldBase, IChecked,IMiniControl
    {
        public CheckBox()
        {
            this.InReady = "Mini2.ui.form.field.CheckBox";

            this.JsNamespace = "Mini2.ui.form.field.CheckBox";
        }


        string m_TrueValue = "on";

        string m_FalseValue;

        bool m_Checked = false;

        string m_BoxLabel;

        /// <summary>
        /// 标签
        /// </summary>
        [DefaultValue("")]
        [Description("标签")]
        public string BoxLabel
        {
            get { return m_BoxLabel; }
            set { m_BoxLabel = value; }
        }


        /// <summary>
        /// True 值显示的文本
        /// </summary>
        [DefaultValue("")]
        [Description("True 值显示的文本")]
        public string TrueText { get; set; }


        /// <summary>
        /// False 值显示的文本
        /// </summary>
        [DefaultValue("")]
        [Description("False 值显示的文本")]
        public string FalseText { get; set; }

        


        /// <summary>
        /// True 值
        /// </summary>
        [DefaultValue("")]
        [Description("True 值")]
        public string TrueValue
        {
            get { return m_TrueValue; }
            set { m_TrueValue = value; }
        }

        /// <summary>
        /// False 值
        /// </summary>
        [DefaultValue("")]
        [Description("False 值")]
        public string FalseValue
        {
            get { return m_FalseValue; }
            set { m_FalseValue = value; }
        }

        /// <summary>
        /// 值
        /// </summary>
        [DefaultValue("")]
        public override string Value
        {
            get
            {
                if (m_Checked)
                {
                    return m_TrueValue;
                }
                else
                {
                    return m_FalseValue;
                }

            }
            set
            {
                if (value == m_TrueValue)
                {
                    m_Checked = true;
                }
                else if (value == m_FalseValue)
                {
                    m_Checked = false;
                }

                base.Value = value;
            }
        }

        /// <summary>
        /// (JScript) 被选中
        /// </summary>
        [DefaultValue(false)]
        [Description("被选中")]
        public bool Checked
        {
            get { return m_Checked; }
            set
            {
                if (!this.DesignMode && m_Checked != value && !MiniScriptManager.ClientScript.ReadOnly)
                {
                    MiniScript.Add("{0}.setValue('{1}')", this.ClientID, value.ToString().ToLower());
                }

                m_Checked = value;
            }
        }



        private void FullScript(StringBuilder sb)
        {
            string clientId = this.ClientID;


            Labelable lab = this.Labelable;

            sb.AppendLine("var field = Mini2.create('" + this.JsNamespace + "', {");


            JsParam(sb, "id", this.ID);
            JsParam(sb, "clientId", clientId);

            if (this.SubItemMode)
            {
                JsParam(sb, "subItemMode", this.SubItemMode, false);

                sb.AppendLine("    parentEl: e.itemEl ,");
            }

            JsParam(sb, "name",StringUtil.NoBlank(this.Name, clientId));
            JsParam(sb, "dataField", this.DataField);

            JsParam(sb, "trueText", this.TrueText);
            JsParam(sb, "falseText", this.FalseText);

            JsParam(sb, "width", this.Width, "100%");
            JsParam(sb, "minWidth", this.MinWidth);
            JsParam(sb, "maxWidth", this.MaxWidth);


            JsParam(sb, "checked", this.m_Checked, false);

            JsParam(sb, "boxLabel", this.BoxLabel);
            JsParam(sb, "fieldLabel", lab.FieldLabel);
            JsParam(sb, "labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
            JsParam(sb, "hideLabel", lab.HideLabel, false);
            JsParam(sb, "labelWidth", lab.LabelWidth, 100);
            JsParam(sb, "required", this.Required, false);
            JsParam(sb, "tabStop", this.TabStop, true);


            JsParam(sb, "dock", this.Dock, DockStyle.None, TextTransform.Lower);
            JsParam(sb, "visible", this.Visible, true);
            JsParam(sb, "dirty", this.Dirty, false);
            JsParam(sb, "readOnly", this.ReadOnly, false);


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



        public void LoadPostData()
        {
            HttpRequest request = this.Context.Request;

            string value = request.Form[this.ClientID];

            if (!string.IsNullOrEmpty(value))
            {
                m_Checked = true;
            }
        }
    }
}
