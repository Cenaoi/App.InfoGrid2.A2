using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.SCodeTemplates
{


    /// <summary>
    /// 脚本工场
    /// </summary>
    public static class SCodeFactory
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 缓冲
        /// </summary>
        static ConcurrentDictionary<int, SCodeTemplate> m_Buffer = new ConcurrentDictionary<int, SCodeTemplate>();

        /// <summary>
        /// 模板解析器
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static SCodeTemplate ParseTemplate(string text)
        {
            int hashCode = text.GetHashCode();

            SCodeTemplate template = null;

            if(m_Buffer.TryGetValue(hashCode, out template))
            {
                return template;
            }



            SCodeItemCollection items = PP(text);

            template = new SCodeTemplate(text);
            template.Items = items;

            if(m_Buffer.TryAdd(hashCode, template))
            {
                log.Error("重复:"+ text);
            }

            return template;
        }


        private static SCodeItemCollection PP(string text)
        {
            SCodeItemCollection items = new SCodeItemCollection();

            int curIndex = 0;

            int n1;
            int n2;

            n1 = text.IndexOf("{{", curIndex);


            //if (n1 > 0)
            //{
            //    char c = text[n1 - 1];

            //    if (c == '\\')
            //    {
            //        n1 = text.IndexOf("{{", curIndex + 1);
            //    }

            //}


            string str;

            while (n1 >= 0)
            {
                if (n1 - curIndex > 0)
                {
                    str = text.Substring(curIndex, n1 - curIndex);
                    items.Add(str);
                }



                n2 = text.IndexOf("}}", n1);

                //if (n2 > 0)
                //{
                //    char c = text[n2 - 1];

                //    if (c == '\\')
                //    {
                //        n2 = text.IndexOf("}}", n2 + 1);
                //    }
                //}

                int len = n2 - n1 - 2;
                str = text.Substring(n1 + 2, len);
                items.AddCode(str);

                curIndex = n2 + 2;



                n1 = text.IndexOf("{{", curIndex);


                //if (n1 > 0)
                //{
                //    char c = text[n1 - 1];

                //    if (c == '\\')
                //    {
                //        n1 = text.IndexOf("{{", curIndex + 1);
                //    }

                //}

            }

            if (text.Length - curIndex > 0)
            {
                str = text.Substring(curIndex, text.Length - curIndex);
                items.Add(str);
            }


            return items;
        }


    }
}
