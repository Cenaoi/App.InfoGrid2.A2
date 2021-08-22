using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.ReportForms
{
    /// <summary>
    /// 组集合类
    /// </summary>
    public class ReportItemGroupCollection : List<ReportItemGroup>
    {

        public ReportItemGroup Add(ReportItem item)
        {
            ReportItemGroup group = new ReportItemGroup();
            group.Add(item);

            base.Add(group);

            return group;
        }

        public ReportItemGroup Insert(int index, ReportItem item)
        {
            ReportItemGroup group = new ReportItemGroup();
            group.Add(item);

            base.Insert(index, group);

            return group;
        }

    }

}
