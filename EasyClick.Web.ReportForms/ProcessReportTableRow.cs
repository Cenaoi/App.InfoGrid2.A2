using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;

namespace EasyClick.Web.ReportForms
{

    /// <summary>
    /// 普通报表处理行
    /// </summary>
    /// <param name="target">目标行</param>
    /// <param name="srcData">明细数据</param>
    public delegate void ProcessReportTableRow(LModel target, LModel srcData);

}
