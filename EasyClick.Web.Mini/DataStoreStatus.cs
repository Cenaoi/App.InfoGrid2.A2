using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Diagnostics;
using EasyClick.Web.Mini.Utility;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 数据仓库状态
    /// </summary>
    public class DataStoreStatus
    {
        string m_DataKeys;

        public string DataKeys
        {
            get { return m_DataKeys; }
            set { m_DataKeys = value; }
        }

        List<DataStoreRow> m_Rows = new List<DataStoreRow>();

        public List<DataStoreRow> Rows
        {
            get { return m_Rows; }
            set { m_Rows = value; }
        }

        /// <summary>
        /// 解释行数据
        /// </summary>
        /// <param name="rowData"></param>
        /// <returns></returns>
        private static DataStoreRow RowParse(JToken rowData)
        {
            DataStoreRow rowStore = new DataStoreRow();

            string stateStr = rowData["state"].Value<string>();

            rowStore.State = (DataStoreRowState)Enum.Parse(typeof(DataStoreRowState), stateStr, true);

            rowStore.Pk = rowData["pk"].Value<string>();

            JToken fields = rowData["fs"];
            JToken fixedFs = rowData["fixedFs"];

            if (fields != null)
            {
                foreach (JProperty field in fields.Children())
                {
                    DataStoreCell cc = new DataStoreCell();

                    cc.Name = field.Name;
                    cc.Value = fields[field.Name].Value<string>();

                    rowStore.Cells.Add(cc);
                }
            }

            if (fixedFs != null)
            {
                foreach (JProperty field in fixedFs.Children())
                {
                    DataStoreCell cc = new DataStoreCell();

                    cc.Name = field.Name;
                    cc.Value = fixedFs[field.Name].Value<string>();

                    rowStore.FixedFields.Add(cc);
                }
            }


            return rowStore;
        }



        public static DataStoreStatus Parse(string json)
        {
            DataStoreStatus store = new DataStoreStatus();

            JObject o = (JObject)JsonConvert.DeserializeObject(json);

            store.DataKeys = o["dataKeys"].Value<string>();

            
            JToken rows = o["rows"];

            foreach (JToken row in rows.Children())
            {
                JToken rowData = row.First;

                DataStoreRow rowStore = RowParse(rowData);
                store.Rows.Add(rowStore);
            }


            return store;
        }
    }
}
