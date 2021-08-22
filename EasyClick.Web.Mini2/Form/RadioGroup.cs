using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2
{
    public class RadioGroup:CheckboxGroup
    {
        public RadioGroup()
        {
            this.InReady = "Mini2.ui.form.RadioGroup";

            this.JsNamespace = "Mini2.ui.form.RadioGroup";
        }

    }
}
