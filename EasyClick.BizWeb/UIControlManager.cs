using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;


namespace EasyClick.BizWeb
{
    /// <summary>
    /// UI 控件管理
    /// </summary>
    public static class UIControlManager
    {
        /// <summary>
        /// 控件集合,key=前缀，value=集合
        /// </summary>
        static SortedDictionary<string, NameControlIndex> m_TagControls = new SortedDictionary<string, NameControlIndex>();

        /// <summary>
        /// 初始化控件索引
        /// </summary>
        static void InitControlIndex()
        {
            m_TagControls.Add("mi", new NameControlIndex());
            m_TagControls.Add("biz", new NameControlIndex());


            NameControlIndex mi = m_TagControls["mi"];

            Type[] ts = typeof(EasyClick.Web.Mini.AutoCompleteBox).Assembly.GetExportedTypes();
            
            foreach (Type t in ts)
            {
                mi.Add(t);
            }

            NameControlIndex biz = m_TagControls["biz"];

            ts = typeof(EasyClick.BizWeb.UI.EcOctopus).Assembly.GetExportedTypes();

            foreach (Type t in ts)
            {
                biz.Add(t);
            }
        }

        static UIControlManager()
        {
            InitControlIndex();
        }



        public static void Add(string name, Type uiControl)
        {
            string[] sp = name.Split(':');

            if (sp.Length == 1)
            {
                NameControlIndex ncList = m_TagControls["mi"];

                ncList.Add(name, uiControl);
            }
            else if (sp.Length == 2)
            {
                NameControlIndex ncList = m_TagControls[sp[0]];

                ncList.Add(sp[1], uiControl);
            }
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <param name="tag">前缀</param>
        /// <param name="name">控件名称</param>
        /// <returns></returns>
        public static Type GetItem(string tag, string name)
        {
            if(!m_TagControls.ContainsKey(tag))
            {
                return null;
            }

            NameControlIndex ncList = m_TagControls[tag];

            if (!ncList.ContainsKey(name))
            {
                return null;
            }

            return ncList[name];
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <param name="tag">前缀</param>
        /// <returns></returns>
        public static Type[] GetItems(string tag)
        {
            if (!m_TagControls.ContainsKey(tag))
            {
                return new Type[0];
            }

            NameControlIndex ncList = m_TagControls[tag];

            Type[] ts = new Type[ncList.Count];

            ncList.Values.CopyTo(ts, 0);

            return ts;
        }


    }
}
