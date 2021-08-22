using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web.UI;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{

    public enum EditorImeMode
    {
        Auto,
        Active,
        Inactive,
        Disabled
    }

    [Description("可编辑文本框")]
    public class EditorTextCell:BoundField
    {

        string m_ID;

        /// <summary>
        /// 默认值
        /// </summary>
        string m_DefaultValue;

        bool m_Required = false;

        string m_OnClientTextChanged;

        EditorImeMode m_ImeMode = EditorImeMode.Auto;

        /// <summary>
        /// 输入法模式
        /// </summary>
        [DefaultValue(EditorImeMode.Auto)]
        public virtual EditorImeMode ImeMode
        {
            get { return m_ImeMode; }
            set { m_ImeMode = value; }
        }

        [DefaultValue("")]
        [Description("客户端脚本,文本发生变化")]
        public string OnClientTextChanged
        {
            get { return m_OnClientTextChanged; }
            set { m_OnClientTextChanged = value; }
        }



        /// <summary>
        /// 默认值
        /// </summary>
        [DefaultValue("")]
        public virtual string DefaultValue
        {
            get { return m_DefaultValue; }
            set { m_DefaultValue = value; }
        }

        [DefaultValue(0)]
        public virtual string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        /// <summary>
        /// 必填
        /// </summary>
        [DefaultValue(false)]
        public bool Required
        {
            get { return m_Required; }
            set { m_Required = value; }
        }

        /// <summary>
        /// 输出客户端脚本
        /// </summary>
        /// <param name="writer"></param>
        public virtual void RenderClientScript(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.ReadOnly)
            {
                return;
            }

            if (Owner is DataGridView && ((DataGridView)Owner).ReadOnly)
            {
                return;
            }

            DataGridView owner = (DataGridView)this.Owner;

            string columnID = string.IsNullOrEmpty(this.ID) ? this.DataField : this.ID;


            //string colGuid = owner.ID + "_" + columnID;

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }


            if (jsMode == "InJs")
            {
                writer.WriteLine("In('mi.EditorTextCell', function(){");
            }

            writer.Write("var {0}_EditorCell = new Mini.ui.EditorTextCell({{", columnID);

            if (this.ImeMode != EditorImeMode.Auto)
            {
                writer.Write("imeMode:'{0}',", this.ImeMode.ToString().ToLower());
            }

            writer.Write("dataStoreStatusId:'{0}'", owner.GetClientID() + "_DSStatus");
            writer.WriteLine("});");

            writer.WriteLine("{0}_EditorCell.setGridView( {1} );", columnID, owner.GetClientID());

            if (!string.IsNullOrEmpty(m_OnClientTextChanged))
            {
                writer.WriteLine("{0}_EditorCell.textChanged( function(sender,e){{ {1} }} );", columnID, m_OnClientTextChanged);
            }


            writer.WriteLine("$('#{0} tbody td[ColumnID={1}]').mousedown(function(){{", owner.GetClientID(), columnID);
            writer.WriteLine("    {0}_EditorCell.show(this);", columnID);
            writer.WriteLine("});");

            //writer.WriteLine("if({0}.setColumnEditor){{", owner.GetClientID());
            writer.WriteLine("{0}.setColumnEditor('{1}', {1}_EditorCell);", owner.GetClientID(), columnID);
            //writer.WriteLine("}");

            if (jsMode == "InJs")
            {
                writer.WriteLine("});");
            }
        }


        protected override void CreateHeaderHtmlTemplate(HtmlTextWriter writer, MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            if (!this.HeaderVisible)
            {
                return;
            }

            if (this.Required)
            {
                writer.AddAttribute("class", "Required ");
            }


            base.CreateHeaderHtmlTemplate(writer, cellType, rowState);
        }


        /// <summary>
        /// 创建 Html 模板
        /// </summary>
        /// <param name="cellType"></param>
        /// <param name="rowState"></param>
        /// <returns></returns>
        public override string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            TextWriter tw = new StringWriter();

            HtmlTextWriter writer = new HtmlTextWriter(tw);

            DataGridView owner = (DataGridView)this.Owner;

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
                CreateHeaderHtmlTemplate(writer, MiniDataControlCellType.Header, rowState);
            }
            else if (cellType == MiniDataControlCellType.DataCell)
            {
                if (ItemAlign != CellAlign.Left)
                {
                    writer.AddAttribute("align", ItemAlign.ToString().ToLower());
                }


                string isEditor = "editor ";
                string isReadOnly = (this.ReadOnly?"readonly ":"");

                if (this.Required)
                {
                    writer.AddAttribute("class", "Required " + isEditor + isReadOnly);
                }
                else
                {
                    writer.AddAttribute("class", isEditor + isReadOnly);
                }


                writer.AddAttribute("nowrap", "nowrap");
                writer.AddAttribute("DBField", this.DataField);
                writer.AddAttribute("ColumnID", string.IsNullOrEmpty(this.ID)?this.DataField:this.ID);

                //if (this.Required)
                //{
                //    writer.AddAttribute("class", "Required");
                //}


                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                //writer.Write(string.Format("{{$T.{0}}}", this.DataField));

                if (string.IsNullOrEmpty(this.NotDisplayValue))
                {
                    //writer.Write(string.Format("{{$T.{0}}}", this.DataField));
                    writer.Write(GetDataBindExp(this.DataField));
                }
                else
                {
                    writer.Write("{{#if $T.{0} == '{1}' }}&nbsp;{{#else}}{{$T.{0}}}{{#/if}}", this.DataField, NotDisplayValue);
                }

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
