using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using EasyClick.Web.Mini.Utility;
using System.IO;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 表格版面
    /// </summary>
    [ToolboxData("<{0}:TableLayoutPanel runat=\"server\" ><Fields></Fields></{0}:TableLayoutPanel>")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public partial class TableLayoutPanel : Control, IAttributeAccessor, IMiniControl
    {
        enum MergeType
        {
            None,
            /// <summary>
            /// 主单元格
            /// </summary>
            First,
            /// <summary>
            /// 隐藏
            /// </summary>
            Hide
        }

        class BaseControlList : List<Control>
        {
            public BaseControlList()
                : base(1)
            {
            }

            MergeType m_Merge = MergeType.None;

            public MergeType Merge
            {
                get { return m_Merge; }
                set { m_Merge = value; }
            }
        }

        private void Render_SplitBar(Control con, IAttributeAccessor attrs, string headerTemplate, HtmlTextWriter writer)
        {
            writer.WriteLine("<td colspan=\"{0}\">", 2);

            StringWriter sw = new StringWriter();
            HtmlTextWriter txtWr = new HtmlTextWriter(sw);

            con.RenderControl(txtWr);

            string txt = sw.ToString();

            txtWr.Dispose();
            sw.Dispose();

            if (string.IsNullOrEmpty(m_SplitBarTemplate))
            {
                writer.Write(m_DefaultSplitBarTemplate, txt);
            }
            else
            {
                writer.Write(m_SplitBarTemplate, txt);
            }

            writer.WriteLine("</td>");
        }

        public override Control FindControl(string id)
        {
            return base.FindControl(id);
        }

        private void RenderColumnTD(Control con, IAttributeAccessor attrs, string headerTemplate, HtmlTextWriter writer)
        {

            bool headerVisible = true;
            string headerText = string.Empty;
            int colspan = 1;

            string beforeText = null;
            string afterText = null;

            bool isSplitBar = false;

            if (attrs != null)
            {
                headerVisible = StringUtility.ToBool(attrs.GetAttribute("HeaderVisible"), true);
                headerText = attrs.GetAttribute("HeaderText");
                colspan = StringUtility.ToInt(attrs.GetAttribute("ColSpan"), 1);

                beforeText = attrs.GetAttribute("BeforeText");
                afterText = attrs.GetAttribute("AfterText");    

                isSplitBar = StringUtility.ToBool(attrs.GetAttribute("IsSplitBar"), false); //分隔符标记
            }


            if (isSplitBar)
            {
                Render_SplitBar(con, attrs, headerTemplate, writer);
                return;
            }


            if (headerVisible)
            {
                if (!string.IsNullOrEmpty(headerText))
                {
                    writer.WriteLine(headerTemplate, headerText);
                }
                else
                {
                    writer.Write("<td>&nbsp;</td>");
                }
            }


            if (headerVisible)
            {
                if (colspan > 1)
                {
                    writer.WriteLine("<td colspan=\"{0}\" style='padding-right:{1}px;' valign=\"top\"  >", colspan, m_ColSpaceWidth);
                }
                else
                {
                    writer.WriteLine("<td style='padding-right:{0}px;' valign=\"top\"  >", m_ColSpaceWidth);
                }
            }
            else
            {
                if (colspan > 1)
                {
                    writer.WriteLine("<td colspan=\"{0}\" style='padding-right:{1}px;' valign=\"top\" >", colspan, m_ColSpaceWidth);
                }
                else
                {
                    writer.WriteLine("<td colspan=\"2\" style='padding-right:{0}px;' valign=\"top\"  >", m_ColSpaceWidth);
                }
            }

            if (!string.IsNullOrEmpty(beforeText))
            {
                writer.Write(beforeText);
            }

            con.RenderControl(writer);

            if (!string.IsNullOrEmpty(afterText))
            {
                writer.Write(afterText);
            }


            writer.WriteLine("</td>");
        }


        private int GetRowLen()
        {
            if (m_FixedRowCount > 0)
            {
                return m_FixedRowCount;
            }

            int rowM = this.Fields.Count % (m_FixedColCount <= 0 ? 1 : m_FixedColCount);

            int rowLen = (this.Fields.Count - rowM) / (m_FixedColCount <= 0 ? 1 : m_FixedColCount);

            if (rowM > 0) { rowLen++; }

            return rowLen;
        }

        private int GetColLen()
        {
            if (m_FixedColCount > 0)
            {
                return m_FixedColCount;
            }

            int colM = this.Fields.Count % (m_FixedRowCount <= 0 ? 1 : m_FixedRowCount);

            int colLen = (this.Fields.Count - colM) / (m_FixedRowCount <= 0 ? 1 : m_FixedRowCount);

            if (colM > 0) { colLen++; }

            return colLen;
        }

        private BaseControlList[,] GetTopDown()
        {
            int rowLen = GetRowLen();
            int colLen = GetColLen();

            BaseControlList[,] tmpConList = new BaseControlList[rowLen, colLen];

            int conIndex = 0;
            
            for (int col = 0; col < colLen; col++)
            {
                for (int row = 0; row < rowLen; row++)
                {

                    if (conIndex >= this.Controls.Count)
                    {
                        continue;
                    }

                    if (tmpConList[row, col] == null)
                    {
                        tmpConList[row, col] = new BaseControlList();
                    }

                    Control con = this.Controls[conIndex];

                    tmpConList[row, col].Add(con);

                    conIndex++;
                }
            }

            return tmpConList;
        }


        class CellControlList : List<BaseControlList>
        {

        }

        

        private BaseControlList[,] GetLeftToRight()
        {
            //int rowLen = GetRowLen();
            int colLen = GetColLen();

            List<CellControlList> rows = new List<CellControlList>();

            int conIndex = 0;
            int conLen = this.Controls.Count;

            CellControlList row = null;

            int maxColLen = 0;

            int colIndex = 0;

            for (int i = 0; i < conLen; i++)
            {
                if (row == null)
                {
                    row = new CellControlList();

                    rows.Add(row);
                }


                Control con = this.Controls[conIndex];

                row.Add(new BaseControlList());

                row[colIndex].Add(con);

                conIndex++;

                colIndex++;
                maxColLen = Math.Max(maxColLen, colIndex);

                IAttributeAccessor attrs = con as IAttributeAccessor;

                if (attrs != null)
                {
                    int colspan = StringUtility.ToInt(attrs.GetAttribute("ColSpan"), 1);

                    if (colspan > 2)
                    {
                        int m = colspan % 2;
                        int m2 = (colspan - m) / 2;

                        if (m > 0) { m2++; }

                        for (int j = 1; j < m2; j++)
                        {
                            BaseControlList bc = new BaseControlList();
                            bc.Merge = MergeType.Hide;
                            row.Add(bc);

                            colIndex++;

                            maxColLen = Math.Max(maxColLen, colIndex);
                        }
                    }
                }

                if (colIndex >= colLen)
                {
                    maxColLen = Math.Max(maxColLen, colIndex);

                    row = null;
                    colIndex = 0;
                }

            }

            BaseControlList[,] tmpConList = new BaseControlList[rows.Count, maxColLen];

            int tRowIndex = 0;
            int tColIndex = 0;

            foreach (CellControlList tRow in rows)
            {
                for (int i = 0; i < tRow.Count; i++)
                {
                    tmpConList[tRowIndex, i] = tRow[i];
                }

                tRowIndex++;
            }

            return tmpConList;


        }

        private void RenderTable(HtmlTextWriter writer)
        {
            int rowLen = 0;// GetRowLen();
            int colLen = 0;// GetColLen();

            BaseControlList[,] tmpConList = null;

            if (m_FlowDirection == LayoutItemFlowDirection.LeftToRight)
            {
                tmpConList = GetLeftToRight();
            }
            else if (m_FlowDirection == LayoutItemFlowDirection.TopDown)
            {
                tmpConList = GetTopDown();
            }

            rowLen = tmpConList.GetLength(0);
            colLen = tmpConList.GetLength(1);

            if (tmpConList == null) { return; }


            string headerTemplate = string.IsNullOrEmpty(m_HeaderTemplate) ? m_DefaultTableHeaderTemplate : m_HeaderTemplate;
            string headerRequiredTemplate = string.IsNullOrEmpty(m_HeaderRequiredTemplate) ? m_DefaultTableHeaderRequiredTemplate : m_HeaderRequiredTemplate;

            string stylePaddingLeft = string.Format("padding-left:{0}px;", m_HeaderIndent);
            string headerWidth = (m_HeaderWidth > 0 ? string.Format("width:{0}px", m_HeaderWidth) : string.Empty);
            string headerVAlign = "vertical-align:" + m_HeaderVAlign.ToString().ToLower();
            string headerAlign = "text-align:" + m_HeaderAlign.ToString().ToLower();     


            headerTemplate = headerTemplate.Replace("@HeaderIndent", stylePaddingLeft)
                .Replace("@HeaderWidth", headerWidth)
                .Replace("@HeaderVAlign", headerVAlign)
                .Replace("@HeaderAlign", headerAlign);

            headerRequiredTemplate = headerRequiredTemplate.Replace("@HeaderIndent", stylePaddingLeft)
                .Replace("@HeaderWidth", headerWidth)
                .Replace("@HeaderVAlign", headerVAlign)
                .Replace("@HeaderAlign", headerAlign);
            


            int n = 0;

            if (m_WriteBorder)
            {
                writer.WriteLine("<table id='{0}' ", GetClientID());

                foreach (MiniHtmlAttr attr in m_HtmlAttrs.Values)
                {
                    writer.Write("{0}=\"{1}\" ", attr.Key, attr.Value);
                }

                writer.WriteLine(">");
            }

            int conIndex = 0;

            

            for (int row = 0; row < rowLen; row++)
            {
                writer.WriteLine("<tr>");

                for (int col = 0; col < colLen; col++)
                {
                    if (tmpConList[row, col] == null)
                    {
                        continue;
                    }

                    BaseControlList conList = tmpConList[row, col];

                    if (conList == null)
                    {
                        continue;
                    }

                    if (conList.Count > 0 && conList[0] != null)
                    {
                        Control con = conList[0];

                        IAttributeAccessor attrs = con as IAttributeAccessor;

                        bool wrap = false;
                        bool required = false;

                        if (attrs != null)
                        {
                            wrap = StringUtility.ToBool(attrs.GetAttribute("Wrap"), false);     //换行
                            required = StringUtility.ToBool(attrs.GetAttribute("Required"), false); //必填标记
                        }

                        //if (wrap)
                        //{
                        //    writer.WriteLine("<td style='width:{0}px;'></td>", m_ColSpaceWidth);
                        //}

                        if (required)
                        {
                            RenderColumnTD(con, attrs, headerRequiredTemplate, writer);
                        }
                        else
                        {
                            RenderColumnTD(con, attrs, headerTemplate, writer);
                        }

                        conIndex++;
                    }
                    else
                    {
                        if (conList.Merge != MergeType.Hide)
                        {
                            writer.Write("<td></td><td></td>");
                        }
                    }

                    //writer.WriteLine("<td style='width:{0}px;'></td>", m_ColSpaceWidth);

                }

                writer.WriteLine("</tr>");
            }


            if (m_WriteBorder)
            {
                writer.WriteLine("</table>");
            }

        }



        protected override void Render(HtmlTextWriter writer)
        {
            EnsureChildControls();

            RenderTable(writer);

        }

        #region IMiniControl 成员

        public void LoadPostData()
        {

        }

        #endregion
    }
}
