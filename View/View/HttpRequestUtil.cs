
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace EC5.YQIC.Windows.HttpUtilitys
{
    public static class HttpRequestUtil
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private static readonly string DefaultUserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.125 Safari/537.36";


        public static HttpWebResponse CreatePostHttpResponseBase(string url, byte[] data, string[] header)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }


            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            request.AllowAutoRedirect = false;
            request.Method = "POST";
            request.UserAgent = DefaultUserAgent;
            request.ContentType = "application/x-www-form-urlencoded";
            
            



            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }


            return request.GetResponse() as HttpWebResponse;
        }


        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreateGetHttpResponseBase(string url, string[] header)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }


            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            request.AllowAutoRedirect = false;

            

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            return response;
        }



        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

    }
}
