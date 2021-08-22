using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 服务器特殊参数:
    /// SESSION_ID = SessionID;
    /// TIME_NOW = 当前时间;
    /// DATE_NOW = 当前日期;
    /// NEW_GUID = 新的 GUID;
    /// </summary>
    [Description("服务器特殊参数")]
    public class ServerParam : Param
    {

        public ServerParam()
        {
        }

        public ServerParam(string name, string serverField)
        {
            this.Name = name;
            m_ServerField = serverField;
        }

        string m_ServerField;

        /// <summary>
        /// 参数名称
        /// </summary>
        [Description("服务器参数名")]
        public string ServerField
        {
            get { return m_ServerField; }
            set { m_ServerField = value; }
        }

        public override object Evaluate(HttpContext context, Control control)
        {
            object value = null;

            string serverField = m_ServerField.ToUpper();

            switch (serverField)
            {
                case "SESSION_ID" :value = context.Session.SessionID;break;
                case "TIME_NOW" :value = DateTime.Now; break;
                case "DATE_NOW": value = DateTime.Today; break;
                case "NEW_GUID": value = Guid.NewGuid(); break;
                case "TODAY_START": value = DateTime.Today; break;
                case "TODAY_END": value = DateUtil.EndByToday(); break;
                default:
                    throw new Exception("参数名 \"" + m_ServerField + "\" 不存在.");
                    
            }

            return value;
            
        }

    }
}
