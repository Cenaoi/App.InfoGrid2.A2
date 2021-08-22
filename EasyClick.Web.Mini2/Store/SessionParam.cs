using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// Session 参数
    /// </summary>
    public class SessionParam : Param
    {
        string m_SessionField;

        /// <summary>
        /// Session 参数名称
        /// </summary>
        public string SessionField
        {
            get { return m_SessionField; }
            set { m_SessionField = value; }
        }

        public override object Evaluate(HttpContext context, Control control)
        {
            object valueObj = context.Session[m_SessionField];

            object value = null;

            if (valueObj != null)
            {
                if (valueObj.GetType() == typeof(string))
                {
                    if (string.IsNullOrEmpty(this.DefaultValue))
                    {
                        value = StringUtil.ChangeType((string)valueObj, this.DbType);
                    }
                    else
                    {
                        value = StringUtil.ChangeType((string)valueObj, this.DbType, this.DefaultValue);
                    }
                }
                else
                {
                    value = valueObj;
                }
            }
            else if (!string.IsNullOrEmpty(this.DefaultValue))
            {
                value = StringUtil.ChangeType(this.DefaultValue, this.DbType);
            }


            return value;
        }
    }
}
