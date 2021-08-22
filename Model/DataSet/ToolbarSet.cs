using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;

namespace App.InfoGrid2.Model.DataSet
{
    public class ToolbarSet
    {
        IG2_TOOLBAR m_Toolbar;

        List<IG2_TOOLBAR_ITEM> m_Items;

        public IG2_TOOLBAR Toolbar
        {
            get { return m_Toolbar; }
            set { m_Toolbar = value; }
        }

        public List<IG2_TOOLBAR_ITEM> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public void SelectForTable(DbDecipher decipher, int tableId)
        {
            LightModelFilter filter = new LightModelFilter(typeof(IG2_TOOLBAR));
            filter.And("TABLE_ID", tableId);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Locks.Add(LockType.NoLock);



            m_Toolbar = decipher.SelectToOneModel<IG2_TOOLBAR>(filter);

            if (m_Toolbar != null)
            {
                LightModelFilter itemFilter = new LightModelFilter(typeof(IG2_TOOLBAR_ITEM));
                itemFilter.And("IG2_TOOLBAR_ID", m_Toolbar.IG2_TOOLBAR_ID);
                itemFilter.And("TABLE_ID", tableId);
                itemFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                itemFilter.Locks.Add(LockType.NoLock);

                itemFilter.TSqlOrderBy = "ROW_USER_SEQ ASC,IG2_TOOLBAR_ITEM_ID ASC";

                m_Items = decipher.SelectModels<IG2_TOOLBAR_ITEM>(itemFilter);
            }
        }

    }
}
