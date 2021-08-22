using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 输入字段错误信息
    /// </summary>
    public class InputFieldError
    {
        string m_CellID;

        string m_Message;

        int m_ItemGuid;

        public InputFieldError()
        {
        }


        public InputFieldError(string message)
        {
            m_Message = message;
        }

        public InputFieldError(string messageFormat,object arg0)
        {
            m_Message = string.Format(messageFormat, arg0);
        }

        public InputFieldError(string messageFormat, object arg0,object arg1)
        {
            m_Message = string.Format(messageFormat, arg0,arg1);
        }

        public InputFieldError(string messageFormat, object arg0, object arg1, object arg2)
        {
            m_Message = string.Format(messageFormat, arg0, arg1, arg2);
        }


        /// <summary>
        /// Web 控件ID
        /// </summary>
        public string CellID
        {
            get { return m_CellID; }
            set { m_CellID = value; }
        }

        [DefaultValue("")]
        public int ItemGuid
        {
            get { return m_ItemGuid; }
            set { m_ItemGuid = value; }
        }

        [DefaultValue("")]
        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }
    }
}
