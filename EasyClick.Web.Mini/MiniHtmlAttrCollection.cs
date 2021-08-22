using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini
{
    public class MiniHtmlAttrCollection:IEnumerable<MiniHtmlAttr>
    {

        #region Attribute

        System.Collections.Generic.SortedDictionary<string, MiniHtmlAttr> m_HtmlAttrs = new SortedDictionary<string, MiniHtmlAttr>();

        public MiniHtmlAttr this[string key]
        {
            get
            {
                string keyLower = key.ToLower();

                if (!m_HtmlAttrs.ContainsKey(keyLower))
                {
                    return null;
                }

                return m_HtmlAttrs[key];
            }
        }

        public SortedDictionary<string, MiniHtmlAttr>.KeyCollection Keys
        {
            get
            {
                return m_HtmlAttrs.Keys;
            }
        }

        public SortedDictionary<string, MiniHtmlAttr>.ValueCollection Values
        {
            get
            {
                return m_HtmlAttrs.Values;
            }
        }

        /// <summary>
        /// 是否存在此对应属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsAttr(string key)
        {
            string keyLower = key.ToLower();

            return m_HtmlAttrs.ContainsKey(keyLower);
        }


        public string GetAttribute(string key)
        {
            string keyLower = key.ToLower();

            if (m_HtmlAttrs.ContainsKey(keyLower))
            {
                return m_HtmlAttrs[keyLower].Value;
            }

            return string.Empty;
        }

        public void SetAttribute(string key, string value)
        {
            string keyLower = key.ToLower();

            if (m_HtmlAttrs.ContainsKey(keyLower))
            {
                m_HtmlAttrs[keyLower].Value = value;
            }
            else
            {
                m_HtmlAttrs.Add(keyLower, new MiniHtmlAttr(key, value));
            }
        }

        #endregion


        public bool GetBool(string key, bool defaultValue)
        {
            string vStr = GetAttribute(key);

            bool v;

            if (bool.TryParse(vStr, out v))
            {
                return v;
            }
            else
            {
                return defaultValue;
            }
        }

        public bool GetBool(string key)
        {
            return GetBool(key, false);
        }


        public int GetInt(string key, int defaultValue)
        {
            string vStr = GetAttribute(key);

            int v;

            if (int.TryParse(vStr, out v))
            {
                return v;
            }
            else
            {
                return defaultValue;
            }
        }

        public int GetInt(string key)
        {
            return GetInt(key, 0);
        }

        public Int64 GetInt64(string key, Int64 defaultValue)
        {
            string vStr = GetAttribute(key);

            int v;

            if (int.TryParse(vStr, out v))
            {
                return v;
            }
            else
            {
                return defaultValue;
            }
        }

        public Int64 GetInt64(string key)
        {
            return GetInt64(key, 0);
        }





        #region IEnumerable<MiniHtmlAttr> 成员

        public IEnumerator<MiniHtmlAttr> GetEnumerator()
        {
            return m_HtmlAttrs.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_HtmlAttrs.Values.GetEnumerator();
        }

        #endregion
    }
}
