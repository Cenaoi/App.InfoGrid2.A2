using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    public class NumberBoxField:TextBoxField
    {
        public NumberBoxField()
        {
            this.ItemAlign = CellAlign.Right;
            this.Width = 60;
        }

        [DefaultValue(60)]
        public override int Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
            }
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
    }
}
