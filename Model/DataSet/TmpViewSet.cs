using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model.DataSet
{
    public class TmpViewSet
    {
        IG2_TMP_VIEW m_View;

        List<IG2_TMP_VIEW_TABLE> m_Tables;

        List<IG2_TMP_VIEW_FIELD> m_Fields;

        public IG2_TMP_VIEW View
        {
            get { return m_View; }
            set { m_View = value; }
        }

        public List<IG2_TMP_VIEW_TABLE> Tables
        {
            get { return m_Tables; }
            set { m_Tables = value; }
        }

        public List<IG2_TMP_VIEW_FIELD> Fields
        {
            get { return m_Fields; }
            set { m_Fields = value; }
        }


        public static TmpViewSet Select(DbDecipher decipher, Guid uid,string sessionId)
        {
            LightModelFilter filter = new LightModelFilter(typeof(IG2_TMP_VIEW));
            filter.And("TMP_GUID", uid);
            filter.And("TMP_SESSION_ID", sessionId);



            LightModelFilter tabFilter = new LightModelFilter(typeof(IG2_TMP_VIEW_TABLE));
            tabFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            tabFilter.And("TMP_GUID", uid);
            tabFilter.And("TMP_SESSION_ID", sessionId);
            tabFilter.TSqlOrderBy = "ROW_USER_SEQ ASC, IG2_TMP_VIEW_TABLE_ID ASC";

            LightModelFilter fieldFilter = new LightModelFilter(typeof(IG2_TMP_VIEW_FIELD));
            //fieldFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            fieldFilter.And("TMP_GUID", uid);
            fieldFilter.And("TMP_SESSION_ID", sessionId);
            fieldFilter.TSqlOrderBy = "ROW_USER_SEQ ASC, IG2_TMP_VIEW_FIELD_ID ASC";

           

            var view = decipher.SelectToOneModel<IG2_TMP_VIEW>(filter);

            if(view == null)
            {
                return null;
            }

            TmpViewSet tvs = new TmpViewSet();
            tvs.m_View = view;

            tvs.m_Tables = decipher.SelectModels<IG2_TMP_VIEW_TABLE>(tabFilter);

            tvs.m_Fields = decipher.SelectModels<IG2_TMP_VIEW_FIELD>(fieldFilter);

            return tvs;
        }

        public void Insert(DbDecipher decipher, Guid tempGuid, string sessionId)
        {
            m_View.TMP_GUID = tempGuid;
            m_View.TMP_SESSION_ID = sessionId;
            m_View.TMP_OP_ID = "";


            foreach (IG2_TMP_VIEW_FIELD col in m_Fields)
            {
                col.TMP_GUID = tempGuid;
                col.TMP_OP_ID = "";
                col.TMP_SESSION_ID = sessionId;
            }


            foreach (IG2_TMP_VIEW_TABLE tab in m_Tables)
            {
                tab.TMP_GUID = tempGuid;
                tab.TMP_OP_ID = "";
                tab.TMP_SESSION_ID = sessionId;
            }



            decipher.InsertModel(m_View);
            decipher.InsertModels<IG2_TMP_VIEW_TABLE>(m_Tables);
            decipher.InsertModels<IG2_TMP_VIEW_FIELD>(m_Fields);

        }






        public ViewSet ToViewSet()
        {
            ViewSet vSet = new ViewSet();

            vSet.View = new IG2_VIEW();
            this.m_View.CopyTo(vSet.View, true);

            vSet.Fields = new List<IG2_VIEW_FIELD>();

            foreach (IG2_TMP_VIEW_FIELD item in this.Fields)
            {
                IG2_VIEW_FIELD field = new IG2_VIEW_FIELD();

                item.CopyTo(field, true);

                vSet.Fields.Add(field);
            }

            vSet.Tables = new List<IG2_VIEW_TABLE>();

            foreach (IG2_TMP_VIEW_TABLE item in this.Tables)
            {
                IG2_VIEW_TABLE table = new IG2_VIEW_TABLE();

                item.CopyTo(table,true);

                vSet.Tables.Add(table);
            }

            return vSet;
        }






        public void Delete(DbDecipher decipher, Guid uid, string sessionId)
        {
            LightModelFilter filter = new LightModelFilter(typeof(IG2_TMP_VIEW));
            filter.And("TMP_GUID", uid);
            filter.And("TMP_SESSION_ID", sessionId);



            LightModelFilter tabFilter = new LightModelFilter(typeof(IG2_TMP_VIEW_TABLE));
            tabFilter.And("TMP_GUID", uid);
            tabFilter.And("TMP_SESSION_ID", sessionId);

            LightModelFilter fieldFilter = new LightModelFilter(typeof(IG2_TMP_VIEW_FIELD));
            fieldFilter.And("TMP_GUID", uid);
            fieldFilter.And("TMP_SESSION_ID", sessionId);


            decipher.DeleteModels(filter);
            decipher.DeleteModels(tabFilter);
            decipher.DeleteModels(fieldFilter);

        }
    }
}
