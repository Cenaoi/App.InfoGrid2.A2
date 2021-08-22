using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 映射条目集合
    /// </summary>
    public class MapItemCollection:List<MapItem>
    {
        
        /// <summary>
        /// 输出 Json 格式
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            for (int i = 0; i < this.Count; i++)
            {
                MapItem item = this[i];

                if (i > 0) { sb.Append(",\n"); }

                sb.AppendFormat("    {{ srcField:'{0}', targetField:'{1}'}}", item.SrcField, item.TargetField);
            }

            sb.AppendLine("]");

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToJson();
        }

    }


    /// <summary>
    /// 映射条目
    /// </summary>
    [DebuggerDisplay("SrcField={SrcField}, TargetField={TargetField}, Remark={Remark}")]
    public class MapItem
    {
        /// <summary>
        /// 映射条目
        /// </summary>
        public MapItem()
        {
        }

        /// <summary>
        /// 映射条目
        /// </summary>
        /// <param name="srcField">源字段名</param>
        /// <param name="targetField">目标字段名</param>
        public MapItem(string srcField, string targetField)
        {
            m_SrcField = srcField;
            m_TargetField = targetField;
        }

        /// <summary>
        /// 源字段名
        /// </summary>
        string m_SrcField;

        /// <summary>
        /// 目标字段名称
        /// </summary>
        string m_TargetField;

        /// <summary>
        /// 备注
        /// </summary>
        string m_Remark;

        /// <summary>
        /// 源字段名
        /// </summary>
        [Description("源字段名")]
        public string SrcField
        {
            get { return m_SrcField; }
            set { m_SrcField = value; }
        }

        /// <summary>
        /// 目标字段名称
        /// </summary>
        [Description("目标字段名称")]
        public string TargetField
        {
            get { return m_TargetField; }
            set { m_TargetField = value; }
        }

        /// <summary>
        /// 备注.
        /// </summary>
        [Description("")]
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }

    }
}
