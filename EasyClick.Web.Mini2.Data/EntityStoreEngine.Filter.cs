using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.Filter;

namespace EasyClick.Web.Mini2.Data
{
    /// <summary>
    /// 数据仓库过滤条目
    /// </summary>
    public class StoreFilterItems : List<ConditionElement>
    {
        /// <summary>
        /// 添加过滤条件
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        public ConditionElement Add(string fieldName, object value)
        {
            if (string.IsNullOrEmpty(fieldName)) { throw new ArgumentNullException("fieldName", "字段名不能为空"); }

            ConditionElement item = new ConditionElement(fieldName, value, Logic.Equality);
            base.Add(item);

            return item;
        }

        /// <summary>
        /// 添加过滤条件
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">字段值</param>
        /// <param name="logic">逻辑</param>
        /// <returns></returns>
        public ConditionElement Add(string fieldName, object value, Logic logic)
        {
            if (string.IsNullOrEmpty(fieldName)) { throw new ArgumentNullException("fieldName","字段名不能为空"); }

            ConditionElement item = new ConditionElement(fieldName, value, logic);
            base.Add(item);

            return item;
        }
    }

    /// <summary>
    /// 实体仓库引擎
    /// </summary>
    partial	class EntityStoreEngine
    {
        #region 过滤参数

        StoreFilterItems m_FilterAll = new StoreFilterItems();
        StoreFilterItems m_FilterSelect = new StoreFilterItems();

        StoreFilterItems m_FilterDelete = new StoreFilterItems();
        StoreFilterItems m_FilterUpdate = new StoreFilterItems();

        StoreFilterItems m_FilterChangeStatus = new StoreFilterItems();


        StoreFilterItems m_NewDefaultValues = new StoreFilterItems();




        /// <summary>
        /// 过滤全部
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void FilterAll(string fieldName, object value)
        {
            m_FilterAll.Add(fieldName, value);
        }

        /// <summary>
        /// 改变状态过滤
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="logic"></param>
        public void FilterChangedStatus(string fieldName, object value, Logic logic)
        {
            m_FilterChangeStatus.Add(fieldName, value, logic);
        }

        /// <summary>
        /// 改变状态过滤
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void FilterChangedStatus(string fieldName, object value)
        {
            m_FilterChangeStatus.Add(fieldName, value);
        }


        /// <summary>
        /// 新记录默认值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void NewDefaultValues(string fieldName, object value)
        {
            m_NewDefaultValues.Add(fieldName, value);
        }

        /// <summary>
        /// 删除过滤
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="logic"></param>
        public void FilterDelete(string fieldName, object value, Logic logic)
        {
            m_FilterDelete.Add(fieldName, value, logic);
        }

        /// <summary>
        /// 删除过滤
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void FilterDelete(string fieldName, object value)
        {
            m_FilterDelete.Add(fieldName, value);
        }

        /// <summary>
        /// 选择查询过滤
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="logic"></param>
        public void FilterSelect(string fieldName, object value, Logic logic)
        {
            m_FilterSelect.Add(fieldName, value, logic);
        }

        /// <summary>
        /// 选择查询过滤
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void FilterSelect(string fieldName, object value)
        {
            m_FilterSelect.Add(fieldName, value, Logic.Equality);
        }

        #endregion
	}
}
