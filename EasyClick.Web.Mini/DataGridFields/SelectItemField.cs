using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;
using System.IO;
using System.Web.UI;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 选择记录
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class SelectItemField : EditorTextCell 
    {

        /// <summary>
        /// 选择记录
        /// </summary>
        public SelectItemField()
        {
            this.ItemAlign = CellAlign.Center;
            this.Width = 16;
            m_AllowInput = false;
            
            this.ID = "SelectedItem";

        }

        private new int Width
        {
            get { return base.Width; }
            set { base.Width = value; }
        }

        [DefaultValue(CellAlign.Center)]
        private new CellAlign ItemAlign
        {
            get
            {
                return base.ItemAlign;
            }
            set
            {
                base.ItemAlign = value;
            }
        }

        [DefaultValue(0)]
        private new string DataField
        {
            get { return base.DataField; }
            set { base.DataField = value; }
        }

        
        static Random m_Random;

        static SelectItemField()
        {
            Random ro = new Random(10);
            long tick = DateTime.Now.Ticks;
            m_Random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
        }

        int m_ColGuid = 0;


        public override void RenderClientScript(HtmlTextWriter writer)
        {
            if (m_ColGuid == 0)
            {
                m_ColGuid = m_Random.Next();
            }

            string colGuid = "C" + m_ColGuid;

            DataGridView owner = (DataGridView)this.Owner;

            string columnID = this.ID;
            string colId = this.Owner.ClientID + "_" + this.ID;

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }


            if (jsMode == "InJs")
            {
                writer.WriteLine("In('mi.SelectItemField',function(){");
            }

            writer.Write("var {0}_EditorCell = new Mini.ui.SelectItemField({{", columnID);
            writer.Write("dataStoreStatusId:'{0}'", owner.GetClientID() + "_DSStatus");
            writer.Write(", colGuid:'{0}'", colGuid);
            writer.Write(", ownerAllId:'#{0}'", colGuid);
            writer.WriteLine("});");

            writer.WriteLine("{0}_EditorCell.setGridView( {1} );", columnID, owner.GetClientID());
            //writer.WriteLine("{0}_EditorCell.setDataStore({1}.getDataStore());", columnID, owner.GetClientID());

            writer.WriteLine("{0}.setColumnEditor('{1}', {1}_EditorCell);", owner.GetClientID(), columnID);

            writer.WriteLine("window.{0}_EditorCell = {1}_EditorCell;", colId, columnID);

            if (jsMode == "InJs")
            {
                writer.WriteLine("});");
            }

            writer.WriteLine();
        }

        public override string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            TextWriter tw = new StringWriter();

            HtmlTextWriter writer = new HtmlTextWriter(tw);


            string columnID = this.ID;
            string colId = this.Owner.ClientID + "_" + this.ID;
            
            if (m_ColGuid == 0)
            {
                m_ColGuid = m_Random.Next();
            } 
            
            string colGuid = "C" + m_ColGuid;


            if (this.Width > 0)
            {
                writer.AddAttribute("width", this.Width + "px");
            }

            if (!this.Visible)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            }

            if (cellType == MiniDataControlCellType.Header)
            {
                if (this.ItemAlign != CellAlign.Left)
                {
                    writer.AddAttribute("align", this.ItemAlign.ToString().ToLower());
                }

                writer.AddAttribute("nowrap", "nowrap");
                writer.RenderBeginTag(HtmlTextWriterTag.Th);


                writer.AddAttribute("id", colGuid);
                writer.AddAttribute("type", "checkbox");
                writer.AddAttribute("onclick", string.Format("{0}_EditorCell.CheckedAll(this);",colId));
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();


                writer.RenderEndTag();
            }
            else if (cellType == MiniDataControlCellType.DataCell)
            {
                writer.AddAttribute("align", this.ItemAlign.ToString().ToLower());

                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.AddAttribute("type", "checkbox");

                writer.AddAttribute(colGuid, "");
                writer.AddAttribute("onclick", string.Format("{0}_EditorCell.click(this);", colId));

                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();

                writer.RenderEndTag();
            }
            else
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write("&nbsp;");
                writer.RenderEndTag();
            }

            string txt = tw.ToString();

            writer.Dispose();
            tw.Dispose();

            return txt;
        }


    }
}
