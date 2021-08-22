using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.AppDomainPlugin
{

    /// <summary>
    /// 动态 LModel 实体
    /// </summary>
    public class DynLModel
    {
        public LModel SrcLModel { get; set; }

        public DynLModel(LModel srcModel)
        {
            this.SrcLModel = srcModel;
        }

        public dynamic this[string name]
        {
            get
            {
                return this.SrcLModel[name];
            }
            set
            {
                this.SrcLModel[name] = value;
            }
        }
        
    }

}
