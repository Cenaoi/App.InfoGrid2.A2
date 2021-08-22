using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{
    /// <summary>
    /// 赋值的字段
    /// </summary>
    public class OperateField
    {

        /// <summary>
        /// 调试的内容
        /// </summary>
        public string DebugText { get; internal set; }


        public OperateField()
        {

        }




        public OperateField(string field, string value, ActionModeType valueMode)
        {
            this.ValueMode = valueMode;
            this.Code = field;
            this.Value = value;
        }


        public OperateField(string field, string value)
        {
            this.ValueMode = ActionModeType.Fixed;
            this.Code = field;
            this.Value = value;
        }

        public OperateField(string field, object value)
        {
            this.ValueMode = ActionModeType.Fixed;
            this.Code = field;

            if (value == null)
            {
                this.Value = "null";
                this.ValueType = "null";
            }
            else
            {
                this.Value = value.ToString();
                this.ValueType = value.GetType().Name;
            }

        }



        /// <summary>
        /// 对应的表名
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 对应的表名称
        /// </summary>
        public string TableText { get; set; }

         
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name
        {
            get { return this.Code; }
            set { this.Code = value; }
        }


        /// <summary>
        /// 字段名
        /// </summary>
        public string Code { get; set; }



        /// <summary>
        /// 字段描述
        /// </summary>
        public string Text { get; set; }

        public string ValueType { get; set; }
               
        /// <summary>
        /// 值是否为空值
        /// </summary>
        public bool IsNull { get; set; }


        /// <summary>
        /// 值是否为空值
        /// </summary>
        public bool IsNull2 { get; set; }

        public string Value { get; set; }

        public string Value2 { get; set; }
        
        /// <summary>
        /// 值类型
        /// </summary>
        public ActionModeType ValueMode { get; set; } = ActionModeType.Fixed;


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
