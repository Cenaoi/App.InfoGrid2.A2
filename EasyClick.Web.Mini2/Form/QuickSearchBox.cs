using EasyClick.Web.Mini;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace EasyClick.Web.Mini2.Form
{
    /// <summary>
    /// 快速查询框
    /// </summary>
    [DefaultProperty("Value")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public class QuickSearchBox : Control, IAttributeAccessor, IDelayRender
    {



        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                return;
            }



            StringBuilder sb = new StringBuilder();
            string clientId = this.ClientID;

            sb.AppendLine("<div class=\"mi-field-quicksearch\">");
            sb.AppendLine($"<input type=\"text\" class=\"mi-field-quicksearch-input\" id=\"{clientId}\"  placeholder=\"搜索\">");
            sb.AppendLine("<span class=\"mi-field-search-btn\"><i class=\"fa fa-search\"></i></span>");
            sb.AppendLine("</div>");
            
            
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
        }

        #endregion

        #region bind data

        /// <summary>
        /// 数据类型
        /// </summary>
        [DefaultValue(DataType.Auto)]
        public DataType DataType { get; set; } = DataType.Auto;


        /// <summary>
        /// 绑定的数据表名称
        /// </summary>
        string m_DataTable;

        /// <summary>
        /// 绑定的数据字段名称
        /// </summary>
        string m_DataField;

        /// <summary>
        /// 逻辑运算符
        /// </summary>
        string m_DataLogic;

        /// <summary>
        /// 数据模糊查询的格式.默认是: %{0}%
        /// </summary>
        string m_DataLikeFormat;

        /// <summary>
        /// 数据模糊查询的格式.默认是: %{0}%
        /// </summary>
        [Description("数据模糊查询的格式.默认是: %{0}%")]
        public string DataLikeFormat
        {
            get { return m_DataLikeFormat; }
            set { m_DataLikeFormat = value; }
        }

        /// <summary>
        /// 绑定的数据表名称
        /// </summary>
        public string DataTable
        {
            get { return m_DataTable; }
            set { m_DataTable = value; }
        }

        /// <summary>
        /// 绑定的数据字段名称
        /// </summary>
        public string DataField
        {
            get { return m_DataField; }
            set { m_DataField = value; }
        }

        /// <summary>
        /// 逻辑运算符
        /// </summary>
        public string DataLogic
        {
            get { return m_DataLogic; }
            set { m_DataLogic = value; }
        }



        #endregion


        /// <summary>
        /// 延迟加载
        /// </summary>
        bool m_IsDelayRender = false;

        /// <summary>
        /// 延迟加载
        /// </summary>
        [Description("延迟加载")]
        [DefaultValue(false)]
        public bool IsDelayRender
        {
            get { return m_IsDelayRender; }
            set { m_IsDelayRender = value; }
        }
    }
}
