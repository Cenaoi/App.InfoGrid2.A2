using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{


    /// <summary>
    /// 单选列表
    /// </summary>
    [DefaultProperty("Value")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:RadioButtonList  runat=\"server\" ><{0}:ListItem Value=\"Item1\" Text=\"项目1\" /><{0}:ListItem Value=\"Item2\" Text=\"项目2\" /></{0}:RadioButtonList>")]
    public class RadioButtonList:MiniHtmlListBase
    {
        /// <summary>
        /// 单选列表
        /// </summary>
        public RadioButtonList()
        {

        }

        RepeatLayout m_RepeatLayout = RepeatLayout.Table;

        RepeatDirection m_RepeatDirection = RepeatDirection.Vertical;

        int m_RepeatColumn = 0;

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

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
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

                    int itemIndex = rowCount * c + row ;

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
                col = this.Items.Count;// int.MaxValue;
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

        /// <summary>
        /// 输出选项条目
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="item"></param>
        /// <param name="index"></param>
        private void RenderItem(System.Web.UI.HtmlTextWriter writer, ListItem item,int index)
        {
            writer.AddAttribute("id", string.Format("{0}_{1}", GetClientID(), index));

            writer.AddAttribute("type", "radio");
            writer.AddAttribute("name", this.ClientID);

            writer.AddAttribute("value", item.Value);

            if (item.Value == this.Value)
            {
                writer.AddAttribute("checked", "checked");
            }
            writer.AddStyleAttribute("float", "left");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);

            writer.RenderEndTag();

            writer.AddAttribute("for", string.Format("{0}_{1}", GetClientID(), index));
            writer.AddStyleAttribute("float", "left");
            writer.RenderBeginTag(HtmlTextWriterTag.Label);
            writer.WriteEncodedText(item.Text);
            writer.RenderEndTag();

        }
    }
}
