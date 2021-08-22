using HWQ.Entity.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.LcValueEngine
{
    /// <summary>
    /// If 判断
    /// </summary>
    public class LcValueIf
    {
        public int ID { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 逻辑.
        /// </summary>
        public Logic Logic { get; set; }

        /// <summary>
        /// 原值
        /// </summary>
        public string ValueFrom { get; set; }

        /// <summary>
        /// 现值
        /// </summary>
        public string ValueTo { get; set; }

        /// <summary>
        /// 组名
        /// </summary>
        public string Group { get; set; }


        /// <summary>
        /// Then 执行
        /// </summary>
        LcValueThenCollection m_ThenList;

        /// <summary>
        /// 执行的代码
        /// </summary>
        public LcValueThenCollection ThenList
        {
            get
            {
                if (m_ThenList == null)
                {
                    m_ThenList = new LcValueThenCollection();
                }

                return m_ThenList;
            }
        }

    }

}
