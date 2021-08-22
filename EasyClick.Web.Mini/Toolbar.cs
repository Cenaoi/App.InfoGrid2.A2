using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 工具栏
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items"), PersistChildren(false)]
    [Description("工具栏")]
    public class ToolBar : Control,ISecControl
    {
        /// <summary>
        /// 设计模式
        /// </summary>
        bool m_MiniDesignMode = false;

        /// <summary>
        /// 设计模式
        /// </summary>
        public void SetDesignMode(bool value)
        {
            m_MiniDesignMode = value;
        }

        /// <summary>
        /// 设计模式
        /// </summary>
        /// <returns></returns>
        public bool GetDesignMode()
        {
            return m_MiniDesignMode;
        }

        public new bool DesignMode
        {
            get
            {
                if (m_MiniDesignMode)
                {
                    return true;
                }

                return base.DesignMode;
            }
        }



        ToolBarItemCollection m_Items;

        bool m_Visible = true;

        [DefaultValue(true)]
        public override bool Visible
        {
            get
            {
                return m_Visible;
            }
            set
            {
                m_Visible = value;
            }
        }

        /// <summary>
        /// 条目集合
        /// </summary>
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public ToolBarItemCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new ToolBarItemCollection();
                }
                return m_Items;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.DesignMode && !m_Visible)
            {
                return;
            }

            writer.Write("<div class='ButtonPanel' ");



            if (!m_Visible)
            {
                writer.Write("style='display:none;' ");
            }

            writer.Write(">");

            foreach (ToolBarItem item in this.Items)
            {
                if (!item.Visible)
                {
                    continue;
                }

                item.Render(writer);
            }

            writer.Write("</div>");
        }


        #region 权限设置


        string m_SecFunCode;
        string m_SecFunID;

        SecControlCollection m_SecControls;

        /// <summary>
        /// 
        /// </summary>
        [Category("Security")]
        public string SecFunCode
        {
            get { return m_SecFunCode; }
            set { m_SecFunCode = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        [Category("Security")]
        public string SecFunID
        {
            get { return m_SecFunID; }
            set { m_SecFunID = value; }
        }

        /// <summary>
        /// 权限组件集合
        /// </summary>
        [Category("Security")]
        [Browsable(false)]
        public SecControlCollection SecControls
        {
            get
            {
                if (m_SecControls == null)
                {
                    m_SecControls = new SecControlCollection();

                    foreach (ISecControl item in m_Items)
                    {
                        if (item == null)
                        {
                            continue;
                        }

                        m_SecControls.Add(item);
                    }
                }


                return m_SecControls;
            }
        }

        #endregion
    }


}
