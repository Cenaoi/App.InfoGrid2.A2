using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// Grid 的数据行
    /// </summary>
    public class GridRow
    {
        object m_Data;

        public GridRow()
        {
        }

        public GridRow(object data)
        {
            m_Data = data;
        }

        public object Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }
    }

    /// <summary>
    /// Grid 的数据行集合
    /// </summary>
    public class GridRowCollection:List<GridRow>
    {
        object m_Owner;

        public GridRowCollection(object owner)
        {
            m_Owner = owner;
        }

        
    }

}
