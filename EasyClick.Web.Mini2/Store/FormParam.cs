using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 提交的 Form 参数
    /// </summary>
    [Description("提交的 Form 参数")]
    public class FormParam : Param
    {

        string m_FormField;

        /// <summary>
        /// Request.Form(...) 参数名称
        /// </summary>
        [DefaultValue("")]
        [Description("Request.Form(...) 参数名称")]
        public string FormField
        {
            get { return m_FormField; }
            set { m_FormField = value; }
        }


        /// <summary>
        /// 必填项目
        /// </summary>
        [Description("必填项")]
        public bool ValidRequired { get; set; }

        /// <summary>
        /// 值枚举的字符串
        /// </summary>
        [Description("值枚举的字符串")]
        public string ValidValueEnum { get; set; }


        public override object Evaluate(System.Web.HttpContext context, System.Web.UI.Control control)
        {
            HttpRequest request = context.Request;

            string valueStr = request.Form[m_FormField];

            object value;

            if (string.IsNullOrEmpty(valueStr) && !string.IsNullOrEmpty(this.DefaultValue))
            {
                value = StringUtil.ChangeType(this.DefaultValue, this.DbType);
            }
            else
            {
                value = StringUtil.ChangeType(valueStr, this.DbType);
            }

            ProValidate(value);

            return value;
        }


        private void ProValidate(object value)
        {
            try
            {
                OnValidate(value);
            }
            catch (Exception ex)
            {
                throw new Exception("验证失败", ex);
            }
        }

        private void OnValidate(object value)
        {
            if (this.ValidRequired)
            {
                if (value == null)
                {
                    throw new Exception($"Post 提交参数 '{this.FormField}' 不能为空.");
                }
            }

            if (!StringUtil.IsBlank(value) && !StringUtil.IsBlank(ValidValueEnum))
            {
                string[] vEnum = StringUtil.Split(this.ValidValueEnum, ";");

                bool exist = ArrayUtil.Exist(vEnum, value.ToString());

                if (!exist)
                {
                    throw new Exception($"Post 提交参数 '{this.FormField}'={value} 非法,不是预期值.");
                }
            }
        }

    }
}
