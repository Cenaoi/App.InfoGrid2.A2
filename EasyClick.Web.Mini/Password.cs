using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 密码框
    /// </summary>
    [Description("密码框")]
    [ToolboxData("<{0}:Password  runat=\"server\" />")]
    [ParseChildren(true), PersistChildren(false)]
    public class Password : MiniHtmlBase
    {
        public Password()
        {
            this.Type = "password";
        }
    }
}
