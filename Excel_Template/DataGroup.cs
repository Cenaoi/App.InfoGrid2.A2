using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.InfoGrid2.Excel_Template
{
    /// <summary>
    /// 这是数据组,继承 Listt<LModel>
    /// </summary>
    public class DataGroup:List<LModel>
    {

        public DataGroup() 
        {

        }

        public DataGroup(List<LModel> lmList):base(lmList)
        {
            
        }

    }

    public class DGList : List<DataGroup> 
    {

    }

}
