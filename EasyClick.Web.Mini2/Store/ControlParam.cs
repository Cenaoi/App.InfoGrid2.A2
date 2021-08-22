using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Reflection;
using EC5.Utility;
using System.ComponentModel;
using System.Web;

namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// 控件参数
    /// </summary>
    [Description("控件参数")]
    public class ControlParam : Param
    {
        string m_ControlID;
        string m_PropertyName;

        /// <summary>
        /// (构造函数)控件参数
        /// </summary>
        public ControlParam()
        {
        }

        /// <summary>
        /// (构造函数)控件参数
        /// </summary>
        /// <param name="name">参数名称</param>
        public ControlParam(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 控件参数的构造函数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="controlId">控件ID</param>
        /// <param name="propertyName">控件属性</param>
        public ControlParam(string name, string controlId, string propertyName)
        {
            this.Name = name;
            this.ControlID = controlId;
            this.PropertyName = propertyName;
        }


        /// <summary>
        /// 控件ID
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [TypeConverter(typeof(System.Web.UI.WebControls.ControlIDConverter))]
        [DefaultValue("")]
        [Description("控件ID")]
        [System.Web.UI.IDReferenceProperty]
        public string ControlID
        {
            get { return m_ControlID; }
            set { m_ControlID = value; }
        }

        /// <summary>
        /// 控件属性名称
        /// </summary>
        [DefaultValue("")]
        [Description("控件属性名称")]
        [TypeConverter(typeof(System.Web.UI.WebControls.ControlPropertyNameConverter))]
        public string PropertyName
        {
            get { return m_PropertyName; }
            set { m_PropertyName = value; }
        }

        private UserControl GetParentBox(System.Web.UI.Control control)
        {

            UserControl uCon = null;

            Control con = control;

            for (int i = 0; i < 99; i++)
            {
                if (con is UserControl)
                {
                    uCon = (UserControl)con;
                    break;
                }

                con = con.Parent;
            }

            return uCon;
        }

        public override object Evaluate(HttpContext context, System.Web.UI.Control control)
        {
            if (StringUtil.IsBlank(this.ControlID))
            {
                throw new Exception("筛选条件的控件不能为空");
            }

            if (StringUtil.IsBlank(this.PropertyName))
            {
                throw new Exception("筛选条件的控件属性不能为空");
            }

            UserControl uCon = GetParentBox(control);

            Control targetCon = uCon.FindControl(this.ControlID);

            if (targetCon == null)
            {
                throw new Exception(string.Format("控件“{0}”不存在", this.ControlID));
            }


            PropertyInfo pi = ObjectUtil.GetProperty(targetCon, this.PropertyName);

            if (pi == null)
            {
                throw new Exception(string.Format("控件“{0}”的属性 “{1}” 不存在", this.ControlID, this.PropertyName));
            }

            object value = pi.GetValue(targetCon, null);

            if (value == null)
            {
                if (!string.IsNullOrEmpty(this.DefaultValue))
                {
                    value = Convert.ChangeType(this.DefaultValue, this.Type);
                }

            }
            else if (value.GetType() == typeof(string) && this.Type != TypeCode.Empty)
            {
                value = ConvertValue((string)value);
            }
            

            return value;
        }

        /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object ConvertValue(string value)
        {
            object targetValue = null;

            if (StringUtil.IsBlank(value))
            {
                if (!StringUtil.IsBlank(this.DefaultValue))
                {
                    targetValue = Convert.ChangeType(this.DefaultValue, this.Type);
                }
            }
            else
            {
                targetValue = Convert.ChangeType(value, this.Type);
            }

            return targetValue;
        }


        public override string ToJson()
        {
            ScriptTextWriter st = new ScriptTextWriter(QuotationMarkConvertor.SingleQuotes);

            st.RetractBengin("{");

            st.WriteParam("role", "control");
            st.WriteParam("name", this.Name);

            st.WriteParam("control", this.ControlID);

            st.WriteParam("default", this.DefaultValue);
            //st.WriteParam("propName", this.PropertyName);

            st.RetractEnd("}");

            string json = st.ToString();

            return json;
        }
    }
}
