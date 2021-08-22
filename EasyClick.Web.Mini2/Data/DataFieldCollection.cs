using System;
using System.Collections.Generic;
using System.Text;
using EC5.Utility;

namespace EasyClick.Web.Mini2.Data
{
    /// <summary>
    /// 字段集合
    /// </summary>
    public class DataFieldCollection : List<DataField>
    {

        /// <summary>
        /// 转换为 Json 对象
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();

            ToJson(sb);

            return sb.ToString();
        }

        /// <summary>
        /// 转换为 Json 对象
        /// </summary>
        /// <param name="sb"></param>
        public void ToJson(StringBuilder sb)
        {
            int n = 0;

            sb.Append("{");

            foreach (DataField field in this)
            {
                if(field == null)
                {
                    continue;
                }

                if(n ++ > 0) { sb.Append(","); }

                sb.Append($"\"{field.Name}\":");

                sb.AppendFormat("\"{0}\"", JsonUtil.ToJson(field.Value));

            }

            sb.Append("}");
        }


        /// <summary>
        /// 判断包含字段
        /// </summary>
        /// <param name="name">字段名称</param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            bool exist = false;

            foreach (DataField item in this)
            {
                if (name.Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                {
                    exist = true;
                    break;
                }
            }

            return exist;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">字段名称</param>
        /// <returns></returns>
        public DataField this[string name]
        {
            get
            {
                foreach (DataField item in this)
                {
                    if (name.Equals(item.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return item;
                    }
                }

                return null;
            }
        }

    }

}
