using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace EasyClick.Web.Mini
{

    /// <summary>
    /// 数字范围
    /// </summary>
    [Description("日期选择框")]
    [DefaultProperty("FromDateValue")]
    [ToolboxData("<{0}:NumberRangeBox  runat=\"server\" />")]
    [ParseChildren(true), PersistChildren(false)]
    public class NumberRangeBox: Control, IAttributeAccessor, IClientIDMode, IMiniPanel,IMiniControl
    {
        NumberBox m_FromNumberBox = new NumberBox();

        NumberBox m_ToNumberBox = new NumberBox();

        /// <summary>
        /// 数字范围
        /// </summary>
        public NumberRangeBox()
        {
            this.SetAttribute("IsPanel", "true");

            //m_FromDatePicker.ID = "From";
            m_FromNumberBox.SetAttribute("class", "ui-widget-content");
            m_FromNumberBox.SetAttribute("style", "width:80px;");
            m_FromNumberBox.SetAttribute("DBType", "Decimal");
            m_FromNumberBox.SetAttribute("DBLogic",">=");
            m_FromNumberBox.SetAttribute("Groups","Search");
            m_FromNumberBox.SetAttribute("validate", "{number:true}");

            //m_ToDatePicker.ID = "To";
            m_ToNumberBox.SetAttribute("class", "ui-widget-content");
            m_ToNumberBox.SetAttribute("style", "width:80px;");
            m_ToNumberBox.SetAttribute("DBType", "Decimal");
            m_ToNumberBox.SetAttribute("DBLogic", "<=");
            m_ToNumberBox.SetAttribute("validate", "{number:true}");

        }

        
        public string FromNumberValue
        {
            get { return m_FromNumberBox.Value; }
            set { m_FromNumberBox.Value = value; }
        }


        public string ToNumberValue
        {
            get { return m_ToNumberBox.Value; }
            set { m_ToNumberBox.Value = value; }
        }



        protected override void CreateChildControls()
        {
            m_FromNumberBox.ID = this.ID + "_From";
            m_ToNumberBox.ID = this.ID + "_To";

            string dbField = this.GetAttribute("DBField");
            string groups = this.GetAttribute("Groups");

            m_FromNumberBox.SetAttribute("DBField", dbField);
            m_ToNumberBox.SetAttribute("DBField", dbField);

            m_FromNumberBox.SetAttribute("Groups", groups);
            m_ToNumberBox.SetAttribute("Groups", groups);


            this.Controls.Add(m_FromNumberBox);
            this.Controls.Add(m_ToNumberBox);
        }

        public override Control FindControl(string id)
        {
            return base.FindControl(id);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            EnsureChildControls();

            writer.Write("<span>");

            m_FromNumberBox.RenderControl(writer);
            
            writer.Write("&nbsp; 至&nbsp;");

            m_ToNumberBox.RenderControl(writer);

            writer.Write("</span>");
        }

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

            if ("DBField".Equals(key, StringComparison.CurrentCultureIgnoreCase))
            {
                m_FromNumberBox.SetAttribute("DBField", value);
                m_ToNumberBox.SetAttribute("DBField", value);
            }

            if ("DBType".Equals(key, StringComparison.CurrentCultureIgnoreCase))
            {
                m_FromNumberBox.SetAttribute("DBType", value);
                m_ToNumberBox.SetAttribute("DBType", value);
            }

            if ("Groups".Equals(key, StringComparison.CurrentCultureIgnoreCase))
            {
                m_FromNumberBox.SetAttribute("Groups", value);
                m_ToNumberBox.SetAttribute("Groups", value);
            }
        }

        #endregion

        /// <summary>
        /// 开始数字的数据字段
        /// </summary>
        [DefaultValue("")]
        [Description("开始数字的数据字段")]
        [Category("DBField 数据绑定")]
        public string FormDBField
        {
            get { return m_FromNumberBox.GetAttribute("DBField"); }
            set { m_FromNumberBox.SetAttribute("DBField",value); }
        }

        /// <summary>
        /// 结束数字的数据字段
        /// </summary>
        [DefaultValue("")]
        [Description("结束数字的数据字段")]
        [Category("DBField 数据绑定")]
        public string ToDBField
        {
            get { return m_ToNumberBox.GetAttribute("DBField"); }
            set { m_ToNumberBox.SetAttribute("DBField", value); }
        }


        #region IClientIDMode 成员

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

        #region IMiniControl 成员

        public void LoadPostData()
        {
            EnsureChildControls(); 

            string formId = this.GetClientID() + "_From";
            string toId = this.GetClientID() + "_To";

            string formValue = this.Page.Request.Form[formId];
            string toValue = this.Page.Request.Form[toId];

            this.m_FromNumberBox.Value = formValue;
            this.m_ToNumberBox.Value = toValue;
        }

        #endregion
    }
}
