using System;
using System.Collections.Generic;
using System.Text;
using EasyClick.Web.Mini;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// Table 控件的列集合
    /// </summary>
    public class TableColumnCollection :List<BoundField>
    {
        System.Web.UI.Control m_Owner;

        int m_CreateIndex = 0;

        SortedList<string, BoundField> m_Sorted;


        /// <summary>
        /// (构造函数)Table 控件的列集合
        /// </summary>
        /// <param name="owner">对应所属 Table </param>
        public TableColumnCollection(System.Web.UI.Control owner)
        {
            m_Owner = owner;
            
        }

        private void AddSorted( BoundField item)
        {
            if (m_Sorted == null)
            {
                m_Sorted = new SortedList<string, BoundField>();
            }

            m_Sorted[item.DataField] = item;
        }

        /// <summary>
        /// 按列的 DataField 属性获取字段
        /// </summary>
        /// <param name="dataField">字段名称</param>
        /// <returns>列控件</returns>
        public BoundField FindByDataField(string dataField)
        {
            BoundField field = null;

            if (m_Sorted != null && m_Sorted.TryGetValue(dataField,out field))
            {
                return field;
            }

            for (int i = m_CreateIndex; i < base.Count; i++)
            {
                BoundField item = base[i];

                m_CreateIndex = i + 1;

                if (string.IsNullOrEmpty(item.DataField))
                {
                    continue;
                }

                AddSorted(item);

                if (dataField.Equals(item.DataField))
                {
                    field = item;
                    break;
                }
            }

            return field;
        }

        /// <summary>
        /// 按列的 ID 属性获取字段
        /// </summary>
        /// <param name="id">id 属性</param>
        /// <returns></returns>
        public BoundField GetInputItemByID(string id)
        {
            BoundField field = null;

            for (int i = 0; i < base.Count; i++)
            {
                BoundField item = base[i];

                if ( (item is IMiniInputField) &&
                    id.Equals(((IMiniInputField)item).ID))
                {
                    field = item;
                    break;
                }
            }

            return field;
        }

        /// <summary>
        /// 按可输入框的 Name 属性获取列
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BoundField GetInputItemByName(string name)
        {
            BoundField field = null;

            for (int i = 0; i < base.Count; i++)
            {
                BoundField item = base[i];

                if ((item is IMiniInputField) &&
                    name.Equals(((IMiniInputField)item).Name))
                {
                    field = item;
                    break;
                }
            }

            return field;
        }


        

        /// <summary>
        /// 所属父控件。一般情况下是 Table 对象
        /// </summary>
        public System.Web.UI.Control Owner
        {
            get { return m_Owner; }
            internal set { m_Owner = value; }
        }

        /// <summary>
        /// 添加列对象
        /// </summary>
        /// <param name="item">列对象</param>
        public new void Add(BoundField item)
        {
            item.m_Owner = m_Owner;

            item.SetIndex(this.Count);

            base.Add(item);
        }

        /// <summary>
        /// 插入列对象
        /// </summary>
        /// <param name="index">位置索引</param>
        /// <param name="item">列对象</param>
        public new void Insert(int index, BoundField item)
        {
            item.m_Owner = m_Owner;

            base.Insert(index, item);

            for (int i = 0; i < this.Count; i++)
            {
                base[i].SetIndex(i);
            }
        }

        /// <summary>
        /// 插入列对象集合
        /// </summary>
        /// <param name="index">位置索引</param>
        /// <param name="collection">列对象集合</param>
        public new void InsertRange(int index, IEnumerable<BoundField> collection)
        {
            foreach (BoundField item in collection)
            {
                item.m_Owner = m_Owner;
            }

            base.InsertRange(index, collection);

            for (int i = 0; i < this.Count; i++)
            {
                base[i].SetIndex(i);
            }

        }
    }
}
