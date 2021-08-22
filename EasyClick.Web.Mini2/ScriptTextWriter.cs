using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 脚本引号转换器
    /// </summary>
    public enum QuotationMarkConvertor
    {
        /// <summary>
        /// 不自动转换
        /// </summary>
        None,

        /// <summary>
        /// 单引号
        /// </summary>
        SingleQuotes ,
        /// <summary>
        /// 双引号
        /// </summary>
        DoubleQuote 
    }


    /// <summary>
    /// 脚本文本
    /// </summary>
    public class ScriptTextWriter : StringWriter
    {
        class ScriptTextItem
        {
            public bool IsRoot = false;

            public int Index;

            public string ParamName;
        }

        /// <summary>
        /// 脚本文本
        /// </summary>
        /// <param name="sb"></param>
        public ScriptTextWriter(StringBuilder sb) : base(sb)
        {
            OnInit();
        }

        /// <summary>
        /// 脚本文本
        /// </summary>
        /// <param name="qmConvertor"></param>
        public ScriptTextWriter(QuotationMarkConvertor qmConvertor) : base(new StringBuilder())
        {
            m_QuotationMarkConvertor = qmConvertor;
            OnInit();
        }

        /// <summary>
        /// 脚本文本
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="qmConvertor">单引号转换器</param>
        public ScriptTextWriter(StringBuilder sb, QuotationMarkConvertor qmConvertor) : base(sb)
        {
            m_QuotationMarkConvertor = qmConvertor;
            OnInit();
        }



        /// <summary>
        /// 字符串格式
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        QuotationMarkConvertor m_QuotationMarkConvertor = QuotationMarkConvertor.DoubleQuote;

        /// <summary>
        /// 脚本引号,默认单引号
        /// </summary>
        [DefaultValue(QuotationMarkConvertor.DoubleQuote)]
        public QuotationMarkConvertor QuotationMarkConvertor
        {
            get { return m_QuotationMarkConvertor; }
            set { m_QuotationMarkConvertor = value; }
        }


        /// <summary>
        /// 索引值
        /// </summary>
        Stack<ScriptTextItem> m_IndexStack = new Stack<ScriptTextItem>();

        /// <summary>
        /// 当前索引
        /// </summary>
        ScriptTextItem m_Cur;

        /// <summary>
        /// 空白数量.默认 4 个
        /// </summary>
        [Description("空白数量")]
        [DefaultValue(4)]
        public int SpaceCount { get; set; } = 4;

        /// <summary>
        /// 缩进宽度
        /// </summary>
        [Description("缩进宽度")]
        public int RetractWidth { get; set; } = 0;


        bool m_FormatCode = true;
        bool m_NoFormatCode = false;

        /// <summary>
        /// 格式化代码
        /// </summary>
        [Description("格式化代码")]
        [DefaultValue(true)]
        public bool FormatCode
        {
            get { return m_FormatCode; }
            set
            {
                m_FormatCode = value;
                m_NoFormatCode = !value;
            }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        private void OnInit()
        {

            ScriptTextItem item = new ScriptTextItem();
            item.ParamName = string.Empty;
            item.IsRoot = true;

            m_Cur = item;
        }

        /// <summary>
        /// 缩进开始
        /// </summary>
        public void RetractBengin()
        {
            this.RetractWidth++;
        }

        /// <summary>
        /// 缩进开始
        /// </summary>
        /// <param name="value"></param>
        public void RetractBengin(string value)
        {

            this.RenderFormatSpance();

            if (m_FormatCode)
            {
                this.WriteLine(value);
            }
            else
            {
                this.Write(value);
            }

            this.RetractWidth++;
        }

        /// <summary>
        /// 缩进结束
        /// </summary>
        public void RetractEnd()
        {
            this.RetractWidth--;
        }

        /// <summary>
        /// 缩进结束
        /// </summary>
        /// <param name="value"></param>
        public void RetractEnd(string value)
        {
            this.RetractWidth--;

            if(m_Cur.Index > 0) { this.WriteLine(); }

            this.RenderFormatSpance();

            if (m_FormatCode)
            {
                this.WriteLine(value);
            }
            else
            {
                this.Write(value);
            }
        }


        /// <summary>
        /// 输出
        /// </summary>
        /// <param name="paramName"></param>
        public void RenderBengin(string paramName)
        {
            if (m_Cur.Index++ > 0) { this.WriteParamNext(","); }

            this.RenderFormatSpance();
            this.Write("{0}: {{", paramName);

            m_IndexStack.Push(m_Cur);

            ScriptTextItem item = new ScriptTextItem();
            item.ParamName = paramName;

            m_Cur = item;


        }



        /// <summary>
        /// 输出结束
        /// </summary>
        public void RenderEnd()
        {
            RenderEnd("}");
        }

        /// <summary>
        /// 输出结束
        /// </summary>
        public void RenderEnd(string text)
        {
            if (m_IndexStack.Count > 0)
            {
                if (m_FormatCode && m_Cur.Index > 0)
                {
                    this.WriteLine();

                    RenderFormatSpance(-1);
                }

                this.Write(text);


                ScriptTextItem item = m_IndexStack.Pop();

                m_Cur = item;
            }
        }


        /// <summary>
        /// 输出代码行
        /// </summary>
        /// <param name="code"></param>
        public void WriteCodeLine(string code)
        {
            RenderFormatSpance();

            if (m_FormatCode)
            {
                this.WriteLine(code);
            }
            else
            {
                this.Write(code);
            }
        }



        #region RenderParam

        /// <summary>
        /// 输出参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public ScriptTextWriter WriteParam(string name, string value, string defaultValue)
        {
            return RenderParamForItem(m_Cur, name, value, defaultValue);
        }

        /// <summary>
        /// 输出参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ScriptTextWriter WriteParam(string name, string value)
        {
            if (value == null)
            {
                return this;
            }

            return RenderParamForItem(m_Cur, name, value, string.Empty);
        }

        /// <summary>
        /// 输出参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public ScriptTextWriter WriteParam(string name, int value, int defaultValue)
        {
            return RenderParamForItem(m_Cur, name, value, defaultValue);
        }

        /// <summary>
        /// 输出参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ScriptTextWriter WriteParam(string name, int value)
        {
            return RenderParamForItem(m_Cur, name, value, 0);
        }

        /// <summary>
        /// 输出参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public ScriptTextWriter WriteParam(string name, decimal value, decimal defaultValue)
        {
            return RenderParamForItem(m_Cur, name, value, defaultValue);
        }

        /// <summary>
        /// 输出参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ScriptTextWriter WriteParam(string name, decimal value)
        {
            return RenderParamForItem(m_Cur, name, value, 0);
        }

        /// <summary>
        /// 输出参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public ScriptTextWriter WriteParam(string name, bool value, bool defaultValue)
        {
            return RenderParamForItem(m_Cur, name, value, defaultValue);
        }

        /// <summary>
        /// 输出参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ScriptTextWriter WriteParam(string name, bool value)
        {
            return RenderParamForItem(m_Cur, name, value, false);
        }

        /// <summary>
        /// 输出参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ScriptTextWriter WriteParam(string name, Enum value)
        {
            return RenderParamForItem(m_Cur, name, value, TextTransform.None);
        }

        /// <summary>
        /// 输出参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="textTransform"></param>
        /// <returns></returns>
        public ScriptTextWriter WriteParam(string name, Enum value, TextTransform textTransform)
        {
            return RenderParamForItem(m_Cur, name, value, textTransform);
        }

        /// <summary>
        /// 输出参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="textTransform"></param>
        /// <returns></returns>
        public ScriptTextWriter WriteParam(string name, Enum value, Enum defaultValue, TextTransform textTransform)
        {
            return RenderParamForItem(m_Cur, name, value, defaultValue, textTransform);
        }

        /// <summary>
        /// 输出函数属性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="funStr">函数</param>
        /// <returns></returns>
        public ScriptTextWriter WriteFunction(string name , string funStr)
        {
            return RenderFunctionForItem(m_Cur, name, funStr);
        }


        /// <summary>
        /// 输出格式化代码
        /// </summary>
        private void RenderFormatSpance(int add)
        {
            if (m_NoFormatCode)
            {
                return;
            }

            int tabCount = this.RetractWidth + m_IndexStack.Count + add;

            if (tabCount > 0)
            {
                string space = string.Empty.PadRight(tabCount * 4);

                this.Write(space);
            }
        }

        /// <summary>
        /// 输出格式化代码
        /// </summary>
        private void RenderFormatSpance()
        {
            if (m_NoFormatCode)
            {
                return;
            }

            int tabCount = this.RetractWidth + m_IndexStack.Count;

            if (tabCount > 0)
            {
                string space = string.Empty.PadRight(tabCount * 4);

                this.Write(space);
            }
        }

        private void WriteParamNext(string str)
        {
            if (m_FormatCode)
            {
                this.WriteLine(str);
            }
            else
            {
                this.Write(str);
            }
        }

        private ScriptTextWriter RenderParamForItem(ScriptTextItem item, string mame, int value, int defaultValue)
        {
            if (value == defaultValue) { return this; }

            if (m_FormatCode && !item.IsRoot && item.Index == 0) { this.WriteLine(); }

            if (item.Index++ > 0) { this.WriteParamNext(","); }

            this.RenderFormatSpance();

            this.Write("{0}: {1}", mame, value);

            return this;
        }

        private ScriptTextWriter RenderParamForItem(ScriptTextItem item, string mame, decimal value, decimal defaultValue)
        {
            if (value == defaultValue) { return this; }

            if (m_FormatCode && !item.IsRoot && item.Index == 0) { this.WriteLine(); }
            if (item.Index++ > 0) { this.WriteParamNext(","); }

            this.RenderFormatSpance();

            this.Write("{0}: {1}", mame, value.ToString("g0"));

            return this;
        }

        private ScriptTextWriter RenderParamForItem(ScriptTextItem item, string name, string value, string defaultValue)
        {
            if (value == defaultValue) { return this; }

            if(value == null)
            {
                return this;
            }

            if (m_FormatCode && !item.IsRoot && item.Index == 0) { this.WriteLine(); }
            if (item.Index++ > 0) { this.WriteParamNext(","); }

            this.RenderFormatSpance();
            
            this.Write(name);
            this.Write(": ");
                        
            if (m_QuotationMarkConvertor == QuotationMarkConvertor.SingleQuotes)
            {
                value = JsonUtil.ToJson(value, JsonQuotationMark.SingleQuotes);
                this.Write("'{0}'", value);
            }
            else if (m_QuotationMarkConvertor == QuotationMarkConvertor.DoubleQuote)
            {
                value = JsonUtil.ToJson(value, JsonQuotationMark.DoubleQuote);
                this.Write("\"{0}\"",value);
            }
            else
            {
                this.Write(value);
            }

            return this;
        }

        /// <summary>
        /// 输出函数
        /// </summary>
        /// <param name="item"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private ScriptTextWriter RenderFunctionForItem(ScriptTextItem item , string name, string value)
        {
            if (m_FormatCode && !item.IsRoot && item.Index == 0) { this.WriteLine(); }

            if (item.Index++ > 0) { this.WriteParamNext(","); }

            this.RenderFormatSpance();

            this.Write("{0}: {1}", name, value);

            return this;
        }

        private ScriptTextWriter RenderParamForItem(ScriptTextItem item, string name, bool value, bool defaultValue)
        {
            if (value == defaultValue) { return this; }

            if (m_FormatCode && !item.IsRoot && item.Index == 0) { this.WriteLine(); }
            if (item.Index++ > 0) { this.WriteParamNext(","); }

            this.RenderFormatSpance();

            this.Write("{0}: {1}", name, value ? "true" : "false");

            return this;
        }

        private ScriptTextWriter RenderParamForItem(ScriptTextItem item, string name, Enum value, TextTransform textTransform)
        {
            
            string valueStr = value.ToString();

            switch (textTransform)
            {
                case TextTransform.Lower: valueStr = valueStr.ToLower(); break;
                case TextTransform.Upper: valueStr = valueStr.ToUpper(); break;
            }

            if (m_FormatCode && !item.IsRoot && item.Index == 0) { this.WriteLine(); }

            if (item.Index++ > 0) { this.WriteParamNext(","); }

            this.RenderFormatSpance();

            if (m_QuotationMarkConvertor == QuotationMarkConvertor.SingleQuotes)
            {
                this.Write("{0}: '{1}'", name, valueStr);
            }
            else if (m_QuotationMarkConvertor == QuotationMarkConvertor.DoubleQuote)
            {
                this.Write("{0}: \"{1}\"", name, valueStr);
            }
            else
            {
                this.Write(value);
            }


            return this;
        }

        private ScriptTextWriter RenderParamForItem(ScriptTextItem item, string name, Enum value, Enum defaultValue, TextTransform textTransform)
        {
            if (value.Equals(defaultValue)) { return this; }

            string valueStr = value.ToString();

            switch (textTransform)
            {
                case TextTransform.Lower: valueStr = valueStr.ToLower(); break;
                case TextTransform.Upper: valueStr = valueStr.ToUpper(); break;
            }

            if (m_FormatCode && !item.IsRoot && item.Index == 0) { this.WriteLine(); }


            if (item.Index++ > 0) { this.WriteParamNext(","); }


            this.RenderFormatSpance();

            if (m_QuotationMarkConvertor == QuotationMarkConvertor.SingleQuotes)
            {
                this.Write("{0}: '{1}'", name, valueStr);
            }
            else if (m_QuotationMarkConvertor == QuotationMarkConvertor.DoubleQuote)
            {
                this.Write("{0}: \"{1}\"", name, valueStr);
            }
            else
            {
                this.Write(value);
            }

            return this;
        }

        #endregion

    }
}
