using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template
{
    public class JSqlTemplate
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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




        public JSqlTemplate()
        {

        }

        /// <summary>
        /// (构造函数)模板引擎
        /// </summary>
        /// <param name="srcText"></param>
        public JSqlTemplate(string srcText)
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

        List<LModel> m_ModelList;


        public List<LModel> ModelList
        {
            get { return m_ModelList; }
            set { m_ModelList = value; }
        }

        public LModel Model
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

            LModelElement modelElem = m_Model.GetModelElement();

            foreach (JItem item in m_Items)
            {
                switch (item.ItemType)
                {
                    case JItemType.String:
                        sb.Append(item.Text);
                        break;
                    case JItemType.Code:

                        string result = ParseItem_ForCode_Table(item, modelElem);

                        sb.Append(result);

                        break;
                }
            }

            m_ResultText = sb.ToString();

            return m_ResultText;
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

            string text = item.Text;

            int remarkIndex = text.LastIndexOf(':');

            if (remarkIndex > 0)
            {
                text = text.Substring(0, remarkIndex);
            }

            string[] paramList = EC5.Utility.StringUtil.Split(text, ",");

            string field = paramList[0];

            if (modelElem.TryGetField(field, out fieldElem))
            {
                object mValue = m_Model[fieldElem];

                if (mValue == null)
                {
                    return string.Empty;
                }

                if (mValue != null)
                {
                    string fieldValue = mValue.ToString();
                    result = fieldValue;
                }
                else if (fieldElem.IsNumber)
                {
                    result = "0";
                }
                else
                {

                }

                if (paramList.Length > 1)
                {
                    //暂时只取一个参数

                    string ps = paramList[1];



                }
            }
            else
            {


                //这是字段找不到的话，直接返回空字符了，不用返回原生的数据了
                //result = "{$" + item.Prefix + "." + item.Text + "}";
            }

            return result;
        }









        /// <summary>
        /// 代码解析
        /// </summary>
        private JItemCollection CodeAnalysis()
        {
            string srcText = m_SrcText;

            int n1 = srcText.IndexOf("<$");
            int lastN = 0;
            int n2 = 0;

            string itemFirstStr;
            string itemStr;

            JItemCollection items = new JItemCollection();

            while (n1 >= 0)
            {
                lastN = srcText.IndexOf("/>", n1 + 1);

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
                    field = itemStr.Substring(dotN + 1).Trim();
                }
                else
                {
                    field = itemStr;
                }

                JItem jItem2 = new JItem(JItemType.Code, field);
                jItem2.Prefix = prefix;
                items.Add(jItem2);

                n2 = lastN + 1;

                n1 = srcText.IndexOf("<$", n2);
            }

            n2++;

            if (n2 < srcText.Length)
            {
                itemStr = srcText.Substring(n2);
                JItem jItem = new JItem(JItemType.String, itemStr);
                items.Add(jItem);
            }


            return items;

        }

    }
}
