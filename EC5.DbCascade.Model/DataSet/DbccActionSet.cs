using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;

namespace EC5.DbCascade.Model.DataSet
{
    /// <summary>
    /// 动作事务
    /// </summary>
    public class DbccActionSet
    {
        int m_ID;

        IG2_ACTION m_Act;

        List<IG2_ACTION_ITEM> m_Items;

        /// <summary>
        /// 左边过滤条件
        /// </summary>
        List<IG2_ACTION_FILTER> m_FilterLeft;

        /// <summary>
        /// 右边过滤条件
        /// </summary>
        List<IG2_ACTION_FILTER> m_FilterRigth;

        /// <summary>
        /// 监控的字段名
        /// </summary>
        List<IG2_ACTION_LISTEN> m_ListenFields;

        List<IG2_ACTION_THEN> m_Thens;

        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        /// <summary>
        /// 监控的字段集
        /// </summary>
        public List<IG2_ACTION_LISTEN> ListenFields
        {
            get { return m_ListenFields; }
        }


        public IG2_ACTION Act
        {
            get { return m_Act; }
            set { m_Act = value; }
        }

        public List<IG2_ACTION_THEN> Thens
        {
            get { return m_Thens; }
            set { m_Thens = value; }
        }

        public List<IG2_ACTION_ITEM> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public List<IG2_ACTION_FILTER> FilterLeft
        {
            get { return m_FilterLeft; }
            set { m_FilterLeft = value; }
        }

        public List<IG2_ACTION_FILTER> FilterRight
        {
            get { return m_FilterRigth; }
            set { m_FilterRigth = value; }
        }




        /// <summary>
        /// 选择集合
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="actId">主表主键ID</param>
        public void Select(DbDecipher decipher, int actId)
        {
            LightModelFilter filter = new LightModelFilter(typeof(IG2_ACTION));
            filter.And("IG2_ACTION_ID", actId);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            LightModelFilter filterListen = new LightModelFilter(typeof(IG2_ACTION_LISTEN));
            filterListen.And("IG2_ACTION_ID", actId);
            filterListen.And("ROW_SID", 0, Logic.GreaterThanOrEqual);


            LightModelFilter filterItems = new LightModelFilter(typeof(IG2_ACTION_ITEM));
            filterItems.And("IG2_ACTION_ID", actId);
            filterItems.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filterItems.TSqlOrderBy = "ROW_USER_SEQ ASC,IG2_ACTION_ID ASC";

            LightModelFilter filterL = new LightModelFilter(typeof(IG2_ACTION_FILTER));
            filterL.And("IG2_ACTION_ID", actId);
            filterL.And("ROW_SID", 0,Logic.GreaterThanOrEqual);
            filterL.And("L_R_TAG", "L");
            filterL.TSqlOrderBy = "ROW_USER_SEQ ASC,IG2_ACTION_FILTER_ID ASC";

            LightModelFilter filterR = new LightModelFilter(typeof(IG2_ACTION_FILTER));
            filterR.And("IG2_ACTION_ID", actId);
            filterR.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filterR.And("L_R_TAG", "R");
            filterR.TSqlOrderBy = "ROW_USER_SEQ ASC,IG2_ACTION_FILTER_ID ASC";


            LightModelFilter filterThen = new LightModelFilter(typeof(IG2_ACTION_THEN));
            filterThen.And("IG2_ACTION_ID", actId);
            filterThen.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filterThen.TSqlOrderBy = "ROW_USER_SEQ ASC,IG2_ACTION_THEN_ID ASC";


            m_Act = decipher.SelectToOneModel<IG2_ACTION>(filter);

            m_ListenFields = decipher.SelectModels<IG2_ACTION_LISTEN>(filterListen);

            m_Items = decipher.SelectModels<IG2_ACTION_ITEM>(filterItems);

            m_FilterLeft = decipher.SelectModels<IG2_ACTION_FILTER>(filterL);

            m_FilterRigth = decipher.SelectModels<IG2_ACTION_FILTER>(filterR);

            m_Thens = decipher.SelectModels<IG2_ACTION_THEN>(filterThen);
        }

    }
}
