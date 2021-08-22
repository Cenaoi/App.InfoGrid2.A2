using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.V1
{
    /// <summary>
    /// 行属性
    /// </summary>
    public class RowPro:List<CellPro>
    {

        /// <summary>
        /// 行高
        /// </summary>
        public float RowHeight { get; set; }

        /// <summary>
        /// 行索引
        /// </summary>
        public int RowIndex { get; set; }


        /// <summary>
        /// 克隆一行记录
        /// </summary>
        /// <returns></returns>
        public RowPro Clone()
        {
            RowPro rp = new RowPro();


            foreach(CellPro cp in this)
            {

                rp.Add(cp.Clone());
            }

            rp.RowHeight = this.RowHeight;
            rp.RowIndex = this.RowIndex;


            return rp;

        }

    }
}
