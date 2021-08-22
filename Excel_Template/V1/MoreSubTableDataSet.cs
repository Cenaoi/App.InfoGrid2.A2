using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.V1
{
   /// <summary>
   /// 多子表数据集
   /// </summary>
   public class MoreSubTableDataSet
    {
        /// <summary>
        /// 头.主表
        /// </summary>
        public LModel Head { get; set; }

        SortedList<string, LModelList<LModel>> m_Items;


        List<LModel> _oneItems;


        /// <summary>
        /// 字典类型子表数据集合
        /// </summary>
        public SortedList<string, LModelList<LModel>> Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new SortedList<string, LModelList<LModel>>();
                }
                return m_Items;
            }
            set { m_Items = value; }
        }


        /// <summary>
        /// 只有一个子表的数据集合
        /// </summary>
        public List<LModel> OneItems
        {

            get
            {

                if(_oneItems == null)
                {
                    _oneItems = new List<LModel>();
                }

                return _oneItems;


            }

            set { _oneItems = value; }


        }


    }
}
