using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.SystemBoard.Web
{
    [Obsolete]
    public static class JsonUtility
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

            string jsonStr = vStr.Replace("\"", "\\\"")
                .Replace("\n", @"\n")
                .Replace("\r", @"\r")
                .Replace("\t", @"\t");

            return jsonStr;
        }



    }
}
