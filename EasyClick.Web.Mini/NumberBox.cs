using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 数字文本框
    /// </summary>
    [ToolboxData("<{0}:NumberBox runat=\"server\" />")]
    [ParseChildren(true), PersistChildren(false)]
    public class NumberBox : TextBox,IValueFormat
    {
        public NumberBox()
            : base()
        {

        }


        string m_ValueFormat;

        [DefaultValue("")]
        public string ValueFormat
        {
            get { return m_ValueFormat; }
            set { m_ValueFormat = value; }
        }
    }
}
