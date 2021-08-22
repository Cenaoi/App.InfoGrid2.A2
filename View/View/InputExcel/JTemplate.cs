using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using HWQ.Entity;

namespace App.InfoGrid2.View.InputExcel
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
    }

    /// <summary>
    /// 模板项目集合
    /// </summary>
    public class JItemCollection : List<JItem>
    {

    }

    /// <summary>
    /// 模板引擎
    /// </summary>
    public  class JTemplate
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
        JItemCollection m_Items;


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


        LightModel m_Model;

        public LightModel Model
        {
            get { return m_Model; }
            set { m_Model = value; }
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

            m_Items = CodeAnalysis();

            StringBuilder sb = new StringBuilder();

            LModelElement modelElem ;

            if (m_Model is LModel)
            {
                modelElem = ((LModel)m_Model).GetModelElement();
            }
            else
            {
                modelElem = LightModel.GetLModelElement(m_Model.GetType());
            }


            foreach (JItem item in m_Items)
            {
                switch (item.ItemType)
                {
                    case JItemType.String: sb.Append(item.Text);
                        break;
                    case JItemType.Code:

                        if (item.Prefix == "T")
                        {
                            Exec_ForT(modelElem, item, sb);
                        }
                        else if(item.Prefix == "P")
                        {

                        }

                        break;
                }
            }

            m_ResultText = sb.ToString();

            return m_ResultText;
        }

        private void Exec_ForT(LModelElement modelElem, JItem item, StringBuilder sb)
        {
            if (!modelElem.Fields.ContainsField(item.Text))
            {
                throw new Exception(string.Format("字段不存在 “{0}”", item.Text));
            }


            LModelFieldElement fieldElem = modelElem.Fields[item.Text];

            if (m_Model.IsNull(item.Text))
            {
                sb.Append("null"); 
                return;
            }
            

            object value = m_Model[item.Text];

            if (value == null)
            {
                sb.Append("null");
                return;
            }


            string fieldValue = value.ToString();

            if (fieldElem.IsNumber)
            {
                sb.Append(fieldValue);
            }
            else if (fieldElem.DBType == LMFieldDBTypes.Boolean)
            {
                string fieldValueU = fieldValue.ToUpper();

                sb.Append(fieldValueU == "TRUE" ? 1 : 0);
            }
            else
            {
                sb.Append("'").Append(fieldValue.Replace("'", "''")).Append("'");
            }
            

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

            JItemCollection items = new JItemCollection();

            while (n1 >= 0)
            {
                lastN = srcText.IndexOf('}', n1 + 1);

                if (lastN == -1)
                {
                    throw new Exception("模板错误,没有结束符号。" + m_SrcText);
                }

                if (n1 > 0)
                {
                    itemFirstStr = srcText.Substring(n2, n1 - n2);

                    JItem jItem = new JItem(JItemType.String, itemFirstStr);
                    items.Add(jItem);
                }

                itemStr = srcText.Substring(n1 + 1, lastN - n1 - 1);

                int dotN = itemStr.IndexOf('.');
                string prefix = string.Empty;
                string field;

                if (dotN > 0)
                {
                    prefix = itemStr.Substring(1, dotN - 1);
                    field = itemStr.Substring(dotN + 1);
                }
                else
                {
                    field = itemStr;
                }

                JItem jItem2 = new JItem(JItemType.Code, field);
                jItem2.Prefix = prefix;
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


        /// <summary>
        /// 数字转字符串,并把数字后面没用的0去掉
        /// </summary>
        /// <param name="num">值</param>
        /// <returns></returns>
        public static string ConvertDecimal(decimal num)
        {
            string numStr = num.ToString();
            //从小数点中切割
            string[] numSp = numStr.Split('.');
            //如果数组只有一位，就证明是整数，不用去小数点
            if (numSp.Length == 1)
            {
                return numStr;
            }

            string xs = numSp[1];
            //去掉后面的所有0
            xs = xs.TrimEnd('0');
            //如果小数点后面都是0就只返回整数就行了
            if (xs.Length > 0)
            {
                return numSp[0] + "." + xs;
            }
            else
            {
                return numSp[0];
            }


        }


    }
}
