using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace EasyClick.Web.ReportForms.Data
{

    

    /// <summary>
    /// 表格
    /// </summary>
    public class RTable
    {
        RHead m_Head;

        RBody m_Body;

        RFoot m_Foot;

        /// <summary>
        /// 行的标题数量
        /// </summary>
        public int RowHeaderCount { get; internal set; }

        /// <summary>
        /// 列总数量
        /// </summary>
        public int ColumnCount { get; internal set; }

        public RHead Head
        {
            get
            {
                if (m_Head == null)
                {
                    m_Head = new RHead();
                }
                return m_Head;
            }
            set { m_Head = value; }
        }


        public RBody Body
        {
            get
            {
                if (m_Body == null)
                {
                    m_Body = new RBody();
                }
                return m_Body;
            }
            set { m_Body = value; }
        }


        public RFoot Foot
        {
            get
            {
                if (m_Foot == null)
                {
                    m_Foot = new RFoot();
                }
                return m_Foot;
            }
            set { m_Foot = value; }
        }
    }

   





}
