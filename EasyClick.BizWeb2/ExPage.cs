using EasyClick.Web.Mini2;
using EC5.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace EasyClick.BizWeb2
{

    /// <summary>
    /// 扩展页面的脚本
    /// </summary>
    public class ExPageScript
    {

    }


    /// <summary>
    /// 扩展页面的脚本集合
    /// </summary>
    public class ExPageScriptCollection:ExPageScript
    {

    }


    public class ExPageScriptManager
    {
        
    }


    /// <summary>
    /// 用户自定义页面
    /// </summary>
    public abstract class ExPage : Control, IDisposable
    {
        Control m_Owner;

        List<Control> m_UserControls;

        /// <summary>
        /// 页面类型 GRID=表格模式, FORM=表单类型
        /// </summary>
        public string PageType { get; set; }

        /// <summary>
        /// 查询
        /// </summary>
        Control m_MainSearch = null;

        /// <summary>
        /// 主数据仓库
        /// </summary>
        Store m_MainStore = null;

        /// <summary>
        /// 主工具栏
        /// </summary>
        Toolbar m_MainToolbar = null;
        /// <summary>
        /// 主表格
        /// </summary>
        Table m_MainTable = null;

        /// <summary>
        /// 初始化完成
        /// </summary>
        bool m_IsInitialized = false;



        public bool IsPostBack
        {
            get
            {
                bool isPost = false;

                string isPostStr = this.Page.Request.QueryString["__IsPost"];

                if (isPostStr == "1" || isPostStr == "true")
                {
                    isPost = true;
                }

                return isPost;
            }
        }

        /// <summary>
        /// 主查询界面
        /// </summary>
        public Control MainSearch
        {
            get { return m_MainSearch; }
        }

        /// <summary>
        /// 主数据仓库
        /// </summary>
        public Store MainStore
        {
            get { return m_MainStore; }
        }

        /// <summary>
        /// 主表
        /// </summary>
        public Table MainTable
        {
            get { return m_MainTable; }
        }

        /// <summary>
        /// 主工具栏
        /// </summary>
        public Toolbar MainToolbar
        {
            get { return m_MainToolbar; }
        }

        /// <summary>
        /// 所属控件
        /// </summary>
        public Control Owner
        {
            get { return m_Owner; }
        }

        /// <summary>
        /// 动态创建的控件集合
        /// </summary>
        public List<Control> UserControls
        {
            get { return m_UserControls; }
            set { m_UserControls = value; }
        }

        /// <summary>
        /// 初始化函数
        /// </summary>
        protected virtual void OnInit()
        {

        }

        /// <summary>
        /// 初始化加载
        /// </summary>
        protected virtual void OnLoad()
        {

        }

        /// <summary>
        /// 根据控件ID，查找控件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Control Find(string id)
        {
            return FindControl(id);
        }

        /// <summary>
        /// 根据控件ID，查找控件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected Control FindControl(string id)
        {
            if (m_UserControls == null)
            {
                throw new Exception("这对象已经释放内存，无法继续操作");
            }

            Control con = m_UserControls.Find(delegate(Control a) { return a.ID == id; });

            if (con != null)
            {
                return con;
            }

            return m_Owner.FindControl(id);
        }

        /// <summary>
        /// 控件实例化后，进行初始化
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="mainSearch"></param>
        /// <param name="mainStore"></param>
        /// <param name="mainTable"></param>
        public void SetDefaultValue(Control owner,
            Control mainSearch, Store mainStore,Toolbar mainToolbar, Table mainTable)
        {
            m_Owner = owner;
            m_MainSearch = mainSearch;
            m_MainStore = mainStore;
            m_MainToolbar = mainToolbar; 
            m_MainTable = mainTable;

            m_IsInitialized = true;
        }

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        public bool IsInitialized
        {
            get { return m_IsInitialized; }
        }

        public void OnProInit()
        {
            this.OnInit();
        }

        public void OnProLoad()
        {

            this.OnLoad();
        }


        /// <summary>
        /// 获取调用的函数 js
        /// </summary>
        /// <param name="methodName">函数名称</param>
        /// <returns></returns>
        public string GetMethodJs(string methodName)
        {
            return GetMethodJs(methodName, null);
        }

        /// <summary>
        /// 获取调用的函数 js
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public string GetMethodJs(string methodName, string methodParams)
        {
            string js ;

            if (string.IsNullOrEmpty(methodParams))
            {
                js = string.Format("widget1.subMethod('form:first',{{subName:'{0}', subMethod:'{1}'}}) || true;", this.ID, methodName);
            }
            else
            {
                if (StringUtil.StartsWith(methodParams, "{") && StringUtil.EndsWith(methodParams, "}"))
                {

                }
                else
                {
                    methodParams = string.Concat("'", methodParams, "'");
                }

                js = string.Format("widget1.subMethod('form:first',{{subName:'{0}', subMethod:'{1}', commandParam:{2} }}) || true;", this.ID, methodName, methodParams);
            }

            return js;
        }




        /// <summary>
        /// 释放内存
        /// </summary>
        public void Dispose()
        {
            if (m_UserControls != null)
            {
                m_UserControls.Clear();
                m_UserControls = null;
            }

            m_MainSearch = null;
            m_MainStore = null;
            m_MainTable = null;
            m_Owner = null;

            base.Dispose();

            GC.SuppressFinalize(this);
        }
    }

}