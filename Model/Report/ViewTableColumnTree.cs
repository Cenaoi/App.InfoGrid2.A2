using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.Report
{
    /// <summary>
    /// 报表用的界面表格列的树新结构类
    /// </summary>
    [Serializable]
    public class ViewTableColumnTree
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key_text">关键字</param>
        /// <param name="ct">自定义表格列对象</param>
        public ViewTableColumnTree(string key_text, ViewTableColumnType ct)
        {

            this.ct = ct;
            this.key_text = key_text;

        }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ViewTableColumnTree() { }


        /// <summary>
        /// 表格列对象
        /// </summary>
        public ViewTableColumnType ct { get; set; }



        /// <summary>
        /// 关键字
        /// </summary>
        public string key_text { get; set; }

        private List<ViewTableColumnTree> m_col_trees;

        /// <summary>
        /// 子表格列集合
        /// </summary>
        public List<ViewTableColumnTree> childres
        {

            get
            {
                if (m_col_trees == null)
                {
                    m_col_trees = new List<ViewTableColumnTree>();
                }
                return m_col_trees;
            }

            set
            {
                m_col_trees = value;
            }

        }

        /// <summary>
        /// 查看是否有子节点
        /// </summary>
        /// <returns></returns>
        public bool is_has_children()
        {
            return childres.Count > 0;
        }

        /// <summary>
        /// 看看关键字是否已存在子节点中了
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public bool is_has_children_by_key(string key)
        {

            foreach (ViewTableColumnTree ct in childres)
            {

                if (ct.key_text == key)
                {
                    return true;
                }

            }

            return false;

        }

        /// <summary>
        /// 根据关键字返回子节点对象
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public ViewTableColumnTree GetChildreByKey(string key)
        {
            //看看是否已经含税和不含税表格列
            foreach (ViewTableColumnTree ct in childres)
            {
                if (ct.key_text != key)
                {
                    continue;
                }

                return ct;

            }

            return null;


        }

    }
}
