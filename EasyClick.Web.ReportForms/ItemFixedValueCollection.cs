using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.ReportForms
{
    /// <summary>
    /// 固定值的集合类
    /// </summary>
    public class ItemFixedValueCollection : List<ItemFixedValue>
    {
        /// <summary>
        /// 添加固定值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="text"></param>
        public void Add(string value, string text)
        {
            base.Add(new ItemFixedValue()
            {
                Text = text,
                Value = value
            });
        }

        /// <summary>
        /// 添加固定值
        /// </summary>
        /// <param name="value">值,可以单值,也可以用逗号隔开.</param>
        /// <param name="text">标题描述</param>
        /// <param name="operatorType">逻辑运算</param>
        public void Add(string value, string text, OperatorTypes operatorType)
        {

            base.Add(new ItemFixedValue()
            {
                Text = text,
                Value = value,
                Operator = operatorType
            });
        }


    }
}
