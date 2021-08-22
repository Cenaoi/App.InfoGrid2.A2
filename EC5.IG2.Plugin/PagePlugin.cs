using System;
using System.Collections.Generic;
using System.Web;
using EasyClick.Web.Mini2;

namespace EC5.IG2.Plugin
{
    
    /// <summary>
    /// 插件差数
    /// </summary>
    public class PagePluginEventArgs:EventArgs
    {
        string m_Action;

        SortedList<string,object> m_Params = new SortedList<string,object>();

        public SortedList<string,object> Params
        {
            get { return m_Params;}
        }

        public object this[string key]
        {
            get { return m_Params[key];}
        }

        /// <summary>
        /// 事件动作名
        /// </summary>
        public string Action
        {
            get { return m_Action;}
        }

        public  PagePluginEventArgs(string action,string key,object value)
        {
            m_Params[key] = value;
            m_Action = action;
        }


        public PagePluginEventArgs(string action)
        {
            m_Action = action;
        }
    }


    /// <summary>
    /// 页面插件
    /// </summary>
    public abstract class PagePlugin
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 用户事件
        /// </summary>
        public event EventHandler<PagePluginEventArgs> DymEvent;

        /// <summary>
        /// 触发用户事件
        /// </summary>
        /// <param name="action"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void OnDymEvent(string action, string key, object value)
        {
            if (DymEvent != null)
            {
                PagePluginEventArgs ea = new PagePluginEventArgs(action, key, value);
                DymEvent(this, ea);
            }
        }

        /// <summary>
        /// 触发用户事件
        /// </summary>
        /// <param name="action"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void OnDymEvent(PagePluginEventArgs ea)
        {
            if (DymEvent != null)
            {
                DymEvent(this, ea);
            }
        }



        System.Web.UI.Control m_Main;

        Store m_MainStore;

        Table m_MainTable;

        /// <summary>
        /// 触发源
        /// </summary>
        Store m_SrcStore;

        /// <summary>
        /// 触发源
        /// </summary>
        Table m_SrcTable;


        int m_ToolbarItemId = 0;

        string m_ClassName;
        string m_Method;

        string m_Params;

        string m_SrcUrl;






        /// <summary>
        /// 触发源地址
        /// </summary>
        public string SrcUrl
        {
            get { return m_SrcUrl; }
            set { m_SrcUrl = value; }
        }


        /// <summary>
        /// 触发源
        /// </summary>
        public Store SrcStore
        {
            get { return m_SrcStore; }
            set { m_SrcStore = value; }
        }

        /// <summary>
        /// 触发源
        /// </summary>
        public Table SrcTable
        {
            get { return m_SrcTable; }
            set { m_SrcTable = value; }
        }


        public int ToolbarItemId
        {
            get { return m_ToolbarItemId; }
            set { m_ToolbarItemId = value; }
        }

        /// <summary>
        /// 动态类名
        /// </summary>
        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }

        /// <summary>
        /// 执行主函数
        /// </summary>
        public string Method
        {
            get { return m_Method; }
            set { m_Method = value; }
        }

        /// <summary>
        /// 参数集合
        /// </summary>
        public string Params
        {
            get { return m_Params; }
            set { m_Params = value; }
        }

        /// <summary>
        /// 主版面
        /// </summary>
        public System.Web.UI.Control Main
        {
            get { return m_Main; }
            set { m_Main = value; }
        }

        /// <summary>
        /// 主数据仓库
        /// </summary>
        public Store MainStore
        {
            get { return m_MainStore; }
            set { m_MainStore = value; }
        }

        /// <summary>
        /// 主表
        /// </summary>
        public Table MainTable
        {
            get { return m_MainTable; }
            set { m_MainTable = value; }
        }


        /// <summary>
        /// 用户动态参数
        /// </summary>
        public string UserParams { get; set; }
    }
}