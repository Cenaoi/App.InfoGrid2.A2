using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EC5.LCodeEngine
{
    /// <summary>
    /// 函数管理
    /// </summary>
    public static class JTemplateFuncManager
    {
        static JTemplateFuncList m_CommonFuncList;

        static JTemplateFuncManager()
        {
            m_CommonFuncList = new JTemplateFuncList();
        }

        public static JTemplateFuncList Commons
        {
            get { return m_CommonFuncList; }
        }
    }



    /// <summary>
    /// 给模板函数调用的
    /// </summary>
    public class JTemplateFunc
    {
        /// <summary>
        /// 函数名称
        /// </summary>
        public string Name { get; set; }
    
        public object Owner { get;set;}

        public MethodInfo Method { get;set;}

        public JTemplateFunc()
        {
        }

        public JTemplateFunc(string name, object owner, MethodInfo method)
        {
            this.Name = name;
            this.Owner = owner;
            this.Method = method;
        }

        public JTemplateFunc(object owner, MethodInfo method)
        {
            this.Owner = owner;
            this.Method = method;
            this.Name = method.Name;
        }

        
    }

    /// <summary>
    /// 模板函数方法集
    /// </summary>
    public class JTemplateFuncList
    {
        SortedList<string, JTemplateFunc> m_Items = new SortedList<string, JTemplateFunc>();

        object m_LockTag = new object();

        public void Add(JTemplateFunc func)
        {
            string key = func.Name.ToUpper();

            lock (m_LockTag)
            {
                m_Items.Add(key, func);
            }
        }

        public void Add(string methodName, object owner, MethodInfo methodInfo)
        {
            JTemplateFunc func = new JTemplateFunc(methodName, owner, methodInfo);

            this.Add(func);
        }

        public void Add(object owner, MethodInfo methodInfo)
        {
            JTemplateFunc func = new JTemplateFunc(owner, methodInfo);
            this.Add(func);
        }

        public bool TryGetItem(string name,out JTemplateFunc func)
        {
            string key = name.ToUpper();

            lock (m_LockTag)
            {
                return m_Items.TryGetValue(key, out func);
            }
        }

        public void Clear()
        {
            lock (m_LockTag)
            {
                m_Items.Clear();
            }
        }
    }
}
