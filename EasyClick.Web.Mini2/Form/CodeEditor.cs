using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 代码编辑器
    /// </summary>
    public class CodeEditor: Textarea
    {
        public CodeEditor()
        {
            this.InReady = "Mini2.ui.form.field.CodeEditor";
            this.JsNamespace = "Mini2.ui.form.field.CodeEditor";
        }


    }
}
