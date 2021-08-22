using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyClick.BizWeb
{
    /// <summary>
    /// 业务资源
    /// </summary>
    public static class BizRes
    {
        /// <summary>
        /// 业务资源
        /// </summary>
        static BizRes()
        {
            TimeSpan span = DateTime.Now - DateTime.Parse("2017-1-1");

            m_RES_VERSION = Convert.ToInt32(span.TotalSeconds).ToString();
        }

        static bool m_DomainEnabled = false;



        /// <summary>
        /// 资源版本号
        /// </summary>
        static string m_RES_VERSION;

        /// <summary>
        /// 资源域名
        /// </summary>
        static string m_RES_DOMAIN = "http://www.yq-ic.com";

        /// <summary>
        /// 远程域名激活
        /// </summary>
        [DefaultValue(false)]
        public static bool DomainEnabled
        {
            get { return m_DomainEnabled; }
            set { m_DomainEnabled = value; }
        }

        /// <summary>
        /// 资源版本号
        /// </summary>
        public static string ResVersion
        {
            get { return m_RES_VERSION; }
        }

        /// <summary>
        /// 资源域名
        /// </summary>
        public static string ResDomain
        {
            get { return m_RES_DOMAIN; }
        }


        /// <summary>
        /// 输出资源文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Write(string url)
        {
            url = url.Trim('\t', '\t');

            string exName = Path.GetExtension(url).ToUpper();

            if (exName == ".JS")
            {
                return Script(url);
            }
            else if (exName == ".CSS")
            {
                return Style(url);
            }
            else
            {
                return url;
            }
        }


        /// <summary>
        /// 资源脚本
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Script(string url)
        {
            if (m_DomainEnabled)
            {
                return $"<script src=\"{m_RES_DOMAIN}{url}?v={m_RES_VERSION}\"></script>";
            }
            else
            {
                return $"<script src=\"{url}?v={m_RES_VERSION}\"></script>";
            }
        }

        /// <summary>
        /// 样式资源
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Style(string url)
        {
            if (m_DomainEnabled)
            {
                return $"<link href=\"{m_RES_DOMAIN}{url}?v={m_RES_VERSION}\" rel=\"stylesheet\" />";
            }
            else
            {
                return $"<link href=\"{url}?v={m_RES_VERSION}\" rel=\"stylesheet\" />";
            }
        }

    }

}
