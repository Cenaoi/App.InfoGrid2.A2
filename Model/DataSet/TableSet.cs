using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;

namespace App.InfoGrid2.Model.DataSet
{
    /// <summary>
    /// 表数据集
    /// </summary>
    public class TableSet
    {

        IG2_TABLE m_Table;

        List<IG2_TABLE_COL> m_Cols;

        /// <summary>
        /// 视图字段，作为缓冲用
        /// </summary>
        List<IG2_TABLE_COL> m_BufferVCols;

        SortedDictionary<string, TColItem> m_ColIndex = null;

        /// <summary>
        /// 表
        /// </summary>
        public IG2_TABLE Table
        {
            get { return m_Table; }
            set { m_Table = value; }
        }

        /// <summary>
        /// 字段集
        /// </summary>
        public List<IG2_TABLE_COL> Cols
        {
            get { return m_Cols; }
            set { m_Cols = value; }
        }

        /// <summary>
        /// 获取视图字段
        /// </summary>
        /// <returns></returns>
        public List<IG2_TABLE_COL> GetVCols()
        {
            if (m_BufferVCols == null)
            {
                m_BufferVCols = new List<IG2_TABLE_COL>();

                foreach (var col in m_Cols)
                {
                    if (!col.IS_VIEW_FIELD)
                    {
                        continue;
                    }

                    m_BufferVCols.Add(col);
                }
            }

            return m_BufferVCols;
        }


        class TColItem
        {
            public IG2_TABLE_COL Item;

            /// <summary>
            /// 原来的位置
            /// </summary>
            public int SrcIndex;
        }

        /// <summary>
        /// 根据字段名获取字段
        /// </summary>
        /// <param name="dbField"></param>
        /// <returns></returns>
        public IG2_TABLE_COL Find(string dbField, out int srcIndex)
        {
            if (m_ColIndex == null)
            {
                m_ColIndex = new SortedDictionary<string, TColItem>();

                int i = 0;

                foreach (var item in m_Cols)
                {
                    if (!m_ColIndex.ContainsKey(item.DB_FIELD))
                    {

                        m_ColIndex.Add(item.DB_FIELD, new TColItem { Item = item, SrcIndex = i });
                    }

                    i++;
                }
            }

            IG2_TABLE_COL col = null;

            TColItem tcItem = null;

            if (m_ColIndex.TryGetValue(dbField, out tcItem))
            {
                col = tcItem.Item;

                srcIndex = tcItem.SrcIndex;
            }
            else
            {
                srcIndex = -1;
            }

            return col;
        }


        /// <summary>
        /// 选择记录,SID ≥ 0, SEC_LEVEL≤6
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="tableId"></param>
        public static TableSet SelectSID_0_5(DbDecipher decipher, int tableId)
        {
            LightModelFilter tableFilter = new LightModelFilter(typeof(IG2_TABLE));
            tableFilter.And("IG2_TABLE_ID", tableId);
            tableFilter.Locks.Add(LockType.NoLock);

            LightModelFilter colsFilter = new LightModelFilter(typeof(IG2_TABLE_COL));
            colsFilter.And("IG2_TABLE_ID", tableId);
            colsFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            colsFilter.And("SEC_LEVEL", 6, Logic.LessThan);
            colsFilter.Locks.Add(LockType.NoLock);

            colsFilter.TSqlOrderBy = "FORM_FIELD_SEQ, FIELD_SEQ ,IG2_TABLE_COL_ID ";

            
            var table = decipher.SelectToOneModel<IG2_TABLE>(tableFilter);

            if(table == null)
            {
                return null;
            }

            TableSet ts = new TableSet();
            ts.Table = table;
            ts.m_Cols = decipher.SelectModels<IG2_TABLE_COL>(colsFilter);
            ts.m_ColIndex = null;

            return ts;
        }

        public static TableSet SelectByPk(DbDecipher decipher, int tableId)
        {
            TableSet ts =  TableSet.Select(decipher, tableId);
            
            return ts;
        }

        /// <summary>
        /// 选择记录
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="tableId"></param>
        public static TableSet Select(DbDecipher decipher, int tableId)
        {

            LightModelFilter tableFilter = new LightModelFilter(typeof(IG2_TABLE));
            tableFilter.And("IG2_TABLE_ID", tableId);
            tableFilter.Locks.Add(LockType.NoLock);

            LightModelFilter colsFilter = new LightModelFilter(typeof(IG2_TABLE_COL));
            colsFilter.And("IG2_TABLE_ID", tableId);
            colsFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            colsFilter.TSqlOrderBy = "FORM_FIELD_SEQ, FIELD_SEQ ,IG2_TABLE_COL_ID ";
            colsFilter.Locks.Add(LockType.NoLock);

            IG2_TABLE table = decipher.SelectToOneModel<IG2_TABLE>(tableFilter);

            if (table == null)
            {
                return null;
            }

            TableSet ts = new TableSet();

            var cols = decipher.SelectModels<IG2_TABLE_COL>(colsFilter);

            ts.Table = table;
            ts.Cols = cols;
            ts.m_ColIndex = null;

            return ts;
        }


        /// <summary>
        /// 选择记录
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="tableId"></param>
        public static TableSet Select(DbDecipher decipher, string tableName)
        {

            LightModelFilter tableFilter = new LightModelFilter(typeof(IG2_TABLE));
            tableFilter.And("TABLE_TYPE_ID", "TABLE");
            tableFilter.And("TABLE_NAME", tableName);
            tableFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            tableFilter.Locks.Add(LockType.NoLock);

            var table = decipher.SelectToOneModel<IG2_TABLE>(tableFilter);

            if (table == null)
            {
                return null;
            }

            LightModelFilter colsFilter = new LightModelFilter(typeof(IG2_TABLE_COL));
            colsFilter.And("IG2_TABLE_ID", table.IG2_TABLE_ID);
            colsFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            colsFilter.TSqlOrderBy = "FORM_FIELD_SEQ, FIELD_SEQ ,IG2_TABLE_COL_ID ";
            colsFilter.Locks.Add(LockType.NoLock);

            var cols  = decipher.SelectModels<IG2_TABLE_COL>(colsFilter);

            TableSet ts = new TableSet();
            ts.Table = table;
            ts.Cols = cols;
            ts.m_ColIndex = null;

            return ts;
        }



        public TmpTableSet ToTmpTableSet()
        {
            TmpTableSet ttSet = new TmpTableSet();

            ttSet.Table = new IG2_TMP_TABLE();

            m_Table.CopyTo(ttSet.Table, true);


            ttSet.Cols = new List<IG2_TMP_TABLECOL>();

            foreach (IG2_TABLE_COL col in m_Cols)
            {
                IG2_TMP_TABLECOL tmpCol = new IG2_TMP_TABLECOL();

                col.CopyTo(tmpCol, true);

                ttSet.Cols.Add(tmpCol);
            }

            return ttSet;
        }


        public void Insert(DbDecipher decipher)
        {


            decipher.InsertModel(m_Table);

            foreach (IG2_TABLE_COL col in m_Cols)
            {
                
                col.TABLE_UID = m_Table.TABLE_UID;
                col.IG2_TABLE_ID = m_Table.IG2_TABLE_ID;
            }

            decipher.InsertModels<IG2_TABLE_COL>(m_Cols);

        }


        /// <summary>
        /// 删除回收模式
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="ownerTableId"></param>
        /// <param name="ownerColId"></param>
        public void DeleteRecycle(DbDecipher decipher, int ownerTableId, string ownerColId)
        {
            LightModelFilter filterTable = new LightModelFilter(typeof(IG2_TABLE));
            filterTable.And("TABLE_TYPE_ID", "VIEW");
            filterTable.And("VIEW_OWNER_TABLE_ID", ownerTableId);
            filterTable.And("VIEW_OWNER_COL_ID", ownerColId);
            filterTable.Locks.Add(LockType.NoLock);


            IG2_TABLE table = decipher.SelectToOneModel<IG2_TABLE>(filterTable);

            if (table == null)
            {
                return;
            }

            LightModelFilter filterCol = new LightModelFilter(typeof(IG2_TABLE_COL));
            filterCol.And("IG2_TABLE_ID", table.IG2_TABLE_ID);
            filterCol.Locks.Add(LockType.NoLock);

            table.ROW_SID = -3;
            table.ROW_DATE_DELETE = DateTime.Now;

            int n = decipher.UpdateModelProps(table, "ROW_SID", "ROW_DATE_DELETE");
            decipher.UpdateProps(filterCol, new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now });

        }

    }
}
