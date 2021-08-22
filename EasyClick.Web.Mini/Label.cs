using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{
    [DefaultProperty("Value")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:Label  runat=\"server\" />")]
    [ParseChildren(true, "Value"), PersistChildren(false)]
    public class Label : Textarea,IValueFormat
    {
        public Label()
        {
            this.HtmlTag = HtmlTextWriterTag.Label;
            base.EnableViewState = false;
        }

        [DefaultValue(false)]
        public override bool EnableViewState
        {
            get
            {
                return base.EnableViewState;
            }
            set
            {
                base.EnableViewState = value;
            }
        }

        string m_SetValueScript = "$('#{0}').html('{1}')";

        [DefaultValue("$('#{0}').html('{1}')")]
        public override string SetValueScript
        {
            get { return m_SetValueScript; }
            set { m_SetValueScript = value; }
        }

        string m_ValueFormat;

        [DefaultValue("")]
        public string ValueFormat
        {
            get
            {
                return m_ValueFormat;
            }
            set
            {
                m_ValueFormat = value;
            }
        }
    }
}
