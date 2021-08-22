using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.ReportForms.Data
{

    /// <summary>
    /// 行对象
    /// </summary>
    public class RRow : List<RCell>
    {
        string m_ID;
        int m_Y;

        public new RCell this[int index]
        {
            get
            {
                return GetCell(index);
            }
        }

        public int Y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }

        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        /// <summary>
        /// 获取单元格
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public RCell GetCell(int index)
        {
            PadRight(index + 1);

            return base[index];
        }

        public void SetCell(int index, RCell cell)
        {
            PadRight(index + 1);

            base[index] = cell;
        }

        public void PadRight(int totalCount)
        {
            for (int i = this.Count; i < totalCount; i++)
            {
                RCell cell = new RCell();
                cell.SetPosition(i, m_Y);

                base.Add(cell);
            }

        }

        public void CopyTo(RRow targetRow,int x)
        {
            targetRow.m_ID = m_ID;
            targetRow.m_Y = m_Y;

            for (int i = 0; i < base.Count; i++)
            {
                RCell srcCell = GetCell(i); ;

                RCell targetCell = targetRow.GetCell(i + x);

                srcCell.CopyTo(targetCell);
            }
        }

    }
}
