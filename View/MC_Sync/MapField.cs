using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;


namespace App.InfoGrid2.MC_Sync
{


    /// <summary>
    /// 字段映射
    /// </summary>
    [Serializable]
    public class MapField
    {
        [XmlAttribute("from")]
        public string From { get; set; }

        [XmlAttribute("to")]
        public string To { get; set; }

        /// <summary>
        /// 字段的描述信息
        /// </summary>
        [XmlAttribute("desc")]
        public string Desc { get; set; }

        [XmlAttribute("default")]
        public string Default { get; set; }

        /// <summary>
        /// 转换数据类型
        /// </summary>
        [XmlAttribute("to-type")]
        public string ToType { get; set; }

        [XmlAttribute("from-type")]
        public string FromType { get; set; }

        /// <summary>
        /// 强制
        /// </summary>
        [DefaultValue(true)]
        [XmlAttribute("mandatory")]
        public bool Mandatory { get; set; } = true;


        MapFieldIsNull m_IsNull = null;

        [DefaultValue(false)]
        [XmlAttribute("server")]
        public bool Server { get; set; } = false;



        [XmlElement("is-null")]
        public MapFieldIsNull IsNull
        {
            get { return m_IsNull; }
            set { m_IsNull = value; }
        }


        MapFieldFromCode m_From = null;


        [XmlElement("from-code")]
        public MapFieldFromCode FromCode
        {
            get { return m_From; }
            set { m_From = value; }
        }
    }


    /// <summary>
    /// 空值的特殊处理
    /// </summary>
    [Serializable]
    public class MapFieldIsNull
    {
        MapFieldFromCode m_From = null;


        [XmlElement("from-code")]
        public MapFieldFromCode From
        {
            get
            {
                if (m_From == null)
                {
                    m_From = new MapFieldFromCode();
                }

                return m_From;
            }
            set { m_From = value; }
        }

    }

    [Serializable]
    public class MapFieldFromCode
    {
        /// <summary>
        /// 语言选择.  sql | sc-code= C#代码
        /// </summary>
        [XmlAttribute("lang")]
        public string Lang { get; set; }

        [XmlElement("sql")]
        public string SQL { get; set; }

        [XmlElement("cs-code")]
        public string CSCode { get; set; }



    }







    public enum VTmpItemType
    {
        Text,

        Code
    }

    public class VTmpItem
    {
        public VTmpItemType Type { get; set; } = VTmpItemType.Text;

        /// <summary>
        /// 原文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 返回的值
        /// </summary>
        public object ResultValue { get; set; }

        public VTmpItem()
        {
        }

        public VTmpItem(VTmpItemType itemType, string itemText)
        {
            this.Type = itemType;
            this.Text = itemText;
        }

        public override string ToString()
        {
            return $"[{this.Type}] {this.Text}";
        }

    }

    public class VTmpItemCollection :List<VTmpItem>
    {

    }

}