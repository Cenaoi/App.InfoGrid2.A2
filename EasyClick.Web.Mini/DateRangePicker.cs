using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 日期范围
    /// </summary>
    [Description("日期选择框")]
    [DefaultProperty("FromDateValue")]
    [ToolboxData("<{0}:DateRangePicker  runat=\"server\" />")]
    [ParseChildren(true), PersistChildren(false)]
    public class DateRangePicker : Control, IAttributeAccessor, IClientIDMode, IMiniPanel,IMiniControl
    {
        DatePicker m_FromDatePicker = new DatePicker();

        DatePicker m_ToDatePicker = new DatePicker();

        public DateRangePicker()
        {
            this.SetAttribute("IsPanel", "true");

            //m_FromDatePicker.ID = "From";
            m_FromDatePicker.SetAttribute("class", "ui-widget-content date");
            m_FromDatePicker.SetAttribute("style", "width:80px;");
            m_FromDatePicker.SetAttribute("DBType","Date");
            m_FromDatePicker.SetAttribute("DBLogic",">=");
            m_FromDatePicker.SetAttribute("Groups","Search");
            m_FromDatePicker.SetAttribute("validate", "{date:true}");

            //m_ToDatePicker.ID = "To";
            m_ToDatePicker.SetAttribute("class", "ui-widget-content date");
            m_ToDatePicker.SetAttribute("style", "width:80px;");
            m_ToDatePicker.SetAttribute("DBType", "Date");
            m_ToDatePicker.SetAttribute("DBLogic", "<=");
            m_ToDatePicker.SetAttribute("validate", "{date:true}");

        }



        protected override void CreateChildControls()
        {
            m_FromDatePicker.ID = this.ID + "_From";
            m_ToDatePicker.ID = this.ID + "_To";

            string dbField = this.GetAttribute("DBField");
            string groups = this.GetAttribute("Groups");

            m_FromDatePicker.SetAttribute("DBField", dbField);
            m_ToDatePicker.SetAttribute("DBField", dbField);

            m_FromDatePicker.SetAttribute("Groups", groups);
            m_ToDatePicker.SetAttribute("Groups", groups);


            this.Controls.Add(m_FromDatePicker);
            this.Controls.Add(m_ToDatePicker);
        }

        public override Control FindControl(string id)
        {
            return base.FindControl(id);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            EnsureChildControls();

            writer.Write("<span>");

            m_FromDatePicker.RenderControl(writer);
            
            writer.Write("&nbsp; 至&nbsp;");

            m_ToDatePicker.RenderControl(writer);

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
                m_FromDatePicker.SetAttribute("DBField", value);
                m_ToDatePicker.SetAttribute("DBField", value);
            }

            if ("Groups".Equals(key, StringComparison.CurrentCultureIgnoreCase))
            {
                m_FromDatePicker.SetAttribute("Groups", value);
                m_ToDatePicker.SetAttribute("Groups", value);
            }
        }

        #endregion

        
        public string FromDateValue
        {
            get { return m_FromDatePicker.Value; }
            set { m_FromDatePicker.Value = value; }
        }


        public string ToDateValue
        {
            get { return m_ToDatePicker.Value; }
            set { m_ToDatePicker.Value = value; }
        }


        /// <summary>
        /// 开始日期数据字段
        /// </summary>
        [DefaultValue("")]
        [Description("开始日期数据字段")]
        [Category("DBField 数据绑定")]
        public string FormDBField
        {
            get { return m_FromDatePicker.GetAttribute("DBField"); }
            set { m_FromDatePicker.SetAttribute("DBField",value); }
        }

        /// <summary>
        /// 结束日期数据字段
        /// </summary>
        [DefaultValue("")]
        [Description("结束日期数据字段")]
        [Category("DBField 数据绑定")]
        public string ToDBField
        {
            get { return m_ToDatePicker.GetAttribute("DBField"); }
            set { m_ToDatePicker.SetAttribute("DBField", value); }
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

            this.m_FromDatePicker.Value = formValue;
            this.m_ToDatePicker.Value = toValue;
        }

        #endregion
    }
}
