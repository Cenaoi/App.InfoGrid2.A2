using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using EC5.Utility;
using System.ComponentModel;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 参数验证接口
    /// </summary>
    public interface IParamValidate
    {
        ValidResult Vlidate();
    }

    public enum ValidResult
    {
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 成功
        /// </summary>
        Success
    }

    /// <summary>
    /// 超链接参数
    /// </summary>
    [Description("超链接参数")]
    public class QueryStringParam : Param
    {
        string m_QueryStringField;

        

        public QueryStringParam()
        {

        }

        public QueryStringParam(string name, string queryStringField)
        {
            this.Name = name;
            this.QueryStringField = queryStringField;
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

        /// <summary>
        /// Request.QueryString(...) 参数名称
        /// </summary>
        [DefaultValue("")]
        [Description("Request.QueryString(...) 参数名称")]
        public string QueryStringField
        {
            get { return m_QueryStringField; }
            set { m_QueryStringField = value; }
        }

        public override object Evaluate(System.Web.HttpContext context, System.Web.UI.Control control)
        {
            HttpRequest request = context.Request;

            string valueStr = request.QueryString[m_QueryStringField];

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
            catch(Exception ex)
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
                    throw new Exception($"超链接参数 '{this.QueryStringField}' 不能为空.");
                }
            }

            if (!StringUtil.IsBlank(value) && !StringUtil.IsBlank(ValidValueEnum))
            {
                string[] vEnum = StringUtil.Split(this.ValidValueEnum, ";");

                bool exist = ArrayUtil.Exist(vEnum, value.ToString());

                if (!exist)
                {
                    throw new Exception($"超链接参数 '{this.QueryStringField}'={value} 非法,不是预期值.");
                }
            }
        }

        public override string ToJson()
        {
            ScriptTextWriter st = new ScriptTextWriter(QuotationMarkConvertor.SingleQuotes);

            st.RetractBengin("{");

            st.WriteParam("role", "query_string");
            st.WriteParam("name", this.Name);
            st.WriteParam("default", this.DefaultValue);
            st.WriteParam("queryStringField", this.QueryStringField);

            st.RetractEnd("}");

            string json = st.ToString();

            return json;
        }
    }


}
