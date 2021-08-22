using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EasyClick.Web.Mini2.Data
{

    /// <summary>
    /// 数据请求信息
    /// </summary>
    public class DataRequest
    {
        string m_Action;

        string m_TSqlSort;

        DataRequestPage m_Page = DataRequestPage.Empty;

        public string Action
        {
            get { return m_Action; }
            set { m_Action = value; }
        }

        public string TSqlSort
        {
            get { return m_TSqlSort; }
            set { m_TSqlSort = value; }
        }

        public DataRequestPage Page
        {
            get { return m_Page; }
            set { m_Page = value; }
        }



        /// <summary>
        /// 解析 Json 数据
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataRequest Parse(string json)
        {
            DataRequest dr = new DataRequest();
            DataRequestPage page = new DataRequestPage();
            dr.Page = page;

            if (string.IsNullOrEmpty(json) || json == "\"\"" || json == "''" || json[0] != '{' )
            {
                return dr;
            }

            JObject o = (JObject)JsonConvert.DeserializeObject(json);

            dr.Action = o["action"].Value<string>();
            dr.TSqlSort = o["sorters"].Value<string>();

            page.CurrentPage = o["page"].Value<int>();
            page.Start = o["start"].Value<int>();
            page.End = o["end"].Value<int>();
            page.Limit = o["limit"].Value<int>();


            return dr;
        }

    }
}
