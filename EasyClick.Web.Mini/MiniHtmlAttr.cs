using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace EasyClick.Web.Mini
{
    [DebuggerDisplay("Key={Key},Value={Value}")]
    public class MiniHtmlAttr
    {
        string m_Key;

        string m_Value;

        public MiniHtmlAttr()
        {
        }

        public MiniHtmlAttr(string key, string value)
        {
            m_Key = key;
            m_Value = value;
        }

        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
    }
}
