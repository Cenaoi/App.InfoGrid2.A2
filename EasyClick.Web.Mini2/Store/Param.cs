using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 数据参数
    /// </summary>
    [DefaultProperty("DefaultValue")]
    [Description("数据参数")]
    public class Param : ICloneable
    {
        bool m_ConvertEmptyStringToNull = true;

        DbType m_DbType = DbType.String;

        string m_DefaultValue;

        /// <summary>
        /// 直接赋值的内部字段
        /// </summary>
        object m_InnerValue;
        /// <summary>
        /// 是否采用内部字段值
        /// </summary>
        bool m_IsInner = false;

        /// <summary>
        /// 忽略空值
        /// </summary>
        bool m_IgnoreEmpty = false;

        ParameterDirection m_Direction;
        string m_Name;

        int m_Size = 0;

        TypeCode m_Type = TypeCode.Empty;

        StoreParamMode m_ParamMode = StoreParamMode.Default;

        /// <summary>
        /// 逻辑运算符.默认 =
        /// </summary>
        string m_Logic;

        string m_Remark;

        /// <summary>
        /// 忽略空值或空字符串
        /// </summary>
        [Description("忽略空值或空字符串")]
        [DefaultValue(false)]
        public bool IgnoreEmpty
        {
            get { return m_IgnoreEmpty; }
            set { m_IgnoreEmpty = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [DefaultValue("")]
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }

        /// <summary>
        /// 是否为内部值
        /// </summary>
        [Browsable(false)]
        public bool IsInnerValue
        {
            get { return m_IsInner; }
        }

        /// <summary>
        /// 设置内部值
        /// </summary>
        /// <param name="value"></param>
        public void SetInnerValue(object value)
        {
            m_IsInner = true;
            m_InnerValue = value;
        }

        /// <summary>
        /// 直接赋值的内部字段
        /// </summary>
        [Browsable(false)]
        public object InnerValue
        {
            get { return m_InnerValue; }
        }

        /// <summary>
        /// 参数模式
        /// </summary>
        [Browsable(false)]
        public StoreParamMode ParamMode
        {
            get { return m_ParamMode; }
            protected set { m_ParamMode = value; }
        }


        /// <summary>
        /// 逻辑运算符.默认 =
        /// </summary>
        [Description("逻辑运算符.默认 =")]
        [DefaultValue("")]
        public string Logic
        {
            get { return m_Logic; }
            set { m_Logic = value; }
        }

        /// <summary>
        /// (构造函数)数据参数
        /// </summary>
        public Param()
        {
        }

        /// <summary>
        /// (构造函数)数据参数
        /// </summary>
        /// <param name="name">参数名称</param>
        public Param(string name)
        {
            m_Name = name;
        }

        /// <summary>
        /// (构造函数)数据参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="defaultValue">默认值</param>
        public Param(string name, string defaultValue)
        {
            m_Name = name;
            m_DefaultValue = defaultValue;
        }

        public Param(string name, object defaultValue)
        {
            m_Name = name;

            if (defaultValue != null)
            {
                m_DefaultValue = defaultValue.ToString();
            }
        }


        /// <summary>
        /// (构造函数)数据参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="dbType">数据类型</param>
        public Param(string name, string defaultValue, DbType dbType)
        {
            m_Name = name;
            m_DefaultValue = defaultValue;
            m_DbType = dbType;
        }

        /// <summary>
        /// (构造函数)数据参数
        /// </summary>
        /// <param name="name">字段名</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="innerValue">内部值</param>
        public Param(string name , DbType dbType, object innerValue)
        {
            m_Name = name;
            m_DbType = dbType;
            SetInnerValue(innerValue);
        }


        /// <summary>
        /// 数据类型
        /// </summary>
        [Description("数据类型")]
        [DefaultValue(DbType.String)]
        public DbType DbType
        {
            get { return m_DbType; }
            set { m_DbType = value; }
        }

        /// <summary>
        /// 空字符串转化为 null
        /// </summary>
        [DefaultValue(true)]
        [Description("空字符串转化为 null")]
        public bool ConvertEmptyStringToNull
        {
            get { return m_ConvertEmptyStringToNull; }
            set { m_ConvertEmptyStringToNull = value; }
        }

        /// <summary>
        /// 默认值
        /// </summary>
        [Description("默认值")]
        [DefaultValue("")]
        public string DefaultValue
        {
            get { return m_DefaultValue; }
            set { m_DefaultValue = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("")]
        public ParameterDirection Direction
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }

        /// <summary>
        /// 参数名称,或字段名称
        /// </summary>
        [DefaultValue("")]
        [Description("参数名称,或字段名称")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// 数据长度
        /// </summary>
        [DefaultValue(0)]
        [Description("数据长度")]
        public int Size
        {
            get { return m_Size; }
            set { m_Size = value; }
        }

        /// <summary>
        /// 数据类型的代码
        /// </summary>
        [Description("数据类型的代码")]
        [DefaultValue(TypeCode.Empty)]
        public TypeCode Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }


        /// <summary>
        /// 更新并返回 System.Web.UI.WebControls.Parameter 对象的值。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public virtual object Evaluate(System.Web.HttpContext context, System.Web.UI.Control control)
        {
            if (this.m_IsInner)
            {
                return m_InnerValue;
            }


            if (m_DefaultValue == null)
            {
                return null;
            }

            object value = null;

            if (string.IsNullOrEmpty(m_DefaultValue))
            {

            }
            else
            {
                string logicUpper = StringUtil.NoBlank(m_Logic, "=").ToUpper();

                string tmpDV = m_DefaultValue.Trim();

                //如果左右两边是括号，那么就是特殊函数了
                if (logicUpper == "IN" || logicUpper == "NOIN")
                {
                    string[] valueStrList = StringUtil.Split(m_DefaultValue, ",");

                    object[] values = new object[valueStrList.Length];

                    for (int i = 0; i < valueStrList.Length; i++)
                    {
                        if(m_DbType == DbType.String)
                        {
                            string vStr = valueStrList[i];

                            if(StringUtil.StartsWith(vStr,"'") && StringUtil.EndsWith(vStr, "'"))
                            {
                                valueStrList[i] = vStr.Substring(1, vStr.Length - 2);
                            }
                        }

                        values[i] = StringUtil.ChangeType(valueStrList[i], m_DbType);
                    }

                    return values;
                }
                else if (logicUpper == "IS" || logicUpper == "ISNOT")
                {
                    return value;
                }
                else if (StringUtil.StartsWith(tmpDV, "(") && StringUtil.EndsWith(tmpDV, ")"))
                {
                    string code = tmpDV.Substring(1, tmpDV.Length - 2);

                    value = ProFunCode(code);
                }
                else
                {
                    if (m_DbType != System.Data.DbType.String)
                    {
                        value = StringUtil.ChangeType(m_DefaultValue, m_DbType);
                    }
                    else if (this.Type != TypeCode.Empty)
                    {
                        value = Convert.ChangeType(m_DefaultValue, this.Type);
                    }
                    else
                    {
                        value = m_DefaultValue;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 处理特殊函数参数
        /// </summary>
        /// <returns></returns>
        protected object ProFunCode(string funCode)
        {
            object result = null;

            if (StringUtil.EndsWith(funCode, "()"))
            {
                string funName = funCode.Substring(0, funCode.Length - 2);

                string funUpperName = funName.ToUpper();

                switch (funUpperName)
                {
                    case "GETDATE": result = DateTime.Now; break;
                    case "NEWID": result = Guid.NewGuid(); break;
                    default:

                        result = StoreParamValueManager.Exec(funUpperName,null);

                        break;
                }

            }
            else
            {
                int n1 = funCode.LastIndexOf(')');

                int n0 = funCode.LastIndexOf('(');

                string psStr = funCode.Substring(n0 + 1, n1 - n0 - 1);

                string[] ps = StringUtil.Split(psStr, ",");

                string funUpperName = funCode.Substring(0, n0);

                result = StoreParamValueManager.Exec(funUpperName,ps);
            }

            return result;
        }





        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            throw new NotImplementedException("未实现");
        }


        /// <summary>
        /// 输出 Json 格式
        /// </summary>
        /// <returns></returns>
        public virtual string ToJson()
        {
            ScriptTextWriter st = new ScriptTextWriter(QuotationMarkConvertor.SingleQuotes);

            st.RetractBengin("{");

            st.WriteParam("name", this.Name);
            st.WriteParam("default", this.DefaultValue);

            st.RetractEnd("}");

            string json = st.ToString();

            return json;
        }

        /// <summary>
        /// 输出当前对象的 字符串格式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }

}
