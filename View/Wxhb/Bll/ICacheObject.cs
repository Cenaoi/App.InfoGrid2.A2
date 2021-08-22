using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.Wxhb.Bll
{

    /// <summary>
    /// 缓冲对象
    /// </summary>
    public class ICacheObject
    {
        /// <summary>
        /// 缓冲的 key
        /// </summary>
        string CacheKey { get; set; }
    }


    /// <summary>
    /// 缓冲的对象
    /// </summary>
    public class CacheObject : IDisposable
    {
        /// <summary>
        /// 销毁时间
        /// </summary>
        public DateTime DisposeTime { get; set; }

        /// <summary>
        /// 元素对象
        /// </summary>
        public object Item { get; set; }

        public CacheObject()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposeTime">释放的时间</param>
        /// <param name="item"></param>
        public CacheObject(DateTime disposeTime, object item)
        {
            this.DisposeTime = disposeTime;
            this.Item = item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="milliseconds">超时的毫秒数</param>
        /// <param name="item"></param>
        public CacheObject(int milliseconds, object item)
        {
            this.DisposeTime = DateTime.Now.AddMilliseconds(milliseconds);

            this.Item = item;
        }

        bool m_IsDisposed = false;

        public void Dispose()
        {
            if (m_IsDisposed)
            {
                m_IsDisposed = true;

                this.Item = null;

                GC.SuppressFinalize(this);
            }
        }
    }



    /// <summary>
    /// 缓冲对象
    /// </summary>
    public class Cache<KeyT> : IDisposable
    {
        ConcurrentDictionary<KeyT, CacheObject> m_Items = new ConcurrentDictionary<KeyT, CacheObject>();

        bool m_IsDisposed = false;

        /// <summary>
        /// 限制最大数量
        /// </summary>
        int m_MaxCount = 0;

        DateTime m_LastClearTime = DateTime.Now;

        int m_ClearSpan = 1000 * 60;//

        /// <summary>
        /// 最大的缓冲数量
        /// </summary>
        public int MaxCount
        {
            get { return m_MaxCount; }
            set { m_MaxCount = value; }
        }
        
        public int ClearSpan
        {
            get { return m_ClearSpan; }
            set { m_ClearSpan = value; }
        }


        /// <summary>
        /// 清理掉超时的记录
        /// </summary>
        public void ClearOvertime()
        {
            DateTime now = DateTime.Now;
            TimeSpan span = now - m_LastClearTime;

            if(span.TotalMilliseconds < m_ClearSpan)
            {
                return;
            }

            lock (m_Items)
            {
                List<KeyT> deleteKeys = null;

                foreach (var item in m_Items)
                {
                    if (item.Value.DisposeTime >= now)
                    {
                        if (deleteKeys == null)
                        {
                            deleteKeys = new List<KeyT>();
                        }

                        deleteKeys.Add(item.Key);
                    }
                }

                if (deleteKeys != null)
                {
                    CacheObject tmpItem;

                    foreach (var key in deleteKeys)
                    {
                        m_Items.TryRemove(key, out tmpItem);
                    }
                }

            }
        }

        public object Get(KeyT key)
        {
            CacheObject obj = null;

            if (m_Items.TryGetValue(key, out obj))
            {
                if (obj.DisposeTime >= DateTime.Now)
                {
                    m_Items.TryRemove(key, out obj);

                    obj.Dispose();

                    return null;
                }

                return obj;
            }

            return null;
        }

        public bool TryGet(KeyT key, out object item)
        {
            CacheObject obj = null;

            if (m_Items.TryGetValue(key, out obj))
            {
                if (obj.DisposeTime >= DateTime.Now)
                {
                    m_Items.TryRemove(key, out obj);

                    obj.Dispose();

                    item = null;

                    return false;
                }

                item = obj.Item;
                return true;
            }

            item = null;
            return false;
        }

        public bool TryRemove(KeyT key,out object item)
        {
            CacheObject obj = null;


            if (m_Items.TryRemove(key, out obj))
            {
                item = obj.Item;
                obj.Dispose();
                return true;
            }
            else
            {
                item = null;
                return false;
            }   
        }


        public void Set(KeyT key, int milliseconds, object item)
        {
            CacheObject obj = new CacheObject(milliseconds, item);

            CacheObject old = null;

            if (m_Items.TryRemove(key, out old))
            {
                old.Dispose();

                m_Items[key] = obj;
            }
            else
            {
                m_Items.TryAdd(key, obj);
            }

        }

        public void Clear()
        {
            m_Items.Clear();
        }



        public void Dispose()
        {
            if (m_IsDisposed)
            {
                m_IsDisposed = true;

                m_Items.Clear();

                GC.SuppressFinalize(this);
            }
        }
    }

    /// <summary>
    /// 缓冲对象
    /// </summary>
    public class Cache : IDisposable
    {
        ConcurrentDictionary<string, CacheObject> m_Items = new ConcurrentDictionary<string, CacheObject>();

        bool m_IsDisposed = false;

        /// <summary>
        /// 限制最大数量
        /// </summary>
        int m_MaxCount = 0;


        public int MaxCount
        {
            get { return m_MaxCount; }
            set { m_MaxCount = value; }
        }


        /// <summary>
        /// 清理掉超时的记录
        /// </summary>
        public void ClearOvertime()
        {

        }

        public object Get(string key)
        {
            CacheObject obj = null;

            if (m_Items.TryGetValue(key, out obj))
            {
                if (obj.DisposeTime >= DateTime.Now)
                {
                    m_Items.TryRemove(key, out obj);

                    obj.Dispose();

                    return null;
                }

                return obj;
            }

            return null;
        }

        public bool TryGet(string key, out object item)
        {
            CacheObject obj = null;

            if (m_Items.TryGetValue(key, out obj))
            {
                if (obj.DisposeTime >= DateTime.Now)
                {
                    m_Items.TryRemove(key, out obj);

                    obj.Dispose();

                    item = null;

                    return false;
                }

                item = obj.Item;
                return true;
            }

            item = null;
            return false;
        }


        public void Set(string key, int milliseconds, object item)
        {
            CacheObject obj = new CacheObject(milliseconds, item);

            CacheObject old = null;

            if (m_Items.TryRemove(key, out old))
            {
                old.Dispose();

                m_Items[key] = obj;
            }
            else
            {
                m_Items.TryAdd(key, obj);
            }

        }

        public void Clear()
        {
            m_Items.Clear();
        }



        public void Dispose()
        {
            if (m_IsDisposed)
            {
                m_IsDisposed = true;

                m_Items.Clear();

                GC.SuppressFinalize(this);
            }
        }
    }
}