using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;
using EasyClick.Web.Mini.Utility;
using System.IO;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 详细视图
    /// </summary>
    [ToolboxData("<{0}:DetailsView runat=\"server\" >" +
        "<Fields>" +
            "<span HeaderText=\"日期\" >" +
            "<{0}:DatePicker runat=\"server\" DBField=\"DATE_BLL\" DBLogic=\">=\" Groups=\"Search\"  validate=\"{{date:true}}\" /> 至 " +
            "<{0}:DatePicker runat=\"server\" DBField=\"DATE_BLL\" DBLogic=\"<=\" Groups=\"Search\"  validate=\"{{date:true}}\" /> " +
            "</span>" +
            "<{0}:TextBox runat=\"server\" HeaderText=\"代码\" DBField=\"CODE\" DBLogic=\"like\" Groups=\"Search\" />" +
            "<{0}:Button runat=\"server\" Value=\"查询\" Groups=\"Search\" />" +
        "</Fields>" +
        "</{0}:DetailsView>")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public partial class DetailsView : Control, IAttributeAccessor, IMiniControl
    {
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

        private void RenderColumnTD(Control con,IAttributeAccessor attrs,string headerTemplate, HtmlTextWriter writer)
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
                colspan = StringUtility.ToInt(attrs.GetAttribute("ColSpan"),1);

                beforeText = attrs.GetAttribute("BeforeText");
                afterText = attrs.GetAttribute("AfterText");

                isSplitBar = StringUtility.ToBool(attrs.GetAttribute("IsSplitBar"),false);
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
                    writer.WriteLine("<td colspan=\"{0}\">",colspan);
                }
                else
                {
                    writer.WriteLine("<td>");
                }
            }
            else
            {
                if (colspan > 1)
                {
                    writer.WriteLine("<td colspan=\"{0}\">", colspan);
                }
                else
                {
                    writer.WriteLine("<td colspan=\"2\">");
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


            //con.RenderControl(writer);

            writer.WriteLine("</td>");
        }

        private void RenderTable(HtmlTextWriter writer)
        {

            string headerTemplate = string.IsNullOrEmpty(m_HeaderTemplate) ? m_DefaultTableHeaderTemplate : m_HeaderTemplate;
            string headerRequiredTemplate = string.IsNullOrEmpty(m_HeaderRequiredTemplate) ? m_DefaultTableHeaderRequiredTemplate : m_HeaderRequiredTemplate;

            //if (m_HeaderIndent > 0)
            //{
                string stylePaddingLeft = string.Format("padding-left:{0}px;", m_HeaderIndent);
                string headerWidth = (m_HeaderWidth > 0? string.Format("width:{0}px",m_HeaderWidth):string.Empty);

                headerTemplate = headerTemplate.Replace("@HeaderIndent", stylePaddingLeft)
                    .Replace("@HeaderWidth", headerWidth);

                headerRequiredTemplate = headerRequiredTemplate.Replace("@HeaderIndent", stylePaddingLeft)
                    .Replace("@HeaderWidth", headerWidth); ;
            //}

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


            writer.WriteLine("<tr>");

            foreach (Control con in this.Fields)
            {
                IAttributeAccessor attrs = con as IAttributeAccessor;

                bool wrap = false;
                bool required = false;

                if (attrs != null)
                {
                    wrap = StringUtility.ToBool(attrs.GetAttribute("Wrap"), false);
                    required = StringUtility.ToBool(attrs.GetAttribute("Required"), false);
                }

                if (!wrap && n > 0 && n < m_Fields.Count)
                {
                    writer.WriteLine("</tr>");
                    writer.WriteLine("<tr>");
                }

                if (wrap)
                {
                    writer.WriteLine("<td style='width:{0}px;'></td>", m_ColSpaceWidth);
                }

                if (required)
                {
                    RenderColumnTD(con, attrs, headerRequiredTemplate, writer);
                }
                else
                {
                    RenderColumnTD(con, attrs, headerTemplate, writer);
                }

                n++;
            }

            writer.WriteLine("</tr>");

            if (m_WriteBorder)
            {
                writer.WriteLine("</table>");
            }

        }

        private void RenderColumnP(Control con, IAttributeAccessor attrs, string headerTemplate, HtmlTextWriter writer)
        {

            bool headerVisible = true;
            string headerText = string.Empty;
            int colspan = 1;

            string afterText = null;
            string beforeText = null;

            if (attrs != null)
            {
                headerVisible = StringUtility.ToBool(attrs.GetAttribute("HeaderVisible"), true);
                headerText = attrs.GetAttribute("HeaderText");
                colspan = StringUtility.ToInt(attrs.GetAttribute("ColSpan"), 1);

                afterText = attrs.GetAttribute("AfterText");
                beforeText = attrs.GetAttribute("BeforeText");
            }



            writer.WriteLine("<p style='margin:0px;float:left; padding-right:{0}px;'>",m_ColSpaceWidth); 

            if (headerVisible)
            {
                if (!string.IsNullOrEmpty(headerText))
                {
                    writer.WriteLine(headerTemplate, headerText);
                }
                else
                {
                    writer.WriteLine("<label></label>");
                }
            }


            //控件前置文字
            if (!string.IsNullOrEmpty(beforeText))
            {
                writer.Write(beforeText);
            }

            con.RenderControl(writer);

            //控件后置文字
            if (!string.IsNullOrEmpty(afterText))
            {
                writer.Write(afterText);
            }

            writer.WriteLine("</p>");
        }

        private void RenderFlow(HtmlTextWriter writer)
        {

            string headerTemplate = string.IsNullOrEmpty(m_HeaderTemplate) ? m_DefaultFlowHeaderTemplate : m_HeaderTemplate;
            string headerRequiredTemplate = string.IsNullOrEmpty(m_HeaderRequiredTemplate) ? m_DefaultFlowHeaderRequiredTemplate : m_HeaderRequiredTemplate;

            
            int n = 0;

            //writer.WriteLine("<table id='{0}' ", GetClientID());

            //foreach (MiniHtmlAttr attr in m_HtmlAttrs.Values)
            //{
            //    writer.Write("{0}=\"{1}\" ", attr.Key, attr.Value);
            //}

            //writer.WriteLine(">");


            //writer.WriteLine("<tr>");

            foreach (Control con in this.Fields)
            {
                IAttributeAccessor attrs = con as IAttributeAccessor;

                bool wrap = false;
                bool required = false;

                if (attrs != null)
                {
                    wrap = StringUtility.ToBool(attrs.GetAttribute("Wrap"), false);
                    required = StringUtility.ToBool(attrs.GetAttribute("Required"), false);
                }

                if (required)
                {
                    RenderColumnP(con, attrs, headerRequiredTemplate, writer);
                }
                else
                {
                    RenderColumnP(con, attrs, headerTemplate, writer);
                }

                n++;
            }

            //writer.WriteLine("</tr>");

            //writer.WriteLine("</table>");

        }

        protected override void Render(HtmlTextWriter writer)
        {
            EnsureChildControls();

            if (m_RepeatLayout == Mini.RepeatLayout.Table)
            {
                RenderTable(writer);
            }
            else if(m_RepeatLayout == Mini.RepeatLayout.Flow)
            {
                RenderFlow(writer);
            }
        }




        public void LoadPostData()
        {
            UserControl c = FindWidget();

            SetMiniControlValue(c.ClientID);
        }

        private UserControl FindWidget()
        {
            Control con = this.Parent;

            for (int i = 0; i < 9; i++)
            {
                if (con is UserControl)
                {
                    break;
                }

                if (con.Parent == null)
                {
                    break;
                }

                con = con.Parent;
            }

            return (UserControl)con;
        }

        public override bool HasControls()
        {
            if (m_Fields == null || m_Fields.Count == 0)
            {
                return false;
            }

            return true;
        }

        protected Control FindControl(Control parent, string id)
        {
            if (!parent.HasControls())
            {
                return null;
            }

            ControlCollection cons = parent.Controls;

            for (int i = 0; i < cons.Count; i++)
            {
                Control con = cons[i];

                if (id.Equals(con.ID))
                {
                    return con;
                }

                Control subCon = FindControl(con, id);

                if (subCon != null)
                {
                    return subCon;
                }
            }

            return null;
        }

        public override Control FindControl(string id)
        {
            return FindControl(this, id);
        }


        protected void SetMiniControlValue(string cid)
        {

            HttpContext content = HttpContext.Current;

            foreach (string item in content.Request.Form.Keys)
            {
                if (item == null || item.Length < cid.Length || !item.StartsWith(cid))
                {
                    continue;
                }

                string keyName = item.Substring(cid.Length + 1);

                Control child = this.FindControl(keyName);

                if (child == null)
                {
                    continue;
                }

                string value = content.Request.Form[item];

                if (child is CheckBox)
                {
                    ((CheckBox)child).Checked = true;
                }
                else
                {
                    MiniHelper.SetValue(child, value);
                }
            }

            //foreach (Control conSub in this.Controls)
            //{
            //    if (conSub is IMiniControl)
            //    {
            //        ((IMiniControl)conSub).LoadPostData();
            //    }
            //}

        }
    }
}
