using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace App.InfoGrid2.Wxhb.Bll
{
    /// <summary>
    /// 微信业务助手类
    /// </summary>
    public class WeChatUtil
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取 sha1 算法的 签名
        /// </summary>
        /// <param name="sm">要排序的数据</param>
        /// <returns></returns>
        public static string GetSha1Sign(SModel sm)
        {

            SortedList<string, string> dic = new SortedList<string, string>();

            foreach (string name in sm.GetFields())
            {

                string value = sm[name].ToString();

                dic.Add(name, value);

            }


            StringBuilder sb = new StringBuilder();

            int i = 0;

            foreach (var item in dic)
            {

                if (i++ > 0)
                {
                    sb.Append("&");
                }

                sb.Append(item.Key + "=" + item.Value);
            }


            string stringSignTemp = sb.ToString();

            log.Debug($"排完序后的字符串：{stringSignTemp}");

            string sign = HashCode(stringSignTemp);

            log.Debug("sha1 算法的签名：" + sign);


            return sign;


        }


        /// <summary>
        /// sha1 签名 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HashCode(string str)
        {

            byte[] StrRes = Encoding.UTF8.GetBytes(str);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();

        }

        public static T GetResultObj<T>(string result)
        {
            byte[] array = Encoding.UTF8.GetBytes(result);

            MemoryStream stream = new MemoryStream(array);

            XmlSerializer xmlSearializer = new XmlSerializer(typeof(T), new XmlRootAttribute("xml"));

            T rr = (T)xmlSearializer.Deserialize(stream);

            return rr;
        }

        






    }
}