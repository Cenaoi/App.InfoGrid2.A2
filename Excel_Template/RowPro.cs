using System;
using System.Collections.Generic;
using System.Web;

namespace App.InfoGrid2.Excel_Template
{

    public class CellProCollection:List<CellPro>
    {
    

    }


    /// <summary>
    /// 行属性
    /// </summary>
    public class RowPro
    {
        private CellProCollection m_cellPro;
        /// <summary>
        /// 行的所有单元格集合
        /// </summary>
        public CellProCollection cellPro 
        {
            get 
            {
                if(m_cellPro == null)
                {
                    m_cellPro = new CellProCollection();
                }
                return m_cellPro;
            }
            set { m_cellPro = value; }
        }
        /// <summary>
        /// 行高
        /// </summary>
        public float RowHeight { get; set; }


    }
}