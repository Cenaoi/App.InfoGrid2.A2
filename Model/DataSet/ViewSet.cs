using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;

namespace App.InfoGrid2.Model.DataSet
{
    /// <summary>
    /// 视图数据集合
    /// </summary>
    public class ViewSet
    {
        IG2_VIEW m_View;

        List<IG2_VIEW_TABLE> m_Tables;

        List<IG2_VIEW_FIELD> m_Fields;

        public IG2_VIEW View
        {
            get { return m_View; }
            set { m_View = value; }
        }

        public List<IG2_VIEW_TABLE> Tables
        {
            get { return m_Tables; }
            set { m_Tables = value; }
        }

        public List<IG2_VIEW_FIELD> Fields
        {
            get { return m_Fields; }
            set { m_Fields = value; }
        }


        public static ViewSet Select(DbDecipher decipher, int id)
        {
            LightModelFilter filter = new LightModelFilter(typeof(IG2_VIEW));
            filter.And("IG2_VIEW_ID", id);
            

            LightModelFilter tabFilter = new LightModelFilter(typeof(IG2_VIEW_TABLE));
            tabFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            tabFilter.And("IG2_VIEW_ID", id);
            tabFilter.TSqlOrderBy = "ROW_USER_SEQ ASC, IG2_VIEW_TABLE_ID ASC";

            LightModelFilter fieldFilter = new LightModelFilter(typeof(IG2_VIEW_FIELD));
            fieldFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            fieldFilter.And("IG2_VIEW_ID", id);
            fieldFilter.TSqlOrderBy = "ROW_USER_SEQ ASC, IG2_VIEW_FIELD_ID ASC";

            
            var view = decipher.SelectToOneModel<IG2_VIEW>(filter);

            if(view == null)
            {
                return null;
            }

            ViewSet vs = new ViewSet();
            vs.m_View = view;

            vs.m_Tables = decipher.SelectModels<IG2_VIEW_TABLE>(tabFilter);

            vs.m_Fields = decipher.SelectModels<IG2_VIEW_FIELD>(fieldFilter);

            return vs;
        }


        public TableSet ToTableSet()
        {
            TableSet tSet = new TableSet();

            tSet.Table = new IG2_TABLE();

            m_View.CopyTo(tSet.Table, true);

            tSet.Table.IG2_TABLE_ID = m_View.IG2_VIEW_ID;

            tSet.Cols = new List<IG2_TABLE_COL>();

            foreach (IG2_VIEW_FIELD item in m_Fields)
            {
                IG2_TABLE_COL col = new IG2_TABLE_COL();

                item.CopyTo(col, true);

                col.TABLE_NAME = m_View.VIEW_NAME;

                col.VIEW_FIELD_SRC = item.TABLE_NAME + "." + item.FIELD_NAME;
                col.DB_FIELD = String.IsNullOrEmpty(item.ALIAS) ? item.TABLE_NAME + "_" + item.FIELD_NAME : item.ALIAS; 
                
                col.F_NAME = item.FIELD_TEXT;
                col.DISPLAY = item.FIELD_TEXT;

                col.IS_READONLY = true;
                col.IS_VISIBLE = item.FIELD_VISIBLE;
                col.IS_LIST_VISIBLE = item.FIELD_VISIBLE;
                col.IS_SEARCH_VISIBLE = item.FIELD_VISIBLE;

                col.IG2_TABLE_ID = tSet.Table.IG2_TABLE_ID;

                tSet.Cols.Add(col);
            }

            return tSet;
        }


        public TmpViewSet ToTmpViewSet()
        {
            TmpViewSet tSet = new TmpViewSet();

            tSet.View = new IG2_TMP_VIEW();

            m_View.CopyTo(tSet.View, true);


            tSet.Tables = new List<IG2_TMP_VIEW_TABLE>();

            foreach (IG2_VIEW_TABLE tmpCol in m_Tables)
            {
                IG2_TMP_VIEW_TABLE tab = new IG2_TMP_VIEW_TABLE();

                tmpCol.CopyTo(tab, true);

                tSet.Tables.Add(tab);
            }

            tSet.Fields = new List<IG2_TMP_VIEW_FIELD>();

            foreach (IG2_VIEW_FIELD item in m_Fields)
            {
                IG2_TMP_VIEW_FIELD tmp = new IG2_TMP_VIEW_FIELD();

                item.CopyTo(tmp, true);

                tSet.Fields.Add(tmp);
            }

            return tSet;
        }


        public void Insert(DbDecipher decipher)
        {
            //IG2_VIEW_ID 主键不是自动递增的...所以可以预想赋值

            foreach (IG2_VIEW_FIELD item in m_Fields)
            {
                item.IG2_VIEW_ID = m_View.IG2_VIEW_ID;
            }

            foreach (IG2_VIEW_TABLE item in m_Tables)
            {
                item.IG2_VIEW_ID = m_View.IG2_VIEW_ID;
            }


            decipher.InsertModel(m_View);
            decipher.InsertModels<IG2_VIEW_TABLE>(m_Tables);
            decipher.InsertModels<IG2_VIEW_FIELD>(m_Fields);

        }

        public void SetID(int viewId)
        {
            m_View.IG2_VIEW_ID = viewId;

            foreach (IG2_VIEW_FIELD item in m_Fields)
            {
                item.IG2_VIEW_ID = viewId;
            }

            foreach (IG2_VIEW_TABLE item in m_Tables)
            {
                item.IG2_VIEW_ID = viewId;
            }
        }
    }
}
