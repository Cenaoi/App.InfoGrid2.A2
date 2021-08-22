using EC5.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{
    /// <summary>
    /// 索引集合
    /// </summary>
    public class ActionList<ItemT>
        where ItemT : CodeIndexItem
    {
        /// <summary>
        /// 根据 Code 作为索引
        /// </summary>
        ConcurrentDictionary<string, ItemT> m_ItemsForCode = new ConcurrentDictionary<string, ItemT>();

        protected ConcurrentDictionary<string, ItemT> Items
        {
            get { return m_ItemsForCode; }
        }

        public ActionList()
        {

        }

        public void Add(ItemT item)
        {
            if (item == null) throw new ArgumentNullException("item 不能为空.");

            m_ItemsForCode.TryAdd(item.Code, item);
        }

        public void Clear()
        {
            m_ItemsForCode.Clear();
        }

        public bool TryGet(string code, out ItemT item)
        {
            if (StringUtil.IsBlank(code)) throw new ArgumentNullException("code 不能为空");

            return m_ItemsForCode.TryGetValue(code, out item);
        }

        public bool ContainsCode(string code)
        {
            if (StringUtil.IsBlank(code)) throw new ArgumentNullException("code 不能为空");

            return m_ItemsForCode.ContainsKey(code);
        }

        public bool Remove(string code)
        {
            if (StringUtil.IsBlank(code)) throw new ArgumentNullException("code 不能为空");

            ItemT item;

            return m_ItemsForCode.TryRemove(code, out item);
        }


        public int Count
        {
            get { return m_ItemsForCode.Count; }
        }


        public List<ItemT> ToList()
        {
            List<ItemT> items = new List<ItemT>(m_ItemsForCode.Values);
            return items;
        }

    }
}
