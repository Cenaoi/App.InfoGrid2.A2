using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.ReportForms.Data
{
    public static class RTableHelper
    {
        /// <summary>
        /// 创建表格
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static RTable CreateBody(RTable table, int colCount, int rowCount)
        {
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                RRow row = CreateRow(table, colCount);

                table.Body.Add(row);
            }

            return table;
        }

        public static RTable CreateHead(RTable table, int colCount, int rowCount)
        {
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                RRow row = CreateRow(table, colCount);

                table.Head.Add(row);
            }

            return table;
        }

        public static RTable CreateFoot(RTable table, int colCount, int rowCount)
        {
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                RRow row = CreateRow(table, colCount);

                table.Foot.Add(row);
            }

            return table;
        }

        private static RRow CreateRow(RTable table, int colCount)
        {
            RRow row = new RRow();

            for (int i = 0; i < colCount; i++)
            {
                RCell cell = new RCell();

                row.Add(cell);
            }

            return row;
        }





    }
}
