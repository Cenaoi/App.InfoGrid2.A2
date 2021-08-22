using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.Web.UI;
using System.ComponentModel;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 动作列、按钮组的列
    /// </summary>
    [Description("动作列、按钮组的列")]
    [ParseChildren(true, "Items"), PersistChildren(false)]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ActionColumn : BoundField
    {
        public ActionColumn()
        {
            this.MiType = "actioncolumn";
            this.EditorMode = Mini2.EditorMode.None;
            this.ItemAlign = Mini.CellAlign.Center;

        }

        public ActionColumn(string headerText)
        {
            this.HeaderText = headerText;

            this.MiType = "actioncolumn";
            this.EditorMode = Mini2.EditorMode.None;
            this.ItemAlign = Mini.CellAlign.Center;
        }

        /// <summary>
        /// 自动隐藏
        /// </summary>
        bool m_AutoHide = false;

        ActionItemCollection m_Items = new ActionItemCollection();

        /// <summary>
        /// 条目集合
        /// </summary>
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public ActionItemCollection Items
        {
            get
            {
                return m_Items;
            }
        }

        /// <summary>
        /// 自动隐藏
        /// </summary>
        [DefaultValue(false)]
        [Description("自动隐藏")]
        public bool AutoHide
        {
            get { return m_AutoHide; }
            set { m_AutoHide = value; }
        }


        public override string CreateHtmlTemplate(Mini.MiniDataControlCellType cellType, Mini.MiniDataControlRowState rowState)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            sb.AppendFormat("miType:'{0}', ", this.MiType);

            sb.AppendFormat("dataIndex:'{0}', ", this.DataField);
            sb.AppendFormat("width:{0}, ", this.Width);
            sb.AppendFormat("headerText:'{0}', ", this.HeaderText);
            sb.AppendFormat("autoHide: {0}, ",EC5.Utility.BoolUtil.ToJson(m_AutoHide));


            if (!this.Resizable)
            {
                sb.AppendFormat("resizable:{0}, ", this.Resizable.ToString().ToLower());
            }



            //sb.Append(CreateEditor());

            CreateItems(sb);

            sb.AppendFormat("align:'{0}'}}", this.ItemAlign.ToString().ToLower());

            return sb.ToString();
        }

        private void CreateItems(StringBuilder sb)
        {
            sb.Append("items:[");

            if (this.Items.Count > 0)
            {
                string itemJson;
                for (int i = 0; i < m_Items.Count; i++)
                {
                    if (i > 0) { sb.Append(","); }

                    itemJson = CreateItemHtml(m_Items[i]);

                    sb.Append(itemJson);
                }
            }

            sb.Append("], ");
        }
       

        private string CreateItemHtml(ActionItem item)
        {

            StringBuilder sb = new StringBuilder();

            
            sb.Append("{");

            sb.AppendFormat("icon:'{0}', ", item.Icon);
            sb.AppendFormat("tooltip:'{0}', ", EC5.Utility.JsonUtil.ToJson(item.Tooltip));

            sb.AppendFormat("text: '{0}', ", EC5.Utility.JsonUtil.ToJson(item.Text));

            if (!StringUtil.IsBlank(item.Href))
            {
                sb.AppendFormat("href :'{0}',", item.Href);
            }

            if (!StringUtil.IsBlank(item.Target))
            {
                sb.AppendFormat("target:'{0}',", item.Target);
            }

            if (!StringUtil.IsBlank(item.TargetText))
            {
                sb.AppendFormat("targetText:'{0}',", EC5.Utility.JsonUtil.ToJson(item.TargetText));
            }

            

            if (item.DisplayMode != DisplayMode.Auto)
            {
                sb.AppendFormat("displayMode:'{0}',", item.DisplayMode.ToString().ToLower());
            }

            if (this.SortMode != Mini.SortMode.Default)
            {
                sb.AppendFormat("sortable:'{0}', ", this.SortMode.ToString().ToLower());
            }

            if (!StringUtil.IsBlank(item.ClickHandler))
            {
                sb.AppendFormat("clickHandler: {0},", item.ClickHandler);
            }
            else if (!StringUtil.IsBlank(item.Click)) {

                sb.Append("handler: function (grid, cell, recordIndex, cellIndex, e, record, row) {");

                sb.Append(item.Click);

                sb.Append("; },");
            }
            else if (!StringUtil.IsBlank(item.Command))
            {
                sb.Append("handler: function (grid, cell, recordIndex, cellIndex, e, record, row) {");

                sb.AppendFormat(" grid.onCommand.call(grid, '{0}', '{1}', record);", item.Command, item.CommandParam);

                sb.Append("},");
            }
            else if (StringUtil.IsBlank(item.Handler))
            {
                sb.Append("handler: function (grid, cell, recordIndex, cellIndex, e, record, row) {");

                sb.Append("},");
            }
            else
            {
                sb.AppendFormat("handler: {0},", item.Handler);
            }

            sb.Append("scope: this ");

            sb.Append("}");

            return sb.ToString();
        }

    }





}
