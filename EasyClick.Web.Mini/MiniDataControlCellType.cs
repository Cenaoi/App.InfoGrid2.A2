using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini
{

    /// <summary>
    /// 单元格数据类型
    /// </summary>
    public enum MiniDataControlCellType
    {
        // 摘要:
        //     作为表的页眉单元格的 System.Web.UI.WebControls.DataControlFieldCell。该单元格中的项不绑定到数据。
        Header = 0,
        //
        // 摘要:
        //     作为表的页脚单元格的 System.Web.UI.WebControls.DataControlFieldCell。该单元格中的项不绑定到数据。
        Footer = 1,
        //
        // 摘要:
        //     包含数据的 System.Web.UI.WebControls.DataControlFieldCell。
        DataCell = 2,
    }

}
