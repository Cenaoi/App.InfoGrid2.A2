using System;
using System.Collections.Generic;
using System.Text;

namespace Excel_Template
{
    /// <summary>
    /// Excel 参数
    /// </summary>
    public class ExcelParam
    {


        public ExcelParam()
        {
        }

        /// <summary>
        /// Excel 参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        public ExcelParam(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Excel 参数集合
    /// </summary>
    public class ExcelParamCollection : SortedList<string, ExcelParam>
    {
        /// <summary>
        /// 添加子项
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, string value)
        {
            ExcelParam param = new ExcelParam(name, value);

            base[name] = param;
        }
    }

}
