using System;
using System.Collections.Generic;
using System.Text;
using EasyClick.Web.ReportForms.Data;

namespace EasyClick.Web.ReportForms
{

    /// <summary>
    /// 行的固定数据
    /// </summary>
    public class CrossRowHeadTreeNode : CrossHeadTreeNode
    {
        RRow m_Data;

        public RRow Data
        {
            get
            {
                return m_Data;
            }
            set
            {
                m_Data = value;
            }
        }
    }
}
