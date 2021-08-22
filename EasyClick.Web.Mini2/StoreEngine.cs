using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace EasyClick.Web.Mini2
{
    


    /// <summary>
    /// 数据仓库引擎,负责跟数据库交互
    /// </summary>
    public class StoreEngine : IStoreEngine
    {

        internal Store m_Store;

        int m_CurPage;

        /// <summary>
        /// (构造函数)数据仓库引擎
        /// </summary>
        /// <param name="store">仓库对象</param>
        public StoreEngine(Store store)
        {
            m_Store = store;
        }

        /// <summary>
        /// (构造函数)数据仓库引擎
        /// </summary>
        public StoreEngine()
        {
            
        }

        /// <summary>
        /// 数据仓库
        /// </summary>
        public Store Store
        {
            get { return m_Store; }
            set { m_Store = value; }
        }


        public virtual void OnLoad()
        {

        }


        public virtual bool ContainsListCollection
        {
            get { throw new NotImplementedException(); }
        }


        /// <summary>
        /// 当期那页面索引
        /// </summary>
        [Description("当期页面索引")]
        public int CurPage
        {
            get{ return m_CurPage; }
            set { m_CurPage = value;  }
        }


        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public virtual IList LoadPage(int page)
        {

            throw new NotImplementedException();
        }

        /// <summary>
        /// 选择
        /// </summary>
        /// <returns></returns>
        public virtual IList Select()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <returns>更新的记录数量</returns>
        public virtual int Update()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <returns></returns>
        public virtual int Insert()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public virtual int Delete()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public virtual IList GetList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetDataTable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 保存全部
        /// </summary>
        /// <returns></returns>
        public virtual int SaveAll()
        {
            throw new NotImplementedException();
        }

        int m_ItemTotal;
        int m_PageSize;

        /// <summary>
        /// 记录总数量
        /// (这个属性分页的时候才起作用)。
        /// </summary>
        [Description("记录总数量")]
        public int ItemTotal
        {
            get { return m_ItemTotal; }
            set { m_ItemTotal = value; }
        }

        /// <summary>
        /// 每页显示的数量
        /// (这个属性分页的时候才起作用)。
        /// </summary>
        [Description("每页显示的数量")]
        public int PageSize
        {
            get { return m_PageSize; }
            set { m_PageSize = value; }
        }

        /// <summary>
        /// 上移
        /// </summary>
        /// <returns></returns>
        public virtual bool MoveUp()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 下移
        /// </summary>
        /// <returns></returns>
        public virtual bool MoveDown()
        {
            throw new NotImplementedException();
        }


        public virtual decimal GetSummary(string field, SummaryType summartyType, ParamCollection summaryFilter)
        {
            throw new NotImplementedException();
        }


        public virtual decimal GetSummary(string field, SummaryType summartyType)
        {
            throw new NotImplementedException();
        }


        public virtual void SetCurRecord(Data.DataRecord record)
        {
            throw new NotImplementedException();
        }


        public virtual bool SortReset()
        {
            throw new NotImplementedException();
        }



        public virtual void LoadSummary()
        {
            throw new NotImplementedException();
        }


        public virtual void PreCurrentChanged(Data.DataRecord record,object data)
        {

        }

        public virtual bool HasChild(object parent)
        {

            throw new NotImplementedException();
        }
    }
}
