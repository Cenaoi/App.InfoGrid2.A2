using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 下载框
    /// </summary>
    public class DownloadWindow:Window
    {

        /// <summary>
        /// 下载地址
        /// </summary>
        public string FileUrl { get; set; }


        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 下载框(构造函数)
        /// </summary>
        public DownloadWindow()
        {
            this.JsNamespace = "Mini2.ui.extend.DownloadWindow";

            this.Text = "下载";
            this.StartPosition = WindowStartPosition.CenterScreen;
            this.Width = 300;
            this.Height = 200;

            this.RegionProperty("FileName","fileName");
            this.RegionProperty("FileUrl","fileUrl");
        }


        /// <summary>
        /// 下载框(构造函数)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="fileUrl">下载路径</param>
        public DownloadWindow(string fileName, string fileUrl) : this()
        {
            this.FileName = fileName;
            this.FileUrl = fileUrl;
        }


        public static void Show(string fileName, string fileUrl)
        {
            DownloadWindow win = new DownloadWindow(fileName, fileUrl);
            win.Show();
        }

    }
}
