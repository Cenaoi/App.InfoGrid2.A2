using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace EC5.SystemBoard.Web
{
    [Obsolete]
    public static class Md5Utility
    {
        public static string ToString(string input,string format)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] bs = md5.ComputeHash(UTF8Encoding.Default.GetBytes(input));
                string str = BitConverter.ToString(bs);

                switch (format)
                {
                    case "N": return str.Replace("-", "");
                    case "D": return str;
                    default:
                        return str;
                }

            }


        }

        public static string ToString(string input)
        {
            return ToString(input, "N");
        }

    }
}
