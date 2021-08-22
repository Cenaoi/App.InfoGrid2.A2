using EC5.Action3.CodeProcessors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EC5.Action3
{
    /// <summary>
    /// 过滤字段
    /// </summary>
    public class FilterField :FilterItem
    {

        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 过滤逻辑
        /// </summary>
        public string Logic { get; set; }

        /// <summary>
        /// 字段值
        /// </summary>
        public object Value { get; set; }


        /// <summary>
        /// 值模式
        /// </summary>
        [DefaultValue(ActionModeType.Fixed)]
        public ActionModeType Mode { get; set; } = ActionModeType.Fixed;

        /// <summary>
        /// 是否为空值
        /// </summary>
        [DefaultValue(false)]
        public bool IsNull { get; set; } = false;

        public override bool Valid(CodeContext context, object item)
        {
            throw new NotImplementedException();
        }
    }

}
