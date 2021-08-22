using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace App.InfoGrid2.Bll
{
    public class ReportCol
    {

        private List<ReportField> m_itmes;


        /// <summary>
        /// 字段名
        /// </summary>
        [XmlElement("field")]
        public string field { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlElement("desc")]
        public string desc { get; set; }

        /// <summary>
        /// 总计
        /// </summary>
        [XmlElement("total")]
        public bool total { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [XmlElement("title")]
        public string title { get; set; }

        /// <summary>
        /// 值模式
        /// </summary>
        [XmlElement("value_mode")]
        public string value_mode { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        [XmlElement("width")]
        public string width { get; set; }


        /// <summary>
        /// 计算方法名
        /// </summary>
        [XmlElement("fun_name")]
        public string fun_name { get; set; }


        /// <summary>
        /// 格式
        /// </summary>
        [XmlElement("format")]
        public string format { get; set; }


        /// <summary>
        /// 格式类型 
        /// default -- 默认  quarter -- 季度  Week -- 周
        /// </summary>
        [XmlElement("format_type")]
        public string format_type { get; set; } = "default";



        /// <summary>
        /// db值
        /// </summary>
        [XmlElement("db_value")]
        public string db_value { get; set; }





        /// <summary>
        /// 样式
        /// </summary>
        [XmlElement("style")]
        public string style { get; set; }

        /// <summary>
        /// 数据表名
        /// </summary>
        [XmlElement("tableName")]
        public string tableName { get; set; }

        /// <summary>
        /// 视图的原字段
        /// </summary>
        [XmlElement("viewFieldSrc")]
        public string viewFieldSrc { get; set; }

        /// <summary>
        /// 允许一个子节点
        /// </summary>
        [DefaultValue(false)]
        [XmlElement("one_child")]
        public bool one_child { get; set; }

        /// <summary>
        /// 数据布局 top--顶部 bottom--底部  默认是底部
        /// </summary
        [DefaultValue("bottom")]
        [XmlElement("data_layout")]
        public string data_layout { get; set; } = "bottom";

        /// <summary>
        /// 自定义表达式   
        /// </summary>
        [DefaultValue("")]
        [XmlElement("use_expr")]
        public string use_expr { get; set; } = "";

        /// <summary>
        /// 默认排序 asc
        /// </summary>
        [DefaultValue("")]
        [XmlElement("order_type")]
        public string order_type { get; set; }


        public List<ReportField> fixed_values
        {
            get 
            {
                if(m_itmes == null)
                {
                    m_itmes = new List<ReportField>();
                }
                return m_itmes;
            }
            set { m_itmes = value; }
        }
    }
}
