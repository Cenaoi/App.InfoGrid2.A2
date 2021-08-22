using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EasyClick.Web.Mini.Utility;

namespace EasyClick.Web.Mini
{


    public class DataSeletedStatus
    {
        public static readonly DataSeletedStatus Empty = new DataSeletedStatus();

        string m_DataKeys;
        string m_FixedFields;

        public string FixedFields
        {
            get { return m_FixedFields; }
            set { m_FixedFields = value; }
        }

        public string DataKeys
        {
            get { return m_DataKeys; }
            set { m_DataKeys = value; }
        }

        List<DataSeletedItem> m_Items = new List<DataSeletedItem>();

        public List<DataSeletedItem> Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new List<DataSeletedItem>();
                }
                return m_Items;
            }
        }

        /// <summary>
        /// 解释行数据
        /// </summary>
        /// <param name="rowData"></param>
        /// <returns></returns>
        private static DataSeletedItem ItemParse(JToken itemData)
        {
            DataSeletedItem item = new DataSeletedItem();

            item.Guid = itemData["guid"].Value<string>();
            item.Pk = itemData["pk"].Value<string>();

            return item;
        }



        public static DataSeletedStatus Parse(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return DataSeletedStatus.Empty;
            }

            DataSeletedStatus store = new DataSeletedStatus();

            JObject o = (JObject)JsonConvert.DeserializeObject(json);

            store.DataKeys = o["dataKeys"].Value<string>();
            store.FixedFields = o["fixedFields"].Value<string>();

            
            JToken rows = o["rows"];

            foreach (JToken row in rows.Children())
            {
                JToken rowData = row.First;

                DataSeletedItem rowStore = ItemParse(rowData);
                store.Items.Add(rowStore);
            }

            return store;
        }

        public int[] GetGuidValues()
        {
            if (m_Items == null || m_Items.Count == 0)
            {
                return new int[0];
            }

            int[] values = new int[m_Items.Count];

            for (int i = 0; i < m_Items.Count; i++)
            {
                values[i] = StringUtility.ToInt(m_Items[i].Guid);
            }

            return values;
        }

        public int[] GetIntPkValues()
        {
            if (m_Items == null || m_Items.Count == 0)
            {
                return new int[0];
            }

            int[] values = new int[m_Items.Count];

            for (int i = 0; i < m_Items.Count; i++)
            {
                values[i] = StringUtility.ToInt( m_Items[i].Pk);
            }

            return values;
        }

        public string[] GetStrPkValues()
        {
            if (m_Items == null || m_Items.Count == 0)
            {
                return new string[0];
            }

            string[] values = new string[m_Items.Count];

            for (int i = 0; i < m_Items.Count; i++)
            {
                values[i] = m_Items[i].Pk;
            }

            return values;
        }



    }
}
