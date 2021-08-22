using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace EasyClick.Web.Mini
{

    public class MiniWindow
    {
        /// <summary>
        /// 父级层次
        /// </summary>
        internal int m_LevelIndex = 0;

        public MiniWindow Parent
        {
            get
            {
                MiniWindow mw = new MiniWindow();
                mw.m_LevelIndex = (m_LevelIndex + 1);

                return mw;
            }
        }

        #region Redirect 跳转

        /// <summary>
        /// (JScript) 跳转
        /// </summary>
        /// <param name="url"></param>
        public void Redirect(string url)
        {
            HttpResponse response = HttpContext.Current.Response;

            if (response == null)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < m_LevelIndex; i++)
            {
                sb.Append("parent.");
            }

            MiniScript.Add("window.{0}location.href='{1}';", sb.ToString(), url);

        }

        /// <summary>
        /// (JScript) 跳转
        /// </summary>
        /// <param name="frameName"></param>
        /// <param name="url"></param>
        public void Redirect(string frameName, string url)
        {
            HttpResponse response = HttpContext.Current.Response;

            if (response == null)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < m_LevelIndex; i++)
            {
                sb.Append("parent.");
            }

            MiniScript.Add("window.{0}frames['{1}'].location.href='{2}';", sb.ToString(), frameName, url);
        }


        /// <summary>
        /// (JScript) 跳转
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="url"></param>
        public void Redirect(int frameIndex, string url)
        {
            HttpResponse response = HttpContext.Current.Response;

            if (response == null)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < m_LevelIndex; i++)
            {
                sb.Append("parent.");
            }

            MiniScript.Add("window.{0}frames[{1}].location.href='{2}';", sb.ToString(), frameIndex, url);
        }

        #endregion

        /// <summary>
        /// (JScript) 刷新页面
        /// </summary>
        /// <param name="url"></param>
        public void Reload()
        {
            HttpResponse response = HttpContext.Current.Response;

            if (response == null)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < m_LevelIndex; i++)
            {
                sb.Append("parent.");
            }

            MiniScript.Add("window.{0}location.reload();", sb.ToString());

        }

        /// <summary>
        /// (JScript) 关闭窗体 
        /// </summary>
        public void CloseForm()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < m_LevelIndex; i++)
            {
                sb.Append("parent.");
            }

            MiniScript.Add("window.{0}external.CloseForm();", sb.ToString());
        }

    }
}
