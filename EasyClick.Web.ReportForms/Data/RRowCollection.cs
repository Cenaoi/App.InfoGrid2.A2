using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.ReportForms.Data
{

    /// <summary>
    /// 行集合
    /// </summary>
    public class RRowCollection : List<RRow>
    {
        public RRowCollection()
        {

        }

        public new RRow this[int index]
        {
            get
            {
                return GetRow(index);
            }
        }

        /// <summary>
        /// 获取行对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public RRow GetRow(int index)
        {
            if (index < this.Count)
            {
                return base[index];
            }

            for (int i = this.Count; i <= index; i++)
            {
                RRow row = new RRow();
                row.Y = i;

                base.Add(row);
            }

            return base[index];

        }

        /// <summary>
        /// 获取单元格
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="colIndex">单元格索引</param>
        /// <returns></returns>
        public RCell GetCell(int rowIndex, int colIndex)
        {
            RRow row = GetRow(rowIndex);

            RCell cell = row.GetCell(colIndex);

            return cell;
        }

        /// <summary>
        /// 右对齐
        /// </summary>
        public void PadRight()
        {
            int colCount = 0;

            foreach (RRow row in this)
            {
                colCount = Math.Max(colCount, row.Count);
            }

            foreach (RRow row in this)
            {
                row.PadRight(colCount);
            }

        }


        /// <summary>
        /// 处理单元格 col 合并
        /// </summary>
        public void ProColSpan()
        {
            ProColSpan(0);
        }

        /// <summary>
        /// 处理单元格 col 合并
        /// </summary>
        public void ProColSpan(int colCount)
        {
            int rowCount = colCount;

            foreach (RRow row in this)
            {
                if (colCount <= 0)
                {
                    rowCount = row.Count;
                }

                for (int i = 0; i < rowCount; i++)
                {
                    RCell cell = row.GetCell(i);

                    if (cell.ColSpan == 1)
                    {
                        continue;
                    }

                    for (int j = i + 1; j < i + cell.ColSpan; j++)
                    {
                        RCell mCell = row.GetCell(j);
                        mCell.IsMergeChild = true;
                        mCell.MergeOwnerX = cell.X;
                        mCell.MergeOwnerY = cell.Y;
                    }

                    i += cell.ColSpan - 1;
                }
            }


        }

        private RCell GetPrvCell_NotNull(int i, int rowCount,ref int margeRow)
        {
            RCell prvCell = null;

            for (int j = rowCount - 1; j >= 0; j--)
            {
                RRow prvRow = this.GetRow(j);
                prvCell = prvRow.GetCell(i);

                if (!prvCell.IsNullValue())
                {
                    break;
                }

                margeRow++;
            }

            return prvCell;
        }

        /// <summary>
        /// 处理单元格 row 合并
        /// </summary>
        public void ProRowSpan()
        {
            int rowCount = this.Count;

            RRow row = base[rowCount - 1];

            for (int i = 0; i < row.Count; i++)
            {
                RCell cell = row[i];

                if (!cell.IsNullValue())
                {
                    continue;
                }

                #region 查找上面行一个没有空的对象

                int margeRow = 1;

                RCell prvCell = GetPrvCell_NotNull(i,rowCount, ref margeRow);

                #endregion

                if (prvCell != null && margeRow > 1)
                {
                    prvCell.RowSpan = margeRow;

                    for (int j = prvCell.Y + 1; j < prvCell.Y + margeRow; j++)
                    {
                        RRow nRow = this.GetRow(j);
                        RCell nCell = nRow.GetCell(i);

                        nCell.IsMergeChild = true;
                        nCell.MergeOwnerX = prvCell.X;
                        nCell.MergeOwnerY = prvCell.Y;
                    }
                }
            }


        }




        public void CopyTo(RRowCollection targetRows,int x,int y)
        {
            for (int y1 = 0; y1 < this.Count; y1++)
            {
                RRow row = base[y1];

                for (int x1 = 0; x1 < row.Count; x1++)
                {
                    RCell cell = row[x1];

                    RCell targetCell = targetRows.GetCell(y1 + y, x1 + x);

                    cell.CopyTo(targetCell);

                    if (cell.IsMergeChild)
                    {
                        cell.MergeOwnerX += x;
                        cell.MergeOwnerY += y;
                    }
                }
            }
        }


    }

}
