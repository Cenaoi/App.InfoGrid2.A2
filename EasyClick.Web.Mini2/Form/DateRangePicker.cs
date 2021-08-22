using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using EC5.Utility;
using System.Web;

namespace EasyClick.Web.Mini2
{


    /// <summary>
    /// 日期区间框
    /// </summary>
    public class DateRangePicker : FieldBase, IRangeControl, EasyClick.Web.Mini.IMiniControl
    {
        /// <summary>
        /// (构造函数)日期区间框
        /// </summary>
        public DateRangePicker()
        {
            this.InReady = "Mini2.ui.form.field.DateRange";
            this.JsNamespace = "Mini2.ui.form.field.DateRange";
        }

        /// <summary>
        /// 显示时间
        /// </summary>
        bool m_ShowTime = false;



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
                    string valueStr = JsonUtil.ToJson(value, JsonQuotationMark.DoubleQuote);
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
        /// 获取开始时间 (例: 2017-1-1 0:0:0.000)
        /// </summary>
        public DateTime? StartDate
        {
            get
            {
                string value = this.StartValue;

                DateTime date;

                if(DateTime.TryParse(value, out date))
                {
                    return date.Date;
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    this.Value = null;
                }
                else
                {
                    this.Value = DateUtil.ToDateTimeString(value.Value);
                }
            }
        }

        /// <summary>
        /// 获取结束日期 (例: 2017-1-1 23:59:59.999)
        /// </summary>
        public DateTime? EndDate
        {
            get
            {
                string value = this.EndValue;

                DateTime date;

                if(DateTime.TryParse(value,out date))
                {
                    return date.Date.Add(new TimeSpan(0, 23, 59, 59, 998));
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    this.Value = null;
                }
                else
                {
                    this.Value = DateUtil.ToDateTimeString(value.Value);
                }
            }
        }


        public DateTime? StartMonth
        {
            get
            {
                return DateUtil.StartByMonth(this.StartDate);
            }
        }

        public DateTime? EndMonth
        {
            get
            {
                return DateUtil.EndByMonth(this.EndDate);
            }
        }

        public DateTime? StartYear
        {
            get
            {
                return DateUtil.StartByYear(this.StartDate);
            }
        }

        public DateTime? EndYear
        {
            get
            {
                return DateUtil.EndByYear(this.EndDate);
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
                //base.Value = value;
            }
        }


        public override void Reset()
        {
            this.StartValue = string.Empty;
            this.EndValue = string.Empty;

            this.Value = string.Empty;
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


        private void FullScript(StringBuilder sb)
        {
            string clientId = this.ClientID;

            Labelable lab = this.Labelable;

            sb.AppendLine("var field = Mini2.create('" + this.JsNamespace + "', {");
            JsParam(sb, "id", this.ID);
            JsParam(sb, "clientId", clientId);
            JsParam(sb, "name", StringUtil.NoBlank(this.Name, clientId));
            JsParam(sb, "width", this.Width, "100%");
            JsParam(sb, "minWidth", this.MinWidth);
            JsParam(sb, "maxWidth", this.MaxWidth);


            JsParam(sb, "fieldLabel", lab.FieldLabel);
            JsParam(sb, "labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
            JsParam(sb, "hideLabel", lab.HideLabel, false);
            JsParam(sb, "labelWidth", lab.LabelWidth, 100);
            JsParam(sb, "required", this.Required, false);

            JsParam(sb, "startValue", this.StartValue);
            JsParam(sb, "endValue", this.EndValue);

            JsParam(sb, "startDataField", this.StartDataField);
            JsParam(sb, "endDataField", this.EndDataField);
            

            JsParam(sb, "colspan", StringUtil.ToInt(GetAttribute("colspan")));

            JsParam(sb, "readOnly", this.ReadOnly, false);

            JsParam(sb, "value", this.Value);

            JsParam(sb, "dock", this.Dock, DockStyle.None, TextTransform.Lower);

            JsParam(sb, "showTime", this.ShowTime, false);

            JsParam(sb,"secFunCode", this.SecFunCode);   //权限编码
            JsParam(sb, "secReadonly", this.SecReadonly);   //只读权限

            sb.AppendFormat("    applyTo: '#{0}'", clientId).AppendLine();
            sb.AppendLine("});");

            sb.AppendLine("field.render();");

            sb.AppendFormat("window.{0} = field;\n", clientId);
            sb.AppendFormat("Mini2.onwerPage.controls['{0}'] = field;\n", this.ID);

        }


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            string clientId = this.ClientID;


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
