using EasyClick.Web.Mini;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Permissions;
using System.Text;
using System.Web;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 列的分组
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [System.Web.UI.ParseChildren(true, "Columns"), System.Web.UI.PersistChildren(false)]
    public class GroupColumn:BoundField, IEnumerable<BoundField>
    {

        public GroupColumn()
        {
        }

        public GroupColumn(string headerText)
        {
            this.HeaderText = headerText;
        }

        public GroupColumn(IEnumerable<BoundField> items)
        {
            if (items != null)
            {
                this.Columns.AddRange(items);
            }
        }

        public void Add(BoundField item)
        {
            this.Columns.Add(item);


        }

        public void AddRange(IEnumerable<BoundField> items)
        {
            this.Columns.AddRange(items);
        }
        /// <summary>
        /// 子列集合
        /// </summary>
        TableColumnCollection m_Columns;

        /// <summary>
        /// 子列集合
        /// </summary>
        [Description("映射集合")]
        [DefaultValue(null)]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        [MergableProperty(false)]
        public TableColumnCollection Columns
        {
            get
            {
                if (m_Columns == null)
                {
                    m_Columns = new TableColumnCollection(m_Owner);
                }
                return m_Columns;
            }
        }


        /// <summary>
        /// 是否有之列集合
        /// </summary>
        /// <returns></returns>
        public bool HasSubCollum()
        {
            return (m_Columns != null && m_Columns.Count > 0);
        }


        /// <summary>
        /// 创建子列集合
        /// </summary>
        protected virtual string CreateColumnsTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            if (m_Columns == null)
            {
                return string.Empty;
            }

            int i = 0;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[").Append("    ");

            foreach (var col in m_Columns)
            {
                if (i++ > 0) { sb.AppendLine(",").Append("    "); }

                string colTemplateJs = col.CreateHtmlTemplate(cellType, rowState);

                sb.Append(colTemplateJs);
            }

            sb.AppendLine("]");

            return sb.ToString();
        }


        /// <summary>
        /// 创建 Html 模板
        /// </summary>
        /// <param name="cellType"></param>
        /// <param name="rowState"></param>
        /// <returns></returns>
        public override string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            sb.AppendFormat("miType:'{0}', ", this.MiType);

            sb.AppendFormat("dataIndex:'{0}', ", this.DataField);
            sb.AppendFormat("headerText:'{0}', ", this.HeaderText);

            if (!string.IsNullOrEmpty(this.Tag))
            {
                sb.AppendFormat("tag:'{0}',", MiniHelper.GetItemJson(this.Tag));
            }

            if (!this.Sortable)
            {
                sb.AppendFormat("sortable:{0}, ", this.Sortable.ToString().ToLower());
            }

            if (this.SortMode != Mini.SortMode.Default)
            {
                sb.AppendFormat("sortable:'{0}', ", this.SortMode.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(this.SortExpression))
            {
                sb.AppendFormat("sortExpression:'{0}', ", this.SortExpression);
            }


            if (!this.Resizable)
            {
                sb.AppendFormat("resizable:{0}, ", this.Resizable.ToString().ToLower());
            }

            if (this.ItemAlign != Mini.CellAlign.Left)
            {
                sb.AppendFormat("align:'{0}', ", this.ItemAlign.ToString().ToLower());
            }

            if (this.HeaderAlign != Mini.CellAlign.Left)
            {
                sb.AppendFormat("headerAlign:'{0}', ", this.HeaderAlign.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(this.Click))
            {
                sb.AppendFormat("click: {0}, ", this.Click);
            }

            //if (!string.IsNullOrEmpty(this.SummaryType))
            //{
            //    sb.AppendFormat("summaryType: '{0}',", this.SummaryType);
            //}

            if (!string.IsNullOrEmpty(this.Renderer))
            {
                sb.AppendFormat("renderer: {0},", this.Renderer);
            }

            //sb.Append(CreateEditor());

            if (this.HasSubCollum())
            {
                sb.Append("columns:").Append(CreateColumnsTemplate(cellType, rowState)).Append(",");
            }

            sb.AppendFormat("width:{0}", this.Width);
            sb.Append("}");

            return sb.ToString();
        }


        public IEnumerator<BoundField> GetEnumerator()
        {
            return m_Columns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Columns.GetEnumerator();
        }
    }
}
