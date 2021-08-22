using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini
{
    public enum ClientIDMode
    {
        /// <summary>
        /// ClientID 值是通过串联每个父命名容器的 ID 值生成的，这些父命名容器都具有控件的 ID 值。 
        /// 在呈现控件的多个实例的数据绑定方案中，将在控件的 ID 值的前面插入递增的值。 
        /// 各部分之间以下划线字符 (_) 分隔。 
        /// 在 ASP.NET 4 之前的 ASP.NET 版本中使用此算法。 
        /// </summary>
        AutoID,
        /// <summary>
        /// ClientID 值设置为 ID 属性的值。 如果控件是命名容器，则该控件将用作其所包含的任何控件的命名容器的顶层。 
        /// </summary>
        Static,
        /// <summary>
        /// 对于数据绑定控件中的控件使用此算法。 
        /// ClientID 值是通过串联每个父命名容器的 ClientID 值生成的，这些父命名容器都具有控件的ID 值。 
        /// 如果控件是生成多个行的数据绑定控件，则在末尾添加 ClientIDRowSuffix 属性中指定的数据字段的值。 
        /// 对于 GridView 控件，可以指定多个数据字段。 
        /// 如果 ClientIDRowSuffix 属性为空白，则在末尾添加顺序号，而非数据字段值。 
        /// 此号码由零开始，并且为每个行递增 1。 
        /// 各部分之间以下划线字符 (_) 分隔。 
        /// </summary>
        Predictable,
        /// <summary>
        /// 控件继承其父控件的 ClientIDMode 设置。 
        /// </summary>
        Inherit
    }
}
