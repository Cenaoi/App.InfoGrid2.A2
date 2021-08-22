using System.Text;
using System.Xml;
using EC5.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace App.InfoGrid2.Bll.Builder
{
    /// <summary>
    /// 模板转换类
    /// </summary>
    public static class TemplateConvert
    {

        /// <summary>
        /// Json信息转成xml信息
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <returns>xml信息</returns>
        public static string JsonToXml(string json) 
        {
            TemplateItem item = ReaderRow(json);

            StringBuilder sb = new StringBuilder();

            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n");

            sb.Append("<page-template>\n");

            for (int i = 0; i < item.Childs.Count;i++ )
            {
                TemplateItem ti = item.Childs[i];

                AddChilds(ti, sb,1);
            }

            sb.Append("</page-template>");


            return sb.ToString(); ;
        }

        


        /// <summary>
        /// 拿到json里面的值
        /// </summary>
        /// <param name="json">json数据</param>
        /// <returns>TemplateItem对象</returns>
        private static TemplateItem ReaderRow(string json) 
        {
            JObject o = (JObject)JsonConvert.DeserializeObject(json);

            JToken rows = o["childs"];

            TemplateItem items = new TemplateItem();


            foreach (JToken row in rows.Children())
            {
                TemplateItem item = TItemParse(row);

                items.Childs.Add(item);


            }

            return items;

        }



        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="rowData"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetAttr(JToken rowData, string name)
        {
            JToken tagRow = rowData[name];
            string tag = null;

            if (tagRow != null)
            {
                tag = tagRow.Value<string>();
            }

            return tag;
        }





        /// <summary>
        /// 解释行数据
        /// </summary>
        /// <param name="rowData"></param>
        /// <returns></returns>
        private static TemplateItem TItemParse(JToken rowData) 
        {
            TemplateItem tItem;
            
            string ecTag = GetAttr(rowData, "ec-tag");

            if (ecTag == "script")
            {
                tItem = new TItemScript();
            }
            else
            {
                tItem = new TemplateItem();
            }

            tItem.DeserializeJsonAttrs(rowData);


            JToken chlids = rowData["childs"];

            if (chlids != null)
            {
                foreach (JToken row in chlids.Children())
                {
                    TemplateItem t = TItemParse(row);
                    tItem.Childs.Add(t);
                }
            }

            return tItem;
        }




        /// <summary>
        /// 添加每个节点
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sb"></param>
        private static void AddChilds(TemplateItem item, StringBuilder sb, int span)
        {
            string d = "";

            string spanStr = d.PadRight(span * 2, ' ');

            sb.Append(spanStr);   //增加 xml 节点之间的空格
            
            sb.AppendFormat("<{0}", item.EcTag);

            item.SerializableXmlAttrs(sb);

            if (item.Childs.Count > 0)
            {
                sb.Append(">\n");

                foreach (TemplateItem ti in item.Childs)
                {
                    AddChilds(ti, sb, span + 1);
                }

                sb.Append(spanStr);
                sb.AppendFormat("</{0}>\n", item.EcTag);
            }
            else
            {
                sb.Append(" />\n");
            }

        }




        /// <summary>
        /// xml信息转成Json信息
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <returns>Json字符串</returns>
        public static string XmlToJson(string xml) 
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);

            StringBuilder sb = new StringBuilder();


            sb.Append("{ ");

            XmlElement root = doc.DocumentElement;

            sb.Append("\"childs\": [");


            if (root.HasChildNodes)
            {
                XmlNodeList childs = root.ChildNodes;

                for (int i = 0; i < childs.Count; i++)
                {
                    XmlNode elem = childs[i];

                    if (i > 0)
                    {
                        sb.Append(",");
                    }

                    ProXmlNode(elem, sb);

                }
            }

            sb.Append("]");
            sb.Append("}");


            return sb.ToString();
            
        }





        /// <summary>
        /// 处理每个节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="sb"></param>
        private static void ProXmlNode(XmlNode node, StringBuilder sb)
        {
            //获取节点属性

            sb.Append("{");

            sb.AppendFormat("\"ec-tag\":\"{0}\"", node.Name);

            int attrLen = node.Attributes.Count;

            for (int i = 0; i < attrLen; i++)
            {
                XmlAttribute attr = node.Attributes[i];
                sb.AppendFormat(",\"{0}\":\"{1}\"", attr.Name, attr.Value);
            }



            //循环子节点

            if (node.HasChildNodes)
            {
                XmlNodeList childs = node.ChildNodes;
                sb.Append(", \"childs\" : [");

                for (int i = 0; i < childs.Count; i++)
                {
                    XmlNode elem = childs[i];

                    if (i > 0)
                    {
                        sb.Append(",");
                    }


                    ProXmlNode(elem, sb);


                }

                sb.Append("]");
            }

            sb.Append("}");
        }
      





    }
}