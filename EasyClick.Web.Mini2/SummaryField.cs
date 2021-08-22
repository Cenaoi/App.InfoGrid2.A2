using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 汇总的值
    /// </summary>
    public class SummaryItemCollection : SortedList<string, object>
    {

    }

    /// <summary>
    /// 汇总类型
    /// </summary>
    public enum SummaryType
    {
        /// <summary>
        /// 合计
        /// </summary>
        SUM,
        /// <summary>
        /// 平均
        /// </summary>
        AVG,
        /// <summary>
        /// 合计
        /// </summary>
        COUNT,
        /// <summary>
        /// 最大值
        /// </summary>
        MAX,
        /// <summary>
        /// 最小值
        /// </summary>
        MIN,
        /// <summary>
        /// 用户自定义
        /// </summary>
        OTHER
    }

    /// <summary>
    /// 汇总字段集合
    /// </summary>
    public class SummaryFieldCollection:List<SummaryField>
    {
        public SummaryField Add(string dataField)
        {
            SummaryField item = new SummaryField(dataField);
            base.Add(item);

            return item;
        }

        public SummaryField Add(string dataField, SummaryType summaryType)
        {
            SummaryField item = new SummaryField(dataField, summaryType);

            base.Add(item);

            return item;
        }


        public SummaryField Add(string srcViewField, string dataField, SummaryType summaryType)
        {
            SummaryField item = new SummaryField(dataField, summaryType);
            item.SrcViewField = srcViewField;

            base.Add(item);

            return item;
        }

        /// <summary>
        /// 包含字段
        /// </summary>
        /// <param name="field">字段名</param>
        /// <returns></returns>
        public bool HasField(string field)
        {
            SummaryField item =  base.Find(s => s.DataField == field);

            return item != null;
        }

        /// <summary>
        /// 获取汇总字段
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public SummaryField Get(string field)
        {
            SummaryField item = base.Find(s => s.DataField == field);

            return item;
        }
    }

    /// <summary>
    /// 汇总字段
    /// </summary>
    public class SummaryField
    {
        string m_DataField;

        /// <summary>
        /// 源视图字段名
        /// </summary>
        string m_SrcViewField;

        string m_UserType;

        /// <summary>
        /// 参数名称.自定义名称
        /// </summary>
        string m_Name;

        SummaryType m_SummaryType = SummaryType.SUM;


        /// <summary>
        /// 统计过滤
        /// </summary>
        ParamCollection m_Filter;

        /// <summary>
        /// 统计过滤
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ParamCollection Filter
        {
            get
            {
                if (m_Filter == null)
                {
                    m_Filter = new ParamCollection();
                }
                return m_Filter;
            }
        }


        /// <summary>
        /// (构造函数)汇总字段
        /// </summary>
        public SummaryField()
        {
        }

        /// <summary>
        /// (构造函数)汇总字段
        /// </summary>
        /// <param name="dataField">字段名</param>
        public SummaryField(string dataField)
        {
            m_DataField= dataField;
        }

        /// <summary>
        /// (构造函数)汇总字段
        /// </summary>
        /// <param name="dataField">字段名</param>
        /// <param name="summaryType"></param>
        public SummaryField(string dataField, SummaryType summaryType)
        {
            m_DataField = dataField;
            m_SummaryType = summaryType;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        [Description("参数名称")]
        [DefaultValue("")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// 视图原字段
        /// </summary>
        [DefaultValue("")]
        [Description("")]
        public string SrcViewField
        {
            get { return m_SrcViewField; }
            set { m_SrcViewField = value; }
        }

        /// <summary>
        /// 汇总字段名
        /// </summary>
        public string DataField
        {
            get { return m_DataField; }
            set { m_DataField = value; }
        }

        /// <summary>
        /// 汇总类型
        /// </summary>
        [DefaultValue("")]
        public string UserType
        {
            get { return m_UserType; }
            set { m_UserType = value; }
        }

        /// <summary>
        /// 合计
        /// </summary>
        [DefaultValue(SummaryType.SUM)]
        public SummaryType SummaryType
        {
            get { return m_SummaryType; }
            set { m_SummaryType = value; }
        }
    }



}
