using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasyClick.Web.ReportForms
{
    /// <summary>
    /// 报表配置信息
    /// </summary>
    public class ReportConfig
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ReportItemGroupCollection m_ColTags = new ReportItemGroupCollection();

        ReportItemGroupCollection m_RowTags = new ReportItemGroupCollection();

        /// <summary>
        /// 数值
        /// </summary>
        ReportItemGroupCollection m_DataTags = new ReportItemGroupCollection();
        
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// 列集合
        /// </summary>
        public ReportItemGroupCollection ColGroupTags
        {
            get { return m_ColTags; }
        }

        /// <summary>
        /// 行区域
        /// </summary>
        public ReportItemGroupCollection RowGroupTags
        {
            get { return m_RowTags; }
        }

        /// <summary>
        /// 数据区域
        /// </summary>
        public ReportItemGroupCollection DataGroupTags
        {
            get { return m_DataTags; }
        }


        /// <summary>
        /// 数据区域
        /// </summary>
        [DefaultValue(ReportDataLayout.Bottom)]
        public ReportDataLayout DataLayout { get; set; } = ReportDataLayout.Bottom;



        /// <summary>
        /// 激活列小计
        /// </summary>
        [DefaultValue(true)]
        public bool EnabledColTotal { get; set; } = true;

        /// <summary>
        /// 激活行小计
        /// </summary>
        [DefaultValue(true)]
        public bool EnabledRowTotal { get; set; } = true;


        /// <summary>
        /// 激活期初
        /// </summary>
        [DefaultValue(false)]
        public bool EnabledBeginningBalance { get; set; } 

        /// <summary>
        /// 激活期末
        /// </summary>
        [DefaultValue(false)]
        public bool EnabledEndingBalance { get; set; }

    }
}
