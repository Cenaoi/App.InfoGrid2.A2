using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;

namespace App.InfoGrid2.Model.DataSet
{
    public class MapSet
    {
        IG2_MAP m_Map;

        List<IG2_MAP_COL> m_Cols;

        public IG2_MAP Map
        {
            get { return m_Map; }
            set { m_Map = value; }
        }

        public List<IG2_MAP_COL> Cols
        {
            get { return m_Cols; }
            set { m_Cols = value; }
        }


        /// <summary>
        /// 选择记录,SID ≥ 0
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="mapId"></param>
        public static MapSet SelectSID_0(DbDecipher decipher, int mapId)
        {
            LightModelFilter tableFilter = new LightModelFilter(typeof(IG2_MAP));
            tableFilter.And("IG2_MAP_ID", mapId);
            tableFilter.Locks.Add(LockType.NoLock);

            LightModelFilter colsFilter = new LightModelFilter(typeof(IG2_MAP_COL));
            colsFilter.And("IG2_MAP_ID", mapId);
            colsFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            colsFilter.Locks.Add(LockType.NoLock);


            colsFilter.TSqlOrderBy = "ROW_USER_SEQ ASC,IG2_MAP_COL_ID ASC";

            MapSet ms = new MapSet();

            ms.m_Map = decipher.SelectToOneModel<IG2_MAP>(tableFilter);

            if(ms.m_Map == null)
            {
                return null;
            }

            ms.m_Cols = decipher.SelectModels<IG2_MAP_COL>(colsFilter);


            return ms;
        }


        /// <summary>
        /// 选择记录,SID ≥ 0
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="mapId"></param>
        /// <param name="leftModel">目标实体</param>
        /// <param name="leftModel">源实体</param>
        public static MapSet SelectSID_0(DbDecipher decipher, int mapId,string leftTable,string rightTable)
        {
            LightModelFilter tableFilter = new LightModelFilter(typeof(IG2_MAP));
            tableFilter.And("IG2_MAP_ID", mapId);
            tableFilter.And("L_TABLE", leftTable);
            tableFilter.And("R_TABLE", rightTable);
            tableFilter.Locks.Add(LockType.NoLock);

            var m_Map = decipher.SelectToOneModel<IG2_MAP>(tableFilter);

            if (m_Map == null)
            {
                return null;
            }

            LightModelFilter colsFilter = new LightModelFilter(typeof(IG2_MAP_COL));
            colsFilter.And("IG2_MAP_ID", mapId);
            colsFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            colsFilter.Locks.Add(LockType.NoLock);


            colsFilter.TSqlOrderBy = "ROW_USER_SEQ ASC,IG2_MAP_COL_ID ASC";

            var m_Cols = decipher.SelectModels<IG2_MAP_COL>(colsFilter);

            return new MapSet() { m_Map = m_Map, m_Cols = m_Cols };
        }

        /// <summary>
        /// 选择记录
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="mapId"></param>
        public static MapSet Select(DbDecipher decipher, int mapId)
        {
            LightModelFilter tableFilter = new LightModelFilter(typeof(IG2_MAP));
            tableFilter.And("IG2_MAP_ID", mapId);
            tableFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            tableFilter.Locks.Add(LockType.NoLock);

            LightModelFilter colsFilter = new LightModelFilter(typeof(IG2_MAP_COL));
            colsFilter.And("IG2_MAP_ID", mapId);
            colsFilter.TSqlOrderBy = "ROW_USER_SEQ ASC,IG2_MAP_COL_ID ASC";
            colsFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            colsFilter.Locks.Add(LockType.NoLock);

            var m_Map = decipher.SelectToOneModel<IG2_MAP>(tableFilter);
            var m_Cols = decipher.SelectModels<IG2_MAP_COL>(colsFilter);


            return new MapSet() { m_Map = m_Map, m_Cols = m_Cols };
        }

    }
}
