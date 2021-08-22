using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.BizWeb2.Hit
{
    /// <summary>
    /// 命中条目
    /// </summary>
    public class HitItem
    {

        DateTime m_Time;


        public DateTime Time
        {
            get { return m_Time; }
            set { m_Time = value; }
        }
    }


    /// <summary>
    /// 命中集合
    /// </summary>
    public class HitItemCollection:IDisposable,IEnumerable<HitItem>
    {
        List<HitItem> m_Items = new List<HitItem>();

        object m_Locked = new object();

        public HitItem this[int index]
        {
            get
            {
                lock (m_Locked)
                {
                    return m_Items[index];
                }
            }
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public void Add(HitItem item)
        {
            lock (m_Locked)
            {
                if (m_Items.Count > 50)
                {
                    m_Items.RemoveAt(50);
                }

                m_Items.Add(item);
            }
        }

        public void Clear()
        {
            lock (m_Locked)
            {
                m_Items.Clear();
            }
        }



        public void Dispose()
        {
            m_Items.Clear();
            m_Items = null;
        }

        public IEnumerator<HitItem> GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }
    }


    /// <summary>
    /// 页面集合,用于命中的
    /// </summary>
    public class HitPageCollection:IDisposable
    {
        object m_Locked = new object();

        SortedDictionary<string, HitPage> m_Pages = new SortedDictionary<string, HitPage>();

        /// <summary>
        /// 命中页面
        /// </summary>
        /// <param name="uri"></param>
        public void HitPage(string uri)
        {
            string uriUpper = uri.ToUpper();

            HitPage page = null;

            lock (m_Locked)
            {
                if (!m_Pages.TryGetValue(uriUpper, out page))
                {
                    page = new HitPage();
                    page.Uri = uri;

                    m_Pages.Add(uriUpper, page);
                }
            }

            page.Hit();
        }

        public bool IsEndlessLoop(string uri)
        {
            string uriUpper = uri.ToUpper();


            HitPage page = null;

            lock (m_Locked)
            {
                if (!m_Pages.TryGetValue(uriUpper, out page))
                {
                    return false;
                }
            }

            return page.IsEndlessLoop();
        }


        public void Dispose()
        {
            foreach (var item in m_Pages.Values)
            {
                item.Dispose();
            }

            m_Pages.Clear();

            m_Pages = null;

            m_Locked = null;
        }

        public void Clear(string uri)
        {
            lock (m_Locked)
            {
                m_Pages.Clear();
            }
        }
    }

    /// <summary>
    /// 页面命中
    /// </summary>
    public class HitPage:IDisposable
    {
        string m_Uri;


        HitItemCollection m_Items = new HitItemCollection();

        /// <summary>
        /// 页面 Uri
        /// </summary>
        public string Uri
        {
            get { return m_Uri; }
            set { m_Uri = value; }
        }

        /// <summary>
        /// 命中的条目集合
        /// </summary>
        public HitItemCollection Items
        {
            get { return m_Items; }
        }


        /// <summary>
        /// 命中
        /// </summary>
        public void Hit()
        {
            HitItem item = new HitItem();
            item.Time = DateTime.Now;

            m_Items.Add(item);
        }


        /// <summary>
        /// 是否死循环
        /// </summary>
        /// <returns></returns>
        public bool IsEndlessLoop()
        {
            if (m_Items.Count < 5)
            {
                return false;
            }

            int n1 = m_Items.Count - 1;

            int n2 = n1 - 50;

            if (n2 < 0)
            {
                n2 = 0;
            }

            HitItem item1 = null, item2 = null;

            int mm = 0;

            int i = 0;

            for (int j = n1; j >= n2; j--)
            {
                item2 = m_Items[j];

                if (i++ > 0)
                {
                    TimeSpan span = item2.Time - item1.Time;

                    if (span.TotalMilliseconds < 500)
                    {
                        mm++;
                    }

                    if (mm > 20)
                    {
                        break;
                    }
                }

                if (item1 == null)
                {
                    item1 = item2;
                }
            }


            return (mm >= 20);

        }




        public void Dispose()
        {
            m_Items.Dispose();
            m_Items = null;
        }
    }

}
