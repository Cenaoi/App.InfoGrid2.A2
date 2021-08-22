using System;
using System.Collections.Generic;
using System.Web;
using HWQ.Entity.LightModels;
using System.Text;
using HWQ.Entity;
using EC5.Utility;
using System.Reflection;

namespace EC5.LCodeEngine
{
    /// <summary>
    /// 模板项目类型
    /// </summary>
    public enum JItemType
    {
        /// <summary>
        /// 普通字符串
        /// </summary>
        String,
        /// <summary>
        /// 代码
        /// </summary>
        Code
    }
    

    /// <summary>
    /// 模板项目
    /// </summary>
    public class JItem
    {
        public JItem()
        {
        }

        public JItem(JItemType itemType, string itemText)
        {
            this.ItemType = itemType;
            this.Text = itemText;
        }

        /// <summary>
        /// 条目类型
        /// </summary>
        public JItemType ItemType { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 字段名描述
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 模板项目集合
    /// </summary>
    public class JItemCollection:List<JItem>
    {

    }

    /// <summary>
    /// 模板引擎
    /// </summary>
    public class JTemplate:IDisposable
    {
        /// <summary>
        /// 原字符串
        /// </summary>
        string m_SrcText;

        /// <summary>
        /// 返回的字符串
        /// </summary>
        string m_ResultText;

        /// <summary>
        /// 执行
        /// </summary>
        JItemCollection m_Items ;


        public JTemplate()
        {

        }

        /// <summary>
        /// (构造函数)模板引擎
        /// </summary>
        /// <param name="srcText"></param>
        public JTemplate(string srcText)
        {
            m_SrcText = srcText;
        }

        /// <summary>
        /// 处理后返回的字符串
        /// </summary>
        public string ResultText
        {
            get { return m_ResultText; }
            set { m_ResultText = value; }
        }

        /// <summary>
        /// 原字符串
        /// </summary>
        public string SrcText
        {
            get { return m_SrcText; }
            set { m_SrcText = value; }
        }


        LModel m_Model;

        public LModel Model
        {
            get { return m_Model; }
            set { m_Model = value; }
        }


        /// <summary>
        /// 获取需要处理的变量名称
        /// </summary>
        /// <returns></returns>
        public JItem[] GetCodeItems()
        {
            List<JItem> items = new List<JItem>();

            if (m_Items == null)
            {
                m_Items = CodeAnalysis();
            }

            foreach (JItem item in m_Items)
            {
                if (item.ItemType == JItemType.Code)
                {
                    items.Add(item);
                }
            }

            return items.ToArray();

        }


        /// <summary>
        /// 处理执行
        /// </summary>
        /// <returns></returns>
        public string Exec()
        {
            if (m_Model == null)
            {
                throw new Exception("实体 = null,无法解析执行.");
            }

            if (m_Items == null)
            {
                m_Items = CodeAnalysis();
            }

            StringBuilder sb = new StringBuilder();

            LModelElement modelElem = m_Model.GetModelElement();

            foreach (JItem item in m_Items)
            {
                Append(sb, item, modelElem);
            }

            m_ResultText = sb.ToString();

            return m_ResultText;
        }

        private void Append(StringBuilder sb, JItem item, LModelElement modelElem)
        {
            switch (item.ItemType)
            {
                case JItemType.String: sb.Append(item.Text); break;
                case JItemType.Code: sb.Append(ParseItem_ForCode(item,modelElem)); break;
            }
        }


        /// <summary>
        /// 函数参数
        /// </summary>
        /// <param name="item"></param>
        /// <param name="modelElem"></param>
        /// <returns></returns>
        private string ParseItem_ForCode_Fun(JItem item, LModelElement modelElem)
        {
            string result = null;
            JTemplateFunc func = null;

            string[] sp = StringUtil.Split(item.Text, ",");

            string funName = sp[0];

            if (!JTemplateFuncManager.Commons.TryGetItem(funName, out func))
            {
                throw new Exception(string.Format("函数名“{0}”不存在。", funName));
            }

            MethodInfo method = func.Method;

            ParameterInfo[] pInfos = method.GetParameters();

            object[] ps;

            int j ;

            if (pInfos.Length > 0 && pInfos[0].ParameterType.IsArray)
            {

                object[] ps2 = new object[sp.Length - 1];

                for (int i = 1; i < sp.Length; i++)
                {
                    j = i - 1;
                    ps2[j] = sp[i];
                }

                ps = new object[] { ps2 };
            }
            else
            {
                ps = new object[sp.Length - 1];

                for (int i = 1; i < sp.Length; i++)
                {
                    j = i - 1;

                    ParameterInfo pi = pInfos[j];

                    Type paramT = pi.ParameterType;

                    try
                    {
                        if (paramT.IsValueType)
                        {
                            ps[j] = Convert.ChangeType(sp[i], paramT);
                        }
                        else
                        {
                            ps[j] = sp[i];
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("“{0}”第 {1} 个参数“{2}”转换为“{3}”错误。",
                            item.Text, i, sp[i], pInfos[i].ParameterType.FullName), ex);
                    }
                }
            }

            result = method.Invoke(func.Owner, ps).ToString();


            return result;
        }

        /// <summary>
        /// 当前对象
        /// </summary>
        /// <param name="item"></param>
        /// <param name="modelElem"></param>
        /// <returns></returns>
        private string ParseItem_ForCode_Table(JItem item, LModelElement modelElem)
        {
            string result = null;

            LModelFieldElement fieldElem = null;

            if (modelElem.TryGetField(item.Text, out fieldElem))
            {
                object mValue = m_Model[fieldElem];

                if (mValue != null)
                {
                    if (fieldElem.IsNumber)
                    {
                        result = Convert.ToDecimal(mValue).ToString("0.######");
                    }
                    else
                    {
                        string fieldValue = mValue.ToString();
                        result = fieldValue;
                    }
                }
                else if (fieldElem.IsNumber)
                {
                    result = "0";
                }
                else
                {

                }
            }
            else
            {
                result = item.Text;
            }

            return result;
        }

        /// <summary>
        /// 参数对象
        /// </summary>
        /// <param name="item"></param>
        /// <param name="modelElem"></param>
        /// <returns></returns>
        private string ParseItem_ForCode_Param(JItem item, LModelElement modelElem)
        {
            string result = null;

            string[] sp = StringUtil.Split(item.Text, ".");

            object cur = null;

            

            if(!m_Params.TryGetValue(sp[0],out cur))
            {
                throw new Exception(string.Format("参数不存在“$P.{0}”。",sp[0]));
            }

            for (int i = 1; i < sp.Length ; i++)
            {
                string propName= sp[i];

                if (cur is LightModel)
                {
                    LightModel model = (LightModel)cur;
                    cur = model[propName];

                    result = cur.ToString();

                    break;
                }
                else
                {

                }

            }

            return result;
        }

        SortedList<string, object> m_Params;

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        public void SetParam(string paramName, object value)
        {
            if (m_Params == null)
            {
                m_Params = new SortedList<string, object>();
            }

            m_Params[paramName] = value;
        }

        public void ClearParamAll()
        {
            if (m_Params != null)
            {
                m_Params.Clear();
            }
        }

        private string ParseItem_ForCode(JItem item, LModelElement modelElem)
        {
            string result = null, prefix = item.Prefix;


            if (prefix == "F")
            {
                result = ParseItem_ForCode_Fun(item, modelElem);
            }
            else if (prefix == "T")
            {
                result = ParseItem_ForCode_Table(item, modelElem);
            }
            else if(prefix == "P") 
            {
                result = ParseItem_ForCode_Param(item, modelElem);
            }


            return result;
        }


        /// <summary>
        /// 代码解析
        /// </summary>
        private JItemCollection CodeAnalysis()
        {
            string srcText = m_SrcText;

            int n1 = srcText.IndexOf("{$");
            int lastN = 0;
            int n2 = 0;

            string itemFirstStr;
            string itemStr;
            string desc;    //描述,字段中文

            JItemCollection items = new JItemCollection();

            while (n1 >= 0)
            {
                lastN = srcText.IndexOf('}', n1 + 1);

                if (lastN == -1)
                {
                    throw new Exception("模板错误,没有结束符号。" + m_SrcText);
                }
                
                if(n1 > 0)
                {
                    itemFirstStr = srcText.Substring(n2 , n1 - n2);

                    JItem jItem = new JItem(JItemType.String, itemFirstStr);
                    items.Add(jItem);
                }

                itemStr = srcText.Substring(n1+ 1, lastN - n1 -1);

                int dotN = itemStr.IndexOf('.');
                string prefix = string.Empty;
                string field;

                if (dotN > 0)
                {
                    prefix = itemStr.Substring(1, dotN-1);
                    field = itemStr.Substring(dotN + 1);
                }
                else
                {
                    field = itemStr;
                }

                //查找冒号,冒号后面的是描述
                int mCharN = field.IndexOf(':');

                if (mCharN > -1)
                {
                    desc = field.Substring(mCharN + 1);
                    field = field.Substring(0, mCharN);
                }
                else
                {
                    desc = string.Empty;
                }


                JItem jItem2 = new JItem(JItemType.Code, field);
                jItem2.Prefix = prefix;
                jItem2.Description = desc;
                items.Add(jItem2);

                n2 = lastN + 1;

                n1 = srcText.IndexOf("{$", n2);
            }


            if (n2 < srcText.Length)
            {
                itemStr = srcText.Substring(n2);
                JItem jItem = new JItem(JItemType.String, itemStr);
                items.Add(jItem);
            }


            return items;

        }


        public void Dispose()
        {
            m_Model = null;

            if (m_Items != null)
            {
                m_Items.Clear();
            }

            m_Items = null;

            GC.SuppressFinalize(this);
        }
    }
}