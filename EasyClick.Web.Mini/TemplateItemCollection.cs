using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 模板项目集合
    /// </summary>
    public class TemplateItemCollection : IEnumerable
    {
        internal Template m_Owner;

        ArrayList m_Items = new ArrayList();

        /// <summary>
        /// 项目索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                return m_Items[index];
            }
            set
            {
                m_Items[index] = value;
            }
        }

        /// <summary>
        /// 模板项目集合的构造方法
        /// </summary>
        public TemplateItemCollection()
        {
        }

        /// <summary>
        /// 模板项目集合的构造方法
        /// </summary>
        /// <param name="owner"></param>
        public TemplateItemCollection(Template owner)
        {
            m_Owner = owner;
        }

        public Template Owner
        {
            get { return m_Owner; }
        }



        /// <summary>
        /// (JScript) 
        /// </summary>
        /// <param name="list"></param>
        public void AddRange(string list)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0}.addItemRange(", m_Owner.GetClientID());

            sb.Append(list);

            sb.Append(");");

            MiniScript.Add(sb.ToString());
        }

        /// <summary>
        /// (JScript) 
        /// </summary>
        /// <param name="list"></param>
        public void AddRange(ICollection list)
        {
            m_Items.AddRange(list);

            if (m_Owner == null || MiniScriptManager.ClientScript.ReadOnly)
            {
                return;
            }

            if (list.Count == 0)
            {
                return;
            }
           

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0}.addItemRange([", m_Owner.GetClientID());

            string[] fields = null;

            if (m_Owner.FilterField)
            {
                fields = m_Owner.GetFieldNames(m_Owner.ItemTemplate);
            }
            

            IEnumerator listEnum = list.GetEnumerator();

            if (listEnum.MoveNext())
            {
                sb.Append(m_Owner.GetItemJson(listEnum.Current,m_Owner.DataFormats ,fields));
            }

            while (listEnum.MoveNext())
            {
                sb.AppendLine(",");
                sb.Append(m_Owner.GetItemJson(listEnum.Current,m_Owner.DataFormats , fields));
            }

            sb.Append("]);");

            MiniScript.Add(sb.ToString());
        }

        /// <summary>
        /// (JScript) 
        /// </summary>
        /// <param name="item"></param>
        public void Add(object item)
        {
            m_Items.Add(item);


            if (m_Owner == null || MiniScriptManager.ClientScript.ReadOnly)
            {
                return;
            }


            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0}.addItem(", m_Owner.GetClientID());

            string[] fields = null;

            if (m_Owner.FilterField)
            {
                fields = m_Owner.GetFieldNames(m_Owner.ItemTemplate);
            }

            sb.Append(m_Owner.GetItemJson(item, m_Owner.DataFormats, fields));

            sb.Append(");");

            MiniScript.Add(sb.ToString());
        }


        public void Insert(int index, object item)
        {
            m_Items.Insert(index, item);

            if (m_Owner == null)
            {
                return;
            }
        }

        public void RemoveAt(int index)
        {
            m_Items.RemoveAt(index);

            if (m_Owner == null)
            {
                return;
            }
        }

        /// <summary>
        /// 只支持客户端 JS
        /// </summary>
        /// <param name="guid"></param>
        public void RemoveAtGuid(int guid)
        {
            if (m_Owner == null)
            {
                return;
            }

            MiniScript.Add("{0}.remoteAtGuid({1});", m_Owner.GetClientID(), guid);
        }

        /// <summary>
        /// 只支持客户端 JS
        /// </summary>
        /// <param name="guids"></param>
        public void RemoveAtGuids(int[] guids)
        {
            if (m_Owner == null || guids.Length == 0)
            {
                return;
            }

            for (int i = 0; i < guids.Length; i++)
            {
                MiniScript.Add("{0}.remoteAtGuid({1});", m_Owner.GetClientID(), guids[i]);
            }
            
        }


        /// <summary>
        /// (JScript) 
        /// </summary>
        public void Clear()
        {
            m_Items.Clear();

            if (m_Owner == null)
            {
                return;
            }

            MiniScript.Add("{0}.clear();", m_Owner.GetClientID());

        }

        public int Count
        {
            get { return m_Items.Count; }
        }



        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }


    }
}
