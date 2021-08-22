using System;
using System.Collections.Generic;
using System.Text;
using EC5.Utility;
using System.ComponentModel;
using System.Web;

namespace EasyClick.Web.Mini2
{
    public class NumRangeBox : FieldBase, IRangeControl, EasyClick.Web.Mini.IMiniControl
    {
         /// <summary>
        /// (构造函数)数值区间框
        /// </summary>
        public NumRangeBox()
        {
            this.InReady = "Mini2.ui.form.field.NumRange";
            this.JsNamespace = "Mini2.ui.form.field.NumRange";
        }

        decimal m_Step = 1;

        int m_DecimalPrecision = 2;

        decimal m_MinValue = decimal.MinValue;

        decimal m_MaxValue = decimal.MaxValue;



        string m_StartValue;

        string m_EndValue;


        string m_StartDataField;

        string m_EndDataField;


        /// <summary>
        /// 清空值
        /// </summary>
        public void Clear()
        {
            this.StartValue = string.Empty;
            this.EndValue = string.Empty;
        }


        /// <summary>
        /// 开始值
        /// </summary>
        [DefaultValue("")]
        public string StartValue
        {
            get { return m_StartValue; }
            set
            {
                m_StartValue = value;


                if (this.DesignMode)
                {
                    return;
                }

                if (string.IsNullOrEmpty(value))
                {
                    ScriptManager.Eval("{0}.setStartValue(\"\");", this.ClientID);
                }
                else
                {
                    string valueStr = JsonUtil.ToJson(value, JsonQuotationMark.DoubleQuote) ;
                    ScriptManager.Eval("{0}.setStartValue(\"{1}\");", this.ClientID, valueStr);
                }
            }
        }


        /// <summary>
        /// 结束值
        /// </summary>
        [DefaultValue("")]
        public string EndValue
        {
            get { return m_EndValue; }
            set
            {
                m_EndValue = value;

                if (this.DesignMode)
                {
                    return;
                }

                if (string.IsNullOrEmpty(value))
                {
                    ScriptManager.Eval("{0}.setEndValue(\"\");", this.ClientID);
                }
                else
                {
                    string valueStr = JsonUtil.ToJson(value, JsonQuotationMark.DoubleQuote);
                    ScriptManager.Eval("{0}.setEndValue(\"{1}\");", this.ClientID, valueStr);
                }
            }
        }

        /// <summary>
        /// 开始的数值
        /// </summary>
        public decimal? StartNumber
        {
            get
            {
                string value = this.StartValue;

                decimal num;

                if(decimal.TryParse(value,out num))
                {
                    return num;
                }

                return null;
            }
        }

        /// <summary>
        /// 结束的值
        /// </summary>
        public decimal? EndNumber
        {
            get
            {
                string value = this.EndValue;

                decimal num;

                if(decimal.TryParse(value,out num))
                {
                    return num;
                }

                return null;

            }
        }


        public int? StartInt
        {
            get
            {
                string value = this.StartValue;

                int num;

                if (int.TryParse(value, out num))
                {
                    return num;
                }

                return null;
            }
        }

        public int? EndInt
        {
            get
            {
                string value = this.EndValue;

                int num;

                if (int.TryParse(value, out num))
                {
                    return num;
                }

                return null;
            }
        }



        public Int64? StartInt64
        {
            get
            {
                string value = this.StartValue;

                Int64 num;

                if (Int64.TryParse(value, out num))
                {
                    return num;
                }

                return null;
            }
        }

        public Int64? EndInt64
        {
            get
            {
                string value = this.EndValue;

                Int64 num;

                if (Int64.TryParse(value, out num))
                {
                    return num;
                }

                return null;
            }
        }

        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;
            }
        }




        /// <summary>
        /// 开始日期数据字段
        /// </summary>
        [DefaultValue("")]
        [Description("开始日期数据字段")]
        [Category("DBField 数据绑定")]
        public string StartDataField
        {
            get { return m_StartDataField; }
            set { m_StartDataField = value; }
        }

        /// <summary>
        /// 结束日期数据字段
        /// </summary>
        [DefaultValue("")]
        [Description("结束日期数据字段")]
        [Category("DBField 数据绑定")]
        public string EndDataField
        {
            get { return m_EndDataField; }
            set { m_EndDataField = value; }
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


        public override void Reset()
        {
            this.StartValue = string.Empty;
            this.EndValue = string.Empty;

            this.Value = string.Empty;
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


            //JsParam(sb, "value", this.Value, "0");


            JsParam(sb, "startValue", this.StartValue);
            JsParam(sb, "endValue", this.EndValue);

            JsParam(sb, "step", this.m_Step);
            JsParam(sb, "minValue", m_MinValue);
            JsParam(sb, "maxValue", m_MaxValue);
            JsParam(sb, "decimalPrecision", 2, 2);
            JsParam(sb, "fieldLabel", JsonUtil.ToJson(lab.FieldLabel, JsonQuotationMark.SingleQuotes));
            JsParam(sb, "labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
            JsParam(sb, "labelWidth", lab.LabelWidth, 100);

            JsParam(sb, "startDataField", this.StartDataField);
            JsParam(sb, "endDataField", this.EndDataField);

            JsParam(sb, "required", this.Required, false);

            JsParam(sb, "dirty", this.Dirty, false);
            JsParam(sb, "readOnly", this.ReadOnly, false);

            JsParam(sb, "tabStop", this.TabStop, true);

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


                sb.AppendFormat("<div type=\"text\" data-id=\"{0}\" style=\"width: 100%;display:none; \" ></div>", this.ClientID);

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


        public void LoadPostData()
        {
            string startId = this.ClientID + "_Start";
            string endId = this.ClientID + "_End";

            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            m_StartValue = request[startId];
            m_EndValue = request[endId];
        }

    }
}
