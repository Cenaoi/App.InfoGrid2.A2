using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.SEngine
{
    /// <summary>
    /// 动态 SModel
    /// </summary>
    public class DynSModel
    {
        public SModel SrcSModel { get; set; }

        public DynSModel(SModel model)
        {
            this.SrcSModel = model;
        }

        public dynamic this[string name]
        {
            get { return this.SrcSModel[name]; }
            set { this.SrcSModel[name] = value; }
        }

        public static DynSModel ParseJson(string jsonData)
        {
            SModel sm = SModel.ParseJson(jsonData);

            DynSModel ds = new DynSModel(sm);

            return ds;
        }

        public string Get(string name)
        {
            return this.SrcSModel[name].ToString();
        }

        public ValueT Get<ValueT>(string name)
        {
            return this.SrcSModel.Get<ValueT>(name);
        }
    }
}
