using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace App.InfoGrid2.Bll
{
    [XmlRootAttribute("")]
    public class ReportTemplateItem
    {

        private List<ReportCol> m_row_value;



        private List<ReportCol> m_col_value;


        private List<ReportCol> m_values;



        /// <summary>
        /// 表名
        /// </summary>
        [XmlElement("table_name")]
        public string table_name { get; set; }

        /// <summary>
        /// 图标显示形式  row 或 col
        /// </summary>
        [XmlElement("chart_row_col")]
        public string chart_row_col { get; set; }


        /// <summary>
        /// 时间字段 
        /// </summary>
        [XmlElement("date_field")]
        public string date_field { get; set; }


        /// <summary>
        /// 一个值列，但是显示多值
        /// </summary>
        [XmlElement("multi_value")]
        public bool multi_value { get; set; }

        /// <summary>
        /// 图表类型  bar--柱子  pie -- 饼状图就是圆图 line -- 线
        /// </summary>
        [DefaultValue("bar")]
        [XmlElement("chart_type")]
        public string chart_type { get; set; } = "bar";

        /// <summary>
        /// 图表类型  样式  
        /// </summary>
        [DefaultValue("")]
        [XmlElement("chart_type_style")]
        public string chart_type_style { get; set; } = "";


        /// <summary>
        /// 行的显示字段  饼状图形用
        /// </summary>
        [DefaultValue("")]
        [XmlElement("row_display_field")]
        public string row_display_field { get; set; } = "";


        /// <summary>
        /// 临时用的 x 索引  以后会去掉的
        /// </summary>
        [DefaultValue(0)]
        [XmlElement("temp_x_index")]
        public int temp_x_index { get; set; } = 0;

        /// <summary>
        /// 期初 激活
        /// </summary>
        [Description("期初 激活")]
        [XmlElement("bb_enabled")]
        public bool bb_enabled { get; set; }

        /// <summary>
        /// 期初 开始时间
        /// </summary>
        [Description("期初开始时间")]
        [XmlElement("bb_time")]
        public string bb_time { get; set; }


        /// <summary>
        ///期末 激活
        /// </summary>
        [Description("期末 激活")]
        [XmlElement("eb_enabled")]
        public bool eb_enabled { get; set; }
        /// <summary>
        /// 期末结束时间
        /// </summary>
        [Description("期末结束时间")]
        [XmlElement("eb_time")]
        public string eb_time { get; set; }



        /// <summary>
        ///是否缓冲
        /// </summary>
        [Description("是否缓冲")]
        [XmlElement("is_data_buffer")]
        public bool is_data_buffer { get; set; }



        //[XmlElementAttribute("col_value")]
        public List<ReportCol> col_value { 
            get 
            {
                if (m_col_value == null)
                {
                    m_col_value = new List<ReportCol>();
                }
                return m_col_value;
            }
            set { m_col_value = value; }
        }

        //[XmlElementAttribute("row_value")]
        public List<ReportCol> row_value
        {
            get
            {
                if (m_row_value == null)
                {
                    m_row_value = new List<ReportCol>();
                }
                return m_row_value;
            }
            set { m_row_value = value; }
        }


        //[XmlElementAttribute("values")]
        public List<ReportCol> values
        {
            get
            {
                if (m_values == null)
                {
                    m_values = new List<ReportCol>();
                }
                return m_values;
            }
            set { m_values = value; }
        }



    }
}
