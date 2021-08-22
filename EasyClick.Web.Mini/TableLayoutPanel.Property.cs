using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace EasyClick.Web.Mini
{
    public enum LayoutItemFlowDirection
    {
        LeftToRight,
        TopDown,
        RightToLeft,
        BottomUp
    }

    public partial class TableLayoutPanel
    {

        #region ClientIDMode

        ClientIDMode m_ClientIDMode = ClientIDMode.AutoID;


        [DefaultValue(ClientIDMode.AutoID)]
        public ClientIDMode ClientIDMode
        {
            get { return m_ClientIDMode; }
            set { m_ClientIDMode = value; }
        }

        public string GetClientID()
        {
            string cId;

            switch (m_ClientIDMode)
            {
                case ClientIDMode.Static:
                    cId = this.ID;
                    break;
                default:
                    cId = this.ClientID;
                    break;
            }

            return cId;
        }

        #endregion


        #region Attribute

        internal MiniHtmlAttrCollection m_HtmlAttrs = new MiniHtmlAttrCollection();

        /// <summary>
        /// 是否存在此对应属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsAttr(string key)
        {
            return m_HtmlAttrs.ContainsAttr(key);
        }

        public string GetAttribute(string key)
        {
            return m_HtmlAttrs.GetAttribute(key);
        }

        public void SetAttribute(string key, string value)
        {
            m_HtmlAttrs.SetAttribute(key, value);
        }

        #endregion

        //ControlCollection m_Fields;

        bool m_HeaderVisible = true;

        VerticalAlign m_HeaderVAlign = VerticalAlign.Top;

        CellAlign m_HeaderAlign = CellAlign.Left;

        int m_FixedRowCount = 0;

        int m_FixedColCount = 1;

        LayoutItemFlowDirection m_FlowDirection = LayoutItemFlowDirection.LeftToRight;

        int m_HeaderWidth = 0;

        /// <summary>
        /// 标签垂直对齐方式
        /// </summary>
        [DefaultValue(VerticalAlign.Top)]
        public VerticalAlign HeaderVAlign
        {
            get { return m_HeaderVAlign; }
            set { m_HeaderVAlign = value; }
        }

        /// <summary>
        /// 标签垂直对齐方式
        /// </summary>
        [DefaultValue(CellAlign.Left)]
        public CellAlign HeaderAlign
        {
            get { return m_HeaderAlign; }
            set { m_HeaderAlign = value; }
        }


        /// <summary>
        /// 标题固定宽度
        /// </summary>
        [DefaultValue(0)]
        public int HeaderWidth
        {
            get { return m_HeaderWidth; }
            set { m_HeaderWidth = value; }
        }



        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ControlCollection Fields
        {
            get
            {
                
                //if (m_Fields == null)
                //{
                //    m_Fields = new ControlCollection(this);
                //}
               
                //return m_Fields;

                return base.Controls;
            }
        }

        public override ControlCollection Controls
        {
            get
            {
                return base.Controls;

                //if (m_Fields == null)
                //{
                //    m_Fields = new ControlCollection(this);
                //}

                //return m_Fields;
            }
        }

        [DefaultValue(true)]
        public bool HeaderVisible
        {
            get { return m_HeaderVisible; }
            set { m_HeaderVisible = value; }
        }

        [DefaultValue(0)]
        public int FixedRowCount
        {
            get { return m_FixedRowCount; }
            set { m_FixedRowCount = value; }
        }

        [DefaultValue(1)]
        public int FixedColCount
        {
            get { return m_FixedColCount; }
            set { m_FixedColCount = value; }
        }

        [DefaultValue(LayoutItemFlowDirection.LeftToRight)]
        public LayoutItemFlowDirection FlowDirection
        {
            get { return m_FlowDirection; }
            set { m_FlowDirection = value; }
        }


        /// <summary>
        /// 标题缩进像素
        /// </summary>
        int m_HeaderIndent = 0;

        string m_HeaderTemplate;
        string m_HeaderRequiredTemplate;

        string m_DefaultTableHeaderTemplate = "<th align=\"left\"  valign=\"top\"  style=\"font-weight:normal;@HeaderIndent;@HeaderWidth;@HeaderVAlign;@HeaderAlign;\"><nobr><label>{0}:</label></nobr></th>";
        string m_DefaultTableHeaderRequiredTemplate = "<th align=\"left\"  valign=\"top\"  style=\"font-weight:normal;@HeaderIndent;@HeaderWidth;@HeaderVAlign;@HeaderAlign\"><nobr><label>{0}:</label><em>*</em></nobr></th>";

        string m_DefaultFlowHeaderTemplate = "<label>{0}:</label>";
        /// <summary>
        /// 必填字段
        /// </summary>
        string m_DefaultFlowHeaderRequiredTemplate = "<label>{0}:</label><em>*</em>";

        /// <summary>
        /// 默认组
        /// </summary>
        string m_DefaultSplitBarTemplate = "<div style=\" margin-top:10px; margin-bottom:10px;\"><div style=\"float:left;\">{0}</div><hr /></div>";

        /// <summary>
        /// 组样式
        /// </summary>
        string m_SplitBarTemplate;

        /// <summary>
        /// 列间隔宽度（单位：像素）
        /// </summary>
        int m_ColSpaceWidth = 40;

        /// <summary>
        /// 输出边框
        /// </summary>
        bool m_WriteBorder = true;
        
        /// <summary>
        /// 输出边框
        /// </summary>
        [DefaultValue(true)]
        [Description("输出边框")]
        public bool WriteBorder
        {
            get { return m_WriteBorder; }
            set { m_WriteBorder = value; }
        }

        /// <summary>
        /// 缩进
        /// </summary>
        [DefaultValue(0)]
        public int HeaderIndent
        {
            get { return m_HeaderIndent; }
            set { m_HeaderIndent = value; }
        }

        /// <summary>
        /// 默认组模板
        /// </summary>
        [DefaultValue(""), PersistenceMode(PersistenceMode.InnerProperty)]
        public string SplitBarTemplate
        {
            get { return m_SplitBarTemplate; }
            set { m_SplitBarTemplate = value; }
        }


        /// <summary>
        /// 标题模板
        /// </summary>
        [MergableProperty(false), DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string HeaderTemplate
        {
            get { return m_HeaderTemplate; }
            set { m_HeaderTemplate = value; }
        }

        /// <summary>
        /// 必填标题模板
        /// </summary>
        [MergableProperty(false), DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string HeaderRequiredTemplate
        {
            get { return m_HeaderRequiredTemplate; }
            set { m_HeaderRequiredTemplate = value; }
        }


        /// <summary>
        /// 分割宽度
        /// </summary>
        [DefaultValue(40)]
        public int ColSpaceWidth
        {
            get { return m_ColSpaceWidth; }
            set { m_ColSpaceWidth = value; }
        }

    }
}
