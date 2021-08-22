using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// TabPage 集合
    /// </summary>
    public class TabCollection : List<Tab>
    {

    }

    /// <summary>
    /// Tab 页版
    /// </summary>
    public class Tab:Panel
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// iframe 框架模式
        /// </summary>
        public bool IFrame { get; set; }
        
    }
}
