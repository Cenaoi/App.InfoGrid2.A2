using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 文本框窗体
    /// </summary>
    public class TextWindow : Window
    {

        /// <summary>
        /// 文本框窗体(构造函数)
        /// </summary>
        public TextWindow()
        {
            this.JsNamespace = "Mini2.ui.extend.TextWindow";

            this.Text = "文本窗口";
            this.StartPosition = WindowStartPosition.CenterScreen;
            this.Width = 600;
            this.Height = 400;

            this.RegionProperty("ContentFontSize", "contentFontSize");
            //this.RegionProperty("FileUrl", "fileUrl");

            //mode: true,
            //text: '文本框',
            //iframe: false,
            //startPosition: 'center_screen',
        }


        /// <summary>
        /// 文本框窗体(构造函数)
        /// </summary>
        /// <param name="text">文件名</param>
        public TextWindow(string text) : this()
        {
            this.Text = text;
        }


        /// <summary>
        /// 字体尺寸
        /// </summary>
        public string ContentFontSize { get; set; }


    }
}
