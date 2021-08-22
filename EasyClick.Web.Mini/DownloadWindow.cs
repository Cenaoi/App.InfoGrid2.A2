using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 下载文件的窗体
    /// </summary>
    public class DownloadWindow:Window
    {
        #region 构造方法

        public DownloadWindow()
        {
            this.Title = "下载文件";
        }

        public DownloadWindow(string title)
            : base(title)
        {
            this.Title = "下载文件";
        }

        public DownloadWindow(int width, int height)
            : base(width, height)
        {
            this.Title = "下载文件";
        }

        public DownloadWindow(string title, int width, int height)
            : base(title, width, height)
        {
            this.Title = "下载文件";
        }

        #endregion

        int m_FileSize;
        string m_FileUrl;
        string m_FileName;

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize
        {
            get { return m_FileSize; }
            set { m_FileSize = value; }
        }

        /// <summary>
        /// 文件下载地址
        /// </summary>
        public string FileUrl
        {
            get { return m_FileUrl; }
            set { m_FileUrl = value; }
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }

        
        /// <summary>
        /// 显示模式窗体
        /// </summary>
        public override void ShowDialog()
        {
            this.ContentHtml = string.Format("<div> <a href='{0}'>下载：{1}</a>",m_FileUrl, m_FileName);

            base.ShowDialog();
        }

        /// <summary>
        /// 显示普通窗体
        /// </summary>
        public override void Show()
        {
            this.ContentHtml = string.Format("<div> <a href='{0}'>下载：{1}</a>", m_FileUrl, m_FileName);

            base.Show();
        }



        #region 静态方法

        /// <summary>
        /// 显示模式窗体
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileUrl"></param>
        public static void ShowDialog(string fileName, string fileUrl)
        {
            DownloadWindow win = new DownloadWindow(400, 80);

            win.FileName = fileName;
            win.FileUrl = fileUrl;

            win.ShowDialog();
        }

        /// <summary>
        /// 显示模式窗体
        /// </summary>
        /// <param name="fileUrl"></param>
        public static void ShowDialog(string fileUrl)
        {
            string filename = Path.GetFileName(fileUrl);

            DownloadWindow win = new DownloadWindow(400, 80);

            win.FileName = filename;
            win.FileUrl = fileUrl;

            win.ShowDialog();
        }

        /// <summary>
        /// 显示普通窗体
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileUrl"></param>
        public static void Show(string fileName, string fileUrl)
        {
            DownloadWindow win = new DownloadWindow(400, 80);

            win.FileName = fileName;
            win.FileUrl = fileUrl;

            win.Show();
        }

        /// <summary>
        /// 显示普通窗体
        /// </summary>
        /// <param name="fileUrl"></param>
        public static void Show(string fileUrl)
        {
            string filename = Path.GetFileName(fileUrl);

            DownloadWindow win = new DownloadWindow(400, 80);

            win.FileName = filename;
            win.FileUrl = fileUrl;

            win.Show();
        }

        #endregion
    }
}
