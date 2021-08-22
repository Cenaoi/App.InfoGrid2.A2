using EC5.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{
    /// <summary>
    /// 操作字段集合
    /// </summary>
    public class OperateFieldCollection : List<OperateField>
    {
        public OperateField Add(string field, string value)
        {
            OperateField item;

            if (value != null && (StringUtil.StartsWith(value,"{{") && StringUtil.EndsWith(value,"}}")) )
            {
                string code = value.Substring(2, value.Length - 4);

                item = new OperateField(field, code, ActionModeType.Fun);

                item.DebugText = value;
            }
            else
            {
                item = new OperateField(field, value, ActionModeType.Fixed);
            }

            base.Add(item);

            return item;
        }

        public OperateField AddFun(string field, string code)
        {
            OperateField item = new OperateField(field, code, ActionModeType.Fun);

            base.Add(item);

            return item;
        }

        public OperateField Add(string field, string value, ActionModeType valueMode)
        {
            OperateField item = new OperateField(field, value, valueMode);

            base.Add(item);

            return item;
        }

        public OperateField Add(string field, object value)
        {
            OperateField item = new OperateField(field, value);

            base.Add(item);

            return item;
        }

        
    }
}
