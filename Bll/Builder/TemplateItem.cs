using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using System.Text;
using EC5.Utility;



namespace App.InfoGrid2.Bll.Builder
{
  
    public class TemplateItem
    {
        /// <summary>
        /// 存放ec-tag属性的值
        /// </summary>        
        public string EcTag { get; set; }


        /// <summary>
        /// 存放ec-type属性的值
        /// </summary>        
        public string EcType { get; set; }

        /// <summary>
        /// 存放value属性的值
        /// </summary>        
        public string  Values { get; set; }

        /// <summary>
        /// 存放title属性的值
        /// </summary>       
        public string Title { get; set; }

        /// <summary>
        /// 存放id属性的值
        /// </summary>       
        public string ID { get; set; }

        /// <summary>
        /// 存放ec-main-view属性的值
        /// </summary>
        public string EcMainView { get; set; }

        public string EcMainType { get; set; }

        public string EcMainName { get; set; }


        /// <summary>
        /// 存放childs属性的值
        /// </summary>        
        TemplateItemCollection m_Childs;

        public TemplateItemCollection Childs
        {
            get
            {
                if (m_Childs == null)
                {
                    m_Childs = new TemplateItemCollection();
                }

                return m_Childs;
            }
        }



        /// <summary>
        /// 把属性添加进节点
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetXmlAttr(StringBuilder sb, string name, string value)
        {
            if (StringUtil.IsBlank(name))
            {
                throw new Exception("设置属性错误，属性名不能为空.");
            }


            if (!StringUtil.IsBlank(value))
            {
                sb.AppendFormat(" {0}=\"{1}\"", name, value);
            }
        }


        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="rowData"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string GetJsonAttr(JToken rowData, string name)
        {
            if (StringUtil.IsBlank(name))
            {
                throw new Exception("设置属性错误，属性名不能为空.");
            }

            JToken tagRow = rowData[name];
            string tag = null;

            if (tagRow != null)
            {
                tag = tagRow.Value<string>();
            }

            return tag;
        }


        /// <summary>
        /// 按json反序列化为对象属性
        /// </summary>
        /// <param name="rowData">json信息</param>
        /// <param name="ds">json类</param>
        public virtual void DeserializeJsonAttrs(JToken rowData)
        {
            this.EcTag = GetJsonAttr(rowData, "ec-tag");
            this.Values = GetJsonAttr(rowData, "value");
            this.ID = GetJsonAttr(rowData, "id");
            this.EcType = GetJsonAttr(rowData, "ec-type");
            this.Title = GetJsonAttr(rowData, "title");
            this.EcMainView = GetJsonAttr(rowData, "ec-main-view");
            this.EcMainType = GetJsonAttr(rowData, "ec-main-type");
            this.EcMainName = GetJsonAttr(rowData, "ec-main-name");

        }

        /// <summary>
        /// 序列化 xml 的属性集合
        /// </summary>
        /// <param name="sb"></param>
        public virtual void SerializableXmlAttrs(StringBuilder sb)
        {

            SetXmlAttr(sb, "ec-type", this.EcType);
            SetXmlAttr(sb, "title", this.Title);
            SetXmlAttr(sb, "value", this.Values);
            SetXmlAttr(sb, "id", this.ID);

            SetXmlAttr(sb, "ec-main-view", this.EcMainView);
            SetXmlAttr(sb, "ec-main-type", this.EcMainType);
            SetXmlAttr(sb, "ec-main-name", this.EcMainName);

        }


    }




}