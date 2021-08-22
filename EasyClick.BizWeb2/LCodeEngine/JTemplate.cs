using System;
using System.Collections.Generic;
using System.Web;
using HWQ.Entity.LightModels;
using System.Text;

namespace EasyClick.BizWeb2.LCodeEngine
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
                throw new Exception("没有给数据实体,无法解析执行.");
            }

            if (m_Items == null)
            {
                m_Items = CodeAnalysis();
            }

            StringBuilder sb = new StringBuilder();

            LModelElement modelElem = m_Model.GetModelElement();

            foreach (JItem item in m_Items)
            {
                switch (item.ItemType)
                {
                    case JItemType.String: sb.Append(item.Text);
                        break;
                    case JItemType.Code:

                        if (modelElem.Fields.ContainsField(item.Text))
                        {
                            object mValue = m_Model[item.Text];

                            if (mValue != null)
                            {
                                string fieldValue = mValue.ToString();
                                sb.Append(fieldValue);
                            }
                            else
                            {
                                sb.Append(0);
                            }
                        }
                        else
                        {
                            sb.Append(item.Text);
                        }

                        break;
                }
            }

            m_ResultText = sb.ToString();

            return m_ResultText;
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
        }
    }
}