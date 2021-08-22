using App.BizCommon;
using App.InfoGrid2.Model.DataSet;
using HWQ.Entity.Decipher.LightDecipher;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.InfoGrid2.Bll
{
    
    /// <summary>
    /// 表缓冲管理
    /// </summary>
    public static class TableBufferMgr
    {
        static BufferItemCollection m_Tables = new BufferItemCollection();

        static BufferItemCollection m_TableForIDs = new BufferItemCollection();

        public static void Remove(string table)
        {
            m_Tables.Remove(table);
        }

        public static void Remove(int tableId)
        {
            m_TableForIDs.Remove(tableId.ToString());
        }


        public static TableSet GetTable(int tableId)
        {
            string id = tableId.ToString();

            TableSet tSet = m_TableForIDs.Get(id) as TableSet;

            if (tSet == null)
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                tSet = TableSet.SelectSID_0_5(decipher, tableId);

                m_TableForIDs.Set(id, tSet);
            }


            return tSet;
        }

        public static TableSet GetTable(string table)
        {
            TableSet tSet = m_Tables.Get(table) as TableSet;

            if (tSet == null)
            {
                tSet = TableMgr.GetTableSet(table);

                m_Tables.Set(table, tSet);
            }


            return tSet;
        }

        public static void SetTable(string table, TableSet tableSet)
        {
            m_Tables.Set(table, tableSet);
        }
    }





    /// <summary>
    /// 缓冲集合
    /// </summary>
    public class BufferItemCollection
    {
        object m_LockObj = new object();

        Dictionary<string, BufferItem> m_Items = new Dictionary<string, BufferItem>();

        public object Get(string key)
        {
            BufferItem item = null;

            lock (m_LockObj)
            {
                if (!m_Items.TryGetValue(key, out item))
                {
                    return null;
                }

                DateTime now = DateTime.Now;

                TimeSpan span = now - item.LastTimeHit;

                if (span.TotalMilliseconds > item.Overtime)
                {
                    m_Items.Remove(key);

                    return null;
                }

                item.LastTimeHit = now;

                return item.Data;
            }
        }

        public void Set(string key, object data)
        {

            BufferItem item = null;

            lock (m_LockObj)
            {
                if (!m_Items.TryGetValue(key, out item))
                {
                    item = new BufferItem();
                    m_Items.Add(key, item);
                }

                item.Data = data;
                item.TimeStart = item.LastTimeHit = DateTime.Now;
            }
        }

        public bool Remove(string key)
        {
            lock (m_LockObj)
            {
                return m_Items.Remove(key);
            }
        }

        public void Clear()
        {
            lock (m_LockObj)
            {
                m_Items.Clear();
            }
        }

    }


    /// <summary>
    /// 缓冲对象
    /// </summary>
    public class BufferItem
    {
        /// <summary>
        /// 缓冲开始时间
        /// </summary>
        DateTime m_TimeStart;

        /// <summary>
        /// 最后访问时间
        /// </summary>
        DateTime m_LastTimeHit;

        /// <summary>
        /// 超时时间(毫秒)
        /// </summary>
        int m_Overtime = 900000;    //15分钟 * 60 * 1000;

        /// <summary>
        /// 缓冲对象
        /// </summary>
        object m_Data;

        string m_Key;


        public DateTime LastTimeHit
        {
            get { return m_LastTimeHit; }
            set { m_LastTimeHit = value; }
        }

        public DateTime TimeStart
        {
            get { return m_TimeStart; }
            set { m_TimeStart = value; }
        }

        public int Overtime
        {
            get { return m_Overtime; }
            set { m_Overtime = value; }
        }

        /// <summary>
        /// 键值
        /// </summary>
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

        /// <summary>
        /// 缓冲对象
        /// </summary>
        public object Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }



    }
}
