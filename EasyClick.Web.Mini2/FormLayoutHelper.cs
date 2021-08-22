using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 表单转换
    /// </summary>
    public static class FormLayoutHelper
    {
        static SortedList<string, Type> m_Bindings = new SortedList<string, Type>();

        /// <summary>
        /// 添加版面
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindings"></param>
        public static void Add(string name, Type bindings)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name", "表单名不能为空");

            if (bindings == null) throw new ArgumentNullException("bindings", "绑定的对象类型不能为 null");

            if (m_Bindings.ContainsKey(name))
            {
                m_Bindings[name] = bindings;
            }
            else
            {
                m_Bindings.Add(name, bindings);
            }
        }

        /// <summary>
        /// 设置 FormLayout 版面的数据
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="model"></param>
        /// <param name="data"></param>
        public static void SetData(FormLayout layout, string model, object data)
        {
            if (layout == null) throw new ArgumentNullException("layout", "UI 表单面板名不能为空");


            Type bindT = m_Bindings["ENTITY"];

            IFormLayoutBindings formLB = (IFormLayoutBindings)Activator.CreateInstance(bindT);
            formLB.Model = model;

            formLB.SetData(layout, data);
        }

        /// <summary>
        /// 获取 FormLayout 版面的数据
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static object GetData(FormLayout layout, string model)
        {
            if (layout == null) throw new ArgumentNullException("layout", "UI 表单面板名不能为空");

            Type bindT = m_Bindings["ENTITY"];

            IFormLayoutBindings formLB = (IFormLayoutBindings)Activator.CreateInstance(bindT);

            formLB.Model = model;

            object data = formLB.GetData(layout);

            return data;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="model"></param>
        public static void EditData(FormLayout layout, object model)
        {
            if (layout == null) throw new ArgumentNullException("layout", "UI 表单面板名不能为空");


            Type bindT = m_Bindings["ENTITY"];

            IFormLayoutBindings formLB = (IFormLayoutBindings)Activator.CreateInstance(bindT);

            formLB.EditData(layout, model);

        }
    }
}
