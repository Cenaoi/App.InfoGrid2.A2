using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;

namespace EC5.Entity.Expanding.ExpandV1
{
    /// <summary>
    /// 过滤模式
    /// </summary>
    public enum FilterMode
    {
        /// <summary>
        /// 通用性
        /// </summary>
        Common,

        /// <summary>
        /// 业务模式, 配合 ROW_SID 使用
        /// </summary>
        Biz
    }

    /// <summary>
    /// 业务过滤
    /// </summary>
    public class BizFilter:LightModelFilter
    {

        public BizFilter(Type modelT) : base(modelT)
        {
            Init();
        }

        public BizFilter(string tableName) : base(tableName)
        {
            Init();
        }

        public BizFilter(Type modelT, FilterMode mode) : base(modelT)
        {
            this.FilterMode = mode;

            Init();
        }

        public BizFilter(string tableName, FilterMode mode) : base(tableName)
        {
            this.FilterMode = mode;
            Init();
        }

        public void Init()
        {
            if (this.FilterMode == FilterMode.Biz)
            {
                this.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            }
        }

        /// <summary>
        /// 过滤模式
        /// </summary>
        public FilterMode FilterMode { get; internal set; } = FilterMode.Biz;
    }
}
