using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web;
using EC5.Utility;
using EasyClick.Web.Mini2.Data;

namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// 数据仓库
    /// </summary>
    [Description("数据仓库")]
    public class StoreCurrentParam : Param
    {
        string m_ControlID;
        string m_PropertyName;

        /// <summary>
        /// (构造函数)控件参数
        /// </summary>
        public StoreCurrentParam()
        {
        }

        /// <summary>
        /// (构造函数)控件参数
        /// </summary>
        /// <param name="name">参数名称</param>
        public StoreCurrentParam(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// (构造函数)控件参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="controlId">控件ID</param>
        /// <param name="propertyName">控件属性</param>
        public StoreCurrentParam(string name, string controlId, string propertyName)
        {
            this.Name = name;
            this.ControlID = controlId;
            this.PropertyName = propertyName;
        }


        /// <summary>
        /// 控件ID
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [TypeConverter(typeof(System.Web.UI.WebControls.ControlIDConverter))]
        [DefaultValue("")]
        [System.Web.UI.IDReferenceProperty]
        [Description("控件ID")]
        public string ControlID
        {
            get { return m_ControlID; }
            set { m_ControlID = value; }
        }

        /// <summary>
        /// 控件属性名称
        /// </summary>
        [DefaultValue("")]
        [TypeConverter(typeof(System.Web.UI.WebControls.ControlPropertyNameConverter))]
        [Description("控件属性名称")]
        public string PropertyName
        {
            get { return m_PropertyName; }
            set { m_PropertyName = value; }
        }

        private UserControl GetParentBox(System.Web.UI.Control control)
        {

            UserControl uCon = null;

            Control con = control;

            for (int i = 0; i < 99; i++)
            {
                if (con is UserControl)
                {
                    uCon = (UserControl)con;
                    break;
                }

                con = con.Parent;
            }

            return uCon;
        }

        public override object Evaluate(HttpContext context, System.Web.UI.Control control)
        {
            if (StringUtil.IsBlank(this.ControlID))
            {
                throw new Exception("筛选条件的“控件ID (ControlID)”不能为空");
            }

            if (StringUtil.IsBlank(this.PropertyName))
            {
                throw new Exception("筛选条件的控件属性 (PropertyName) 不能为空");
            }

            UserControl uCon = GetParentBox(control);

            Store targetCon = uCon.FindControl(this.ControlID) as Store;

            if (targetCon == null)
            {
                throw new Exception(string.Format("控件“{0}”不存在", this.ControlID));
            }

            DataRecord current = targetCon.GetDataCurrent();

            if (current == null)
            {
                return null;
            }
            
            DataField field = current.Fields[m_PropertyName];

            if (field == null)
            {
                throw new Exception($"数据仓库 \"{targetCon.ID}\" ,焦点行没 \"{m_PropertyName}\" 字段名.");
            }

            object value = field.Value;

            //PropertyInfo pi = ObjectUtil.GetProperty(targetCon, this.PropertyName);

            //if (pi == null)
            //{
            //    throw new Exception(string.Format("控件'{0}'的属性 '{1}' 不存在", this.ControlID, this.PropertyName));
            //}

            //object value = pi.GetValue(targetCon, null);

            return value;
        }

    }
}
