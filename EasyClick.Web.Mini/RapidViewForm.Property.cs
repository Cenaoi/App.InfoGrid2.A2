using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace EasyClick.Web.Mini
{
    partial class RapidViewForm
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

        string m_HeaderTemplate;
        string m_HeaderRequiredTemplate;

        string m_DefaultTableHeaderTemplate = "<th align=\"left\" style=\"font-weight:normal;\"><nobr><label>{0}:</label></nobr></th>";
        string m_DefaultTableHeaderRequiredTemplate = "<th align=\"left\" style=\"font-weight:normal;\"><nobr><label>{0}:</label><em>*</em></nobr></th>";

        string m_DefaultFlowHeaderTemplate = "<label>{0}:</label>";
        /// <summary>
        /// 必填字段
        /// </summary>
        string m_DefaultFlowHeaderRequiredTemplate = "<label>{0}:</label><em>*</em>";

        int m_ColSpaceWidth = 40;

        RepeatLayout m_RepeatLayout = RepeatLayout.Table;


        [DefaultValue(RepeatLayout.Table)]
        public RepeatLayout RepeatLayout
        {
            get { return m_RepeatLayout; }
            set { m_RepeatLayout = value; }
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
