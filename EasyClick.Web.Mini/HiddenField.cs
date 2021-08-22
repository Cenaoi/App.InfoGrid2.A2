using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 隐藏字段
    /// </summary>
    [Description("隐藏字段")]
    [ToolboxData("<{0}:HiddenField  runat=\"server\" />")]
    [ParseChildren(true), PersistChildren(false)]
    public class HiddenField : MiniHtmlBase
    {
        public HiddenField()
        {
            this.Type = "hidden";
        }


    }
}
