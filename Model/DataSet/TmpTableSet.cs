using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System.Collections;

namespace App.InfoGrid2.Model.DataSet
{
    public class TmpTableSet
    {
        IG2_TMP_TABLE m_Table;

        List<IG2_TMP_TABLECOL> m_Cols;

        /// <summary>
        /// 表
        /// </summary>
        public IG2_TMP_TABLE Table
        {
            get { return m_Table; }
            set { m_Table = value; }
        }

        /// <summary>
        /// 字段集
        /// </summary>
        public List<IG2_TMP_TABLECOL> Cols
        {
            get { return m_Cols; }
            set { m_Cols = value; }
        }




        public static TmpTableSet Select(DbDecipher decipher, Guid tempGuid)
        {

            LightModelFilter filterTmpTable = new LightModelFilter(typeof(IG2_TMP_TABLE));
            filterTmpTable.And("TMP_GUID", tempGuid);

            LightModelFilter filterTmpCol = new LightModelFilter(typeof(IG2_TMP_TABLECOL));
            filterTmpCol.And("TMP_GUID", tempGuid);
                        
            var table = decipher.SelectToOneModel<IG2_TMP_TABLE>(filterTmpTable);

            if(table == null)
            {
                return null;
            }

            TmpTableSet ts = new TmpTableSet();
            ts.Table = table;
            
            ts.m_Cols = decipher.SelectModels<IG2_TMP_TABLECOL>(filterTmpCol);

            return ts;
        }

        public static void Delete(DbDecipher decipher, Guid tempGuid)
        {
            LightModelFilter filterTmpTable = new LightModelFilter(typeof(IG2_TMP_TABLE));
            filterTmpTable.And("TMP_GUID", tempGuid);

            LightModelFilter filterTmpCol = new LightModelFilter(typeof(IG2_TMP_TABLECOL));
            filterTmpCol.And("TMP_GUID", tempGuid);


            decipher.DeleteModels(filterTmpTable);
            decipher.DeleteModels(filterTmpCol);

        }

        public void Insert(DbDecipher decipher, Guid tempGuid,string sessionId)
        {
            m_Table.TMP_GUID = tempGuid;
            m_Table.TMP_SESSION_ID = sessionId;
            m_Table.TMP_OP_ID="A";

            decipher.InsertModel(m_Table);

            foreach (IG2_TMP_TABLECOL col in m_Cols)
            {
                col.TABLE_UID = m_Table.TABLE_UID;
                col.TMP_GUID = tempGuid;
                col.TMP_OP_ID = "A";
                col.TMP_SESSION_ID = sessionId;
            }

            decipher.InsertModels<IG2_TMP_TABLECOL>(m_Cols);


        }



        public TableSet ToTableSet()
        {
            TableSet tSet = new TableSet();

            tSet.Table = new IG2_TABLE();

            m_Table.CopyTo(tSet.Table, true);


            tSet.Cols = new List<IG2_TABLE_COL>();

            foreach (IG2_TMP_TABLECOL tmpCol in m_Cols)
            {
                IG2_TABLE_COL col = new IG2_TABLE_COL();

                tmpCol.CopyTo(col, true);

                tSet.Cols.Add(col);
            }

            return tSet;
        }

    }
}
