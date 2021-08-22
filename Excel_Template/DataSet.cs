using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Excel_Template
{
    /// <summary>
    /// 数据集合
    /// </summary>
    public class DataSet
    {
        /// <summary>
        /// 头.主表
        /// </summary>
        public LModel Head { get; set; }

        List<LModel> m_Items;

        /// <summary>
        /// 子表
        /// </summary>
        public List<LModel> Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new List<LModel>();
                }
                return m_Items;
            }
            set { m_Items = value; }
        }
    }
}
