using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 窗体脚部
    /// </summary>
    public class WindowFooter:Panel
    {
        /// <summary>
        /// 窗体脚部
        /// </summary>
        public WindowFooter()
        {
            this.Dock = DockStyle.Bottom;
            this.Region = RegionType.South;
            this.Height = 44;
            this.Ui = UiStyle.Footer;
            this.Scroll = ScrollBars.None;
            this.Layout = LayoutStyle.Toolbar;
            this.ItemMarginRight = "10px";
            this.PaddingRight = 10;

            this.Padding = 8;
        }

        

    }
}
