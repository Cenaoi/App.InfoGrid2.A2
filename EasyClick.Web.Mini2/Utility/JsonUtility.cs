using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2.Utility
{
    static class JsonUtil
    {
        public static string ToJson(object v)
        {
            if (v == null)
            {
                return string.Empty;
            }

            string vStr = v.ToString();

            if (vStr.Length == 0)
            {
                return string.Empty;
            }

            string jsonStr = ToJsonString(vStr);

            return jsonStr;
        }


        static string ToJsonString(string s)
        {

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {

                char c = s[i];

                switch (c)
                {

                    case '\"': sb.Append("\\\""); break;

                    case '\\': sb.Append("\\\\"); break;

                    case '/': sb.Append("\\/"); break;

                    case '\b': sb.Append("\\b"); break;

                    case '\f': sb.Append("\\f"); break;

                    case '\n': sb.Append("\\n"); break;

                    case '\r': sb.Append("\\r"); break;

                    case '\t': sb.Append("\\t"); break;

                    default: sb.Append(c); break;

                }

            }

            return sb.ToString();

        }

    }
}
