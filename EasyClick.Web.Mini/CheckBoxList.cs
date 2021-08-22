using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;
using System.Collections.Specialized;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 复选框列表
    /// </summary>
    [DefaultProperty("Value")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:CheckBoxList  runat=\"server\" ><{0}:ListItem Value=\"Item1\" Text=\"项目1\" /><{0}:ListItem Value=\"Item2\" Text=\"项目2\" /></{0}:CheckBoxList>")]
    public class CheckBoxList : MiniHtmlListBase, IMiniControl
    {
        /// <summary>
        /// 复选框列表
        /// </summary>
        public CheckBoxList()
        {
            this.HtmlTag = System.Web.UI.HtmlTextWriterTag.Input;
        }

        RepeatLayout m_RepeatLayout = RepeatLayout.Table;

        RepeatDirection m_RepeatDirection = RepeatDirection.Vertical;

        int m_RepeatColumn = 0;

        TextAlign m_TextAlign = TextAlign.Right;

        List<string> m_Values = new List<string>();

        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if (value == null)
                {
                    base.Value = string.Empty;

                    if (m_Values != null)
                    {
                        m_Values.Clear();
                    }

                    return;
                }

                m_Values.Clear();
                m_Values.AddRange(value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

                base.Value = value;
            }
        }

        /// <summary>
        /// 文字
        /// </summary>
        [DefaultValue(TextAlign.Right)]
        public virtual TextAlign TextAlign
        {
            get { return m_TextAlign; }
            set { m_TextAlign = value; }
        }

        [DefaultValue(RepeatLayout.Table)]
        public RepeatLayout RepeatLayout
        {
            get { return m_RepeatLayout; }
            set { m_RepeatLayout = value; }
        }

        [DefaultValue(RepeatDirection.Vertical)]
        public RepeatDirection RepeatDirection
        {
            get { return m_RepeatDirection; }
            set { m_RepeatDirection = value; }
        }

        [DefaultValue(0)]
        public int RepeatColumn
        {
            get { return m_RepeatColumn; }
            set
            {
                m_RepeatColumn = value;
            }
        }


        protected override void RenderHtml(HtmlTextWriter writer)
        {
            //writer.Write("未完成!");

            if (this.RepeatLayout == Mini.RepeatLayout.Table)
            {
                if (this.RepeatDirection == Mini.RepeatDirection.Horizontal)
                {
                    TableHorizontal(writer);
                }
                else if (this.RepeatDirection == Mini.RepeatDirection.Vertical)
                {
                    TableVertical(writer);
                }
            }
            else if (this.RepeatLayout == Mini.RepeatLayout.Flow)
            {
                //if (this.RepeatDirection == Mini.RepeatDirection.Horizontal)
                //{

                //}

                FlowHorizontal(writer);
            }
        }


        private void FlowHorizontal(HtmlTextWriter writer)
        {
            if (this.RepeatColumn == 0)
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    RenderItem(writer, Items[i], i);
                }
            }
            else if (this.RepeatColumn > 0)
            {


                for (int i = 0; i < this.Items.Count; i++)
                {
                    RenderItem(writer, Items[i], i);

                    if (i > 0 && i % this.RepeatColumn == 0)
                    {
                        writer.WriteBreak();
                    }
                }
            }

        }

        /// <summary>
        /// 表格形式，按竖顺序显示
        /// </summary>
        /// <param name="writer"></param>
        private void TableVertical(System.Web.UI.HtmlTextWriter writer)
        {

            int col = m_RepeatColumn;

            if (col <= 0)
            {
                col = 1;
            }

            int m = this.Items.Count % col;

            int rowCount = (this.Items.Count - m) / col;

            if (m > 0)
            {
                rowCount++;
            }

            writer.AddAttribute("id", GetClientID());
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            //int itemIndex = 0;

            for (int row = 0; row < rowCount; row++)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                for (int c = 0; c < col; c++)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);

                    int itemIndex = rowCount * c + row;

                    if (itemIndex < this.Items.Count)
                    {
                        ListItem item = this.Items[itemIndex];

                        RenderItem(writer, item, itemIndex);
                    }
                    else
                    {
                        writer.Write("&nbsp;");
                    }

                    writer.RenderEndTag();

                    itemIndex++;
                }

                writer.RenderEndTag();
            }


            writer.RenderEndTag();
        }


        /// <summary>
        /// 表格形式，从左到右，从上到下
        /// </summary>
        /// <param name="writer"></param>
        private void TableHorizontal(System.Web.UI.HtmlTextWriter writer)
        {

            int col = m_RepeatColumn;

            if (col <= 0)
            {
                col = 1;
            }

            int m = this.Items.Count % col;

            int rowCount = (this.Items.Count - m) / col;

            if (m > 0)
            {
                rowCount++;
            }

            writer.AddAttribute("id", GetClientID());
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            int itemIndex = 0;

            for (int row = 0; row < rowCount; row++)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                for (int c = 0; c < col; c++)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);

                    if (itemIndex < this.Items.Count)
                    {
                        ListItem item = this.Items[itemIndex];

                        RenderItem(writer, item, itemIndex);
                    }
                    else
                    {
                        writer.Write("&nbsp;");
                    }

                    writer.RenderEndTag();

                    itemIndex++;
                }

                writer.RenderEndTag();
            }


            writer.RenderEndTag();

        }

        private void RenderItem( HtmlTextWriter writer,ListItem item,int index)
        {
            string parentId = GetClientID();

            string itemName = ClientID + "$" + index;
            string itemID = parentId + "_" + index;

            if (m_TextAlign == Mini.TextAlign.Left)
            {
                writer.AddAttribute("for", itemID);
                writer.RenderBeginTag(HtmlTextWriterTag.Label);
                writer.Write(item.Text);
                writer.RenderEndTag();
            }

            writer.AddAttribute("id", itemID);
            writer.AddAttribute("name", itemName);
            writer.AddAttribute("type", "checkbox");
            
            if (m_Values.Contains(item.Value))
            {
                writer.AddAttribute("checked", "checked");
            }

            writer.AddAttribute("value", item.Value);

            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            if (m_TextAlign == Mini.TextAlign.Right)
            {
                writer.AddAttribute("for", itemID);
                writer.RenderBeginTag(HtmlTextWriterTag.Label);
                writer.Write(item.Text);
                writer.RenderEndTag();
            }
        }

        /// <summary>
        /// 加载 Post 上传的数据
        /// </summary>
        public void LoadPostData()
        {
            string proExName = ClientID + "$";

            List<string> keyList = new List<string>();

            NameValueCollection form = this.Page.Request.Form;

            foreach (string key in form.Keys)
	        {
                if (key.StartsWith(proExName))
                {
                    keyList.Add(key);
                }
            }

            if (keyList == null || keyList.Count == 0)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            string key2 = keyList[0];
            string v = form[key2];

            sb.Append(v);

            for (int i = 1; i < keyList.Count; i++)
            {
                key2 = keyList[i];
                v = form[key2];

                sb.Append(",");
                sb.Append(v);
            }

            this.Value = sb.ToString();

        }
    }
}
