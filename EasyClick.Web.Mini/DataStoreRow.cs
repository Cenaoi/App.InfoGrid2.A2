using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using EasyClick.Web.Mini.Utility;

namespace EasyClick.Web.Mini
{


    /// <summary>
    /// 数据仓库记录行
    /// </summary>
    public class DataStoreRow
    {
        DataStoreRowState m_State;

        string m_Pk;

        DataStoreCellCollection m_Cells;
        DataStoreCellCollection m_FixedFields;

        int m_RowIndex = 0;


        public int RowIndex
        {
            get { return m_RowIndex; }
            set { m_RowIndex = value; }
        }

        public DataStoreRowState State
        {
            get { return m_State; }
            set { m_State = value; }
        }

        public string Pk
        {
            get { return m_Pk; }
            set { m_Pk = value; }
        }

        public bool HasCells()
        {
            return (m_Cells != null && m_Cells.Count > 0);
        }

        public bool HasFixedFields()
        {
            return (m_FixedFields != null && m_FixedFields.Count > 0);
        }

        public DataStoreCellCollection Cells
        {
            get
            {
                if (m_Cells == null)
                {
                    m_Cells = new DataStoreCellCollection();
                }
                return m_Cells;
            }
        }

        public DataStoreCellCollection FixedFields
        {
            get
            {
                if (m_FixedFields == null)
                {
                    m_FixedFields = new DataStoreCellCollection();
                }
                return m_FixedFields;
            }
        }

        public void FillObject(object obj)
        {

            Type objT = obj.GetType();

            if (m_Cells != null)
            {
                foreach (DataStoreCell cell in m_Cells)
                {
                    PropertyInfo propInfo = objT.GetProperty(cell.Name);

                    if (propInfo == null || !propInfo.CanWrite) { continue; }

                    object targetValue = StringUtility.ChangeType(cell.Value, propInfo.PropertyType);

                    propInfo.SetValue(obj, targetValue, null);
                }
            }

            if (m_FixedFields != null)
            {
                foreach (DataStoreCell cell in m_FixedFields)
                {
                    PropertyInfo propInfo = objT.GetProperty(cell.Name);

                    if (propInfo == null || !propInfo.CanWrite) { continue; }

                    object targetValue = StringUtility.ChangeType(cell.Value, propInfo.PropertyType);

                    propInfo.SetValue(obj, targetValue, null);
                }
            }

        }


    }

}
