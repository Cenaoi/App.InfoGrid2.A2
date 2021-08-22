using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 可编辑的货币编辑框
    /// </summary>
    [Description("货币文本编辑框")]
    public class EditorCurrencyCell:EditorNumberCell
    {
        /// <summary>
        /// 
        /// </summary>
        public EditorCurrencyCell():base()
        {
            this.DataFormatString = "{0:#,##0.00}";
        }

        [DefaultValue("{0:#,##0.00}")]
        public override string DataFormatString
        {
            get
            {
                return base.DataFormatString;
            }
            set
            {
                base.DataFormatString = value;
            }
        }
    }
}
