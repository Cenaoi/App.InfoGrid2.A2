using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Web;
using EC5.Utility;
using System.Web.UI;
using System.Reflection;
using EasyClick.Web.Mini2.Data;

namespace EasyClick.Web.Mini2
{   

    /// <summary>
    /// 数据仓库
    /// </summary>
    [Description("数据仓库")]
    partial class Store
    {
        /// <summary>
        /// 删除参数
        /// </summary>
        ParamCollection m_DeleteQuery;

        /// <summary>
        /// 查询选择条件
        /// </summary>
        ParamCollection m_SelectQuery;

        /// <summary>
        /// 过滤查询
        /// </summary>
        ParamCollection m_FilterParams;

        /// <summary>
        /// 插入数据的条件
        /// </summary>
        ParamCollection m_InsertQuery;

        /// <summary>
        /// 插入数据的参数
        /// </summary>
        ParamCollection m_InsertParams;

        /// <summary>
        /// 更新查询条件
        /// </summary>
        ParamCollection m_UpdateQuery;

        /// <summary>
        /// 更新的参数
        /// </summary>
        ParamCollection m_UpdateParams;


        /// <summary>
        /// 假删除的参数
        /// </summary>
        ParamCollection m_DeleteRecycleParams;



        /// <summary>
        /// 更新数据查询条件
        /// </summary>
        [DefaultValue("")]
        [MergableProperty(false)]
        [Description("更新数据查询条件")]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        public ParamCollection UpdateQuery
        {
            get
            {
                if (m_UpdateQuery == null)
                {
                    m_UpdateQuery = new ParamCollection();
                }
                return m_UpdateQuery;
            }
        }

        /// <summary>
        /// 更新的参数集合
        /// </summary>
        [DefaultValue("")]
        [MergableProperty(false)]
        [Description("更新的参数集合")]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        public ParamCollection UpdateParams
        {
            get
            {
                if (m_UpdateParams == null)
                {
                    m_UpdateParams = new ParamCollection();
                }
                return m_UpdateParams;
            }
        }

        /// <summary>
        /// 删除回收模式的参数集合
        /// </summary>
        [DefaultValue("")]
        [MergableProperty(false)]
        [Description("删除回收模式的参数集合")]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        public ParamCollection DeleteRecycleParams
        {
            get
            {
                if (m_DeleteRecycleParams == null)
                {
                    m_DeleteRecycleParams = new ParamCollection();
                }
                return m_DeleteRecycleParams;
            }
        }

        /// <summary>
        /// 删除筛选条件集合
        /// </summary>
        [DefaultValue("")]
        [MergableProperty(false)]
        [Description("删除筛选条件集合")]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        public ParamCollection DeleteQuery
        {
            get
            {
                if (m_DeleteQuery == null)
                {
                    m_DeleteQuery = new ParamCollection();
                }
                return m_DeleteQuery;
            }
        }

        /// <summary>
        /// 选择筛选条件集合
        /// </summary>
        [DefaultValue("")]
        [Description("选择筛选条件集合")]
        [MergableProperty(false)]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        public ParamCollection SelectQuery
        {
            get
            {
                if (m_SelectQuery == null)
                {
                    m_SelectQuery = new ParamCollection();
                }
                return m_SelectQuery;
            }

        }


        /// <summary>
        /// 过滤参数集合
        /// </summary>
        [DefaultValue("")]
        [Description("过滤参数集合")]
        [MergableProperty(false)]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        public ParamCollection FilterParams
        {
            get
            {
                if (m_FilterParams == null)
                {
                    m_FilterParams = new ParamCollection();
                }
                return m_FilterParams;
            }
        }


        /// <summary>
        /// 插入查询条件集合
        /// </summary>
        [DefaultValue("")]
        [Description("插入查询条件集合")]
        [MergableProperty(false)]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        public ParamCollection InsertQuery
        {
            get
            {
                if (m_InsertQuery == null)
                {
                    m_InsertQuery = new ParamCollection();
                }
                return m_InsertQuery;
            }
        }


        /// <summary>
        /// 插入数据的参数
        /// </summary>
        [Description("插入数据的参数")]
        [DefaultValue("")]
        [MergableProperty(false)]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        public ParamCollection InsertParams
        {
            get
            {
                if (m_InsertParams == null)
                {
                    m_InsertParams = new ParamCollection();
                }
                return m_InsertParams;
            }
        }




    }
}
