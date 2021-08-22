using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EC5.Utility;

namespace EC5.Action3.Xml
{
    public class XOperateField
    {

        public XOperateField()
        {

        }

        public XOperateField(string name, string value)
        {
            this.Name = name;

            if(StringUtil.StartsWith(value,"{{") && StringUtil.EndsWith(value,"}}"))
            {
                this.ValueMode = "fun";

                this.Value = value.Substring(2, value.Length - 4).Trim();
            }
            else
            {
                this.Value = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        public XOperateField(string name, string value, string valueType) : this(name, value)
        {
            this.ValueType = ValueType;
        }




        /// <summary>
        /// 对应的表名
        /// </summary>
        [XmlAttribute("table")]
        public string Table { get; set; }

        /// <summary>
        /// 对应的表名称
        /// </summary>
        [XmlAttribute("table-text")]
        public string TableText { get; set; }



        /// <summary>
        /// 字段名
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        [XmlAttribute("text")]
        public string Text { get; set; }

        /// <summary>
        /// 值类型, 指: int, string, DataTime, bool
        /// </summary>
        [XmlAttribute("value-type")]
        public string ValueType { get; set; }

        /// <summary>
        /// 值是否为空值
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("is-null")]
        public bool IsNull { get; set; }


        /// <summary>
        /// 值是否为空值
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("is-null2")]
        public bool IsNull2 { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("value2")]
        public string Value2 { get; set; }

        /// <summary>
        /// 值类型: fixed=固定值, fun=函数值
        /// </summary>
        [DefaultValue("fixed")]
        [XmlAttribute("value-mode")]
        public string ValueMode { get; set; } = "fixed";
        

        /// <summary>
        /// 备注
        /// </summary>
        [XmlAttribute("remark")]
        public string Remark { get; set; }
    }
}
