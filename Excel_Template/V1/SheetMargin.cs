using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.V1
{

    /// <summary>
    /// 打印页面外边距
    /// </summary>
    public class SheetMargin
    {
        /// <summary>
        /// 底部外边距
        /// </summary>
        public double BottomMargin { get; set; }

        /// <summary>
        /// 尾部外边距
        /// </summary>
        public double FooterMargin { get; set; }

        /// <summary>
        /// 头部外边距
        /// </summary>
        public double HeaderMargin { get; set; }

        /// <summary>
        /// 左边外边距
        /// </summary>
        public double LeftMargin { get; set; }

        /// <summary>
        /// 右边外边距
        /// </summary>
        public double RightMargin { get; set; }

        /// <summary>
        /// 顶部外边距
        /// </summary>
        public double TopMargin { get; set; }
    }
}
