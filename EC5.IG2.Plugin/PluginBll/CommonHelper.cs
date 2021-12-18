using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EC5.IG2.Plugin.PluginBll
{
    public class CommonHelper
    {
        public static string CuttingSz(string str)
        {
            //取出字符串中所有的数字
            string res = Regex.Replace(str, "[a-z]", "", RegexOptions.IgnoreCase);

            return res;
        }

        public static string CuttingZm(string str)
        {
            //取出字符串中所有的英文字母      
            string res = Regex.Replace(str, "[0-9]", "", RegexOptions.IgnoreCase);

            return res;
        }

        public static List<string> GetFieldNameByString(string str)
        {
            List<string> list = new List<string>();

            if (string.IsNullOrWhiteSpace(str))
            {
                return list;
            }

            while (true)
            {
                int sindex = str.IndexOf("{");

                if (sindex == -1)
                {
                    break;
                }

                int eindex = str.IndexOf("}");

                if (eindex == -1)
                {
                    break;
                }

                sindex++;

                string fitem = str.Substring(sindex, eindex - sindex);

                list.Add(fitem);

                eindex++;

                str = str.Substring(eindex);
            }

            return list;
        }


    }
}
