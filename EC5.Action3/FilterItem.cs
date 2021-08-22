using EC5.Action3.CodeProcessors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EC5.Action3
{

    /// <summary>
    /// 逻辑运算
    /// </summary>
    public enum FilterCondition
    {
        And,
        Or
    }


    public abstract class FilterItem
    {
        public abstract bool Valid(CodeContext context, object item);
    }


    /// <summary>
    /// 过滤项目
    /// </summary>
    public class FilterGroup:FilterItem
    {
        /// <summary>
        /// 过滤条件
        /// </summary>
        [DefaultValue(FilterCondition.And)]
        public FilterCondition Condition { get; set; } = FilterCondition.And;


        List<FilterItem> m_Items = new List<FilterItem>();

        public override bool Valid(CodeContext context, object item)
        {
            throw new NotImplementedException("未实现");
        }

        /// <summary>
        /// 是否包含有子项目
        /// </summary>
        /// <returns></returns>
        public bool HasItem()
        {
            return m_Items.Count > 0;
        }

        public List<FilterItem> Items
        {
            get
            {
                return m_Items;
            }
        }

    }
}
