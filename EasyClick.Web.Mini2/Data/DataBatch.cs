using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using EC5.Utility;
using Newtonsoft.Json;

namespace EasyClick.Web.Mini2.Data
{
    /// <summary>
    /// 数据批次
    /// </summary>
    public class DataBatch
    {
        DataRecordCollection m_Records;

        DataRecord m_Current;

        /// <summary>
        /// 当前选择的对象
        /// </summary>
        public DataRecord Current
        {
            get { return m_Current; }
            set { m_Current = value; }
        }

        /// <summary>
        /// 数据集合
        /// </summary>
        public DataRecordCollection Records
        {
            get
            {
                if (m_Records == null)
                {
                    m_Records = new DataRecordCollection();
                }
                return m_Records;
            }
        }





        /// <summary>
        /// 解析 Json 数据
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataBatch Parse(string json)
        {
            DataBatch store = new DataBatch();
            DataRecordCollection storeRs = store.Records;

            JObject o = (JObject)JsonConvert.DeserializeObject(json);


            JToken records = o["records"];

            foreach (JToken record in records.Children())
            {
                DataRecord rowStore = DataRecord.Parse(record);
                storeRs.Add(rowStore);
            }

            return store;
        }


    }
}
