using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;
using EC5.Utility;
using System.IO;
using System.Xml.Serialization;

namespace App.InfoGrid2.Bll
{


    public class ExcelConvert
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  

        /// <summary>
        /// Json信息转成xml信息
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <returns>xml信息</returns>
        public string JsonToXml(string json)
        {
            ReportTemplateItem item = JsonConvert.DeserializeObject<ReportTemplateItem>(json);


            string xmlString = "";
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            MemoryStream ms = new MemoryStream();
            ms.Position = 0;
            
            //xml序列化开始
            using (XmlWriter writer = XmlWriter.Create(ms, settings))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                //去除xml声明
                
                ns.Add("", "");　　//第一个参数是前缀，第二个参数是命名空间
                Type t = item.GetType();
                XmlSerializer xml = new XmlSerializer(t);
                xml.Serialize(writer, item, ns);
                byte[] arr = ms.ToArray();
                xmlString = Encoding.UTF8.GetString(arr, 0, arr.Length);
               // ms.Close();
            }


          

            string str = xmlString;


            return str;
        }



        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string GetTableName(string json) 
        {
            ReportTemplateItem item = JsonConvert.DeserializeObject<ReportTemplateItem>(json);

            return item.table_name;
        }



        /// <summary>
        /// xml转成json
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public string XmlToJson(string xml)
        {
            ReportTemplateItem item = new ReportTemplateItem();
            if(string.IsNullOrEmpty(xml))
            {
                return "";
            }
            try
            {
                item = XmlUtil.Deserialize<ReportTemplateItem>(xml);

                string json = JsonConvert.SerializeObject(item);

                return json;

            }
            catch (Exception ex)
            {

                throw new Exception("转化出错！", ex);
            }


        }




    }
}
