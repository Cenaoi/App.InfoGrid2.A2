using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Security.Permissions;
using System.ComponentModel;
using System.IO;

namespace EasyClick.Web.Mini
{

    /// <summary>
    /// 基础表格类
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public class GridBase : Control
    {
        /// <summary>
        /// 允许排序
        /// </summary>
        bool m_AllowSorting = false;

        DataGridColumnCollection m_Columns;


        /// <summary>
        /// 隔行显示的样式
        /// </summary>
        string m_OddClass = "odd";

        /// <summary>
        /// 隔行显示的样式
        /// </summary>
        [DefaultValue("odd")]
        public string OddClass
        {
            get { return m_OddClass; }
            set { m_OddClass = value; }
        }

        [DefaultValue(false)]
        public bool AllowSorting
        {
            get { return m_AllowSorting; }
            set { m_AllowSorting = value; }
        }



        #region 事件

        /// <summary>
        /// 排序的事件
        /// </summary>
        [Description("排序的事件")]
        public event EventHandler Sort;

        /// <summary>
        /// 触发排序的事件
        /// </summary>
        protected void OnSort()
        {
            if (Sort != null) { Sort(this, EventArgs.Empty); }
        }



        #endregion

        /// <summary>
        /// 数据列集合
        /// </summary>
        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("数据列集合")]
        public virtual DataGridColumnCollection Columns
        {
            get
            {
                if (m_Columns == null)
                {
                    m_Columns = new DataGridColumnCollection(this);
                }

                return m_Columns;
            }
        }

        GridRowCollection m_Rows;

        public GridRowCollection Rows
        {
            get
            {
                if (m_Rows == null)
                {
                    m_Rows = new GridRowCollection(this);
                }

                return m_Rows;
            }
        }


        string m_EmptyDataText;

        /// <summary>
        /// 没有数据记录的时候，显示的文字
        /// </summary>
        [Description("没有数据记录的时候，显示的文字")]
        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string EmptyDataText
        {
            get { return m_EmptyDataText; }
            set { m_EmptyDataText = value; }
        }



        DataStoreControl m_DataStore = null;

        public DataStoreControl DataStore
        {
            get { return m_DataStore; }
            set
            {
                m_DataStore = value;
            }
        }

        private void ListenDtaStore(DataStoreControl store)
        {
            store.Selected += new DataListEventHandler(store_Selected);
            store.Selecting += new DataListCancelEventHandler(store_Selecting);
        }

        void store_Selecting(object sender, DataListCancelEventArgs e)
        {
            DataStore_Selecting(e);
        }

        void store_Selected(object sender, DataListEventArgs e)
        {
            DataStore_Selected(e);
        }

        protected void DataStore_Selecting(DataListCancelEventArgs e)
        {

        }

        protected void DataStore_Selected(DataListEventArgs e)
        {
            foreach (object data in e.DataList)
            {
                GridRow row = new GridRow(data);

                this.Rows.Add(row);
            }
        }


        /// <summary>
        /// 创建 Hand 的模板
        /// </summary>
        /// <returns></returns>
        protected virtual string CreateHead()
        {
            return string.Empty;
        }

        /// <summary>
        /// 创建 body 的行模板
        /// </summary>
        /// <returns></returns>
        protected virtual string CreateColGroup()
        {

            StringBuilder sb = new StringBuilder();


            foreach (BoundField col in m_Columns)
            {
                sb.AppendLine("<colgroup>");
                {
                    sb.AppendFormat("<col style=\"width:{0}px;\">", col.Width).AppendLine();
                }
                sb.AppendLine("</colgroup>");
            }

            return sb.ToString();
        }


        /// <summary>
        /// 创建没一行记录的模板
        /// </summary>
        /// <returns></returns>
        protected virtual string CreateBodyRowTemplate()
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(sw);

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);


            writer.RenderEndTag();

            return sw.ToString();
        }


        protected override void Render(HtmlTextWriter writer)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<table>");
            {
                foreach (BoundField col in m_Columns)
                {
                    sb.AppendLine("<colgroup>");
                    {
                        sb.AppendFormat("<col style=\"width:{0}px;\">",col.Width).AppendLine();
                    }
                    sb.AppendLine("</colgroup>");
                }
            }
            sb.AppendLine("</table>");

        }

        
    }


}
