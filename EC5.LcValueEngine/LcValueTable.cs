using HWQ.Entity.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.LcValueEngine
{



    public class LcValueTable
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 激活
        /// </summary>
        public bool Enabled { get; set; }


        LcValueIfCollection m_IfList;

        /// <summary>
        /// 判断的类型
        /// </summary>
        public LcValueIfCollection IfList
        {
            get
            {
                if (m_IfList == null)
                {
                    m_IfList = new LcValueIfCollection();
                }

                return m_IfList;
            }
        }
    }





}
