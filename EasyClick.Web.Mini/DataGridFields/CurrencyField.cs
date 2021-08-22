using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 货币字段
    /// </summary>
    public class CurrencyField : BoundField
    {
        /// <summary>
        /// 货币字段
        /// </summary>
        public CurrencyField()
            : base()
        {
            base.ItemAlign = CellAlign.Right;
            this.Width = 60;
            this.DataFormatString = "{0:#,##0.00}";
        }

        [DefaultValue(CellAlign.Right)]
        public override CellAlign ItemAlign
        {
            get
            {
                return base.ItemAlign;
            }
            set
            {
                base.ItemAlign = value;
            }
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
