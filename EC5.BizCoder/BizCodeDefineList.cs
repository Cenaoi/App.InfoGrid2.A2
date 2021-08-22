using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.BizCoder
{
    /// <summary>
    /// 编码集合
    /// </summary>
    public class BizCodeDefineList
    {
        object m_Locker = new object();

        SortedDictionary<string, BizCodeDefine> m_Items = new SortedDictionary<string, BizCodeDefine>();

        public void Add(BizCodeDefine item)
        {
            string key = item.TCode.ToUpper();

            lock (m_Locker)
            {
                if (m_Items.ContainsKey(key))
                {
                    throw new Exception(string.Format("这个“{0}”编码定义已经存在.", item.TCode));
                }
                m_Items.Add(key, item);
            }
        }

        public BizCodeDefine GetItem(string tCode)
        {

            string key = tCode.ToUpper();

            BizCodeDefine item;

            lock (m_Locker)
            {
                if (!m_Items.TryGetValue(key, out item))
                {
                    throw new Exception(string.Format("这个“{0}”编码定义不存在.", item.TCode));
                }

                return item;
            }
        }

        public void Clear()
        {
            lock (m_Locker)
            {
                m_Items.Clear();
            }
        }



    }
}
