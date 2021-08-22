using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 表单版面的子控件
    /// </summary>
    public class FormLayoutItemEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="item"></param>
        public FormLayoutItemEventArgs(Control item)
        {
            this.Item = item;
        }

        /// <summary>
        /// 取消
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// 控件项目
        /// </summary>
        public Control Item { get; protected set; }
    }
}
