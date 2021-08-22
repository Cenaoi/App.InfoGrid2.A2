using System;
using System.Collections.Generic;
using System.Web;
using System.Collections.Specialized;
using System.Text;

namespace EasyClick.Web.Mini.Utility
{
    /// <summary>
    /// Request ��������
    /// </summary>
    static partial class WebUtil
    {
        /// <summary>
        /// ��ȡ��ѯ���ַ���
        /// </summary>
        /// <param name="nameValueCollection"></param>
        /// <returns></returns>
        public static string ToQueryString(NameValueCollection nameValueCollection)
        {
            NameValueCollection nv = nameValueCollection;

            StringBuilder sb = new StringBuilder();

            string key = nv.Keys[0];
            string v = nv[key];

            sb.AppendFormat("{0}={1}", key, v);

            for (int i = 1; i < nv.Count; i++)
            {
                key = nv.Keys[i];
                v = nv[key];

                sb.AppendFormat("&{0}={1}", key, v);
            }

            return sb.ToString();
        }

    }

}
