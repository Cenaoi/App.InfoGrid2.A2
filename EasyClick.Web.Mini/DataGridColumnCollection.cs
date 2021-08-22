using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 添加列的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="field"></param>
    public delegate void DataGridColumnAddedDelegate(object sender,BoundField field);


    /// <summary>
    /// 表格列集合
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public sealed class DataGridColumnCollection : List<BoundField>
    {

        System.Web.UI.Control m_Owner;

        public DataGridColumnCollection(System.Web.UI.Control owner)
        {
            m_Owner = owner;
            
        }

        #region 事件


        internal event DataGridColumnAddedDelegate Added;

        internal void OnAdded(BoundField field)
        {
            if (Added != null) { Added(this, field); }
        }


        #endregion

        /// <summary>
        /// 按列的 DataField 属性获取字段
        /// </summary>
        /// <param name="dataField"></param>
        /// <returns></returns>
        public BoundField FindByDataField(string dataField)
        {
            BoundField field = null;

            for (int i = 0; i < base.Count; i++)
            {
                BoundField item = base[i];

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
        /// <param name="id"></param>
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

            OnAdded(item);
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

            if (Added != null)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    OnAdded(base[i]);
                }
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

            if (Added != null)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    OnAdded(base[i]);
                }
            }

        }
    }

}
